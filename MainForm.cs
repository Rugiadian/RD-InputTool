using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RD_Tools
{
    public class MainForm : Form
    {
        // ==========================================
        // 1. Theme Configuration
        // ==========================================
        private class ThemeColors
        {
            public Color FormBg { get; set; }
            public Color BarBg { get; set; }
            public Color GroupBg { get; set; }
            public Color GroupBorder { get; set; }
            public Color TextPrimary { get; set; }
            public Color TextSecondary { get; set; }
            public Color TextMuted { get; set; }
            public Color Accent { get; set; }
            public Color InputBg { get; set; }
            public Color InputFg { get; set; }
            public Color PnlP1Bg { get; set; }
            public Color PnlP2Bg { get; set; }
            public Color CardHeaderBg { get; set; }
            public Color CardBg { get; set; }
            public Color StatusBadgeBg { get; set; }
            public Color StatusBadgeFg { get; set; }
            public Color BannerBg { get; set; }
            public Color BannerBorder { get; set; }
            public Color BannerFg { get; set; }
            public bool IsDark { get; set; }
        }

        private static readonly Dictionary<string, ThemeColors> Themes = new Dictionary<string, ThemeColors>
        {
            ["아이보리 웜"] = new ThemeColors
            {
                FormBg = Color.FromArgb(250, 248, 243),
                BarBg = Color.FromArgb(242, 238, 231),
                GroupBg = Color.FromArgb(255, 254, 250),
                GroupBorder = Color.FromArgb(228, 222, 212),
                TextPrimary = Color.FromArgb(52, 44, 38),
                TextSecondary = Color.FromArgb(85, 75, 65),
                TextMuted = Color.FromArgb(120, 110, 100),
                Accent = Color.FromArgb(197, 128, 32),
                InputBg = Color.White,
                InputFg = Color.FromArgb(40, 35, 30),
                PnlP1Bg = Color.FromArgb(248, 245, 238),
                PnlP2Bg = Color.FromArgb(242, 246, 250),
                CardHeaderBg = Color.FromArgb(244, 240, 232),
                CardBg = Color.FromArgb(255, 254, 250),
                StatusBadgeBg = Color.FromArgb(242, 238, 230),
                StatusBadgeFg = Color.FromArgb(110, 100, 90),
                BannerBg = Color.FromArgb(254, 250, 240),
                BannerBorder = Color.FromArgb(235, 210, 175),
                BannerFg = Color.FromArgb(165, 80, 15),
                IsDark = false
            },
            ["모던 다크"] = new ThemeColors
            {
                FormBg = Color.FromArgb(28, 30, 33),
                BarBg = Color.FromArgb(20, 22, 25),
                GroupBg = Color.FromArgb(36, 38, 42),
                GroupBorder = Color.FromArgb(55, 58, 64),
                TextPrimary = Color.FromArgb(235, 238, 242),
                TextSecondary = Color.FromArgb(195, 200, 208),
                TextMuted = Color.FromArgb(150, 155, 165),
                Accent = Color.FromArgb(245, 158, 11),
                InputBg = Color.FromArgb(48, 51, 57),
                InputFg = Color.FromArgb(245, 245, 245),
                PnlP1Bg = Color.FromArgb(32, 34, 38),
                PnlP2Bg = Color.FromArgb(26, 38, 52),
                CardHeaderBg = Color.FromArgb(44, 46, 52),
                CardBg = Color.FromArgb(34, 36, 40),
                StatusBadgeBg = Color.FromArgb(44, 47, 53),
                StatusBadgeFg = Color.FromArgb(180, 185, 195),
                BannerBg = Color.FromArgb(48, 40, 30),
                BannerBorder = Color.FromArgb(120, 85, 30),
                BannerFg = Color.FromArgb(245, 185, 80),
                IsDark = true
            },
            ["쿨 블루"] = new ThemeColors
            {
                FormBg = Color.FromArgb(240, 245, 252),
                BarBg = Color.FromArgb(226, 234, 246),
                GroupBg = Color.FromArgb(252, 254, 255),
                GroupBorder = Color.FromArgb(210, 224, 242),
                TextPrimary = Color.FromArgb(24, 43, 73),
                TextSecondary = Color.FromArgb(45, 70, 105),
                TextMuted = Color.FromArgb(100, 120, 150),
                Accent = Color.FromArgb(28, 105, 205),
                InputBg = Color.White,
                InputFg = Color.FromArgb(20, 35, 60),
                PnlP1Bg = Color.FromArgb(235, 242, 252),
                PnlP2Bg = Color.FromArgb(228, 238, 250),
                CardHeaderBg = Color.FromArgb(230, 238, 248),
                CardBg = Color.FromArgb(255, 255, 255),
                StatusBadgeBg = Color.FromArgb(232, 240, 250),
                StatusBadgeFg = Color.FromArgb(60, 90, 130),
                BannerBg = Color.FromArgb(238, 244, 255),
                BannerBorder = Color.FromArgb(180, 205, 240),
                BannerFg = Color.FromArgb(25, 85, 175),
                IsDark = false
            },
            ["세이지 그린"] = new ThemeColors
            {
                FormBg = Color.FromArgb(242, 247, 243),
                BarBg = Color.FromArgb(228, 237, 230),
                GroupBg = Color.FromArgb(252, 255, 253),
                GroupBorder = Color.FromArgb(212, 228, 216),
                TextPrimary = Color.FromArgb(30, 52, 36),
                TextSecondary = Color.FromArgb(50, 78, 58),
                TextMuted = Color.FromArgb(100, 130, 110),
                Accent = Color.FromArgb(42, 130, 75),
                InputBg = Color.White,
                InputFg = Color.FromArgb(25, 45, 30),
                PnlP1Bg = Color.FromArgb(236, 245, 238),
                PnlP2Bg = Color.FromArgb(230, 243, 241),
                CardHeaderBg = Color.FromArgb(232, 242, 234),
                CardBg = Color.FromArgb(255, 255, 255),
                StatusBadgeBg = Color.FromArgb(234, 243, 236),
                StatusBadgeFg = Color.FromArgb(65, 105, 75),
                BannerBg = Color.FromArgb(240, 248, 242),
                BannerBorder = Color.FromArgb(185, 220, 195),
                BannerFg = Color.FromArgb(35, 115, 65),
                IsDark = false
            }
        };

        private ThemeColors _currentTheme;

        // ==========================================
        // UI Controls Fields
        // ==========================================
        // Top Bar
        private Panel topBar;
        private Label lblAppTitle;
        private Label lblTheme;
        private ComboBox cboTheme;

        // Section 1: Text
        private GroupBox grpText;
        private TextBox txtInput;
        private CheckBox chkSeparateText2;
        private TextBox txtInput2;
        private Panel pnlText2Row;
        private CheckBox chkEnter;
        private CheckBox chkClipboard;

        // Section 2: Coordinates
        private GroupBox grpCoord;
        private CheckBox chkEnablePoint2;
        private TableLayoutPanel pnlPoint1;
        private Label lblP1Title;
        private Label lblX1;
        private NumericUpDown numX;
        private Label lblY1;
        private NumericUpDown numY;
        private Button btnPickCoord;
        private Button btnGetCurCoord;

        private TableLayoutPanel pnlPoint2Controls;
        private Label lblP2Title;
        private Label lblX2;
        private NumericUpDown numX2;
        private Label lblY2;
        private NumericUpDown numY2;
        private Button btnPickCoord2;
        private Button btnGetCurCoord2;
        private Label lblPointInterval;
        private NumericUpDown numPointInterval;
        private Label lblPointIntervalUnit;

        private Label lblDelay;
        private NumericUpDown numClickDelay;
        private Label lblDelayUnit;

        // Section 3: Repeat & Duration
        private GroupBox grpRepeat;
        private Label lblStartDelay;
        private RadioButton rbStartImmediate;
        private RadioButton rbStartDelay;
        private NumericUpDown numStartDelay;
        private Label lblStartDelayUnit;

        private Label lblInterval;
        private NumericUpDown numInterval;
        private ComboBox cboIntervalUnit;
        private Label lblIntervalUnit;

        private CheckBox chkEnableDuration;
        private NumericUpDown numDuration;
        private ComboBox cboDurationUnit;
        private Label lblDurationUnit;

        private CheckBox chkEnableCount;
        private NumericUpDown numCount;
        private Label lblCountUnit;
        private Label lblModeHint;

        // Section 4: Action & Shortcuts
        private GroupBox grpControl;
        private Button btnStart;
        private Button btnStop;
        private Label lblGuide;

        // Custom Hotkey Controls
        private TableLayoutPanel pnlHotkeys;
        private Label lblStartHk;
        private ComboBox cboStartMod;
        private Label lblPlus1;
        private ComboBox cboStartKey;

        private Label lblStopHk;
        private ComboBox cboStopMod;
        private Label lblPlus2;
        private ComboBox cboStopKey;

        private Label lblEmergencyHk;
        private ComboBox cboEmergencyMod;
        private Label lblPlus3;
        private ComboBox cboEmergencyKey;

        private Label lblHkTip;
        private Button btnResetHotkeys;

        private bool _isUpdatingHotkeyUI = false;

        private static readonly string[] ModifierOptions = new[]
        {
            "없음", "Alt", "Ctrl", "Shift", "Ctrl + Alt", "Ctrl + Shift", "Alt + Shift"
        };

        private static readonly string[] KeyOptions = new[]
        {
            "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12",
            "Pause", "Escape", "Space", "Tab", "Insert", "Delete", "Home", "End", "PageUp", "PageDown",
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z",
            "NumPad0", "NumPad1", "NumPad2", "NumPad3", "NumPad4", "NumPad5", "NumPad6", "NumPad7", "NumPad8", "NumPad9"
        };

        // Section 5: Dashboard / Status
        private GroupBox grpStatus;
        private Panel cardRemainCount;
        private Label lblCardTitle1;
        private Label lblRemainCount;
        private Panel cardRemainTime;
        private Label lblCardTitle2;
        private Label lblRemainTime;
        private Panel cardCompleted;
        private Label lblCardTitle3;
        private Label lblCompletedCount;

        private Label lblEnergyTitle;
        private EnergyBar energyBar;
        private Label lblStatus;

        // Bottom Bar
        private Panel bottomBar;
        private Button btnTopMost;
        private Button btnToggleHotkeys;

        // App Logic State
        private GlobalKeyboardHook _keyboardHook;
        private CancellationTokenSource _cts;
        private bool _isRunning = false;
        private bool _isEmergencyStop = false;
        private bool _hotkeysEnabled = true;
        private readonly AppSettings _settings;

        public MainForm()
        {
            _isUpdatingHotkeyUI = true;
            _settings = AppSettings.Load();
            InitializeComponent();
            ApplySettingsToUI();
            SetupGlobalKeyboardHook();
            _isUpdatingHotkeyUI = false;
        }

        private void InitializeComponent()
        {
            Text = "RD 오토 입력 툴 (Auto Input Tool)";
            AutoScaleMode = AutoScaleMode.Dpi;
            Size = new Size(620, 1160);
            MinimumSize = new Size(560, 860);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Malgun Gothic", 9f, FontStyle.Regular);

            // ==========================================
            // Top Bar: Title & Theme Selector
            // ==========================================
            topBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 44,
                Padding = new Padding(12, 6, 12, 6)
            };

            lblAppTitle = new Label
            {
                Text = "⚡ RD 오토 입력 툴 (Auto Input Tool)",
                Dock = DockStyle.Left,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 10.5f, FontStyle.Bold),
                Padding = new Padding(0, 5, 0, 0)
            };

            var themeFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0)
            };

            lblTheme = new Label
            {
                Text = "🎨 테마 :",
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Padding = new Padding(0, 6, 4, 0)
            };

            cboTheme = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 115,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Margin = new Padding(0, 2, 0, 0),
                Cursor = Cursors.Hand
            };
            cboTheme.Items.AddRange(new object[] { "아이보리 웜", "모던 다크", "쿨 블루", "세이지 그린" });
            cboTheme.SelectedIndex = 0;
            cboTheme.SelectedIndexChanged += (s, e) =>
            {
                string selected = cboTheme.SelectedItem?.ToString() ?? "아이보리 웜";
                _settings.SelectedTheme = selected;
                _settings.Save();
                ApplyTheme(selected);
            };

            themeFlow.Controls.Add(lblTheme);
            themeFlow.Controls.Add(cboTheme);

            topBar.Controls.Add(lblAppTitle);
            topBar.Controls.Add(themeFlow);

            // ==========================================
            // Bottom Panel: Side-by-side TopMost & Hotkeys Toggle Buttons
            // ==========================================
            bottomBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 48,
                Padding = new Padding(10, 6, 10, 8)
            };

            var bottomTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0)
            };
            bottomTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            bottomTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnTopMost = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 4, 0)
            };
            btnTopMost.FlatAppearance.BorderSize = 0;
            btnTopMost.Click += (s, e) =>
            {
                bool currentActual = IsHandleCreated ? NativeMethods.IsWindowTopMost(Handle) : TopMost;
                bool newTop = !currentActual;
                ApplyTopMost(newTop);
                _settings.AlwaysOnTop = newTop;
                _settings.Save();
                SystemSounds.Beep.Play();
                lblStatus.Text = newTop
                    ? "📌 창 항상 위: [ON] 활성화됨 (다른 창 위에 고정)"
                    : "📌 창 항상 위: [OFF] 비활성화됨 (일반 창 모드)";
            };

            btnToggleHotkeys = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 0, 0, 0)
            };
            btnToggleHotkeys.FlatAppearance.BorderSize = 0;
            btnToggleHotkeys.Click += (s, e) =>
            {
                _hotkeysEnabled = !_hotkeysEnabled;
                _settings.EnableHotkeys = _hotkeysEnabled;
                _settings.Save();
                UpdateHotkeyButtonUI();
                SystemSounds.Beep.Play();
                lblStatus.Text = _hotkeysEnabled
                    ? $"단축키 활성화됨 ({_settings.HotkeyStart}: 시작, {_settings.HotkeyStop}: 정지, {_settings.HotkeyEmergency}: 긴급탈출)"
                    : $"단축키 비활성 중 (긴급탈출 단축키 {_settings.HotkeyEmergency}는 항상 동작)";
            };

            bottomTable.Controls.Add(btnTopMost, 0, 0);
            bottomTable.Controls.Add(btnToggleHotkeys, 1, 0);
            bottomBar.Controls.Add(bottomTable);

            // ==========================================
            // Root Scrollable Panel
            // ==========================================
            var rootPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(12, 10, 12, 8)
            };

            // Main Vertical Stack Layout
            var contentLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 5,
                Padding = new Padding(0)
            };
            contentLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // ==========================================
            // 1. 원하는 문구 지정 그룹 (Text Input)
            // ==========================================
            grpText = new GroupBox
            {
                Text = " 1. 원하는 문구 지정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold)
            };

            var textInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            textInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            txtInput = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Height = 52,
                Dock = DockStyle.Top,
                Font = new Font("Malgun Gothic", 9.5f),
                Margin = new Padding(0, 2, 0, 4)
            };

            var optLayout = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                Margin = new Padding(0, 2, 0, 2)
            };

            chkEnter = new CheckBox
            {
                Text = "입력 후 Enter 전송",
                AutoSize = true,
                Checked = true,
                Margin = new Padding(0, 0, 14, 0),
                Cursor = Cursors.Hand
            };

            chkClipboard = new CheckBox
            {
                Text = "클립보드 붙여넣기 (한글/특수문자 완벽 호환)",
                AutoSize = true,
                Checked = true,
                Margin = new Padding(0),
                Cursor = Cursors.Hand
            };

            optLayout.Controls.Add(chkEnter);
            optLayout.Controls.Add(chkClipboard);

            // Separate Text 2 Row (visible when 2-point mode is enabled)
            pnlText2Row = new Panel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Visible = false,
                Margin = new Padding(0, 4, 0, 2)
            };
            chkSeparateText2 = new CheckBox
            {
                Text = "포인트 2에 별도 문구 사용 (미체크 시 위 문구와 동일하게 입력)",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 0, 2)
            };
            txtInput2 = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Height = 44,
                Dock = DockStyle.Top,
                Font = new Font("Malgun Gothic", 9.5f),
                Enabled = false,
                Margin = new Padding(0, 2, 0, 4)
            };
            chkSeparateText2.CheckedChanged += (s, e) =>
            {
                txtInput2.Enabled = chkSeparateText2.Checked;
                txtInput2.BackColor = chkSeparateText2.Checked
                    ? (_currentTheme?.InputBg ?? Color.White)
                    : (_currentTheme?.IsDark == true ? Color.FromArgb(40, 42, 46) : Color.FromArgb(245, 242, 237));
            };
            pnlText2Row.Controls.Add(txtInput2);
            pnlText2Row.Controls.Add(chkSeparateText2);

            textInner.Controls.Add(txtInput);
            textInner.Controls.Add(optLayout);
            textInner.Controls.Add(pnlText2Row);
            grpText.Controls.Add(textInner);

            // ==========================================
            // 2. 좌표 지정 그룹 (Coordinate)
            // ==========================================
            grpCoord = new GroupBox
            {
                Text = " 2. 입력 포인트 (좌표) 지정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold)
            };

            var coordInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 4,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            coordInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // 0) Toggle Option: 2 Points Mode
            chkEnablePoint2 = new CheckBox
            {
                Text = "입력 포인트 2개로 확장 (1초 간격 순차 입력)",
                Dock = DockStyle.Top,
                AutoSize = true,
                Checked = false,
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                Margin = new Padding(0, 2, 0, 8),
                Cursor = Cursors.Hand
            };

            // 1) Point 1 Panel (TableLayoutPanel with proper spacing)
            pnlPoint1 = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10, 8, 10, 10),
                Margin = new Padding(0, 0, 0, 8)
            };
            pnlPoint1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            lblP1Title = new Label
            {
                Text = "📍 포인트 1 (P1)",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 6)
            };

            var coord1BtnRow = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 10)
            };
            coord1BtnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            coord1BtnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnPickCoord = new Button
            {
                Text = "🎯 P1 좌표 지정",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 3, 0)
            };
            btnPickCoord.FlatAppearance.BorderSize = 0;
            btnPickCoord.Click += (s, e) => PickCoordinate(numX, numY, "포인트 1");

            btnGetCurCoord = new Button
            {
                Text = "📍 P1 현재 위치 등록",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(3, 0, 0, 0)
            };
            btnGetCurCoord.FlatAppearance.BorderSize = 0;
            btnGetCurCoord.Click += (s, e) => CaptureCurrentMouse(numX, numY, "포인트 1");

            coord1BtnRow.Controls.Add(btnPickCoord, 0, 0);
            coord1BtnRow.Controls.Add(btnGetCurCoord, 1, 0);

            var coord1Inputs = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 2),
                Padding = new Padding(0, 2, 0, 0)
            };
            lblX1 = new Label { Text = "X:", AutoSize = true, Padding = new Padding(0, 4, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numX = new NumericUpDown { Minimum = -10000, Maximum = 10000, Value = 500, Width = 75, Margin = new Padding(2, 0, 14, 0) };
            lblY1 = new Label { Text = "Y:", AutoSize = true, Padding = new Padding(0, 4, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numY = new NumericUpDown { Minimum = -10000, Maximum = 10000, Value = 500, Width = 75, Margin = new Padding(2, 0, 0, 0) };
            coord1Inputs.Controls.AddRange(new Control[] { lblX1, numX, lblY1, numY });

            pnlPoint1.Controls.Add(lblP1Title, 0, 0);
            pnlPoint1.Controls.Add(coord1BtnRow, 0, 1);
            pnlPoint1.Controls.Add(coord1Inputs, 0, 2);

            // 2) Point 2 Panel (Collapsible TableLayoutPanel with proper spacing)
            pnlPoint2Controls = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10, 8, 10, 10),
                Margin = new Padding(0, 0, 0, 8),
                Visible = false
            };
            pnlPoint2Controls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            lblP2Title = new Label
            {
                Text = "📍 포인트 2 (P2) - 1초 간격 순차 입력",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Margin = new Padding(0, 0, 0, 6)
            };

            var coord2BtnRow = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 10)
            };
            coord2BtnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            coord2BtnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnPickCoord2 = new Button
            {
                Text = "🎯 P2 좌표 지정",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 3, 0)
            };
            btnPickCoord2.FlatAppearance.BorderSize = 0;
            btnPickCoord2.Click += (s, e) => PickCoordinate(numX2, numY2, "포인트 2");

            btnGetCurCoord2 = new Button
            {
                Text = "📍 P2 현재 위치 등록",
                Dock = DockStyle.Fill,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(3, 0, 0, 0)
            };
            btnGetCurCoord2.FlatAppearance.BorderSize = 0;
            btnGetCurCoord2.Click += (s, e) => CaptureCurrentMouse(numX2, numY2, "포인트 2");

            coord2BtnRow.Controls.Add(btnPickCoord2, 0, 0);
            coord2BtnRow.Controls.Add(btnGetCurCoord2, 1, 0);

            var coord2Inputs = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 0, 0, 2),
                Padding = new Padding(0, 2, 0, 0)
            };
            lblX2 = new Label { Text = "X:", AutoSize = true, Padding = new Padding(0, 4, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numX2 = new NumericUpDown { Minimum = -10000, Maximum = 10000, Value = 600, Width = 75, Margin = new Padding(2, 0, 14, 0) };
            lblY2 = new Label { Text = "Y:", AutoSize = true, Padding = new Padding(0, 4, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numY2 = new NumericUpDown { Minimum = -10000, Maximum = 10000, Value = 600, Width = 75, Margin = new Padding(2, 0, 0, 0) };
            coord2Inputs.Controls.AddRange(new Control[] { lblX2, numX2, lblY2, numY2 });

            pnlPoint2Controls.Controls.Add(lblP2Title, 0, 0);
            pnlPoint2Controls.Controls.Add(coord2BtnRow, 0, 1);
            pnlPoint2Controls.Controls.Add(coord2Inputs, 0, 2);

            // 3) Common Timing Options Row
            var timingRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 2, 0, 2)
            };

            lblPointInterval = new Label { Text = "P1→P2 간격 :", AutoSize = true, Padding = new Padding(0, 3, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold), Visible = false };
            numPointInterval = new NumericUpDown { DecimalPlaces = 1, Increment = 0.1m, Minimum = 0.1m, Maximum = 3600m, Value = 1.0m, Width = 65, Margin = new Padding(2, 0, 4, 0), Visible = false };
            lblPointIntervalUnit = new Label { Text = "초", AutoSize = true, Padding = new Padding(0, 3, 14, 0), Visible = false };

            lblDelay = new Label { Text = "클릭 후 대기 :", AutoSize = true, Padding = new Padding(0, 3, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Regular) };
            numClickDelay = new NumericUpDown { Minimum = 10, Maximum = 5000, Value = 100, Width = 65, Margin = new Padding(2, 0, 4, 0) };
            lblDelayUnit = new Label { Text = "ms", AutoSize = true, Padding = new Padding(0, 3, 0, 0) };

            timingRow.Controls.AddRange(new Control[] { lblPointInterval, numPointInterval, lblPointIntervalUnit, lblDelay, numClickDelay, lblDelayUnit });

            chkEnablePoint2.CheckedChanged += (s, e) =>
            {
                pnlPoint2Controls.Visible = chkEnablePoint2.Checked;
                pnlText2Row.Visible = chkEnablePoint2.Checked;
                lblPointInterval.Visible = chkEnablePoint2.Checked;
                numPointInterval.Visible = chkEnablePoint2.Checked;
                lblPointIntervalUnit.Visible = chkEnablePoint2.Checked;
            };

            coordInner.Controls.Add(chkEnablePoint2);
            coordInner.Controls.Add(pnlPoint1);
            coordInner.Controls.Add(pnlPoint2Controls);
            coordInner.Controls.Add(timingRow);
            grpCoord.Controls.Add(coordInner);

            // ==========================================
            // 3. 반복 간격 및 지속 시간 그룹 (Repeat & Duration)
            // ==========================================
            grpRepeat = new GroupBox
            {
                Text = " 3. 반복 간격 및 지속 시간 지정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold)
            };

            var repeatInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 5,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            repeatInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // 0) Start Delay Row
            var startDelayRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 4)
            };
            lblStartDelay = new Label
            {
                Text = "시작 대기 :",
                AutoSize = true,
                Padding = new Padding(0, 3, 0, 0),
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold)
            };
            rbStartImmediate = new RadioButton
            {
                Text = "바로 시작 (0초)",
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Margin = new Padding(4, 1, 8, 0),
                Cursor = Cursors.Hand
            };
            rbStartDelay = new RadioButton
            {
                Text = "대기 후 시작 :",
                AutoSize = true,
                Checked = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Margin = new Padding(0, 1, 2, 0),
                Cursor = Cursors.Hand
            };
            numStartDelay = new NumericUpDown
            {
                DecimalPlaces = 1,
                Increment = 1m,
                Minimum = 0.5m,
                Maximum = 60m,
                Value = 2.0m,
                Width = 60,
                Margin = new Padding(2, 0, 4, 0)
            };
            lblStartDelayUnit = new Label
            {
                Text = "초 후 시작",
                AutoSize = true,
                Padding = new Padding(0, 3, 0, 0)
            };

            rbStartImmediate.CheckedChanged += (s, e) =>
            {
                numStartDelay.Enabled = !rbStartImmediate.Checked;
                lblStartDelayUnit.ForeColor = !rbStartImmediate.Checked
                    ? (_currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                    : (_currentTheme?.TextMuted ?? Color.FromArgb(160, 150, 140));
            };
            rbStartDelay.CheckedChanged += (s, e) =>
            {
                numStartDelay.Enabled = rbStartDelay.Checked;
                lblStartDelayUnit.ForeColor = rbStartDelay.Checked
                    ? (_currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                    : (_currentTheme?.TextMuted ?? Color.FromArgb(160, 150, 140));
            };
            startDelayRow.Controls.AddRange(new Control[] { lblStartDelay, rbStartImmediate, rbStartDelay, numStartDelay, lblStartDelayUnit });

            // 1) Interval Row (with Seconds / Minutes / Hours ComboBox)
            var intervalRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 3)
            };
            lblInterval = new Label { Text = "반복 간격 :", AutoSize = true, Padding = new Padding(0, 3, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numInterval = new NumericUpDown
            {
                DecimalPlaces = 2,
                Increment = 0.1m,
                Minimum = 0.05m,
                Maximum = 86400m,
                Value = 1.0m,
                Width = 72,
                Margin = new Padding(4, 0, 4, 0)
            };

            cboIntervalUnit = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 65,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Margin = new Padding(0, 0, 6, 0),
                Cursor = Cursors.Hand
            };
            cboIntervalUnit.Items.AddRange(new object[] { "초", "분", "시간" });
            cboIntervalUnit.SelectedIndex = 0;
            cboIntervalUnit.SelectedIndexChanged += (s, e) =>
            {
                string unit = cboIntervalUnit.SelectedItem?.ToString() ?? "초";
                if (unit == "분")
                {
                    numInterval.Minimum = 0.01m;
                    numInterval.Maximum = 1440m;
                    numInterval.Increment = 0.1m;
                }
                else if (unit == "시간")
                {
                    numInterval.Minimum = 0.01m;
                    numInterval.Maximum = 24m;
                    numInterval.Increment = 0.1m;
                }
                else
                {
                    numInterval.Minimum = 0.05m;
                    numInterval.Maximum = 86400m;
                    numInterval.Increment = 0.5m;
                }
                numInterval.Value = Math.Clamp(numInterval.Value, numInterval.Minimum, numInterval.Maximum);
                _settings.IntervalUnit = unit;
                UpdateModeHint();
            };

            lblIntervalUnit = new Label { Text = "마다 입력", AutoSize = true, Padding = new Padding(0, 3, 0, 0) };
            intervalRow.Controls.AddRange(new Control[] { lblInterval, numInterval, cboIntervalUnit, lblIntervalUnit });

            // 2) Duration Toggle Row (with Seconds / Minutes / Hours ComboBox)
            var durationRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 3)
            };
            chkEnableDuration = new CheckBox
            {
                Text = "지속 시간 지정 (ON/OFF) :",
                AutoSize = true,
                Checked = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Margin = new Padding(0, 2, 4, 0),
                Cursor = Cursors.Hand
            };
            numDuration = new NumericUpDown
            {
                DecimalPlaces = 1,
                Increment = 1m,
                Minimum = 0.5m,
                Maximum = 86400m,
                Value = 10.0m,
                Width = 72,
                Margin = new Padding(2, 0, 4, 0)
            };

            cboDurationUnit = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 65,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Margin = new Padding(0, 0, 6, 0),
                Cursor = Cursors.Hand
            };
            cboDurationUnit.Items.AddRange(new object[] { "초", "분", "시간" });
            cboDurationUnit.SelectedIndex = 0;
            cboDurationUnit.SelectedIndexChanged += (s, e) =>
            {
                string unit = cboDurationUnit.SelectedItem?.ToString() ?? "초";
                if (unit == "시간")
                {
                    numDuration.Minimum = 0.01m;
                    numDuration.Maximum = 720m;
                    numDuration.Increment = 0.1m;
                    numDuration.DecimalPlaces = 2;
                }
                else if (unit == "분")
                {
                    numDuration.Minimum = 0.1m;
                    numDuration.Maximum = 1440m;
                    numDuration.Increment = 0.5m;
                    numDuration.DecimalPlaces = 1;
                }
                else
                {
                    numDuration.Minimum = 0.5m;
                    numDuration.Maximum = 86400m;
                    numDuration.Increment = 1m;
                    numDuration.DecimalPlaces = 1;
                }
                numDuration.Value = Math.Clamp(numDuration.Value, numDuration.Minimum, numDuration.Maximum);
                lblDurationUnit.Text = chkEnableDuration.Checked ? $"{unit} 동안 반복 후 자동 종료" : "(꺼짐: 시간 제한 없이 계속)";
                _settings.DurationUnit = unit;
                UpdateModeHint();
                RefreshDashboardInitialValues();
            };

            lblDurationUnit = new Label
            {
                Text = "초 동안 반복 후 자동 종료",
                AutoSize = true,
                Padding = new Padding(0, 3, 0, 0)
            };

            chkEnableDuration.CheckedChanged += (s, e) =>
            {
                string unit = cboDurationUnit.SelectedItem?.ToString() ?? "초";
                numDuration.Enabled = chkEnableDuration.Checked;
                cboDurationUnit.Enabled = chkEnableDuration.Checked;
                lblDurationUnit.Text = chkEnableDuration.Checked ? $"{unit} 동안 반복 후 자동 종료" : "(꺼짐: 시간 제한 없이 계속)";
                lblDurationUnit.ForeColor = chkEnableDuration.Checked
                    ? (_currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                    : (_currentTheme?.TextMuted ?? Color.FromArgb(160, 150, 140));
                UpdateModeHint();
                RefreshDashboardInitialValues();
            };
            durationRow.Controls.AddRange(new Control[] { chkEnableDuration, numDuration, cboDurationUnit, lblDurationUnit });

            // 3) Count Toggle Row
            var countRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 3)
            };
            chkEnableCount = new CheckBox
            {
                Text = "반복 횟수 지정 (ON/OFF) :",
                AutoSize = true,
                Checked = false,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Margin = new Padding(0, 2, 4, 0),
                Cursor = Cursors.Hand
            };
            numCount = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 1000000,
                Value = 10,
                Width = 75,
                Enabled = false,
                Margin = new Padding(2, 0, 6, 0)
            };
            lblCountUnit = new Label
            {
                Text = "(꺼짐: 횟수 제한 없음)",
                AutoSize = true,
                Padding = new Padding(0, 3, 0, 0)
            };

            chkEnableCount.CheckedChanged += (s, e) =>
            {
                numCount.Enabled = chkEnableCount.Checked;
                lblCountUnit.Text = chkEnableCount.Checked ? "회 입력 후 자동 종료" : "(꺼짐: 횟수 제한 없음)";
                lblCountUnit.ForeColor = chkEnableCount.Checked
                    ? (_currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                    : (_currentTheme?.TextMuted ?? Color.FromArgb(160, 150, 140));
                UpdateModeHint();
                RefreshDashboardInitialValues();
            };
            countRow.Controls.AddRange(new Control[] { chkEnableCount, numCount, lblCountUnit });

            // 4) Live Mode Hint
            lblModeHint = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(2, 1, 2, 2),
                Font = new Font("Malgun Gothic", 8.5f, FontStyle.Bold)
            };

            repeatInner.Controls.Add(startDelayRow);
            repeatInner.Controls.Add(intervalRow);
            repeatInner.Controls.Add(durationRow);
            repeatInner.Controls.Add(countRow);
            repeatInner.Controls.Add(lblModeHint);
            grpRepeat.Controls.Add(repeatInner);

            // ==========================================
            // 4. 실행 및 단축키 설정 그룹 (Controls)
            // ==========================================
            grpControl = new GroupBox
            {
                Text = " 4. 실행 및 단축키 설정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold)
            };

            var ctrlInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 3,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            ctrlInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Start / Stop Buttons Table (Side-by-Side 50% / 50%)
            var btnTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 42,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 1, 0, 6)
            };
            btnTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnStart = new Button
            {
                Text = "▶ 시작",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(46, 139, 87),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 4, 0)
            };
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Click += (s, e) => StartAutoInput();

            btnStop = new Button
            {
                Text = "⏹ 정지",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(205, 65, 55),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 11f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(4, 0, 0, 0)
            };
            btnStop.FlatAppearance.BorderSize = 0;
            btnStop.Click += (s, e) => StopAutoInput();

            btnTable.Controls.Add(btnStart, 0, 0);
            btnTable.Controls.Add(btnStop, 1, 0);

            // Warning / Guide Banner for Shortcuts
            lblGuide = new Label
            {
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(0, 0, 0, 6)
            };

            // Hotkey Customization Panel (Dropdowns only)
            pnlHotkeys = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 5,
                RowCount = 4,
                Margin = new Padding(0, 2, 0, 2),
                Padding = new Padding(2, 2, 2, 2)
            };
            pnlHotkeys.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 135f)); // Title
            pnlHotkeys.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f)); // Modifier Dropdown
            pnlHotkeys.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 24f));  // +
            pnlHotkeys.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120f)); // Key Dropdown
            pnlHotkeys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));  // Reset button / spacing

            // Row 0: Start Hotkey
            lblStartHk = new Label { Text = "▶ 시작 단축키 :", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            cboStartMod = CreateModCombo();
            lblPlus1 = new Label { Text = "+", AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            cboStartKey = CreateKeyCombo();

            pnlHotkeys.Controls.Add(lblStartHk, 0, 0);
            pnlHotkeys.Controls.Add(cboStartMod, 1, 0);
            pnlHotkeys.Controls.Add(lblPlus1, 2, 0);
            pnlHotkeys.Controls.Add(cboStartKey, 3, 0);

            // Row 1: Stop Hotkey
            lblStopHk = new Label { Text = "⏹ 정지 단축키 :", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            cboStopMod = CreateModCombo();
            lblPlus2 = new Label { Text = "+", AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            cboStopKey = CreateKeyCombo();

            pnlHotkeys.Controls.Add(lblStopHk, 0, 1);
            pnlHotkeys.Controls.Add(cboStopMod, 1, 1);
            pnlHotkeys.Controls.Add(lblPlus2, 2, 1);
            pnlHotkeys.Controls.Add(cboStopKey, 3, 1);

            // Row 2: Emergency Hotkey (Default: Alt + F4)
            lblEmergencyHk = new Label { Text = "🚨 긴급탈출 단축키 :", AutoSize = true, Anchor = AnchorStyles.Left, Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            cboEmergencyMod = CreateModCombo();
            lblPlus3 = new Label { Text = "+", AutoSize = false, TextAlign = ContentAlignment.MiddleCenter, Dock = DockStyle.Fill, Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            cboEmergencyKey = CreateKeyCombo();

            pnlHotkeys.Controls.Add(lblEmergencyHk, 0, 2);
            pnlHotkeys.Controls.Add(cboEmergencyMod, 1, 2);
            pnlHotkeys.Controls.Add(lblPlus3, 2, 2);
            pnlHotkeys.Controls.Add(cboEmergencyKey, 3, 2);

            // Row 3: Bottom info & Reset button
            lblHkTip = new Label
            {
                Text = "※ 드롭다운 선택 시 즉시 저장 및 반영됩니다. (긴급탈출은 단축키 OFF 시에도 항상 동작)",
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Font = new Font("Malgun Gothic", 8.25f, FontStyle.Regular),
                Margin = new Padding(0, 4, 0, 0)
            };
            btnResetHotkeys = new Button
            {
                Text = "🔄 기본값",
                Dock = DockStyle.Fill,
                Height = 26,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 8.5f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 2, 0, 0)
            };
            btnResetHotkeys.FlatAppearance.BorderSize = 0;
            btnResetHotkeys.Click += (s, e) => ResetHotkeysToDefault();

            pnlHotkeys.Controls.Add(lblHkTip, 0, 3);
            pnlHotkeys.SetColumnSpan(lblHkTip, 4);
            pnlHotkeys.Controls.Add(btnResetHotkeys, 4, 3);

            ctrlInner.Controls.Add(btnTable);
            ctrlInner.Controls.Add(lblGuide);
            ctrlInner.Controls.Add(pnlHotkeys);
            grpControl.Controls.Add(ctrlInner);

            // ==========================================
            // 5. 실시간 기동 상태 대시보드 (Dashboard)
            // ==========================================
            grpStatus = new GroupBox
            {
                Text = " 5. 실시간 기동 상태 대시보드 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(12, 10, 12, 12),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold)
            };

            var statusInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 4,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            statusInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Stat Cards Row: Height 86px
            var statsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 86,
                ColumnCount = 3,
                RowCount = 1,
                Margin = new Padding(0, 0, 0, 8)
            };
            statsGrid.ColumnStyles.Clear();
            statsGrid.RowStyles.Clear();
            statsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            statsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            statsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34f));
            statsGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            cardRemainCount = CreateStatCard("🎯 남은 횟수", out lblRemainCount, out lblCardTitle1, "무제한", Color.FromArgb(180, 100, 25));
            cardRemainCount.Margin = new Padding(0, 0, 4, 0);

            cardRemainTime = CreateStatCard("⏳ 남은 시간", out lblRemainTime, out lblCardTitle2, "10.0초", Color.FromArgb(140, 70, 130));
            cardRemainTime.Margin = new Padding(2, 0, 2, 0);

            cardCompleted = CreateStatCard("📊 입력 완료", out lblCompletedCount, out lblCardTitle3, "0회", Color.FromArgb(46, 139, 87));
            cardCompleted.Margin = new Padding(4, 0, 0, 0);

            statsGrid.Controls.Add(cardRemainCount, 0, 0);
            statsGrid.Controls.Add(cardRemainTime, 1, 0);
            statsGrid.Controls.Add(cardCompleted, 2, 0);

            // Energy Bar Header
            lblEnergyTitle = new Label
            {
                Text = "⚡ 실시간 기동 에너지 바 (다음 입력 충전 게이지):",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Margin = new Padding(0, 2, 0, 4)
            };

            // Energy Bar Control
            energyBar = new EnergyBar
            {
                Dock = DockStyle.Top,
                Height = 34,
                Margin = new Padding(0, 0, 0, 8)
            };

            // Status Description Text
            lblStatus = new Label
            {
                Text = "상태: 준비 완료 (시작을 누르면 동작합니다)",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(2, 0, 0, 0)
            };

            statusInner.Controls.Add(statsGrid);
            statusInner.Controls.Add(lblEnergyTitle);
            statusInner.Controls.Add(energyBar);
            statusInner.Controls.Add(lblStatus);
            grpStatus.Controls.Add(statusInner);

            // Add all groupboxes to content layout
            contentLayout.Controls.Add(grpText, 0, 0);
            contentLayout.Controls.Add(grpCoord, 0, 1);
            contentLayout.Controls.Add(grpRepeat, 0, 2);
            contentLayout.Controls.Add(grpControl, 0, 3);
            contentLayout.Controls.Add(grpStatus, 0, 4);

            rootPanel.Controls.Add(contentLayout);

            // Form Controls docking: rootPanel fills the middle area
            Controls.Add(rootPanel);
            Controls.Add(topBar);
            Controls.Add(bottomBar);
            rootPanel.BringToFront();

            FormClosing += MainForm_FormClosing;
        }

        private Panel CreateStatCard(string title, out Label valueLabel, out Label titleLabel, string initialValue, Color valueColor)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };

            titleLabel = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            valueLabel = new Label
            {
                Text = initialValue,
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 14f, FontStyle.Bold),
                ForeColor = valueColor,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(valueLabel);
            card.Controls.Add(titleLabel);
            titleLabel.BringToFront();
            return card;
        }

        private static string ModToDisplay(string mod)
        {
            if (string.IsNullOrEmpty(mod) || mod.Equals("None", StringComparison.OrdinalIgnoreCase) || mod == "없음")
                return "없음";
            if (mod.Equals("Ctrl+Alt", StringComparison.OrdinalIgnoreCase)) return "Ctrl + Alt";
            if (mod.Equals("Ctrl+Shift", StringComparison.OrdinalIgnoreCase)) return "Ctrl + Shift";
            if (mod.Equals("Alt+Shift", StringComparison.OrdinalIgnoreCase)) return "Alt + Shift";
            return mod;
        }

        private static string ModFromDisplay(string display)
        {
            if (string.IsNullOrEmpty(display) || display == "없음" || display.Equals("None", StringComparison.OrdinalIgnoreCase))
                return "None";
            if (display == "Ctrl + Alt") return "Ctrl+Alt";
            if (display == "Ctrl + Shift") return "Ctrl+Shift";
            if (display == "Alt + Shift") return "Alt+Shift";
            return display;
        }

        private ComboBox CreateModCombo()
        {
            var cbo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 0, 2)
            };
            cbo.Items.AddRange(ModifierOptions);
            cbo.SelectedIndex = 0;
            cbo.SelectedIndexChanged += (s, e) => OnHotkeyControlChanged();
            return cbo;
        }

        private ComboBox CreateKeyCombo()
        {
            var cbo = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 2, 0, 2)
            };
            cbo.Items.AddRange(KeyOptions);
            cbo.SelectedIndex = 0;
            cbo.SelectedIndexChanged += (s, e) => OnHotkeyControlChanged();
            return cbo;
        }

        private void OnHotkeyControlChanged()
        {
            if (_isUpdatingHotkeyUI ||
                cboStartMod == null || cboStartKey == null ||
                cboStopMod == null || cboStopKey == null ||
                cboEmergencyMod == null || cboEmergencyKey == null)
            {
                return;
            }

            string startMod = ModFromDisplay(cboStartMod.SelectedItem?.ToString());
            string startKey = cboStartKey.SelectedItem?.ToString() ?? "F1";

            string stopMod = ModFromDisplay(cboStopMod.SelectedItem?.ToString());
            string stopKey = cboStopKey.SelectedItem?.ToString() ?? "F2";

            string emgMod = ModFromDisplay(cboEmergencyMod.SelectedItem?.ToString());
            string emgKey = cboEmergencyKey.SelectedItem?.ToString() ?? "F4";

            _settings.HotkeyStart = new HotkeyConfig(startMod, startKey);
            _settings.HotkeyStop = new HotkeyConfig(stopMod, stopKey);
            _settings.HotkeyEmergency = new HotkeyConfig(emgMod, emgKey);
            _settings.Save();

            UpdateHotkeyButtonUI();
            UpdateModeHint();
        }

        private void ResetHotkeysToDefault()
        {
            _isUpdatingHotkeyUI = true;
            cboStartMod.SelectedItem = "Alt";
            cboStartKey.SelectedItem = "F1";

            cboStopMod.SelectedItem = "없음";
            cboStopKey.SelectedItem = "F2";

            cboEmergencyMod.SelectedItem = "Alt";
            cboEmergencyKey.SelectedItem = "F4";
            _isUpdatingHotkeyUI = false;

            OnHotkeyControlChanged();
            SystemSounds.Beep.Play();
            lblStatus.Text = "단축키가 기본값으로 복원되었습니다. (시작: Alt+F1, 정지: F2, 긴급탈출: Alt+F4)";
        }

        // ==========================================
        // Theme Engine
        // ==========================================
        private void ApplyTheme(string themeName)
        {
            if (string.IsNullOrEmpty(themeName) || !Themes.TryGetValue(themeName, out var theme))
            {
                theme = Themes["아이보리 웜"];
                themeName = "아이보리 웜";
            }
            _currentTheme = theme;

            BackColor = theme.FormBg;

            // TopBar
            topBar.BackColor = theme.BarBg;
            lblAppTitle.ForeColor = theme.TextPrimary;
            lblTheme.ForeColor = theme.TextSecondary;
            cboTheme.BackColor = theme.InputBg;
            cboTheme.ForeColor = theme.InputFg;

            // BottomBar
            bottomBar.BackColor = theme.BarBg;

            // GroupBoxes
            GroupBox[] groups = { grpText, grpCoord, grpRepeat, grpControl, grpStatus };
            foreach (var grp in groups)
            {
                grp.BackColor = theme.GroupBg;
                grp.ForeColor = theme.TextPrimary;
            }

            // Section 1 Controls
            txtInput.BackColor = theme.InputBg;
            txtInput.ForeColor = theme.InputFg;
            chkEnter.ForeColor = theme.TextPrimary;
            chkClipboard.ForeColor = theme.TextPrimary;
            chkSeparateText2.ForeColor = theme.TextSecondary;
            txtInput2.BackColor = chkSeparateText2.Checked
                ? theme.InputBg
                : (theme.IsDark ? Color.FromArgb(40, 42, 46) : Color.FromArgb(245, 242, 237));
            txtInput2.ForeColor = theme.InputFg;

            // Section 2 Controls
            chkEnablePoint2.ForeColor = theme.Accent;
            pnlPoint1.BackColor = theme.PnlP1Bg;
            lblP1Title.ForeColor = theme.TextSecondary;
            lblX1.ForeColor = theme.TextPrimary;
            lblY1.ForeColor = theme.TextPrimary;
            numX.BackColor = theme.InputBg;
            numX.ForeColor = theme.InputFg;
            numY.BackColor = theme.InputBg;
            numY.ForeColor = theme.InputFg;
            btnPickCoord.BackColor = theme.Accent;
            btnGetCurCoord.BackColor = theme.IsDark ? Color.FromArgb(80, 84, 92) : Color.FromArgb(112, 102, 92);

            pnlPoint2Controls.BackColor = theme.PnlP2Bg;
            lblP2Title.ForeColor = theme.IsDark ? Color.FromArgb(120, 180, 240) : Color.FromArgb(30, 80, 130);
            lblX2.ForeColor = theme.TextPrimary;
            lblY2.ForeColor = theme.TextPrimary;
            numX2.BackColor = theme.InputBg;
            numX2.ForeColor = theme.InputFg;
            numY2.BackColor = theme.InputBg;
            numY2.ForeColor = theme.InputFg;
            btnPickCoord2.BackColor = theme.IsDark ? Color.FromArgb(40, 110, 180) : Color.FromArgb(45, 110, 175);
            btnGetCurCoord2.BackColor = theme.IsDark ? Color.FromArgb(70, 95, 120) : Color.FromArgb(100, 120, 140);

            lblPointInterval.ForeColor = theme.TextPrimary;
            numPointInterval.BackColor = theme.InputBg;
            numPointInterval.ForeColor = theme.InputFg;
            lblPointIntervalUnit.ForeColor = theme.TextMuted;

            lblDelay.ForeColor = theme.TextPrimary;
            numClickDelay.BackColor = theme.InputBg;
            numClickDelay.ForeColor = theme.InputFg;
            lblDelayUnit.ForeColor = theme.TextMuted;

            // Section 3 Controls
            lblStartDelay.ForeColor = theme.TextPrimary;
            rbStartImmediate.ForeColor = theme.TextPrimary;
            rbStartDelay.ForeColor = theme.TextPrimary;
            numStartDelay.BackColor = theme.InputBg;
            numStartDelay.ForeColor = theme.InputFg;
            lblStartDelayUnit.ForeColor = !rbStartImmediate.Checked
                ? (theme.IsDark ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                : theme.TextMuted;

            lblInterval.ForeColor = theme.TextPrimary;
            numInterval.BackColor = theme.InputBg;
            numInterval.ForeColor = theme.InputFg;
            cboIntervalUnit.BackColor = theme.InputBg;
            cboIntervalUnit.ForeColor = theme.InputFg;
            lblIntervalUnit.ForeColor = theme.TextMuted;

            chkEnableDuration.ForeColor = theme.TextPrimary;
            numDuration.BackColor = theme.InputBg;
            numDuration.ForeColor = theme.InputFg;
            cboDurationUnit.BackColor = theme.InputBg;
            cboDurationUnit.ForeColor = theme.InputFg;
            lblDurationUnit.ForeColor = chkEnableDuration.Checked
                ? (theme.IsDark ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                : theme.TextMuted;

            chkEnableCount.ForeColor = theme.TextPrimary;
            numCount.BackColor = theme.InputBg;
            numCount.ForeColor = theme.InputFg;
            lblCountUnit.ForeColor = chkEnableCount.Checked
                ? (theme.IsDark ? Color.FromArgb(245, 180, 80) : Color.FromArgb(180, 100, 25))
                : theme.TextMuted;

            // Section 4 Controls
            ApplyThemeToGuide();
            if (pnlHotkeys != null)
            {
                lblStartHk.ForeColor = theme.TextPrimary;
                lblStopHk.ForeColor = theme.TextPrimary;
                lblEmergencyHk.ForeColor = theme.IsDark ? Color.FromArgb(245, 140, 140) : Color.FromArgb(195, 50, 50);
                lblPlus1.ForeColor = theme.TextMuted;
                lblPlus2.ForeColor = theme.TextMuted;
                lblPlus3.ForeColor = theme.TextMuted;
                lblHkTip.ForeColor = theme.TextMuted;

                cboStartMod.BackColor = theme.InputBg; cboStartMod.ForeColor = theme.InputFg;
                cboStartKey.BackColor = theme.InputBg; cboStartKey.ForeColor = theme.InputFg;
                cboStopMod.BackColor = theme.InputBg; cboStopMod.ForeColor = theme.InputFg;
                cboStopKey.BackColor = theme.InputBg; cboStopKey.ForeColor = theme.InputFg;
                cboEmergencyMod.BackColor = theme.InputBg; cboEmergencyMod.ForeColor = theme.InputFg;
                cboEmergencyKey.BackColor = theme.InputBg; cboEmergencyKey.ForeColor = theme.InputFg;

                btnResetHotkeys.BackColor = theme.IsDark ? Color.FromArgb(50, 54, 60) : Color.FromArgb(230, 226, 218);
                btnResetHotkeys.ForeColor = theme.TextSecondary;
            }

            // Section 5 Controls
            lblEnergyTitle.ForeColor = theme.TextPrimary;
            lblStatus.ForeColor = theme.TextSecondary;

            // Stat Cards
            ApplyCardTheme(cardRemainCount, lblCardTitle1, lblRemainCount, theme);
            ApplyCardTheme(cardRemainTime, lblCardTitle2, lblRemainTime, theme);
            ApplyCardTheme(cardCompleted, lblCardTitle3, lblCompletedCount, theme);

            UpdateTopMostButtonUI();
            UpdateHotkeyButtonUI();
            UpdateModeHint();
        }

        private void ApplyCardTheme(Panel card, Label titleLabel, Label valueLabel, ThemeColors theme)
        {
            card.BackColor = theme.CardBg;
            titleLabel.BackColor = theme.CardHeaderBg;
            titleLabel.ForeColor = theme.TextMuted;
            valueLabel.BackColor = theme.CardBg;
        }

        private void ApplyThemeToGuide()
        {
            if (_currentTheme == null) return;

            if (!_hotkeysEnabled)
            {
                lblGuide.Text = $"⚠️ 단축키 비활성 상태  |  [긴급탈출 정지: {_settings.HotkeyEmergency}]";
                lblGuide.BackColor = _currentTheme.BannerBg;
                lblGuide.ForeColor = _currentTheme.IsDark ? Color.FromArgb(245, 170, 70) : Color.FromArgb(190, 80, 20);
            }
            else
            {
                lblGuide.Text = $"🚨 [{_settings.HotkeyStart}] 시작  |  [{_settings.HotkeyStop}] 정지  (긴급탈출: {_settings.HotkeyEmergency})";
                lblGuide.BackColor = _currentTheme.BannerBg;
                lblGuide.ForeColor = _currentTheme.BannerFg;
            }
        }

        private void ApplyTopMost(bool enable)
        {
            TopMost = enable;
            _settings.AlwaysOnTop = enable;
            if (IsHandleCreated)
            {
                NativeMethods.SetWindowTopMost(Handle, enable);
            }
            UpdateTopMostButtonUI();
        }

        private void UpdateTopMostButtonUI()
        {
            // 실제 OS 윈도우 스타일(WS_EX_TOPMOST)을 직접 확인하여 표기
            bool isActuallyTop = IsHandleCreated ? NativeMethods.IsWindowTopMost(Handle) : (_settings?.AlwaysOnTop ?? TopMost);

            if (IsHandleCreated && TopMost != isActuallyTop)
            {
                TopMost = isActuallyTop;
            }

            if (isActuallyTop)
            {
                btnTopMost.Text = "📌 창 항상 위 [ON] (클릭 시 OFF)";
                btnTopMost.BackColor = _currentTheme?.Accent ?? Color.FromArgb(197, 128, 32);
                btnTopMost.ForeColor = Color.White;
            }
            else
            {
                btnTopMost.Text = "📌 창 항상 위 [OFF] (클릭 시 ON)";
                btnTopMost.BackColor = _currentTheme?.IsDark == true ? Color.FromArgb(55, 58, 65) : Color.FromArgb(236, 232, 224);
                btnTopMost.ForeColor = _currentTheme?.TextPrimary ?? Color.FromArgb(85, 75, 65);
            }
        }

        private void UpdateHotkeyButtonUI()
        {
            if (_hotkeysEnabled)
            {
                btnToggleHotkeys.Text = $"⌨ 단축키 활성 [ON] ({_settings.HotkeyStart}/{_settings.HotkeyStop})";
                btnToggleHotkeys.BackColor = Color.FromArgb(46, 139, 87);
                btnToggleHotkeys.ForeColor = Color.White;
                btnStart.Text = $"▶ 시작 ({_settings.HotkeyStart})";
                btnStop.Text = $"⏹ 정지 ({_settings.HotkeyStop})";
            }
            else
            {
                btnToggleHotkeys.Text = $"단축키 비활성 중. 긴급탈출 {_settings.HotkeyEmergency}";
                btnToggleHotkeys.BackColor = _currentTheme?.IsDark == true ? Color.FromArgb(70, 72, 78) : Color.FromArgb(145, 138, 130);
                btnToggleHotkeys.ForeColor = Color.White;
                btnStart.Text = "▶ 시작";
                btnStop.Text = "⏹ 정지";
            }
            ApplyThemeToGuide();
        }

        private string FormatRemainingTime(long remainMs, string unit)
        {
            if (unit == "시간")
            {
                return $"{remainMs / 3600000.0:F2} 시간";
            }
            else if (unit == "분")
            {
                return $"{remainMs / 60000.0:F1} 분";
            }
            else
            {
                return $"{remainMs / 1000.0:F1} 초";
            }
        }

        private void RefreshDashboardInitialValues()
        {
            if (_isRunning) return;

            string durUnit = cboDurationUnit?.SelectedItem?.ToString() ?? "초";
            string durValueStr = durUnit == "시간" ? $"{numDuration.Value:F2}" : $"{numDuration.Value:F1}";
            lblCompletedCount.Text = "0 회";
            lblRemainCount.Text = chkEnableCount.Checked ? $"{numCount.Value} 회" : "무제한";
            lblRemainTime.Text = chkEnableDuration.Checked ? $"{durValueStr} {durUnit}" : "무제한";
        }

        private void UpdateModeHint()
        {
            string durUnit = cboDurationUnit?.SelectedItem?.ToString() ?? "초";
            string durValueStr = durUnit == "시간" ? $"{numDuration.Value:F2}" : $"{numDuration.Value:F1}";
            string intUnit = cboIntervalUnit?.SelectedItem?.ToString() ?? "초";

            if (chkEnableDuration.Checked && chkEnableCount.Checked)
            {
                lblModeHint.Text = $"💡 설정: {durValueStr}{durUnit} 경과 또는 {numCount.Value}회 입력 중 먼저 도달 시 자동 종료됩니다. (간격: {numInterval.Value}{intUnit})";
                lblModeHint.ForeColor = _currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(160, 95, 25);
            }
            else if (chkEnableDuration.Checked)
            {
                lblModeHint.Text = $"💡 설정: {durValueStr}{durUnit} 동안 반복 실행 후 자동 종료됩니다. (간격: {numInterval.Value}{intUnit})";
                lblModeHint.ForeColor = _currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(160, 95, 25);
            }
            else if (chkEnableCount.Checked)
            {
                lblModeHint.Text = $"💡 설정: {numCount.Value}회 입력 후 자동 종료됩니다. (간격: {numInterval.Value}{intUnit})";
                lblModeHint.ForeColor = _currentTheme?.IsDark == true ? Color.FromArgb(245, 180, 80) : Color.FromArgb(160, 95, 25);
            }
            else
            {
                lblModeHint.Text = _hotkeysEnabled
                    ? $"💡 설정: [무제한 반복] 모드 - {_settings.HotkeyStart}/{_settings.HotkeyStop}/{_settings.HotkeyEmergency} 키 또는 버튼으로 조작합니다. (간격: {numInterval.Value}{intUnit})"
                    : $"💡 설정: [무제한 반복] 모드 - 정지 버튼 또는 긴급탈출({_settings.HotkeyEmergency})로 중지합니다. (간격: {numInterval.Value}{intUnit})";
                lblModeHint.ForeColor = _currentTheme?.IsDark == true ? Color.FromArgb(245, 100, 90) : Color.FromArgb(195, 65, 45);
            }
        }

        private void SetupGlobalKeyboardHook()
        {
            try
            {
                _keyboardHook = new GlobalKeyboardHook();
                _keyboardHook.KeyDown += (sender, e) =>
                {
                    // 1. Emergency Escape: ALWAYS stops auto input when running, regardless of _hotkeysEnabled!
                    if (_isRunning && _settings.HotkeyEmergency != null && _settings.HotkeyEmergency.Matches(e.Key, e.Alt, e.Control, e.Shift))
                    {
                        e.Handled = true; // Block Alt+F4 from closing target application
                        _isEmergencyStop = true;
                        BeginInvoke(new Action(() =>
                        {
                            StopAutoInput();
                            SystemSounds.Hand.Play();
                        }));
                        return;
                    }

                    // 2. Regular hotkeys only active when _hotkeysEnabled is true
                    if (!_hotkeysEnabled) return;

                    if (_isRunning)
                    {
                        // Stop hotkey while running
                        if (_settings.HotkeyStop != null && _settings.HotkeyStop.Matches(e.Key, e.Alt, e.Control, e.Shift))
                        {
                            e.Handled = true;
                            BeginInvoke(new Action(() =>
                            {
                                StopAutoInput();
                                SystemSounds.Beep.Play();
                            }));
                            return;
                        }
                    }
                    else
                    {
                        // Start hotkey while stopped
                        if (_settings.HotkeyStart != null && _settings.HotkeyStart.Matches(e.Key, e.Alt, e.Control, e.Shift))
                        {
                            e.Handled = true;
                            BeginInvoke(new Action(() =>
                            {
                                StartAutoInput();
                                SystemSounds.Beep.Play();
                            }));
                            return;
                        }
                    }
                };
            }
            catch
            {
                // Non-intrusive fallback
            }
        }

        private void PickCoordinate(NumericUpDown targetX, NumericUpDown targetY, string pointName)
        {
            using (var picker = new CoordinatePickerForm())
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                {
                    targetX.Value = picker.SelectedPoint.X;
                    targetY.Value = picker.SelectedPoint.Y;
                    SystemSounds.Beep.Play();
                    lblStatus.Text = $"{pointName} 좌표 설정 완료: X={picker.SelectedPoint.X}, Y={picker.SelectedPoint.Y}";
                }
            }
            if (_settings.AlwaysOnTop && IsHandleCreated)
            {
                ApplyTopMost(true);
            }
        }

        private void CaptureCurrentMouse(NumericUpDown targetX, NumericUpDown targetY, string pointName)
        {
            Point p = Cursor.Position;
            targetX.Value = p.X;
            targetY.Value = p.Y;
            SystemSounds.Beep.Play();
            lblStatus.Text = $"{pointName} 현재 마우스 위치 등록됨: X={p.X}, Y={p.Y}";
        }

        private void ApplyStoppedState(int currentIteration, bool isEmergency = false)
        {
            energyBar.IsActive = false;
            energyBar.IsStopped = true;
            energyBar.Value = 0.0;
            energyBar.StatusText = isEmergency
                ? $"🚨 [{_settings.HotkeyEmergency}] 비상탈출 긴급 중단됨"
                : (_hotkeysEnabled ? $"⏹ [{_settings.HotkeyStop}] 키 또는 정지로 중단됨" : "⏹ 정지 버튼으로 중단됨");

            lblEnergyTitle.Text = "⚡ 실시간 기동 에너지 바: ⏹ 중단됨";

            // Loop reset: Return to initial values instead of keeping leftovers
            RefreshDashboardInitialValues();

            lblStatus.Text = currentIteration > 0
                ? $"⏹ 작업이 중단되었습니다. (총 {currentIteration}회 실행 후 루프 초기화됨)"
                : "⏹ 작업이 시작 전 중단되었습니다.";
        }

        private async void StartAutoInput()
        {
            if (_isRunning) return;

            string text1 = txtInput.Text;
            if (string.IsNullOrEmpty(text1))
            {
                lblStatus.Text = "⚠️ 입력할 문구를 먼저 작성해 주세요.";
                return;
            }

            bool enablePoint2 = chkEnablePoint2.Checked;
            string text2 = (enablePoint2 && chkSeparateText2.Checked && !string.IsNullOrEmpty(txtInput2.Text))
                ? txtInput2.Text
                : text1;

            int x1 = (int)numX.Value;
            int y1 = (int)numY.Value;
            int x2 = (int)numX2.Value;
            int y2 = (int)numY2.Value;
            int pointIntervalMs = (int)(numPointInterval.Value * 1000);

            string intUnit = cboIntervalUnit.SelectedItem?.ToString() ?? "초";
            int intervalMs = intUnit switch
            {
                "분" => (int)(numInterval.Value * 60 * 1000),
                "시간" => (int)(numInterval.Value * 3600 * 1000),
                _ => (int)(numInterval.Value * 1000)
            };
            if (intervalMs < 10) intervalMs = 10;

            int clickDelayMs = (int)numClickDelay.Value;
            bool sendEnter = chkEnter.Checked;
            bool useClipboard = chkClipboard.Checked;

            bool enableDuration = chkEnableDuration.Checked;
            string durUnit = cboDurationUnit.SelectedItem?.ToString() ?? "초";
            long durationMs = durUnit switch
            {
                "시간" => (long)(numDuration.Value * 3600 * 1000),
                "분" => (long)(numDuration.Value * 60 * 1000),
                _ => (long)(numDuration.Value * 1000)
            };

            bool enableCount = chkEnableCount.Checked;
            int targetCount = (int)numCount.Value;

            bool immediateStart = rbStartImmediate.Checked;
            decimal startDelaySec = numStartDelay.Value;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _isRunning = true;
            _isEmergencyStop = false;
            UpdateUIState(true);

            SaveCurrentSettings();

            // Initial dashboard setup
            energyBar.IsActive = false;
            energyBar.IsStopped = false;
            energyBar.Value = 0.0;
            energyBar.StatusText = "⏳ 준비 중...";
            lblEnergyTitle.Text = "⚡ 실시간 기동 에너지 바 (다음 입력 충전 게이지):";

            int currentIteration = 0;

            try
            {
                // Start Delay / Countdown Option
                if (!immediateStart && startDelaySec > 0)
                {
                    int totalDelayMs = (int)(startDelaySec * 1000);
                    var delaySw = Stopwatch.StartNew();

                    while (delaySw.ElapsedMilliseconds < totalDelayMs)
                    {
                        token.ThrowIfCancellationRequested();

                        double remainSec = Math.Max(0, (totalDelayMs - delaySw.ElapsedMilliseconds) / 1000.0);
                        string cancelGuide = _hotkeysEnabled ? $"[{_settings.HotkeyStop} 누르면 취소]" : "[정지 누르면 취소]";
                        lblStatus.Text = $"⏳ {remainSec:F1}초 후 입력이 시작됩니다... {cancelGuide}";
                        energyBar.StatusText = $"⏳ {remainSec:F1}초 후 시작... {cancelGuide}";

                        int step = Math.Min(100, (int)(totalDelayMs - delaySw.ElapsedMilliseconds));
                        if (step <= 0) break;
                        await Task.Delay(step, token);
                    }
                }

                token.ThrowIfCancellationRequested();

                energyBar.IsActive = true;
                lblEnergyTitle.Text = "⚡ 실시간 기동 에너지 바 (다음 입력 충전 게이지):";

                var stopwatch = Stopwatch.StartNew();

                while (!token.IsCancellationRequested)
                {
                    currentIteration++;

                    // 1. Move & Click Point 1
                    energyBar.Value = 1.0;
                    energyBar.StatusText = enablePoint2
                        ? $"💥 [포인트 1] 입력 실행 중... (제 {currentIteration}회차)"
                        : $"💥 입력 실행 중... (제 {currentIteration}회차)";
                    InputSimulator.ClickAt(x1, y1);

                    // Wait for focus
                    await Task.Delay(clickDelayMs, token);

                    // Type text at Point 1
                    if (useClipboard)
                    {
                        InputSimulator.SendClipboardPaste(text1);
                    }
                    else
                    {
                        InputSimulator.SendUnicodeText(text1);
                    }

                    // Enter key
                    if (sendEnter)
                    {
                        await Task.Delay(30, token);
                        InputSimulator.SendEnter();
                    }

                    // 2. Point 2 Sequential Input (if enabled)
                    if (enablePoint2)
                    {
                        token.ThrowIfCancellationRequested();

                        energyBar.StatusText = $"⏳ 포인트 2 대기 중... ({numPointInterval.Value:F1}초)";
                        var ptSw = Stopwatch.StartNew();
                        while (ptSw.ElapsedMilliseconds < pointIntervalMs)
                        {
                            token.ThrowIfCancellationRequested();
                            int ptStep = Math.Min(25, (int)(pointIntervalMs - ptSw.ElapsedMilliseconds));
                            if (ptStep <= 0) break;
                            await Task.Delay(ptStep, token);
                        }

                        token.ThrowIfCancellationRequested();

                        // Move & Click Point 2
                        energyBar.StatusText = $"💥 [포인트 2] 입력 실행 중... (제 {currentIteration}회차)";
                        InputSimulator.ClickAt(x2, y2);

                        // Wait for focus
                        await Task.Delay(clickDelayMs, token);

                        // Type text at Point 2
                        if (useClipboard)
                        {
                            InputSimulator.SendClipboardPaste(text2);
                        }
                        else
                        {
                            InputSimulator.SendUnicodeText(text2);
                        }

                        // Enter key
                        if (sendEnter)
                        {
                            await Task.Delay(30, token);
                            InputSimulator.SendEnter();
                        }
                    }

                    // 3. Update Stats & Check completion
                    long elapsedMs = stopwatch.ElapsedMilliseconds;
                    int remainCount = enableCount ? Math.Max(0, targetCount - currentIteration) : -1;
                    long remainDurationMs = enableDuration ? Math.Max(0, durationMs - elapsedMs) : -1;

                    // Update Stat Cards
                    lblCompletedCount.Text = enablePoint2 ? $"{currentIteration} 회 (P1+P2)" : $"{currentIteration} 회";
                    lblRemainCount.Text = enableCount ? $"{remainCount} 회" : "무제한";
                    lblRemainTime.Text = enableDuration
                        ? FormatRemainingTime(remainDurationMs, durUnit)
                        : "무제한";

                    if (!enableDuration && !enableCount)
                    {
                        // Unlimited mode
                        string stopGuide = _hotkeysEnabled ? $"[{_settings.HotkeyStop}로 정지]" : "[정지 버튼 클릭]";
                        lblStatus.Text = $"▶ 기동 중... (입력: {currentIteration}회, 경과: {elapsedMs / 1000.0:F1}초) {stopGuide}";
                    }
                    else if (enableDuration && enableCount)
                    {
                        lblStatus.Text = $"▶ 기동 중... [남은 시간: {FormatRemainingTime(remainDurationMs, durUnit)} | 남은 횟수: {remainCount}회]";

                        if (elapsedMs >= durationMs || currentIteration >= targetCount)
                        {
                            break;
                        }
                    }
                    else if (enableDuration)
                    {
                        lblStatus.Text = $"▶ 기동 중... [남은 시간: {FormatRemainingTime(remainDurationMs, durUnit)}] ({currentIteration}회 입력 완료)";

                        if (elapsedMs >= durationMs)
                        {
                            break;
                        }
                    }
                    else // enableCount
                    {
                        lblStatus.Text = $"▶ 기동 중... [남은 횟수: {remainCount}회] ({currentIteration}/{targetCount}회 완료)";

                        if (currentIteration >= targetCount)
                        {
                            break;
                        }
                    }

                    // 4. Energy Bar Charging Animation during repeat interval
                    var intervalSw = Stopwatch.StartNew();
                    while (intervalSw.ElapsedMilliseconds < intervalMs && !token.IsCancellationRequested)
                    {
                        long intElapsed = intervalSw.ElapsedMilliseconds;
                        double progress = Math.Clamp((double)intElapsed / intervalMs, 0.0, 1.0);
                        double remainingIntervalSec = Math.Max(0.0, (intervalMs - intElapsed) / 1000.0);

                        energyBar.Value = progress;
                        string timeRemainingStr = remainingIntervalSec switch
                        {
                            >= 3600 => $"{remainingIntervalSec / 3600.0:F1}시간",
                            >= 60 => $"{remainingIntervalSec / 60.0:F1}분",
                            _ => $"{remainingIntervalSec:F1}초"
                        };
                        energyBar.StatusText = $"⚡ 기동 중: 다음 입력까지 충전 {(int)(progress * 100)}% ({timeRemainingStr} 남음)";

                        // Live remaining duration countdown in Card
                        long liveElapsed = stopwatch.ElapsedMilliseconds;
                        if (enableDuration)
                        {
                            long liveRemainMs = Math.Max(0, durationMs - liveElapsed);
                            lblRemainTime.Text = FormatRemainingTime(liveRemainMs, durUnit);
                            if (liveRemainMs <= 0) break;
                        }

                        int step = Math.Min(25, (int)(intervalMs - intElapsed));
                        if (step <= 0) break;
                        await Task.Delay(step, token);
                    }
                }

                if (token.IsCancellationRequested)
                {
                    ApplyStoppedState(currentIteration, _isEmergencyStop);
                }
                else
                {
                    energyBar.IsActive = false;
                    energyBar.IsStopped = false;
                    energyBar.Value = 1.0;
                    energyBar.StatusText = "✔ 작업 완료";
                    lblEnergyTitle.Text = "⚡ 실시간 기동 에너지 바: ✔ 작업 완료";

                    // Loop reset: Return to initial values so next run starts from beginning!
                    RefreshDashboardInitialValues();
                    lblStatus.Text = $"✔ 작업이 완료되었습니다. (총 {currentIteration}회 입력 완료 후 루프 초기화됨)";
                }
            }
            catch (OperationCanceledException)
            {
                ApplyStoppedState(currentIteration, _isEmergencyStop);
            }
            catch (Exception ex)
            {
                energyBar.IsActive = false;
                energyBar.IsStopped = true;
                energyBar.Value = 0.0;
                energyBar.StatusText = "⚠️ 오류 발생으로 중단됨";
                lblEnergyTitle.Text = "⚡ 실시간 기동 에너지 바: ⚠️ 중단됨";
                lblStatus.Text = $"오류 발생: {ex.Message}";
                RefreshDashboardInitialValues();
            }
            finally
            {
                InputSimulator.ReleaseStuckKeys();
                _isRunning = false;
                energyBar.IsActive = false;
                UpdateUIState(false);
            }
        }

        private void StopAutoInput()
        {
            if (_isRunning && _cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
            else if (!_isRunning)
            {
                RefreshDashboardInitialValues();
                energyBar.IsActive = false;
                energyBar.IsStopped = false;
                energyBar.Value = 0.0;
                energyBar.StatusText = "⚪ 대기 중 (준비 완료)";
                lblEnergyTitle.Text = "⚡ 실시간 기동 에너지 바 (다음 입력 충전 게이지):";
                lblStatus.Text = "상태: 준비 완료 (루프 초기화됨)";
            }
        }

        private void UpdateUIState(bool running)
        {
            btnStart.Enabled = !running;
            btnStop.Enabled = running;

            cboTheme.Enabled = !running;

            // Point 1
            btnPickCoord.Enabled = !running;
            btnGetCurCoord.Enabled = !running;
            numX.Enabled = !running;
            numY.Enabled = !running;

            // Point 2
            chkEnablePoint2.Enabled = !running;
            btnPickCoord2.Enabled = !running && chkEnablePoint2.Checked;
            btnGetCurCoord2.Enabled = !running && chkEnablePoint2.Checked;
            numX2.Enabled = !running && chkEnablePoint2.Checked;
            numY2.Enabled = !running && chkEnablePoint2.Checked;
            numPointInterval.Enabled = !running && chkEnablePoint2.Checked;

            // Text
            txtInput.Enabled = !running;
            chkSeparateText2.Enabled = !running && chkEnablePoint2.Checked;
            txtInput2.Enabled = !running && chkEnablePoint2.Checked && chkSeparateText2.Checked;

            // Start Delay
            rbStartImmediate.Enabled = !running;
            rbStartDelay.Enabled = !running;
            numStartDelay.Enabled = !running && rbStartDelay.Checked;

            // Repeat & Timing
            numInterval.Enabled = !running;
            cboIntervalUnit.Enabled = !running;
            chkEnableDuration.Enabled = !running;
            numDuration.Enabled = !running && chkEnableDuration.Checked;
            cboDurationUnit.Enabled = !running && chkEnableDuration.Checked;
            chkEnableCount.Enabled = !running;
            numCount.Enabled = !running && chkEnableCount.Checked;
            chkEnter.Enabled = !running;
            chkClipboard.Enabled = !running;
            numClickDelay.Enabled = !running;

            // Custom Hotkey Controls
            cboStartMod.Enabled = !running;
            cboStartKey.Enabled = !running;

            cboStopMod.Enabled = !running;
            cboStopKey.Enabled = !running;

            cboEmergencyMod.Enabled = !running;
            cboEmergencyKey.Enabled = !running;

            btnResetHotkeys.Enabled = !running;

            if (running)
            {
                btnStart.BackColor = _currentTheme?.IsDark == true ? Color.FromArgb(70, 75, 80) : Color.FromArgb(205, 198, 190);
                btnStop.BackColor = Color.FromArgb(205, 65, 55);
            }
            else
            {
                btnStart.BackColor = Color.FromArgb(46, 139, 87);
                btnStop.BackColor = _currentTheme?.IsDark == true ? Color.FromArgb(70, 75, 80) : Color.FromArgb(205, 198, 190);
            }
        }

        private void ApplySettingsToUI()
        {
            txtInput.Text = _settings.TextToInput;
            chkSeparateText2.Checked = _settings.UseSeparateText2;
            txtInput2.Text = _settings.TextToInput2;
            txtInput2.Enabled = _settings.UseSeparateText2;

            // Point 1
            numX.Value = Math.Clamp(_settings.TargetX, numX.Minimum, numX.Maximum);
            numY.Value = Math.Clamp(_settings.TargetY, numY.Minimum, numY.Maximum);

            // Point 2
            chkEnablePoint2.Checked = _settings.EnablePoint2;
            pnlPoint2Controls.Visible = _settings.EnablePoint2;
            pnlText2Row.Visible = _settings.EnablePoint2;
            lblPointInterval.Visible = _settings.EnablePoint2;
            numPointInterval.Visible = _settings.EnablePoint2;
            lblPointIntervalUnit.Visible = _settings.EnablePoint2;

            numX2.Value = Math.Clamp(_settings.TargetX2, numX2.Minimum, numX2.Maximum);
            numY2.Value = Math.Clamp(_settings.TargetY2, numY2.Minimum, numY2.Maximum);
            numPointInterval.Value = Math.Clamp(_settings.PointIntervalSeconds, numPointInterval.Minimum, numPointInterval.Maximum);

            // Start Delay
            rbStartImmediate.Checked = _settings.ImmediateStart;
            rbStartDelay.Checked = !_settings.ImmediateStart;
            numStartDelay.Value = Math.Clamp(_settings.StartDelaySeconds, numStartDelay.Minimum, numStartDelay.Maximum);
            numStartDelay.Enabled = !_settings.ImmediateStart;

            // Repeat Interval & Unit
            if (!string.IsNullOrEmpty(_settings.IntervalUnit) && cboIntervalUnit.Items.Contains(_settings.IntervalUnit))
            {
                cboIntervalUnit.SelectedItem = _settings.IntervalUnit;
            }
            numInterval.Value = Math.Clamp(_settings.IntervalSeconds, numInterval.Minimum, numInterval.Maximum);

            // Duration & Unit
            if (!string.IsNullOrEmpty(_settings.DurationUnit) && cboDurationUnit.Items.Contains(_settings.DurationUnit))
            {
                cboDurationUnit.SelectedItem = _settings.DurationUnit;
            }
            string curDurUnit = cboDurationUnit.SelectedItem?.ToString() ?? "초";
            if (curDurUnit == "시간")
            {
                numDuration.Minimum = 0.01m;
                numDuration.Maximum = 720m;
                numDuration.Increment = 0.1m;
                numDuration.DecimalPlaces = 2;
            }
            else if (curDurUnit == "분")
            {
                numDuration.Minimum = 0.1m;
                numDuration.Maximum = 1440m;
                numDuration.Increment = 0.5m;
                numDuration.DecimalPlaces = 1;
            }
            else
            {
                numDuration.Minimum = 0.5m;
                numDuration.Maximum = 86400m;
                numDuration.Increment = 1m;
                numDuration.DecimalPlaces = 1;
            }

            chkEnableDuration.Checked = _settings.EnableDuration;
            numDuration.Value = Math.Clamp(_settings.DurationSeconds, numDuration.Minimum, numDuration.Maximum);
            numDuration.Enabled = _settings.EnableDuration;
            cboDurationUnit.Enabled = _settings.EnableDuration;
            lblDurationUnit.Text = _settings.EnableDuration
                ? $"{cboDurationUnit.SelectedItem} 동안 반복 후 자동 종료"
                : "(꺼짐: 시간 제한 없이 계속)";

            // Count
            chkEnableCount.Checked = _settings.EnableCount;
            numCount.Value = Math.Clamp(_settings.RepeatCount, numCount.Minimum, numCount.Maximum);
            numCount.Enabled = _settings.EnableCount;
            lblCountUnit.Text = _settings.EnableCount ? "회 입력 후 자동 종료" : "(꺼짐: 횟수 제한 없음)";

            chkEnter.Checked = _settings.SendEnterAfterInput;
            chkClipboard.Checked = _settings.UseClipboardPaste;
            numClickDelay.Value = Math.Clamp(_settings.ClickDelayMs, numClickDelay.Minimum, numClickDelay.Maximum);

            // TopMost & Hotkeys
            TopMost = _settings.AlwaysOnTop;
            _hotkeysEnabled = _settings.EnableHotkeys;

            // Theme
            string targetTheme = _settings.SelectedTheme ?? "아이보리 웜";
            if (cboTheme.Items.Contains(targetTheme))
            {
                cboTheme.SelectedItem = targetTheme;
            }
            ApplyTheme(targetTheme);

            ApplyHotkeysToUI();
            UpdateTopMostButtonUI();
            UpdateHotkeyButtonUI();
            UpdateModeHint();
            RefreshDashboardInitialValues();
        }

        private void ApplyHotkeysToUI()
        {
            _isUpdatingHotkeyUI = true;
            cboStartMod.SelectedItem = ModToDisplay(_settings.HotkeyStart?.Modifier);
            if (!cboStartKey.Items.Contains(_settings.HotkeyStart?.Key))
                cboStartKey.Items.Add(_settings.HotkeyStart?.Key);
            cboStartKey.SelectedItem = _settings.HotkeyStart?.Key;

            cboStopMod.SelectedItem = ModToDisplay(_settings.HotkeyStop?.Modifier);
            if (!cboStopKey.Items.Contains(_settings.HotkeyStop?.Key))
                cboStopKey.Items.Add(_settings.HotkeyStop?.Key);
            cboStopKey.SelectedItem = _settings.HotkeyStop?.Key;

            cboEmergencyMod.SelectedItem = ModToDisplay(_settings.HotkeyEmergency?.Modifier);
            if (!cboEmergencyKey.Items.Contains(_settings.HotkeyEmergency?.Key))
                cboEmergencyKey.Items.Add(_settings.HotkeyEmergency?.Key);
            cboEmergencyKey.SelectedItem = _settings.HotkeyEmergency?.Key;
            _isUpdatingHotkeyUI = false;
        }

        private void SaveCurrentSettings()
        {
            _settings.TextToInput = txtInput.Text;
            _settings.UseSeparateText2 = chkSeparateText2.Checked;
            _settings.TextToInput2 = txtInput2.Text;

            _settings.TargetX = (int)numX.Value;
            _settings.TargetY = (int)numY.Value;

            _settings.EnablePoint2 = chkEnablePoint2.Checked;
            _settings.TargetX2 = (int)numX2.Value;
            _settings.TargetY2 = (int)numY2.Value;
            _settings.PointIntervalSeconds = numPointInterval.Value;

            _settings.ImmediateStart = rbStartImmediate.Checked;
            _settings.StartDelaySeconds = numStartDelay.Value;

            _settings.IntervalSeconds = numInterval.Value;
            _settings.IntervalUnit = cboIntervalUnit.SelectedItem?.ToString() ?? "초";
            _settings.EnableDuration = chkEnableDuration.Checked;
            _settings.DurationSeconds = numDuration.Value;
            _settings.DurationUnit = cboDurationUnit.SelectedItem?.ToString() ?? "초";
            _settings.EnableCount = chkEnableCount.Checked;
            _settings.RepeatCount = (int)numCount.Value;
            _settings.SendEnterAfterInput = chkEnter.Checked;
            _settings.UseClipboardPaste = chkClipboard.Checked;
            _settings.ClickDelayMs = (int)numClickDelay.Value;
            _settings.AlwaysOnTop = IsHandleCreated ? NativeMethods.IsWindowTopMost(Handle) : _settings.AlwaysOnTop;
            _settings.EnableHotkeys = _hotkeysEnabled;
            _settings.SelectedTheme = cboTheme.SelectedItem?.ToString() ?? "아이보리 웜";

            _settings.HotkeyStart = new HotkeyConfig(ModFromDisplay(cboStartMod.SelectedItem?.ToString()), cboStartKey.SelectedItem?.ToString() ?? "F1");
            _settings.HotkeyStop = new HotkeyConfig(ModFromDisplay(cboStopMod.SelectedItem?.ToString()), cboStopKey.SelectedItem?.ToString() ?? "F2");
            _settings.HotkeyEmergency = new HotkeyConfig(ModFromDisplay(cboEmergencyMod.SelectedItem?.ToString()), cboEmergencyKey.SelectedItem?.ToString() ?? "F4");

            _settings.Save();
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            // 최초 실행 시 창이 화면에 올라온 뒤 OS 레벨에서 TopMost를 확실하게 강제 적용 및 상태 검증
            if (_settings.AlwaysOnTop)
            {
                ApplyTopMost(true);
            }
            else
            {
                ApplyTopMost(false);
            }

            UpdateTopMostButtonUI();

            bool isActuallyTop = NativeMethods.IsWindowTopMost(Handle);
            if (isActuallyTop)
            {
                lblStatus.Text = "상태: 준비 완료 (창 항상 위: ON)";
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // 창이 최소화되었다가 복원될 때 OS에서 HWND_TOPMOST가 풀리는 현상 방지 및 상태 동기화
            if (WindowState != FormWindowState.Minimized && _settings.AlwaysOnTop && IsHandleCreated)
            {
                if (!NativeMethods.IsWindowTopMost(Handle))
                {
                    NativeMethods.SetWindowTopMost(Handle, true);
                    TopMost = true;
                }
                UpdateTopMostButtonUI();
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            // 창 활성화 시 항상 위 상태가 해제되어 있다면 다시 복구 및 UI 갱신
            if (_settings.AlwaysOnTop && IsHandleCreated && !NativeMethods.IsWindowTopMost(Handle))
            {
                NativeMethods.SetWindowTopMost(Handle, true);
                TopMost = true;
                UpdateTopMostButtonUI();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAutoInput();
            SaveCurrentSettings();
            _keyboardHook?.Dispose();
        }
    }
}
