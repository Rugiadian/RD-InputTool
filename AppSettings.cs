using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;

namespace RD_Tools
{
    public class HotkeyConfig
    {
        public string Modifier { get; set; } = "None";
        public string Key { get; set; } = "F1";

        public HotkeyConfig() { }

        public HotkeyConfig(string modifier, string key)
        {
            Modifier = string.IsNullOrWhiteSpace(modifier) ? "None" : modifier.Trim();
            Key = string.IsNullOrWhiteSpace(key) ? "F1" : key.Trim();
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Modifier) || Modifier.Equals("None", StringComparison.OrdinalIgnoreCase) || Modifier == "없음")
                return Key;
            return $"{Modifier}+{Key}";
        }

        public bool Matches(Keys key, bool alt, bool ctrl, bool shift)
        {
            if (key == Keys.Menu || key == Keys.ControlKey || key == Keys.ShiftKey ||
                key == Keys.LMenu || key == Keys.RMenu ||
                key == Keys.LControlKey || key == Keys.RControlKey ||
                key == Keys.LShiftKey || key == Keys.RShiftKey)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Key)) return false;

            bool keyMatched = false;
            if (Enum.TryParse<Keys>(Key, true, out var targetKey))
            {
                keyMatched = (key == targetKey);
            }

            if (!keyMatched && Key.Length == 1 && char.IsDigit(Key[0]))
            {
                if (Enum.TryParse<Keys>("D" + Key, true, out var digitKey) && key == digitKey)
                    keyMatched = true;
                else if (Enum.TryParse<Keys>("NumPad" + Key, true, out var numpadKey) && key == numpadKey)
                    keyMatched = true;
            }

            if (!keyMatched) return false;

            string mod = Modifier ?? "None";
            bool needAlt = mod.Contains("Alt", StringComparison.OrdinalIgnoreCase);
            bool needCtrl = mod.Contains("Ctrl", StringComparison.OrdinalIgnoreCase);
            bool needShift = mod.Contains("Shift", StringComparison.OrdinalIgnoreCase);

            return (alt == needAlt) && (ctrl == needCtrl) && (shift == needShift);
        }
    }

    public class AppSettings
    {
        public string TextToInput { get; set; } = "테스트 문구입니다.";
        public bool UseSeparateText2 { get; set; } = false;
        public string TextToInput2 { get; set; } = "테스트 문구 2입니다.";

        // Point 1
        public int TargetX { get; set; } = 500;
        public int TargetY { get; set; } = 500;

        // Point 2 (Option)
        public bool EnablePoint2 { get; set; } = false;
        public int TargetX2 { get; set; } = 600;
        public int TargetY2 { get; set; } = 600;
        public decimal PointIntervalSeconds { get; set; } = 1.0m;

        // Start Delay Option
        public bool ImmediateStart { get; set; } = false;
        public decimal StartDelaySeconds { get; set; } = 2.0m;

        public decimal IntervalSeconds { get; set; } = 1.0m;
        public string IntervalUnit { get; set; } = "초";
        public bool EnableDuration { get; set; } = true;
        public decimal DurationSeconds { get; set; } = 10.0m;
        public string DurationUnit { get; set; } = "초";
        public bool EnableCount { get; set; } = false;
        public int RepeatCount { get; set; } = 10;
        public bool SendEnterAfterInput { get; set; } = true;
        public bool UseClipboardPaste { get; set; } = true;
        public int ClickDelayMs { get; set; } = 100;
        public bool AlwaysOnTop { get; set; } = true;
        public bool EnableHotkeys { get; set; } = true;
        public string SelectedTheme { get; set; } = "아이보리 웜";

        // Customizable Hotkeys (Default Emergency: Alt + F4)
        public HotkeyConfig HotkeyStart { get; set; } = new HotkeyConfig("Alt", "F1");
        public HotkeyConfig HotkeyStop { get; set; } = new HotkeyConfig("None", "F2");
        public HotkeyConfig HotkeyEmergency { get; set; } = new HotkeyConfig("Alt", "F4");

        private static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auto_input_config.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    var s = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                    s.HotkeyStart ??= new HotkeyConfig("Alt", "F1");
                    s.HotkeyStop ??= new HotkeyConfig("None", "F2");
                    s.HotkeyEmergency ??= new HotkeyConfig("Alt", "F4");
                    s.EnableHotkeys = true; // Always enable hotkeys on startup for responsive shortcut operation
                    return s;
                }
            }
            catch { }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(ConfigPath, json);
            }
            catch { }
        }
    }
}
