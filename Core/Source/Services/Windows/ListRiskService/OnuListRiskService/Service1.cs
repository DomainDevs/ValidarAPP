using OnuListRisk;
using System;
using System.Configuration;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace OnuListRiskService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            OnuListRiskInitializer onuListRiskInitializer = new OnuListRiskInitializer();
            
            int processFrequency = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessFrequency"]);
            int processNodes = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessNodes"]);
            int processRetries = Convert.ToInt32(ConfigurationManager.AppSettings["ProcessRetries"]);
            Task.Run(() => onuListRiskInitializer.CheckProcessRequest(processFrequency, processNodes, processRetries));
            Task.Run(() => onuListRiskInitializer.GenerateProcess(processFrequency, processNodes, processRetries, null));       
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
