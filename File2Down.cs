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

        public async Task<File2Down> BuildAsync(HttpClient client, string requestUri)
        {
            try
            {
                name = await client.GetStringAsync(requestUri + "/name");
                path = await client.GetStringAsync(requestUri + "/path");
                size = long.Parse(await client.GetStringAsync(requestUri + "/size"));
                md5 = await client.GetStringAsync(requestUri + "/md5");
                name = name == "" ? GetName(path) : name;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            return this;
        }

        private static string GetName(string path)
        {
            string[] s = path.Split("/");
            string name = s[^1];
            return name;
        }

        private string GetFileSize()
        {
            double num = 1024.00;
            long size = this.size / 2;
            return size < num
                ? size + "Byte"
                : size < Math.Pow(num, 2)
                ? (size / num).ToString("f2") + "KB"
                : size < Math.Pow(num, 3)
                ? (size / Math.Pow(num, 2)).ToString("f2") + "MB"
                : size < Math.Pow(num, 4)
                ? (size / Math.Pow(num, 3)).ToString("f2") + "GB"
                : (size / Math.Pow(num, 4)).ToString("f2") + "TB";
        }

        public override string ToString()
        {
            return $"{name} ({GetFileSize()})";
        }
    }
}