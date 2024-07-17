namespace Genshin.Downloader
{
    public partial class Form_Main : Form
    {
        private readonly Dictionary<string, int> ContorlSize = [];
        private readonly List<File2Down> Files = [];
        private string Aria2Input = "";

        public Form_Main()
        {
            InitializeComponent();
            ContorlSize.Add("groupBox_path.Width", Width - groupBox_path.Width);
            ContorlSize.Add("textBox_path.Width", Width - textBox_path.Width);
            ContorlSize.Add("groupBox_voicePack.Width", Width - groupBox_voicePack.Width);
            ContorlSize.Add("groupBox_check.Width", Width - groupBox_check.Width);
            ContorlSize.Add("textBox_update.Width", Width - textBox_update.Width);
            ContorlSize.Add("groupBox_files.Width", Width - groupBox_files.Width);
            ContorlSize.Add("groupBox_files.Height", Height - groupBox_files.Height);
            ContorlSize.Add("textBox_aria2.Width", Width - textBox_aria2.Width);
            ContorlSize.Add("textBox_aria2.Height", Height - textBox_aria2.Height);
        }

        private void Form_Main_SizeChanged(object sender, EventArgs e)
        {
            groupBox_path.Width = Width - ContorlSize["groupBox_path.Width"];
            textBox_path.Width = Width - ContorlSize["textBox_path.Width"];
            groupBox_voicePack.Width = Width - ContorlSize["groupBox_voicePack.Width"];
            groupBox_check.Width = Width - ContorlSize["groupBox_check.Width"];
            textBox_update.Width = Width - ContorlSize["textBox_update.Width"];
            groupBox_files.Width = Width - ContorlSize["groupBox_files.Width"];
            groupBox_files.Height = Height - ContorlSize["groupBox_files.Height"];
            textBox_aria2.Width = Width - ContorlSize["textBox_aria2.Width"];
            textBox_aria2.Height = Height - ContorlSize["textBox_aria2.Height"];
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (comboBox_channel.Items.Count > 0)
            {
                comboBox_channel.SelectedIndex = 0;
            }
            if (!Properties.Settings.Default.Location.IsEmpty) Location = Properties.Settings.Default.Location;
            if (!Properties.Settings.Default.Size.IsEmpty) Size = Properties.Settings.Default.Size;
            if (Properties.Settings.Default.Maximized) WindowState = FormWindowState.Maximized;
            Show(); SetGamePath(Properties.Settings.Default.GamePath);
        }

        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(Properties.Settings.Default.Maximized = WindowState == FormWindowState.Maximized))
            {
                Properties.Settings.Default.Location = Location;
                Properties.Settings.Default.Size = Size;
            }
            Properties.Settings.Default.Save();
        }

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                SetGamePath(folderBrowserDialog1.SelectedPath);
            }
        }

        private void SetGamePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) { return; }
            if (!path.EndsWith(@"\Genshin Impact game"))
            {
                if (!path.EndsWith('\\'))
                {
                    path += '\\';
                }
                path += "Genshin Impact game";
            }


            for (int i = 0; i < checkedListBox_voicePack.Items.Count; i++)
            {
                var item = (string?)checkedListBox_voicePack.Items[i];
                if (item is not null)
                {
                    string? key = StringH.GetKeyName(item);
                    if (key is not null)
                    {
                        checkedListBox_voicePack.SetItemChecked(i, File.Exists(@$"{path}\{API.AudioList[key]}_pkg_version"));
                    }
                }
            }

            Config config = new(textBox_path.Text = Properties.Settings.Default.GamePath = DirectoryH.EnsureExists(path).FullName);
            textBox_version.Text = config.Version ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(config.Channel))
            {
                foreach (string item in comboBox_channel.Items)
                {
                    string? key = StringH.GetKeyName(item);
                    if (key is null)
                    {
                        continue;
                    }
                    else if (key == config.Channel)
                    {
                        comboBox_channel.SelectedItem = item;
                        break;
                    }
                }
            }
            else _ = MessageBox.Show(this, "没有识别到游戏渠道，请手动选择！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Button_Check_Click(object sender, EventArgs e)
        {
            button_check.Enabled = false;
            textBox_update.Text = "[pending] 正在检查更新..";
            string channel = StringH.GetKeyName(comboBox_channel.Text) ?? throw new Exception();
            dynamic data = await API.GetAsync(channel) ?? throw new Exception();
            await CheckUpdate(data, checkBox_pre.Checked);
            button_check.Enabled = true;
        }

        private async Task File2Down_Add(dynamic data)
        {
            File2Down? file = await File2Down.BuildAsync(data);
            if (file is null) return;
            Files.Add(file);
        }

        private async Task CheckUpdate(dynamic content, bool pre_download = false)
        {
            try
            {
                Files.Clear();
                if (content.message == "OK")
                {
                    dynamic data = content.data;
                    dynamic game = data.game;
                    try
                    {
                        if (pre_download)
                        {
                            game = data.pre_download_game;
                        }

                        textBox_update.Text = $"[{game.latest.version}] {(textBox_version.Text == (string)game.latest.version ? "当前已是最新版本，若资源缺失可尝试修复器" : "有新版本可下载")}";
                    }
                    catch
                    {
                        if (pre_download)
                        {
                            checkBox_pre.Checked = false;
                            throw new Exception("未找到可用的预下载");
                        }
                        else
                        {
                            throw new Exception("检查更新失败");
                        }
                    }
                    string current_version = textBox_version.Text;

                    dynamic download = game.latest;
                    foreach (dynamic diff in game.diffs)
                    {
                        if (diff.version == current_version)
                        {
                            download = diff;
                        }
                    }

                    try
                    {
                        dynamic segments = download.segments;
                        if (segments != null)
                        {
                            foreach (dynamic segment in segments)
                            {
                                await File2Down_Add(segment);
                            }
                        }
                        else throw new Exception("未找到可下载的资源");
                    }
                    catch
                    {
                        string? path = StringH.EmptyCheck((string?)download.path);
                        if (path != null)
                        {
                            await File2Down_Add(download);
                        }
                        else
                        {
                            throw;
                        }
                    }
                    try
                    {
                        List<string> languages = [];
                        foreach (string item in checkedListBox_voicePack.CheckedItems)
                        {
                            languages.Add(item[1..6]);
                        }
                        dynamic voice_packs = download.voice_packs;
                        foreach (dynamic voice_pack in voice_packs)
                        {
                            if (languages.IndexOf((string)voice_pack.language) != -1)
                            {
                                File2Down_Add(voice_pack);
                            }
                        }
                    }
                    catch { }

                    Aria2Input = Aria2.GetInput([.. Files]);
                    textBox_aria2.Text = Aria2Input.Replace("\n", "\r\n");
                }
                else
                {
                    throw new Exception("无法获取版本信息");
                }
            }
            catch (Exception ex)
            {
                textBox_update.Text = $"[error] {ex.Message}";
            }
        }

        private async void Button_Download_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Aria2Input)) { return; }
            int exitCode = await Aria2.DownloadAsync(Aria2Input, DirectoryH.EnsureExists(Properties.Settings.Default.DownPath).FullName);
            _ = MessageBox.Show(this, Aria2.GetMessage(exitCode), "Aria2c.exe");
        }

        private void Button_Copy_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Aria2Input)) { return; }
            Clipboard.SetText(Aria2Input);
            _ = MessageBox.Show(this, "复制成功", Text);
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            bool v = SaveConfig();
            _ = MessageBox.Show(this, $"保存{(v ? "成功" : "失败")}", Text, MessageBoxButtons.OK, v ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private bool SaveConfig()
        {
            Config config;
            config = new(Properties.Settings.Default.GamePath);
            string
                            channel = comboBox_channel.Text, version = textBox_version.Text;
            if (!string.IsNullOrWhiteSpace(channel))
            {
                config.Channel = StringH.GetKeyName(channel);
            }
            if (!string.IsNullOrWhiteSpace(version))
            {
                config.Version = version;
            }
            config.Save();
            return config.Channel == StringH.GetKeyName(channel) && config.Version == version;
        }

        private void Button_Installer_Click(object sender, EventArgs e)
        {
            _ = SaveConfig();
            Dictionary<string, object> args = [];
            args["Size"] = Size; args["WindowState"] = WindowState;
            Hide(); new Form_Installer(args).ShowDialog(this); Show();
        }
    }
}
