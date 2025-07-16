namespace STaTool.utils {

    public class ScreenRegionSelector {
        private static Rectangle? _selectedRegion;
        private static Point? _startPoint;
        private static Form _selectorForm;

        /// <summary>
        /// 交互式选择屏幕区域
        /// </summary>
        public static Rectangle? SelectScreenRegion() {
            _selectedRegion = null;
            _startPoint = null;

            // 创建全屏透明窗体
            _selectorForm = new Form {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                Opacity = 0.3,
                TopMost = true,
                WindowState = FormWindowState.Maximized,
                Cursor = Cursors.Cross
            };

            // 设置鼠标事件
            _selectorForm.MouseDown += SelectorForm_MouseDown;
            _selectorForm.MouseMove += SelectorForm_MouseMove;
            _selectorForm.MouseUp += SelectorForm_MouseUp;
            _selectorForm.KeyDown += SelectorForm_KeyDown;
            _selectorForm.Paint += SelectorForm_Paint;

            // 显示窗体
            _selectorForm.ShowDialog();

            return _selectedRegion;
        }

        private static void SelectorForm_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                _startPoint = e.Location;
            }
        }

        private static void SelectorForm_MouseMove(object sender, MouseEventArgs e) {
            if (_startPoint.HasValue) {
                // 重绘选区
                _selectorForm.Invalidate();
            }
        }

        private static void SelectorForm_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && _startPoint.HasValue) {
                // 确定选区
                int x = Math.Min(_startPoint.Value.X, e.X);
                int y = Math.Min(_startPoint.Value.Y, e.Y);
                int width = Math.Abs(e.X - _startPoint.Value.X);
                int height = Math.Abs(e.Y - _startPoint.Value.Y);

                // 最小区域限制
                if (width >= 10 && height >= 10) {
                    _selectedRegion = new Rectangle(x, y, width, height);
                }

                _selectorForm.Close();
            }
        }

        private static void SelectorForm_KeyDown(object sender, KeyEventArgs e) {
            // 按ESC取消选择
            if (e.KeyCode == Keys.Escape) {
                _selectedRegion = null;
                _selectorForm.Close();
            }
        }

        private static void SelectorForm_Paint(object sender, PaintEventArgs e) {
            if (_startPoint.HasValue) {
                var currentPos = _selectorForm.PointToClient(Cursor.Position);
                int x = Math.Min(_startPoint.Value.X, currentPos.X);
                int y = Math.Min(_startPoint.Value.Y, currentPos.Y);
                int width = Math.Abs(currentPos.X - _startPoint.Value.X);
                int height = Math.Abs(currentPos.Y - _startPoint.Value.Y);

                // 绘制选区边框
                using (var pen = new Pen(Color.Yellow, 2)) {
                    e.Graphics.DrawRectangle(pen, x, y, width, height);
                }

                // 绘制选区尺寸提示
                string sizeText = $"{width} × {height}";
                using (var font = new Font("Arial", 8))
                using (var brush = new SolidBrush(Color.Yellow)) {
                    e.Graphics.DrawString(sizeText, font, brush, x + 5, y + 5);
                }
            }
        }
    }
}
