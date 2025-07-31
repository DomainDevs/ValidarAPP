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
            ListRiskMatchingProcessInitializer listRiskMatchingProcessInitializer = new ListRiskMatchingProcessInitializer();

            int processFrequency = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessFrequency"]);
            int processNodes = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessNodes"]);
            int processRetries = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessRetries"]);

            listRiskMatchingProcessInitializer.GenerateProcess(processFrequency, processNodes, processRetries);
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
