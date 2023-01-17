using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Genshin.Downloader
{
    public partial class Form2 : Form
    {
        private readonly string path_game;

        public Form2(string path_game)
        {
            InitializeComponent();
            this.path_game = path_game;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Text += $" ({path_game})";
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory() + "\\Downloads";
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
                textBox_size.Text = GetFileSize(fileInfo.Length);
                textBox_fullname.Text = fileInfo.FullName;
            }
        }

        private static string GetFileSize(long size)
        {
            double num = 1024.00;
            return size < num
                ? size + "Byte"
                : size < Math.Pow(num, 2)
                ? (size / num).ToString("f2") + "KB"
                : size < Math.Pow(num, 3)
                ? (size / Math.Pow(num, 2)).ToString("f2") + "MB"
                : size < Math.Pow(num, 4)
                ? (size / Math.Pow(num, 3)).ToString("f2") + "GB"
                : (size / Math.Pow(num, 4)).ToString("f2") + "TB";
        }

        private async void Button_Start_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            string path = textBox_fullname.Text;
            if (File.Exists(path))
            {
                await Install(path, path_game);
            }
            else
            {
                Button_Install_File_Browse_Click(sender, e);
            }
            button1.Enabled = true;
        }

        private async Task Install(string zipFile, string gamePath)
        {
            string name = new FileInfo(zipFile).Name;
            string tmp = Directory.GetCurrentDirectory() + "\\Temp";

            bool game;
            string version_current = INI.Read("General", "game_version", $"{gamePath}\\config.ini");
            string version_new;

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
                    string message = $"Current Version: {version_current}\n" +
                        $"Package Version: {version_old} to {version_new}\n\n" +
                        $"Seems you've installed this package, continue however?";
                    cancel = MessageBox.Show(message, "Notice", MessageBoxButtons.YesNo) != DialogResult.Yes;
                }
                else if (version_current != version_old)
                {
                    string message = $"Current Version: {version_current}\n" +
                        $"Package Version: {version_old} to {version_new}\n\n" +
                        $"Seems you're using wrong package, continue however?";
                    cancel = MessageBox.Show(message, "Notice", MessageBoxButtons.YesNo) != DialogResult.Yes;
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
                    string message = $"Current Version: {version_current}\n" +
                        $"Package Version: {version_new}\n\n" +
                        $"Seems you've installed this package, continue however?";
                    cancel = MessageBox.Show(message, "Notice", MessageBoxButtons.YesNo) != DialogResult.Yes;
                }
                if (cancel)
                {
                    return;
                }

                game = name.StartsWith("GenshinImpact");
            }

            await UnzipAsync(zipFile, tmp);
            await UseHDiffAsync(gamePath, tmp);

            if (game)
            {
                INI.Write("General", "game_version", version_new, $"{gamePath}\\config.ini");
            }

            Process.Start(new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = $"/c xcopy /f /e /y \"{tmp}\" \"{gamePath}\" & del /s /q \"{tmp}\\*\" & rd /s /q \"{tmp}\\GenshinImpact_Data\" &pause&exit"
            })?.WaitForExit();
        }

        private static async Task UnzipAsync(string @in, string @out)
        {
            Process? process = Process.Start(new ProcessStartInfo()
            {
                FileName = "7za.exe",
                Arguments = $"x -o\"{@out}\" \"{@in}\""
            });
            if (process is not null)
            {
                await process.WaitForExitAsync();
            }
        }

        private static async Task UseHDiffAsync(string path, string path_hdiff)
        {
            if (File.Exists($"{path_hdiff}\\deletefiles.txt"))
            {
                string delete_file = $"{path_hdiff}\\deletefiles.txt";
                string batch_file = $"{path_hdiff}\\deletefiles.bat";
                StreamReader reader = new(delete_file);
                StreamWriter writer = new(batch_file, false);
                await writer.WriteLineAsync("@echo off");
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        string command_line = $"del /f \"{path}\\{line.Replace("/", "\\")}\"";
                        await writer.WriteLineAsync(command_line);
                    }
                }
                reader.Close();
                await writer.WriteLineAsync($"del /f \"{delete_file}\"");
                await writer.WriteLineAsync("del %0 &pause&exit");
                writer.Close();
                Process? process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{batch_file}\""
                });
                if (process is not null)
                {
                    await process.WaitForExitAsync();
                }
            }
            if (File.Exists($"{path_hdiff}\\hdifffiles.txt"))
            {
                string hdiff_file = $"{path_hdiff}\\hdifffiles.txt";
                string batch_file = $"{path_hdiff}\\hdifffiles.bat";
                string hpatchz = $"{Directory.GetCurrentDirectory()}\\hpatchz.exe";
                StreamReader reader = new(hdiff_file);
                StreamWriter writer = new(batch_file, false);
                await writer.WriteLineAsync("@echo off");
                while (!reader.EndOfStream)
                {
                    string? line = await reader.ReadLineAsync();
                    if (line != null)
                    {
                        string pattern = @"{""remoteName"": ""(.+)""}";
                        Match match = Regex.Match(line, pattern);
                        if (match.Success)
                        {
                            string remoteName = match.Groups[1].Value.Replace("/", "\\");
                            string oldFile = $"{path}\\{remoteName}";
                            string diffFile = $"{path_hdiff}\\{remoteName}.hdiff";
                            string newFile = $"{path_hdiff}\\{remoteName}";
                            string command_line = $"\"{hpatchz}\" \"{oldFile}\" \"{diffFile}\" \"{newFile}\" & del /f \"{diffFile}\"";
                            await writer.WriteLineAsync(command_line);
                        }
                    }
                }
                reader.Close();
                await writer.WriteLineAsync($"del /f \"{hdiff_file}\"");
                await writer.WriteLineAsync("del %0 &pause&exit");
                writer.Close();
                Process? process = Process.Start(new ProcessStartInfo()
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c \"{batch_file}\""
                });
                if (process is not null)
                {
                    await process.WaitForExitAsync();
                }
            }
        }
    }
}
