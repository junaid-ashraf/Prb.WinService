namespace Prb.ADWinService
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ADServiceInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();
            // 
            // ADServiceInstaller
            // 
            this.ADServiceInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ADServiceInstaller.Password = null;
            this.ADServiceInstaller.Username = null;
            // 
            // serviceInstaller1
            // 
            this.serviceInstaller1.Description = "Collect information from the network";
            this.serviceInstaller1.DisplayName = "Prb.ADWinService";
            this.serviceInstaller1.ServiceName = "PrbADWinService";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ADServiceInstaller,
            this.serviceInstaller1});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ADServiceInstaller;
        private System.ServiceProcess.ServiceInstaller serviceInstaller1;
    }
}