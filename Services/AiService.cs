using System;
using System.Collections.Generic;
using System.Linq;
using AshkanAQMS.Models;

namespace AshkanAQMS.Services
{
    public class AiService
    {
        private const double AnomalyZScoreThreshold = 2.5;

        /// <summary>
        /// شناسایی ناهنجاری سنسور بر اساس انحراف از معیار پویا (Z-Score & Spike Detection)
        /// </summary>
        public bool IsAnomalyDetected(IEnumerable<double> historicalValues, double currentValue, out string reason)
        {
            reason = string.Empty;
            var list = historicalValues?.ToList() ?? new List<double>();

            if (list.Count < 5)
                return false;

            double avg = list.Average();
            double sumSquares = list.Select(val => (val - avg) * (val - avg)).Sum();
            double stdDev = Math.Sqrt(sumSquares / list.Count);

            if (stdDev > 0.001)
            {
                double zScore = Math.Abs((currentValue - avg) / stdDev);
                if (zScore > AnomalyZScoreThreshold)
                {
                    reason = $"ناهنجاری آماری شدید سنسور: تغییر ناگهانی مقادیر (Z-Score: {zScore:F2})";
                    return true;
                }
            }

            // بررسی خطای فریز شدن مقدار سنسور (Flatline) - سازگار با .NET Framework 4.8
            var last5Items = list.Skip(Math.Max(0, list.Count - 5));
            if (last5Items.All(v => Math.Abs(v - currentValue) < 0.0001))
            {
                reason = "سنسور بدون تغییر وضعیت مانده است (امکان خرابی یا قطع اتصال سنسور).";
                return true;
            }

            return false;
        }

        /// <summary>
        /// پیش‌بینی روند مقدار آلاینده برای بازه پیش‌رو بر اساس میانگین متحرک وزنی نمایی (EMA)
        /// </summary>
        public double PredictNextValue(IEnumerable<double> historicalValues, double alpha = 0.3)
        {
            var values = historicalValues?.ToList() ?? new List<double>();
            if (!values.Any()) return 0;

            double ema = values.First();
            foreach (var val in values.Skip(1))
            {
                ema = (alpha * val) + ((1 - alpha) * ema);
            }

            // اعمال ضریب شیب تغییرات اخیر (Linear Trend Adjustment)
            if (values.Count >= 3)
            {
                double recentTrend = values[values.Count - 1] - values[values.Count - 3];
                ema += (recentTrend * 0.5);
            }

            return Math.Max(0, ema);
        }

        /// <summary>
        /// تولید پیشنهادات ایمنی و تحلیلی برای اپراتور اتاق کنترل
        /// </summary>
        public string GenerateActionRecommendation(AirQualityData data, double predictedAqi)
        {
            if (predictedAqi > 200 || (data != null && data.OverallAqi > 200))
            {
                return "وضعیت بحرانی: فعال‌سازی تهویه اضطراری زون و اعلام هشدار ماسک فیلتردار در سالن تولید.";
            }
            if (predictedAqi > 150)
            {
                return "کیفیت هوا ناسالم: کاهش بار کاری تجهیزات آلاینده و بررسی فیلتراسیون مرکزی.";
            }
            if (data != null && data.PM25 > 55.0)
            {
                return "افزایش ذرات معلق: بررسی آب‌پاش‌های محوطه و درزبندی کانال‌های هوای تازه.";
            }

            return "شرایط در وضعیت مجاز و پایدار قرار دارد.";
        }
    }
}
