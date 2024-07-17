namespace Helper;

public partial class DirectoryH
{
    public static DirectoryInfo EnsureExists(string path) => Directory.Exists(path) ? new DirectoryInfo(path) : Directory.CreateDirectory(path);
    public static DirectoryInfo EnsureNew(string path)
    {
        if (Directory.Exists(path)) Directory.Delete(path, true);
        return EnsureExists(path);
    }
}
