namespace Genshin.Downloader
{
    partial class Form_Fixer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_Fixer));
            groupBox_gameVersion = new GroupBox();
            textBox_gameVersion = new TextBox();
            groupBox_path = new GroupBox();
            textBox_game = new TextBox();
            groupBox_method = new GroupBox();
            button_cancel = new Button();
            radioButton_both = new RadioButton();
            radioButton_md5 = new RadioButton();
            button_compare = new Button();
            radioButton_hash = new RadioButton();
            radioButton_none = new RadioButton();
            groupBox_progress = new GroupBox();
            progressBar = new ProgressBar();
            groupBox_missing = new GroupBox();
            textBox_missing = new TextBox();
            groupBox_suplus = new GroupBox();
            textBox_suplus = new TextBox();
            groupBox1 = new GroupBox();
            button_start = new Button();
            timer_RAM = new System.Windows.Forms.Timer(components);
            groupBox_gameVersion.SuspendLayout();
            groupBox_path.SuspendLayout();
            groupBox_method.SuspendLayout();
            groupBox_progress.SuspendLayout();
            groupBox_missing.SuspendLayout();
            groupBox_suplus.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_gameVersion
            // 
            resources.ApplyResources(groupBox_gameVersion, "groupBox_gameVersion");
            groupBox_gameVersion.Controls.Add(textBox_gameVersion);
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
            // groupBox_method
            // 
            groupBox_method.Controls.Add(button_cancel);
            groupBox_method.Controls.Add(radioButton_both);
            groupBox_method.Controls.Add(radioButton_md5);
            groupBox_method.Controls.Add(button_compare);
            groupBox_method.Controls.Add(radioButton_hash);
            groupBox_method.Controls.Add(radioButton_none);
            resources.ApplyResources(groupBox_method, "groupBox_method");
            groupBox_method.Name = "groupBox_method";
            groupBox_method.TabStop = false;
            // 
            // button_cancel
            // 
            resources.ApplyResources(button_cancel, "button_cancel");
            button_cancel.Name = "button_cancel";
            button_cancel.UseVisualStyleBackColor = true;
            button_cancel.Click += Button_Cancel_Click;
            // 
            // radioButton_both
            // 
            resources.ApplyResources(radioButton_both, "radioButton_both");
            radioButton_both.Name = "radioButton_both";
            radioButton_both.TabStop = true;
            radioButton_both.UseVisualStyleBackColor = true;
            // 
            // radioButton_md5
            // 
            resources.ApplyResources(radioButton_md5, "radioButton_md5");
            radioButton_md5.Name = "radioButton_md5";
            radioButton_md5.TabStop = true;
            radioButton_md5.UseVisualStyleBackColor = true;
            // 
            // button_compare
            // 
            resources.ApplyResources(button_compare, "button_compare");
            button_compare.Name = "button_compare";
            button_compare.UseVisualStyleBackColor = true;
            button_compare.Click += Button_Compare_Click;
            // 
            // radioButton_hash
            // 
            resources.ApplyResources(radioButton_hash, "radioButton_hash");
            radioButton_hash.Name = "radioButton_hash";
            radioButton_hash.UseVisualStyleBackColor = true;
            // 
            // radioButton_none
            // 
            resources.ApplyResources(radioButton_none, "radioButton_none");
            radioButton_none.Checked = true;
            radioButton_none.Name = "radioButton_none";
            radioButton_none.TabStop = true;
            radioButton_none.UseVisualStyleBackColor = true;
            // 
            // groupBox_progress
            // 
            groupBox_progress.Controls.Add(progressBar);
            resources.ApplyResources(groupBox_progress, "groupBox_progress");
            groupBox_progress.Name = "groupBox_progress";
            groupBox_progress.TabStop = false;
            // 
            // progressBar
            // 
            resources.ApplyResources(progressBar, "progressBar");
            progressBar.Name = "progressBar";
            progressBar.Style = ProgressBarStyle.Continuous;
            // 
            // groupBox_missing
            // 
            groupBox_missing.Controls.Add(textBox_missing);
            resources.ApplyResources(groupBox_missing, "groupBox_missing");
            groupBox_missing.Name = "groupBox_missing";
            groupBox_missing.TabStop = false;
            // 
            // textBox_missing
            // 
            textBox_missing.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_missing, "textBox_missing");
            textBox_missing.Name = "textBox_missing";
            textBox_missing.ReadOnly = true;
            textBox_missing.TabStop = false;
            // 
            // groupBox_suplus
            // 
            groupBox_suplus.Controls.Add(textBox_suplus);
            resources.ApplyResources(groupBox_suplus, "groupBox_suplus");
            groupBox_suplus.Name = "groupBox_suplus";
            groupBox_suplus.TabStop = false;
            // 
            // textBox_suplus
            // 
            textBox_suplus.BackColor = SystemColors.Window;
            resources.ApplyResources(textBox_suplus, "textBox_suplus");
            textBox_suplus.Name = "textBox_suplus";
            textBox_suplus.ReadOnly = true;
            textBox_suplus.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button_start);
            resources.ApplyResources(groupBox1, "groupBox1");
            groupBox1.Name = "groupBox1";
            groupBox1.TabStop = false;
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
            // Form_Fixer
            // 
            resources.ApplyResources(this, "$this");
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(groupBox1);
            Controls.Add(groupBox_missing);
            Controls.Add(groupBox_suplus);
            Controls.Add(groupBox_progress);
            Controls.Add(groupBox_method);
            Controls.Add(groupBox_gameVersion);
            Controls.Add(groupBox_path);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form_Fixer";
            FormClosing += Form_Fixer_FormClosing;
            FormClosed += Form_Fixer_FormClosed;
            Load += Form_Fixer_Load;
            groupBox_gameVersion.ResumeLayout(false);
            groupBox_gameVersion.PerformLayout();
            groupBox_path.ResumeLayout(false);
            groupBox_path.PerformLayout();
            groupBox_method.ResumeLayout(false);
            groupBox_method.PerformLayout();
            groupBox_progress.ResumeLayout(false);
            groupBox_missing.ResumeLayout(false);
            groupBox_missing.PerformLayout();
            groupBox_suplus.ResumeLayout(false);
            groupBox_suplus.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_gameVersion;
        private TextBox textBox_gameVersion;
        private GroupBox groupBox_path;
        private TextBox textBox_game;
        private GroupBox groupBox_method;
        private RadioButton radioButton_hash;
        private RadioButton radioButton_none;
        private Button button_compare;
        private RadioButton radioButton_md5;
        private RadioButton radioButton_both;
        private GroupBox groupBox_progress;
        private ProgressBar progressBar;
        private GroupBox groupBox_missing;
        private TextBox textBox_missing;
        private GroupBox groupBox_suplus;
        private TextBox textBox_suplus;
        private GroupBox groupBox1;
        private Button button_start;
        private System.Windows.Forms.Timer timer_RAM;
        private Button button_cancel;
    }
}