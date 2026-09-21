using System;
using System.IO;
using System.Text.Json;

namespace RD_Tools
{
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

        private static string ConfigPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "auto_input_config.json");

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(ConfigPath))
                {
                    string json = File.ReadAllText(ConfigPath);
                    return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
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
