namespace STaTool.utils {
    partial class ImageReviewInputBox {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            pictureBox_image_review = new PictureBox();
            label_image_name = new Label();
            textBox_image_name = new TextBox();
            button_save = new Button();
            button_cancel = new Button();
            ((System.ComponentModel.ISupportInitialize) pictureBox_image_review).BeginInit();
            SuspendLayout();
            // 
            // pictureBox_image_review
            // 
            pictureBox_image_review.Anchor =  AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox_image_review.BackColor = SystemColors.ControlLight;
            pictureBox_image_review.Location = new Point(12, 12);
            pictureBox_image_review.Name = "pictureBox_image_review";
            pictureBox_image_review.Size = new Size(458, 128);
            pictureBox_image_review.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox_image_review.TabIndex = 0;
            pictureBox_image_review.TabStop = false;
            // 
            // label_image_name
            // 
            label_image_name.AutoSize = true;
            label_image_name.Location = new Point(12, 143);
            label_image_name.Name = "label_image_name";
            label_image_name.Size = new Size(82, 24);
            label_image_name.TabIndex = 1;
            label_image_name.Text = "图片名称";
            // 
            // textBox_image_name
            // 
            textBox_image_name.Location = new Point(12, 170);
            textBox_image_name.Name = "textBox_image_name";
            textBox_image_name.Size = new Size(458, 30);
            textBox_image_name.TabIndex = 2;
            // 
            // button_save
            // 
            button_save.DialogResult = DialogResult.OK;
            button_save.Location = new Point(228, 214);
            button_save.Name = "button_save";
            button_save.Size = new Size(112, 34);
            button_save.TabIndex = 3;
            button_save.Text = "保存";
            button_save.UseVisualStyleBackColor = true;
            // 
            // button_cancel
            // 
            button_cancel.DialogResult = DialogResult.Cancel;
            button_cancel.Location = new Point(358, 214);
            button_cancel.Name = "button_cancel";
            button_cancel.Size = new Size(112, 34);
            button_cancel.TabIndex = 3;
            button_cancel.Text = "取消";
            button_cancel.UseVisualStyleBackColor = true;
            // 
            // ImageReviewInputBox
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(482, 261);
            Controls.Add(button_cancel);
            Controls.Add(button_save);
            Controls.Add(textBox_image_name);
            Controls.Add(label_image_name);
            Controls.Add(pictureBox_image_review);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImageReviewInputBox";
            StartPosition = FormStartPosition.CenterParent;
            Text = "保存按钮图片";
            ((System.ComponentModel.ISupportInitialize) pictureBox_image_review).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBox_image_review;
        private Label label_image_name;
        private TextBox textBox_image_name;
        private Button button_save;
        private Button button_cancel;
    }
}