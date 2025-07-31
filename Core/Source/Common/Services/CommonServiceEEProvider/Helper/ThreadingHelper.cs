using System;
using System.Configuration;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF.Engine;
using System.Collections.Generic;

namespace Sistran.Core.Application.CommonService.Helper
{
    public class ThreadingHelper
    {
        private Context context = null;
        public List<int> PackageProcesses;
        public Context GetContext { get { return context; } }
        public ThreadingHelper()
        {
            PackageProcesses = new List<int>();
            if (Sistran.Core.Framework.Contexts.Context.Current == null)
            {
                context = new Context();
            }
        }

        public void SetPackageProcesses(int processesCount, string tagConfig)
        {
            int packageThreads = Convert.ToInt32(ConfigurationManager.AppSettings[tagConfig]);

            int packages = processesCount / packageThreads;

            for (int i = 0; i < packages; i++)
            {
                PackageProcesses.Add(packageThreads);
            }

            int lastPackage = processesCount % packageThreads;

            if (lastPackage > 0)
            {
                PackageProcesses.Add(lastPackage);
            }
        }
        public void ClearContex()
        {
            context = null;
        }
    }
}