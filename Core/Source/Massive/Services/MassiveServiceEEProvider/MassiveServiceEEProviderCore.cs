using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveServices.EEProvider.Resources;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.PrintingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Core.Application.MassiveServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveServiceEEProviderCore : IMassiveServiceCore
    {
        public MassiveServiceEEProviderCore()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Obtener Tipos de Cargue Por Tipo de Proceso
        /// </summary>
        /// <param name="massiveProcessType">Tipo de Proceso</param>
        /// <returns>Tipos de Cargue</returns>
        public List<LoadType> GetLoadTypesByMassiveProcessType(MassiveProcessType massiveProcessType)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetLoadTypesByMassiveProcessType(massiveProcessType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetLoadTypes), ex);
            }
        }

        /// <summary>
        /// Crear Cargue
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad CreateMassiveLoad(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.CreateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Actualizar Cargue
        /// </summary>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad UpdateMassiveLoad(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.UpdateMassiveLoad(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Obtener Cargue Por Identificador
        /// </summary>
        /// <param name="massiveLoadId">Id Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveLoad GetMassiveLoadByMassiveLoadId(int massiveLoadId)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetMassiveLoadByMassiveLoadId(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Obtener Cargues Por Descripción
        /// </summary>
        /// <param name="description">Descripción</param>
        /// <returns>Cargues</returns>
        public virtual List<MassiveLoad> GetMassiveLoadsByDescription(string description)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetMassiveLoadsByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoads), ex);
            }
        }

        /// <summary>
        /// Obtener Cargues Por Fecha, Tipo de Proceso, Tipo de Cargue, Descripción, Usuario
        /// </summary>
        /// <param name="rangeFrom">Fecha Desde</param>
        /// <param name="rangeTo">Fecha Hasta</param>
        /// <param name="massiveLoad">Cargue</param>
        /// <returns>Cargues</returns>
        public List<MassiveLoad> GetMassiveLoadsByRangeFromRangeToMassiveLoad(DateTime rangeFrom, DateTime rangeTo, MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetMassiveLoadsByRangeFromRangeToMassiveLoad(rangeFrom, rangeTo, massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoads), ex);
            }
        }

        public Printing CreatePrinting(Printing printing)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.CreatePrinting(printing);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatePrinting), ex);
            }
        }

        public void CreatePrintLog(PrintingLog printingLog)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                massiveLoadDAO.CreatePrintLog(printingLog);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatePrintingLog), ex);
            }
        }

        public Printing UpdatePrinting(Printing printing)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.UpdatePrinting(printing);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdatePrinting), ex);
            }
        }

        public string DeleteProcess(MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.DeleteProcess(massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorDeletePrinting), ex);
            }
        }

        public Printing GetPrinting(int massiveLoadId, int userId)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetPrinting(massiveLoadId, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrinting), ex);
            }
        }

        public string GenerateMassiveProcessReport(List<MassiveLoad> massiveLoads)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GenerateMassiveProcessReport(massiveLoads);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }

        public List<AuthorizationRequest> GetAuthorizationPolicies(Risk risk, MassiveLoad massiveLoad)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetAuthorizationPolicies(risk, massiveLoad);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorValidateAuthorizationPolicies), ex);
            }
        }

        public List<AuthorizationRequest> ValidateAuthorizationPolicies(List<PoliciesAut> infringementPolicies, MassiveLoad massiveLoad, int temporalId)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.ValidateAuthorizationPolicies(infringementPolicies, massiveLoad, temporalId);
            }
            catch (BusinessException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorValidateAuthorizationPolicies), ex);
            }
        }
        

        public Printing GetPrintingByPrintingId(int printingId)
        {
            try
            {
                MassiveLoadDAO massiveLoadDAO = new MassiveLoadDAO();
                return massiveLoadDAO.GetPrintingByPrintingId(printingId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetPrinting), ex);
            }
        }
        // <summary>
        // Metodo que realiza la actualización de la fila del cargue desde politicas
        // </summary>
        // <param name="massiveLoadId">Id del cargue</param>
        // <param name="temporalId">Id del temporal</param>
        // <returns></returns>
        public string UpdateMassiveLoadAuthorization(string massiveLoadId, List<string> temporalId)
        {
            try
            {
                MassiveLoadDAO loadDao = new MassiveLoadDAO();
                loadDao.UpdateMassiveLoadAuthorization(massiveLoadId, temporalId);
                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateMassiveLoad), ex);
            }
        }

    }
}