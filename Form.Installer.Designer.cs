namespace Genshin.Downloader
{
    partial class Form_Installer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Installer));
            groupBox_path = new GroupBox();
            textBox_game = new TextBox();
            groupBox_pack = new GroupBox();
            button_browse = new Button();
            textBox_pack = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            groupBox_name = new GroupBox();
            textBox_name = new TextBox();
            groupBox_type = new GroupBox();
            textBox_type = new TextBox();
            groupBox_version = new GroupBox();
            textBox_version = new TextBox();
            groupBox_gameVersion = new GroupBox();
            textBox_gameVersion = new TextBox();
            groupBox_install = new GroupBox();
            button_start = new Button();
            timer_RAM = new System.Windows.Forms.Timer(components);
            groupBox_path.SuspendLayout();
            groupBox_pack.SuspendLayout();
            groupBox_name.SuspendLayout();
            groupBox_type.SuspendLayout();
            groupBox_version.SuspendLayout();
            groupBox_gameVersion.SuspendLayout();
            groupBox_install.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_path
            // 
            groupBox_path.Controls.Add(textBox_game);
            resources.ApplyResources(groupBox_path, "groupBox_path");
            groupBox_path.Name = "groupBox_path";
            groupBox_path.TabStop = false;
            // 
            // textBox_game
            // 
            textBox_game.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_game, "textBox_game");
            textBox_game.Name = "textBox_game";
            textBox_game.ReadOnly = true;
            textBox_game.TabStop = false;
            // 
            // groupBox_pack
            // 
            groupBox_pack.Controls.Add(button_browse);
            groupBox_pack.Controls.Add(textBox_pack);
            resources.ApplyResources(groupBox_pack, "groupBox_pack");
            groupBox_pack.Name = "groupBox_pack";
            groupBox_pack.TabStop = false;
            // 
            // button_browse
            // 
            resources.ApplyResources(button_browse, "button_browse");
            button_browse.Name = "button_browse";
            button_browse.UseVisualStyleBackColor = true;
            button_browse.Click += Button_Browse_Click;
            // 
            // textBox_pack
            // 
            textBox_pack.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_pack, "textBox_pack");
            textBox_pack.Name = "textBox_pack";
            textBox_pack.ReadOnly = true;
            textBox_pack.TabStop = false;
            // 
            // openFileDialog1
            // 
            resources.ApplyResources(openFileDialog1, "openFileDialog1");
            openFileDialog1.ReadOnlyChecked = true;
            // 
            // groupBox_name
            // 
            groupBox_name.Controls.Add(textBox_name);
            resources.ApplyResources(groupBox_name, "groupBox_name");
            groupBox_name.Name = "groupBox_name";
            groupBox_name.TabStop = false;
            // 
            // textBox_name
            // 
            textBox_name.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_name, "textBox_name");
            textBox_name.Name = "textBox_name";
            textBox_name.ReadOnly = true;
            textBox_name.TabStop = false;
            // 
            // groupBox_type
            // 
            groupBox_type.Controls.Add(textBox_type);
            resources.ApplyResources(groupBox_type, "groupBox_type");
            groupBox_type.Name = "groupBox_type";
            groupBox_type.TabStop = false;
            // 
            // textBox_type
            // 
            textBox_type.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_type, "textBox_type");
            textBox_type.Name = "textBox_type";
            textBox_type.ReadOnly = true;
            textBox_type.TabStop = false;
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
            textBox_version.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_version, "textBox_version");
            textBox_version.Name = "textBox_version";
            textBox_version.ReadOnly = true;
            textBox_version.TabStop = false;
            // 
            // groupBox_gameVersion
            // 
            groupBox_gameVersion.Controls.Add(textBox_gameVersion);
            resources.ApplyResources(groupBox_gameVersion, "groupBox_gameVersion");
            groupBox_gameVersion.Name = "groupBox_gameVersion";
            groupBox_gameVersion.TabStop = false;
            // 
            // textBox_gameVersion
            // 
            textBox_gameVersion.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_gameVersion, "textBox_gameVersion");
            textBox_gameVersion.Name = "textBox_gameVersion";
            textBox_gameVersion.ReadOnly = true;
            textBox_gameVersion.TabStop = false;
            // 
            // groupBox_install
            // 
            groupBox_install.Controls.Add(button_start);
            resources.ApplyResources(groupBox_install, "groupBox_install");
            groupBox_install.Name = "groupBox_install";
            groupBox_install.TabStop = false;
            // 
            // button_start
            // 
            resources.ApplyResources(button_start, "button_start");
            button_start.Name = "button_start";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += Button_Start_Click;
            // 
            // timer_RAM
            // 
            timer_RAM.Enabled = true;
            timer_RAM.Interval = 1000;
            timer_RAM.Tick += Timer_RAM_Tick;
            // 
            // Form_Installer
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox_install);
            Controls.Add(groupBox_gameVersion);
            Controls.Add(groupBox_version);
            Controls.Add(groupBox_type);
            Controls.Add(groupBox_name);
            Controls.Add(groupBox_pack);
            Controls.Add(groupBox_path);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form_Installer";
            FormClosed += Form_Installer_FormClosed;
            Load += Form_Installer_Load;
            groupBox_path.ResumeLayout(false);
            groupBox_path.PerformLayout();
            groupBox_pack.ResumeLayout(false);
            groupBox_pack.PerformLayout();
            groupBox_name.ResumeLayout(false);
            groupBox_name.PerformLayout();
            groupBox_type.ResumeLayout(false);
            groupBox_type.PerformLayout();
            groupBox_version.ResumeLayout(false);
            groupBox_version.PerformLayout();
            groupBox_gameVersion.ResumeLayout(false);
            groupBox_gameVersion.PerformLayout();
            groupBox_install.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox_path;
        private TextBox textBox_game;
        private GroupBox groupBox_pack;
        private Button button_browse;
        private TextBox textBox_pack;
        private OpenFileDialog openFileDialog1;
        private GroupBox groupBox_name;
        private TextBox textBox_name;
        private GroupBox groupBox_type;
        private TextBox textBox_type;
        private GroupBox groupBox_version;
        private TextBox textBox_version;
        private GroupBox groupBox_gameVersion;
        private TextBox textBox_gameVersion;
        private GroupBox groupBox_install;
        private Button button_start;
        private System.Windows.Forms.Timer timer_RAM;
    }
}