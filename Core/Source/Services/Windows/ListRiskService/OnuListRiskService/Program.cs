using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace OnuListRiskService
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
#if DEBUG
            var ServicesToRun = new Service1();
            ServicesToRun.InDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

#else
            //In Release this section is used. This is the "normal" way.
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new Service1()
            };
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
