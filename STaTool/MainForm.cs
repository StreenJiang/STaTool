using log4net;
using STaTool.constants;
using STaTool.tasks;
using STaTool.utils;
using System.Drawing.Drawing2D;

namespace STaTool {
    public partial class MainForm: Form {
        private readonly ILog log;
        private Sta6000PlusTask sta6000PlusTask;
        private Config config;

        private volatile bool connecting = false;
        private volatile bool fetchingCurveData = false;

        public MainForm() {
            // Initialize log
            log = LogManager.GetLogger(GetType());

            // Initialize component
            InitializeComponent();

            // Load config
            config = FileUtil.LoadConfig();

            // Initialize auto startup checkbox
            checkBox_auto_startup.Checked = config.AutoStartup;

            // Initialize auto fetch checkbox
            checkBox_auto_fetch.Checked = config.AutoStartFetch;

            // Initialize default images from resources if Button Images folder is empty
            bool imagesInitialized = ResourceImageManager.InitializeDefaultImages();
            if (imagesInitialized) {
                WidgetUtils.AppendMsg("✅ 已初始化默认按钮图片");
            }

            // Initialize data
            if (config.Ip.Count > 0) {
                comboBox_ip.Items.AddRange(config.Ip.ToArray());
                comboBox_ip.SelectedIndex = 0;
            }
            if (config.Port.Count > 0) {
                comboBox_port.Items.AddRange(config.Port.Cast<object>().ToArray());
                comboBox_port.SelectedIndex = 0;
            }

            _loadImages(config.UpdateBtnImg, comboBox_update_btn_img, "更新按钮图片");
            _loadImages(config.CrvHeaderImg, comboBox_crv_header_img, "曲线表头图片");
            _loadImages(config.ExportBtnImg, comboBox_export_btn_img, "导出按钮图片");
            _loadImages(config.BlmBtnImg, comboBox_blm_btn_img, "BLM按钮图片");
            _loadImages(config.SaveBtnImg, comboBox_save_btn_img, "保存按钮图片");
            _loadImages(config.CloseBtnImg, comboBox_close_btn_img, "关闭按钮图片");

            void _loadImages(Queue<string> queue, ComboBox box, string defaultName) {
                if (queue.Count > 0) {
                    // Check if image is still there
                    List<string> imageNamesTemp = new();
                    while (queue.Count > 0) {
                        string imageName = queue.Dequeue();
                        if (FileUtil.ImageExists(imageName)) {
                            imageNamesTemp.Add(imageName);
                        }
                    }

                    // Reenqueue elements
                    foreach (string imageName in imageNamesTemp) {
                        queue.Enqueue(imageName);
                    }

                    if (imageNamesTemp.Count > 0) {
                        box.Items.AddRange(imageNamesTemp.ToArray());
                        box.SelectedIndex = 0;
                    }

                    FileUtil.SaveConfig(config);
                } else {
                    // If no images in config, load from Button Images folder
                    LoadDefaultImagesFromFolder(box, defaultName);
                    // After loading, if an item was successfully added to the ComboBox, save it to config
                    if (box.Items.Count > 0) {
                        // The LoadDefaultImagesFromFolder method adds 'defaultName' if successful.
                        // So, if items exist, we can assume defaultName was added.
                        queue.Enqueue(defaultName);
                        FileUtil.SaveConfig(config);
                    }
                }
            }

            if (config.PlcIp.Count > 0) {
                comboBox_plc_ip.Items.AddRange(config.PlcIp.ToArray());
                comboBox_plc_ip.SelectedIndex = 0;
            }
            if (config.PlcPort.Count > 0) {
                comboBox_plc_port.Items.AddRange(config.PlcPort.Cast<object>().ToArray());
                comboBox_plc_port.SelectedIndex = 0;
            }
            if (config.PlcFlagPos != 0) {
                textBox_plc_flag_pos.Text = config.PlcFlagPos.ToString();
            }
            if (config.PlcHeartBeatPos != 0) {
                textBox_plc_heartbeat_pos.Text = config.PlcHeartBeatPos.ToString();
            }

            if (!string.IsNullOrEmpty(config.StoragePath)) {
                textBox_storage_path.Text = config.StoragePath;
            }

            if (!string.IsNullOrEmpty(config.CurveStoragePath)) {
                textBox_curve_storage_path.Text = config.CurveStoragePath;
            }

            // Add event listener
            comboBox_ip.SelectedIndexChanged += ComboBox_Ip_SelectedIndexChanged;
            button_connect.Click += ButtonConnect_Click;
            button_disconnect.Click += ButtonDisConnect_Click;
            button_start_fetch.Click += ButtonStartFetch_Click;
            button_clear_log.Click += ButtonClearLog_Click;
            button_browse.Click += ButtonBrowse_Click;
            button_browse_curve.Click += ButtonBrowseCurve_Click;
            checkBox_auto_startup.CheckedChanged += CheckBoxAutoStartup_CheckedChanged;
            checkBox_auto_fetch.CheckedChanged += CheckBoxAutoFetch_CheckedChanged;

            button_capture_update_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_update_btn_img, "更新按钮", cfg => cfg.UpdateBtnImg);
            button_crv_header_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_crv_header_img, "曲线表头", cfg => cfg.CrvHeaderImg);
            button_export_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_export_btn_img, "导出按钮", cfg => cfg.ExportBtnImg);
            button_blm_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_blm_btn_img, "BLM按钮", cfg => cfg.BlmBtnImg);
            button_save_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_save_btn_img, "保存按钮", cfg => cfg.SaveBtnImg);
            button_close_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_close_btn_img, "关闭按钮", cfg => cfg.CloseBtnImg);

            // Initialize log
            WidgetUtils.TextBox_realtime_log = textBox_realtime_log;
            WidgetUtils.AppendMsg("等待连接...");

            // Auto start fetch if enabled
            if (config.AutoStartFetch) {
                // Use a timer to delay the auto start to ensure UI is fully loaded
                var timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000; // 1 second delay
                timer.Tick += (s, e) => {
                    timer.Stop();
                    timer.Dispose();
                    AutoStartFetch();
                };
                timer.Start();
            }
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

                connecting = true;

                button_connect.Enabled = false;
                comboBox_ip.Enabled = false;
                comboBox_port.Enabled = false;
                button_disconnect.Enabled = true;
                textBox_storage_path.Enabled = false;
                button_browse.Enabled = false;

                // --- 修正逻辑：移动当前IP和端口到队列首位 ---
                // 1. 将队列转换为列表，方便操作
                var ipList = new List<string>(config.Ip);
                var portList = new List<int>(config.Port);

                // 2. 从列表中移除当前IP和端口（如果存在）
                ipList.Remove(ip);
                portList.Remove(port);

                // 3. 将当前IP和端口插入到列表开头
                ipList.Insert(0, ip);
                portList.Insert(0, port);

                // 4. 重新创建队列并赋值给config
                config.Ip = new Queue<string>(ipList);
                config.Port = new Queue<int>(portList);

                // 5. 限制队列大小（从队列末尾移除，即移除最旧的）
                while (config.Ip.Count > 5) config.Ip.Dequeue();
                while (config.Port.Count > 5) config.Port.Dequeue();
                // --- 结束修正逻辑 ---

                config.StoragePath = pathPrefix;
                FileUtil.SaveConfig(config);

                // Add to combo box
                comboBox_ip.Items.Clear();
                comboBox_port.Items.Clear();
                comboBox_ip.Items.AddRange(config.Ip.ToArray());
                comboBox_port.Items.AddRange(config.Port.Cast<object>().ToArray());
                // 设置第一个为选中项（即刚连接成功的）
                if (comboBox_ip.Items.Count > 0) comboBox_ip.SelectedIndex = 0;
                if (comboBox_port.Items.Count > 0) comboBox_port.SelectedIndex = 0; // 确保端口也同步更新
            } catch (Exception er) {
                WidgetUtils.AppendMsg("连接出错，请联系管理员");
                log.Warn($"Connect failed, e = {er}");
            }
        }

        private void ButtonDisConnect_Click(object? sender, EventArgs e) {
            sta6000PlusTask.IsConnected = false;
            connecting = false;

            button_disconnect.Enabled = false;
            button_connect.Enabled = true;
            comboBox_ip.Enabled = true;
            comboBox_port.Enabled = true;
            comboBox_update_btn_img.Enabled = true;

            if (!fetchingCurveData) {
                button_browse.Enabled = true;
                textBox_storage_path.Enabled = true;
                button_browse_curve.Enabled = true;
                textBox_curve_storage_path.Enabled = true;
            }
        }

        private void ButtonCaptureBtnImage_Click(ComboBox box, string defaultName, Func<Config, Queue<string>> queueFunc) {
            Rectangle? imageRect = ScreenRegionSelector.SelectScreenRegion();
            if (imageRect == null) {
                return;
            }

            // Create button image
            Rectangle rect = imageRect.Value;
            using (Bitmap image = new Bitmap(rect.Width, rect.Height))
            using (Graphics imgGraphics = Graphics.FromImage(image)) {
                imgGraphics.SmoothingMode = SmoothingMode.HighQuality;
                imgGraphics.CompositingQuality = CompositingQuality.HighQuality;
                imgGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                // Set captured range
                imgGraphics.CopyFromScreen(rect.X, rect.Y, 0, 0, new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));

                // Show popup form and save image
                var form = new ImageReviewInputBox(image, defaultName, queueFunc, () => {
                    // Reload latest config
                    config = FileUtil.LoadConfig();

                    // Refresh combo box
                    box.Items.Clear();
                    box.Items.AddRange(queueFunc(config).ToArray());
                    box.SelectedIndex = box.Items.Count - 1;
                });
                form.ShowDialog();
            }
        }

        private void ButtonStartFetch_Click(object? sender, EventArgs e) {
            // 曲线数据存储路径校验（在打开子窗口之前）
            string curveStoragePath = textBox_curve_storage_path.Text;
            if (string.IsNullOrWhiteSpace(curveStoragePath)) {
                WidgetUtils.ShowWarningPopUp("请先选择曲线数据存储路径！");
                return;
            }
            if (!FileUtil.IsPathValid(curveStoragePath)) {
                WidgetUtils.SetError(textBox_curve_storage_path, "请输入正确的曲线数据存储路径");
                return;
            } else {
                WidgetUtils.SetError(textBox_curve_storage_path, "");
            }

            string updateBtnImgName = comboBox_update_btn_img.Text;
            string crvHeaderImgName = comboBox_crv_header_img.Text;
            string exportBtnImgName = comboBox_export_btn_img.Text;
            string blmBtnImgName = comboBox_blm_btn_img.Text;
            string saveBtnImgName = comboBox_save_btn_img.Text;
            string closeBtnImgName = comboBox_close_btn_img.Text;
            if (string.IsNullOrWhiteSpace(updateBtnImgName) || string.IsNullOrWhiteSpace(crvHeaderImgName)
                || string.IsNullOrWhiteSpace(exportBtnImgName) || string.IsNullOrWhiteSpace(blmBtnImgName)
                || string.IsNullOrWhiteSpace(saveBtnImgName) || string.IsNullOrWhiteSpace(closeBtnImgName)
                ) {
                WidgetUtils.ShowWarningPopUp("请先选择所有按钮的图片！");
                return;
            }

            object plcIpTemp = comboBox_plc_ip.SelectedItem ?? comboBox_plc_ip.Text;
            object plcPortTemp = comboBox_plc_port.SelectedItem ?? comboBox_plc_port.Text;
            string flagPosStr = textBox_plc_flag_pos.Text;
            string heartbeatPosStr = textBox_plc_heartbeat_pos.Text;

            if (!ArgumentValidator.ValidateIPv4(plcIpTemp)) {
                WidgetUtils.SetError(comboBox_plc_ip, "请输入正确的IPv4地址");
                return;
            } else {
                WidgetUtils.SetError(comboBox_plc_ip, "");
            }

            if (!ArgumentValidator.ValidatePortInWindows(plcPortTemp)) {
                WidgetUtils.SetError(comboBox_plc_port, "请输入正确的端口号");
                return;
            } else {
                WidgetUtils.SetError(comboBox_plc_port, "");
            }

            if (string.IsNullOrWhiteSpace(flagPosStr) || string.IsNullOrWhiteSpace(heartbeatPosStr)) {
                WidgetUtils.ShowWarningPopUp("标识位和心跳位均不能为空！");
                return;
            }

            var ip = plcIpTemp.ToString();
            var port = int.Parse(plcPortTemp.ToString());
            var flagPos = int.Parse(flagPosStr);
            var heartbeatPos = int.Parse(heartbeatPosStr);

            // --- 修正逻辑：移动当前PLC IP和端口到队列首位 ---
            var plcIpList = new List<string>(config.PlcIp);
            var plcPortList = new List<int>(config.PlcPort);

            plcIpList.Remove(ip);
            plcPortList.Remove(port);

            plcIpList.Insert(0, ip);
            plcPortList.Insert(0, port);

            config.PlcIp = new Queue<string>(plcIpList);
            config.PlcPort = new Queue<int>(plcPortList);

            while (config.PlcIp.Count > 5) config.PlcIp.Dequeue();
            while (config.PlcPort.Count > 5) config.PlcPort.Dequeue();
            // --- 结束修正逻辑 ---

            config.PlcFlagPos = flagPos;
            config.PlcHeartBeatPos = heartbeatPos;
            config.CurveStoragePath = curveStoragePath;
            FileUtil.SaveConfig(config);

            button_capture_update_btn_img.Enabled = false;
            button_crv_header_img.Enabled = false;
            button_export_btn_img.Enabled = false;
            button_blm_btn_img.Enabled = false;
            button_save_btn_img.Enabled = false;
            button_close_btn_img.Enabled = false;
            comboBox_update_btn_img.Enabled = false;
            comboBox_crv_header_img.Enabled = false;
            comboBox_export_btn_img.Enabled = false;
            comboBox_blm_btn_img.Enabled = false;
            comboBox_save_btn_img.Enabled = false;
            comboBox_close_btn_img.Enabled = false;
            comboBox_plc_ip.Enabled = false;
            comboBox_plc_port.Enabled = false;
            button_start_fetch.Enabled = false;
            textBox_curve_storage_path.Enabled = false;
            button_browse_curve.Enabled = false;

            fetchingCurveData = true;

            List<string> buttons = new() {
                updateBtnImgName,
                crvHeaderImgName,
                exportBtnImgName,
                blmBtnImgName,
                saveBtnImgName,
                closeBtnImgName,
            };

            var fetchConfig = new FetchConfig {
                PlcIp = ip,
                PlcPort = port,
                CurveStoragePath = curveStoragePath,
            };

            var imageButtonClickerToolForm = new ImageButtonClickerToolForm(this, buttons, fetchConfig, () => {
                fetchingCurveData = false;

                button_capture_update_btn_img.Enabled = true;
                button_crv_header_img.Enabled = true;
                button_export_btn_img.Enabled = true;
                button_blm_btn_img.Enabled = true;
                button_save_btn_img.Enabled = true;
                button_close_btn_img.Enabled = true;
                comboBox_update_btn_img.Enabled = true;
                comboBox_crv_header_img.Enabled = true;
                comboBox_export_btn_img.Enabled = true;
                comboBox_blm_btn_img.Enabled = true;
                comboBox_save_btn_img.Enabled = true;
                comboBox_close_btn_img.Enabled = true;
                comboBox_plc_ip.Enabled = true;
                comboBox_plc_port.Enabled = true;
                button_start_fetch.Enabled = true;
                textBox_curve_storage_path.Enabled = true;
                button_browse_curve.Enabled = true;
            });
            imageButtonClickerToolForm.Show();
        }

        private void ButtonBrowseCurve_Click(object? sender, EventArgs e) {
            using var folderDialog = new FolderBrowserDialog();
            folderDialog.ShowNewFolderButton = true;
            if (!string.IsNullOrEmpty(textBox_curve_storage_path.Text)
                    && !string.IsNullOrWhiteSpace(textBox_curve_storage_path.Text)) {
                folderDialog.SelectedPath = textBox_curve_storage_path.Text;
            }

            folderDialog.Description = "请选择曲线文件存储文件夹";

            if (folderDialog.ShowDialog() == DialogResult.OK) {
                string selectedFolderPath = folderDialog.SelectedPath;

                if (!FileUtil.IsPathValid(selectedFolderPath)) {
                    WidgetUtils.SetError(textBox_curve_storage_path, "请输入正确的路径");
                    return;
                } else {
                    WidgetUtils.SetError(textBox_curve_storage_path, "");
                }

                textBox_curve_storage_path.Text = selectedFolderPath;
            }
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

        /// <summary>
        /// 开机自启动复选框变化事件
        /// </summary>
        private void CheckBoxAutoStartup_CheckedChanged(object? sender, EventArgs e) {
            try {
                bool enable = checkBox_auto_startup.Checked;
                bool success = AutoStartupManager.SetAutoStartup(enable);

                if (success) {
                    config.AutoStartup = enable;
                    FileUtil.SaveConfig(config);
                    WidgetUtils.AppendMsg(enable ? "✅ 开机自动启动 - 已启用" : "❎ 开机自动启动 - 已禁用");
                } else {
                    checkBox_auto_startup.Checked = !enable; // 恢复原状态
                    WidgetUtils.AppendMsg("❌ 设置开机自启动失败，请检查权限");
                }
            } catch (Exception ex) {
                WidgetUtils.AppendMsg($"❌ 设置开机自启动失败: {ex.Message}");
                log.Error($"Set auto startup failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 启动自动抓取复选框变化事件
        /// </summary>
        private void CheckBoxAutoFetch_CheckedChanged(object? sender, EventArgs e) {
            try {
                bool enable = checkBox_auto_fetch.Checked;
                config.AutoStartFetch = enable;
                FileUtil.SaveConfig(config);
                WidgetUtils.AppendMsg(enable ? "✅ 启动自动抓取 - 已启用" : "❎ 启动自动抓取 - 已禁用");
            } catch (Exception ex) {
                WidgetUtils.AppendMsg($"❌ 设置启动自动抓取失败: {ex.Message}");
                log.Error($"Set auto fetch failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 自动启动抓取功能
        /// </summary>
        private void AutoStartFetch() {
            try {
                log.Info("Auto start fetch triggered");
                WidgetUtils.AppendMsg("🚀 自动启动抓取...");

                // 检查是否有必要的图片配置
                if (string.IsNullOrWhiteSpace(comboBox_update_btn_img.Text) ||
                    string.IsNullOrWhiteSpace(comboBox_crv_header_img.Text) ||
                    string.IsNullOrWhiteSpace(comboBox_export_btn_img.Text) ||
                    string.IsNullOrWhiteSpace(comboBox_blm_btn_img.Text) ||
                    string.IsNullOrWhiteSpace(comboBox_save_btn_img.Text) ||
                    string.IsNullOrWhiteSpace(comboBox_close_btn_img.Text)) {
                    WidgetUtils.AppendMsg("❌ 自动抓取失败：请先配置所有按钮图片");
                    return;
                }
                // 检查PLC配置
                if (string.IsNullOrWhiteSpace(comboBox_plc_ip.Text) ||
                    string.IsNullOrWhiteSpace(comboBox_plc_port.Text) ||
                    string.IsNullOrWhiteSpace(textBox_plc_flag_pos.Text) ||
                    string.IsNullOrWhiteSpace(textBox_plc_heartbeat_pos.Text)) {
                    WidgetUtils.AppendMsg("❌ 自动抓取失败：请先配置PLC连接信息（IP、端口、标志位、心跳位）");
                    return;
                }

                // 调用开始抓取按钮的逻辑
                ButtonStartFetch_Click(this, EventArgs.Empty);
            } catch (Exception ex) {
                WidgetUtils.AppendMsg($"❌ 自动抓取失败: {ex.Message}");
                log.Error($"Auto start fetch failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 从Button Images文件夹加载默认图片到ComboBox
        /// </summary>
        /// <param name="comboBox">目标ComboBox</param>
        private void LoadDefaultImagesFromFolder(ComboBox comboBox, string defaultName) {
            try {
                // Check if the specific default image exists in the "Button Images" folder.
                // FileUtil.ImageExists checks for the image name with a .png extension.
                if (FileUtil.ImageExists(defaultName)) {
                    // Clear any existing items in the ComboBox, as only one image is needed.
                    comboBox.Items.Clear();
                    // Add only this specific default image name to the ComboBox.
                    comboBox.Items.Add(defaultName);
                    // Select the added image.
                    comboBox.SelectedIndex = 0;
                    log.Info($"已加载默认图片 '{defaultName}' 到 {comboBox.Name}");
                } else {
                    log.Warn($"默认图片 '{defaultName}' 不存在于Button Images文件夹中，无法加载到 {comboBox.Name}");
                }
            } catch (Exception ex) {
                log.Error($"加载默认图片失败: {ex.Message}", ex);
            }
        }
    }
}
