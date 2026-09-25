using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace AshkanAQMS.Controls
{
    public sealed class BackupRecoveryControl : UserControl
    {
        private readonly Label status=new Label();
        public BackupRecoveryControl()
        {
            Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);
            var header=new Panel{Dock=DockStyle.Top,Height=96,BackColor=IndustrialTheme.Navy800,Padding=new Padding(22)};header.Controls.Add(new Label{Text="BACKUP, RECOVERY & DISASTER READINESS",Dock=DockStyle.Top,Height=32,ForeColor=Color.White,Font=new Font("Segoe UI Semibold",17,FontStyle.Bold)});header.Controls.Add(new Label{Text="Portable station snapshot • configuration evidence • recovery workflow",Dock=DockStyle.Bottom,Height=25,ForeColor=Color.FromArgb(175,210,224)});
            var card=new Panel{Dock=DockStyle.Top,Height=150,BackColor=Color.White,Padding=new Padding(20),Margin=new Padding(0,18,0,0)};var create=Btn("CREATE STATION SNAPSHOT");var open=Btn("OPEN DATA FOLDER");create.Location=new Point(20,54);open.Location=new Point(235,54);create.Click+=(s,e)=>CreateSnapshot();open.Click+=(s,e)=>{try{System.Diagnostics.Process.Start("explorer.exe",AppDomain.CurrentDomain.BaseDirectory);}catch(Exception ex){status.Text=ex.Message;}};card.Controls.Add(new Label{Text="Recovery Point",Location=new Point(20,18),AutoSize=true,Font=new Font("Segoe UI Semibold",12,FontStyle.Bold),ForeColor=IndustrialTheme.Navy800});card.Controls.Add(create);card.Controls.Add(open);status.Location=new Point(20,104);status.AutoSize=true;status.ForeColor=Color.FromArgb(80,100,110);card.Controls.Add(status);Controls.Add(card);Controls.Add(header);
        }
        private Button Btn(string t){return new Button{Text=t,Width=195,Height=36,FlatStyle=FlatStyle.Flat,BackColor=IndustrialTheme.Navy800,ForeColor=Color.White};}
        private void CreateSnapshot(){using(var d=new SaveFileDialog{Filter="AQMS snapshot|*.zip",FileName="AshkanAQMS-station-snapshot-"+DateTime.Now.ToString("yyyyMMdd-HHmm")+".zip"}){if(d.ShowDialog(this)!=DialogResult.OK)return;try{var temp=Path.Combine(Path.GetTempPath(),"AshkanAQMS-Snapshot-"+Guid.NewGuid().ToString("N"));Directory.CreateDirectory(temp);var baseDir=AppDomain.CurrentDomain.BaseDirectory;foreach(var name in new[]{"settings.xml","analyzers.xml","App.config"}){var src=Path.Combine(baseDir,name);if(File.Exists(src))File.Copy(src,Path.Combine(temp,name),true);}File.WriteAllText(Path.Combine(temp,"snapshot-info.txt"),"AshkanAQMS station snapshot\r\nCreated: "+DateTime.Now.ToString("O")+"\r\nMachine: "+Environment.MachineName);if(File.Exists(d.FileName))File.Delete(d.FileName);ZipFile.CreateFromDirectory(temp,d.FileName);Directory.Delete(temp,true);status.Text="Snapshot created: "+d.FileName;}catch(Exception ex){status.Text="Snapshot failed: "+ex.Message;}}}
    }
}
