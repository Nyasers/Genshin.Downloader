using Genshin.Downloader.Helper;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text.Json.Nodes;

namespace Genshin.Downloader
{
    public partial class Form_Fixer : Form
    {
        private const string FormName = "Genshin Impact 修复器";
        public readonly string path;
        public readonly string key;
        public readonly string[] voice_packs;

        public Form_Fixer(string path, string ver, string[] voice_packs)
        {
            InitializeComponent();
            this.path = path;
            key = ver;
            List<string> list = new();
            foreach (string s in voice_packs)
            {
                list.Add(s[(s.IndexOf("]") + 1)..]);
            }
            this.voice_packs = list.ToArray();
        }

        private void Form_Fixer_Load(object sender, EventArgs e)
        {
            textBox_info.Text += $"版本：[{key}]{Game.GetVersion(path)}\r\n语音：";
            foreach (string s in voice_packs)
            {
                textBox_info.Text += s + " ";
            }
            textBox_info.Text += $"\r\n目录：{path}";
        }

        private async void Button1_Count_Click(object sender, EventArgs e)
        {
            try
            {
                ButtonLock(true);
                string temp = DirectoryH.EnsureExists(Const.TempPath).FullName;
                List<JsonNode> online_pkg_version = await GetOnlinePkgVersion(temp, voice_packs, SetProgressStyle, Logger);
                await GetLocalPkgVersion(temp, path, key, online_pkg_version, SetProgressStyle, GetProgressValue, SetProgressValue, Logger);
            }
            catch (Exception ex)
            {
                SetProgressStyle(ProgressBarStyle.Blocks);
                SetProgressValue((0, 0));
                Logger($"{ex.Message}");
            }
            finally
            {
                ButtonLock(false);
            }
        }

        private void ButtonLock(bool enable)
        {
            button1_Count.Enabled = button2_Fix.Enabled = button3_Launch.Enabled = !enable;
        }

        private static async Task<List<JsonNode>> GetOnlinePkgVersion(string path_temp, System.Collections.IEnumerable voice_packs, Action<ProgressBarStyle> SetProgressStyle, Action<string>? logger = null)
        {
            SetProgressStyle(ProgressBarStyle.Marquee);
            File.Delete($"{path_temp}\\online_pkg_version");
            List<JsonNode> online_pkg_version = new();
            HttpResponseMessage response = await Const.Client.GetAsync("");
            if (response.IsSuccessStatusCode)
            {
                string content_raw = await response.Content.ReadAsStringAsync();
                dynamic content = DynamicJson.Parse(content_raw);
                string url_path = content.data.game.latest.decompressed_path;
                string resp = await GetStringAsync($"{url_path}/pkg_version", logger);
                foreach (string voice_pack in voice_packs)
                {
                    resp += await GetStringAsync($"{url_path}/Audio_{voice_pack}_pkg_version", logger);
                }
                foreach (string line in resp.Split('\n'))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        JsonNode? json = JsonNode.Parse(line);
                        if (json != null)
                        {
                            JsonNode? json1 = JsonNode.Parse(
                                $"{{" +
                                $"\"remoteName\":\"{json["remoteName"]}\"," +
                                $"\"md5\":\"{json["md5"]}\"," +
                                $"\"fileSize\":{json["fileSize"]}" +
                                $"}}");
                            if (json1 != null)
                            {
                                online_pkg_version.Add(json1);
                            }
                        }
                    }
                }
                await WriteJsonFileAsync($"{path_temp}\\online_pkg_version", online_pkg_version, logger);
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }

            SetProgressStyle(ProgressBarStyle.Blocks);
            return online_pkg_version;
        }

        private static async Task GetLocalPkgVersion(string path_temp, string path_game, string key, List<JsonNode> online_pkg_version, Action<ProgressBarStyle> SetProgressStyle, Func<(int, int)> GetProgressValue, Action<(int, int)> SetProgressValue, Action<string>? logger = null)
        {
            SetProgressStyle(ProgressBarStyle.Continuous);
            File.Delete($"{path_temp}\\local_pkg_version");
            List<JsonNode> local_pkg_version = new();
            List<string> files = new();
            files.AddRange(GetFiles(path_game, false, logger));
            files.AddRange(GetFiles($"{path_game}\\{(key == "global" ? "GenshinImpact" : "YuanShen")}_Data", true, logger));
            files = files.Where(file => !FreeFile(path_game, file)).ToList();
            int totalsize = 0;
            int zoom = 0b1;
            foreach (string file in files)
            {
                int filesize = (int)new FileInfo(file).Length / zoom;
                while (totalsize + filesize >= 0x8000)
                {
                    zoom *= 0b10;
                    filesize /= 2;
                    totalsize /= 2;
                }
                totalsize += filesize;
            }
            SetProgressValue.Invoke((0, totalsize));
            foreach (string file in files)
            {
                //string file = files[GetProgressValue().Item1];
                int filesize = (int)new FileInfo(file).Length / zoom;
                FileR? fileR = await GetFileInfoAsync(path_game, file, false, logger);
                JsonNode? online = online_pkg_version.Find((s) =>
                {
                    string? name = s["remoteName"]?.ToString();
                    return name == fileR?.remoteName;
                });
                string? size = online?["fileSize"]?.ToString();
                if (size == fileR?.fileSize.ToString())
                {
                    fileR = await GetFileInfoAsync(path_game, file, true);
                }
                else
                {
                    logger?.Invoke($"Skipped hashing due to wrong file size which shall be {FileH.ParseSize(long.Parse(size ?? "0"))}");
                }

                JsonNode? json = fileR?.GetJSON();
                if (json != null)
                {
                    local_pkg_version.Add(json);
                }

                (int, int) progressValue = GetProgressValue();
                if (progressValue.Item1 + filesize <= progressValue.Item2)
                {
                    SetProgressValue((progressValue.Item1 + filesize, progressValue.Item2));
                }
            }
            SetProgressValue((0, 0));
            SetProgressStyle(ProgressBarStyle.Marquee);
            await WriteJsonFileAsync($"{path_temp}\\local_pkg_version", local_pkg_version, logger);
            SetProgressStyle(ProgressBarStyle.Blocks);
        }

        private static async Task WriteJsonFileAsync(string path, List<JsonNode> json, Action<string>? logger = null)
        {
            logger?.Invoke($"{path}");

            List<string> lines = new();
            json.Sort((JsonNode x, JsonNode y) =>
            {
                string? X = (string?)x["remoteName"], Y = (string?)y["remoteName"];
                return X switch
                {
                    null => Y is null ? 0 : -1,
                    _ => Y is null ? 1 : string.Compare(X, Y),
                };
            });
            json.ForEach((JsonNode node) =>
            {
                lines.Add(node.ToJsonString());
            });
            await File.AppendAllLinesAsync(path, lines.ToArray(), System.Text.Encoding.UTF8);
        }

        private static string[] GetFiles(string path, bool alldir = false, Action<string>? logger = null)
        {
            logger?.Invoke($"{path}");
            return Directory.GetFiles(path, "*", (SearchOption)(alldir ? 1 : 0));
        }

        private static async Task<string> GetStringAsync(string url, Action<string>? logger = null)
        {
            logger?.Invoke($"{url}");
            try
            {
                return await Const.Client.GetStringAsync(url);
            }
            catch (Exception ex) { throw new NotImplementedException(ex.Message, ex); }
        }

        private static async Task<FileR?> GetFileInfoAsync(string gamePath, string filePath, bool hash = false, Action<string>? logger = null)
        {
            if (FreeFile(gamePath, filePath))
            {
                return null;
            }

            string remoteName = filePath.Replace(gamePath + "\\", "").Replace("\\", "/");
            long fileSize = new FileInfo(filePath).Length;
            logger?.Invoke($"{remoteName} ({FileH.ParseSize(fileSize)})");
            string md5 = hash ? Convert.ToHexString(await MD5.Create().ComputeHashAsync(File.OpenRead(filePath))) : "";
            return new FileR(remoteName, md5, fileSize);
        }

        private static bool FreeFile(string gamePath, string filePath)
        {
            string remoteName = filePath.Replace(gamePath + "\\", "").Replace("\\", "/");
            return remoteName.Equals("config.ini")
                || remoteName.EndsWith("pkg_version")
                || remoteName.Contains("/webCaches/")
                || remoteName.Contains("/SDKCaches/")
                || remoteName.Contains("/Persistent/");
        }

        private async void Button2_Fix_Click(object sender, EventArgs e)
        {
            ButtonLock(true);
            if (File.Exists(Const.TempPath + "\\online_pkg_version") && File.Exists(Const.TempPath + "\\local_pkg_version"))
            {
                try
                {
                    SetProgressStyle(ProgressBarStyle.Marquee);
                    string Path1 = $"{DirectoryH.EnsureExists(Const.TempPath).FullName}\\online_pkg_version";
                    Logger($"{Path1}");
                    string[] online_pkg_version = await File.ReadAllLinesAsync(Path1);

                    string Path2 = $"{DirectoryH.EnsureExists(Const.TempPath).FullName}\\local_pkg_version";
                    Logger($"{Path2}");
                    string[] local_pkg_version = await File.ReadAllLinesAsync(Path2);

                    List<FileR> missing = FromStringArray(online_pkg_version.Except(local_pkg_version).ToArray());
                    List<FileR> surplus = FromStringArray(local_pkg_version.Except(online_pkg_version).ToArray());

                    Logger($"缺少 {missing.Count} 个文件，多余 {surplus.Count} 个文件。");
                    if (missing.Count > 0)
                    {
                        await WriteMissing(missing, Logger);
                    }
                    if (surplus.Count > 0)
                    {
                        await WriteSurplus(surplus, Logger);
                    }
                    string tempPath = DirectoryH.EnsureExists(Const.TempPath).FullName;
                    HttpResponseMessage response = await Const.Client.GetAsync("");
                    if (response.IsSuccessStatusCode)
                    {
                        string content_raw = await response.Content.ReadAsStringAsync();
                        dynamic content = DynamicJson.Parse(content_raw);
                        string url_path = content.data.game.latest.decompressed_path;
                        // string url_path = await GetStringAsync($"{Const.APIs[key].Value}/data/game/latest/decompressed_path", Logger);
                        if (File.Exists($"{tempPath}\\downloadfiles.txt"))
                        {
                            Logger("开始下载缺失的文件。");
                            await FileH.ApplyDownload(url_path, tempPath);
                        }

                        if (File.Exists($"{tempPath}\\deletefiles.txt"))
                        {
                            Logger("开始删除多余的文件。");
                            await FileH.ApplyDelete(path, tempPath);
                        }

                        Logger("更新 pkg_version..");
                        await WritePkgVersion(tempPath, url_path);

                        string version_new = content.data.game.latest.version;
                        Logger($"应用更新，版本号：{version_new}");
                        await FileH.ApplyUpdate(path, tempPath, version_new);
                    }
                    else
                    {
                        throw new Exception(response.StatusCode.ToString());
                    }
                }
                catch (Exception ex)
                {
                    Logger($"{ex.Message}");
                }
                finally
                {
                    Logger("修复结束");
                    SetProgressStyle(ProgressBarStyle.Blocks);
                    SetProgressValue((0, 0));
                }
            }
            else
            {
                _ = MessageBox.Show(this, $"请先执行 {button1_Count.Text}。", Text);
            }
            ButtonLock(false);

            async Task WriteMissing(List<FileR> missing, Action<string> messager)
            {
                string path = $"{DirectoryH.EnsureExists(Const.TempPath).FullName}\\downloadfiles.txt";
                messager($"{path}");
                List<string> lines = new();
                foreach (FileR file in missing)
                {
                    string? json = file.GetJSON()?.ToJsonString();
                    if (json != null)
                    {
                        lines.Add(json);
                    }
                }

                await File.WriteAllLinesAsync(path, lines.ToArray());
            }

            async Task WriteSurplus(List<FileR> surplus, Action<string> messager)
            {
                string path = $"{DirectoryH.EnsureExists(Const.TempPath).FullName}\\deletefiles.txt";
                messager($"{path}");
                List<string> lines = new();
                foreach (FileR file in surplus)
                {
                    lines.Add(file.remoteName);
                }

                await File.WriteAllLinesAsync(path, lines.ToArray());
            }

            async Task WritePkgVersion(string tempPath, string url_path)
            {
                File.Delete(tempPath + "\\online_pkg_version");
                File.Delete(tempPath + "\\local_pkg_version");
                await File.WriteAllTextAsync($"{tempPath}\\pkg_version", await GetStringAsync($"{url_path}/pkg_version", Logger));
                foreach (string lang in voice_packs)
                {
                    await File.WriteAllTextAsync($"{tempPath}\\Audio_{lang}_pkg_version", await GetStringAsync($"{url_path}/Audio_{lang}_pkg_version", Logger));
                }
            }
        }

        private void Logger(string log)
        {
            textBox_log.AppendText($"[线程 {Thread.GetCurrentProcessorId()}] {log}\r\n");
        }

        private void SetProgressStyle(ProgressBarStyle style)
        {
            progressBar1.Style = style;
            TaskbarList.Instance.SetProgressState(Handle, (int)style switch
            {
                1 => TaskbarProgressBarStatus.Normal,
                2 => TaskbarProgressBarStatus.Indeterminate,
                _ => TaskbarProgressBarStatus.NoProgress
            });
        }

        private (int, int) GetProgressValue()
        {
            return (progressBar1.Value, progressBar1.Maximum);
        }

        private void SetProgressValue((int, int) value)
        {
            (progressBar1.Value, progressBar1.Maximum) = value;
            TaskbarList.Instance.SetProgressValue(Handle, (ulong)value.Item1, (ulong)value.Item2);
            Text = value.Item2 != 0 ? $"{(double)value.Item1 / value.Item2 * 100:f2}% - {FormName}" : FormName;
        }

        private static List<FileR> FromStringArray(string[] s1)
        {
            List<FileR> list = new();
            foreach (string s in s1)
            {
                JsonNode? json = JsonNode.Parse(s);
                if (json != null)
                {
                    list.Add(new(json));
                }
            }
            return list;
        }

        private void Button3_Luanch_Click(object sender, EventArgs e)
        {
            ButtonLock(true);
            try
            {
                using Process? process = Process.Start(new ProcessStartInfo()
                {
                    FileName = $"{DirectoryH.EnsureExists(path).FullName}\\{(key == "global" ? "GenshinImpact" : "Yuanshen")}.exe"
                });
                if (process != null)
                {
                    Application.Exit();
                }
                else
                {
                    throw new NullReferenceException(nameof(process));
                }
            }
            catch (Exception)
            {
                _ = MessageBox.Show(this, $"请先执行 {button2_Fix.Text}。", Text);
            }
            ButtonLock(false);
        }
    }
}
