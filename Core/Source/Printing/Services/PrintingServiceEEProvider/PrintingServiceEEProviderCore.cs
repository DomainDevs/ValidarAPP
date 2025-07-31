using Sistran.Core.Application.PrintingServices;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.PrintingServicesEEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.ServiceModel;

namespace Sistran.Core.Application.PrintingServicesEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class PrintingServiceEEProviderCore : IPrintingServiceCore
    {
        /// <summary>
        /// Obtener Resumen
        /// </summary>
        /// <param name="endorsementId">Id Endoso</param>
        /// /// <param name="isCurrent">endoso Actual</param>
        /// <returns>Resumen</returns>
        public Summary GetSummaryByEndorsementId(int endorsementId)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                return printingDAO.GetSummaryByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al obtener resumen"));
            }
        }

        /// <summary>
        /// Obtener Resumen
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Resumen</returns>
        public Summary GetSummaryByTemporalId(int temporalId)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                return printingDAO.GetSummaryByTemporalId(temporalId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al obtener resumen"));
            }
        }

        /// <summary>
        /// Metodo que realiza el grabado del estado de impresión.
        /// </summary>
        /// <returns></returns>
        public LogPrintStatusDTO RegisterLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                printingDAO.RegisterLogPrintStatus(logPrintStatusDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al registrar LogPrintStatus"));
            }
            return logPrintStatusDTO;
        }

        /// <summary>
        /// Metodo que realiza la actualización del estado de impresión.
        /// </summary>
        /// <returns></returns>
        public LogPrintStatusDTO UpdateLogPrintStatus(LogPrintStatusDTO logPrintStatusDTO)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                printingDAO.UpdateLogPrintStatus(logPrintStatusDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al actualizar LogPrintStatus"));
            }
            return logPrintStatusDTO;
        }

        /// <summary>
        /// Metodo que obtiene el estado de impresión de un endoso.
        /// </summary>
        /// <returns></returns>
        public LogPrintStatusDTO GetLogPrintStatus(int endorsementId)
        {
            LogPrintStatusDTO result = new LogPrintStatusDTO();
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                result = printingDAO.GetLogPrintStatus(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al obtener LogPrintStatus"));
            }
            return result;
        }

        /// <summary>
        /// Metodo que realiza el grabado del estado de impresión.
        /// </summary>
        /// <returns></returns>
        public LogErrorPrintDTO RegisterLogErrorPrint(LogErrorPrintDTO logErrorPrintDTO)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                printingDAO.RegisterLogErrorPrint(logErrorPrintDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al registrar LogErrorPrint"));
            }
            return logErrorPrintDTO;
        }

        /// <summary>
        /// Metodo que realiza la actualizacion del estado de impresión.
        /// </summary>
        /// <returns></returns>
        public LogErrorPrintDTO UpdateLogErrorPrint(LogErrorPrintDTO logErrorPrintDTO)
        {
            try
            {
                PrintingDAO printingDAO = new PrintingDAO();
                printingDAO.UpdateLogErrorPrint(logErrorPrintDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error al actualizar LogErrorPrint"));
            }
            return logErrorPrintDTO;
        }
    }
}