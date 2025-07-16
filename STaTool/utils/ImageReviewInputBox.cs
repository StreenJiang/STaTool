namespace STaTool.utils {
    public partial class ImageReviewInputBox: Form {
        private Image image { get; set; }

        public PictureBox pictureBox => pictureBox_image_review;
        public TextBox TextBox => textBox_image_name;

        public ImageReviewInputBox(Image image, string defaultName, Func<Config, Queue<string>> queueFunc, Action refrechComboBox) {
            InitializeComponent();
            CancelButton = button_cancel;
            AcceptButton = button_save;

            this.image = image;
            textBox_image_name.Text = defaultName;
            pictureBox_image_review.Image = image;

            button_save.DialogResult = DialogResult.None;
            button_save.Click += (s, e) => {
                string imageName = textBox_image_name.Text;

                if (string.IsNullOrWhiteSpace(imageName)) {
                    WidgetUtils.ShowWarningPopUp("请输入图片名称！");
                    textBox_image_name.Focus();
                    return;
                }

                Config config = FileUtil.LoadConfig();
                Queue<string> queue = queueFunc(config);
                if (queue.Contains(imageName)) {
                    if (!WidgetUtils.ShowConfirmPopUp("已存在同名图片，是否覆盖？")) {
                        textBox_image_name.SelectAll();
                        return;
                    }
                }

                // Save image to local
                FileUtil.SaveImage(image, imageName);

                // Save config
                if (!queue.Contains(imageName)) {
                    queue.Enqueue(imageName);
                }
                // Remove the oldest one if the queue is greater than 5
                if (queue.Count > 5) {
                    string imageNameTemp = queue.Dequeue();
                    FileUtil.DeleteImage(imageNameTemp);
                }
                FileUtil.SaveConfig(config);

                // Refresh combo box
                refrechComboBox();

                DialogResult = DialogResult.OK;
                Close();
            };
        }
    }
}
