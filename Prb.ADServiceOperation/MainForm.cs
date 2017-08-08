using Prb.SqliteDAL;
using Prb.DTO;
using System;
using System.Collections.Specialized;
using System.Configuration.Install;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Windows.Forms;
using System.Xml;
using Prb.ActiveDirectoryOperations;

namespace Prb.ServiceOperation
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        double timeoutMilliseconds = 365;
        Prb_SettingsDAL _Prb_SettingsDAL = new Prb_SettingsDAL();
        Prb_SettingDTO _Prb_SettingDTO = new Prb_SettingDTO();

        private void btnService_Click(object sender, EventArgs e)
        {
            try
            {
                // Probe Running Settings End

                //    System.Diagnostics.Debugger.Launch();

                ServiceInstaller serviceInstallerObj;
                using (ServiceProcessInstaller procesServiceInstaller = new ServiceProcessInstaller())
                {
                    Cursor.Current = Cursors.WaitCursor;

                    // string BasePath = Path.Combine(Application.StartupPath);
                    // var FullPath = @"/assemblypath="+ BasePath + "\\Prb.WinService.exe";

                    serviceInstallerObj = new ServiceInstaller();
                    //var path = @"/assemblypath=D:\Projects\Zones\Source_Codes\Prb.WinService-05Jan16\Prb.WinSeD:\Projects\Zones\Source_Codes\Prb.WinService\Prb.WinServicervice\Prb.WinService\bin\Debug\Prb.WinService.exe";
                    // var path = @"/assemblypath=D:\Projects\Zones\Source_Codes\Prb.WinService\Prb.ADWinService\bin\Debug\Prb.ADWinService.exe";
                    //var path = @"/assemblypath=E:\TechImplementProjects\1ATranportation\Prb.WinService\Prb.ADWinService\bin\Debug\Prb.ADWinService.exe";
                    //var path = @"/assemblypath=" + Application.StartupPath.Replace("Prb.ADServiceOperation", "Prb.ADWinService") + "\\Prb.ADWinService.exe";
                    var path = @"/assemblypath=" + Application.StartupPath.Replace("Prb.ADServiceOperation", "Prb.ADWinService") + "\\Prb.ADWinService.exe";

                    //C:\Prb.WinService\Prb.WinService\Prb.ADWinService\bin\Debug
                    string[] cmdline = { path };

                    var context = new InstallContext("", cmdline);
                    serviceInstallerObj.Context = context;
                    serviceInstallerObj.DisplayName = "Collect information of  Active Directory from the network";
                    serviceInstallerObj.Description = "Prb.ADWinService";
                    serviceInstallerObj.ServiceName = "PrbADWinService";
                    serviceInstallerObj.StartType = ServiceStartMode.Automatic;
                    serviceInstallerObj.Parent = procesServiceInstaller;

                    Cursor.Current = Cursors.Default;
                }
                var state = new ListDictionary();
                serviceInstallerObj.Install(state);
                txtMessage.AppendText("Service Installation Successfull!!" + "\n\n", Color.Green);
            }
            catch (Exception ex)
            {
                txtMessage.AppendText(ex.Message + "!!Service Installation Failed!!" + "\n\n", Color.Red);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {

            //  System.Diagnostics.Debugger.Launch();
            // Curser  
            Cursor.Current = Cursors.WaitCursor;


            // Probe Running Settings Start

            // Probe Setting
            _Prb_SettingDTO.CustomerName = tbDmoanName.Text;


            // Scanning type
            _Prb_SettingDTO.HardwareDetail = checkBoxHwDetail.Checked;
            _Prb_SettingDTO.SoftwareDetail = checkBoxSwDetail.Checked;

            // Probe Scanning 
            if (radioButtonDomainAuto.Checked == true)
            {
                _Prb_SettingDTO.DomainType = "BySite";
                _Prb_SettingDTO.DomainAdminUser = "";
                _Prb_SettingDTO.Password = "";
            }
            else
            {
                _Prb_SettingDTO.DomainType = "Custom";
                _Prb_SettingDTO.DomainAdminUser = tbDomainAdmin.Text;
                _Prb_SettingDTO.Password = tbPassword.Text;
            }

            _Prb_SettingDTO.WindowsAccess = checkBoxWin.Checked;
            _Prb_SettingDTO.MacAccess = checkBoxMac.Checked;
            _Prb_SettingDTO.LinuxAccess = checkBoxLinux.Checked;
            _Prb_SettingDTO.PrintersAccess = checkBoxPrinters.Checked;
            _Prb_SettingDTO.RoutersSwitchesAccess = checkBoxRoutersSwitches.Checked;
            _Prb_SettingDTO.OthersAccess = checkBoxOthers.Checked;

            txtMessage.AppendText("Service Strating Progress!!" + "\n\n", Color.Purple);
            ServiceController service = new ServiceController("PrbADWinService");
            try
            {
                // Add Probe Setting to DB
                _Prb_SettingDTO.SettingId = _Prb_SettingsDAL.AddProbeSetting(_Prb_SettingDTO);
                //XmlFileCreateProbeSetting();
                //XmlFileReadProbeSetting();


                var _prb_settingObj = _Prb_SettingsDAL.GetProbeSetting(_Prb_SettingDTO);
                if (_prb_settingObj != null)
                {
                    Console.WriteLine("Probe Settings Data Succefully Push And Update in XML and Get info  for Service");
                    // MessageBox.Show("GetInfo");
                }


                if (service.Status != ServiceControllerStatus.Running)
                {
                    var timeout = TimeSpan.FromDays(timeoutMilliseconds);
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                    //System.Threading.Thread.Sleep(1000 * 2);
                    txtMessage.AppendText("Service Started Successfully!!" + "\n\n", Color.Green);
                    Common.WriteTextFile.WriteErrorLog("=================================Service Started Successfully==================================");
                }
                else
                {
                    txtMessage.AppendText("Service Start queued Successfully!!" + "\n\n", Color.Green);
                }
            }
            catch (Exception ex)
            {
                txtMessage.AppendText(ex.Message + "!!Service Start Failed!!" + "\n\n", Color.Red);
            }
            Cursor.Current = Cursors.Default;
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            ServiceController service = new ServiceController("PrbADWinService");
            try
            {
                var millisec1 = Environment.TickCount;
                var timeout = TimeSpan.FromDays(timeoutMilliseconds);

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                // count the rest of the timeout
                var millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.Start();
                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
                txtMessage.AppendText("Service Restarted Successfully!!" + "\n\n", Color.Green);
            }
            catch (Exception ex)
            {
                txtMessage.AppendText(ex.Message + "!!Service Restart Failed!!" + "\n\n", Color.Red);
            }
            Cursor.Current = Cursors.Default;
        }
        private void btnStopService_Click(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController("PrbADWinService");
            try
            {
                var timeout = TimeSpan.FromDays(timeoutMilliseconds);
                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
                txtMessage.AppendText("Service Stopped Successfully!!" + "\n\n", Color.Green);
            }
            catch (Exception ex)
            {
                txtMessage.AppendText(ex.Message + "!!Service Stop Failed!!" + "\n\n", Color.Red);
            }
        }
        private void btnUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                using (ServiceInstaller serviceInstallerObj = new ServiceInstaller())
                {
                    var context = new InstallContext(@"" + Application.StartupPath.Replace("Prb.ADServiceOperation", "Prb.ADWinService") + "\\Prb.ADWinService.exe", null);
                    //var context = new InstallContext(@"E:\TechImplementProjects\1ATranportation\Prb.WinService\Prb.ADWinService\bin\Debug\Prb.ADWinService.exe", null);
                    serviceInstallerObj.Context = context;
                    serviceInstallerObj.ServiceName = "PrbADWinService";
                    serviceInstallerObj.Uninstall(null);
                }
                txtMessage.AppendText("Service Uninstalled Successfully!!" + "\n\n", Color.Green);
            }
            catch (Exception ex)
            {
                txtMessage.AppendText(ex.Message + "!!Service Uninstallation Failed!!" + "\n\n", Color.Red);
            }
        }
        private void btnclearresult_Click(object sender, EventArgs e)
        {
            txtMessage.Text = "";
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void radioButtonDomainSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDomainSelect.Checked == true)
            {
                if (tbDmoanName.Text == "" || tbDomainAdmin.Text == "" || tbPassword.Text == "")
                {
                    MessageBox.Show("Please Fill Domain Information Or Selct By Default Setting");
                }
                tbDomainAdmin.Enabled = true;
                tbPassword.Enabled = true;
            }
            else if (radioButtonDomainAuto.Checked == true)
            {
                // MessageBox.Show("Auto Fill Domain Name");
                tbDomainAdmin.Enabled = false;
                tbPassword.Enabled = false;
            }
        }

        #region XMLfilecode

        #endregion
        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void tbPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PrbADWinService objSvc = new PrbADWinService();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ActiveDirectoryExtraction obj = new ActiveDirectoryExtraction();
            obj.StartActiveDirectoryExecution();
        }
    }
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
            box.SelectionStart = box.TextLength;
            box.ScrollToCaret();
        }
    }
}