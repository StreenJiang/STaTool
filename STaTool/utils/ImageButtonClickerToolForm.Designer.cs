namespace STaTool.utils {
    partial class ImageButtonClickerToolForm {
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
            label_info = new Label();
            button_start_fetch = new Button();
            button_stop_fetch = new Button();
            label1 = new Label();
            textBox_repeat_times = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox_click_interval = new TextBox();
            label5 = new Label();
            label6 = new Label();
            textBox_check_interval = new TextBox();
            SuspendLayout();
            // 
            // label_info
            // 
            label_info.AutoSize = true;
            label_info.Location = new Point(96, 23);
            label_info.Name = "label_info";
            label_info.Size = new Size(214, 24);
            label_info.TabIndex = 0;
            label_info.Text = "提示：按 F10 可停止抓取";
            // 
            // button_start_fetch
            // 
            button_start_fetch.Location = new Point(67, 207);
            button_start_fetch.Name = "button_start_fetch";
            button_start_fetch.Size = new Size(112, 34);
            button_start_fetch.TabIndex = 1;
            button_start_fetch.Text = "开始抓取";
            button_start_fetch.UseVisualStyleBackColor = true;
            // 
            // button_stop_fetch
            // 
            button_stop_fetch.Enabled = false;
            button_stop_fetch.Location = new Point(207, 207);
            button_stop_fetch.Name = "button_stop_fetch";
            button_stop_fetch.Size = new Size(112, 34);
            button_stop_fetch.TabIndex = 1;
            button_stop_fetch.Text = "停止抓取";
            button_stop_fetch.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(64, 82);
            label1.Name = "label1";
            label1.Size = new Size(118, 24);
            label1.TabIndex = 2;
            label1.Text = "重复抓取次数";
            // 
            // textBox_repeat_times
            // 
            textBox_repeat_times.Location = new Point(203, 76);
            textBox_repeat_times.Name = "textBox_repeat_times";
            textBox_repeat_times.Size = new Size(87, 30);
            textBox_repeat_times.TabIndex = 3;
            textBox_repeat_times.Text = "2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(296, 79);
            label2.Name = "label2";
            label2.Size = new Size(28, 24);
            label2.TabIndex = 2;
            label2.Text = "次";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(64, 118);
            label3.Name = "label3";
            label3.Size = new Size(118, 24);
            label3.TabIndex = 2;
            label3.Text = "鼠标点击间隔";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(296, 115);
            label4.Name = "label4";
            label4.Size = new Size(35, 24);
            label4.TabIndex = 2;
            label4.Text = "ms";
            // 
            // textBox_click_interval
            // 
            textBox_click_interval.Location = new Point(203, 112);
            textBox_click_interval.Name = "textBox_click_interval";
            textBox_click_interval.Size = new Size(87, 30);
            textBox_click_interval.TabIndex = 3;
            textBox_click_interval.Text = "2000";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(64, 157);
            label5.Name = "label5";
            label5.Size = new Size(118, 24);
            label5.TabIndex = 2;
            label5.Text = "标识检查间隔";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(296, 154);
            label6.Name = "label6";
            label6.Size = new Size(35, 24);
            label6.TabIndex = 2;
            label6.Text = "ms";
            // 
            // textBox_check_interval
            // 
            textBox_check_interval.Location = new Point(203, 151);
            textBox_check_interval.Name = "textBox_check_interval";
            textBox_check_interval.Size = new Size(87, 30);
            textBox_check_interval.TabIndex = 3;
            textBox_check_interval.Text = "2000";
            // 
            // ImageButtonClickerToolForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(405, 271);
            Controls.Add(textBox_check_interval);
            Controls.Add(textBox_click_interval);
            Controls.Add(label6);
            Controls.Add(label4);
            Controls.Add(textBox_repeat_times);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button_stop_fetch);
            Controls.Add(button_start_fetch);
            Controls.Add(label_info);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ImageButtonClickerToolForm";
            StartPosition = FormStartPosition.Manual;
            Text = "曲线抓取";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label_info;
        private Button button_start_fetch;
        private Button button_stop_fetch;
        private Label label1;
        private TextBox textBox_repeat_times;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox_click_interval;
        private Label label5;
        private Label label6;
        private TextBox textBox_check_interval;
    }
}