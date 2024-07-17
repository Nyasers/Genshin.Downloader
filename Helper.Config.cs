using Microsoft.Extensions.Configuration;

namespace Helper;

public partial class Config
{
    private readonly string file;
    private readonly IConfigurationRoot config;
    private readonly IConfigurationSection section;
    public Config(string path, string key = "General")
    {
        ArgumentNullException.ThrowIfNull(path);
        file = path + "\\config.ini";
        if (!File.Exists(file)) File.Create(file).Close();
        config = new ConfigurationBuilder()
            .AddIniFile(file)
            .Build();
        section = config.GetSection(key);
    }

    public void Reload() => config.Reload();

    public async void Save()
    {
        await File.WriteAllTextAsync(file, ToString());
        Reload();
    }

    public override string ToString()
    {
        string result = "";
        foreach (var item in config.AsEnumerable())
        {
            if (item.Value is null && !item.Key.Contains(':'))
            {
                result += $"[{item.Key}]\r\n";
            }
            else if (item.Value is not null && item.Key.Contains(':'))
            {
                result += $"{item.Key[(item.Key.IndexOf(':') + 1)..]}={item.Value}\r\n";
            }
        }
        return result;
    }

    public string? Version
    {
        get { return section["game_version"]; }
        set { section["game_version"] = value; }
    }

    public string? Channel
    {
        get
        {
            string? channel = section["channel"], sub_channel = section["sub_channel"];
            if (channel != null && sub_channel != null)
            {
                return $"hk4e_{channel}_{sub_channel}";
            }
            else
            {
                return null;
            }
        }
        set
        {
            string? channel = value?[5..^2];
            string? sub_channel = value?[^1..];
            if (channel != null && sub_channel != null)
            {
                section["channel"] = channel;
                section["sub_channel"] = sub_channel;
            }
        }
    }
}
