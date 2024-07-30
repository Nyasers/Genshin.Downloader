using System.Diagnostics;
using System.Resources;

namespace Genshin.Downloader
{
    public partial class Form_Main : Form
    {
        private readonly Dictionary<string, int> ContorlSize = [];
        private readonly List<File2Down> Files = [];
        private string Aria2Input = "";
        private static readonly ResourceManager resource = new(typeof(Form_Main));

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
            else _ = MessageBox.Show(this, resource.GetString("mbox.channelNotFound"), resource.GetString("mbox.title.notice"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void Button_Check_Click(object sender, EventArgs e)
        {
            button_check.Enabled = false;
            textBox_update.Text = resource.GetString("tbox.checkingUpdate");
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

        private async Task CheckUpdate(dynamic pkgs, bool pre_download = false)
        {
            try
            {
                Files.Clear();
                dynamic? game = null;
                try
                {
                    game = pre_download ? pkgs.pre_download : pkgs.main;
                    textBox_update.Text = $"[{game.major.version}] {(textBox_version.Text == (string)game.major.version ? resource.GetString("tbox.updated") : resource.GetString("tbox.newFound"))}";
                }
                catch
                {
                    if (pre_download)
                    {
                        checkBox_pre.Checked = false;
                        throw new Exception(resource.GetString("error.preDownloadNotFound"));
                    }
                    else
                    {
                        throw new Exception(resource.GetString("error.failedCheckUpdate"));
                    }
                }
                string current_version = textBox_version.Text;

                dynamic download = game.major;
                foreach (dynamic patch in game.patches)
                {
                    if (patch.version == current_version)
                    {
                        download = patch;
                        break;
                    }
                }

                dynamic game_pkgs = download.game_pkgs;
                if (game_pkgs != null)
                {
                    foreach (dynamic game_pkg in game_pkgs)
                    {
                        await File2Down_Add(game_pkg);
                    }
                }
                else throw new Exception(resource.GetString("error.resourceNotFound"));

                try
                {
                    List<string> languages = [];
                    foreach (string item in checkedListBox_voicePack.CheckedItems)
                    {
                        languages.Add(item[1..6]);
                    }
                    dynamic audio_pkgs = download.audio_pkgs;
                    foreach (dynamic audio_pkg in audio_pkgs)
                    {
                        if (languages.IndexOf((string)audio_pkg.language) != -1)
                        {
                            File2Down_Add(audio_pkg);
                        }
                    }
                }
                catch { }

                Aria2Input = Aria2.GetInput([.. Files]);
                textBox_aria2.Text = Aria2Input.Replace("\n", "\r\n");
            }
            catch (Exception ex)
            {
                textBox_update.Text = $"[{resource.GetString("tbox.error")}] {ex.Message}";
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
            _ = MessageBox.Show(this, resource.GetString("mbox.copyS"), Text);
        }

        private void Button_Save_Click(object sender, EventArgs e)
        {
            bool v = SaveConfig();
            _ = MessageBox.Show(this, resource.GetString($"mbox.save{(v ? "S" : "F")}"), Text, MessageBoxButtons.OK, v ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }

        private bool SaveConfig()
        {
            Config config = new(Properties.Settings.Default.GamePath);
            string channel = comboBox_channel.Text, version = textBox_version.Text;
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
            using Form_Installer form = new();
            ShowDialog(form);
        }

        private void Button_Fixer_Click(object sender, EventArgs e)
        {
            Dictionary<string, object> args = [];
            args["AudioList"] = new List<string>();
            foreach (var item in checkedListBox_voicePack.CheckedItems)
            {
                string? key = StringH.GetKeyName(item.ToString());
                if (key is not null) ((List<string>)args["AudioList"]).Add(API.AudioList[key]);
            }
            using Form_Fixer form = new(args);
            ShowDialog(form);
        }

        private void ShowDialog(Form form)
        {
            ArgumentNullException.ThrowIfNull(form);

            if (SaveConfig() is false) return;

            timer_RAM.Enabled = false;

            form.Shown += (object? sender, EventArgs e) => Hide();
            form.FormClosed += (object? sender, FormClosedEventArgs e) => Show();
            form.ShowDialog(this);
            form.Dispose();

            timer_RAM.Enabled = true;

            GC.Collect(2, GCCollectionMode.Aggressive, true, true);
            GC.WaitForFullGCComplete();
        }

        private void Timer_RAM_Tick(object sender, EventArgs e)
        {
            Resource.MemoryManager(this, resource);
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            GC.Collect(2, GCCollectionMode.Aggressive, true, true);
            GC.WaitForFullGCComplete();
        }

        private void Button_Launch_Click(object sender, EventArgs e)
        {
            string path = textBox_path.Text;
            if (Directory.Exists(path))
            {
                if (Directory.Exists(path + "\\YuanShen_Data") && File.Exists(path + "\\YuanShen.exe"))
                {
                    Process.Start("cmd.exe", "/c start \"\" \"" + path + "\\YuanShen.exe\"");
                }
                else if (Directory.Exists(path + "\\GenshinImpact_Data") && File.Exists(path + "\\GenshinImpact.exe"))
                {
                    Process.Start("cmd.exe", "/c start \"\" \"" + path + "\\GenshinImpact.exe\"");
                }
                else
                {
                    _ = MessageBox.Show(this, resource.GetString("mbox.launchFailed"), Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
