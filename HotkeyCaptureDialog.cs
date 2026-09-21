using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RD_Tools
{
    public class HotkeyCaptureDialog : Form
    {
        private readonly Label lblTarget;
        private readonly Label lblKeyDisplay;
        private readonly Label lblStatusTip;
        private readonly Button btnApply;
        private readonly Button btnCancel;

        public HotkeyConfig ResultConfig { get; private set; }

        public HotkeyCaptureDialog(string targetName, HotkeyConfig initialConfig, bool isDarkTheme = false)
        {
            Text = "⌨ 단축키 직접 입력 감지";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(400, 260);
            Font = new Font("Malgun Gothic", 9f, FontStyle.Regular);
            KeyPreview = true;

            Color bgColor = isDarkTheme ? Color.FromArgb(32, 34, 38) : Color.FromArgb(250, 248, 245);
            Color fgColor = isDarkTheme ? Color.FromArgb(235, 238, 242) : Color.FromArgb(40, 35, 30);
            Color cardBg = isDarkTheme ? Color.FromArgb(44, 46, 52) : Color.FromArgb(242, 238, 230);
            Color accentColor = isDarkTheme ? Color.FromArgb(245, 158, 11) : Color.FromArgb(197, 128, 32);

            BackColor = bgColor;
            ForeColor = fgColor;

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(18, 14, 18, 14)
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 28f));  // Target label
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36f));  // Description
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60f));  // Key Display Box
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 24f));  // Status Tip
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));  // Buttons

            lblTarget = new Label
            {
                Text = $"🎯 설정 대상: {targetName}",
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 10f, FontStyle.Bold),
                ForeColor = accentColor,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var lblDesc = new Label
            {
                Text = "키보드에서 설정할 단축키 조합을 누르세요.\n(예: F1~F12, Alt+F1, Alt+F4, Ctrl+Shift+S 등)",
                Dock = DockStyle.Fill,
                ForeColor = isDarkTheme ? Color.FromArgb(180, 185, 195) : Color.FromArgb(90, 80, 70),
                TextAlign = ContentAlignment.TopLeft
            };

            var pnlDisplay = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = cardBg,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(4),
                Margin = new Padding(0, 4, 0, 4)
            };

            lblKeyDisplay = new Label
            {
                Text = initialConfig != null ? initialConfig.ToString() : "단축키를 누르세요...",
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 16f, FontStyle.Bold),
                ForeColor = accentColor,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlDisplay.Controls.Add(lblKeyDisplay);

            lblStatusTip = new Label
            {
                Text = "※ Esc: 취소  |  Enter: 설정 적용",
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 8.5f, FontStyle.Regular),
                ForeColor = isDarkTheme ? Color.FromArgb(140, 145, 155) : Color.FromArgb(120, 110, 100),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var btnTable = new TableLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 36,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 6, 0, 0)
            };
            btnTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnApply = new Button
            {
                Text = "✔ 적용 (Enter)",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(46, 139, 87),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 4, 0)
            };
            btnApply.FlatAppearance.BorderSize = 0;
            btnApply.Click += (s, e) =>
            {
                if (ResultConfig != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            };

            btnCancel = new Button
            {
                Text = "✖ 취소 (Esc)",
                Dock = DockStyle.Fill,
                BackColor = isDarkTheme ? Color.FromArgb(60, 64, 72) : Color.FromArgb(220, 215, 208),
                ForeColor = fgColor,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 0, 0, 0)
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            btnTable.Controls.Add(btnApply, 0, 0);
            btnTable.Controls.Add(btnCancel, 1, 0);

            root.Controls.Add(lblTarget, 0, 0);
            root.Controls.Add(lblDesc, 0, 1);
            root.Controls.Add(pnlDisplay, 0, 2);
            root.Controls.Add(lblStatusTip, 0, 3);
            root.Controls.Add(btnTable, 0, 4);

            Controls.Add(root);

            ResultConfig = initialConfig != null ? new HotkeyConfig(initialConfig.Modifier, initialConfig.Key) : null;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            bool alt = (keyData & Keys.Alt) != 0;
            bool ctrl = (keyData & Keys.Control) != 0;
            bool shift = (keyData & Keys.Shift) != 0;
            Keys keyCode = keyData & Keys.KeyCode;

            // Pure Escape cancels dialog
            if (keyCode == Keys.Escape && !alt && !ctrl && !shift)
            {
                DialogResult = DialogResult.Cancel;
                Close();
                return true;
            }

            // Pure Enter applies captured hotkey
            if (keyCode == Keys.Enter && !alt && !ctrl && !shift)
            {
                if (ResultConfig != null)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                    return true;
                }
            }

            // Standalone modifier press: show pending indicator
            if (keyCode == Keys.Menu || keyCode == Keys.ControlKey || keyCode == Keys.ShiftKey ||
                keyCode == Keys.LMenu || keyCode == Keys.RMenu ||
                keyCode == Keys.LControlKey || keyCode == Keys.RControlKey ||
                keyCode == Keys.LShiftKey || keyCode == Keys.RShiftKey ||
                keyCode == Keys.None)
            {
                var held = new List<string>();
                if (ctrl) held.Add("Ctrl");
                if (alt) held.Add("Alt");
                if (shift) held.Add("Shift");
                if (held.Count > 0)
                {
                    lblKeyDisplay.Text = string.Join(" + ", held) + " + ...";
                }
                return true;
            }

            // Format modifier
            string modStr = "None";
            if (ctrl && alt) modStr = "Ctrl+Alt";
            else if (ctrl && shift) modStr = "Ctrl+Shift";
            else if (alt && shift) modStr = "Alt+Shift";
            else if (alt) modStr = "Alt";
            else if (ctrl) modStr = "Ctrl";
            else if (shift) modStr = "Shift";

            // Format key code
            string keyStr = keyCode.ToString();
            if (keyCode >= Keys.D0 && keyCode <= Keys.D9)
            {
                keyStr = ((char)('0' + (keyCode - Keys.D0))).ToString();
            }

            ResultConfig = new HotkeyConfig(modStr, keyStr);
            lblKeyDisplay.Text = ResultConfig.ToString();
            lblStatusTip.Text = $"💡 감지됨: [{ResultConfig}] - [Enter]를 누르거나 적용 버튼을 클릭하세요.";
            btnApply.Enabled = true;

            // Return true to prevent Windows from handling Alt+F4 or system keys
            return true;
        }
    }
}
