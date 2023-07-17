using System.Configuration;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Genshin.Downloader
{
    namespace Helpers
    {
        internal static class StringH
        {
            public static string? EmptyCheck(string? left)
            {
                return !string.IsNullOrEmpty(left) ? left : null;
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

            public static async Task HPatchAsync(string path_game, string path_hdiff)
            {
                if (File.Exists($"{path_hdiff}\\deletefiles.txt"))
                {
                    string delete_file = $"{path_hdiff}\\deletefiles.txt";
                    string batch_file = $"{path_hdiff}\\deletefiles.bat";
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
                    using Process? process = Process.Start(new ProcessStartInfo()
                    {
                        FileName = "cmd.exe",
                        Arguments = $"/c \"{batch_file}\""
                    });
                    if (process is not null)
                    {
                        await process.WaitForExitAsync();
                    }
                }
                if (File.Exists($"{path_hdiff}\\hdifffiles.txt"))
                {
                    string hdiff_file = $"{path_hdiff}\\hdifffiles.txt";
                    string batch_file = $"{path_hdiff}\\hdifffiles.bat";
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
                                string diffFile = $"{path_hdiff}\\{remoteName}.hdiff";
                                string newFile = $"{path_hdiff}\\{remoteName}";
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
            }

            public static async Task UnzipAsync(string @in, string @out)
            {
                string seven_zip = $"{Directory.GetCurrentDirectory()}\\7za.exe";
                Process? process = Process.Start(new ProcessStartInfo()
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
        }

        internal static class DirectoryH
        {
            public static DirectoryInfo EnsureExists(string path)
            {
                if (Directory.Exists(path))
                {
                    return new DirectoryInfo(path);
                }
                return Directory.CreateDirectory(path);
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

        internal class INI
        {
            [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

            [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
            private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


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
            public static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            public static readonly KeyValueConfigurationCollection settings = config.AppSettings.Settings;

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
    }
}