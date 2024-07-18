using Newtonsoft.Json;
using System.Resources;

namespace Genshin.Downloader;

public partial class Form_Fixer : Form
{
    private Config? Config;
    private readonly List<string> AudioList;
    private static readonly ResourceManager resourceManager = new(typeof(Form_Fixer));

    public Form_Fixer(Dictionary<string, object> args)
    {
        InitializeComponent();
        AudioList = (List<string>)args["AudioList"];
    }

    private void Form_Fixer_Load(object sender, EventArgs e)
    {
        Show();
        if (StringH.WhiteSpaceCheck(Properties.Settings.Default.GamePath) is null)
        {
            _ = MessageBox.Show(this, "未设置游戏目录！", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close(); return;
        }
        textBox_game.Text = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        Config = new Config(Properties.Settings.Default.GamePath);
        textBox_gameVersion.Text = Config.Version;
    }

    private bool CancellingCompare;

    private void Button_Compare_Click(object sender, EventArgs e)
    {
        button_cancel.Enabled = !(CancellingCompare = button_compare.Enabled = button_start.Enabled = false);
        Compare().GetAwaiter().OnCompleted(() =>
        {
            GC.Collect(2, GCCollectionMode.Aggressive, true, true);
            GC.WaitForFullGCComplete();
            button_cancel.Enabled = !(button_compare.Enabled = button_start.Enabled = true);
        });
    }

    private void Button_Cancel_Click(object sender, EventArgs e)
    {
        CancellingCompare = true;
    }

    private async Task Compare()
    {
        if (Config is null || Config.Channel is null) return;
        groupBox_suplus.Text = resourceManager.GetString("groupBox_suplus.Text"); textBox_suplus.Clear();
        groupBox_missing.Text = resourceManager.GetString("groupBox_missing.Text"); textBox_missing.Clear();
        groupBox_progress.Text = resourceManager.GetString("groupBox_progress.Text") + " (等待服务器响应)";
        progressBar.Style = ProgressBarStyle.Marquee;
        HttpClient http = new()
        {
            BaseAddress = new Uri(await API.GetDecompressedPath(Config.Channel))
        };
        Dictionary<string, string> pkg_version = [];
        pkg_version["game"] = await http.GetStringAsync($"{http.BaseAddress}/pkg_version");
        foreach (string item in AudioList)
        {
            pkg_version[item] = await http.GetStringAsync($"{http.BaseAddress}/{item}_pkg_version");
        }

        List<FileInfoH> online = [];
        online.AddRange(from KeyValuePair<string, string> p in pkg_version
                        from string i in p.Value.Split('\n')
                        where StringH.WhiteSpaceCheck(i) is not null
                        let j = JsonConvert.DeserializeObject<dynamic>(i)
                        where j is not null
                        select new FileInfoH(j));
        online.Sort((FileInfoH left, FileInfoH right) =>
        {
            return (int)(left.fileSize - right.fileSize);
        });

        List<FileInfoH> local = [];
        List<FileInfo> items = [];
        foreach (FileInfo item in DirectoryH.EnsureExists(textBox_game.Text).EnumerateFiles("*", SearchOption.AllDirectories))
        {
            string remoteName = FileInfoH.GetRemoteName(item.FullName);
            if (remoteName.Equals("config.ini")
             || remoteName.EndsWith("pkg_version")
             || remoteName.Contains("/webCaches/")
             || remoteName.Contains("/SDKCaches/")
             || (remoteName.Contains("/Persistent/")
             && !remoteName.Contains("/StreamingAssets/")))
                continue;
            items.Add(item);
        }
        items.Sort((FileInfo left, FileInfo right) =>
        {
            return (int)(left.Length - right.Length);
        });

        progressBar.Value = 0;
        progressBar.Maximum = items.Count;
        progressBar.Style = ProgressBarStyle.Continuous;
        foreach (var item in items)
        {
            using (FileInfoH fi = new(item.FullName))
            {
                if (radioButton_md5.Checked) await fi.ComputeMD5();
                if (radioButton_hash.Checked) await fi.ComputeHash();
                if (radioButton_both.Checked) await fi.ComputeAll();
                local.Add(fi);
            }
            progressBar.Value = local.Count;
            groupBox_progress.Text = $"{resourceManager.GetString("groupBox_progress.Text")} ({local.Count} of {items.Count})";
            if (CancellingCompare) break;
        }
        items.Clear();
        if (CancellingCompare) return;
        List<FileInfoH> surplus = local.Except(online).ToList();
        List<FileInfoH> missing = online.Except(local).ToList();
        groupBox_suplus.Text = $"{resourceManager.GetString("groupBox_suplus.Text")} ({surplus.Count} of {local.Count})"; local.Clear();
        groupBox_missing.Text = $"{resourceManager.GetString("groupBox_missing.Text")} ({missing.Count} of {online.Count})"; online.Clear();
        surplus.ForEach((i) => textBox_suplus.Text += $"{i.remoteName}\r\n"); surplus.Clear();
        missing.ForEach((i) => textBox_missing.Text += $"{i}\r\n"); missing.Clear();
    }

    private void Button_Start_Click(object sender, EventArgs e)
    {
        button_compare.Enabled = button_start.Enabled = false;
        StartFix().GetAwaiter().OnCompleted(() =>
        {
            radioButton_none.Checked = true;
            Button_Compare_Click(sender, e);
        });
    }

    private async Task StartFix()
    {
        if (string.IsNullOrWhiteSpace(textBox_suplus.Text) && string.IsNullOrWhiteSpace(textBox_missing.Text))
        {
            _ = MessageBox.Show(this, "没有需要修复的文件！", Text, MessageBoxButtons.OK, MessageBoxIcon.Information); return;
        }
        string version = await API.GetLatestVersion(Config?.Channel);
        string path_temp = DirectoryH.EnsureNew(Properties.Settings.Default.TempPath).FullName;
        if (!string.IsNullOrWhiteSpace(textBox_suplus.Text))
        {
            await File.AppendAllTextAsync($"{path_temp}\\deletefiles.txt", textBox_suplus.Text); textBox_suplus.Clear();
        }
        if (!string.IsNullOrWhiteSpace(textBox_missing.Text))
        {
            await File.AppendAllTextAsync($"{path_temp}\\downloadfiles.txt", textBox_missing.Text); textBox_missing.Clear();
        }
        await Worker.HPatchAsync(this, Config?.Channel);
        await Worker.ApplyUpdate(this, version);
        Directory.Delete(path_temp, true);
    }

    private void Timer_RAM_Tick(object sender, EventArgs e)
    {
        Resource.MemoryManager(this, resourceManager);
    }

    private void Form_Fixer_FormClosing(object sender, FormClosingEventArgs e)
    {
        CancellingCompare = true;
    }

    private void Form_Fixer_FormClosed(object sender, FormClosedEventArgs e)
    {
        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
        GC.WaitForFullGCComplete();
    }
}
