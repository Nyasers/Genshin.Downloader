namespace Helper;

public partial class DirectoryH
{
    public static DirectoryInfo EnsureExists(string path)
    {
        return Directory.Exists(path) ? new DirectoryInfo(path) : Directory.CreateDirectory(path);
    }

    public static DirectoryInfo EnsureNew(string path)
    {
        if (Directory.Exists(path))
        {
            try
            {
                Directory.Delete(new DirectoryInfo(path).FullName, true);
            }
            catch (IOException)
            {
                throw;
            }
        }

        return EnsureExists(path);
    }
}
