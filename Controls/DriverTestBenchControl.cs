using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AshkanAQMS.Drivers;

namespace AshkanAQMS.Controls
{
    public sealed class DriverTestBenchControl : UserControl
    {
        private readonly ComboBox drivers=new ComboBox(); private readonly TextBox input=new TextBox(); private readonly TextBox output=new TextBox();
        public DriverTestBenchControl()
        {
            Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);
            var header=new Panel{Dock=DockStyle.Top,Height=96,BackColor=IndustrialTheme.Navy800,Padding=new Padding(22)};
            header.Controls.Add(new Label{Text="DRIVER TEST BENCH & PACKET INSPECTOR",Dock=DockStyle.Top,Height=32,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",17,FontStyle.Bold)});
            header.Controls.Add(new Label{Text="Offline protocol inspection • ASCII/HEX conversion • driver selection • no hardware commands are transmitted",Dock=DockStyle.Bottom,Height=25,ForeColor=Color.FromArgb(175,210,224)});
            var top=new FlowLayoutPanel{Dock=DockStyle.Top,Height=58,Padding=new Padding(0,12,0,4)};drivers.Width=280;drivers.DropDownStyle=ComboBoxStyle.DropDownList;
            try{foreach(var d in AnalyzerDriverCatalog.Items.OrderBy(x=>x.Manufacturer).ThenBy(x=>x.DisplayName))drivers.Items.Add(d.Manufacturer+" • "+d.DisplayName+" • "+d.Id);}catch{}
            if(drivers.Items.Count>0)drivers.SelectedIndex=0;var ascii=Btn("ASCII → HEX");var hex=Btn("HEX → ASCII");var clear=Btn("CLEAR");ascii.Click+=(s,e)=>ToHex();hex.Click+=(s,e)=>ToAscii();clear.Click+=(s,e)=>{input.Clear();output.Clear();};top.Controls.AddRange(new Control[]{drivers,ascii,hex,clear});
            var split=new SplitContainer{Dock=DockStyle.Fill,Orientation=Orientation.Horizontal,SplitterDistance=180,BackColor=IndustrialTheme.Canvas};input.Multiline=true;input.Dock=DockStyle.Fill;input.Font=new Font("Consolas",10);input.ScrollBars=ScrollBars.Both;output.Multiline=true;output.Dock=DockStyle.Fill;output.ReadOnly=true;output.Font=new Font("Consolas",10);output.BackColor=Color.FromArgb(13,24,32);output.ForeColor=Color.FromArgb(137,220,235);output.ScrollBars=ScrollBars.Both;split.Panel1.Controls.Add(input);split.Panel2.Controls.Add(output);Controls.Add(split);Controls.Add(top);Controls.Add(header);
        }
        private Button Btn(string t){return new Button{Text=t,Width=125,Height=32,FlatStyle=FlatStyle.Flat,BackColor=IndustrialTheme.Navy800,ForeColor=Color.White,Margin=new Padding(10,0,0,0)};}
        private void ToHex(){var b=Encoding.ASCII.GetBytes(input.Text??"");output.Text=BitConverter.ToString(b).Replace("-"," ");}
        private void ToAscii(){try{var clean=new string((input.Text??"").Where(c=>Uri.IsHexDigit(c)).ToArray());if(clean.Length%2!=0)throw new FormatException("HEX input must contain complete bytes.");var b=new byte[clean.Length/2];for(int i=0;i<b.Length;i++)b[i]=Convert.ToByte(clean.Substring(i*2,2),16);output.Text=Encoding.ASCII.GetString(b)+Environment.NewLine+Environment.NewLine+"Bytes: "+b.Length;}catch(Exception ex){output.Text="Parse error: "+ex.Message;}}
    }
}
