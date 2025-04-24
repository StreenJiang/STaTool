using log4net;
using STaTool.db.models;
using STaTool.Extensions;
using STaTool.tasks;
using STaTool.utils;

namespace STaTool {
    public partial class MainForm: Form {
        private readonly ILog log;
        private Sta6000PlusTask sta6000PlusTask;
        private readonly Config config;

        public MainForm() {
            // Initialize log
            log = LogManager.GetLogger(GetType());

            // Initialize component
            InitializeComponent();

            // Load config
            config = ConfigFileUtil.LoadConfig();

            // Initialize data
            if (config.Ip.Count > 0) {
                comboBox_ip.Items.AddRange(config.Ip.ToArray());
                comboBox_ip.SelectedIndex = 0;
            }
            if (config.Port.Count > 0) {
                comboBox_port.Items.AddRange(config.Port.Cast<object>().ToArray());
                comboBox_port.SelectedIndex = 0;
            }
            if (!string.IsNullOrEmpty(config.StoragePath)) {
                textBox_storage_path.Text = config.StoragePath;
            }

            // Add event listener
            comboBox_ip.SelectedIndexChanged += ComboBox_Ip_SelectedIndexChanged;
            button_connect.Click += ButtonConnect_Click;
            button_disconnect.Click += BUttonDisConnect_Click;
            button_clear_log.Click += ButtonClearLog_Click;

            // Initialize log
            WidgetUtils.TextBox_realtime_log = textBox_realtime_log;
            WidgetUtils.AppendMsg("等待连接...");

            // 测试
            var tighteningDataList = new List<TighteningData> {
                new() {
                    tool_ip = "192.168.1.1",
                    tool_port = 1234,
                    cell_id = 1,
                    channel_id = 1,
                    torque_controller_name = "TC1",
                    vin_number = "VIN1234567890",
                }
            };
            List<string> headers = new List<string> {
                "IP地址",
                "端口号",
                "cell_id",
                "channel_id",
                "torque_controller_name",
                "vin_number",
            };

            if (ConfigFileUtil.IsExcelFileLocked("D:/Tightening data/data.xlsx")) {
                WidgetUtils.AppendMsg("Excel文件被锁定，请先关闭Excel程序");
                WidgetUtils.ShowWarningPopUp("Excel文件被锁定，请先关闭Excel程序");
            } else {
                tighteningDataList.ExportToExcelFile(headers, "D:/Tightening data/data.xlsx");
                tighteningDataList.ExportToTextFileAsync(headers, "D:/Tightening data/data.txt");
            }
        }

        private void ComboBox_Ip_SelectedIndexChanged(object? sender, EventArgs e) {
            comboBox_port.SelectedIndex = comboBox_ip.SelectedIndex;
        }

        private void ButtonConnect_Click(object? sender, EventArgs e) {
            string filePath = textBox_storage_path.Text;
            if (!ConfigFileUtil.IsPathValid(filePath)) {
                WidgetUtils.SetError(textBox_storage_path, "请输入正确的存储路径");
                return;
            } else {
                WidgetUtils.SetError(textBox_storage_path, "");
            }

            try {
                object ipTemp = comboBox_ip.SelectedItem ?? comboBox_ip.Text;
                object portTemp = comboBox_port.SelectedItem ?? comboBox_port.Text;

                if (!ArgumentValidator.ValidateIPv4(ipTemp)) {
                    WidgetUtils.SetError(comboBox_ip, "请输入正确的IPv4地址");
                    return;
                } else {
                    WidgetUtils.SetError(comboBox_ip, "");
                }

                if (!ArgumentValidator.ValidatePortInWindows(portTemp)) {
                    WidgetUtils.SetError(comboBox_port, "请输入正确的端口号");
                    return;
                } else {
                    WidgetUtils.SetError(comboBox_port, "");
                }

                var ip = ipTemp.ToString();
                var port = int.Parse(portTemp.ToString());
                sta6000PlusTask = new Sta6000PlusTask(ip, port);

                button_connect.Enabled = false;
                comboBox_ip.Enabled = false;
                comboBox_port.Enabled = false;
                button_disconnect.Enabled = true;

                // Save config
                if (!config.Ip.Contains(ip) && !config.Port.Contains(port)) {
                    config.Ip.Enqueue(ip);
                    config.Port.Enqueue(port);
                }
                // Remove the oldest one if the queue is greater than 5
                if (config.Ip.Count > 5) {
                    config.Ip.Dequeue();
                }
                if (config.Port.Count > 5) {
                    config.Port.Dequeue();
                }
                config.StoragePath = filePath;
                ConfigFileUtil.SaveConfig(config);

                // Add to combo box
                comboBox_ip.Items.Clear();
                comboBox_port.Items.Clear();
                comboBox_ip.Items.AddRange(config.Ip.ToArray());
                comboBox_port.Items.AddRange(config.Port.Cast<object>().ToArray());
            } catch (Exception er) {
                log.Warn($"Connect failed, e = {er}");
            }
        }

        private void BUttonDisConnect_Click(object? sender, EventArgs e) {
            sta6000PlusTask.IsConnected = false;

            button_disconnect.Enabled = false;
            button_connect.Enabled = true;
            comboBox_ip.Enabled = true;
            comboBox_port.Enabled = true;
        }

        private void ButtonClearLog_Click(object? sender, EventArgs e) {
            textBox_realtime_log.Clear();
        }
    }
}
