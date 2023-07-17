using System.Net.Http.Headers;
using System.Text.Json.Nodes;

namespace Genshin.Downloader
{
    internal partial class File2Down
    {
        public string name;
        public string path;
        public long size;
        public string md5;

        public File2Down()
        {
            name = "";
            path = "";
            size = 0;
            md5 = "";
        }

        public async Task<File2Down?> BuildAsync(HttpClient client, string requestUri)
        {
            try
            {
                var res = await client.GetStringAsync(requestUri);
                JsonNode? data = JsonNode.Parse(res);
                path = (string?)data?["path"] ?? string.Empty;
                if (string.IsNullOrEmpty(path))
                {
                    throw new();
                }
                name = (string?)data?["name"] ?? string.Empty;
                name = string.IsNullOrEmpty(name) ? GetName(path) : name;
                size = long.Parse((string?)data?["package_size"] ?? "0");
                size = size == 0 ? await GetSizeAsync(path) : size;
                md5 = (string?)data?["md5"] ?? string.Empty;
            }
            catch
            {
                return null;
            }
            return this;
        }

        public static async Task<long> GetSizeAsync(string path)
        {
            using HttpClient client = new();
            return (await client.GetAsync(path, HttpCompletionOption.ResponseHeadersRead)).Content.Headers.ContentLength ?? 0;
        }

        public static string GetName(string path)
        {
            string[] s = path.Split("/");
            string name = s[^1];
            return name;
        }

        public static string GetFileSize(long size)
        {
            double num = 1024.00;
            return size < num
                ? size + " Byte"
                : size < Math.Pow(num, 2)
                ? (size / num).ToString("f2") + " KB"
                : size < Math.Pow(num, 3)
                ? (size / Math.Pow(num, 2)).ToString("f2") + " MB"
                : size < Math.Pow(num, 4)
                ? (size / Math.Pow(num, 3)).ToString("f2") + " GB"
                : (size / Math.Pow(num, 4)).ToString("f2") + " TB";
        }

        public override string ToString()
        {
            return $"{name} ({GetFileSize(size)})";
        }
    }
}