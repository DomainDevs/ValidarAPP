using System;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;

namespace ConsumerQueueAuditService
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
            installer.ServiceName = GetConfigurationValue("ServiceName", "SistranConsumerQueueAudit");
            installer.DisplayName = GetConfigurationValue("DisplayName", "Sistran Consumer Queue Audit");
            installer.Description = "Servicio que toma los mensajes de RabbitMQ y los envía al servicio correspondiente";
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