using ConsumerRuleSetCacheQueue;
using System.ServiceProcess;

namespace ConsumerRuleSetCacheQueueService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            InitializerConsumers.InitializeQuee();
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