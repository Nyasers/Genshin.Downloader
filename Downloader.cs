using Codeplex.Data;
using Genshin.Downloader.Helper;
using System.Configuration;

namespace Genshin.Downloader
{
    public partial class Form_Downloader : Form
    {
        public Form_Downloader()
        {
            InitializeComponent();
            MinimumSize = Size;
        }

        private async void Form_Downloader_Load(object sender, EventArgs e)
        {
            if (Config.Read("run") != bool.TrueString)
            {
                _ = Helper.Help.Show();
            }

            foreach (KeyValueConfigurationElement api in Const.APIs)
            {
                _ = comboBox_API.Items.Add($"{api.Key}"/* => {api.Value[8..]}"*/); // ȥ��https://
            }

            SetItem(Config.Read("item") ?? Const.NormalAPI);
            textBox_path.Text = Config.Read("path");
            /*
            try
            {
                _ = (await Const.Client.GetAsync("https://genshin.nyaser.tk/")).EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                if (DialogResult.Retry == MessageBox.Show($"�޷������������ͨ�ţ�{ex.Message}", "����", MessageBoxButtons.RetryCancel))
                {
                    Application.Restart();
                }
                else
                {
                    Application.Exit();
                }
            }*/
        }

        private void Form_Downloader_FormClosing(object sender, FormClosingEventArgs e)
        {
            Config.Write("run", bool.TrueString);
            Config.Write("item", GetAPIKey() ?? Const.NormalAPI);
            Config.Write("path", textBox_path.Text);
        }

        private void ComboBox_API_SelectedIndexChanged(object sender, EventArgs e)
        {
            string? key = GetAPIKey();
            Const.Client.Dispose();
            Const.Client = new HttpClient()
            {
                BaseAddress = new Uri(Const.APIs[key].Value),
                DefaultRequestVersion = Version.Parse("2.0"),
            };
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
                textBox_path.Text = DirectoryH.EnsureExists(path).FullName;
            }
        }

        private void TextBox_Path_TextChanged(object sender, EventArgs e)
        {
            if (textBox_path.Text.EndsWith("\\Genshin Impact game"))
            {
                string path = textBox_path.Text;
                textBox_version_current.Text = Game.GetVersion(path);

                if (Directory.Exists(path))
                {
                    for (int i = 0; i < checkedListBox_voicePacks.Items.Count; i++)
                    {
                        if (checkedListBox_voicePacks.Items[i] is not null)
                        {
                            checkedListBox_voicePacks.SetItemChecked(i, File.Exists(@$"{path}\Audio_{checkedListBox_voicePacks.Items[i].ToString()?[7..]}_pkg_version"));
                        }
                    }
                }
            }
        }

        private async void Button_Check_Update_Click(object sender, EventArgs e)
        {
            try
            {
                comboBox_API.Enabled = button_check_update.Enabled = false;
                await CheckUpdate(checkBox_pre_download.Checked);
            }
            finally
            {
                comboBox_API.Enabled = button_check_update.Enabled = true;
            }
        }

        private async void Button_Download_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = $"������������..";
            List<File2Down> files = new();
            foreach (File2Down file in listBox_file2down.SelectedItems)
            {
                files.Add(file);
            }
            (string, string, string) download_info = await FileH.GetDownloadInfoAsync(files.ToArray());
            int exitCode = await FileH.DownloadFileDialogAsync(download_info, this, toolStripStatusLabel1, 0);
            toolStripStatusLabel1.Text = exitCode switch
            {
                -1 => $"���� aria2c.exe ʧ�ܡ�",
                0 => $"���سɹ���",
                7 => $"����ȡ����",
                9 => $"����ʧ�ܣ����̿ռ䲻�㡣",
                _ => $"���ؽ���������ֵ��{exitCode}��",
            };
        }

        private void Button_Open_Installer_Click(object sender, EventArgs e)
        {
            string path_game = textBox_path.Text;
            if (path_game.EndsWith("\\Genshin Impact game"))
            {
                if (!Directory.Exists(path_game))
                {
                    _ = Directory.CreateDirectory(path_game);
                }

                using Form_Installer installer = new(path_game, @$"{Directory.GetCurrentDirectory()}\{Const.DownPath}", @$"{Directory.GetCurrentDirectory()}\{Const.TempPath}");
                installer.Shown += (object? sender, EventArgs e) => Hide();
                installer.FormClosed += (object? sender, FormClosedEventArgs e) => Show();
                _ = installer.ShowDialog(this);
            }
            else
            {
                Button_Path_Browse_Click(sender, e);
            }
        }

        private void ListBox_file2down_SelectedIndexChanged(object sender, EventArgs e)
        {
            button_select_all.Text = (listBox_file2down.Items.Count == listBox_file2down.SelectedItems.Count) ? "ȫ��ѡ" : "ȫѡ";
        }

        private void Button_select_all_Click(object sender, EventArgs e)
        {
            bool select = listBox_file2down.Items.Count != listBox_file2down.SelectedItems.Count;
            for (int i = 0; i < listBox_file2down.Items.Count; i++)
            {
                listBox_file2down.SetSelected(i, select);
            }
        }

        private string GetAPIKey()
        {
            return comboBox_API.Text/*[..comboBox_API.Text.IndexOf(" => ")]*/;
        }

        private void SetItem(string key)
        {
            foreach (string item in comboBox_API.Items)
            {
                if (item.StartsWith(key))
                {
                    comboBox_API.SelectedItem = item;
                }
            }
        }

        private async Task CheckUpdate(bool pre_download = false)
        {
            try
            {
                textBox_version_latest.Text = "";
                listBox_file2down.Items.Clear();
                HttpResponseMessage response = await Const.Client.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    string content_raw = await response.Content.ReadAsStringAsync();
                    dynamic content = DynamicJson.Parse(content_raw);
                    if (content.message == "OK")
                    {
                        dynamic data = content.data;
                        dynamic game = data.game;
                        try
                        {
                            if (pre_download) game = data.pre_download_game;
                            textBox_version_latest.Text = game.latest.version;
                        }
                        catch
                        {
                            if (pre_download)
                            {
                                toolStripStatusLabel1.Text = "��ǰ�����ڿ��õ�Ԥ���ء�";
                                checkBox_pre_download.Checked = false;
                                return;
                            }
                            else
                            {
                                throw;
                            }
                        }
                        string current_version = textBox_version_current.Text;
                        toolStripStatusLabel1.Text = current_version == game.latest.version
                                ? "�������°汾�����������������ļ���"
                                : $"���� {textBox_version_latest.Text} �汾�����ء�";

                        dynamic download = game.latest;
                        foreach (dynamic diff in game.diffs)
                        {
                            if (diff.version == current_version) download = diff;
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
                        }
                        catch
                        {
                            dynamic path = StringH.EmptyCheck(download.path);
                            if (path != null)
                            {
                                await File2Down_Add(download);
                            }
                            else throw;
                        }
                        try
                        {
                            List<string> languages = new();
                            foreach (string item in checkedListBox_voicePacks.CheckedItems)
                            {
                                languages.Add(item[1..6]);
                            }
                            dynamic voice_packs = download.voice_packs;
                            foreach (dynamic voice_pack in voice_packs)
                            {
                                if (languages.IndexOf(voice_pack.language) != -1)
                                {
                                    File2Down_Add(voice_pack);
                                }
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        throw new Exception(content.message);
                    }
                }
                else
                {
                    toolStripStatusLabel1.Text = "�޷���ȡ�汾��Ϣ������" + response.StatusCode.ToString();
                }
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(this, ex.Message, "����");
            }
        }

        private async Task File2Down_Add(dynamic data)
        {
            File2Down? file = await new File2Down().BuildAsync(data);
            _ = file is null ? 0 : listBox_file2down.Items.Add(file);
        }

        private void ѡ��Ŀ¼PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button_Path_Browse_Click(sender, e);
        }

        private void ������UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button_Check_Update_Click(sender, e);
        }

        private void �򿪰�װ��IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Button_Open_Installer_Click(sender, e);
        }

        private void ���޸���FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = textBox_path.Text;
            if (path.EndsWith("\\Genshin Impact game") && DirectoryH.EnsureExists(path).Exists)
            {
                string ver = GetAPIKey();
                using Form_Fixer fixer = new(path, ver, ArrayH<string>.Parse(checkedListBox_voicePacks.CheckedItems));
                fixer.Shown += (object? sender, EventArgs e) => Hide();
                fixer.FormClosing += (object? sender, FormClosingEventArgs e) =>
                {
                    Show();
                    TextBox_Path_TextChanged(new(), new());
                };
                _ = fixer.ShowDialog(this);
            }
        }
    }
}