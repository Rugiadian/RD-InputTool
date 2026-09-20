using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace RD_Tools
{
    public class EnergyBar : Control
    {
        private double _value = 0.0; // 0.0 to 1.0
        private string _text = "⚪ 대기 중 (시작 버튼 또는 F6을 누르면 기동합니다)";
        private bool _isActive = false;
        private int _shimmerOffset = 0;
        private readonly System.Windows.Forms.Timer _shimmerTimer;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public double Value
        {
            get => _value;
            set
            {
                _value = Math.Clamp(value, 0.0, 1.0);
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StatusText
        {
            get => _text;
            set
            {
                _text = value;
                Invalidate();
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                if (_isActive)
                {
                    _shimmerTimer.Start();
                }
                else
                {
                    _shimmerTimer.Stop();
                    _shimmerOffset = 0;
                }
                Invalidate();
            }
        }

        public EnergyBar()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            Height = 34;
            Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold);
            BackColor = Color.FromArgb(32, 36, 44);

            _shimmerTimer = new System.Windows.Forms.Timer { Interval = 35 };
            _shimmerTimer.Tick += (s, e) =>
            {
                if (_isActive)
                {
                    _shimmerOffset = (_shimmerOffset + 4) % 60;
                    Invalidate();
                }
            };
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var rect = ClientRectangle;
            if (rect.Width <= 0 || rect.Height <= 0) return;

            // 1. Dark Cyber Background
            using (var bgBrush = new SolidBrush(Color.FromArgb(28, 32, 40)))
            {
                g.FillRectangle(bgBrush, rect);
            }

            // 2. Draw Energy Fill (when active and Value > 0)
            int fillWidth = (int)(rect.Width * _value);
            if (fillWidth > 2 && _isActive)
            {
                var fillRect = new Rectangle(0, 0, fillWidth, rect.Height);

                // Neon Cyber Gradient: Emerald Green to Vivid Cyan
                using (var fillBrush = new LinearGradientBrush(
                    fillRect,
                    Color.FromArgb(39, 174, 96),   // Neon Emerald
                    Color.FromArgb(0, 210, 211),   // Bright Cyan
                    LinearGradientMode.Horizontal))
                {
                    g.FillRectangle(fillBrush, fillRect);
                }

                // Animated Diagonal Energy Shimmer Stripes
                using (var clipRegion = new Region(fillRect))
                {
                    g.Clip = clipRegion;
                    using (var stripePen = new Pen(Color.FromArgb(50, 255, 255, 255), 6))
                    {
                        for (int x = -rect.Height + _shimmerOffset; x < fillWidth + rect.Height; x += 30)
                        {
                            g.DrawLine(stripePen, x, rect.Height, x + rect.Height, 0);
                        }
                    }
                    g.ResetClip();
                }

                // Top Gloss Highlight Line
                using (var glossPen = new Pen(Color.FromArgb(120, 255, 255, 255), 1))
                {
                    g.DrawLine(glossPen, 0, 1, fillWidth, 1);
                }

                // Leading Edge Glow Indicator
                if (fillWidth < rect.Width)
                {
                    using (var edgeBrush = new SolidBrush(Color.FromArgb(220, 255, 255, 255)))
                    {
                        g.FillRectangle(edgeBrush, fillWidth - 2, 0, 2, rect.Height);
                    }
                }
            }

            // 3. Border
            Color borderColor = _isActive ? Color.FromArgb(0, 180, 180) : Color.FromArgb(70, 75, 85);
            using (var borderPen = new Pen(borderColor, 1))
            {
                g.DrawRectangle(borderPen, 0, 0, rect.Width - 1, rect.Height - 1);
            }

            // 4. Center Text with Drop Shadow for 100% Readability
            if (!string.IsNullOrEmpty(_text))
            {
                var size = g.MeasureString(_text, Font);
                float textX = (rect.Width - size.Width) / 2f;
                float textY = (rect.Height - size.Height) / 2f;

                // Subtle shadow
                using (var shadowBrush = new SolidBrush(Color.FromArgb(180, 10, 15, 20)))
                {
                    g.DrawString(_text, Font, shadowBrush, textX + 1, textY + 1);
                }

                // Main text (White if active, Light Gray if idle)
                Color textColor = _isActive ? Color.White : Color.FromArgb(180, 185, 195);
                using (var textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(_text, Font, textBrush, textX, textY);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _shimmerTimer.Stop();
                _shimmerTimer.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
