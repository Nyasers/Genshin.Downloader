
namespace Helper;

internal class FileInfoH
{
    public string remoteName;
    public string? md5;
    public string? hash;
    public long fileSize;

    public FileInfoH(dynamic json)
    {
        remoteName = (string)json.remoteName;
        md5 = (string)json.md5;
        hash = (string)json.hash;
        fileSize = (long)json.fileSize;
    }

    public FileInfoH(FileInfo file, bool hash = false, bool md5 = false)
    {
        if (File.Exists(file.FullName) is false) throw new FileNotFoundException("File does not exsit.", file.FullName);
        remoteName = GetRemoteName(file.FullName);
        fileSize = file.Length;
        if (hash) foreach (byte item in System.IO.Hashing.XxHash64.Hash(File.ReadAllBytes(file.FullName)))
            {
                this.hash += item.ToString("x2");
            }
        if (md5) foreach (byte item in System.Security.Cryptography.MD5.HashData(File.OpenRead(file.FullName)))
            {
                this.md5 += item.ToString("x2");
            }
    }

    public static async Task<FileInfoH> BuildAsync(FileInfo file, bool hash = false, bool md5 = false)
    {
        FileInfoH result = new(file);
        if (hash)
        {
            byte[] _hash = System.IO.Hashing.XxHash64.Hash(await File.ReadAllBytesAsync(file.FullName));
            foreach (byte item in _hash)
            {
                result.hash += item.ToString("x2");
            }
        }

        if (md5)
        {
            byte[] _md5 = await System.Security.Cryptography.MD5.HashDataAsync(File.OpenRead(file.FullName));
            foreach (byte item in _md5)
            {
                result.md5 += item.ToString("x2");
            }
        }
        return result;
    }

    public static string GetRemoteName(string fileName)
    {
        return fileName[(Properties.Settings.Default.GamePath.Length + 1)..].Replace('\\', '/');
    }

    public override string ToString()
    {
        return $@"{{""remoteName"": ""{remoteName}"", ""md5"": ""{md5 ?? ""}"", ""hash"": ""{hash ?? ""}"", ""fileSize"": {fileSize}}}";
    }

    public override int GetHashCode()
    {
        return remoteName.GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj is not FileInfoH)
            return false;
        if (remoteName != ((FileInfoH)obj).remoteName)
            return false;
        if (fileSize != ((FileInfoH)obj).fileSize)
            return false;
        else if (hash is not null && ((FileInfoH)obj).hash is not null)
            if (hash != ((FileInfoH)obj).hash)
                return false;
        if (md5 is not null && ((FileInfoH)obj).md5 is not null)
            if (md5 != ((FileInfoH)obj).md5)
                return false;
        return true;
    }
}
