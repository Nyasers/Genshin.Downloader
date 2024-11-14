namespace Helper;

internal class FileInfoH : IDisposable
{
    public string remoteName;
    public string? md5;
    public string? hash;
    public long size;

    private string? path;
    private byte[]? data;
    private Stream? stream;

    private bool disposedValue;

    public FileInfoH(dynamic json)
    {
        remoteName = (string)json.remoteName;
        md5 = (string)json.md5;
        hash = (string)json.hash;
        size = (long)json.fileSize;
    }

    public FileInfoH(string file)
    {
        if (File.Exists(file) is false) throw new FileNotFoundException("File does not exsit.", file);
        path = file;
        remoteName = GetRemoteName(file);
        size = new FileInfo(file).Length;
    }

    public async Task ComputeMD5Async(CancellationToken token = default)
    {
        if (path is null) return;
        md5 = null;
        stream ??= File.OpenRead(path);
        foreach (byte item in await System.Security.Cryptography.MD5.HashDataAsync(stream, token))
        {
            md5 += item.ToString("x2");
        }
    }

    public async Task ComputeMD5()
    {
        if (path is null) return;
        md5 = null;
        data ??= await File.ReadAllBytesAsync(path);
        foreach (byte item in System.Security.Cryptography.MD5.HashData(data))
        {
            md5 += item.ToString("x2");
        }
    }

    public async Task ComputeHash()
    {
        if (path is null) return;
        hash = null;
        data ??= await File.ReadAllBytesAsync(path);
        foreach (byte item in System.IO.Hashing.XxHash64.Hash(data))
        {
            hash += item.ToString("x2");
        }
    }

    public async Task ComputeAll()
    {
        await ComputeMD5();
        await ComputeHash();
    }

    public static string GetRemoteName(string fileName)
    {
        return fileName[(Properties.Settings.Default.GamePath.Length + 1)..].Replace('\\', '/');
    }

    public override string ToString()
    {
        string result = $"{{\"remoteName\": \"{remoteName}\", ";
        if (md5 is not null) result += $"\"md5\": \"{md5}\", ";
        result += $"\"size\": {size}}}";
        return result;
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
        if (size != ((FileInfoH)obj).size)
            return false;
        else if (hash is not null && ((FileInfoH)obj).hash is not null)
            if (hash != ((FileInfoH)obj).hash)
                return false;
        if (md5 is not null && ((FileInfoH)obj).md5 is not null)
            if (md5 != ((FileInfoH)obj).md5)
                return false;
        return true;
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: 释放托管状态(托管对象)
                stream?.Dispose();
            }

            // TODO: 释放未托管的资源(未托管的对象)并重写终结器
            // TODO: 将大型字段设置为 null
            path = null;
            data = null;
            stream = null;
            disposedValue = true;
        }
    }

    // TODO: 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~FileInfoH()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
