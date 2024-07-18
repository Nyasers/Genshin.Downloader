namespace Genshin.Downloader
{
    partial class Form_Main
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Main));
            groupBox_path = new GroupBox();
            button_browse = new Button();
            textBox_path = new TextBox();
            folderBrowserDialog1 = new FolderBrowserDialog();
            groupBox_channel = new GroupBox();
            comboBox_channel = new ComboBox();
            groupBox_version = new GroupBox();
            textBox_version = new TextBox();
            groupBox_voicePack = new GroupBox();
            checkedListBox_voicePack = new CheckedListBox();
            groupBox_check = new GroupBox();
            textBox_update = new TextBox();
            checkBox_pre = new CheckBox();
            button_check = new Button();
            groupBox_files = new GroupBox();
            button_fixer = new Button();
            button_installer = new Button();
            textBox_aria2 = new TextBox();
            button_copy = new Button();
            button_download = new Button();
            groupBox_config = new GroupBox();
            button_save = new Button();
            timer_RAM = new System.Windows.Forms.Timer(components);
            groupBox_path.SuspendLayout();
            groupBox_channel.SuspendLayout();
            groupBox_version.SuspendLayout();
            groupBox_voicePack.SuspendLayout();
            groupBox_check.SuspendLayout();
            groupBox_files.SuspendLayout();
            groupBox_config.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_path
            // 
            groupBox_path.Controls.Add(button_browse);
            groupBox_path.Controls.Add(textBox_path);
            resources.ApplyResources(groupBox_path, "groupBox_path");
            groupBox_path.Name = "groupBox_path";
            groupBox_path.TabStop = false;
            // 
            // button_browse
            // 
            resources.ApplyResources(button_browse, "button_browse");
            button_browse.Name = "button_browse";
            button_browse.UseVisualStyleBackColor = true;
            button_browse.Click += Button_Browse_Click;
            // 
            // textBox_path
            // 
            textBox_path.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_path, "textBox_path");
            textBox_path.Name = "textBox_path";
            textBox_path.ReadOnly = true;
            textBox_path.TabStop = false;
            // 
            // folderBrowserDialog1
            // 
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;
            // 
            // groupBox_channel
            // 
            groupBox_channel.Controls.Add(comboBox_channel);
            resources.ApplyResources(groupBox_channel, "groupBox_channel");
            groupBox_channel.Name = "groupBox_channel";
            groupBox_channel.TabStop = false;
            // 
            // comboBox_channel
            // 
            comboBox_channel.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox_channel.Items.AddRange(new object[] { resources.GetString("comboBox_channel.Items"), resources.GetString("comboBox_channel.Items1"), resources.GetString("comboBox_channel.Items2") });
            resources.ApplyResources(comboBox_channel, "comboBox_channel");
            comboBox_channel.Name = "comboBox_channel";
            comboBox_channel.Sorted = true;
            // 
            // groupBox_version
            // 
            groupBox_version.Controls.Add(textBox_version);
            resources.ApplyResources(groupBox_version, "groupBox_version");
            groupBox_version.Name = "groupBox_version";
            groupBox_version.TabStop = false;
            // 
            // textBox_version
            // 
            resources.ApplyResources(textBox_version, "textBox_version");
            textBox_version.Name = "textBox_version";
            // 
            // groupBox_voicePack
            // 
            groupBox_voicePack.Controls.Add(checkedListBox_voicePack);
            resources.ApplyResources(groupBox_voicePack, "groupBox_voicePack");
            groupBox_voicePack.Name = "groupBox_voicePack";
            groupBox_voicePack.TabStop = false;
            // 
            // checkedListBox_voicePack
            // 
            resources.ApplyResources(checkedListBox_voicePack, "checkedListBox_voicePack");
            checkedListBox_voicePack.Items.AddRange(new object[] { resources.GetString("checkedListBox_voicePack.Items"), resources.GetString("checkedListBox_voicePack.Items1"), resources.GetString("checkedListBox_voicePack.Items2"), resources.GetString("checkedListBox_voicePack.Items3") });
            checkedListBox_voicePack.Name = "checkedListBox_voicePack";
            // 
            // groupBox_check
            // 
            groupBox_check.Controls.Add(textBox_update);
            groupBox_check.Controls.Add(checkBox_pre);
            groupBox_check.Controls.Add(button_check);
            resources.ApplyResources(groupBox_check, "groupBox_check");
            groupBox_check.Name = "groupBox_check";
            groupBox_check.TabStop = false;
            // 
            // textBox_update
            // 
            resources.ApplyResources(textBox_update, "textBox_update");
            textBox_update.Name = "textBox_update";
            textBox_update.ReadOnly = true;
            textBox_update.TabStop = false;
            // 
            // checkBox_pre
            // 
            resources.ApplyResources(checkBox_pre, "checkBox_pre");
            checkBox_pre.Name = "checkBox_pre";
            checkBox_pre.UseVisualStyleBackColor = true;
            // 
            // button_check
            // 
            resources.ApplyResources(button_check, "button_check");
            button_check.Name = "button_check";
            button_check.UseVisualStyleBackColor = true;
            button_check.Click += Button_Check_Click;
            // 
            // groupBox_files
            // 
            groupBox_files.Controls.Add(button_fixer);
            groupBox_files.Controls.Add(button_installer);
            groupBox_files.Controls.Add(textBox_aria2);
            groupBox_files.Controls.Add(button_copy);
            groupBox_files.Controls.Add(button_download);
            resources.ApplyResources(groupBox_files, "groupBox_files");
            groupBox_files.Name = "groupBox_files";
            groupBox_files.TabStop = false;
            // 
            // button_fixer
            // 
            resources.ApplyResources(button_fixer, "button_fixer");
            button_fixer.Name = "button_fixer";
            button_fixer.UseVisualStyleBackColor = true;
            button_fixer.Click += Button_Fixer_Click;
            // 
            // button_installer
            // 
            resources.ApplyResources(button_installer, "button_installer");
            button_installer.Name = "button_installer";
            button_installer.UseVisualStyleBackColor = true;
            button_installer.Click += Button_Installer_Click;
            // 
            // textBox_aria2
            // 
            textBox_aria2.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_aria2, "textBox_aria2");
            textBox_aria2.Name = "textBox_aria2";
            textBox_aria2.ReadOnly = true;
            // 
            // button_copy
            // 
            resources.ApplyResources(button_copy, "button_copy");
            button_copy.Name = "button_copy";
            button_copy.UseVisualStyleBackColor = true;
            button_copy.Click += Button_Copy_Click;
            // 
            // button_download
            // 
            resources.ApplyResources(button_download, "button_download");
            button_download.Name = "button_download";
            button_download.UseVisualStyleBackColor = true;
            button_download.Click += Button_Download_Click;
            // 
            // groupBox_config
            // 
            groupBox_config.Controls.Add(button_save);
            resources.ApplyResources(groupBox_config, "groupBox_config");
            groupBox_config.Name = "groupBox_config";
            groupBox_config.TabStop = false;
            // 
            // button_save
            // 
            resources.ApplyResources(button_save, "button_save");
            button_save.Name = "button_save";
            button_save.UseVisualStyleBackColor = true;
            button_save.Click += Button_Save_Click;
            // 
            // timer_RAM
            // 
            timer_RAM.Enabled = true;
            timer_RAM.Interval = 250;
            timer_RAM.Tick += Timer_RAM_Tick;
            // 
            // Form_Main
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox_config);
            Controls.Add(groupBox_files);
            Controls.Add(groupBox_check);
            Controls.Add(groupBox_voicePack);
            Controls.Add(groupBox_version);
            Controls.Add(groupBox_channel);
            Controls.Add(groupBox_path);
            Name = "Form_Main";
            FormClosing += Form_Main_FormClosing;
            FormClosed += Form_Main_FormClosed;
            Load += Form_Main_Load;
            SizeChanged += Form_Main_SizeChanged;
            groupBox_path.ResumeLayout(false);
            groupBox_path.PerformLayout();
            groupBox_channel.ResumeLayout(false);
            groupBox_version.ResumeLayout(false);
            groupBox_version.PerformLayout();
            groupBox_voicePack.ResumeLayout(false);
            groupBox_check.ResumeLayout(false);
            groupBox_check.PerformLayout();
            groupBox_files.ResumeLayout(false);
            groupBox_files.PerformLayout();
            groupBox_config.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_path;
        private Button button_browse;
        private TextBox textBox_path;
        private FolderBrowserDialog folderBrowserDialog1;
        private GroupBox groupBox_channel;
        private ComboBox comboBox_channel;
        private GroupBox groupBox_version;
        private TextBox textBox_version;
        private GroupBox groupBox_voicePack;
        private CheckedListBox checkedListBox_voicePack;
        private GroupBox groupBox_check;
        private TextBox textBox_update;
        private CheckBox checkBox_pre;
        private Button button_check;
        private GroupBox groupBox_files;
        private GroupBox groupBox_config;
        private Button button_save;
        private Button button_copy;
        private Button button_download;
        private Button button_fixer;
        private Button button_installer;
        private TextBox textBox_aria2;
        private System.Windows.Forms.Timer timer_RAM;
    }
}
