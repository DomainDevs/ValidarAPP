using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ListRiskMatchingProcessService
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {
            var ServicesToRun = new Service1();
            ServicesToRun.InDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
        }
    }
}
