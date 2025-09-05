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
            _loadImages(config.YesBtnImg, comboBox_yes_btn_img, "是否替换按钮图片");
            _loadImages(config.OkBtnImg, comboBox_ok_btn_img, "确认按钮图片");
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

            // Add event listener
            comboBox_ip.SelectedIndexChanged += ComboBox_Ip_SelectedIndexChanged;
            button_connect.Click += ButtonConnect_Click;
            button_disconnect.Click += ButtonDisConnect_Click;
            button_start_fetch.Click += ButtonStartFetch_Click;
            button_clear_log.Click += ButtonClearLog_Click;
            button_browse.Click += ButtonBrowse_Click;
            checkBox_auto_startup.CheckedChanged += CheckBoxAutoStartup_CheckedChanged;

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
            button_yes_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_yes_btn_img, "是否替换按钮", cfg => cfg.YesBtnImg);
            button_ok_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_ok_btn_img, "确认按钮", cfg => cfg.OkBtnImg);
            button_close_btn_img.Click += (s, e)
                => ButtonCaptureBtnImage_Click(comboBox_close_btn_img, "关闭按钮", cfg => cfg.CloseBtnImg);

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

                connecting = true;

                button_connect.Enabled = false;
                comboBox_ip.Enabled = false;
                comboBox_port.Enabled = false;
                button_disconnect.Enabled = true;
                textBox_storage_path.Enabled = false;
                button_browse.Enabled = false;

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
            string updateBtnImgName = comboBox_update_btn_img.Text;
            string crvHeaderImgName = comboBox_crv_header_img.Text;
            string exportBtnImgName = comboBox_export_btn_img.Text;
            string blmBtnImgName = comboBox_blm_btn_img.Text;
            string saveBtnImgName = comboBox_save_btn_img.Text;
            string yesBtnImgName = comboBox_yes_btn_img.Text;
            string okBtnImgName = comboBox_ok_btn_img.Text;
            string closeBtnImgName = comboBox_close_btn_img.Text;
            if (string.IsNullOrWhiteSpace(updateBtnImgName) || string.IsNullOrWhiteSpace(crvHeaderImgName)
                || string.IsNullOrWhiteSpace(exportBtnImgName) || string.IsNullOrWhiteSpace(blmBtnImgName)
                || string.IsNullOrWhiteSpace(saveBtnImgName) || string.IsNullOrWhiteSpace(yesBtnImgName)
                || string.IsNullOrWhiteSpace(okBtnImgName) || string.IsNullOrWhiteSpace(closeBtnImgName)
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

            // Save config
            if (!config.PlcIp.Contains(ip) && !config.PlcPort.Contains(port)) {
                config.PlcIp.Enqueue(ip);
                config.PlcPort.Enqueue(port);
            }
            // Remove the oldest one if the queue is greater than 5
            if (config.PlcIp.Count > 5) {
                config.PlcIp.Dequeue();
            }
            if (config.PlcPort.Count > 5) {
                config.PlcPort.Dequeue();
            }
            config.PlcFlagPos = flagPos;
            config.PlcHeartBeatPos = heartbeatPos;
            FileUtil.SaveConfig(config);

            button_capture_update_btn_img.Enabled = false;
            button_crv_header_img.Enabled = false;
            button_export_btn_img.Enabled = false;
            button_blm_btn_img.Enabled = false;
            button_save_btn_img.Enabled = false;
            button_yes_btn_img.Enabled = false;
            button_ok_btn_img.Enabled = false;
            button_close_btn_img.Enabled = false;
            comboBox_update_btn_img.Enabled = false;
            comboBox_crv_header_img.Enabled = false;
            comboBox_export_btn_img.Enabled = false;
            comboBox_blm_btn_img.Enabled = false;
            comboBox_save_btn_img.Enabled = false;
            comboBox_yes_btn_img.Enabled = false;
            comboBox_ok_btn_img.Enabled = false;
            comboBox_close_btn_img.Enabled = false;
            comboBox_plc_ip.Enabled = false;
            comboBox_plc_port.Enabled = false;
            button_start_fetch.Enabled = false;

            fetchingCurveData = true;

            List<string> buttons = new() {
                updateBtnImgName,
                crvHeaderImgName,
                exportBtnImgName,
                blmBtnImgName,
                saveBtnImgName,
                yesBtnImgName,
                okBtnImgName,
                closeBtnImgName,
            };
            var imageButtonClickerToolForm = new ImageButtonClickerToolForm(buttons, ip, port, () => {
                fetchingCurveData = false;

                button_capture_update_btn_img.Enabled = true;
                button_crv_header_img.Enabled = true;
                button_export_btn_img.Enabled = true;
                button_blm_btn_img.Enabled = true;
                button_save_btn_img.Enabled = true;
                button_yes_btn_img.Enabled = true;
                button_ok_btn_img.Enabled = true;
                button_close_btn_img.Enabled = true;
                comboBox_update_btn_img.Enabled = true;
                comboBox_crv_header_img.Enabled = true;
                comboBox_export_btn_img.Enabled = true;
                comboBox_blm_btn_img.Enabled = true;
                comboBox_save_btn_img.Enabled = true;
                comboBox_yes_btn_img.Enabled = true;
                comboBox_ok_btn_img.Enabled = true;
                comboBox_close_btn_img.Enabled = true;
                comboBox_plc_ip.Enabled = true;
                comboBox_plc_port.Enabled = true;
                button_start_fetch.Enabled = true;
            });
            imageButtonClickerToolForm.Show();
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
