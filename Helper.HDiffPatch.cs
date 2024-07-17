using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Helper;

internal static class HDiffPatch
{
    public static async Task<int> ApplyDelete(string path_game, string path_temp)
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
                string command_line = $"del /f \"{path_game}\\{line.Replace("/", "\\")}\"||pause";
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

    public static async Task<int> ApplyHDiff(string path_game, string path_temp)
    {
        string hdiff_file = $"{path_temp}\\hdifffiles.txt";
        string batch_file = $"{path_temp}\\hdifffiles.bat";
        string hpatchz = await Resource.GetTempFileAsync("hpatchz.exe");
        StreamReader reader = new(hdiff_file);
        StreamWriter writer = new(batch_file, false, new UTF8Encoding(false));
        await writer.WriteLineAsync("@echo off");
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
                    string command_line = $"(\"{hpatchz}\" -C-all \"{oldFile}\" \"{diffFile}\" \"{newFile}\"||(pause&exit -1))&&del /f \"{diffFile}\"";
                    await writer.WriteLineAsync(command_line);
                }
            }
        }
        reader.Close();
        await writer.WriteLineAsync($"del /f \"{hdiff_file}\"");
        await writer.WriteLineAsync($"del /f \"{hpatchz}\"");
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

    public static async Task HPatchAsync(Form? owner = null)
    {
        int exitCode = -1;
        string path_game = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        if (File.Exists($"{path_temp}\\deletefiles.txt") && 0 != (exitCode = await ApplyDelete(path_game, path_temp)))
        {
            if (DialogResult.Retry == MessageBox.Show(owner, $"错误代码：{exitCode}", "任务未正常完成", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
            {
                await HPatchAsync(owner);
            }
            return;
        }
        if (File.Exists($"{path_temp}\\hdifffiles.txt") && 0 != (await ApplyHDiff(path_game, path_temp)))
        {
            if (DialogResult.Retry == MessageBox.Show(owner, $"错误代码：{exitCode}", "任务未正常完成", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
            {
                await HPatchAsync(owner);
            }
            return;
        }
    }
}