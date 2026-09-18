using System;
using System.Drawing;
using System.Windows.Forms;

namespace AshkanAQMS
{
    public static class IndustrialTheme
    {
        public static readonly Color BackgroundDark = Color.FromArgb(24, 26, 31);
        public static readonly Color SurfaceCard = Color.FromArgb(32, 36, 43);
        public static readonly Color BorderColor = Color.FromArgb(48, 54, 64);
        public static readonly Color TextPrimary = Color.FromArgb(236, 240, 241);
        public static readonly Color TextSecondary = Color.FromArgb(149, 165, 166);

        public static readonly Color StatusGood = Color.FromArgb(46, 204, 113);
        public static readonly Color StatusModerate = Color.FromArgb(241, 196, 15);
        public static readonly Color StatusUnhealthySensitive = Color.FromArgb(230, 126, 34);
        public static readonly Color StatusUnhealthy = Color.FromArgb(231, 76, 60);
        public static readonly Color StatusHazardous = Color.FromArgb(142, 68, 173);

        public static void ApplyDarkTheme(Control root)
        {
            if (root == null) return;

            if (root is Form form)
            {
                form.BackColor = BackgroundDark;
                form.ForeColor = TextPrimary;
            }
            else if (root is Panel panel)
            {
                // Preserve explicit panel backgrounds configured by the owning view.
                if (panel.BackColor == Color.Empty) panel.BackColor = SurfaceCard;
            }
            else if (root is DataGridView dgv)
            {
                dgv.BackgroundColor = SurfaceCard;
                dgv.DefaultCellStyle.BackColor = SurfaceCard;
                dgv.DefaultCellStyle.ForeColor = TextPrimary;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 73, 94);
                dgv.DefaultCellStyle.SelectionForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.EnableHeadersVisualStyles = false;
                dgv.GridColor = BorderColor;
            }

            foreach (Control child in root.Controls)
            {
                ApplyDarkTheme(child);
            }
        }
    }
}
