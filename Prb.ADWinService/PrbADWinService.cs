using System;
using System.ServiceProcess;
using System.Threading;
using Prb.ActiveDirectoryOperations;


namespace Prb.ADWinService
{
    partial class PrbADWinService : ServiceBase
    {
        public PrbADWinService()
        {
            InitializeComponent();
        }

        ActiveDirectoryExtraction obj = new ActiveDirectoryExtraction();
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
            {
                new PrbADWinService()
            };

            // Debugger for Win-Service
            System.Diagnostics.Debugger.Launch();

            ServiceBase.Run(servicesToRun);
        }

        protected override void OnStart(string[] args)
        {
            Common.WriteTextFile.EmptyErrorLog();
            Console.WriteLine("Service Runing Before Process");

            Common.WriteTextFile.WriteErrorLog("=================================(Inside Service)Prb Service started==================================");
            this.StartProcess();
            Console.WriteLine("Service Runing ");
        }
        /// <summary>
        /// Start Active directory Scan Process
        /// </summary>
        public void StartProcess()
        {
            try
            {
                var worker = new Thread(obj.StartActiveDirectoryExecution);
                worker.IsBackground = false;
                worker.Start();
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Update status on stop
        /// </summary>
        protected override void OnStop()
        {
            obj.UpdateProbeSettingStatus();
        }

    }
}
