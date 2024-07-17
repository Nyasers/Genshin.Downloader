using System.Diagnostics;

namespace Helper;

internal static class FileH
{
    public static string GetName(string path)
    {
        return path.Split("/")[^1];
    }

    public static string ParseSize(long size)
    {
        return Unit.Parse(1024, ["Byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB"], size);
    }

    public static async Task<long> GetSizeAsync(string path)
    {
        return (await new HttpClient().GetAsync(path, HttpCompletionOption.ResponseHeadersRead)).Content.Headers.ContentLength ?? 0;
    }

    public static async Task<int> UnzipAsync(string @in, string @out)
    {
        ProcessStartInfo info = new()
        {
            FileName = "7za.exe",
            Arguments = $"x -o\"{@out}\" \"{@in}\"",
            ErrorDialog = true
        };

        return await Resource.ProcessStartAsync(info);
    }
}
