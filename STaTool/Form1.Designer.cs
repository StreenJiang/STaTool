namespace STaTool {
    partial class Form1 {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            groupBox_connection_info = new GroupBox();
            label_ip = new Label();
            label_port = new Label();
            comboBox_ip = new ComboBox();
            comboBox_port = new ComboBox();
            button_connect = new Button();
            button_disconnect = new Button();
            groupBox_realtime_log = new GroupBox();
            button_clear_log = new Button();
            textBox_realtime_log = new TextBox();
            groupBox_storage_settings = new GroupBox();
            button_browse = new Button();
            textBox_storage_path = new TextBox();
            label_storage_path = new Label();
            groupBox_connection_info.SuspendLayout();
            groupBox_realtime_log.SuspendLayout();
            groupBox_storage_settings.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox_connection_info
            // 
            groupBox_connection_info.Controls.Add(label_ip);
            groupBox_connection_info.Controls.Add(label_port);
            groupBox_connection_info.Controls.Add(comboBox_ip);
            groupBox_connection_info.Controls.Add(comboBox_port);
            groupBox_connection_info.Controls.Add(button_connect);
            groupBox_connection_info.Controls.Add(button_disconnect);
            groupBox_connection_info.Location = new Point(12, 12);
            groupBox_connection_info.Name = "groupBox_connection_info";
            groupBox_connection_info.Size = new Size(805, 136);
            groupBox_connection_info.TabIndex = 0;
            groupBox_connection_info.TabStop = false;
            groupBox_connection_info.Text = "连接信息";
            // 
            // label_ip
            // 
            label_ip.AutoSize = true;
            label_ip.Location = new Point(28, 40);
            label_ip.Name = "label_ip";
            label_ip.Size = new Size(62, 24);
            label_ip.TabIndex = 2;
            label_ip.Text = "IP地址";
            // 
            // label_port
            // 
            label_port.AutoSize = true;
            label_port.Location = new Point(353, 40);
            label_port.Name = "label_port";
            label_port.Size = new Size(64, 24);
            label_port.TabIndex = 2;
            label_port.Text = "端口号";
            // 
            // comboBox_ip
            // 
            comboBox_ip.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_ip.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_ip.FormattingEnabled = true;
            comboBox_ip.Items.AddRange(new object[] { "192.168.1.99", "192.168.1.10" });
            comboBox_ip.Location = new Point(28, 67);
            comboBox_ip.Name = "comboBox_ip";
            comboBox_ip.Size = new Size(295, 32);
            comboBox_ip.TabIndex = 1;
            // 
            // comboBox_port
            // 
            comboBox_port.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_port.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_port.FormattingEnabled = true;
            comboBox_port.Items.AddRange(new object[] { "60000", "1888" });
            comboBox_port.Location = new Point(353, 67);
            comboBox_port.Name = "comboBox_port";
            comboBox_port.Size = new Size(146, 32);
            comboBox_port.TabIndex = 1;
            // 
            // button_connect
            // 
            button_connect.Location = new Point(537, 65);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(112, 34);
            button_connect.TabIndex = 3;
            button_connect.Text = "连接";
            button_connect.UseVisualStyleBackColor = true;
            // 
            // button_disconnect
            // 
            button_disconnect.Enabled = false;
            button_disconnect.Location = new Point(665, 65);
            button_disconnect.Name = "button_disconnect";
            button_disconnect.Size = new Size(112, 34);
            button_disconnect.TabIndex = 3;
            button_disconnect.Text = "断开";
            button_disconnect.UseVisualStyleBackColor = true;
            // 
            // groupBox_realtime_log
            // 
            groupBox_realtime_log.Controls.Add(button_clear_log);
            groupBox_realtime_log.Controls.Add(textBox_realtime_log);
            groupBox_realtime_log.Location = new Point(12, 154);
            groupBox_realtime_log.Name = "groupBox_realtime_log";
            groupBox_realtime_log.Size = new Size(805, 286);
            groupBox_realtime_log.TabIndex = 1;
            groupBox_realtime_log.TabStop = false;
            groupBox_realtime_log.Text = "实时日志";
            // 
            // button_clear_log
            // 
            button_clear_log.Location = new Point(665, 18);
            button_clear_log.Name = "button_clear_log";
            button_clear_log.Size = new Size(112, 34);
            button_clear_log.TabIndex = 1;
            button_clear_log.Text = "清除";
            button_clear_log.UseVisualStyleBackColor = true;
            // 
            // textBox_realtime_log
            // 
            textBox_realtime_log.BackColor = SystemColors.Control;
            textBox_realtime_log.Location = new Point(28, 58);
            textBox_realtime_log.Multiline = true;
            textBox_realtime_log.Name = "textBox_realtime_log";
            textBox_realtime_log.ReadOnly = true;
            textBox_realtime_log.Size = new Size(749, 203);
            textBox_realtime_log.TabIndex = 0;
            textBox_realtime_log.Text = "等待连接...";
            // 
            // groupBox_storage_settings
            // 
            groupBox_storage_settings.Controls.Add(button_browse);
            groupBox_storage_settings.Controls.Add(textBox_storage_path);
            groupBox_storage_settings.Controls.Add(label_storage_path);
            groupBox_storage_settings.Location = new Point(12, 446);
            groupBox_storage_settings.Name = "groupBox_storage_settings";
            groupBox_storage_settings.Size = new Size(805, 132);
            groupBox_storage_settings.TabIndex = 2;
            groupBox_storage_settings.TabStop = false;
            groupBox_storage_settings.Text = "存储配置";
            // 
            // button_browse
            // 
            button_browse.Location = new Point(665, 65);
            button_browse.Name = "button_browse";
            button_browse.Size = new Size(112, 34);
            button_browse.TabIndex = 4;
            button_browse.Text = "浏览";
            button_browse.UseVisualStyleBackColor = true;
            // 
            // textBox_storage_path
            // 
            textBox_storage_path.Location = new Point(28, 67);
            textBox_storage_path.Name = "textBox_storage_path";
            textBox_storage_path.Size = new Size(621, 30);
            textBox_storage_path.TabIndex = 3;
            // 
            // label_storage_path
            // 
            label_storage_path.AutoSize = true;
            label_storage_path.Location = new Point(28, 40);
            label_storage_path.Name = "label_storage_path";
            label_storage_path.Size = new Size(82, 24);
            label_storage_path.TabIndex = 2;
            label_storage_path.Text = "存储路径";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(829, 590);
            Controls.Add(groupBox_storage_settings);
            Controls.Add(groupBox_realtime_log);
            Controls.Add(groupBox_connection_info);
            Name = "Form1";
            Text = "STa Tool";
            groupBox_connection_info.ResumeLayout(false);
            groupBox_connection_info.PerformLayout();
            groupBox_realtime_log.ResumeLayout(false);
            groupBox_realtime_log.PerformLayout();
            groupBox_storage_settings.ResumeLayout(false);
            groupBox_storage_settings.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox_connection_info;
        private ComboBox comboBox_ip;
        private Label label_ip;
        private Label label_port;
        private ComboBox comboBox_port;
        private Button button_disconnect;
        private Button button_connect;
        private GroupBox groupBox_realtime_log;
        private TextBox textBox_realtime_log;
        private Button button_clear_log;
        private GroupBox groupBox_storage_settings;
        private Button button_browse;
        private TextBox textBox_storage_path;
        private Label label_storage_path;
    }
}
