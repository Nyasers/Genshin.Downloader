using Genshin.Downloader.Helpers;
using System.Configuration;
using System.Diagnostics;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Genshin.Downloader
{
    public partial class Form_Downloader : Form
    {
        private const string DownPath = "Downloads", TempPath = "Temp", NormalAPI = "global";
        private readonly KeyValueConfigurationCollection APIs = new()
        {
            new KeyValueConfigurationElement("global", "https://genshin-global.nyaser.tk"),
            new KeyValueConfigurationElement("official", "https://genshin-official.nyaser.tk"),
            new KeyValueConfigurationElement("bilibili", "https://genshin-bilibili.nyaser.tk")
        };

        private HttpClient Client { get; set; } = new();

        public Form_Downloader()
        {
            InitializeComponent();
            MinimumSize = Size;
        }

        #region ControlEvents
        private void Form_Downloader_Load(object sender, EventArgs e)
        {
            if (Config.Read("run") != bool.TrueString)
            {
                string message = $@"1. 此提示信息仅在首次运行时显示。

2. 下载的文件保存在工作目录下的 {DownPath} 文件夹，请确保磁盘空间充足。
位置：{Directory.GetCurrentDirectory()}\{DownPath}

3. 下载后的文件请保留其原名称，安装器需识别文件的名称以获取一些信息。

4. 安装器在安装过程中，会将文件缓存在工作目录下的 {TempPath} 文件夹，注意磁盘空间。
位置：{Directory.GetCurrentDirectory()}\{TempPath}

5. 建议先安装语音包再安装游戏本体，安装游戏本体后将更新游戏配置文件的版本号。";
                _ = MessageBox.Show(this, message, Text);
            }

            foreach (KeyValueConfigurationElement api in APIs)
            {
                _ = comboBox_API.Items.Add($"{api.Key} => {api.Value}");
            }

            SetAPIByKey(Config.Read("api") ?? NormalAPI);
            textBox_path.Text = Config.Read("path");
        }

        private void Form_Downloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Write("run", bool.TrueString);
            Config.Write("api", GetKeyByAPI() ?? NormalAPI);
            Config.Write("path", textBox_path.Text);
        }

        private void ComboBox_API_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? key = GetKeyByAPI();
            Client.Dispose();
            Client = new HttpClient()
            {
                BaseAddress = new Uri(APIs[key].Value),
                DefaultRequestVersion = Version.Parse("2.0"),
            };
        }

        private void Button_Path_Browse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                if (!path.EndsWith(@"\Genshin Impact game"))
                {
                    if (!path.EndsWith('\\'))
                    {
                        path += '\\';
                    }
                    path += "Genshin Impact game";
                }
                textBox_path.Text = path;
            }
        }

        private async void Button_Check_Update_Click(object sender, EventArgs e)
        {
            comboBox_API.Enabled = button_check_update.Enabled = false;
            await CheckUpdate(checkBox_pre_download.Checked);
            comboBox_API.Enabled = button_check_update.Enabled = true;
        }

        private void TextBox_Path_TextChanged(object sender, EventArgs e)
        {
            if (textBox_path.Text.EndsWith("\\Genshin Impact game"))
            {
                string path = textBox_path.Text;

                string path_config = $"{path}\\config.ini";
                if (File.Exists(path_config))
                {
                    textBox_version_current.Text = INI.Read("General", "game_version", path_config);
                }

                string path_voicePacks = @$"{path}\GenshinImpact_Data\StreamingAssets\AudioAssets\";
                if (Directory.Exists(path_voicePacks))
                {
                    for (int i = 0; i < checkedListBox_voicePacks.Items.Count; i++)
                    {
                        string? language = checkedListBox_voicePacks.Items[i].ToString()?[7..];
                        if (checkedListBox_voicePacks.Items[i] is not null && language is not null)
                        {
                            checkedListBox_voicePacks.SetItemChecked(i, Directory.Exists(path_voicePacks + language));
                        }
                    }
                }
            }
        }

        private async void Button_Download_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"创建下载任务..";
            List<File2Down> files = new();
            foreach (File2Down file in listBox_file2down.SelectedItems)
            {
                files.Add(file);
            }
            int exitCode = await DownloadFileAsync(files.ToArray(), 1);
            toolStripStatusLabel1.Text = exitCode switch
            {
                -1 => $"启动 aria2c.exe 失败。",
                0 => $"下载成功。",
                7 => $"下载取消。",
                9 => $"下载失败，磁盘空间不足。",
                _ => $"下载结束，返回值：{exitCode}。",
            };
        }

        private void Button_Open_Installer_Click(object sender, EventArgs e)
        {
            string path_game = textBox_path.Text;
            if (path_game.EndsWith("\\Genshin Impact game"))
            {
                if (!Directory.Exists(path_game))
                {
                    _ = Directory.CreateDirectory(path_game);
                }

                using Form_Installer form = new(path_game, @$"{Directory.GetCurrentDirectory()}\{DownPath}", @$"{Directory.GetCurrentDirectory()}\{TempPath}");
                form.FormClosed += (object? sender, FormClosedEventArgs e) => Show();
                Hide();
                _ = form.ShowDialog(this);
            }
            else
            {
                Button_Path_Browse_Click(sender, e);
            }
        }

        private void ListBox_file2down_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_select_all.Text = (listBox_file2down.Items.Count == listBox_file2down.SelectedItems.Count) ? "全不选" : "全选";
        }

        private void Button_select_all_Click(object sender, EventArgs e)
        {
            bool select = listBox_file2down.Items.Count != listBox_file2down.SelectedItems.Count;
            for (int i = 0; i < listBox_file2down.Items.Count; i++)
            {
                listBox_file2down.SetSelected(i, select);
            }
        }

        #endregion

        #region ToolFunction

        private void SetAPIByKey(string key)
        {
            foreach (string? api in from string api in comboBox_API.Items
                                    where api.StartsWith(key)
                                    select api)
            {
                comboBox_API.SelectedItem = api;
            }
        }

        private string GetKeyByAPI()
        {
            string? key = comboBox_API.SelectedItem.ToString();
            key = string.IsNullOrEmpty(key) ? NormalAPI : key;
            key = key[..key.IndexOf(" => ")];
            return key;
        }

        private async Task CheckUpdate(bool pre_download = false)
        {
            try
            {
                string game = pre_download ? "pre_download_game" : "game";
                HttpResponseMessage message = await Client.GetAsync($"/data/{game}/latest/version");
                if (message.IsSuccessStatusCode)
                {
                    textBox_version_latest.Text = await message.Content.ReadAsStringAsync();
                    toolStripStatusLabel1.Text = textBox_version_current.Text == textBox_version_latest.Text
                        ? "已是最新版本，可重新下载完整文件。"
                        : $"存在 {textBox_version_latest.Text} 版本可下载。";
                    string requestUri = $"/data/{game}/diffs?version=" + textBox_version_current.Text;
                    HttpResponseMessage message1 = await Client.GetAsync(requestUri);
                    if (!message1.IsSuccessStatusCode)
                    {
                        requestUri = $"/data/{game}/latest";
                    }
                    listBox_file2down.Items.Clear();
                    await File2Down_Add(requestUri);
                    if (requestUri.EndsWith("latest"))
                    {
                        string res = await Client.GetStringAsync(requestUri + "/segments");
                        JsonNode? json = JsonNode.Parse(res);
                        if (json is not null)
                        {
                            int count = json.AsArray().Count;
                            for (int i = 0; i < count; i++)
                            {
                                await File2Down_Add(requestUri + $"/segments/{i}");
                            }
                        }
                    }
                    string pattern = @"\[(.+)\]";
                    foreach (string item in checkedListBox_voicePacks.CheckedItems)
                    {
                        string language = Regex.Match(item, pattern).Value[1..^1];
                        await File2Down_Add(requestUri + "/voice_packs?language=" + language);
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "无法获取版本信息，错误：" + message.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(this, ex.Message, "错误");
            }
        }

        private async Task File2Down_Add(string requestUri)
        {
            File2Down? file = await new File2Down().BuildAsync(Client, requestUri);
            _ = file is null ? 0 : listBox_file2down.Items.Add(file);
        }

        private async Task<int> DownloadFileAsync(File2Down[] files, int log_level = 0, int console_log_level = 2)
        {
            if (files is not null)
            {
                string file_list = string.Empty, origin_input = string.Empty;
                long size_total = 0;
                foreach (File2Down file in files)
                {
                    file_list += $"\t{file}\n";
                    origin_input += $"#{file}\n" +
                        $"{file.path}\n" +
                        $" out={file.name}\n" +
                        $" checksum=md5={file.md5}\n\n";
                    size_total += file.size;
                }
                origin_input += $"#Count: {files.Length} file(s), {FileH.ParseSize(size_total)} in total.\n";
                string time_now = $"{DateTime.Now.Year:0000}{DateTime.Now.Month:00}{DateTime.Now.Day:00}{DateTime.Now.Hour:00}{DateTime.Now.Minute:00}{DateTime.Now.Second:00}";
                string file_input = $"{time_now}.aria2";
                string file_log = $"{time_now}.aria2.log";
                if (!Directory.Exists(DownPath))
                {
                    _ = Directory.CreateDirectory(DownPath);
                }
                await File.WriteAllTextAsync($"{DownPath}\\{file_input}", origin_input);
                return await DownloadFileDialogAsync(log_level, console_log_level, file_list, file_input, file_log);
            }
            return -1;
        }

        private async Task<int> DownloadFileDialogAsync(int log_level, int console_log_level, string file_list, string file_input, string file_log)
        {
            List<string> aria2c_log_levels = new()
            {
                "debug", "info", "notice", "warn", "error"
            };
            List<string> aria2c_errors = new() {
                "All downloads were successful.",
                "An unknown error occurred.",
                "Time out occurred.",
                "A resource was not found.",
                "Aria2 saw the specified number of \"resource not found\" error. See --max-file-not-found option.",
                "A download aborted because download speed was too slow. See --lowest-speed-limit option.",
                "Network problem occurred.",
                "There were unfinished downloads. This error is only reported if all finished downloads were successful and there were unfinished downloads in a queue when aria2 exited by pressing Ctrl-C by an user or sending TERM or INT signal.",
                "Remote server did not support resume when resume was required to complete download.",
                "There was not enough disk space available.",
                "Piece length was different from one in .aria2 control file. See --allow-piece-length-change option.",
                "Aria2 was downloading same file at that moment.",
                "Aria2 was downloading same info hash torrent at that moment.",
                "FileH already existed. See --allow-overwrite option.",
                "Renaming file failed. See --auto-file-renaming option.",
                "Aria2 could not open existing file.",
                "Aria2 could not create new file or truncate existing file.",
                "FileH I/O error occurred.",
                "Aria2 could not create directory.",
                "Name resolution failed.",
                "Aria2 could not parse Metalink document.",
                "FTP command failed.",
                "HTTP response header was bad or unexpected.",
                "Too many redirects occurred.",
                "HTTP authorization failed.",
                "Aria2 could not parse bencoded file (usually \".torrent\" file).",
                "\".torrent\" file was corrupted or missing information that aria2 needed.",
                "Magnet URI was bad.",
                "Bad/Unrecognized option was given or unexpected option argument was given.",
                "The remote server was unable to handle the request due to a temporary overloading or maintenance.",
                "Aria2 could not parse JSON-RPC request.",
                "Reserved. Not used.",
                "Checksum validation failed."
            };
            switch (MessageBox.Show(this, $"文件列表：\n{file_list}\n选择是：创建下载任务 (aria2c.exe)\n选择否：查看原始输入 (notepad.exe)", $"下载文件：{file_input}", MessageBoxButtons.YesNoCancel))
            {
                case DialogResult.Yes:
                    toolStripStatusLabel1.Text = $"下载任务已创建：{file_input}。";
                    int exitCode = -1;
                    ProcessStartInfo startInfo = new()
                    {
                        FileName = "aria2c.exe",
                        Arguments = $"-R --log-level={aria2c_log_levels[log_level]} --console-log-level={aria2c_log_levels[console_log_level]} " +
                        $"--input-file={file_input} --log={file_log}",
                        WorkingDirectory = DownPath,
                        UseShellExecute = false,
                        ErrorDialog = true,
                    };
                    Process? process = Process.Start(startInfo);
                    if (process is not null)
                    {
                        await process.WaitForExitAsync();
                        exitCode = process.ExitCode;
                    }
                    string error = "Unknown error.";
                    if (exitCode is >= 0 and <= 32)
                    {
                        error = aria2c_errors[exitCode];
                    }
                    _ = MessageBox.Show(this, error, "下载结束");
                    return exitCode;
                case DialogResult.No:
                    File.SetAttributes($"{DownPath}\\{file_input}", FileAttributes.ReadOnly);
                    await Process.Start("notepad.exe", $"{DownPath}\\{file_input}").WaitForExitAsync();
                    File.SetAttributes($"{DownPath}\\{file_input}", FileAttributes.Normal);
                    return await DownloadFileDialogAsync(log_level, console_log_level, file_list, file_input, file_log);
                default:
                    File.Delete($"{DownPath}\\{file_input}");
                    return 7;
            }
        }

        #endregion
    }
}