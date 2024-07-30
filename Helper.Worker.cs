using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper;

internal static class Worker
{
    public static async Task<int> ApplyDelete()
    {
        string path_game = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        string delete_file = $"{path_temp}\\deletefiles.txt";
        string batch_file = Path.GetTempFileName();
        StreamReader reader = new(delete_file);
        StreamWriter writer = new(batch_file, false, new UTF8Encoding(false));
        await writer.WriteLineAsync($"@echo off & title {Genshin.Downloader.Text.msg_doing_delete}");
        while (!reader.EndOfStream)
        {
            string? line = await reader.ReadLineAsync();
            if (line != null)
            {
                string command_line = $"del /f \"{path_game}\\{line.Replace("/", "\\")}\" || (pause & exit 1)";
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
            return process.ExitCode;
        }
        return -1;
    }

    public static async Task<int> ApplyDownload(string? channel)
    {
        string url = (await API.GetAsync(channel)).main.major.res_list_url;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        string download_file = $"{path_temp}\\downloadfiles.txt";
        List<File2Down> files = [];
        foreach (string line in await File.ReadAllLinesAsync(download_file))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                dynamic? json = JsonConvert.DeserializeObject(line);
                if (json != null)
                {
                    json.url = $"{url}/{json.remoteName}";
                    File2Down? file = await File2Down.BuildAsync(json);
                    if (file is not null && !string.IsNullOrEmpty(file.remoteName))
                    {
                        files.Add(file);
                    }
                }
            }
        }
        string input = Aria2.GetInput([.. files]);
        int exitCode = await Aria2.DownloadAsync(input, DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName, 0);
        if (exitCode is 0) File.Delete(download_file);
        return exitCode;
    }

    public static async Task<int> ApplyHDiff()
    {
        string path_game = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        string hdiff_file = $"{path_temp}\\hdifffiles.txt";
        string batch_file = Path.GetTempFileName();
        string hpatchz = await Resource.GetTempFileAsync("hpatchz.exe");
        StreamReader reader = new(hdiff_file);
        StreamWriter writer = new(batch_file, false, new UTF8Encoding(false));
        await writer.WriteLineAsync($"@echo off & title {Genshin.Downloader.Text.msg_doing_hpatch}");
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
                    string command_line = $"(\"{hpatchz}\" -C-all \"{oldFile}\" \"{diffFile}\" \"{newFile}\" || (pause & exit -1)) && del /f \"{diffFile}\"";
                    await writer.WriteLineAsync(command_line);
                }
            }
        }
        reader.Close();
        await writer.WriteLineAsync($"del /f \"{hpatchz}\" & del /f \"{hdiff_file}\"");
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
            return process.ExitCode;
        }
        return -1;
    }

    public static async Task ApplyUpdate(Form? owner = null, string? version = null)
    {
        string path_game = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        string command_line = $"@title {Genshin.Downloader.Text.msg_doing_update} & (xcopy /f /e /y \"{path_temp}\" \"{path_game}\" && del /s /q \"{path_temp}\\*\" && rd /s /q \"{path_temp}\" && exit 0) || (pause & exit 1)";
        Process? process = Process.Start(new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c {command_line}"
        });
        if (process is not null)
        {
            await process.WaitForExitAsync();
        }
        if (process?.ExitCode is 0)
        {
            if (version is not null)
            {
                Config config = new(path_game)
                {
                    Version = version
                };
                config.Save();
            }
        }
        else if (DialogResult.Retry == MessageBox.Show(owner, $"{Genshin.Downloader.Text.msg_failed_code}{process?.ExitCode}", Genshin.Downloader.Text.msg_failed_task, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
        {
            await ApplyUpdate(owner, version);
        }
    }

    public static async Task ApplyUnzip(Form? owner, string path)
    {
        try
        {
            string path_temp = DirectoryH.EnsureNew(Properties.Settings.Default.TempPath).FullName;
            int exitCode = await FileH.UnzipAsync(path, path_temp);
            if (exitCode is not 0)
            {
                if (DialogResult.Retry == MessageBox.Show(owner, $"{exitCode}", "7za.exe", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
                {
                    await ApplyUnzip(owner, path);
                }
            }
        }
        catch (IOException ex)
        {
            if (DialogResult.Retry == MessageBox.Show(owner, ex.Message, Genshin.Downloader.Text.msg_failed_task, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
            {
                await ApplyUnzip(owner, path);
            }
        }
    }

    public static async Task HPatchAsync(Form? owner = null, string? channel = null)
    {
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        int exitCode;
        if ((File.Exists($"{path_temp}\\downloadfiles.txt") && 0 != (exitCode = await ApplyDownload(channel)))
         || (File.Exists($"{path_temp}\\hdifffiles.txt") && 0 != (exitCode = await ApplyHDiff()))
         || (File.Exists($"{path_temp}\\deletefiles.txt") && 0 != (exitCode = await ApplyDelete())))
        {
            if (DialogResult.Retry == MessageBox.Show(owner, $"{Genshin.Downloader.Text.msg_failed_code}{exitCode}", Genshin.Downloader.Text.msg_failed_task, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
            {
                await HPatchAsync(owner, channel);
            }
            return;
        }
    }
}