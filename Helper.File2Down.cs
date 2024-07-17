namespace Helper
{
    internal partial class File2Down
    {
        public string name;
        public string path;
        public string remoteName;
        public long size;
        public string md5;

        public string ParsedSize => FileH.ParseSize(size);

        public File2Down()
        {
            name = "";
            remoteName = "";
            path = "";
            size = 0;
            md5 = "";
        }

        public async static Task<File2Down?> BuildAsync(dynamic data)
        {
            File2Down result = new();
            try
            {
                result.path = data.path ?? string.Empty;
                if (string.IsNullOrEmpty(result.path))
                {
                    throw new();
                }
                try
                {
                    result.name = data.name ?? string.Empty;
                }
                catch { }
                result.name = StringH.EmptyCheck(result.name) ?? FileH.GetName(result.path);
                result.size = long.Parse((string)data.package_size ?? "0");
                result.size = result.size == 0 ? await FileH.GetSizeAsync(result.path) : result.size;
                result.md5 = data.md5 ?? string.Empty;
            }
            catch
            {
                return null;
            }
            return result;
        }

        public override string ToString()
        {
            return $"{name} ({ParsedSize})";
        }
    }
}
