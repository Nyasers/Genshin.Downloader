using Genshin.Downloader.Helpers;
using System.Text.Json.Nodes;

namespace Genshin.Downloader
{
    internal partial class File2Down
    {
        public string name;
        public string path;
        public long size;
        public string md5;

        public string ParsedSize => FileH.ParseSize(size);

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
                string res = await client.GetStringAsync(requestUri);
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
}