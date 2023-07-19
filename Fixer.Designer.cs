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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_info = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button2_Fix = new System.Windows.Forms.Button();
            this.button3_Launch = new System.Windows.Forms.Button();
            this.button1_Count = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_info);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(692, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息";
            // 
            // textBox_info
            // 
            this.textBox_info.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_info.Location = new System.Drawing.Point(3, 19);
            this.textBox_info.Multiline = true;
            this.textBox_info.Name = "textBox_info";
            this.textBox_info.ReadOnly = true;
            this.textBox_info.Size = new System.Drawing.Size(686, 72);
            this.textBox_info.TabIndex = 0;
            this.textBox_info.TabStop = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 446);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(692, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button2_Fix);
            this.groupBox2.Controls.Add(this.button3_Launch);
            this.groupBox2.Controls.Add(this.button1_Count);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 94);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(692, 47);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "操作";
            // 
            // button2_Fix
            // 
            this.button2_Fix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button2_Fix.Location = new System.Drawing.Point(203, 19);
            this.button2_Fix.Name = "button2_Fix";
            this.button2_Fix.Size = new System.Drawing.Size(286, 25);
            this.button2_Fix.TabIndex = 1;
            this.button2_Fix.Text = "Step 2: 修复差异";
            this.button2_Fix.UseVisualStyleBackColor = true;
            this.button2_Fix.Click += new System.EventHandler(this.Button2_Fix_Click);
            // 
            // button3_Launch
            // 
            this.button3_Launch.Dock = System.Windows.Forms.DockStyle.Right;
            this.button3_Launch.Location = new System.Drawing.Point(489, 19);
            this.button3_Launch.Name = "button3_Launch";
            this.button3_Launch.Size = new System.Drawing.Size(200, 25);
            this.button3_Launch.TabIndex = 2;
            this.button3_Launch.Text = "Step 3: 启动游戏";
            this.button3_Launch.UseVisualStyleBackColor = true;
            this.button3_Launch.Click += new System.EventHandler(this.Button3_Luanch_Click);
            // 
            // button1_Count
            // 
            this.button1_Count.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1_Count.Location = new System.Drawing.Point(3, 19);
            this.button1_Count.Name = "button1_Count";
            this.button1_Count.Size = new System.Drawing.Size(200, 25);
            this.button1_Count.TabIndex = 0;
            this.button1_Count.Text = "Step 1: 统计文件";
            this.button1_Count.UseVisualStyleBackColor = true;
            this.button1_Count.Click += new System.EventHandler(this.Button1_Count_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.textBox_log);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 141);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(692, 305);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "日志";
            // 
            // textBox_log
            // 
            this.textBox_log.BackColor = System.Drawing.SystemColors.Window;
            this.textBox_log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox_log.Location = new System.Drawing.Point(3, 19);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ReadOnly = true;
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(686, 283);
            this.textBox_log.TabIndex = 0;
            // 
            // Form_Fixer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 469);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form_Fixer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Genshin Impact 修复器";
            this.Load += new System.EventHandler(this.Form_Fixer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox1;
        private TextBox textBox_info;
        private ProgressBar progressBar1;
        private GroupBox groupBox2;
        private Button button1_Count;
        private GroupBox groupBox4;
        private TextBox textBox_log;
        private Button button2_Fix;
        private Button button3_Launch;
    }
}