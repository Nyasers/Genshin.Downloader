using System.Configuration;
using System.Diagnostics;

namespace Genshin.Downloader
{
    namespace Helpers
    {
        internal static class String
        {
            public static string? EmptyCheck(string? left)
            {
                return !string.IsNullOrEmpty(left) ? left : null;
            }
        }

        internal static class File
        {
            public static string GetName(string path)
            {
                return path.Split("/")[^1];
            }

            public static async Task<long> GetSizeAsync(string path)
            {
                return (await new HttpClient().GetAsync(path, HttpCompletionOption.ResponseHeadersRead)).Content.Headers.ContentLength ?? 0;
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

            public static string ParseSize(long size)
            {
                return Parse(1024, new string[] { "Byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB" }, size);
            }
        }

        internal static class Process
        {
            public static async Task<int> Run(string fileName, string arguments, string workingDirectory, DataReceivedEventHandler? onOutputDataReceived = null, DataReceivedEventHandler? onErrorDataReceived = null)
            {
                ProcessStartInfo startInfo = new()
                {
                    FileName = fileName,
                    Arguments = arguments,
                    WorkingDirectory = workingDirectory,
                    UseShellExecute = false,
                    ErrorDialog = true,
                };
                if (onOutputDataReceived is not null)
                {
                    startInfo.RedirectStandardOutput = true;
                }
                if (onErrorDataReceived is not null)
                {
                    startInfo.RedirectStandardError = true;
                }
                if (startInfo.RedirectStandardOutput & startInfo.RedirectStandardError)
                {
                    startInfo.CreateNoWindow = true;
                }
                System.Diagnostics.Process? process = System.Diagnostics.Process.Start(startInfo);
                if (process is not null)
                {
                    if (startInfo.RedirectStandardOutput)
                    {
                        process.OutputDataReceived += onOutputDataReceived;
                        process.BeginOutputReadLine();
                    }
                    if (startInfo.RedirectStandardError)
                    {
                        process.ErrorDataReceived += onErrorDataReceived;
                        process.BeginErrorReadLine();
                    }
                    await process.WaitForExitAsync();
                    return process.ExitCode;
                }
                return -1;
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
                System.IO.File.Delete(FilePath);
            }
        }

        internal static class Config
        {
            public static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            public static readonly KeyValueConfigurationCollection settings = config.AppSettings.Settings;

            public static string? GetValue(string key)
            {
                return settings.AllKeys.Contains(key) ? settings[key].Value : null;
            }

            public static void SetValue(string key, string? value = null)
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