using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using REPMOD = Sistran.Core.Application.ReportingServices.Models;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Helper
{
    /// <summary>
    /// WorkerFactory
    /// </summary>
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
        /// <param name="moduleId"></param>
        /// <param name="processId"></param>
        public void CreateWorker(int processId, int moduleId)
        {
            try
            {

                ReinsuranceApplicationServiceProvider reinsuranceApplicationServiceProvider = new ReinsuranceApplicationServiceProvider();
                Thread thread = new Thread(() => reinsuranceApplicationServiceProvider.ReinsuranceMassive(processId, moduleId));
                thread.Start();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceMasive), ex);
            }
        }

        /// <summary>
        /// CreateWorkerStructure
        /// </summary>
        /// <param name="isExcel"></param>
        /// <param name="report"></param>
        public void CreateWorkerStructure(REPMOD.Report report, bool isExcel)
        {
            try
            {
                if (isExcel)
                {
                    Thread thread = new Thread(() => DelegateService.reportingService.GenerateFileByReport(report));
                    thread.Start();
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }

        /// <summary>
        /// CreateWorkerReportByStoreProcedure
        /// </summary>
        /// <param name="report"></param>
        /// <param name="massiveReport"></param>
        public void CreateWorkerReportByStoreProcedure(REPMOD.Report report, REPMOD.MassiveReport massiveReport)
        {
            try
            {
                Thread thread = new Thread(() => DelegateService.reportingService.GenerateMassiveReport(report, massiveReport));
                thread.Start();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Resources.ErrorReinsuranceReport), ex);
            }
        }
    }
}
