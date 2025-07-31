using System;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace ListRiskMatchingProcessService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            Installers.Add(GetServiceInstaller());
            Installers.Add(GetServiceProcessInstaller());
        }

        private ServiceInstaller GetServiceInstaller()
        {
            ServiceInstaller installer = new ServiceInstaller();
            installer.ServiceName = GetConfigurationValue("ServiceName", "SASServiceNseInt4310ListRiskMatching");
            installer.DisplayName = GetConfigurationValue("DisplayName", "SAS Service (Nse Int 4310 List Risk Matching)");
            installer.Description = "Servicio que gestiona y administra la lista de riesgos Matching";
            installer.StartType = ServiceStartMode.Automatic;
            return installer;
        }

        private ServiceProcessInstaller GetServiceProcessInstaller()
        {
            ServiceProcessInstaller installer = new ServiceProcessInstaller();
            installer.Account = ServiceAccount.LocalSystem;
            return installer;
        }

        private string GetConfigurationValue(string key, string defaultValue)
        {
            Assembly service = Assembly.GetAssembly(typeof(ProjectInstaller));

            Configuration config = ConfigurationManager.OpenExeConfiguration(service.Location);

            if (config.AppSettings.Settings[key] != null)
            {
                return config.AppSettings.Settings[key].Value;
            }
            else
            {
                return defaultValue;
            }
        }
    }
}