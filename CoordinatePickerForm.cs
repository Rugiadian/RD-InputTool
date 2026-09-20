using System;
using System.Drawing;
using System.Windows.Forms;

namespace RD_Tools
{
    public class CoordinatePickerForm : Form
    {
        public Point SelectedPoint { get; private set; }
        private Point _currentMouse = Point.Empty;
        private readonly System.Windows.Forms.Timer _timer;

        public CoordinatePickerForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            StartPosition = FormStartPosition.Manual;
            ShowInTaskbar = false;
            TopMost = true;
            DoubleBuffered = true;
            Cursor = Cursors.Cross;

            // Cover all monitors
            Rectangle virtualScreen = SystemInformation.VirtualScreen;
            Bounds = virtualScreen;

            // Semi-transparent background
            BackColor = Color.FromArgb(30, 30, 40);
            Opacity = 0.35;

            _timer = new System.Windows.Forms.Timer { Interval = 16 };
            _timer.Tick += (s, e) =>
            {
                var cur = Cursor.Position;
                if (cur != _currentMouse)
                {
                    _currentMouse = cur;
                    Invalidate();
                }
            };
            _timer.Start();

            KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            };

            MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    SelectedPoint = Cursor.Position;
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else if (e.Button == MouseButtons.Right)
                {
                    DialogResult = DialogResult.Cancel;
                    Close();
                }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;

            // Convert screen coordinates to form client coordinates
            Point clientPoint = PointToClient(_currentMouse);

            // Draw full-screen crosshair guides
            using (var guidePen = new Pen(Color.FromArgb(180, 255, 60, 60), 1))
            {
                guidePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                g.DrawLine(guidePen, 0, clientPoint.Y, Width, clientPoint.Y);
                g.DrawLine(guidePen, clientPoint.X, 0, clientPoint.X, Height);
            }

            // Draw target circle
            using (var circlePen = new Pen(Color.LimeGreen, 2))
            {
                g.DrawEllipse(circlePen, clientPoint.X - 16, clientPoint.Y - 16, 32, 32);
                g.FillEllipse(Brushes.Red, clientPoint.X - 2, clientPoint.Y - 2, 4, 4);
            }

            // Draw floating coordinate badge near mouse
            string text = $"클릭하여 좌표 선택 (X: {_currentMouse.X}, Y: {_currentMouse.Y})\n[ESC 또는 우클릭: 취소]";
            using (var font = new Font("Malgun Gothic", 10.5f, FontStyle.Bold))
            {
                var size = g.MeasureString(text, font);
                int badgeX = clientPoint.X + 22;
                int badgeY = clientPoint.Y + 22;

                if (badgeX + size.Width + 16 > Width)
                    badgeX = clientPoint.X - (int)size.Width - 26;
                if (badgeY + size.Height + 16 > Height)
                    badgeY = clientPoint.Y - (int)size.Height - 26;

                var badgeRect = new Rectangle(badgeX, badgeY, (int)size.Width + 16, (int)size.Height + 12);

                using (var bgBrush = new SolidBrush(Color.FromArgb(230, 20, 20, 25)))
                using (var borderPen = new Pen(Color.LimeGreen, 1.5f))
                {
                    g.FillRectangle(bgBrush, badgeRect);
                    g.DrawRectangle(borderPen, badgeRect);
                    g.DrawString(text, font, Brushes.White, badgeX + 8, badgeY + 6);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _timer.Stop();
            _timer.Dispose();
            base.OnFormClosing(e);
        }
    }
}
