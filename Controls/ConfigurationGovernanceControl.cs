using System;using System.Drawing;using System.IO;using System.Linq;using System.Windows.Forms;using AshkanAQMS.Services;
namespace AshkanAQMS.Controls
{
 public sealed class ConfigurationGovernanceControl:UserControl
 {
  readonly StorageService storage=new StorageService();readonly ListBox list=new ListBox();readonly Label info=new Label();
  public ConfigurationGovernanceControl(){Dock=DockStyle.Fill;BackColor=IndustrialTheme.Canvas;Padding=new Padding(24);Build();RefreshView();}
  void Build(){var h=new Label{Text="CONFIGURATION GOVERNANCE & RECOVERY",Dock=DockStyle.Top,Height=44,Font=new Font("Segoe UI Semibold",18,FontStyle.Bold),ForeColor=IndustrialTheme.Ink};info.Dock=DockStyle.Top;info.Height=62;info.ForeColor=IndustrialTheme.Muted;var bar=new FlowLayoutPanel{Dock=DockStyle.Top,Height=48,FlowDirection=FlowDirection.LeftToRight};var backup=Btn("Create analyzer backup");backup.Click+=(s,e)=>{try{var p=AnalyzerConfigBackupService.CreateBackup(storage.LoadAnalyzers(),storage.GetAnalyzerBackupFolder());MessageBox.Show("Backup created:\n"+p,"Configuration Governance");RefreshView();}catch(Exception ex){MessageBox.Show(ex.Message,"Backup Error");}};var refresh=Btn("Refresh");refresh.Click+=(s,e)=>RefreshView();bar.Controls.Add(backup);bar.Controls.Add(refresh);list.Dock=DockStyle.Fill;list.Font=new Font("Consolas",9.5f);Controls.Add(list);Controls.Add(bar);Controls.Add(info);Controls.Add(h);}
  Button Btn(string t)=>new Button{Text=t,Width=190,Height=34,FlatStyle=FlatStyle.Flat,Margin=new Padding(0,4,10,4)};
  void RefreshView(){list.Items.Clear();var folder=storage.GetAnalyzerBackupFolder();Directory.CreateDirectory(folder);var files=Directory.GetFiles(folder).OrderByDescending(File.GetLastWriteTime).Take(30).ToArray();foreach(var f in files)list.Items.Add(File.GetLastWriteTime(f).ToString("yyyy-MM-dd HH:mm:ss")+"   "+Path.GetFileName(f));info.Text=$"Analyzer configuration backups: {files.Length} shown   •   Folder: {folder}\nBackups provide recovery points; restore should be reviewed before applying to a live station.";}
 }
}
