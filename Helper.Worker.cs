using System.Diagnostics;
using System.Text.Json.Nodes;

namespace Helper;

internal static class Worker
{
    public static async Task ApplyDownload(string channel)
    {
        string url = (await API.Get(channel)).data.game.latest.decompressed_path;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        string download_file = $"{path_temp}\\downloadfiles.txt";
        List<File2Down> files = [];
        foreach (string line in await File.ReadAllLinesAsync(download_file))
        {
            if (!string.IsNullOrWhiteSpace(line))
            {
                JsonNode? json = JsonNode.Parse(line);
                if (json != null)
                {
                    string remoteName = (string?)json["remoteName"] ?? string.Empty;
                    string name = FileH.GetName(remoteName);
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
        string input = Aria2.GetInput([.. files]);
        int exitCode = await Aria2.Download(input, DirectoryH.EnsureExists(Properties.Settings.Default.DownPath).FullName, 0);
        if (exitCode is not 0)
        {
            throw new Exception($"aria2c.exe exited with exception code {exitCode}.");
        }
        File.Delete(download_file);
    }

    public static async Task ApplyUpdate(Form? owner = null, string? version = null)
    {
        string path_game = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        string path_temp = DirectoryH.EnsureExists(Properties.Settings.Default.TempPath).FullName;
        string command_line = $"(xcopy /f /e /y \"{path_temp}\" \"{path_game}\" && del /s /q \"{path_temp}\\*\" && rd /s /q \"{path_temp}\" && exit 0) || pause";
        Process? process = Process.Start(new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = $"/c title 正在应用更新.. & {command_line}"
        });
        if (process is not null)
        {
            await process.WaitForExitAsync();
        }
        if (process?.ExitCode is not 0)
        {
            if (version is not null)
            {
                new Config(path_game).Version = version;
            }
        }
        else if (DialogResult.Retry == MessageBox.Show(owner, $"错误代码：{process?.ExitCode}", "任务未正常完成", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
        {
            await ApplyUpdate(owner, version);
        }
    }
}