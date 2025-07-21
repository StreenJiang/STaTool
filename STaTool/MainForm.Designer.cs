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
            groupBox_connection_info.SuspendLayout();
            groupBox_realtime_log.SuspendLayout();
            groupBox_storage_settings.SuspendLayout();
            groupBox_curve_settings.SuspendLayout();
            groupBox_plc_settings.SuspendLayout();
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
            comboBox_ip.Location = new Point(28, 67);
            comboBox_ip.Name = "comboBox_ip";
            comboBox_ip.Size = new Size(295, 32);
            comboBox_ip.TabIndex = 0;
            // 
            // comboBox_port
            // 
            comboBox_port.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_port.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_port.FormattingEnabled = true;
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
            button_connect.TabIndex = 2;
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
            groupBox_realtime_log.Location = new Point(12, 868);
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
            textBox_realtime_log.BackColor = SystemColors.ButtonHighlight;
            textBox_realtime_log.BorderStyle = BorderStyle.FixedSingle;
            textBox_realtime_log.Location = new Point(28, 58);
            textBox_realtime_log.Multiline = true;
            textBox_realtime_log.Name = "textBox_realtime_log";
            textBox_realtime_log.ReadOnly = true;
            textBox_realtime_log.ScrollBars = ScrollBars.Vertical;
            textBox_realtime_log.Size = new Size(749, 203);
            textBox_realtime_log.TabIndex = 0;
            textBox_realtime_log.TabStop = false;
            // 
            // groupBox_storage_settings
            // 
            groupBox_storage_settings.Controls.Add(button_browse);
            groupBox_storage_settings.Controls.Add(textBox_storage_path);
            groupBox_storage_settings.Controls.Add(label_storage_path);
            groupBox_storage_settings.Location = new Point(12, 730);
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
            groupBox_curve_settings.Location = new Point(12, 154);
            groupBox_curve_settings.Name = "groupBox_curve_settings";
            groupBox_curve_settings.Size = new Size(805, 570);
            groupBox_curve_settings.TabIndex = 3;
            groupBox_curve_settings.TabStop = false;
            groupBox_curve_settings.Text = "曲线抓取配置";
            // 
            // label_close_btn_img
            // 
            label_close_btn_img.AutoSize = true;
            label_close_btn_img.Location = new Point(433, 255);
            label_close_btn_img.Name = "label_close_btn_img";
            label_close_btn_img.Size = new Size(154, 24);
            label_close_btn_img.TabIndex = 2;
            label_close_btn_img.Text = "【关闭】按钮图片";
            // 
            // label_yes_btn_img
            // 
            label_yes_btn_img.AutoSize = true;
            label_yes_btn_img.Location = new Point(433, 190);
            label_yes_btn_img.Name = "label_yes_btn_img";
            label_yes_btn_img.Size = new Size(190, 24);
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
            groupBox_plc_settings.Location = new Point(28, 350);
            groupBox_plc_settings.Name = "groupBox_plc_settings";
            groupBox_plc_settings.Size = new Size(749, 136);
            groupBox_plc_settings.TabIndex = 0;
            groupBox_plc_settings.TabStop = false;
            groupBox_plc_settings.Text = "PLC配置";
            // 
            // textBox_plc_flag_pos
            // 
            textBox_plc_flag_pos.Location = new Point(647, 65);
            textBox_plc_flag_pos.Name = "textBox_plc_flag_pos";
            textBox_plc_flag_pos.Size = new Size(64, 30);
            textBox_plc_flag_pos.TabIndex = 3;
            // 
            // textBox_plc_heartbeat_pos
            // 
            textBox_plc_heartbeat_pos.Location = new Point(551, 65);
            textBox_plc_heartbeat_pos.Name = "textBox_plc_heartbeat_pos";
            textBox_plc_heartbeat_pos.Size = new Size(64, 30);
            textBox_plc_heartbeat_pos.TabIndex = 2;
            // 
            // label_plc_flag_pos
            // 
            label_plc_flag_pos.AutoSize = true;
            label_plc_flag_pos.Location = new Point(647, 38);
            label_plc_flag_pos.Name = "label_plc_flag_pos";
            label_plc_flag_pos.Size = new Size(64, 24);
            label_plc_flag_pos.TabIndex = 2;
            label_plc_flag_pos.Text = "标识位";
            // 
            // label_plc_ip
            // 
            label_plc_ip.AutoSize = true;
            label_plc_ip.Location = new Point(33, 40);
            label_plc_ip.Name = "label_plc_ip";
            label_plc_ip.Size = new Size(62, 24);
            label_plc_ip.TabIndex = 2;
            label_plc_ip.Text = "IP地址";
            // 
            // label_plc_heartbeat_pos
            // 
            label_plc_heartbeat_pos.AutoSize = true;
            label_plc_heartbeat_pos.Location = new Point(551, 38);
            label_plc_heartbeat_pos.Name = "label_plc_heartbeat_pos";
            label_plc_heartbeat_pos.Size = new Size(64, 24);
            label_plc_heartbeat_pos.TabIndex = 2;
            label_plc_heartbeat_pos.Text = "心跳位";
            // 
            // label_plc_port
            // 
            label_plc_port.AutoSize = true;
            label_plc_port.Location = new Point(365, 38);
            label_plc_port.Name = "label_plc_port";
            label_plc_port.Size = new Size(64, 24);
            label_plc_port.TabIndex = 2;
            label_plc_port.Text = "端口号";
            // 
            // comboBox_plc_ip
            // 
            comboBox_plc_ip.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_plc_ip.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_plc_ip.FormattingEnabled = true;
            comboBox_plc_ip.Location = new Point(33, 67);
            comboBox_plc_ip.Name = "comboBox_plc_ip";
            comboBox_plc_ip.Size = new Size(295, 32);
            comboBox_plc_ip.TabIndex = 0;
            // 
            // comboBox_plc_port
            // 
            comboBox_plc_port.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_plc_port.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_plc_port.FormattingEnabled = true;
            comboBox_plc_port.Location = new Point(365, 65);
            comboBox_plc_port.Name = "comboBox_plc_port";
            comboBox_plc_port.Size = new Size(146, 32);
            comboBox_plc_port.TabIndex = 1;
            // 
            // label_ok_btn_img
            // 
            label_ok_btn_img.AutoSize = true;
            label_ok_btn_img.Location = new Point(28, 255);
            label_ok_btn_img.Name = "label_ok_btn_img";
            label_ok_btn_img.Size = new Size(154, 24);
            label_ok_btn_img.TabIndex = 2;
            label_ok_btn_img.Text = "【确认】按钮图片";
            // 
            // label_save_btn_img
            // 
            label_save_btn_img.AutoSize = true;
            label_save_btn_img.Location = new Point(28, 190);
            label_save_btn_img.Name = "label_save_btn_img";
            label_save_btn_img.Size = new Size(154, 24);
            label_save_btn_img.TabIndex = 2;
            label_save_btn_img.Text = "【保存】按钮图片";
            // 
            // label_blm_btn_img
            // 
            label_blm_btn_img.AutoSize = true;
            label_blm_btn_img.Location = new Point(433, 115);
            label_blm_btn_img.Name = "label_blm_btn_img";
            label_blm_btn_img.Size = new Size(210, 24);
            label_blm_btn_img.TabIndex = 2;
            label_blm_btn_img.Text = "【BLM Curve】按钮图片";
            // 
            // label_export_btn_img
            // 
            label_export_btn_img.AutoSize = true;
            label_export_btn_img.Location = new Point(28, 117);
            label_export_btn_img.Name = "label_export_btn_img";
            label_export_btn_img.Size = new Size(197, 24);
            label_export_btn_img.TabIndex = 2;
            label_export_btn_img.Text = "【导出到crv】按钮图片";
            // 
            // label_crv_header_img
            // 
            label_crv_header_img.AutoSize = true;
            label_crv_header_img.Location = new Point(433, 43);
            label_crv_header_img.Name = "label_crv_header_img";
            label_crv_header_img.Size = new Size(154, 24);
            label_crv_header_img.TabIndex = 2;
            label_crv_header_img.Text = "【曲线】表头图片";
            // 
            // comboBox_close_btn_img
            // 
            comboBox_close_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_close_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_close_btn_img.FormattingEnabled = true;
            comboBox_close_btn_img.Location = new Point(433, 282);
            comboBox_close_btn_img.Name = "comboBox_close_btn_img";
            comboBox_close_btn_img.Size = new Size(245, 32);
            comboBox_close_btn_img.TabIndex = 1;
            // 
            // comboBox_ok_btn_img
            // 
            comboBox_ok_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_ok_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_ok_btn_img.FormattingEnabled = true;
            comboBox_ok_btn_img.Location = new Point(28, 282);
            comboBox_ok_btn_img.Name = "comboBox_ok_btn_img";
            comboBox_ok_btn_img.Size = new Size(245, 32);
            comboBox_ok_btn_img.TabIndex = 1;
            // 
            // comboBox_yes_btn_img
            // 
            comboBox_yes_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_yes_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_yes_btn_img.FormattingEnabled = true;
            comboBox_yes_btn_img.Location = new Point(433, 217);
            comboBox_yes_btn_img.Name = "comboBox_yes_btn_img";
            comboBox_yes_btn_img.Size = new Size(245, 32);
            comboBox_yes_btn_img.TabIndex = 1;
            // 
            // button_close_btn_img
            // 
            button_close_btn_img.Location = new Point(684, 280);
            button_close_btn_img.Name = "button_close_btn_img";
            button_close_btn_img.Size = new Size(93, 34);
            button_close_btn_img.TabIndex = 3;
            button_close_btn_img.Text = "截图";
            button_close_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_save_btn_img
            // 
            comboBox_save_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_save_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_save_btn_img.FormattingEnabled = true;
            comboBox_save_btn_img.Location = new Point(28, 217);
            comboBox_save_btn_img.Name = "comboBox_save_btn_img";
            comboBox_save_btn_img.Size = new Size(245, 32);
            comboBox_save_btn_img.TabIndex = 1;
            // 
            // button_yes_btn_img
            // 
            button_yes_btn_img.Location = new Point(684, 215);
            button_yes_btn_img.Name = "button_yes_btn_img";
            button_yes_btn_img.Size = new Size(93, 34);
            button_yes_btn_img.TabIndex = 3;
            button_yes_btn_img.Text = "截图";
            button_yes_btn_img.UseVisualStyleBackColor = true;
            // 
            // button_ok_btn_img
            // 
            button_ok_btn_img.Location = new Point(279, 280);
            button_ok_btn_img.Name = "button_ok_btn_img";
            button_ok_btn_img.Size = new Size(93, 34);
            button_ok_btn_img.TabIndex = 3;
            button_ok_btn_img.Text = "截图";
            button_ok_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_blm_btn_img
            // 
            comboBox_blm_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_blm_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_blm_btn_img.FormattingEnabled = true;
            comboBox_blm_btn_img.Location = new Point(433, 142);
            comboBox_blm_btn_img.Name = "comboBox_blm_btn_img";
            comboBox_blm_btn_img.Size = new Size(245, 32);
            comboBox_blm_btn_img.TabIndex = 1;
            // 
            // button_save_btn_img
            // 
            button_save_btn_img.Location = new Point(279, 215);
            button_save_btn_img.Name = "button_save_btn_img";
            button_save_btn_img.Size = new Size(93, 34);
            button_save_btn_img.TabIndex = 3;
            button_save_btn_img.Text = "截图";
            button_save_btn_img.UseVisualStyleBackColor = true;
            // 
            // label_update_btn_img
            // 
            label_update_btn_img.AutoSize = true;
            label_update_btn_img.Location = new Point(28, 45);
            label_update_btn_img.Name = "label_update_btn_img";
            label_update_btn_img.Size = new Size(154, 24);
            label_update_btn_img.TabIndex = 2;
            label_update_btn_img.Text = "【更新】按钮图片";
            // 
            // button_blm_btn_img
            // 
            button_blm_btn_img.Location = new Point(684, 140);
            button_blm_btn_img.Name = "button_blm_btn_img";
            button_blm_btn_img.Size = new Size(93, 34);
            button_blm_btn_img.TabIndex = 3;
            button_blm_btn_img.Text = "截图";
            button_blm_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_crv_header_img
            // 
            comboBox_crv_header_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_crv_header_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_crv_header_img.FormattingEnabled = true;
            comboBox_crv_header_img.Location = new Point(433, 70);
            comboBox_crv_header_img.Name = "comboBox_crv_header_img";
            comboBox_crv_header_img.Size = new Size(245, 32);
            comboBox_crv_header_img.TabIndex = 1;
            // 
            // comboBox_export_btn_img
            // 
            comboBox_export_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_export_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_export_btn_img.FormattingEnabled = true;
            comboBox_export_btn_img.Location = new Point(28, 144);
            comboBox_export_btn_img.Name = "comboBox_export_btn_img";
            comboBox_export_btn_img.Size = new Size(245, 32);
            comboBox_export_btn_img.TabIndex = 1;
            // 
            // button_crv_header_img
            // 
            button_crv_header_img.Location = new Point(684, 68);
            button_crv_header_img.Name = "button_crv_header_img";
            button_crv_header_img.Size = new Size(93, 34);
            button_crv_header_img.TabIndex = 3;
            button_crv_header_img.Text = "截图";
            button_crv_header_img.UseVisualStyleBackColor = true;
            // 
            // button_export_btn_img
            // 
            button_export_btn_img.Location = new Point(279, 142);
            button_export_btn_img.Name = "button_export_btn_img";
            button_export_btn_img.Size = new Size(93, 34);
            button_export_btn_img.TabIndex = 3;
            button_export_btn_img.Text = "截图";
            button_export_btn_img.UseVisualStyleBackColor = true;
            // 
            // comboBox_update_btn_img
            // 
            comboBox_update_btn_img.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox_update_btn_img.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox_update_btn_img.FormattingEnabled = true;
            comboBox_update_btn_img.Location = new Point(28, 72);
            comboBox_update_btn_img.Name = "comboBox_update_btn_img";
            comboBox_update_btn_img.Size = new Size(245, 32);
            comboBox_update_btn_img.TabIndex = 1;
            // 
            // button_capture_update_btn_img
            // 
            button_capture_update_btn_img.Location = new Point(279, 70);
            button_capture_update_btn_img.Name = "button_capture_update_btn_img";
            button_capture_update_btn_img.Size = new Size(93, 34);
            button_capture_update_btn_img.TabIndex = 3;
            button_capture_update_btn_img.Text = "截图";
            button_capture_update_btn_img.UseVisualStyleBackColor = true;
            // 
            // button_start_fetch
            // 
            button_start_fetch.Location = new Point(665, 507);
            button_start_fetch.Name = "button_start_fetch";
            button_start_fetch.Size = new Size(112, 34);
            button_start_fetch.TabIndex = 3;
            button_start_fetch.Text = "开始抓取";
            button_start_fetch.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            ClientSize = new Size(829, 1168);
            Controls.Add(groupBox_curve_settings);
            Controls.Add(groupBox_storage_settings);
            Controls.Add(groupBox_realtime_log);
            Controls.Add(groupBox_connection_info);
            FormBorderStyle = FormBorderStyle.FixedSingle;
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
    }
}
