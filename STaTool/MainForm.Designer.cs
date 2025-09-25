namespace STaTool {
    partial class MainForm {
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
            groupBox_curve_settings = new GroupBox();
            label_close_btn_img = new Label();
            label_yes_btn_img = new Label();
            groupBox_plc_settings = new GroupBox();
            textBox_plc_flag_pos = new TextBox();
            textBox_plc_heartbeat_pos = new TextBox();
            label_plc_flag_pos = new Label();
            label_plc_ip = new Label();
            label_plc_heartbeat_pos = new Label();
            label_plc_port = new Label();
            comboBox_plc_ip = new ComboBox();
            comboBox_plc_port = new ComboBox();
            label_ok_btn_img = new Label();
            label_save_btn_img = new Label();
            label_blm_btn_img = new Label();
            label_export_btn_img = new Label();
            label_crv_header_img = new Label();
            comboBox_close_btn_img = new ComboBox();
            comboBox_ok_btn_img = new ComboBox();
            comboBox_yes_btn_img = new ComboBox();
            button_close_btn_img = new Button();
            comboBox_save_btn_img = new ComboBox();
            button_yes_btn_img = new Button();
            button_ok_btn_img = new Button();
            comboBox_blm_btn_img = new ComboBox();
            button_save_btn_img = new Button();
            label_update_btn_img = new Label();
            button_blm_btn_img = new Button();
            comboBox_crv_header_img = new ComboBox();
            comboBox_export_btn_img = new ComboBox();
            button_crv_header_img = new Button();
            button_export_btn_img = new Button();
            comboBox_update_btn_img = new ComboBox();
            button_capture_update_btn_img = new Button();
            button_start_fetch = new Button();
            checkBox_auto_startup = new CheckBox();
            checkBox_auto_fetch = new CheckBox();
            groupBox1 = new GroupBox();
            groupBox_connection_info.SuspendLayout();
            groupBox_realtime_log.SuspendLayout();
            groupBox_storage_settings.SuspendLayout();
            groupBox_curve_settings.SuspendLayout();
            groupBox_plc_settings.SuspendLayout();
            groupBox1.SuspendLayout();
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
            groupBox_connection_info.Location = new Point(8, 8);
            groupBox_connection_info.Margin = new Padding(2);
            groupBox_connection_info.Name = "groupBox_connection_info";
            groupBox_connection_info.Padding = new Padding(2);
            groupBox_connection_info.Size = new Size(512, 96);
            groupBox_connection_info.TabIndex = 0;
            groupBox_connection_info.TabStop = false;
            groupBox_connection_info.Text = "连接信息";
            // 
            // label_ip
            // 
            label_ip.AutoSize = true;
            label_ip.Location = new Point(18, 28);
            label_ip.Margin = new Padding(2, 0, 2, 0);
            label_ip.Name = "label_ip";
            label_ip.Size = new Size(43, 17);
            label_ip.TabIndex = 2;
            label_ip.Text = "IP地址";
            // 
            // label_port
            // 
            label_port.AutoSize = true;
            label_port.Location = new Point(225, 28);
            label_port.Margin = new Padding(2, 0, 2, 0);
            label_port.Name = "label_port";
            label_port.Size = new Size(44, 17);
            label_port.TabIndex = 2;
            label_port.Text = "端口号";
            // 
            // comboBox_ip
            // 
            comboBox_ip.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_ip.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_ip.FormattingEnabled = true;
            comboBox_ip.Location = new Point(18, 47);
            comboBox_ip.Margin = new Padding(2);
            comboBox_ip.Name = "comboBox_ip";
            comboBox_ip.Size = new Size(189, 25);
            comboBox_ip.TabIndex = 0;
            // 
            // comboBox_port
            // 
            comboBox_port.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_port.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_port.FormattingEnabled = true;
            comboBox_port.Location = new Point(225, 47);
            comboBox_port.Margin = new Padding(2);
            comboBox_port.Name = "comboBox_port";
            comboBox_port.Size = new Size(94, 25);
            comboBox_port.TabIndex = 1;
            // 
            // button_connect
            // 
            button_connect.Location = new Point(342, 46);
            button_connect.Margin = new Padding(2);
            button_connect.Name = "button_connect";
            button_connect.Size = new Size(71, 24);
            button_connect.TabIndex = 2;
            button_connect.Text = "连接";
            button_connect.UseVisualStyleBackColor = true;
            // 
            // button_disconnect
            // 
            button_disconnect.Enabled = false;
            button_disconnect.Location = new Point(423, 46);
            button_disconnect.Margin = new Padding(2);
            button_disconnect.Name = "button_disconnect";
            button_disconnect.Size = new Size(71, 24);
            button_disconnect.TabIndex = 3;
            button_disconnect.Text = "断开";
            button_disconnect.UseVisualStyleBackColor = true;
            // 
            // groupBox_realtime_log
            // 
            groupBox_realtime_log.Controls.Add(button_clear_log);
            groupBox_realtime_log.Controls.Add(textBox_realtime_log);
            groupBox_realtime_log.Location = new Point(529, 169);
            groupBox_realtime_log.Margin = new Padding(2);
            groupBox_realtime_log.Name = "groupBox_realtime_log";
            groupBox_realtime_log.Padding = new Padding(2);
            groupBox_realtime_log.Size = new Size(512, 203);
            groupBox_realtime_log.TabIndex = 1;
            groupBox_realtime_log.TabStop = false;
            groupBox_realtime_log.Text = "实时日志";
            // 
            // button_clear_log
            // 
            button_clear_log.Location = new Point(423, 13);
            button_clear_log.Margin = new Padding(2);
            button_clear_log.Name = "button_clear_log";
            button_clear_log.Size = new Size(71, 24);
            button_clear_log.TabIndex = 1;
            button_clear_log.Text = "清除";
            button_clear_log.UseVisualStyleBackColor = true;
            // 
            // textBox_realtime_log
            // 
            textBox_realtime_log.BackColor = SystemColors.ButtonHighlight;
            textBox_realtime_log.BorderStyle = BorderStyle.FixedSingle;
            textBox_realtime_log.Location = new Point(18, 41);
            textBox_realtime_log.Margin = new Padding(2);
            textBox_realtime_log.Multiline = true;
            textBox_realtime_log.Name = "textBox_realtime_log";
            textBox_realtime_log.ReadOnly = true;
            textBox_realtime_log.ScrollBars = ScrollBars.Vertical;
            textBox_realtime_log.Size = new Size(477, 144);
            textBox_realtime_log.TabIndex = 0;
            textBox_realtime_log.TabStop = false;
            // 
            // groupBox_storage_settings
            // 
            groupBox_storage_settings.Controls.Add(button_browse);
            groupBox_storage_settings.Controls.Add(textBox_storage_path);
            groupBox_storage_settings.Controls.Add(label_storage_path);
            groupBox_storage_settings.Location = new Point(529, 71);
            groupBox_storage_settings.Margin = new Padding(2);
            groupBox_storage_settings.Name = "groupBox_storage_settings";
            groupBox_storage_settings.Padding = new Padding(2);
            groupBox_storage_settings.Size = new Size(512, 94);
            groupBox_storage_settings.TabIndex = 2;
            groupBox_storage_settings.TabStop = false;
            groupBox_storage_settings.Text = "存储配置";
            // 
            // button_browse
            // 
            button_browse.Location = new Point(423, 46);
            button_browse.Margin = new Padding(2);
            button_browse.Name = "button_browse";
            button_browse.Size = new Size(71, 24);
            button_browse.TabIndex = 4;
            button_browse.Text = "浏览";
            button_browse.UseVisualStyleBackColor = true;
            // 
            // textBox_storage_path
            // 
            textBox_storage_path.Location = new Point(18, 47);
            textBox_storage_path.Margin = new Padding(2);
            textBox_storage_path.Name = "textBox_storage_path";
            textBox_storage_path.Size = new Size(397, 23);
            textBox_storage_path.TabIndex = 3;
            // 
            // label_storage_path
            // 
            label_storage_path.AutoSize = true;
            label_storage_path.Location = new Point(18, 28);
            label_storage_path.Margin = new Padding(2, 0, 2, 0);
            label_storage_path.Name = "label_storage_path";
            label_storage_path.Size = new Size(56, 17);
            label_storage_path.TabIndex = 2;
            label_storage_path.Text = "存储路径";
            // 
            // groupBox_curve_settings
            // 
            groupBox_curve_settings.Controls.Add(label_close_btn_img);
            groupBox_curve_settings.Controls.Add(label_yes_btn_img);
            groupBox_curve_settings.Controls.Add(groupBox_plc_settings);
            groupBox_curve_settings.Controls.Add(label_ok_btn_img);
            groupBox_curve_settings.Controls.Add(label_save_btn_img);
            groupBox_curve_settings.Controls.Add(label_blm_btn_img);
            groupBox_curve_settings.Controls.Add(label_export_btn_img);
            groupBox_curve_settings.Controls.Add(label_crv_header_img);
            groupBox_curve_settings.Controls.Add(comboBox_close_btn_img);
            groupBox_curve_settings.Controls.Add(comboBox_ok_btn_img);
            groupBox_curve_settings.Controls.Add(comboBox_yes_btn_img);
            groupBox_curve_settings.Controls.Add(button_close_btn_img);
            groupBox_curve_settings.Controls.Add(comboBox_save_btn_img);
            groupBox_curve_settings.Controls.Add(button_yes_btn_img);
            groupBox_curve_settings.Controls.Add(button_ok_btn_img);
            groupBox_curve_settings.Controls.Add(comboBox_blm_btn_img);
            groupBox_curve_settings.Controls.Add(button_save_btn_img);
            groupBox_curve_settings.Controls.Add(label_update_btn_img);
            groupBox_curve_settings.Controls.Add(button_blm_btn_img);
            groupBox_curve_settings.Controls.Add(comboBox_crv_header_img);
            groupBox_curve_settings.Controls.Add(comboBox_export_btn_img);
            groupBox_curve_settings.Controls.Add(button_crv_header_img);
            groupBox_curve_settings.Controls.Add(button_export_btn_img);
            groupBox_curve_settings.Controls.Add(comboBox_update_btn_img);
            groupBox_curve_settings.Controls.Add(button_capture_update_btn_img);
            groupBox_curve_settings.Controls.Add(button_start_fetch);
            groupBox_curve_settings.Location = new Point(8, 109);
            groupBox_curve_settings.Margin = new Padding(2);
            groupBox_curve_settings.Name = "groupBox_curve_settings";
            groupBox_curve_settings.Padding = new Padding(2);
            groupBox_curve_settings.Size = new Size(512, 365);
            groupBox_curve_settings.TabIndex = 3;
            groupBox_curve_settings.TabStop = false;
            groupBox_curve_settings.Text = "曲线抓取配置";
            // 
            // label_close_btn_img
            // 
            label_close_btn_img.AutoSize = true;
            label_close_btn_img.Location = new Point(276, 181);
            label_close_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_close_btn_img.Name = "label_close_btn_img";
            label_close_btn_img.Size = new Size(104, 17);
            label_close_btn_img.TabIndex = 2;
            label_close_btn_img.Text = "【关闭】按钮图片";
            // 
            // label_yes_btn_img
            // 
            label_yes_btn_img.AutoSize = true;
            label_yes_btn_img.Location = new Point(276, 135);
            label_yes_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_yes_btn_img.Name = "label_yes_btn_img";
            label_yes_btn_img.Size = new Size(128, 17);
            label_yes_btn_img.TabIndex = 2;
            label_yes_btn_img.Text = "【是否替换】按钮图片";
            // 
            // groupBox_plc_settings
            // 
            groupBox_plc_settings.Controls.Add(textBox_plc_flag_pos);
            groupBox_plc_settings.Controls.Add(textBox_plc_heartbeat_pos);
            groupBox_plc_settings.Controls.Add(label_plc_flag_pos);
            groupBox_plc_settings.Controls.Add(label_plc_ip);
            groupBox_plc_settings.Controls.Add(label_plc_heartbeat_pos);
            groupBox_plc_settings.Controls.Add(label_plc_port);
            groupBox_plc_settings.Controls.Add(comboBox_plc_ip);
            groupBox_plc_settings.Controls.Add(comboBox_plc_port);
            groupBox_plc_settings.Location = new Point(18, 236);
            groupBox_plc_settings.Margin = new Padding(2);
            groupBox_plc_settings.Name = "groupBox_plc_settings";
            groupBox_plc_settings.Padding = new Padding(2);
            groupBox_plc_settings.Size = new Size(477, 84);
            groupBox_plc_settings.TabIndex = 0;
            groupBox_plc_settings.TabStop = false;
            groupBox_plc_settings.Text = "PLC配置";
            // 
            // textBox_plc_flag_pos
            // 
            textBox_plc_flag_pos.Location = new Point(412, 42);
            textBox_plc_flag_pos.Margin = new Padding(2);
            textBox_plc_flag_pos.Name = "textBox_plc_flag_pos";
            textBox_plc_flag_pos.Size = new Size(42, 23);
            textBox_plc_flag_pos.TabIndex = 3;
            // 
            // textBox_plc_heartbeat_pos
            // 
            textBox_plc_heartbeat_pos.Location = new Point(351, 42);
            textBox_plc_heartbeat_pos.Margin = new Padding(2);
            textBox_plc_heartbeat_pos.Name = "textBox_plc_heartbeat_pos";
            textBox_plc_heartbeat_pos.Size = new Size(42, 23);
            textBox_plc_heartbeat_pos.TabIndex = 2;
            // 
            // label_plc_flag_pos
            // 
            label_plc_flag_pos.AutoSize = true;
            label_plc_flag_pos.Location = new Point(412, 23);
            label_plc_flag_pos.Margin = new Padding(2, 0, 2, 0);
            label_plc_flag_pos.Name = "label_plc_flag_pos";
            label_plc_flag_pos.Size = new Size(44, 17);
            label_plc_flag_pos.TabIndex = 2;
            label_plc_flag_pos.Text = "标识位";
            // 
            // label_plc_ip
            // 
            label_plc_ip.AutoSize = true;
            label_plc_ip.Location = new Point(21, 25);
            label_plc_ip.Margin = new Padding(2, 0, 2, 0);
            label_plc_ip.Name = "label_plc_ip";
            label_plc_ip.Size = new Size(43, 17);
            label_plc_ip.TabIndex = 2;
            label_plc_ip.Text = "IP地址";
            // 
            // label_plc_heartbeat_pos
            // 
            label_plc_heartbeat_pos.AutoSize = true;
            label_plc_heartbeat_pos.Location = new Point(351, 23);
            label_plc_heartbeat_pos.Margin = new Padding(2, 0, 2, 0);
            label_plc_heartbeat_pos.Name = "label_plc_heartbeat_pos";
            label_plc_heartbeat_pos.Size = new Size(44, 17);
            label_plc_heartbeat_pos.TabIndex = 2;
            label_plc_heartbeat_pos.Text = "心跳位";
            // 
            // label_plc_port
            // 
            label_plc_port.AutoSize = true;
            label_plc_port.Location = new Point(232, 23);
            label_plc_port.Margin = new Padding(2, 0, 2, 0);
            label_plc_port.Name = "label_plc_port";
            label_plc_port.Size = new Size(44, 17);
            label_plc_port.TabIndex = 2;
            label_plc_port.Text = "端口号";
            // 
            // comboBox_plc_ip
            // 
            comboBox_plc_ip.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_plc_ip.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_plc_ip.FormattingEnabled = true;
            comboBox_plc_ip.Location = new Point(21, 44);
            comboBox_plc_ip.Margin = new Padding(2);
            comboBox_plc_ip.Name = "comboBox_plc_ip";
            comboBox_plc_ip.Size = new Size(189, 25);
            comboBox_plc_ip.TabIndex = 0;
            // 
            // comboBox_plc_port
            // 
            comboBox_plc_port.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_plc_port.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_plc_port.FormattingEnabled = true;
            comboBox_plc_port.Location = new Point(232, 42);
            comboBox_plc_port.Margin = new Padding(2);
            comboBox_plc_port.Name = "comboBox_plc_port";
            comboBox_plc_port.Size = new Size(94, 25);
            comboBox_plc_port.TabIndex = 1;
            // 
            // label_ok_btn_img
            // 
            label_ok_btn_img.AutoSize = true;
            label_ok_btn_img.Location = new Point(18, 181);
            label_ok_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_ok_btn_img.Name = "label_ok_btn_img";
            label_ok_btn_img.Size = new Size(104, 17);
            label_ok_btn_img.TabIndex = 2;
            label_ok_btn_img.Text = "【确认】按钮图片";
            // 
            // label_save_btn_img
            // 
            label_save_btn_img.AutoSize = true;
            label_save_btn_img.Location = new Point(18, 135);
            label_save_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_save_btn_img.Name = "label_save_btn_img";
            label_save_btn_img.Size = new Size(104, 17);
            label_save_btn_img.TabIndex = 2;
            label_save_btn_img.Text = "【保存】按钮图片";
            // 
            // label_blm_btn_img
            // 
            label_blm_btn_img.AutoSize = true;
            label_blm_btn_img.Location = new Point(276, 81);
            label_blm_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_blm_btn_img.Name = "label_blm_btn_img";
            label_blm_btn_img.Size = new Size(143, 17);
            label_blm_btn_img.TabIndex = 2;
            label_blm_btn_img.Text = "【BLM Curve】按钮图片";
            // 
            // label_export_btn_img
            // 
            label_export_btn_img.AutoSize = true;
            label_export_btn_img.Location = new Point(18, 83);
            label_export_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_export_btn_img.Name = "label_export_btn_img";
            label_export_btn_img.Size = new Size(133, 17);
            label_export_btn_img.TabIndex = 2;
            label_export_btn_img.Text = "【导出到crv】按钮图片";
            // 
            // label_crv_header_img
            // 
            label_crv_header_img.AutoSize = true;
            label_crv_header_img.Location = new Point(276, 30);
            label_crv_header_img.Margin = new Padding(2, 0, 2, 0);
            label_crv_header_img.Name = "label_crv_header_img";
            label_crv_header_img.Size = new Size(104, 17);
            label_crv_header_img.TabIndex = 2;
            label_crv_header_img.Text = "【曲线】表头图片";
            // 
            // comboBox_close_btn_img
            // 
            comboBox_close_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_close_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_close_btn_img.FormattingEnabled = true;
            comboBox_close_btn_img.Location = new Point(276, 200);
            comboBox_close_btn_img.Margin = new Padding(2);
            comboBox_close_btn_img.Name = "comboBox_close_btn_img";
            comboBox_close_btn_img.Size = new Size(157, 25);
            comboBox_close_btn_img.TabIndex = 1;
            // 
            // comboBox_ok_btn_img
            // 
            comboBox_ok_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_ok_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_ok_btn_img.FormattingEnabled = true;
            comboBox_ok_btn_img.Location = new Point(18, 200);
            comboBox_ok_btn_img.Margin = new Padding(2);
            comboBox_ok_btn_img.Name = "comboBox_ok_btn_img";
            comboBox_ok_btn_img.Size = new Size(157, 25);
            comboBox_ok_btn_img.TabIndex = 1;
            // 
            // comboBox_yes_btn_img
            // 
            comboBox_yes_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_yes_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_yes_btn_img.FormattingEnabled = true;
            comboBox_yes_btn_img.Location = new Point(276, 154);
            comboBox_yes_btn_img.Margin = new Padding(2);
            comboBox_yes_btn_img.Name = "comboBox_yes_btn_img";
            comboBox_yes_btn_img.Size = new Size(157, 25);
            comboBox_yes_btn_img.TabIndex = 1;
            // 
            // button_close_btn_img
            // 
            button_close_btn_img.Location = new Point(435, 198);
            button_close_btn_img.Margin = new Padding(2);
            button_close_btn_img.Name = "button_close_btn_img";
            button_close_btn_img.Size = new Size(59, 24);
            button_close_btn_img.TabIndex = 3;
            button_close_btn_img.Text = "截图";
            button_close_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_save_btn_img
            // 
            comboBox_save_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_save_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_save_btn_img.FormattingEnabled = true;
            comboBox_save_btn_img.Location = new Point(18, 154);
            comboBox_save_btn_img.Margin = new Padding(2);
            comboBox_save_btn_img.Name = "comboBox_save_btn_img";
            comboBox_save_btn_img.Size = new Size(157, 25);
            comboBox_save_btn_img.TabIndex = 1;
            // 
            // button_yes_btn_img
            // 
            button_yes_btn_img.Location = new Point(435, 152);
            button_yes_btn_img.Margin = new Padding(2);
            button_yes_btn_img.Name = "button_yes_btn_img";
            button_yes_btn_img.Size = new Size(59, 24);
            button_yes_btn_img.TabIndex = 3;
            button_yes_btn_img.Text = "截图";
            button_yes_btn_img.UseVisualStyleBackColor = true;
            // 
            // button_ok_btn_img
            // 
            button_ok_btn_img.Location = new Point(178, 198);
            button_ok_btn_img.Margin = new Padding(2);
            button_ok_btn_img.Name = "button_ok_btn_img";
            button_ok_btn_img.Size = new Size(59, 24);
            button_ok_btn_img.TabIndex = 3;
            button_ok_btn_img.Text = "截图";
            button_ok_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_blm_btn_img
            // 
            comboBox_blm_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_blm_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_blm_btn_img.FormattingEnabled = true;
            comboBox_blm_btn_img.Location = new Point(276, 101);
            comboBox_blm_btn_img.Margin = new Padding(2);
            comboBox_blm_btn_img.Name = "comboBox_blm_btn_img";
            comboBox_blm_btn_img.Size = new Size(157, 25);
            comboBox_blm_btn_img.TabIndex = 1;
            // 
            // button_save_btn_img
            // 
            button_save_btn_img.Location = new Point(178, 152);
            button_save_btn_img.Margin = new Padding(2);
            button_save_btn_img.Name = "button_save_btn_img";
            button_save_btn_img.Size = new Size(59, 24);
            button_save_btn_img.TabIndex = 3;
            button_save_btn_img.Text = "截图";
            button_save_btn_img.UseVisualStyleBackColor = true;
            // 
            // label_update_btn_img
            // 
            label_update_btn_img.AutoSize = true;
            label_update_btn_img.Location = new Point(18, 32);
            label_update_btn_img.Margin = new Padding(2, 0, 2, 0);
            label_update_btn_img.Name = "label_update_btn_img";
            label_update_btn_img.Size = new Size(104, 17);
            label_update_btn_img.TabIndex = 2;
            label_update_btn_img.Text = "【更新】按钮图片";
            // 
            // button_blm_btn_img
            // 
            button_blm_btn_img.Location = new Point(435, 99);
            button_blm_btn_img.Margin = new Padding(2);
            button_blm_btn_img.Name = "button_blm_btn_img";
            button_blm_btn_img.Size = new Size(59, 24);
            button_blm_btn_img.TabIndex = 3;
            button_blm_btn_img.Text = "截图";
            button_blm_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_crv_header_img
            // 
            comboBox_crv_header_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_crv_header_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_crv_header_img.FormattingEnabled = true;
            comboBox_crv_header_img.Location = new Point(276, 50);
            comboBox_crv_header_img.Margin = new Padding(2);
            comboBox_crv_header_img.Name = "comboBox_crv_header_img";
            comboBox_crv_header_img.Size = new Size(157, 25);
            comboBox_crv_header_img.TabIndex = 1;
            // 
            // comboBox_export_btn_img
            // 
            comboBox_export_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_export_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_export_btn_img.FormattingEnabled = true;
            comboBox_export_btn_img.Location = new Point(18, 102);
            comboBox_export_btn_img.Margin = new Padding(2);
            comboBox_export_btn_img.Name = "comboBox_export_btn_img";
            comboBox_export_btn_img.Size = new Size(157, 25);
            comboBox_export_btn_img.TabIndex = 1;
            // 
            // button_crv_header_img
            // 
            button_crv_header_img.Location = new Point(435, 48);
            button_crv_header_img.Margin = new Padding(2);
            button_crv_header_img.Name = "button_crv_header_img";
            button_crv_header_img.Size = new Size(59, 24);
            button_crv_header_img.TabIndex = 3;
            button_crv_header_img.Text = "截图";
            button_crv_header_img.UseVisualStyleBackColor = true;
            // 
            // button_export_btn_img
            // 
            button_export_btn_img.Location = new Point(178, 101);
            button_export_btn_img.Margin = new Padding(2);
            button_export_btn_img.Name = "button_export_btn_img";
            button_export_btn_img.Size = new Size(59, 24);
            button_export_btn_img.TabIndex = 3;
            button_export_btn_img.Text = "截图";
            button_export_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_update_btn_img
            // 
            comboBox_update_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_update_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_update_btn_img.FormattingEnabled = true;
            comboBox_update_btn_img.Location = new Point(18, 51);
            comboBox_update_btn_img.Margin = new Padding(2);
            comboBox_update_btn_img.Name = "comboBox_update_btn_img";
            comboBox_update_btn_img.Size = new Size(157, 25);
            comboBox_update_btn_img.TabIndex = 1;
            // 
            // button_capture_update_btn_img
            // 
            button_capture_update_btn_img.Location = new Point(178, 50);
            button_capture_update_btn_img.Margin = new Padding(2);
            button_capture_update_btn_img.Name = "button_capture_update_btn_img";
            button_capture_update_btn_img.Size = new Size(59, 24);
            button_capture_update_btn_img.TabIndex = 3;
            button_capture_update_btn_img.Text = "截图";
            button_capture_update_btn_img.UseVisualStyleBackColor = true;
            // 
            // button_start_fetch
            // 
            button_start_fetch.Font = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            button_start_fetch.Location = new Point(423, 331);
            button_start_fetch.Margin = new Padding(2);
            button_start_fetch.Name = "button_start_fetch";
            button_start_fetch.Size = new Size(71, 24);
            button_start_fetch.TabIndex = 3;
            button_start_fetch.Text = "开始抓取";
            button_start_fetch.UseVisualStyleBackColor = true;
            // 
            // checkBox_auto_startup
            // 
            checkBox_auto_startup.AutoSize = true;
            checkBox_auto_startup.Location = new Point(18, 28);
            checkBox_auto_startup.Margin = new Padding(2);
            checkBox_auto_startup.Name = "checkBox_auto_startup";
            checkBox_auto_startup.Size = new Size(99, 21);
            checkBox_auto_startup.TabIndex = 0;
            checkBox_auto_startup.Text = "开机自动启动";
            checkBox_auto_startup.UseVisualStyleBackColor = true;
            // 
            // checkBox_auto_fetch
            // 
            checkBox_auto_fetch.AutoSize = true;
            checkBox_auto_fetch.Location = new Point(127, 28);
            checkBox_auto_fetch.Margin = new Padding(2);
            checkBox_auto_fetch.Name = "checkBox_auto_fetch";
            checkBox_auto_fetch.Size = new Size(99, 21);
            checkBox_auto_fetch.TabIndex = 1;
            checkBox_auto_fetch.Text = "启动自动抓取";
            checkBox_auto_fetch.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(checkBox_auto_fetch);
            groupBox1.Controls.Add(checkBox_auto_startup);
            groupBox1.Location = new Point(529, 8);
            groupBox1.Margin = new Padding(2);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(2);
            groupBox1.Size = new Size(512, 59);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "其他设置";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(1053, 483);
            Controls.Add(groupBox1);
            Controls.Add(groupBox_curve_settings);
            Controls.Add(groupBox_storage_settings);
            Controls.Add(groupBox_realtime_log);
            Controls.Add(groupBox_connection_info);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2);
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "STa Tool";
            groupBox_connection_info.ResumeLayout(false);
            groupBox_connection_info.PerformLayout();
            groupBox_realtime_log.ResumeLayout(false);
            groupBox_realtime_log.PerformLayout();
            groupBox_storage_settings.ResumeLayout(false);
            groupBox_storage_settings.PerformLayout();
            groupBox_curve_settings.ResumeLayout(false);
            groupBox_curve_settings.PerformLayout();
            groupBox_plc_settings.ResumeLayout(false);
            groupBox_plc_settings.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
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
        private GroupBox groupBox_curve_settings;
        private Label label_update_btn_img;
        private ComboBox comboBox_update_btn_img;
        private Button button_start_fetch;
        private Button button_capture_update_btn_img;
        private Label label_blm_btn_img;
        private Label label_export_btn_img;
        private Label label_crv_header_img;
        private ComboBox comboBox_blm_btn_img;
        private Button button_blm_btn_img;
        private ComboBox comboBox_crv_header_img;
        private ComboBox comboBox_export_btn_img;
        private Button button_crv_header_img;
        private Button button_export_btn_img;
        private Label label_save_btn_img;
        private ComboBox comboBox_save_btn_img;
        private Button button_save_btn_img;
        private Label label_close_btn_img;
        private Label label_yes_btn_img;
        private Label label_ok_btn_img;
        private ComboBox comboBox_close_btn_img;
        private ComboBox comboBox_ok_btn_img;
        private ComboBox comboBox_yes_btn_img;
        private Button button_close_btn_img;
        private Button button_yes_btn_img;
        private Button button_ok_btn_img;
        private GroupBox groupBox_plc_settings;
        private Label label_plc_ip;
        private Label label_plc_heartbeat_pos;
        private Label label_plc_port;
        private ComboBox comboBox_plc_ip;
        private ComboBox comboBox_plc_port;
        private TextBox textBox_plc_heartbeat_pos;
        private TextBox textBox_plc_flag_pos;
        private Label label_plc_flag_pos;
        private CheckBox checkBox_auto_startup;
        private CheckBox checkBox_auto_fetch;
        private GroupBox groupBox1;
    }
}
