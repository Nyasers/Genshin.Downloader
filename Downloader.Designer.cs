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
            this.button_path_browse = new System.Windows.Forms.Button();
            this.textBox_path = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.listBox_file2down = new System.Windows.Forms.ListBox();
            this.groupBox_file2down = new System.Windows.Forms.GroupBox();
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
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.button_open_installer = new System.Windows.Forms.Button();
            this.groupBox_version = new System.Windows.Forms.GroupBox();
            this.groupBox_path.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.groupBox_file2down.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_version_game.SuspendLayout();
            this.groupBox_version_voicePacks.SuspendLayout();
            this.groupBox_version.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_path
            // 
            this.groupBox_path.Controls.Add(this.button_path_browse);
            this.groupBox_path.Controls.Add(this.textBox_path);
            this.groupBox_path.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_path.Location = new System.Drawing.Point(0, 0);
            this.groupBox_path.Name = "groupBox_path";
            this.groupBox_path.Size = new System.Drawing.Size(364, 51);
            this.groupBox_path.TabIndex = 0;
            this.groupBox_path.TabStop = false;
            this.groupBox_path.Text = "游戏目录";
            // 
            // button_path_browse
            // 
            this.button_path_browse.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button_path_browse.Location = new System.Drawing.Point(283, 22);
            this.button_path_browse.Name = "button_path_browse";
            this.button_path_browse.Size = new System.Drawing.Size(75, 23);
            this.button_path_browse.TabIndex = 0;
            this.button_path_browse.Text = "浏览..";
            this.button_path_browse.UseVisualStyleBackColor = true;
            this.button_path_browse.Click += new System.EventHandler(this.Button_Path_Browse_Click);
            // 
            // textBox_path
            // 
            this.textBox_path.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBox_path.Location = new System.Drawing.Point(6, 22);
            this.textBox_path.Name = "textBox_path";
            this.textBox_path.PlaceholderText = "...\\Genshin Impact game";
            this.textBox_path.Size = new System.Drawing.Size(271, 23);
            this.textBox_path.TabIndex = 1;
            this.textBox_path.TextChanged += new System.EventHandler(this.TextBox_Path_TextChanged);
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
            this.statusStrip1.Location = new System.Drawing.Point(3, 110);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(358, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(260, 17);
            this.toolStripStatusLabel1.Text = "建议先设置游戏目录，然后再进行进一步操作。";
            this.toolStripStatusLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listBox_file2down
            // 
            this.listBox_file2down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox_file2down.FormattingEnabled = true;
            this.listBox_file2down.ItemHeight = 17;
            this.listBox_file2down.Location = new System.Drawing.Point(3, 19);
            this.listBox_file2down.Name = "listBox_file2down";
            this.listBox_file2down.Size = new System.Drawing.Size(358, 91);
            this.listBox_file2down.TabIndex = 1;
            this.listBox_file2down.DoubleClick += new System.EventHandler(this.ListBox_File2Down_DoubleClick);
            // 
            // groupBox_file2down
            // 
            this.groupBox_file2down.Controls.Add(this.listBox_file2down);
            this.groupBox_file2down.Controls.Add(this.statusStrip1);
            this.groupBox_file2down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_file2down.Location = new System.Drawing.Point(0, 230);
            this.groupBox_file2down.Name = "groupBox_file2down";
            this.groupBox_file2down.Size = new System.Drawing.Size(364, 135);
            this.groupBox_file2down.TabIndex = 2;
            this.groupBox_file2down.TabStop = false;
            this.groupBox_file2down.Text = "文件列表 (双击下载)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBox_API);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(364, 53);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "资源包API";
            // 
            // comboBox_API
            // 
            this.comboBox_API.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_API.FormattingEnabled = true;
            this.comboBox_API.Location = new System.Drawing.Point(6, 22);
            this.comboBox_API.Name = "comboBox_API";
            this.comboBox_API.Size = new System.Drawing.Size(352, 25);
            this.comboBox_API.Sorted = true;
            this.comboBox_API.TabIndex = 0;
            this.comboBox_API.SelectedIndexChanged += new System.EventHandler(this.ComboBox_API_SelectedIndexChanged);
            // 
            // button_check_update
            // 
            this.button_check_update.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_check_update.Location = new System.Drawing.Point(273, 22);
            this.button_check_update.Name = "button_check_update";
            this.button_check_update.Size = new System.Drawing.Size(85, 46);
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
            this.groupBox_version_game.Location = new System.Drawing.Point(12, 22);
            this.groupBox_version_game.Name = "groupBox_version_game";
            this.groupBox_version_game.Size = new System.Drawing.Size(96, 98);
            this.groupBox_version_game.TabIndex = 5;
            this.groupBox_version_game.TabStop = false;
            this.groupBox_version_game.Text = "游戏本体";
            // 
            // checkBox_pre_download
            // 
            this.checkBox_pre_download.AutoSize = true;
            this.checkBox_pre_download.Location = new System.Drawing.Point(6, 71);
            this.checkBox_pre_download.Name = "checkBox_pre_download";
            this.checkBox_pre_download.Size = new System.Drawing.Size(87, 21);
            this.checkBox_pre_download.TabIndex = 8;
            this.checkBox_pre_download.Text = "检查预下载";
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
            this.textBox_version_latest.Location = new System.Drawing.Point(47, 45);
            this.textBox_version_latest.Name = "textBox_version_latest";
            this.textBox_version_latest.Size = new System.Drawing.Size(40, 23);
            this.textBox_version_latest.TabIndex = 6;
            this.textBox_version_latest.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_version_current
            // 
            this.textBox_version_current.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.textBox_version_current.Location = new System.Drawing.Point(47, 16);
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
            this.groupBox_version_voicePacks.Controls.Add(this.checkedListBox1);
            this.groupBox_version_voicePacks.Location = new System.Drawing.Point(114, 22);
            this.groupBox_version_voicePacks.Name = "groupBox_version_voicePacks";
            this.groupBox_version_voicePacks.Size = new System.Drawing.Size(153, 98);
            this.groupBox_version_voicePacks.TabIndex = 6;
            this.groupBox_version_voicePacks.TabStop = false;
            this.groupBox_version_voicePacks.Text = "语音包";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "[zh-cn]Chinese",
            "[en-us]English(US)",
            "[ja-jp]Japanese",
            "[ko-kr]Korean"});
            this.checkedListBox1.Location = new System.Drawing.Point(3, 19);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(147, 76);
            this.checkedListBox1.TabIndex = 0;
            // 
            // button_open_installer
            // 
            this.button_open_installer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_open_installer.Location = new System.Drawing.Point(273, 74);
            this.button_open_installer.Name = "button_open_installer";
            this.button_open_installer.Size = new System.Drawing.Size(85, 46);
            this.button_open_installer.TabIndex = 7;
            this.button_open_installer.Text = "打开安装器";
            this.button_open_installer.UseVisualStyleBackColor = true;
            this.button_open_installer.Click += new System.EventHandler(this.Button_Open_Installer_Click);
            // 
            // groupBox_version
            // 
            this.groupBox_version.Controls.Add(this.button_open_installer);
            this.groupBox_version.Controls.Add(this.groupBox_version_voicePacks);
            this.groupBox_version.Controls.Add(this.groupBox_version_game);
            this.groupBox_version.Controls.Add(this.button_check_update);
            this.groupBox_version.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_version.Location = new System.Drawing.Point(0, 104);
            this.groupBox_version.Name = "groupBox_version";
            this.groupBox_version.Size = new System.Drawing.Size(364, 126);
            this.groupBox_version.TabIndex = 1;
            this.groupBox_version.TabStop = false;
            this.groupBox_version.Text = "游戏版本信息";
            // 
            // Form_Downloader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 365);
            this.Controls.Add(this.groupBox_file2down);
            this.Controls.Add(this.groupBox_version);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox_path);
            this.MaximizeBox = false;
            this.Name = "Form_Downloader";
            this.Text = "Genshin Impact 下载器";
            this.Load += new System.EventHandler(this.Form_Downloader_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.groupBox_path.ResumeLayout(false);
            this.groupBox_path.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox_file2down.ResumeLayout(false);
            this.groupBox_file2down.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox_version_game.ResumeLayout(false);
            this.groupBox_version_game.PerformLayout();
            this.groupBox_version_voicePacks.ResumeLayout(false);
            this.groupBox_version.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private CheckedListBox checkedListBox1;
        private GroupBox groupBox_version;
        private ComboBox comboBox_API;
    }
}