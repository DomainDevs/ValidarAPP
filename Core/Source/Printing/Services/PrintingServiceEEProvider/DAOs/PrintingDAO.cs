using Newtonsoft.Json;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.PrintingServicesEEProvider.Assemblers;
using Sistran.Core.Application.Report.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;

namespace Sistran.Core.Application.PrintingServicesEEProvider.DAOs
{
    public class PrintingDAO
    {
        /// <summary>
        /// Obtener Resumen
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Resumen</returns>
        public Summary GetSummaryByEndorsementId(int endorsementId)
        {
            Summary summary = DelegateService.underwritningServiceCore.GetSummaryByEndorsementId(endorsementId);
            return summary;
        }

        /// <summary>
        /// Obtener Resumen
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Resumen</returns>
        public Summary GetSummaryByTemporalId(int temporalId)
        {
            PendingOperation pendingOperation = new PendingOperation();
            pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(temporalId);
            if (pendingOperation != null && pendingOperation.Operation != null) {
                Policy policy = JsonConvert.DeserializeObject<Policy>(pendingOperation.Operation);
                return policy.Summary;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Insertar en la tabla LogPrintStatus
        /// </summary>
        public void RegisterLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO)
        {
            LogPrintStatus printStatus = Assemblers.EntityAssembler.CreateLogPrintStatus(logPrintStatusDTO);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(printStatus);
        }


        /// <summary>
        /// Insertar en la tabla LogErrorPrint
        /// </summary>
        public void RegisterLogErrorPrint(LogErrorPrintDTO logErrorPrintDTO)
        {
            LogErrorPrint errorPrint = Assemblers.EntityAssembler.CreateLogErrorPrint(logErrorPrintDTO);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(errorPrint);
        }

        /// <summary>
        /// Actualiza en la tabla LogErrorPrint
        /// </summary>
        public void UpdateLogErrorPrint(LogErrorPrintDTO logErrorPrintDTO)
        {
            LogErrorPrint errorPrint = Assemblers.EntityAssembler.CreateLogErrorPrint(logErrorPrintDTO);
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(errorPrint);
        }

        /// <summary>
        /// Actualiza en la tabla LogPrintStatus
        /// </summary>
        public void UpdateLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO)
        {
            PrimaryKey key = LogPrintStatus.CreatePrimaryKey(logPrintStatusDTO.Id);
            LogPrintStatus logStatus = (LogPrintStatus)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

               
                logStatus.Observacion = logPrintStatusDTO.Observacion;
                logStatus.StatusId = logPrintStatusDTO.StatusId;
                logStatus.Date = logPrintStatusDTO.Date;
                logStatus.UserName = logPrintStatusDTO.UserName;
                logStatus.Url = logPrintStatusDTO.Url;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(logStatus);
        }

        /// <summary>
        /// Obtiene de la tabla LogPrintStatus
        /// </summary>

        public LogPrintStatusDTO GetLogPrintStatus(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(LogPrintStatus.Properties.EndorsementId, typeof(LogPrintStatusDTO).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            BusinessCollection businessCollection = null;
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(LogPrintStatus), filter.GetPredicate()));
            return ModelAssembler.CreateLogsStatus(businessCollection).FirstOrDefault();
        }

        //private void CreatePrintLog(PrintLog printLog)
        //{

        //    PrimaryKey primaryKey = MSVEN.MassiveLoadStatus.CreatePrimaryKey((int)printLog.KEY_ID);
        //    MSVEN.MassiveLoadStatus entityMassiveLoadStatus = (MSVEN.MassiveLoadStatus)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

        //    PrintLog entityMassivePrintLog = new PrintLog
        //    {
        //        MassiveLoadId = massiveLoad.Id,
        //        Description = entityMassiveLoadStatus.Description,
        //        Time = DateTime.Now
        //    };

        //    DataFacadeManager.Instance.GetDataFacade().InsertObject(entityMassivePrintLog);

        //    DataFacadeManager.Dispose();

        //}
    }
}