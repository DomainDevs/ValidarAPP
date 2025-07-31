using Sistran.Core.Application.ReportingServices.Models;
using System;
using System.Threading;

namespace Sistran.Core.Application.AccountingClosingServices.EEProvider
{
    public sealed class WorkerFactory
    {
        private static volatile WorkerFactory _instance;
        private static object syncRoot = new Object();

        public static WorkerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new WorkerFactory();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// CreateWorker
        /// </summary>
        /// <param name="report"></param>
        public void CreateWorker(Report report)
        {
            Thread thread = new Thread(() => DelegateService.reportService.GenerateReport(report));
            thread.Start();
        }

        /// <summary>
        /// CreateWorker
        /// </summary>
        /// <param name="report"></param>
        /// <param name="isExcel"></param>
        public void CreateWorker(Report report, bool isExcel)
        {
            if (isExcel)
            {
                Thread thread = new Thread(() => DelegateService.reportService.GenerateMassiveReport(report, null));
                thread.Start();
            }
        }

        /// <summary>
        /// CreateWorkerStructure
        /// </summary>
        /// <param name="report"></param>
        /// <param name="isExcel"></param>
        public void CreateWorkerStructure(Report report, bool isExcel)
        {
            if (isExcel)
            {
                Thread thread = new Thread(() => DelegateService.reportService.GenerateFileByReport(report));
                thread.Start();
            }
        }

        /// <summary>
        /// CreateWorkerReportByStoreProcedure
        /// </summary>
        /// <param name="report"></param>
        /// <param name="massiveReport"></param>
        public void CreateWorkerReportByStoreProcedure(Report report, MassiveReport massiveReport)
        {
            Thread thread = new Thread(() => DelegateService.reportService.GenerateMassiveReport(report, massiveReport));
            thread.Start();
        }
    }
}
