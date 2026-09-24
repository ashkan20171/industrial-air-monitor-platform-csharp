using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AshkanAQMS
{
    public static class IndustrialTheme
    {
        public static readonly Color Navy900 = Color.FromArgb(9, 24, 38);
        public static readonly Color Navy800 = Color.FromArgb(13, 37, 56);
        public static readonly Color Navy700 = Color.FromArgb(18, 52, 77);
        public static readonly Color Canvas = Color.FromArgb(244, 247, 250);
        public static readonly Color Surface = Color.White;
        public static readonly Color Border = Color.FromArgb(222, 230, 237);
        public static readonly Color Ink = Color.FromArgb(22, 42, 58);
        public static readonly Color Muted = Color.FromArgb(101, 119, 133);
        public static readonly Color Cyan = Color.FromArgb(0, 166, 190);
        public static readonly Color Blue = Color.FromArgb(30, 113, 176);
        public static readonly Color Green = Color.FromArgb(30, 160, 106);
        public static readonly Color Amber = Color.FromArgb(230, 159, 38);
        public static readonly Color Red = Color.FromArgb(210, 67, 67);
        public static readonly Color Purple = Color.FromArgb(111, 78, 170);

        // Backward compatible aliases used by older controls.
        public static readonly Color BackgroundDark = Navy900;
        public static readonly Color SurfaceCard = Color.FromArgb(27, 45, 59);
        public static readonly Color BorderColor = Color.FromArgb(48, 68, 82);
        public static readonly Color TextPrimary = Color.FromArgb(238, 244, 247);
        public static readonly Color TextSecondary = Color.FromArgb(164, 181, 191);
        public static readonly Color StatusGood = Green;
        public static readonly Color StatusModerate = Amber;
        public static readonly Color StatusUnhealthySensitive = Color.FromArgb(226, 120, 52);
        public static readonly Color StatusUnhealthy = Red;
        public static readonly Color StatusHazardous = Purple;

        public static Panel Card(int padding = 16)
        {
            return new Panel { BackColor = Surface, Padding = new Padding(padding), Margin = new Padding(8) };
        }
        public static Label Caption(string text) { return new Label { Text=text, AutoSize=true, Font=new Font("Segoe UI",9f,FontStyle.Bold), ForeColor=Muted }; }
        public static Label Heading(string text, float size=15f) { return new Label { Text=text, AutoSize=true, Font=new Font("Segoe UI Semibold",size,FontStyle.Bold), ForeColor=Ink }; }
        public static Button Action(string text)
        {
            return new Button { Text=text, Height=38, AutoSize=true, FlatStyle=FlatStyle.Flat, BackColor=Navy700, ForeColor=Color.White, Font=new Font("Segoe UI Semibold",9.5f,FontStyle.Bold), Padding=new Padding(12,0,12,0), Cursor=Cursors.Hand };
        }
        public static void PolishGrid(DataGridView grid)
        {
            grid.BackgroundColor=Surface; grid.BorderStyle=BorderStyle.None; grid.GridColor=Border; grid.RowHeadersVisible=false;
            grid.EnableHeadersVisualStyles=false; grid.ColumnHeadersDefaultCellStyle.BackColor=Navy700; grid.ColumnHeadersDefaultCellStyle.ForeColor=Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font=new Font("Segoe UI Semibold",9.5f,FontStyle.Bold); grid.ColumnHeadersHeight=38;
            grid.DefaultCellStyle.BackColor=Surface; grid.DefaultCellStyle.ForeColor=Ink; grid.DefaultCellStyle.SelectionBackColor=Color.FromArgb(224,240,246); grid.DefaultCellStyle.SelectionForeColor=Ink;
            grid.DefaultCellStyle.Font=new Font("Segoe UI",9.2f); grid.RowTemplate.Height=34; grid.CellBorderStyle=DataGridViewCellBorderStyle.SingleHorizontal;
        }
        public static void ApplyDarkTheme(Control root)
        {
            if(root==null)return; if(root is Form){root.BackColor=Navy900;root.ForeColor=TextPrimary;} foreach(Control child in root.Controls)ApplyDarkTheme(child);
        }
    }

    public sealed class AccentPanel : Panel
    {
        public Color Accent { get; set; } = IndustrialTheme.Cyan;
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e); using(var p=new Pen(IndustrialTheme.Border))e.Graphics.DrawRectangle(p,0,0,Width-1,Height-1); using(var b=new SolidBrush(Accent))e.Graphics.FillRectangle(b,0,0,4,Height);
        }
    }
}
