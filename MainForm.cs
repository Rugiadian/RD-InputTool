using System;
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
        private TextBox txtInput;
        private CheckBox chkEnter;
        private CheckBox chkClipboard;
        private NumericUpDown numX;
        private NumericUpDown numY;
        private Button btnPickCoord;
        private Button btnGetCurCoord;

        private NumericUpDown numInterval;
        private CheckBox chkEnableDuration;
        private NumericUpDown numDuration;
        private Label lblDurationUnit;
        private CheckBox chkEnableCount;
        private NumericUpDown numCount;
        private Label lblCountUnit;
        private Label lblModeHint;

        private NumericUpDown numClickDelay;

        private Button btnStart;
        private Button btnStop;

        // Dashboard / Status Controls
        private Label lblRunningBadge;
        private Label lblRemainCount;
        private Label lblRemainTime;
        private Label lblCompletedCount;
        private EnergyBar energyBar;
        private ProgressBar progressBar;
        private Label lblStatus;

        // Bottom Easy-to-Click Always-on-Top Button
        private Button btnTopMost;

        private GlobalKeyboardHook _keyboardHook;
        private CancellationTokenSource _cts;
        private bool _isRunning = false;
        private readonly AppSettings _settings;

        public MainForm()
        {
            _settings = AppSettings.Load();
            InitializeComponent();
            ApplySettingsToUI();
            SetupGlobalKeyboardHook();
        }

        private void InitializeComponent()
        {
            Text = "RD 오토 입력 툴 (Auto Input Tool)";
            AutoScaleMode = AutoScaleMode.Dpi;
            Size = new Size(590, 940);
            MinimumSize = new Size(520, 820);
            StartPosition = FormStartPosition.CenterScreen;
            Font = new Font("Malgun Gothic", 9f, FontStyle.Regular);
            BackColor = Color.FromArgb(246, 248, 250);

            // ==========================================
            // Bottom Panel: Large, Easy-to-Click TopMost Button
            // ==========================================
            var bottomBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 46,
                Padding = new Padding(12, 6, 12, 8),
                BackColor = Color.FromArgb(238, 241, 245)
            };

            btnTopMost = new Button
            {
                Dock = DockStyle.Fill,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnTopMost.FlatAppearance.BorderSize = 0;
            btnTopMost.Click += (s, e) =>
            {
                TopMost = !TopMost;
                _settings.AlwaysOnTop = TopMost;
                _settings.Save();
                UpdateTopMostButtonUI();
                SystemSounds.Beep.Play();
            };
            bottomBar.Controls.Add(btnTopMost);

            // Root Scrollable Panel
            var rootPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(12, 10, 12, 8)
            };

            // Main Vertical Stack
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
            // 1. 문구 지정 그룹 (Text Input)
            // ==========================================
            var grpText = new GroupBox
            {
                Text = " 1. 원하는 문구 지정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 12),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 35, 45)
            };

            var textInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
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

            textInner.Controls.Add(txtInput);
            textInner.Controls.Add(optLayout);
            grpText.Controls.Add(textInner);

            // ==========================================
            // 2. 좌표 지정 그룹 (Coordinate)
            // ==========================================
            var grpCoord = new GroupBox
            {
                Text = " 2. 입력 포인트 (좌표) 지정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 12),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 35, 45)
            };

            var coordInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            coordInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Buttons Row
            var coordBtnRow = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 32,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 1, 0, 5)
            };
            coordBtnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            coordBtnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnPickCoord = new Button
            {
                Text = "🎯 클릭으로 좌표 지정",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(41, 128, 185),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(0, 0, 4, 0)
            };
            btnPickCoord.FlatAppearance.BorderSize = 0;
            btnPickCoord.Click += BtnPickCoord_Click;

            btnGetCurCoord = new Button
            {
                Text = "📍 현재 위치 등록 (F8)",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(85, 95, 110),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                Cursor = Cursors.Hand,
                Margin = new Padding(4, 0, 0, 0)
            };
            btnGetCurCoord.FlatAppearance.BorderSize = 0;
            btnGetCurCoord.Click += (s, e) => CaptureCurrentMouse();

            coordBtnRow.Controls.Add(btnPickCoord, 0, 0);
            coordBtnRow.Controls.Add(btnGetCurCoord, 1, 0);

            // Inputs Row
            var coordInputs = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 2)
            };

            var lblX = new Label { Text = "X:", AutoSize = true, Padding = new Padding(0, 3, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numX = new NumericUpDown { Minimum = -10000, Maximum = 10000, Value = 500, Width = 75, Margin = new Padding(2, 0, 10, 0) };

            var lblY = new Label { Text = "Y:", AutoSize = true, Padding = new Padding(0, 3, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numY = new NumericUpDown { Minimum = -10000, Maximum = 10000, Value = 500, Width = 75, Margin = new Padding(2, 0, 12, 0) };

            var lblDelay = new Label { Text = "대기(ms):", AutoSize = true, Padding = new Padding(0, 3, 0, 0) };
            numClickDelay = new NumericUpDown { Minimum = 10, Maximum = 5000, Value = 100, Width = 70, Margin = new Padding(2, 0, 0, 0) };

            coordInputs.Controls.AddRange(new Control[] { lblX, numX, lblY, numY, lblDelay, numClickDelay });

            coordInner.Controls.Add(coordBtnRow);
            coordInner.Controls.Add(coordInputs);
            grpCoord.Controls.Add(coordInner);

            // ==========================================
            // 3. 반복 및 지속 시간 그룹 (Repeat & Duration)
            // ==========================================
            var grpRepeat = new GroupBox
            {
                Text = " 3. 반복 간격 및 지속 시간 지정 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 12),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 35, 45)
            };

            var repeatInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 4,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            repeatInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // 1) Interval Row
            var intervalRow = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(0, 1, 0, 3)
            };
            var lblInterval = new Label { Text = "반복 간격 :", AutoSize = true, Padding = new Padding(0, 3, 0, 0), Font = new Font("Malgun Gothic", 9f, FontStyle.Bold) };
            numInterval = new NumericUpDown
            {
                DecimalPlaces = 2,
                Increment = 0.1m,
                Minimum = 0.05m,
                Maximum = 3600m,
                Value = 1.0m,
                Width = 75,
                Margin = new Padding(4, 0, 6, 0)
            };
            var lblIntervalUnit = new Label { Text = "초 마다 입력", AutoSize = true, Padding = new Padding(0, 3, 0, 0), ForeColor = Color.DimGray };
            intervalRow.Controls.AddRange(new Control[] { lblInterval, numInterval, lblIntervalUnit });

            // 2) Duration Toggle Row
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
                Width = 75,
                Margin = new Padding(2, 0, 6, 0)
            };
            lblDurationUnit = new Label
            {
                Text = "초 동안 반복 후 자동 종료",
                AutoSize = true,
                Padding = new Padding(0, 3, 0, 0),
                ForeColor = Color.FromArgb(41, 128, 185)
            };

            chkEnableDuration.CheckedChanged += (s, e) =>
            {
                numDuration.Enabled = chkEnableDuration.Checked;
                lblDurationUnit.Text = chkEnableDuration.Checked ? "초 동안 반복 후 자동 종료" : "(꺼짐: 시간 제한 없이 계속)";
                lblDurationUnit.ForeColor = chkEnableDuration.Checked ? Color.FromArgb(41, 128, 185) : Color.Gray;
                UpdateModeHint();
                RefreshDashboardInitialValues();
            };
            durationRow.Controls.AddRange(new Control[] { chkEnableDuration, numDuration, lblDurationUnit });

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
                Padding = new Padding(0, 3, 0, 0),
                ForeColor = Color.Gray
            };

            chkEnableCount.CheckedChanged += (s, e) =>
            {
                numCount.Enabled = chkEnableCount.Checked;
                lblCountUnit.Text = chkEnableCount.Checked ? "회 입력 후 자동 종료" : "(꺼짐: 횟수 제한 없음)";
                lblCountUnit.ForeColor = chkEnableCount.Checked ? Color.FromArgb(41, 128, 185) : Color.Gray;
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
                Font = new Font("Malgun Gothic", 8.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(41, 128, 185)
            };

            repeatInner.Controls.Add(intervalRow);
            repeatInner.Controls.Add(durationRow);
            repeatInner.Controls.Add(countRow);
            repeatInner.Controls.Add(lblModeHint);
            grpRepeat.Controls.Add(repeatInner);

            // ==========================================
            // 4. 실행 및 단축키 안내 그룹 (Controls)
            // ==========================================
            var grpControl = new GroupBox
            {
                Text = " 4. 실행 및 단축키 안내 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(10, 6, 10, 8),
                Margin = new Padding(0, 0, 0, 12),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 35, 45)
            };

            var ctrlInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 2,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            ctrlInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Start / Stop Buttons Table (Side-by-Side 50% / 50%)
            var btnTable = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 40,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0, 1, 0, 6)
            };
            btnTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            btnTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            btnStart = new Button
            {
                Text = "▶ 시작 (F6)",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(39, 174, 96),
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
                Text = "⏹ 정지 (F7 / ESC)",
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(231, 76, 60),
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

            // Full-Width ESC Warning Banner
            var lblGuide = new Label
            {
                Text = "🚨 [ESC] 누르면 즉시 중지 (F6: 시작, F7: 정지)",
                ForeColor = Color.FromArgb(183, 79, 3),
                BackColor = Color.FromArgb(254, 249, 231),
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(0, 0, 0, 2)
            };

            ctrlInner.Controls.Add(btnTable);
            ctrlInner.Controls.Add(lblGuide);
            grpControl.Controls.Add(ctrlInner);

            // ==========================================
            // 5. 실시간 기동 상태 대시보드 (Dashboard)
            // ==========================================
            var grpStatus = new GroupBox
            {
                Text = " 5. 실시간 기동 상태 대시보드 ",
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(12, 10, 12, 12),
                Margin = new Padding(0, 0, 0, 8),
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Bold),
                ForeColor = Color.FromArgb(30, 35, 45)
            };

            var statusInner = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                ColumnCount = 1,
                RowCount = 7,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Regular)
            };
            statusInner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f));

            // Running Status Badge (Top Indicator)
            lblRunningBadge = new Label
            {
                Text = "⚪ 대기 중 (준비 완료)",
                Dock = DockStyle.Top,
                Height = 32,
                Font = new Font("Malgun Gothic", 10f, FontStyle.Bold),
                ForeColor = Color.FromArgb(90, 95, 105),
                BackColor = Color.FromArgb(235, 238, 242),
                TextAlign = ContentAlignment.MiddleCenter,
                Margin = new Padding(0, 0, 0, 8)
            };

            // Stat Cards Row: Height 86px, spacious & bold numbers are 100% visible with zero clipping!
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

            // Card 1: 남은 횟수
            var cardRemainCount = CreateStatCard("🎯 남은 횟수", out lblRemainCount, "무제한", Color.FromArgb(41, 128, 185));
            cardRemainCount.Margin = new Padding(0, 0, 4, 0);

            // Card 2: 남은 시간
            var cardRemainTime = CreateStatCard("⏳ 남은 시간", out lblRemainTime, "10.0초", Color.FromArgb(142, 68, 173));
            cardRemainTime.Margin = new Padding(2, 0, 2, 0);

            // Card 3: 현재 완료 횟수
            var cardCompleted = CreateStatCard("📊 입력 완료", out lblCompletedCount, "0회", Color.FromArgb(39, 174, 96));
            cardCompleted.Margin = new Padding(4, 0, 0, 0);

            statsGrid.Controls.Add(cardRemainCount, 0, 0);
            statsGrid.Controls.Add(cardRemainTime, 1, 0);
            statsGrid.Controls.Add(cardCompleted, 2, 0);

            // Energy Bar Header
            var lblEnergyTitle = new Label
            {
                Text = "⚡ 실시간 기동 에너지 바 (다음 입력 충전 게이지):",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 45, 55),
                Margin = new Padding(0, 2, 0, 4)
            };

            // Energy Bar Control
            energyBar = new EnergyBar
            {
                Dock = DockStyle.Top,
                Height = 34,
                Margin = new Padding(0, 0, 0, 8)
            };

            // Overall Progress Bar Header
            var lblProgressTitle = new Label
            {
                Text = "📈 전체 작업 진행률:",
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(40, 45, 55),
                Margin = new Padding(0, 2, 0, 4)
            };

            // Overall Progress Bar
            progressBar = new ProgressBar
            {
                Dock = DockStyle.Top,
                Height = 16,
                Minimum = 0,
                Maximum = 100,
                Value = 0,
                Margin = new Padding(0, 0, 0, 6)
            };

            // Status Description Text
            lblStatus = new Label
            {
                Text = "상태: 준비 완료 (시작을 누르면 2초 후 동작합니다)",
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Malgun Gothic", 9.5f, FontStyle.Regular),
                ForeColor = Color.FromArgb(70, 75, 85),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(2, 0, 0, 0)
            };

            statusInner.Controls.Add(lblRunningBadge);
            statusInner.Controls.Add(statsGrid);
            statusInner.Controls.Add(lblEnergyTitle);
            statusInner.Controls.Add(energyBar);
            statusInner.Controls.Add(lblProgressTitle);
            statusInner.Controls.Add(progressBar);
            statusInner.Controls.Add(lblStatus);
            grpStatus.Controls.Add(statusInner);

            // Add all to content layout
            contentLayout.Controls.Add(grpText, 0, 0);
            contentLayout.Controls.Add(grpCoord, 0, 1);
            contentLayout.Controls.Add(grpRepeat, 0, 2);
            contentLayout.Controls.Add(grpControl, 0, 3);
            contentLayout.Controls.Add(grpStatus, 0, 4);

            rootPanel.Controls.Add(contentLayout);

            // Add to Form: RootPanel (Fill) and BottomBar (Bottom)
            Controls.Add(rootPanel);
            Controls.Add(bottomBar);

            FormClosing += MainForm_FormClosing;
        }

        private Panel CreateStatCard(string title, out Label valueLabel, string initialValue, Color valueColor)
        {
            var card = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(1)
            };

            var lblTitle = new Label
            {
                Text = title,
                Dock = DockStyle.Top,
                Height = 26,
                Font = new Font("Malgun Gothic", 9f, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 105, 115),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(246, 248, 251)
            };

            valueLabel = new Label
            {
                Text = initialValue,
                Dock = DockStyle.Fill,
                Font = new Font("Malgun Gothic", 14f, FontStyle.Bold),
                ForeColor = valueColor,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };

            card.Controls.Add(lblTitle);
            card.Controls.Add(valueLabel);
            lblTitle.BringToFront();
            return card;
        }

        private void UpdateTopMostButtonUI()
        {
            if (TopMost)
            {
                btnTopMost.Text = "📌 창 항상 위에 고정됨 [ON]  (클릭하여 해제)";
                btnTopMost.BackColor = Color.FromArgb(41, 128, 185);
                btnTopMost.ForeColor = Color.White;
            }
            else
            {
                btnTopMost.Text = "📌 창 항상 위에 고정 [OFF]  (클릭하여 켜기)";
                btnTopMost.BackColor = Color.FromArgb(225, 228, 234);
                btnTopMost.ForeColor = Color.FromArgb(60, 65, 75);
            }
        }

        private void RefreshDashboardInitialValues()
        {
            if (_isRunning) return;

            lblCompletedCount.Text = "0 회";
            lblRemainCount.Text = chkEnableCount.Checked ? $"{numCount.Value} 회" : "무제한";
            lblRemainTime.Text = chkEnableDuration.Checked ? $"{numDuration.Value:F1} 초" : "무제한";
        }

        private void UpdateModeHint()
        {
            if (chkEnableDuration.Checked && chkEnableCount.Checked)
            {
                lblModeHint.Text = $"💡 설정: {numDuration.Value}초 경과 또는 {numCount.Value}회 입력 중 먼저 도달 시 자동 종료됩니다.";
                lblModeHint.ForeColor = Color.FromArgb(41, 128, 185);
            }
            else if (chkEnableDuration.Checked)
            {
                lblModeHint.Text = $"💡 설정: {numDuration.Value}초 동안 반복 실행 후 자동 종료됩니다.";
                lblModeHint.ForeColor = Color.FromArgb(41, 128, 185);
            }
            else if (chkEnableCount.Checked)
            {
                lblModeHint.Text = $"💡 설정: {numCount.Value}회 입력 후 자동 종료됩니다.";
                lblModeHint.ForeColor = Color.FromArgb(41, 128, 185);
            }
            else
            {
                lblModeHint.Text = "💡 설정: [무제한 반복] 모드 - ESC 키 또는 정지 버튼을 누를 때까지 계속 입력합니다.";
                lblModeHint.ForeColor = Color.FromArgb(192, 57, 43);
            }
        }

        private void SetupGlobalKeyboardHook()
        {
            try
            {
                _keyboardHook = new GlobalKeyboardHook();
                _keyboardHook.KeyDown += key =>
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (key == Keys.Escape)
                        {
                            if (_isRunning)
                            {
                                StopAutoInput();
                            }
                        }
                        else if (key == Keys.F6)
                        {
                            if (!_isRunning)
                            {
                                StartAutoInput();
                            }
                        }
                        else if (key == Keys.F7)
                        {
                            if (_isRunning)
                            {
                                StopAutoInput();
                            }
                        }
                        else if (key == Keys.F8)
                        {
                            CaptureCurrentMouse();
                        }
                    }));
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show($"전역 키보드 훅 등록 실패: {ex.Message}\n창 내부 단축키는 계속 동작합니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnPickCoord_Click(object sender, EventArgs e)
        {
            using (var picker = new CoordinatePickerForm())
            {
                if (picker.ShowDialog(this) == DialogResult.OK)
                {
                    numX.Value = picker.SelectedPoint.X;
                    numY.Value = picker.SelectedPoint.Y;
                    SystemSounds.Beep.Play();
                    lblStatus.Text = $"좌표 설정 완료: X={picker.SelectedPoint.X}, Y={picker.SelectedPoint.Y}";
                }
            }
        }

        private void CaptureCurrentMouse()
        {
            Point p = Cursor.Position;
            numX.Value = p.X;
            numY.Value = p.Y;
            SystemSounds.Beep.Play();
            lblStatus.Text = $"현재 마우스 위치 등록됨: X={p.X}, Y={p.Y}";
        }

        private async void StartAutoInput()
        {
            if (_isRunning) return;

            string text = txtInput.Text;
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("입력할 문구를 작성해 주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtInput.Focus();
                return;
            }

            int x = (int)numX.Value;
            int y = (int)numY.Value;
            int intervalMs = (int)(numInterval.Value * 1000);
            int clickDelayMs = (int)numClickDelay.Value;
            bool sendEnter = chkEnter.Checked;
            bool useClipboard = chkClipboard.Checked;

            bool enableDuration = chkEnableDuration.Checked;
            int durationMs = (int)(numDuration.Value * 1000);
            bool enableCount = chkEnableCount.Checked;
            int targetCount = (int)numCount.Value;

            _cts = new CancellationTokenSource();
            var token = _cts.Token;
            _isRunning = true;
            UpdateUIState(true);

            SaveCurrentSettings();

            // Badge: Starting
            lblRunningBadge.Text = "⏳ 시작 준비 중 (카운트다운)...";
            lblRunningBadge.BackColor = Color.FromArgb(254, 243, 199);
            lblRunningBadge.ForeColor = Color.FromArgb(146, 64, 14);

            energyBar.IsActive = false;
            energyBar.Value = 0.0;
            energyBar.StatusText = "⏳ 준비 중...";

            try
            {
                // 2-second countdown
                for (int i = 2; i > 0; i--)
                {
                    if (token.IsCancellationRequested) break;
                    lblStatus.Text = $"⏳ {i}초 후 입력이 시작됩니다... [ESC 누르면 취소]";
                    energyBar.StatusText = $"⏳ {i}초 후 시작... [ESC 누르면 취소]";
                    await Task.Delay(1000, token);
                }

                if (token.IsCancellationRequested) return;

                // Badge: Active Running
                lblRunningBadge.Text = "🟢 ● 기동 중 (RUNNING) - [ESC]로 즉시 정지";
                lblRunningBadge.BackColor = Color.FromArgb(209, 250, 229);
                lblRunningBadge.ForeColor = Color.FromArgb(6, 95, 70);

                energyBar.IsActive = true;

                var stopwatch = Stopwatch.StartNew();
                int currentIteration = 0;

                while (!token.IsCancellationRequested)
                {
                    currentIteration++;

                    // 1. Move & Click
                    energyBar.Value = 1.0;
                    energyBar.StatusText = $"💥 입력 실행 중... (제 {currentIteration}회차)";
                    InputSimulator.ClickAt(x, y);

                    // Wait for focus
                    await Task.Delay(clickDelayMs, token);

                    // 2. Type text
                    if (useClipboard)
                    {
                        InputSimulator.SendClipboardPaste(text);
                    }
                    else
                    {
                        InputSimulator.SendUnicodeText(text);
                    }

                    // 3. Enter key
                    if (sendEnter)
                    {
                        await Task.Delay(30, token);
                        InputSimulator.SendEnter();
                    }

                    // 4. Update Stats & Check completion
                    long elapsedMs = stopwatch.ElapsedMilliseconds;
                    int remainCount = enableCount ? Math.Max(0, targetCount - currentIteration) : -1;
                    long remainDurationMs = enableDuration ? Math.Max(0, durationMs - elapsedMs) : -1;

                    // Update Stat Cards
                    lblCompletedCount.Text = $"{currentIteration} 회";
                    lblRemainCount.Text = enableCount ? $"{remainCount} 회" : "무제한";
                    lblRemainTime.Text = enableDuration ? $"{remainDurationMs / 1000.0:F1} 초" : "무제한";

                    if (!enableDuration && !enableCount)
                    {
                        // Unlimited mode
                        lblStatus.Text = $"▶ 기동 중... (입력: {currentIteration}회, 경과: {elapsedMs / 1000.0:F1}초) [ESC로 정지]";
                        progressBar.Style = ProgressBarStyle.Marquee;
                    }
                    else if (enableDuration && enableCount)
                    {
                        double timePercent = (double)elapsedMs / durationMs;
                        double countPercent = (double)currentIteration / targetCount;
                        int maxPercent = Math.Clamp((int)(Math.Max(timePercent, countPercent) * 100), 0, 100);

                        lblStatus.Text = $"▶ 기동 중... [남은 시간: {remainDurationMs / 1000.0:F1}초 | 남은 횟수: {remainCount}회]";
                        progressBar.Style = ProgressBarStyle.Blocks;
                        progressBar.Value = maxPercent;

                        if (elapsedMs >= durationMs || currentIteration >= targetCount)
                        {
                            break;
                        }
                    }
                    else if (enableDuration)
                    {
                        int percent = Math.Clamp((int)((double)elapsedMs / durationMs * 100), 0, 100);
                        lblStatus.Text = $"▶ 기동 중... [남은 시간: {remainDurationMs / 1000.0:F1}초] ({currentIteration}회 입력 완료)";
                        progressBar.Style = ProgressBarStyle.Blocks;
                        progressBar.Value = percent;

                        if (elapsedMs >= durationMs)
                        {
                            break;
                        }
                    }
                    else // enableCount
                    {
                        int percent = Math.Clamp((int)((double)currentIteration / targetCount * 100), 0, 100);
                        lblStatus.Text = $"▶ 기동 중... [남은 횟수: {remainCount}회] ({currentIteration}/{targetCount}회 완료)";
                        progressBar.Style = ProgressBarStyle.Blocks;
                        progressBar.Value = percent;

                        if (currentIteration >= targetCount)
                        {
                            break;
                        }
                    }

                    // 5. Energy Bar Charging Animation during repeat interval
                    var intervalSw = Stopwatch.StartNew();
                    while (intervalSw.ElapsedMilliseconds < intervalMs && !token.IsCancellationRequested)
                    {
                        long intElapsed = intervalSw.ElapsedMilliseconds;
                        double progress = Math.Clamp((double)intElapsed / intervalMs, 0.0, 1.0);
                        double remainingIntervalSec = Math.Max(0.0, (intervalMs - intElapsed) / 1000.0);

                        energyBar.Value = progress;
                        energyBar.StatusText = $"⚡ 기동 중: 충전 {(int)(progress * 100)}% ({remainingIntervalSec:F1}초 남음)";

                        // Live remaining time countdown in Card
                        long liveElapsed = stopwatch.ElapsedMilliseconds;
                        if (enableDuration)
                        {
                            long liveRemainMs = Math.Max(0, durationMs - liveElapsed);
                            lblRemainTime.Text = $"{liveRemainMs / 1000.0:F1} 초";
                            if (liveRemainMs <= 0) break;
                        }

                        int step = Math.Min(25, (int)(intervalMs - intElapsed));
                        if (step <= 0) break;
                        await Task.Delay(step, token);
                    }
                }

                if (token.IsCancellationRequested)
                {
                    lblRunningBadge.Text = "⏹ [ESC] 키로 중지됨";
                    lblRunningBadge.BackColor = Color.FromArgb(254, 226, 226);
                    lblRunningBadge.ForeColor = Color.FromArgb(153, 27, 27);

                    energyBar.Value = 0.0;
                    energyBar.StatusText = "⏹ [ESC] 긴급 중지됨";
                    lblStatus.Text = $"⏹ [ESC] 키로 중지되었습니다. (총 {currentIteration}회 입력 완료)";
                    SystemSounds.Exclamation.Play();
                }
                else
                {
                    lblRunningBadge.Text = "✔ 작업 완료 (SUCCESS)";
                    lblRunningBadge.BackColor = Color.FromArgb(219, 234, 254);
                    lblRunningBadge.ForeColor = Color.FromArgb(30, 64, 175);

                    energyBar.Value = 1.0;
                    energyBar.StatusText = "✔ 모든 반복 작업 완료!";
                    lblRemainCount.Text = "0 회";
                    if (enableDuration) lblRemainTime.Text = "0.0 초";
                    lblStatus.Text = $"✔ 설정된 작업이 완료되었습니다! (총 {currentIteration}회 입력 완료)";
                    progressBar.Value = 100;
                    SystemSounds.Asterisk.Play();
                }
            }
            catch (OperationCanceledException)
            {
                lblRunningBadge.Text = "⏹ [ESC] 키로 즉시 중지됨";
                lblRunningBadge.BackColor = Color.FromArgb(254, 226, 226);
                lblRunningBadge.ForeColor = Color.FromArgb(153, 27, 27);

                energyBar.Value = 0.0;
                energyBar.StatusText = "⏹ 중지됨";
                lblStatus.Text = $"⏹ [ESC] 키로 즉시 중지되었습니다.";
                SystemSounds.Exclamation.Play();
            }
            catch (Exception ex)
            {
                lblRunningBadge.Text = "⚠️ 오류 발생";
                lblRunningBadge.BackColor = Color.FromArgb(254, 226, 226);
                lblRunningBadge.ForeColor = Color.FromArgb(153, 27, 27);
                lblStatus.Text = $"오류 발생: {ex.Message}";
            }
            finally
            {
                _isRunning = false;
                energyBar.IsActive = false;
                progressBar.Style = ProgressBarStyle.Blocks;
                UpdateUIState(false);
            }
        }

        private void StopAutoInput()
        {
            if (_isRunning && _cts != null && !_cts.IsCancellationRequested)
            {
                _cts.Cancel();
            }
        }

        private void UpdateUIState(bool running)
        {
            btnStart.Enabled = !running;
            btnStop.Enabled = running;
            btnPickCoord.Enabled = !running;
            btnGetCurCoord.Enabled = !running;
            txtInput.Enabled = !running;
            numX.Enabled = !running;
            numY.Enabled = !running;
            numInterval.Enabled = !running;
            chkEnableDuration.Enabled = !running;
            numDuration.Enabled = !running && chkEnableDuration.Checked;
            chkEnableCount.Enabled = !running;
            numCount.Enabled = !running && chkEnableCount.Checked;
            chkEnter.Enabled = !running;
            chkClipboard.Enabled = !running;

            if (running)
            {
                btnStart.BackColor = Color.Gray;
                btnStop.BackColor = Color.FromArgb(231, 76, 60);
            }
            else
            {
                btnStart.BackColor = Color.FromArgb(39, 174, 96);
                btnStop.BackColor = Color.Gray;
            }
        }

        private void ApplySettingsToUI()
        {
            txtInput.Text = _settings.TextToInput;
            numX.Value = Math.Clamp(_settings.TargetX, numX.Minimum, numX.Maximum);
            numY.Value = Math.Clamp(_settings.TargetY, numY.Minimum, numY.Maximum);
            numInterval.Value = Math.Clamp(_settings.IntervalSeconds, numInterval.Minimum, numInterval.Maximum);

            chkEnableDuration.Checked = _settings.EnableDuration;
            numDuration.Value = Math.Clamp(_settings.DurationSeconds, numDuration.Minimum, numDuration.Maximum);
            numDuration.Enabled = _settings.EnableDuration;
            lblDurationUnit.Text = _settings.EnableDuration ? "초 동안 반복 후 자동 종료" : "(꺼짐: 시간 제한 없이 계속)";
            lblDurationUnit.ForeColor = _settings.EnableDuration ? Color.FromArgb(41, 128, 185) : Color.Gray;

            chkEnableCount.Checked = _settings.EnableCount;
            numCount.Value = Math.Clamp(_settings.RepeatCount, numCount.Minimum, numCount.Maximum);
            numCount.Enabled = _settings.EnableCount;
            lblCountUnit.Text = _settings.EnableCount ? "회 입력 후 자동 종료" : "(꺼짐: 횟수 제한 없음)";
            lblCountUnit.ForeColor = _settings.EnableCount ? Color.FromArgb(41, 128, 185) : Color.Gray;

            chkEnter.Checked = _settings.SendEnterAfterInput;
            chkClipboard.Checked = _settings.UseClipboardPaste;
            numClickDelay.Value = Math.Clamp(_settings.ClickDelayMs, numClickDelay.Minimum, numClickDelay.Maximum);
            
            TopMost = _settings.AlwaysOnTop;
            UpdateTopMostButtonUI();

            UpdateModeHint();
            RefreshDashboardInitialValues();
        }

        private void SaveCurrentSettings()
        {
            _settings.TextToInput = txtInput.Text;
            _settings.TargetX = (int)numX.Value;
            _settings.TargetY = (int)numY.Value;
            _settings.IntervalSeconds = numInterval.Value;
            _settings.EnableDuration = chkEnableDuration.Checked;
            _settings.DurationSeconds = numDuration.Value;
            _settings.EnableCount = chkEnableCount.Checked;
            _settings.RepeatCount = (int)numCount.Value;
            _settings.SendEnterAfterInput = chkEnter.Checked;
            _settings.UseClipboardPaste = chkClipboard.Checked;
            _settings.ClickDelayMs = (int)numClickDelay.Value;
            _settings.AlwaysOnTop = TopMost;
            _settings.Save();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAutoInput();
            SaveCurrentSettings();
            _keyboardHook?.Dispose();
        }
    }
}
