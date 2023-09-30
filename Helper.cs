using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;

namespace Genshin.Downloader
{
    namespace Helper
    {
        internal static class Const
        {
            public const string DownPath = "Downloads";
            public const string TempPath = "Temp";
            public const string NormalAPI = "global";

            public static HttpClient Client { get; set; } = new();
            public static readonly KeyValueConfigurationCollection APIs = new()
            {
                new KeyValueConfigurationElement("global", "https://genshin-global.nyaser.tk"),
                new KeyValueConfigurationElement("official", "https://genshin-official.nyaser.tk"),
                new KeyValueConfigurationElement("bilibili", "https://genshin-bilibili.nyaser.tk")
            };
        }

        internal static class Help
        {
            public static DialogResult Show(Form? owner = null)
            {
                string message = $"1. 下载的文件保存在工作目录下的 {Const.DownPath} 文件夹，请确保磁盘空间充足。\r\n位置：{Directory.GetCurrentDirectory()}\\{Const.DownPath}\r\n\r\n2. 下载后的文件请保留其原名称，安装器需识别文件的名称以获取一些信息。\r\n\r\n3. 安装器在安装过程中，会将文件缓存在工作目录下的 {Const.TempPath} 文件夹，注意磁盘空间。\r\n位置：{Directory.GetCurrentDirectory()}\\{Const.TempPath}\r\n\r\n4. 请先安装语音包再安装游戏本体，安装游戏本体后将更新配置文件中的游戏版本号。";
                return MessageBox.Show(owner, message, owner?.Text);
            }
        }

        internal static class Game
        {
            public static string? GetVersion(string path)
            {
                if (File.Exists($"{path}\\config.ini"))
                {
                    string version = INI.Read("General", "game_version", $"{path}\\config.ini");
                    return string.IsNullOrWhiteSpace(version) ? null : version;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static class StringH
        {
            public static string? EmptyCheck(string? left)
            {
                return !string.IsNullOrEmpty(left) ? left : null;
            }
        }

        internal static class ArrayH<T>
        {
            public static T[] Parse(CheckedListBox.CheckedItemCollection collection)
            {
                List<T> list = new();
                foreach (T item in collection)
                {
                    list.Add(item);
                }
                return list.ToArray();
            }
        }

        internal static class FileH
        {
            public static string GetName(string path)
            {
                return path.Split("/")[^1];
            }

            public static string ParseSize(long size)
            {
                return Unit.Parse(1024, new string[] { "Byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB" }, size);
            }

            public static async Task<long> GetSizeAsync(string path)
            {
                return (await new HttpClient().GetAsync(path, HttpCompletionOption.ResponseHeadersRead)).Content.Headers.ContentLength ?? 0;
            }

            public static async Task HPatchAsync(string path_game, string path_temp)
            {
                if (File.Exists($"{path_temp}\\deletefiles.txt"))
                {
                    await ApplyDelete(path_game, path_temp);
                }
                if (File.Exists($"{path_temp}\\hdifffiles.txt"))
                {
                    await ApplyHDiff(path_game, path_temp);
                }
            }

            public static async Task ApplyHDiff(string path_game, string path_temp)
            {
                string hdiff_file = $"{path_temp}\\hdifffiles.txt";
                string batch_file = $"{path_temp}\\hdifffiles.bat";
                string hpatchz = $"{Directory.GetCurrentDirectory()}\\hpatchz.exe";
                StreamReader reader = new(hdiff_file);
                StreamWriter writer = new(batch_file, false, new UTF8Encoding(false));
                await writer.WriteLineAsync("@echo off & chcp 65001");
                await writer.WriteLineAsync("@title 正在处理差异文件..");
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        string pattern = @"{""remoteName"": ""(.+)""}";
                        Match match = Regex.Match(line, pattern);
                        if (match.Success)
                        {
                            string remoteName = match.Groups[1].Value.Replace("/", "\\");
                            string oldFile = $"{path_game}\\{remoteName}";
                            string diffFile = $"{path_temp}\\{remoteName}.hdiff";
                            string newFile = $"{path_temp}\\{remoteName}";
                            string command_line = $"\"{hpatchz}\" \"{oldFile}\" \"{diffFile}\" \"{newFile}\" & del /f \"{diffFile}\"";
                            await writer.WriteLineAsync(command_line);
                        }
                    }
                }
                reader.Close();
                await writer.WriteLineAsync($"del /f \"{hdiff_file}\"");
                await writer.WriteLineAsync("del %0 & exit");
                writer.Close();
                Process? process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{batch_file}\""
                });
                if (process is not null)
                {
                    await process.WaitForExitAsync();
                }
            }

            public static async Task ApplyDelete(string path_game, string path_temp)
            {
                string delete_file = $"{path_temp}\\deletefiles.txt";
                string batch_file = $"{path_temp}\\deletefiles.bat";
                StreamReader reader = new(delete_file);
                StreamWriter writer = new(batch_file, false, new UTF8Encoding(false));
                await writer.WriteLineAsync("@echo off & chcp 65001");
                await writer.WriteLineAsync("@title 正在删除旧文件..");
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        string command_line = $"del /f \"{path_game}\\{line.Replace("/", "\\")}\"";
                        await writer.WriteLineAsync(command_line);
                    }
                }
                reader.Close();
                await writer.WriteLineAsync($"del /f \"{delete_file}\"");
                await writer.WriteLineAsync("del %0 & exit");
                writer.Close();
                Process? process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{batch_file}\""
                });
                if (process is not null)
                {
                    await process.WaitForExitAsync();
                }
            }

            public static async Task ApplyDownload(string url, string path_temp)
            {
                string download_file = $"{path_temp}\\downloadfiles.txt";
                string input_file = $"{path_temp}\\download.aria2c.txt";
                List<File2Down> files = new();
                foreach (string line in await File.ReadAllLinesAsync(download_file))
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        JsonNode? json = JsonNode.Parse(line);
                        if (json != null)
                        {
                            string remoteName = (string?)json["remoteName"] ?? string.Empty;
                            string name = GetName(remoteName);
                            string path = $"{url}/{remoteName}";
                            long size = (long?)json["fileSize"] ?? 0;
                            string md5 = (string?)json["md5"] ?? string.Empty;
                            File2Down file = new() { name = name, remoteName = remoteName, path = path, size = size, md5 = md5 };
                            if (!string.IsNullOrEmpty(file.remoteName))
                            {
                                files.Add(file);
                            }
                        }
                    }
                }
                string input = GetAria2cInput(files.ToArray());
                await File.WriteAllTextAsync(input_file, input);
                Process? process = Process.Start(new ProcessStartInfo
                {
                    FileName = "aria2c.exe",
                    Arguments = $"-R --input-file=\"{input_file}\"",
                    UseShellExecute = false,
                    ErrorDialog = true,
                    WorkingDirectory = path_temp
                });
                if (process is not null)
                {
                    await process.WaitForExitAsync();
                    File.Delete(input_file);
                    if (process.ExitCode is not 0)
                    {
                        throw new Exception($"aria2c.exe exited with exception code {process.ExitCode}.");
                    }
                    File.Delete(download_file);
                }
            }

            public static async Task UnzipAsync(string @in, string @out)
            {
                string seven_zip = $"{Directory.GetCurrentDirectory()}\\7za.exe";
                Process? process = Process.Start(new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c title 正在解压资源包.. & \"{seven_zip}\" x -o\"{@out}\" \"{@in}\"",
                    UseShellExecute = false,
                    ErrorDialog = true
                });
                if (process is not null)
                {
                    await process.WaitForExitAsync();
                    if (process.ExitCode is not 0)
                    {
                        throw new Exception($"7za.exe exited with exception code {process.ExitCode}.");
                    }
                }
            }

            public static async Task<(string list, string input, string log)> GetDownloadInfoAsync(File2Down[] files)
            {
                if (files is not null)
                {
                    DateTime now = DateTime.Now;
                    string file_list = string.Empty;
                    foreach (File2Down file in files)
                    {
                        file_list += $"\t{file}\n";
                    }
                    string origin_input = GetAria2cInput(files);
                    string file_name = $"{now.Year:0000}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00}";
                    string file_input = $"{file_name}.aria2";
                    string file_log = $"{file_name}.aria2.log";
                    _ = DirectoryH.EnsureExists(Const.DownPath);
                    await File.WriteAllTextAsync($"{Const.DownPath}\\{file_input}", origin_input);
                    return (file_list, file_input, file_log);
                }
                throw new ArgumentNullException(nameof(files));
            }

            public static string GetAria2cInput(File2Down[] files)
            {
                long size_total = 0;
                string origin_input = $"#Created Time: {DateTime.Now.ToLocalTime()}\n";
                foreach (File2Down file in files)
                {
                    string? outPath = string.IsNullOrEmpty(file.remoteName) ? file.name : file.remoteName;
                    origin_input += $"#{file}\n" +
                    $"{file.path}\n" +
                    $" out={outPath}\n" +
                    $" checksum=md5={file.md5}\n\n";
                    size_total += file.size;
                }
                origin_input += $"#Count: {files.Length} file(s), {ParseSize(size_total)} in total.\n";
                return origin_input;
            }

            public static async Task<int> DownloadFileDialogAsync((string list, string input, string log) download_info, IWin32Window? form, ToolStripStatusLabel? label, int log_level = 1, int console_log_level = 2)
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
                "Aria2 saw the specified number of \"resource not found\" error. See --max-download_info-not-found option.",
                "A download aborted because download speed was too slow. See --lowest-speed-limit option.",
                "Network problem occurred.",
                "There were unfinished downloads. This error is only reported if all finished downloads were successful and there were unfinished downloads in a queue when aria2 exited by pressing Ctrl-C by an user or sending TERM or INT signal.",
                "Remote server did not support resume when resume was required to complete download.",
                "There was not enough disk space available.",
                "Piece length was different from one in .aria2 control download_info. See --allow-piece-length-change option.",
                "Aria2 was downloading same download_info at that moment.",
                "Aria2 was downloading same info hash torrent at that moment.",
                "FileH already existed. See --allow-overwrite option.",
                "Renaming download_info failed. See --auto-download_info-renaming option.",
                "Aria2 could not open existing download_info.",
                "Aria2 could not create new download_info or truncate existing download_info.",
                "FileH I/O error occurred.",
                "Aria2 could not create directory.",
                "Name resolution failed.",
                "Aria2 could not parse Metalink document.",
                "FTP command failed.",
                "HTTP response header was bad or unexpected.",
                "Too many redirects occurred.",
                "HTTP authorization failed.",
                "Aria2 could not parse bencoded download_info (usually \".torrent\" download_info).",
                "\".torrent\" download_info was corrupted or missing information that aria2 needed.",
                "Magnet URI was bad.",
                "Bad/Unrecognized option was given or unexpected option argument was given.",
                "The remote server was unable to handle the request due to a temporary overloading or maintenance.",
                "Aria2 could not parse JSON-RPC request.",
                "Reserved. Not used.",
                "Checksum validation failed."
            };

                switch (MessageBox.Show(form, $"文件列表：\n{download_info.list}\n选择是：创建下载任务 (aria2c.exe)\n选择否：查看原始输入 (notepad.exe)", $"下载文件：{download_info.input}", MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        if (label is not null)
                        {
                            label.Text = $"下载任务已创建：{download_info.input}。";
                        }

                        int exitCode = -1;
                        ProcessStartInfo startInfo = new()
                        {
                            FileName = "aria2c.exe",
                            Arguments = $"-R --log-level={aria2c_log_levels[log_level]} --console-log-level={aria2c_log_levels[console_log_level]} " +
                            $"--input-file=\"{download_info.input}\" --log=\"{download_info.log}\"",
                            WorkingDirectory = DirectoryH.EnsureExists(Const.DownPath).FullName,
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
                        _ = MessageBox.Show(form, error, "下载结束");
                        return exitCode;
                    case DialogResult.No:
                        File.SetAttributes($"{Const.DownPath}\\{download_info.input}", FileAttributes.ReadOnly);
                        await Process.Start("notepad.exe", $"{Const.DownPath}\\{download_info.input}").WaitForExitAsync();
                        File.SetAttributes($"{Const.DownPath}\\{download_info.input}", FileAttributes.Normal);
                        return await DownloadFileDialogAsync(download_info, form, label, log_level, console_log_level);
                    default:
                        File.Delete($"{Const.DownPath}\\{download_info.input}");
                        return 7;
                }
            }

            public static async Task ApplyUpdate(string path_game, string path_temp, string? version_new = null)
            {
                if (version_new is not null)
                {
                    INI.Write("General", "game_version", version_new, $"{path_game}\\config.ini");
                }

                string command_line = $"xcopy /f /e /y \"{path_temp}\" \"{path_game}\" && del /s /q \"{path_temp}\\*\" && rd /s /q \"{path_temp}\\GenshinImpact_Data\" || pause";
                Process? process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c title 正在应用更新.. & {command_line}"
                });
                if (process != null)
                {
                    await process.WaitForExitAsync();
                }
            }
        }

        internal static class DirectoryH
        {
            public static DirectoryInfo EnsureExists(string path)
            {
                return Directory.Exists(path) ? new DirectoryInfo(path) : Directory.CreateDirectory(path);
            }
        }

        internal partial class File2Down
        {
            public string name;
            public string path;
            public string remoteName;
            public long size;
            public string md5;

            public string ParsedSize => FileH.ParseSize(size);

            public File2Down()
            {
                name = "";
                remoteName = "";
                path = "";
                size = 0;
                md5 = "";
            }

            public async Task<File2Down?> BuildAsync(string requestUri)
            {
                try
                {
                    string res = await Const.Client.GetStringAsync(requestUri);
                    JsonNode? data = JsonNode.Parse(res);
                    path = (string?)data?["path"] ?? string.Empty;
                    if (string.IsNullOrEmpty(path))
                    {
                        throw new();
                    }
                    name = StringH.EmptyCheck((string?)data?["name"]) ?? FileH.GetName(path);
                    size = long.Parse((string?)data?["package_size"] ?? "0");
                    size = size == 0 ? await FileH.GetSizeAsync(path) : size;
                    md5 = (string?)data?["md5"] ?? string.Empty;
                }
                catch
                {
                    return null;
                }
                return this;
            }

            public override string ToString()
            {
                return $"{name} ({ParsedSize})";
            }
        }

        internal class FileR
        {
            public string remoteName;
            public string md5;
            public long fileSize;

            public FileR(string remoteName, string md5, long fileSize)
            {
                this.remoteName = remoteName.Replace('\\', '/');
                this.md5 = md5.ToLower();
                this.fileSize = fileSize;
            }

            public FileR(JsonNode json)
            {
                remoteName = (json["remoteName"]?.GetValue<string>() ?? throw new ArgumentException(null, nameof(json))).Replace('\\', '/');
                md5 = (json["md5"]?.GetValue<string>() ?? throw new ArgumentException(null, nameof(json))).ToLower();
                fileSize = json["fileSize"]?.GetValue<long>() ?? throw new ArgumentException(null, nameof(json));
            }

            public JsonNode? GetJSON()
            {
                return JsonNode.Parse($"{{\"remoteName\":\"{remoteName}\",\"md5\":\"{md5}\",\"fileSize\":{fileSize}}}");
            }
        }

        internal static class Unit
        {
            public static string Parse(int @base, string[] units, double num)
            {
                int unit = 0;
                while (@base > 1)
                {
                    if (num.CompareTo(Math.Pow(@base, unit + 1)) is -1 || unit == (units.Length - 1))
                    {
                        return $"{num / Math.Pow(@base, unit):f2} {units[unit]}";
                    }
                    unit++;
                }
                throw new NotImplementedException();
            }
        }

        internal static class INI
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            [DllImport("kernel32", CharSet = CharSet.Unicode)]
            public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


            public static void Write(string section, string key, string value, string path)
            {
                _ = WritePrivateProfileString(section, key, value, path);
            }

            public static string Read(string section, string key, string path)
            {
                System.Text.StringBuilder temp = new(255);
                _ = GetPrivateProfileString(section, key, "", temp, 255, path);
                return temp.ToString();
            }

            public static void Delete(string FilePath)
            {
                File.Delete(FilePath);
            }
        }

        internal static class Config
        {
            private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            private static readonly KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            public static string? Read(string key)
            {
                return settings.AllKeys.Contains(key) ? settings[key].Value : null;
            }

            public static void Write(string key, string? value = null)
            {
                if (settings.AllKeys.Contains(key))
                {
                    settings.Remove(key);
                }
                if (!string.IsNullOrEmpty(value))
                {
                    settings.Add(key, value);
                }
                config.Save(ConfigurationSaveMode.Modified);
            }
        }

        /// <summary>
        /// Represents an instance of the Windows taskbar
        /// </summary>
        public static class TaskbarManager
        {
            /// <summary>
            /// Sets the handle of the window whose taskbar button will be used
            /// to display progress.
            /// </summary>
            private static readonly IntPtr ownerHandle = IntPtr.Zero;

            static TaskbarManager()
            {
                Process currentProcess = Process.GetCurrentProcess();
                if (currentProcess != null && currentProcess.MainWindowHandle != IntPtr.Zero)
                {
                    ownerHandle = currentProcess.MainWindowHandle;
                }
            }

            /// <summary>
            /// Indicates whether this feature is supported on the current platform.
            /// </summary>
            private static bool IsPlatformSupported => Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.CompareTo(new Version(6, 1)) >= 0;

            /// <summary>
            /// Displays or updates a progress bar hosted in a taskbar button of the main application window 
            /// to show the specific percentage completed of the full operation.
            /// </summary>
            /// <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
            /// <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
            public static void SetProgressValue(int currentValue, int maximumValue)
            {
                if (IsPlatformSupported && ownerHandle != IntPtr.Zero)
                {
                    TaskbarList.Instance.SetProgressValue(
                        ownerHandle,
                        Convert.ToUInt32(currentValue),
                        Convert.ToUInt32(maximumValue));
                }
            }

            /// <summary>
            /// Displays or updates a progress bar hosted in a taskbar button of the given window handle 
            /// to show the specific percentage completed of the full operation.
            /// </summary>
            /// <param name="windowHandle">The handle of the window whose associated taskbar button is being used as a progress indicator.
            /// This window belong to a calling process associated with the button's application and must be already loaded.</param>
            /// <param name="currentValue">An application-defined value that indicates the proportion of the operation that has been completed at the time the method is called.</param>
            /// <param name="maximumValue">An application-defined value that specifies the value currentValue will have when the operation is complete.</param>
            public static void SetProgressValue(int currentValue, int maximumValue, IntPtr windowHandle)
            {
                if (IsPlatformSupported)
                {
                    TaskbarList.Instance.SetProgressValue(
                        windowHandle,
                        Convert.ToUInt32(currentValue),
                        Convert.ToUInt32(maximumValue));
                }
            }

            /// <summary>
            /// Sets the type and state of the progress indicator displayed on a taskbar button of the main application window.
            /// </summary>
            /// <param name="state">Progress state of the progress button</param>
            public static void SetProgressState(TaskbarProgressBarState state)
            {
                if (IsPlatformSupported && ownerHandle != IntPtr.Zero)
                {
                    TaskbarList.Instance.SetProgressState(ownerHandle, (TaskbarProgressBarStatus)state);
                }
            }

            /// <summary>
            /// Sets the type and state of the progress indicator displayed on a taskbar button 
            /// of the given window handle 
            /// </summary>
            /// <param name="windowHandle">The handle of the window whose associated taskbar button is being used as a progress indicator.
            /// This window belong to a calling process associated with the button's application and must be already loaded.</param>
            /// <param name="state">Progress state of the progress button</param>
            public static void SetProgressState(TaskbarProgressBarState state, IntPtr windowHandle)
            {
                if (IsPlatformSupported)
                {
                    TaskbarList.Instance.SetProgressState(windowHandle, (TaskbarProgressBarStatus)state);
                }
            }
        }


        /// <summary>
        /// Represents the thumbnail progress bar state.
        /// </summary>
        public enum TaskbarProgressBarState
        {
            /// <summary>
            /// No progress is displayed.
            /// </summary>
            NoProgress = 0,

            /// <summary>
            /// The progress is indeterminate (marquee).
            /// </summary>
            Indeterminate = 0x1,

            /// <summary>
            /// Normal progress is displayed.
            /// </summary>
            Normal = 0x2,

            /// <summary>
            /// An error occurred (red).
            /// </summary>
            Error = 0x4,

            /// <summary>
            /// The operation is paused (yellow).
            /// </summary>
            Paused = 0x8
        }

        /// <summary>
        /// Provides internal access to the functions provided by the ITaskbarList4 interface,
        /// without being forced to refer to it through another singleton.
        /// </summary>
        internal static class TaskbarList
        {
            private static readonly object _syncLock = new();

            private static ITaskbarList4? _taskbarList;
            internal static ITaskbarList4 Instance
            {
                get
                {
                    if (_taskbarList == null)
                    {
                        lock (_syncLock)
                        {
                            if (_taskbarList == null)
                            {
                                _taskbarList = (ITaskbarList4)new CTaskbarList();
                                _taskbarList.HrInit();
                            }
                        }
                    }

                    return _taskbarList;
                }
            }
        }

        [Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
        [ClassInterface(ClassInterfaceType.None)]
        [ComImport()]
        internal class CTaskbarList { }


        [ComImport()]
        [Guid("c43dc798-95d1-4bea-9030-bb99e2983a1a")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface ITaskbarList4
        {
            // ITaskbarList
            [PreserveSig]
            void HrInit();
            [PreserveSig]
            void AddTab(IntPtr hwnd);
            [PreserveSig]
            void DeleteTab(IntPtr hwnd);
            [PreserveSig]
            void ActivateTab(IntPtr hwnd);
            [PreserveSig]
            void SetActiveAlt(IntPtr hwnd);

            // ITaskbarList2
            [PreserveSig]
            void MarkFullscreenWindow(
                IntPtr hwnd,
                [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

            // ITaskbarList3
            [PreserveSig]
            void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);
            [PreserveSig]
            void SetProgressState(IntPtr hwnd, TaskbarProgressBarStatus tbpFlags);
            [PreserveSig]
            void RegisterTab(IntPtr hwndTab, IntPtr hwndMDI);
            [PreserveSig]
            void UnregisterTab(IntPtr hwndTab);
            [PreserveSig]
            void SetTabOrder(IntPtr hwndTab, IntPtr hwndInsertBefore);
            [PreserveSig]
            void SetTabActive(IntPtr hwndTab, IntPtr hwndInsertBefore, uint dwReserved);
            [PreserveSig]
            HResult ThumbBarAddButtons(
                IntPtr hwnd,
                uint cButtons,
                [MarshalAs(UnmanagedType.LPArray)] ThumbButton[] pButtons);
            [PreserveSig]
            HResult ThumbBarUpdateButtons(
                IntPtr hwnd,
                uint cButtons,
                [MarshalAs(UnmanagedType.LPArray)] ThumbButton[] pButtons);
            [PreserveSig]
            void ThumbBarSetImageList(IntPtr hwnd, IntPtr himl);
            [PreserveSig]
            void SetOverlayIcon(
              IntPtr hwnd,
              IntPtr hIcon,
              [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
            [PreserveSig]
            void SetThumbnailTooltip(
                IntPtr hwnd,
                [MarshalAs(UnmanagedType.LPWStr)] string pszTip);
            [PreserveSig]
            void SetThumbnailClip(
                IntPtr hwnd,
                IntPtr prcClip);

            // ITaskbarList4
            void SetTabProperties(IntPtr hwndTab, SetTabPropertiesOption stpFlags);
        }

        internal enum TaskbarProgressBarStatus
        {
            NoProgress = 0,
            Indeterminate = 0x1,
            Normal = 0x2,
            Error = 0x4,
            Paused = 0x8
        }

        internal enum ThumbButtonMask
        {
            Bitmap = 0x1,
            Icon = 0x2,
            Tooltip = 0x4,
            THB_FLAGS = 0x8
        }

        [Flags]
        internal enum ThumbButtonOptions
        {
            Enabled = 0x00000000,
            Disabled = 0x00000001,
            DismissOnClick = 0x00000002,
            NoBackground = 0x00000004,
            Hidden = 0x00000008,
            NonInteractive = 0x00000010
        }

        internal enum SetTabPropertiesOption
        {
            None = 0x0,
            UseAppThumbnailAlways = 0x1,
            UseAppThumbnailWhenActive = 0x2,
            UseAppPeekAlways = 0x4,
            UseAppPeekWhenActive = 0x8
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal struct ThumbButton
        {
            /// <summary>
            /// WPARAM value for a THUMBBUTTON being clicked.
            /// </summary>
            internal const int Clicked = 0x1800;

            [MarshalAs(UnmanagedType.U4)]
            internal ThumbButtonMask Mask;
            internal uint Id;
            internal uint Bitmap;
            internal IntPtr Icon;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            internal string Tip;
            [MarshalAs(UnmanagedType.U4)]
            internal ThumbButtonOptions Flags;
        }

        /// <summary>
        /// HRESULT Wrapper
        /// </summary>
        public enum HResult
        {
            /// <summary>
            /// S_OK
            /// </summary>
            Ok = 0x0000,

            /// <summary>
            /// S_FALSE
            /// </summary>
            False = 0x0001,

            /// <summary>
            /// E_INVALIDARG
            /// </summary>
            InvalidArguments = unchecked((int)0x80070057),

            /// <summary>
            /// E_OUTOFMEMORY
            /// </summary>
            OutOfMemory = unchecked((int)0x8007000E),

            /// <summary>
            /// E_NOINTERFACE
            /// </summary>
            NoInterface = unchecked((int)0x80004002),

            /// <summary>
            /// E_FAIL
            /// </summary>
            Fail = unchecked((int)0x80004005),

            /// <summary>
            /// E_ELEMENTNOTFOUND
            /// </summary>
            ElementNotFound = unchecked((int)0x80070490),

            /// <summary>
            /// TYPE_E_ELEMENTNOTFOUND
            /// </summary>
            TypeElementNotFound = unchecked((int)0x8002802B),

            /// <summary>
            /// NO_OBJECT
            /// </summary>
            NoObject = unchecked((int)0x800401E5),

            /// <summary>
            /// Win32 Error code: ERROR_CANCELLED
            /// </summary>
            Win32ErrorCanceled = 1223,

            /// <summary>
            /// ERROR_CANCELLED
            /// </summary>
            Canceled = unchecked((int)0x800704C7),

            /// <summary>
            /// The requested resource is in use
            /// </summary>
            ResourceInUse = unchecked((int)0x800700AA),

            /// <summary>
            /// The requested resources is read-only.
            /// </summary>
            AccessDenied = unchecked((int)0x80030005)
        }
    }
}