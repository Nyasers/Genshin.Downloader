using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Genshin.Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int textBox_path_width;
        private int groupBox_version_voicePacks_width;

        public HttpClient Client { get; } = new()
        {
            BaseAddress = new Uri("https://genshin.nyaser.tk"),
            DefaultRequestVersion = Version.Parse("2.0")
        };

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox_path_width = Width - textBox_path.Width;
            groupBox_version_voicePacks_width = Width - groupBox_version_voicePacks.Width;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            textBox_path.Width = Width - textBox_path_width;
            groupBox_version_voicePacks.Width = Width - groupBox_version_voicePacks_width;
        }

        private void Button_Path_Browse_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = folderBrowserDialog1.SelectedPath;
                if (!path.EndsWith(@"\Genshin Impact game"))
                {
                    if (!path.EndsWith('\\'))
                    {
                        path += '\\';
                    }
                    path += "Genshin Impact game";
                }
                textBox_path.Text = path;
            }
        }

        private void Button_Check_Update_Click(object sender, EventArgs e)
        {
            CheckUpdate(checkBox_pre_download.Checked);
        }

        private async void CheckUpdate(bool pre_download = false)
        {
            button_check_update.Enabled = false;
            try
            {
                string game = pre_download ? "pre_download_game" : "game";
                HttpResponseMessage message = await Client.GetAsync($"/{game}/latest/version");
                if (message.IsSuccessStatusCode)
                {
                    textBox_version_latest.Text = await message.Content.ReadAsStringAsync();
                    toolStripStatusLabel1.Text = textBox_version_current.Text == textBox_version_latest.Text
                        ? "You's using the latest version."
                        : $"Version {textBox_version_latest.Text} is available.";
                    string requestUri = $"/{game}/diffs?version=" + textBox_version_current.Text;
                    HttpResponseMessage message1 = await Client.GetAsync(requestUri);
                    if (!message1.IsSuccessStatusCode)
                    {
                        requestUri = $"/{game}/latest";
                    }
                    listBox_file2down.Items.Clear();
                    _ = listBox_file2down.Items.Add(await new File2Down().BuildAsync(Client, requestUri));
                    string pattern = @"\[(.+)\]";
                    foreach (string item in checkedListBox1.CheckedItems)
                    {
                        string language = Regex.Match(item, pattern).Value[1..^1];
                        _ = listBox_file2down.Items.Add(await new File2Down().BuildAsync(Client, requestUri + "/voice_packs?language=" + language));
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "Cannot get version information, " + message.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message, "Error");
            }
            finally
            {
                button_check_update.Enabled = true;
            }
        }

        private void TextBox_Path_TextChanged(object sender, EventArgs e)
        {
            if (textBox_path.Text.EndsWith("\\Genshin Impact game"))
            {
                string path = textBox_path.Text;

                string path_config = $"{path}\\config.ini";
                if (File.Exists(path_config))
                {
                    textBox_version_current.Text = INI.Read("General", "game_version", path_config);
                }

                string path_voicePacks = @$"{path}\GenshinImpact_Data\StreamingAssets\Audio\GeneratedSoundBanks\Windows\";
                if (Directory.Exists(path_voicePacks))
                {
                    checkedListBox1.SetItemChecked(0, Directory.Exists(path_voicePacks + "Chinese"));
                    checkedListBox1.SetItemChecked(1, Directory.Exists(path_voicePacks + "English(US)"));
                    checkedListBox1.SetItemChecked(2, Directory.Exists(path_voicePacks + "Japanese"));
                    checkedListBox1.SetItemChecked(3, Directory.Exists(path_voicePacks + "Korean"));
                }
            }
        }

        private void ListBox_File2Down_DoubleClick(object sender, EventArgs e)
        {
            DownloadFile((File2Down)listBox_file2down.SelectedItem, 1);
        }

        private static void DownloadFile(File2Down file, int log_level = 0)
        {
            List<string> aria2c_log_levels = new()
            {
                "debug", "info", "notice", "warn", "error"
            };
            List<string> aria2c_errors = new() {
                "All downloads were successful.",
                "An unknown error occurred.",
                "Time out occurred.",
                "A resource was not found.",
                "Aria2 saw the specified number of \"resource not found\" error. See --max-file-not-found option.",
                "A download aborted because download speed was too slow. See --lowest-speed-limit option.",
                "Network problem occurred.",
                "There were unfinished downloads. This error is only reported if all finished downloads were successful and there were unfinished downloads in a queue when aria2 exited by pressing Ctrl-C by an user or sending TERM or INT signal.",
                "Remote server did not support resume when resume was required to complete download.",
                "There was not enough disk space available.",
                "Piece length was different from one in .aria2 control file. See --allow-piece-length-change option.",
                "Aria2 was downloading same file at that moment.",
                "Aria2 was downloading same info hash torrent at that moment.",
                "File already existed. See --allow-overwrite option.",
                "Renaming file failed. See --auto-file-renaming option.",
                "Aria2 could not open existing file.",
                "Aria2 could not create new file or truncate existing file.",
                "File I/O error occurred.",
                "Aria2 could not create directory.",
                "Name resolution failed.",
                "Aria2 could not parse Metalink document.",
                "FTP command failed.",
                "HTTP response header was bad or unexpected.",
                "Too many redirects occurred.",
                "HTTP authorization failed.",
                "Aria2 could not parse bencoded file (usually \".torrent\" file).",
                "\".torrent\" file was corrupted or missing information that aria2 needed.",
                "Magnet URI was bad.",
                "Bad/Unrecognized option was given or unexpected option argument was given.",
                "The remote server was unable to handle the request due to a temporary overloading or maintenance.",
                "Aria2 could not parse JSON-RPC request.",
                "Reserved. Not used.",
                "Checksum validation failed."
            };

            if (file is not null)
            {
                switch (MessageBox.Show($"MD5: {file.md5}\nURL: {file.path}\n\nYes - Download Now (via aria2c.exe)\nNo - Copy URL", file.ToString(), MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        if (!Directory.Exists("Downloads"))
                        {
                            _ = Directory.CreateDirectory("Downloads");
                        }
                        ProcessStartInfo psi = new()
                        {
                            FileName = "aria2c.exe",
                            Arguments = $"-x 8 -s 8 -j 8 --log={file.name}.aria2.log --log-level={aria2c_log_levels[log_level]} --checksum=md5={file.md5} {file.path}",
                            WorkingDirectory = "Downloads"
                        };
                        Task task = new(() =>
                        {
                            Process? process = Process.Start(psi);
                            if (process is not null)
                            {
                                process.WaitForExit();
                                string error = "Unknown error.";
                                if (process.ExitCode is >= 0 and <= 32)
                                {
                                    error = aria2c_errors[process.ExitCode];
                                }
                                _ = MessageBox.Show(error, "aria2c.exe Exited");
                            }
                        });
                        task.Start();
                        break;
                    case DialogResult.No:
                        Clipboard.SetText(file.path);
                        break;
                    default:
                        break;
                }
            }
        }

        private void Button_Open_Installer_Click(object sender, EventArgs e)
        {
            if (textBox_path.Text.EndsWith("\\Genshin Impact game"))
            {
                string path = textBox_path.Text;
                if (!Directory.Exists(path))
                {
                    _ = Directory.CreateDirectory(path);
                }
                Form2 form = new(path);
                _ = form.ShowDialog(this);
            }
            else
            {
                Button_Path_Browse_Click(sender, e);
            }
        }
    }
}