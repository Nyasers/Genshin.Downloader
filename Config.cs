using System.Configuration;

namespace Genshin.Downloader
{
    internal static class Config
    {

        private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        private static readonly KeyValueConfigurationCollection settings = config.AppSettings.Settings;

        public static string? GetValue(string key)
        {
            return settings.AllKeys.Contains(key) ? settings[key].Value : null;
        }

        public static void SetValue(string key, string? value = null)
        {
            if (settings.AllKeys.Contains(key))
            {
                settings.Remove(key);
            }
            if (!string.IsNullOrEmpty(value))
            {
                settings.Add(key, value);
            }
            config.Save(ConfigurationSaveMode.Modified);
        }
    }
}