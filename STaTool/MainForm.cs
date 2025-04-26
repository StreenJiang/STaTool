using log4net;
using STaTool.constants;
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
            config = FileUtil.LoadConfig();

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
            button_browse.Click += ButtonBrowse_Click;

            // Initialize log
            WidgetUtils.TextBox_realtime_log = textBox_realtime_log;
            WidgetUtils.AppendMsg("等待连接...");
        }

        private void ComboBox_Ip_SelectedIndexChanged(object? sender, EventArgs e) {
            comboBox_port.SelectedIndex = comboBox_ip.SelectedIndex;
        }

        private void ButtonConnect_Click(object? sender, EventArgs e) {
            string pathPrefix = textBox_storage_path.Text;
            try {
                if (!FileUtil.IsPathValid(pathPrefix)) {
                    WidgetUtils.SetError(textBox_storage_path, "请输入正确的存储路径");
                    return;
                } else {
                    WidgetUtils.SetError(textBox_storage_path, "");
                }

                string folderPath = Path.Combine(pathPrefix, $"{FileUtil.Year()}", $"{FileUtil.Year()}-{FileUtil.Month()}");
                FileUtil.CheckAndCreateFolder(folderPath);
                FileHelper.CurrentPath = folderPath;

                List<string> fileTypes = FileType.GetAll();
                foreach (string type in fileTypes) {
                    string fileName = $"{FileHelper.GetFileName(type)}";
                    string filePath = Path.Combine(folderPath, fileName);
                    if (FileUtil.IsFileLocked(filePath)) {
                        WidgetUtils.AppendMsg($"{fileName}文件被锁定，请先关闭该文件");
                        WidgetUtils.ShowWarningPopUp($"{fileName}文件被锁定，请先关闭该文件");
                        return;
                    }
                }
            } catch (Exception ex) {
                WidgetUtils.AppendMsg("检查文件路径时出错，请检查文件路径是否正确。如文件路径无误，请联系管理员");
                log.Warn($"Error occurs while checking file path, e = {ex}");
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
                config.StoragePath = pathPrefix;
                FileUtil.SaveConfig(config);

                // Add to combo box
                comboBox_ip.Items.Clear();
                comboBox_port.Items.Clear();
                comboBox_ip.Items.AddRange(config.Ip.ToArray());
                comboBox_port.Items.AddRange(config.Port.Cast<object>().ToArray());
            } catch (Exception er) {
                WidgetUtils.AppendMsg("连接出错，请联系管理员");
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

        private void ButtonBrowse_Click(object? sender, EventArgs e) {
            // Create a FolderBrowserDialog instance
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = true;
            if (!string.IsNullOrEmpty(textBox_storage_path.Text)
                    && !string.IsNullOrWhiteSpace(textBox_storage_path.Text)) {
                folderDialog.SelectedPath = textBox_storage_path.Text;
            }

            // Set the dialog title (optional)
            folderDialog.Description = "请选择一个文件夹";

            // Show the dialog and check if the user clicked "OK"
            if (folderDialog.ShowDialog() == DialogResult.OK) {
                // Get the selected folder path
                string selectedFolderPath = folderDialog.SelectedPath;

                if (!FileUtil.IsPathValid(selectedFolderPath)) {
                    WidgetUtils.SetError(textBox_storage_path, "请输入正确的存储路径");
                    return;
                } else {
                    WidgetUtils.SetError(textBox_storage_path, "");
                }

                // Show the selected folder path in a message box or store it in a variable
                textBox_storage_path.Text = selectedFolderPath;
            }
        }
    }
}
