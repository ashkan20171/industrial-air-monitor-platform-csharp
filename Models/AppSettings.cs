using System;

namespace AshkanAQMS.Models
{
    [Serializable]
    public class AppSettings
    {
        // تنظیمات دریافت داده و بازه رفرش (ثانیه)
        public int PollingIntervalSeconds { get; set; } = 3;

        // پارامترهای موتور تحلیل و هوش مصنوعی (AiService)
        public double AnomalyZScoreThreshold { get; set; } = 2.5;
        public double EmaAlpha { get; set; } = 0.3;
        public int HistoryWindowSize { get; set; } = 20;

        // تنظیمات هشدار و رفتار سامانه
        public bool EnableSoundAlerts { get; set; } = false;
        public bool EnableAutoArchive { get; set; } = true;
    }
}
