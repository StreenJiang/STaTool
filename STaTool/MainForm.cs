using log4net;
using STaTool.db.dao;
using STaTool.db.models;
using STaTool.tasks;
using STaTool.utils;

namespace STaTool {
    public partial class MainForm: Form {
        private readonly ILog log;
        private Sta6000PlusTask sta6000PlusTask;

        public MainForm() {
            // Initialize log
            log = LogManager.GetLogger(GetType());

            // Initialize component
            InitializeComponent();

            // Initialize data
            comboBox_ip.SelectedIndex = 0;
            comboBox_port.SelectedIndex = 0;

            // Add event listener
            button_connect.Click += ButtonConnect_Click;
            button_disconnect.Click += BUttonDisConnect_Click;

            var dao = new TighteningDataDao();
            var tighteningData = new TighteningData();
            tighteningData.torque = 2.888;
            dao.Insert(tighteningData);
        }

        private void ButtonConnect_Click(object? sender, EventArgs e) {
            try {
                object ipTemp = comboBox_ip.SelectedItem;
                object portTemp = comboBox_port.SelectedItem;

                if (!ArgumentValidator.ValidateIPv4(ipTemp)) {
                    WidgetUtils.SetError(comboBox_ip, "请输入正确的IPv4地址");
                    return;
                } else {
                    WidgetUtils.SetError(comboBox_ip, "");
                }

                if (!ArgumentValidator.ValidatePortInWindows(portTemp)) {
                    WidgetUtils.SetError(comboBox_ip, "请输入正确的端口号");
                    return;
                } else {
                    WidgetUtils.SetError(comboBox_ip, "");
                }

                var ip = ipTemp.ToString();
                var port = int.Parse(portTemp.ToString());
                sta6000PlusTask = new Sta6000PlusTask(ip, port);

                button_connect.Enabled = false;
                comboBox_ip.Enabled = false;
                comboBox_port.Enabled = false;
                button_disconnect.Enabled = true;
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
    }
}
