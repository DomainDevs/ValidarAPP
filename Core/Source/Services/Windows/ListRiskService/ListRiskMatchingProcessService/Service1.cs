using ListRiskMatchingProcess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcessService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            int processFrequency = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessFrequency"]);
            int processNodes = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessNodes"]);
            int processRetries = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessRetries"]);
            ListRiskMatchingProcessInitializer listRiskMatchingProcessInitializer = new ListRiskMatchingProcessInitializer();
            Task.Run(() => listRiskMatchingProcessInitializer.CheckProcessRequest(processFrequency, processNodes, processRetries));
            //Task.Run(() => listRiskMatchingProcessInitializer.GenerateProcess(processFrequency, processNodes, processRetries, 0, true, ""));
        }

        protected override void OnStop()
        {
        }

        public void InDebug()
        {
            OnStart(null);
        }
    }
}
