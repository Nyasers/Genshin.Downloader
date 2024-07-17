using Newtonsoft.Json;

namespace Genshin.Downloader
{
    public partial class Form_Fixer : Form
    {
        private Config? Config;
        private readonly List<string> AudioList;

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

        private void Button_Compare_Click(object sender, EventArgs e)
        {
            button_compare.Enabled = button_start.Enabled = false;
            Compare().GetAwaiter().OnCompleted(() => button_compare.Enabled = button_start.Enabled = true);
        }

        private async Task Compare()
        {
            if (Config is null || Config.Channel is null) return;
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
                            select new FileInfoH(JsonConvert.DeserializeObject<dynamic>(i)));
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

            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.Maximum = items.Count;
            string text = groupBox_progress.Text;
            foreach (var item in items)
            {
                local.Add(await FileInfoH.BuildAsync(item, radioButton_hash.Checked || radioButton_both.Checked, radioButton_md5.Checked || radioButton_both.Checked));
                progressBar.Value = local.Count;
                groupBox_progress.Text = $"{text} ({local.Count} of {items.Count})";
            }
            textBox_suplus.Clear(); local.Except(online).ToList().ForEach((i) => textBox_suplus.Text += $"{i}\r\n");
            textBox_missing.Clear(); online.Except(local).ToList().ForEach((i) => textBox_missing.Text += $"{i}\r\n");

            _ = MessageBox.Show(this, $"比对完成啦！", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            groupBox_progress.Text = text;
            progressBar.Value = 0;
        }

        private void Button_Start_Click(object sender, EventArgs e)
        {
            button_compare.Enabled = button_start.Enabled = false;
            StartFix().GetAwaiter().OnCompleted(() => button_compare.Enabled = button_start.Enabled = true); ;
        }

        private async Task StartFix()
        {
            string version = await API.GetLatestVersion(Config?.Channel);
            string path_temp = DirectoryH.EnsureNew(Properties.Settings.Default.TempPath).FullName;
            if (!string.IsNullOrWhiteSpace(textBox_suplus.Text))
                await File.AppendAllTextAsync($"{path_temp}\\deletefiles.txt", textBox_suplus.Text);
            if (!string.IsNullOrWhiteSpace(textBox_missing.Text))
                await File.AppendAllTextAsync($"{path_temp}\\downloadfiles.txt", textBox_missing.Text);
            await Worker.HPatchAsync(this, Config?.Channel);
            await Worker.ApplyUpdate(this, version);
            Directory.Delete(path_temp, true);
        }
    }
}
