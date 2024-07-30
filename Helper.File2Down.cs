namespace Helper
{
    internal partial class File2Down
    {
        public string name;
        public string url;
        public string remoteName;
        public long size;
        public string md5;

        public string ParsedSize => FileH.ParseSize(size);

        public File2Down()
        {
            name = "";
            remoteName = "";
            url = "";
            size = 0;
            md5 = "";
        }

        public async static Task<File2Down?> BuildAsync(dynamic pkg)
        {
            File2Down res = new();
            try
            {
                res.url = pkg.url ?? string.Empty;
                if (string.IsNullOrEmpty(res.url))
                {
                    throw new ArgumentNullException(nameof(pkg));
                }
                res.name = FileH.GetName(res.url);
                res.md5 = pkg.md5 ?? string.Empty;
                res.size = long.Parse((string)pkg.size ?? "0");
                res.size = res.size == 0 ? await FileH.GetSizeAsync(res.url) : res.size;
                res.remoteName = pkg.remoteName ?? string.Empty;
            }
            catch
            {
                return null;
            }
            return res;
        }

        public override string ToString()
        {
            return $"{name} ({ParsedSize})";
        }
    }
}
