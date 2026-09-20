using System;
using System.IO;
using System.Text.Json;

namespace RD_Tools
{
    public class AppSettings
    {
        public string TextToInput { get; set; } = "테스트 문구입니다.";
        public int TargetX { get; set; } = 500;
        public int TargetY { get; set; } = 500;
        public decimal IntervalSeconds { get; set; } = 1.0m;
        public bool EnableDuration { get; set; } = true;
        public decimal DurationSeconds { get; set; } = 10.0m;
        public bool EnableCount { get; set; } = false;
        public int RepeatCount { get; set; } = 10;
        public bool SendEnterAfterInput { get; set; } = true;
        public bool UseClipboardPaste { get; set; } = true;
        public int ClickDelayMs { get; set; } = 100;
        public bool AlwaysOnTop { get; set; } = true;

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
