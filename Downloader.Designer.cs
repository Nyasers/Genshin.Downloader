namespace Genshin.Downloader
{
    partial class Form_Downloader
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox_path = new System.Windows.Forms.GroupBox();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.button_path_browse = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox_file2down = new System.Windows.Forms.ListBox();
            this.groupBox_file2down = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button_download = new System.Windows.Forms.Button();
            this.button_select_all = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox_API = new System.Windows.Forms.ComboBox();
            this.button_check_update = new System.Windows.Forms.Button();
            this.groupBox_version_game = new System.Windows.Forms.GroupBox();
            this.checkBox_pre_download = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_version_latest = new System.Windows.Forms.TextBox();
            this.textBox_version_current = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_version_voicePacks = new System.Windows.Forms.GroupBox();
            this.checkedListBox_voicePacks = new System.Windows.Forms.CheckedListBox();
            this.button_open_installer = new System.Windows.Forms.Button();
            this.groupBox_version = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox_path.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox_file2down.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_version_game.SuspendLayout();
            this.groupBox_version_voicePacks.SuspendLayout();
            this.groupBox_version.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_path
            // 
            this.groupBox_path.Controls.Add(this.textBox_path);
            this.groupBox_path.Controls.Add(this.button_path_browse);
            this.groupBox_path.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_path.Location = new System.Drawing.Point(5, 5);
            this.groupBox_path.Name = "groupBox_path";
            this.groupBox_path.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            this.groupBox_path.Size = new System.Drawing.Size(362, 49);
            this.groupBox_path.TabIndex = 0;
            this.groupBox_path.TabStop = false;
            this.groupBox_path.Text = "目录";
            // 
            // textBox_path
            // 
            this.textBox_path.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_path.Location = new System.Drawing.Point(6, 19);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.PlaceholderText = "...\\Genshin Impact game";
            this.textBox_path.Size = new System.Drawing.Size(275, 23);
            this.textBox_path.TabIndex = 1;
            this.textBox_path.TextChanged += new System.EventHandler(this.TextBox_Path_TextChanged);
            // 
            // button_path_browse
            // 
            this.button_path_browse.Dock = System.Windows.Forms.DockStyle.Right;
            this.button_path_browse.Location = new System.Drawing.Point(281, 19);
            this.button_path_browse.Name = "button_path_browse";
            this.button_path_browse.Size = new System.Drawing.Size(75, 24);
            this.button_path_browse.TabIndex = 0;
            this.button_path_browse.Text = "浏览..";
            this.button_path_browse.UseVisualStyleBackColor = true;
            this.button_path_browse.Click += new System.EventHandler(this.Button_Path_Browse_Click);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.UseDescriptionForTitle = true;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(5, 351);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(362, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(248, 17);
            this.toolStripStatusLabel1.Text = "建议先设置游戏目录，然后再进行其他操作。";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBox_file2down
            // 
            this.listBox_file2down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_file2down.FormattingEnabled = true;
            this.listBox_file2down.ItemHeight = 17;
            this.listBox_file2down.Location = new System.Drawing.Point(3, 19);
            this.listBox_file2down.Name = "listBox_file2down";
            this.listBox_file2down.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.listBox_file2down.Size = new System.Drawing.Size(248, 72);
            this.listBox_file2down.TabIndex = 1;
            this.listBox_file2down.SelectedIndexChanged += new System.EventHandler(this.ListBox_file2down_SelectedIndexChanged);
            // 
            // groupBox_file2down
            // 
            this.groupBox_file2down.Controls.Add(this.groupBox4);
            this.groupBox_file2down.Controls.Add(this.groupBox3);
            this.groupBox_file2down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_file2down.Location = new System.Drawing.Point(5, 231);
            this.groupBox_file2down.Name = "groupBox_file2down";
            this.groupBox_file2down.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            this.groupBox_file2down.Size = new System.Drawing.Size(362, 120);
            this.groupBox_file2down.TabIndex = 2;
            this.groupBox_file2down.TabStop = false;
            this.groupBox_file2down.Text = "文件";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBox_file2down);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(6, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.groupBox4.Size = new System.Drawing.Size(254, 95);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "列表";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.button_download);
            this.groupBox3.Controls.Add(this.button_select_all);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(260, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(6, 3, 6, 8);
            this.groupBox3.Size = new System.Drawing.Size(96, 95);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "按钮";
            // 
            // button_download
            // 
            this.button_download.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_download.Location = new System.Drawing.Point(6, 53);
            this.button_download.Name = "button_download";
            this.button_download.Size = new System.Drawing.Size(84, 34);
            this.button_download.TabIndex = 2;
            this.button_download.Text = "下载";
            this.button_download.UseVisualStyleBackColor = true;
            this.button_download.Click += new System.EventHandler(this.Button_Download_Click);
            // 
            // button_select_all
            // 
            this.button_select_all.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_select_all.Location = new System.Drawing.Point(6, 19);
            this.button_select_all.Name = "button_select_all";
            this.button_select_all.Size = new System.Drawing.Size(84, 34);
            this.button_select_all.TabIndex = 3;
            this.button_select_all.Text = "全选";
            this.button_select_all.UseVisualStyleBackColor = true;
            this.button_select_all.Click += new System.EventHandler(this.Button_select_all_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_API);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(5, 54);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(6, 3, 6, 8);
            this.groupBox1.Size = new System.Drawing.Size(362, 51);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "版本";
            // 
            // comboBox_API
            // 
            this.comboBox_API.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox_API.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_API.FormattingEnabled = true;
            this.comboBox_API.Location = new System.Drawing.Point(6, 19);
            this.comboBox_API.Name = "comboBox_API";
            this.comboBox_API.Size = new System.Drawing.Size(350, 25);
            this.comboBox_API.Sorted = true;
            this.comboBox_API.TabIndex = 0;
            this.comboBox_API.SelectedIndexChanged += new System.EventHandler(this.ComboBox_API_SelectedIndexChanged);
            // 
            // button_check_update
            // 
            this.button_check_update.Dock = System.Windows.Forms.DockStyle.Top;
            this.button_check_update.Location = new System.Drawing.Point(6, 19);
            this.button_check_update.Name = "button_check_update";
            this.button_check_update.Size = new System.Drawing.Size(84, 36);
            this.button_check_update.TabIndex = 4;
            this.button_check_update.Text = "检查更新";
            this.button_check_update.UseVisualStyleBackColor = true;
            this.button_check_update.Click += new System.EventHandler(this.Button_Check_Update_Click);
            // 
            // groupBox_version_game
            // 
            this.groupBox_version_game.Controls.Add(this.checkBox_pre_download);
            this.groupBox_version_game.Controls.Add(this.label2);
            this.groupBox_version_game.Controls.Add(this.textBox_version_latest);
            this.groupBox_version_game.Controls.Add(this.textBox_version_current);
            this.groupBox_version_game.Controls.Add(this.label1);
            this.groupBox_version_game.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_version_game.Location = new System.Drawing.Point(6, 19);
            this.groupBox_version_game.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox_version_game.Name = "groupBox_version_game";
            this.groupBox_version_game.Size = new System.Drawing.Size(93, 101);
            this.groupBox_version_game.TabIndex = 5;
            this.groupBox_version_game.TabStop = false;
            this.groupBox_version_game.Text = "本体";
            // 
            // checkBox_pre_download
            // 
            this.checkBox_pre_download.AutoSize = true;
            this.checkBox_pre_download.Location = new System.Drawing.Point(17, 74);
            this.checkBox_pre_download.Name = "checkBox_pre_download";
            this.checkBox_pre_download.Size = new System.Drawing.Size(63, 21);
            this.checkBox_pre_download.TabIndex = 8;
            this.checkBox_pre_download.Text = "预下载";
            this.checkBox_pre_download.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "最新:";
            // 
            // textBox_version_latest
            // 
            this.textBox_version_latest.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_version_latest.Location = new System.Drawing.Point(44, 46);
            this.textBox_version_latest.Name = "textBox_version_latest";
            this.textBox_version_latest.Size = new System.Drawing.Size(40, 23);
            this.textBox_version_latest.TabIndex = 6;
            this.textBox_version_latest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_version_current
            // 
            this.textBox_version_current.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_version_current.Location = new System.Drawing.Point(44, 17);
            this.textBox_version_current.Name = "textBox_version_current";
            this.textBox_version_current.Size = new System.Drawing.Size(40, 23);
            this.textBox_version_current.TabIndex = 5;
            this.textBox_version_current.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "当前:";
            // 
            // groupBox_version_voicePacks
            // 
            this.groupBox_version_voicePacks.Controls.Add(this.checkedListBox_voicePacks);
            this.groupBox_version_voicePacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_version_voicePacks.Location = new System.Drawing.Point(99, 19);
            this.groupBox_version_voicePacks.Name = "groupBox_version_voicePacks";
            this.groupBox_version_voicePacks.Padding = new System.Windows.Forms.Padding(3, 3, 3, 4);
            this.groupBox_version_voicePacks.Size = new System.Drawing.Size(161, 101);
            this.groupBox_version_voicePacks.TabIndex = 6;
            this.groupBox_version_voicePacks.TabStop = false;
            this.groupBox_version_voicePacks.Text = "语音";
            // 
            // checkedListBox_voicePacks
            // 
            this.checkedListBox_voicePacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox_voicePacks.FormattingEnabled = true;
            this.checkedListBox_voicePacks.Items.AddRange(new object[] {
            "[zh-cn]Chinese",
            "[en-us]English(US)",
            "[ja-jp]Japanese",
            "[ko-kr]Korean"});
            this.checkedListBox_voicePacks.Location = new System.Drawing.Point(3, 19);
            this.checkedListBox_voicePacks.Name = "checkedListBox_voicePacks";
            this.checkedListBox_voicePacks.Size = new System.Drawing.Size(155, 78);
            this.checkedListBox_voicePacks.TabIndex = 0;
            // 
            // button_open_installer
            // 
            this.button_open_installer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button_open_installer.Location = new System.Drawing.Point(6, 55);
            this.button_open_installer.Name = "button_open_installer";
            this.button_open_installer.Size = new System.Drawing.Size(84, 38);
            this.button_open_installer.TabIndex = 7;
            this.button_open_installer.Text = "打开安装器";
            this.button_open_installer.UseVisualStyleBackColor = true;
            this.button_open_installer.Click += new System.EventHandler(this.Button_Open_Installer_Click);
            // 
            // groupBox_version
            // 
            this.groupBox_version.Controls.Add(this.groupBox_version_voicePacks);
            this.groupBox_version.Controls.Add(this.groupBox2);
            this.groupBox_version.Controls.Add(this.groupBox_version_game);
            this.groupBox_version.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_version.Location = new System.Drawing.Point(5, 105);
            this.groupBox_version.Name = "groupBox_version";
            this.groupBox_version.Padding = new System.Windows.Forms.Padding(6, 3, 6, 6);
            this.groupBox_version.Size = new System.Drawing.Size(362, 126);
            this.groupBox_version.TabIndex = 1;
            this.groupBox_version.TabStop = false;
            this.groupBox_version.Text = "游戏";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button_open_installer);
            this.groupBox2.Controls.Add(this.button_check_update);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(260, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(6, 3, 6, 8);
            this.groupBox2.Size = new System.Drawing.Size(96, 101);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "按钮";
            // 
            // Form_Downloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 373);
            this.Controls.Add(this.groupBox_file2down);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox_version);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_path);
            this.Name = "Form_Downloader";
            this.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Genshin Impact 下载器";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Downloader_FormClosing);
            this.Load += new System.EventHandler(this.Form_Downloader_Load);
            this.groupBox_path.ResumeLayout(false);
            this.groupBox_path.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox_file2down.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox_version_game.ResumeLayout(false);
            this.groupBox_version_game.PerformLayout();
            this.groupBox_version_voicePacks.ResumeLayout(false);
            this.groupBox_version.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GroupBox groupBox_path;
        private Button button_path_browse;
        private TextBox textBox_path;
        private FolderBrowserDialog folderBrowserDialog1;
        private GroupBox groupBox1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private ListBox listBox_file2down;
        private GroupBox groupBox_file2down;
        private GroupBox groupBox_version_game;
        private CheckBox checkBox_pre_download;
        private Label label2;
        private TextBox textBox_version_latest;
        private TextBox textBox_version_current;
        private Label label1;
        private Button button_open_installer;
        private Button button_check_update;
        private GroupBox groupBox_version_voicePacks;
        private CheckedListBox checkedListBox_voicePacks;
        private GroupBox groupBox_version;
        private ComboBox comboBox_API;
        private Button button_download;
        private GroupBox groupBox2;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private Button button_select_all;
    }
}