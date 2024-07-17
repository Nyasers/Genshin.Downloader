using System.Diagnostics;

namespace Helper
{
    internal static class Aria2
    {
        public static List<string> LogLevels =
            [
                "error", "warn", "notice", "info", "debug"
            ];
        public static List<string> Errors = [
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
            ];

        public static async Task<int> DownloadAsync(string input, string path, int console_log_level = 1)
        {
            string download_info = await GetInputFileAsync(input);

            string[] args = [
                $"-R",
                $"--console-log-level={LogLevels[console_log_level]}",
                $"--input-file=\"{download_info}\""
            ];

            ProcessStartInfo startInfo = new()
            {
                FileName = "aria2c.exe",
                Arguments = string.Join(' ', args),
                WorkingDirectory = path,
                ErrorDialog = true,
            };

            return await Resource.ProcessStartAsync(startInfo);
        }

        public static string GetMessage(int exitCode)
        {
            string error = "Unknown error.";
            if (exitCode is >= 0 and <= 32)
            {
                error = Errors[exitCode];
            }
            return error;
        }

        public static string GetInput(File2Down[] files)
        {
            long size_total = 0;
            string origin_input = $"#Created by Genshin.Downloader at {DateTime.Now.ToLocalTime()}\n\n";
            foreach (File2Down file in files)
            {
                string? outPath = string.IsNullOrEmpty(file.remoteName) ? file.name : file.remoteName;
                origin_input += $"#{file}\n" +
                $"{file.path}\n" +
                $" out={outPath}\n" +
                $" checksum=md5={file.md5}\n\n";
                size_total += file.size;
            }
            origin_input += $"#Count: {files.Length} file(s), {FileH.ParseSize(size_total)} in total.\n";
            return origin_input;
        }

        private static async Task<string> GetInputFileAsync(string aria2input)
        {
            if (string.IsNullOrWhiteSpace(aria2input)) throw new ArgumentNullException(nameof(aria2input));
            string file_input = Path.GetTempFileName();
            await File.WriteAllTextAsync(file_input, aria2input);
            return file_input;
        }
    }
}