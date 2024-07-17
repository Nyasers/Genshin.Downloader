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
            groupBox_gameVersion = new GroupBox();
            textBox_gameVersion = new TextBox();
            groupBox_path = new GroupBox();
            textBox_game = new TextBox();
            groupBox_method = new GroupBox();
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
            groupBox_gameVersion.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox_gameVersion.Controls.Add(textBox_gameVersion);
            groupBox_gameVersion.Location = new Point(420, 12);
            groupBox_gameVersion.Name = "groupBox_gameVersion";
            groupBox_gameVersion.Size = new Size(72, 51);
            groupBox_gameVersion.TabIndex = 6;
            groupBox_gameVersion.TabStop = false;
            groupBox_gameVersion.Text = "游戏版本";
            // 
            // textBox_gameVersion
            // 
            textBox_gameVersion.BackColor = SystemColors.Window;
            textBox_gameVersion.Location = new Point(6, 22);
            textBox_gameVersion.Name = "textBox_gameVersion";
            textBox_gameVersion.ReadOnly = true;
            textBox_gameVersion.Size = new Size(60, 23);
            textBox_gameVersion.TabIndex = 0;
            textBox_gameVersion.TabStop = false;
            textBox_gameVersion.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox_path
            // 
            groupBox_path.Controls.Add(textBox_game);
            groupBox_path.Location = new Point(12, 12);
            groupBox_path.Name = "groupBox_path";
            groupBox_path.Size = new Size(402, 51);
            groupBox_path.TabIndex = 5;
            groupBox_path.TabStop = false;
            groupBox_path.Text = "游戏目录";
            // 
            // textBox_game
            // 
            textBox_game.BackColor = SystemColors.Window;
            textBox_game.Location = new Point(6, 22);
            textBox_game.Name = "textBox_game";
            textBox_game.ReadOnly = true;
            textBox_game.Size = new Size(390, 23);
            textBox_game.TabIndex = 0;
            textBox_game.TabStop = false;
            // 
            // groupBox_method
            // 
            groupBox_method.Controls.Add(radioButton_both);
            groupBox_method.Controls.Add(radioButton_md5);
            groupBox_method.Controls.Add(button_compare);
            groupBox_method.Controls.Add(radioButton_hash);
            groupBox_method.Controls.Add(radioButton_none);
            groupBox_method.Location = new Point(12, 69);
            groupBox_method.Name = "groupBox_method";
            groupBox_method.Size = new Size(480, 55);
            groupBox_method.TabIndex = 7;
            groupBox_method.TabStop = false;
            groupBox_method.Text = "比对方式";
            // 
            // radioButton_both
            // 
            radioButton_both.Appearance = Appearance.Button;
            radioButton_both.AutoSize = true;
            radioButton_both.Location = new Point(235, 22);
            radioButton_both.Name = "radioButton_both";
            radioButton_both.Size = new Size(45, 27);
            radioButton_both.TabIndex = 3;
            radioButton_both.TabStop = true;
            radioButton_both.Text = "Both";
            radioButton_both.UseVisualStyleBackColor = true;
            // 
            // radioButton_md5
            // 
            radioButton_md5.Appearance = Appearance.Button;
            radioButton_md5.AutoSize = true;
            radioButton_md5.Location = new Point(183, 22);
            radioButton_md5.Name = "radioButton_md5";
            radioButton_md5.Size = new Size(46, 27);
            radioButton_md5.TabIndex = 2;
            radioButton_md5.TabStop = true;
            radioButton_md5.Text = "MD5";
            radioButton_md5.UseVisualStyleBackColor = true;
            // 
            // button_compare
            // 
            button_compare.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            button_compare.Location = new Point(286, 22);
            button_compare.Name = "button_compare";
            button_compare.Size = new Size(188, 27);
            button_compare.TabIndex = 4;
            button_compare.Text = "开始比对";
            button_compare.UseVisualStyleBackColor = true;
            button_compare.Click += Button_Compare_Click;
            // 
            // radioButton_hash
            // 
            radioButton_hash.Appearance = Appearance.Button;
            radioButton_hash.AutoSize = true;
            radioButton_hash.Location = new Point(102, 22);
            radioButton_hash.Name = "radioButton_hash";
            radioButton_hash.Size = new Size(75, 27);
            radioButton_hash.TabIndex = 1;
            radioButton_hash.Text = "XxHash64";
            radioButton_hash.UseVisualStyleBackColor = true;
            // 
            // radioButton_none
            // 
            radioButton_none.Appearance = Appearance.Button;
            radioButton_none.AutoSize = true;
            radioButton_none.Checked = true;
            radioButton_none.Location = new Point(6, 22);
            radioButton_none.Name = "radioButton_none";
            radioButton_none.Size = new Size(90, 27);
            radioButton_none.TabIndex = 0;
            radioButton_none.TabStop = true;
            radioButton_none.Text = "FileSize Only";
            radioButton_none.UseVisualStyleBackColor = true;
            // 
            // groupBox_progress
            // 
            groupBox_progress.Controls.Add(progressBar);
            groupBox_progress.Location = new Point(12, 126);
            groupBox_progress.Name = "groupBox_progress";
            groupBox_progress.Size = new Size(480, 51);
            groupBox_progress.TabIndex = 10;
            groupBox_progress.TabStop = false;
            groupBox_progress.Text = "进度条君";
            // 
            // progressBar
            // 
            progressBar.Location = new Point(6, 22);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(468, 23);
            progressBar.Style = ProgressBarStyle.Continuous;
            progressBar.TabIndex = 0;
            // 
            // groupBox_missing
            // 
            groupBox_missing.Controls.Add(textBox_missing);
            groupBox_missing.Location = new Point(12, 289);
            groupBox_missing.Name = "groupBox_missing";
            groupBox_missing.Size = new Size(480, 100);
            groupBox_missing.TabIndex = 12;
            groupBox_missing.TabStop = false;
            groupBox_missing.Text = "缺失文件";
            // 
            // textBox_missing
            // 
            textBox_missing.BackColor = SystemColors.Window;
            textBox_missing.Dock = DockStyle.Fill;
            textBox_missing.Location = new Point(3, 19);
            textBox_missing.Multiline = true;
            textBox_missing.Name = "textBox_missing";
            textBox_missing.ReadOnly = true;
            textBox_missing.Size = new Size(474, 78);
            textBox_missing.TabIndex = 0;
            textBox_missing.TabStop = false;
            // 
            // groupBox_suplus
            // 
            groupBox_suplus.Controls.Add(textBox_suplus);
            groupBox_suplus.Location = new Point(12, 183);
            groupBox_suplus.Name = "groupBox_suplus";
            groupBox_suplus.Size = new Size(480, 100);
            groupBox_suplus.TabIndex = 11;
            groupBox_suplus.TabStop = false;
            groupBox_suplus.Text = "多余文件";
            // 
            // textBox_suplus
            // 
            textBox_suplus.BackColor = SystemColors.Window;
            textBox_suplus.Dock = DockStyle.Fill;
            textBox_suplus.Location = new Point(3, 19);
            textBox_suplus.Multiline = true;
            textBox_suplus.Name = "textBox_suplus";
            textBox_suplus.ReadOnly = true;
            textBox_suplus.Size = new Size(474, 78);
            textBox_suplus.TabIndex = 0;
            textBox_suplus.TabStop = false;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button_start);
            groupBox1.Location = new Point(12, 395);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(480, 74);
            groupBox1.TabIndex = 13;
            groupBox1.TabStop = false;
            groupBox1.Text = "开始修复";
            // 
            // button_start
            // 
            button_start.Dock = DockStyle.Fill;
            button_start.Font = new Font("Microsoft YaHei UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 134);
            button_start.Location = new Point(3, 19);
            button_start.Name = "button_start";
            button_start.Size = new Size(474, 52);
            button_start.TabIndex = 0;
            button_start.Text = "  启动！";
            button_start.UseVisualStyleBackColor = true;
            button_start.Click += Button_Start_Click;
            // 
            // Form_Fixer
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 481);
            Controls.Add(groupBox1);
            Controls.Add(groupBox_missing);
            Controls.Add(groupBox_suplus);
            Controls.Add(groupBox_progress);
            Controls.Add(groupBox_method);
            Controls.Add(groupBox_gameVersion);
            Controls.Add(groupBox_path);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimumSize = new Size(520, 520);
            Name = "Form_Fixer";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Genshin Impact 修复器";
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
    }
}