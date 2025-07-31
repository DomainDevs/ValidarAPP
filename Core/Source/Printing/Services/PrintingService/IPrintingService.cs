using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Core.Application.PrintingServices
{
    [ServiceContract]
    public interface IPrintingServiceCore
    {
        /// <summary>
        /// Obtener Resumen
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// <returns>Resumen</returns>
        [OperationContract]
        Summary GetSummaryByEndorsementId(int endorsementId);

        /// <summary>
        /// Obtener Resumen
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Resumen</returns>
        [OperationContract]
        Summary GetSummaryByTemporalId(int temporalId);

        /// <summary>
        /// Insertar LogPrintStatus
        /// </summary>
        /// <returns>Resumen</returns>
        [OperationContract]
        LogPrintStatusDTO RegisterLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO);

        /// <summary>
        /// Actualiza LogPrintStatus
        /// </summary>
        /// <returns>Resumen</returns>
        [OperationContract]
        LogPrintStatusDTO UpdateLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO);

        /// <summary>
        /// Obtiene LogPrintStatus
        /// </summary>
        /// <returns>Resumen</returns>
        [OperationContract]
        LogPrintStatusDTO GetLogPrintStatus(int endorsementId);

        /// <summary>
        /// Inserta LogErrorPrint
        /// </summary>
        /// <returns>Resumen</returns>
        [OperationContract]
        LogErrorPrintDTO RegisterLogErrorPrint(LogErrorPrintDTO logErrorPrintDTO);

        /// <summary>
        /// Actualiza LogErrorPrint
        /// </summary>
        /// <returns>Resumen</returns>
        [OperationContract]
        LogErrorPrintDTO UpdateLogErrorPrint(LogErrorPrintDTO logErrorPrintDTO);










    }
}