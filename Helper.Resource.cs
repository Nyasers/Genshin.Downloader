using System.Diagnostics;
using System.Reflection;
using System.Resources;

namespace Helper
{
    internal static class Resource
    {
        private const string RootNamespace = "Genshin.Downloader";

        public static async Task<byte[]> GetBytesAsync(string name)
        {
            string resourceName = RootNamespace + "." + name;
            Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName) ?? throw new FileLoadException(resourceName);
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer);
            stream.Close();
            return buffer;
        }

        public static async Task<string> GetTempFileAsync(string name)
        {
            string path = Path.GetTempFileName();
            byte[] bytes = await GetBytesAsync(name);
            await File.WriteAllBytesAsync(path, bytes);
            return path;
        }

        public static async Task<int> ProcessStartAsync(ProcessStartInfo info)
        {
            info.FileName = await GetTempFileAsync(info.FileName);
            var process = Process.Start(info);
            if (process is null)
            {
                File.Delete(info.FileName);
                return -1;
            }
            else
            {
                process.Exited += (object? sender, EventArgs e) => File.Delete(info.FileName);
                await process.WaitForExitAsync();
                return process.ExitCode;
            }
        }

        public static void MemoryManager(Form? form = null, ResourceManager? resourceManager = null)
        {
            const int MB = 1024 * 1024;
            long privateMemorySize64 = Process.GetCurrentProcess().PrivateMemorySize64;
            if (privateMemorySize64 >= MB * 128)
            {
                GC.Collect(2,
                    privateMemorySize64 >= MB * 256 ?
                    privateMemorySize64 >= MB * 1024 ?
                    GCCollectionMode.Aggressive : GCCollectionMode.Forced : GCCollectionMode.Optimized,
                    privateMemorySize64 >= MB * 512,
                    privateMemorySize64 >= MB * 512);
                GC.WaitForFullGCComplete();
            }
            if (form is not null && resourceManager is not null)
                form.Text = $"{resourceManager.GetString("$this.Text")} ({privateMemorySize64 / MB} MB)";
        }

        /*public static async Task<MethodInfo> GetMethod(string name)
        {
            try
            {
                byte[] buffer = await GetBytes(name);
                Assembly assembly = Assembly.Load(buffer);
                MethodInfo pointInfo = assembly.EntryPoint ?? throw new FileLoadException("Failed to get EntryPoint.", name);
                return pointInfo;
            }
            catch
            {
                throw;
            }
        }*/
    }
}