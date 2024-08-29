using Newtonsoft.Json;
using System.Resources;

namespace Genshin.Downloader;

public partial class Form_Fixer : Form
{
    private Config? Config;
    private readonly List<string> AudioList;
    private static readonly ResourceManager Resources = new(typeof(Form_Fixer));
    private CancellationTokenSource cancellationTokenSource = new();

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
            _ = MessageBox.Show(this, Downloader.Text.mbox_gamePathEmpty, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            Close(); return;
        }
        textBox_game.Text = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
        Config = new Config(Properties.Settings.Default.GamePath);
        textBox_gameVersion.Text = Config.Version;
    }

    private void Button_Compare_Click(object sender, EventArgs e)
    {
        button_cancel.Enabled = !(button_compare.Enabled = button_start.Enabled = false);
        Compare(cancellationTokenSource.Token).GetAwaiter().OnCompleted(() =>
        {
            GC.Collect(2, GCCollectionMode.Aggressive, true, true); GC.WaitForFullGCComplete();
            progressBar.Value = 0; progressBar.Maximum = 100; progressBar.Style = ProgressBarStyle.Continuous;
            button_cancel.Enabled = !(button_compare.Enabled = button_start.Enabled = true);
        });
    }

    private void Button_Cancel_Click(object sender, EventArgs e)
    {
        cancellationTokenSource.Cancel(); cancellationTokenSource.Token.WaitHandle.WaitOne();
        cancellationTokenSource.Dispose(); cancellationTokenSource = new CancellationTokenSource();
    }

    private async Task Compare(CancellationToken token)
    {
        if (Config is null || Config.Channel is null) return;
        groupBox_suplus.Text = Resources.GetString("groupBox_suplus.Text"); textBox_suplus.Clear();
        groupBox_missing.Text = Resources.GetString("groupBox_missing.Text"); textBox_missing.Clear();
        groupBox_progress.Text = $"{Resources.GetString("groupBox_progress.Text")} ({Downloader.Text.tbox_waitServer})";
        progressBar.Style = ProgressBarStyle.Marquee;
        HttpClient http = new()
        {
            BaseAddress = new Uri((await API.GetAsync(Config.Channel)).main.major.res_list_url)
        };
        Dictionary<string, string> pkg_version = [];
        pkg_version["game"] = await http.GetStringAsync($"{http.BaseAddress}/pkg_version", token);
        foreach (string item in AudioList)
        {
            pkg_version[item] = await http.GetStringAsync($"{http.BaseAddress}/{item}_pkg_version", token);
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
            return (int)(left.size - right.size);
        });

        List<FileInfoH> local = [];
        List<FileInfo> items = [];
        foreach (FileInfo item in DirectoryH.EnsureExists(textBox_game.Text).EnumerateFiles("*", SearchOption.AllDirectories))
        {
            string remoteName = FileInfoH.GetRemoteName(item.FullName);
            if (remoteName.Equals("config.ini")
             || remoteName.StartsWith("ScreenShot")
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
            groupBox_progress.Text = $"{Resources.GetString("groupBox_progress.Text")} ({local.Count} of {items.Count})";
            if (token.IsCancellationRequested) break;
        }
        items.Clear();
        if (token.IsCancellationRequested) return;
        List<FileInfoH> surplus = local.Except(online).ToList();
        List<FileInfoH> missing = online.Except(local).ToList();
        groupBox_suplus.Text = $"{Resources.GetString("groupBox_suplus.Text")} ({surplus.Count} of {local.Count})"; local.Clear();
        groupBox_missing.Text = $"{Resources.GetString("groupBox_missing.Text")} ({missing.Count} of {online.Count})"; online.Clear();
        string surplus_str = string.Empty; surplus.ForEach((i) => surplus_str += $"{i.remoteName}\r\n"); textBox_suplus.Text = surplus_str; surplus.Clear();
        string missing_str = string.Empty; missing.ForEach((i) => missing_str += $"{i}\r\n"); textBox_missing.Text = missing_str; missing.Clear();
    }

    private void Button_Start_Click(object sender, EventArgs e)
    {
        button_compare.Enabled = button_start.Enabled = false;
        StartFix().GetAwaiter().OnCompleted(() =>
        {
            groupBox_progress.Text = Resources.GetString("groupBox_progress.Text"); progressBar.Value = 0;
            groupBox_suplus.Text = Resources.GetString("groupBox_suplus.Text"); textBox_suplus.Clear();
            groupBox_missing.Text = Resources.GetString("groupBox_missing.Text"); textBox_missing.Clear();
            button_compare.Enabled = button_start.Enabled = true;
        });
    }

    private async Task StartFix()
    {
        if (string.IsNullOrWhiteSpace(textBox_suplus.Text) && string.IsNullOrWhiteSpace(textBox_missing.Text))
        {
            _ = MessageBox.Show(this, Downloader.Text.mbox_nothing2Fix, Resources.GetString("$this.Text"), MessageBoxButtons.OK, MessageBoxIcon.Information); return;
        }
        string version = (await API.GetAsync(Config?.Channel)).main.major.version;
        try
        {
            string path_temp = DirectoryH.EnsureNew(Properties.Settings.Default.TempPath).FullName;
            if (!string.IsNullOrWhiteSpace(textBox_suplus.Text))
            {
                await File.AppendAllTextAsync($"{path_temp}\\deletefiles.txt", textBox_suplus.Text);
            }
            if (!string.IsNullOrWhiteSpace(textBox_missing.Text))
            {
                await File.AppendAllTextAsync($"{path_temp}\\downloadfiles.txt", textBox_missing.Text);
            }
            await Worker.HPatchAsync(this, Config?.Channel);
            await Worker.ApplyUpdate(this, version);
        }
        catch (IOException ex)
        {
            if (DialogResult.Retry == MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
            {
                await StartFix();
            }
            else throw;
        }
    }

    private void Timer_RAM_Tick(object sender, EventArgs e)
    {
        Resource.MemoryManager(this, typeof(Form_Fixer));
    }

    private void Form_Fixer_FormClosing(object sender, FormClosingEventArgs e)
    {
        Button_Cancel_Click(sender, e);
    }

    private void Form_Fixer_FormClosed(object sender, FormClosedEventArgs e)
    {
        GC.Collect(2, GCCollectionMode.Aggressive, true, true);
        GC.WaitForFullGCComplete();
    }
}
