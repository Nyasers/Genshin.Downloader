namespace Genshin.Downloader
{
    internal class INI
    {
        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [System.Runtime.InteropServices.DllImport("kernel32", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int GetPrivateProfileString(string section, string key, string def, System.Text.StringBuilder retVal, int size, string filePath);


        public static void Write(string section, string key, string value, string path)
        {
            _ = WritePrivateProfileString(section, key, value, path);
        }

        public static string Read(string section, string key, string path)
        {
            System.Text.StringBuilder temp = new(255);

            _ = GetPrivateProfileString(section, key, "", temp, 255, path);
            return temp.ToString();

        }

        public static void Delete(string FilePath)
        {
            File.Delete(FilePath);
        }
    }
}