using Genshin.Downloader.Helpers;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Genshin.Downloader
{
    public partial class Form_Installer : Form
    {
        private readonly string path_game;
        private readonly string path_down;
        private readonly string path_temp;

        public Form_Installer(string path_game, string path_down, string path_temp)
        {
            InitializeComponent();
            this.path_game = path_game;
            this.path_down = path_down;
            this.path_temp = path_temp;
        }

        private void Form_Installer_Load(object sender, EventArgs e)
        {
            Text += $" ({path_game})";
            openFileDialog1.InitialDirectory = path_down;
            _ = DirectoryH.EnsureExists(path_game);
            _ = DirectoryH.EnsureExists(path_down);
            _ = DirectoryH.EnsureExists(path_temp);
        }

        private void Button_Install_File_Browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox_install_file_path.Text = openFileDialog1.FileName;
            }
        }

        private void TextBox_Install_File_Path_TextChanged(object sender, EventArgs e)
        {
            string path = textBox_install_file_path.Text;
            if (File.Exists(path))
            {
                FileInfo fileInfo = new(path);
                textBox_name.Text = fileInfo.Name;
                textBox_size.Text = FileH.ParseSize(fileInfo.Length);
                textBox_fullname.Text = fileInfo.FullName;
            }
        }

        private async void Button_Start_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            string path = textBox_fullname.Text;
            if (File.Exists(path))
            {
                await Install(path);
            }
            else
            {
                Button_Install_File_Browse_Click(sender, e);
            }
            button1.Enabled = true;
        }

        private async Task Install(string zipFile)
        {
            FileInfo fileInfo = new(zipFile);
            string name = fileInfo.Name, ext = fileInfo.Extension;

            if (ext is not (".zip" or ".001"))
            {
                return;
            }

            bool game;
            string version_new, version_current = INI.Read("General", "game_version", $"{path_game}\\config.ini");

            if (name.Contains("hdiff"))
            {
                //game_x.x.x_x.x.x_hdiff_xxxxxxxxxxxxxxxx.zip
                //la-ng_x.x.x_x.x.x_hdiff_xxxxxxxxxxxxxxxx.zip
                int left, right;
                left = name.IndexOf("_") + 1;
                right = name.IndexOf("_", left);
                string version_old = name[left..right];

                left = right + 1;
                right = name.IndexOf("_", left);
                version_new = name[left..right];

                bool cancel = false;

                if (version_current == version_new)
                {
                    string message = $"当前版本：{version_current}\n" +
                        $"资源包版本：从 {version_old} 更新到 {version_new}\n\n" +
                        $"看起来你好像已经安装了这个更新包，要继续吗？";
                    cancel = MessageBox.Show(message, "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
                }
                else if (version_current != version_old)
                {
                    string message = $"当前版本：{version_current}\n" +
                        $"资源包版本：从 {version_old} 更新到 {version_new}\n\n" +
                        $"看起来你好像选择了错误的资源包，要继续吗？";
                    cancel = MessageBox.Show(message, "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
                }

                if (cancel)
                {
                    return;
                }

                game = name.StartsWith("game");
            }
            else
            {
                // GenshinImpact_x.x.x.zip
                // Audio_Language_x.x.x.zip
                int left;
                left = name.LastIndexOf("_") + 1;
                version_new = name[left..^4];

                bool cancel = false;
                if (version_current == version_new)
                {
                    string message = $"当前版本：{version_current}\n" +
                        $"资源包版本：{version_new}\n\n" +
                        $"看起来你好像已经安装了这个更新包，要继续吗？";
                    cancel = MessageBox.Show(message, "注意", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
                }
                if (cancel)
                {
                    return;
                }

                game = name.StartsWith("GenshinImpact") | name.StartsWith("YuanShen");
            }

            await FileH.UnzipAsync(zipFile, path_temp);
            await FileH.HPatchAsync(path_game, path_temp);

            if (game)
            {
                INI.Write("General", "game_version", version_new, $"{path_game}\\config.ini");
            }

            string command_line = $"xcopy /f /e /y \"{path_temp}\" \"{path_game}\" && del /s /q \"{path_temp}\\*\" && rd /s /q \"{path_temp}\\GenshinImpact_Data\" || pause";
            Process? process = Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $"/c title 正在应用更新.. & {command_line}"
            });
            if (process != null)
            {
                await process.WaitForExitAsync();
            }
        }
    }
}
