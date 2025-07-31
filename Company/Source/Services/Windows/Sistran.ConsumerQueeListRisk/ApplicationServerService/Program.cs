using System;
using System.ServiceProcess;


namespace ApplicationServerService
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            try
            {
                //ServiceBase[] ServicesToRun;
                //ServicesToRun = new ServiceBase[]
                //{
                //    new Service1()
                //};
                //ServiceBase.Run(ServicesToRun);

                #region PARA DEPURAR

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

                #endregion PARA DEPURAR
            }
            catch (Exception e)
            {
              //  EventViewerAsistant.SingleInstance.WriteInEventViewer(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }
    }
}