using Newtonsoft.Json;

namespace Helper;

internal class API
{
    private static readonly Dictionary<string, string> audioList = new()
    {
        {
            "zh-cn",
            "Audio_Chinese"
        },
        {
            "en-us",
            "Audio_English(US)"
        },
        {
            "ja-jp",
            "Audio_Japanese"
        },
        {
            "ko-kr",
            "Audio_Korean"
        }
    };

    private static readonly Dictionary<string, string> apiList = new()
    {
        {
            "hk4e_1_0",
            "https://sdk-static.mihoyo.com/hk4e_cn/mdk/launcher/api/resource?launcher_id=18&key=eYd89JmJ&channel_id=1&sub_channel_id=1"
        },
        {
            "hk4e_1_1",
            "https://hk4e-launcher-static.hoyoverse.com/hk4e_global/mdk/launcher/api/resource?launcher_id=10&key=gcStgarh&channel_id=1&sub_channel_id=3"
        },
        {
            "hk4e_14_0",
            "https://sdk-static.mihoyo.com/hk4e_cn/mdk/launcher/api/resource?launcher_id=17&key=KAtdSsoQ&channel_id=14&sub_channel_id=0"
        }
    };

    public static Dictionary<string, string> ApiList => apiList;

    public static Dictionary<string, string> AudioList => audioList;

    public async static Task<dynamic> Get(string? channel)
    {
        if (channel is not null && ApiList.TryGetValue(channel, out string? api) && api is not null)
        {
            using HttpClient http = new();
            string data = await http.GetStringAsync(api);
            return JsonConvert.DeserializeObject<dynamic>(data) ?? throw new Exception();
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(channel), channel, "Not Found.");
        }
    }

    public static async Task<dynamic> GetLatest(string? channel)
    {
        return (await Get(channel)).data.game.latest;
    }

    public static async Task<string> GetLatestVersion(string? channel)
    {
        return (await GetLatest(channel)).version;
    }

    public static async Task<string> GetDecompressedPath(string? channel)
    {
        return (await GetLatest(channel)).decompressed_path;
    }
}
