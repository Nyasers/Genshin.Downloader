using Newtonsoft.Json;
using System.Resources;

namespace Helper;

internal class API
{
    private static readonly ResourceManager resource = new(typeof(API));

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
            "https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGamePackages?launcher_id=jGHBHlcOq1&game_ids[]=1Z8W5NHUQb"
        },
        {
            "hk4e_1_1",
            "https://sg-hyp-api.hoyoverse.com/hyp/hyp-connect/api/getGamePackages?launcher_id=VYTpXlbWo8&game_ids[]=gopR6Cufr3"
        },
        {
            "hk4e_14_0",
            "https://hyp-api.mihoyo.com/hyp/hyp-connect/api/getGamePackages?launcher_id=umfgRO5gh5&game_ids[]=T2S0Gz4Dr2"
        }
    };

    public static Dictionary<string, string> ApiList => apiList;

    public static Dictionary<string, string> AudioList => audioList;

    public async static Task<dynamic> GetAsync(string? channel)
    {
        if (channel is not null && ApiList.TryGetValue(channel, out string? api) && api is not null)
        {
            using HttpClient http = new();
            string value = await http.GetStringAsync(api);
            dynamic ret = JsonConvert.DeserializeObject<dynamic>(value) ?? throw new Exception();
            if ((int?)ret.retcode is not 0) throw new Exception(ret.message);
            dynamic res = ret.data.game_packages[0] ?? throw new Exception();
            return res;
        }
        else
        {
            throw new ArgumentOutOfRangeException(nameof(channel), channel, resource.GetString("msg.error.notfound"));
        }
    }
}
