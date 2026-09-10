using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS
{
    public static class IndustrialTheme
    {
        // رنگ‌های زمینه و ساختار
        public static readonly Color BackgroundDark = Color.FromArgb(24, 26, 31);
        public static readonly Color SurfaceCard = Color.FromArgb(32, 36, 43);
        public static readonly Color BorderColor = Color.FromArgb(48, 54, 64);
        public static readonly Color TextPrimary = Color.FromArgb(236, 240, 241);
        public static readonly Color TextSecondary = Color.FromArgb(149, 165, 166);

        // رنگ‌های روانشناسی هشدار و AQI
        public static readonly Color StatusGood = Color.FromArgb(46, 204, 113);       // سبز پایدار
        public static readonly Color StatusModerate = Color.FromArgb(241, 196, 15);   // کهربایی هشدار
        public static readonly Color StatusUnhealthySensitive = Color.FromArgb(230, 126, 34); // نارنجی احتیاط
        public static readonly Color StatusUnhealthy = Color.FromArgb(231, 76, 60);    // قرمز هشدار
        public static readonly Color StatusHazardous = Color.FromArgb(142, 68, 173);   // بنفش خطرناک

        public static void ApplyDarkTheme(Control root)
        {
            root.BackColor = BackgroundDark;
            root.ForeColor = TextPrimary;

            foreach (Control ctrl in root.Controls)
            {
                if (ctrl is Panel panel)
                {
                    panel.BackColor = SurfaceCard;
                }
                else if (ctrl is DataGridView grid)
                {
                    grid.BackgroundColor = BackgroundDark;
                    grid.DefaultCellStyle.BackColor = SurfaceCard;
                    grid.DefaultCellStyle.ForeColor = TextPrimary;
                    grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(41, 128, 185);
                    grid.DefaultCellStyle.SelectionForeColor = Color.White;
                    grid.GridColor = BorderColor;
                    grid.EnableHeadersVisualStyles = false;
                    grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 49, 58);
                    grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
                }

                if (ctrl.HasChildren)
                {
                    ApplyDarkTheme(ctrl);
                }
            }
        }
    }
}
