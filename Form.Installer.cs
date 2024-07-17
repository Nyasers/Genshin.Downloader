namespace Genshin.Downloader
{
    public partial class Form_Installer : Form
    {
        private Config? Config;

        public Form_Installer()
        {
            InitializeComponent();
        }


        private void Form_Installer_Load(object sender, EventArgs e)
        {
            Show();
            if (StringH.WhiteSpaceCheck(Properties.Settings.Default.GamePath) is null)
            {
                _ = MessageBox.Show(this, "未设置游戏目录！", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            else
            {
                textBox_game.Text = DirectoryH.EnsureExists(Properties.Settings.Default.GamePath).FullName;
                Config = new Config(Properties.Settings.Default.GamePath);
                textBox_gameVersion.Text = Config.Version;
            }
            openFileDialog1.InitialDirectory = DirectoryH.EnsureExists($"{Directory.GetCurrentDirectory()}\\{Properties.Settings.Default.DownPath}").FullName;
        }

        private void Button_Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK) SetPackage(openFileDialog1.FileName);
        }

        private void SetPackage(string? fileName)
        {
            textBox_pack.Text =
            textBox_name.Text =
            textBox_type.Text =
            textBox_version.Text = string.Empty;
            if (File.Exists(fileName))
            {
                FileInfo info = new(fileName);

                string file = info.Name;
                if (info.Extension == ".001") file = file[..(file.Length - 4)]; file = file[..(file.Length - 4)];

                string key = file[..file.IndexOf('_')],
                    type = file.Contains("hdiff") ? "HDiff" : "Full", version;
                if (type == "HDiff")
                {
                    version = file[(file.IndexOf('_') + 1)..file.IndexOf("_hdiff")];
                    type += key == "game" ? ".Game" : ".Audio";
                }
                else
                {
                    version = file[(file.LastIndexOf('_') + 1)..];
                    type += key != "Audio" ? ".Game" : ".Audio";
                }

                textBox_pack.Text = info.FullName;
                textBox_name.Text = file + ".zip";
                textBox_type.Text = type;
                textBox_version.Text = version;

                if (type.StartsWith("HDiff")
                    && version[..version.IndexOf('_')] != textBox_gameVersion.Text
                    && DialogResult.Yes != MessageBox.Show(this, $"你选择的资源包版本 ({version})\r\n可能不适用于已安装的游戏版本 ({textBox_gameVersion.Text})\r\n是否仍要使用该资源包？", Text, MessageBoxButtons.YesNo))
                    SetPackage(null);
            }
        }

        private async void Button_Start_Click(object sender, EventArgs e)
        {
            button_browse.Enabled = button_start.Enabled = false;
            await StartInstall();
            button_browse.Enabled = button_start.Enabled = true;
        }

        private async Task StartInstall()
        {
            string pack = textBox_pack.Text, version = textBox_version.Text[(textBox_version.Text.IndexOf('_') + 1)..];

            if (!File.Exists(pack))
            {
                MessageBox.Show(this, $"未指定资源包或指定的资源包不存在", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string path_temp = DirectoryH.EnsureNew(Properties.Settings.Default.TempPath).FullName;
            int exitCode = await FileH.UnzipAsync(pack, path_temp);
            if (exitCode is not 0)
            {
                if (DialogResult.Retry == MessageBox.Show(this, $"{exitCode}", "7za.exe", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error))
                {
                    await StartInstall();
                }
                return;
            }

            string[] type = textBox_type.Text.Split('.');
            if (type[0] is "HDiff")
            {
                await Worker.HPatchAsync(this, Config?.Channel);
            }
            else if (type[0] is not "Full")
            {
                _ = MessageBox.Show(this, "意外的资源包类型", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            await Worker.ApplyUpdate(this, type[1] is "Game" ? version : null);
        }
    }
}
