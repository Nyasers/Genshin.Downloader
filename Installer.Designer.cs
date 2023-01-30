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
            this.groupBox_install_file = new System.Windows.Forms.GroupBox();
            this.button_install_file_browse = new System.Windows.Forms.Button();
            this.textBox_install_file_path = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox_file_info = new System.Windows.Forms.GroupBox();
            this.textBox_fullname = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_size = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox_install_button = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox_install_file.SuspendLayout();
            this.groupBox_file_info.SuspendLayout();
            this.groupBox_install_button.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_install_file
            // 
            this.groupBox_install_file.Controls.Add(this.button_install_file_browse);
            this.groupBox_install_file.Controls.Add(this.textBox_install_file_path);
            this.groupBox_install_file.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_install_file.Location = new System.Drawing.Point(0, 0);
            this.groupBox_install_file.Name = "groupBox_install_file";
            this.groupBox_install_file.Size = new System.Drawing.Size(388, 52);
            this.groupBox_install_file.TabIndex = 0;
            this.groupBox_install_file.TabStop = false;
            this.groupBox_install_file.Text = "资源文件 (.zip)";
            // 
            // button_install_file_browse
            // 
            this.button_install_file_browse.Location = new System.Drawing.Point(307, 22);
            this.button_install_file_browse.Name = "button_install_file_browse";
            this.button_install_file_browse.Size = new System.Drawing.Size(75, 23);
            this.button_install_file_browse.TabIndex = 1;
            this.button_install_file_browse.Text = "浏览..";
            this.button_install_file_browse.UseVisualStyleBackColor = true;
            this.button_install_file_browse.Click += new System.EventHandler(this.Button_Install_File_Browse_Click);
            // 
            // textBox_install_file_path
            // 
            this.textBox_install_file_path.Location = new System.Drawing.Point(6, 22);
            this.textBox_install_file_path.Name = "textBox_install_file_path";
            this.textBox_install_file_path.Size = new System.Drawing.Size(295, 23);
            this.textBox_install_file_path.TabIndex = 0;
            this.textBox_install_file_path.TextChanged += new System.EventHandler(this.TextBox_Install_File_Path_TextChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "zip";
            this.openFileDialog1.Filter = ".zip file|*.zip";
            this.openFileDialog1.ReadOnlyChecked = true;
            // 
            // groupBox_file_info
            // 
            this.groupBox_file_info.Controls.Add(this.textBox_fullname);
            this.groupBox_file_info.Controls.Add(this.label3);
            this.groupBox_file_info.Controls.Add(this.textBox_size);
            this.groupBox_file_info.Controls.Add(this.label2);
            this.groupBox_file_info.Controls.Add(this.textBox_name);
            this.groupBox_file_info.Controls.Add(this.label1);
            this.groupBox_file_info.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_file_info.Location = new System.Drawing.Point(0, 52);
            this.groupBox_file_info.Name = "groupBox_file_info";
            this.groupBox_file_info.Size = new System.Drawing.Size(388, 110);
            this.groupBox_file_info.TabIndex = 1;
            this.groupBox_file_info.TabStop = false;
            this.groupBox_file_info.Text = "文件信息";
            // 
            // textBox_fullname
            // 
            this.textBox_fullname.Location = new System.Drawing.Point(74, 80);
            this.textBox_fullname.Name = "textBox_fullname";
            this.textBox_fullname.ReadOnly = true;
            this.textBox_fullname.Size = new System.Drawing.Size(308, 23);
            this.textBox_fullname.TabIndex = 5;
            this.textBox_fullname.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "完整名称";
            // 
            // textBox_size
            // 
            this.textBox_size.Location = new System.Drawing.Point(50, 48);
            this.textBox_size.Name = "textBox_size";
            this.textBox_size.ReadOnly = true;
            this.textBox_size.Size = new System.Drawing.Size(332, 23);
            this.textBox_size.TabIndex = 3;
            this.textBox_size.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "大小";
            // 
            // textBox_name
            // 
            this.textBox_name.Location = new System.Drawing.Point(50, 16);
            this.textBox_name.Name = "textBox_name";
            this.textBox_name.ReadOnly = true;
            this.textBox_name.Size = new System.Drawing.Size(332, 23);
            this.textBox_name.TabIndex = 1;
            this.textBox_name.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称";
            // 
            // groupBox_install_button
            // 
            this.groupBox_install_button.Controls.Add(this.button1);
            this.groupBox_install_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_install_button.Location = new System.Drawing.Point(0, 162);
            this.groupBox_install_button.Name = "groupBox_install_button";
            this.groupBox_install_button.Size = new System.Drawing.Size(388, 179);
            this.groupBox_install_button.TabIndex = 2;
            this.groupBox_install_button.TabStop = false;
            this.groupBox_install_button.Text = "开始按钮";
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.button1.Location = new System.Drawing.Point(3, 19);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(382, 157);
            this.button1.TabIndex = 0;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button_Start_Click);
            // 
            // Form_Installer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 341);
            this.Controls.Add(this.groupBox_install_button);
            this.Controls.Add(this.groupBox_file_info);
            this.Controls.Add(this.groupBox_install_file);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form_Installer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Genshin Impact 安装器";
            this.Load += new System.EventHandler(this.Form_Installer_Load);
            this.groupBox_install_file.ResumeLayout(false);
            this.groupBox_install_file.PerformLayout();
            this.groupBox_file_info.ResumeLayout(false);
            this.groupBox_file_info.PerformLayout();
            this.groupBox_install_button.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private GroupBox groupBox_install_file;
        private TextBox textBox_install_file_path;
        private Button button_install_file_browse;
        private OpenFileDialog openFileDialog1;
        private GroupBox groupBox_file_info;
        private TextBox textBox_size;
        private Label label2;
        private TextBox textBox_name;
        private Label label1;
        private GroupBox groupBox_install_button;
        private TextBox textBox_fullname;
        private Label label3;
        private Button button1;
    }
}