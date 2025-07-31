using System;
using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CollectiveServices.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CollectiveServices.EEProvider.Resources;

namespace Sistran.Core.Application.CollectiveServices.EEProvider
{
    using AuthorizationPoliciesServices.Models;
    using MassiveServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using UnderwritingServices.Models;
    using Utilities.Configuration;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CollectiveServiceEEProviderCore : ICollectiveServiceCore
    {
        public CollectiveServiceEEProviderCore()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            try
            {
                CollectiveEmissionDAO dao = new CollectiveEmissionDAO();
                return dao.CreateCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateMassiveLoad), ex);
            }
        }

        public CollectiveEmissionRow CreateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow)
        {
            try
            {
                CollectiveEmissionRowDAO dao = new CollectiveEmissionRowDAO();
                return dao.CreateCollectiveEmissionRow(collectiveEmissionRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateRow), ex);
            }
        }

        public CollectiveEmission UpdatCollectiveEmission(CollectiveEmission collectiveEmission)
        {
            try
            {
                CollectiveEmissionDAO dao = new CollectiveEmissionDAO();
                return dao.UpdateCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateMassiveLoad), ex);
            }
        }

        public CollectiveEmissionRow UpdateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow)
        {
            try
            {
                CollectiveEmissionRowDAO dao = new CollectiveEmissionRowDAO();
                return dao.UpdateCollectiveEmissionRow(collectiveEmissionRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateRow), ex);
            }
        }

        /// <summary>
        /// Actualiza los temporales a excluir
        /// </summary>
        /// <param name="massiveLoadId">Id del cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Usuario Actual</param>
        /// <param name="deleteTemporal">Si debe eliminar el temporal</param>
        public CollectiveEmission ExcludeCollectiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            try
            {
                CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();
                return collectiveEmissionDAO.ExcludeCollectiveEmissionRowsTemporals(massiveLoadId, temps, userName, deleteTemporal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeRows), ex);
            }
        }

        public CollectiveEmission GetCollectiveEmissionByMassiveLoadId(int massiveLoadId, bool withRows, bool? withErrors = null, bool? withEvents = null)
        {
            try
            {
                CollectiveEmissionDAO dao = new CollectiveEmissionDAO();
                return dao.GetCollectiveEmissionByMassiveLoadIdWithRowsErrors(massiveLoadId, withRows, withErrors, withEvents);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoad), ex);
            }
        }

        public List<CollectiveEmission> GetCollectiveEmissionsByTempId(int tempId, bool withRows, bool withEvents)
        {
            try
            {
                CollectiveEmissionDAO dao = new CollectiveEmissionDAO();
                return dao.GetCollectiveEmissionsByTempId(tempId, withRows, withEvents);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoads), ex);
            }
        }

        /// <summary>
        /// Actualizar Proceso De Cargue coletivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public List<CollectiveEmissionRow> GetCollectiveEmissionRowByMassiveLoadId(int collectiveLoadId, CollectiveLoadProcessStatus collectiveLoadProcessStatus)
        {
            try
            {
                CollectiveEmissionRowDAO dao = new CollectiveEmissionRowDAO();
                return dao.GetCollectiveEmissionRowByMassiveLoadId(collectiveLoadId, collectiveLoadProcessStatus);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRows), ex);
            }
        }

        // <summary>
        // Genera el archivo de error del proceso de emisión colectiva
        // </summary>
        // <param name = "massiveLoadId" ></ param >
        // < returns ></ returns >
        public string GenerateFileErrorsCollective(int massiveLoadId, FileProcessType fileProcessType)
        {
            try
            {
                CollectiveEmissionDAO collectiveEmissionDAO = new CollectiveEmissionDAO();
                return collectiveEmissionDAO.GenerateFileErrorsCollective(massiveLoadId, fileProcessType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }

        /// <summary>
        /// Actualizar Proceso De Cargue coletivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public List<CollectiveEmissionRow> GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(int collectiveLoadId, CollectiveLoadProcessStatus? collectiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            try
            {
                CollectiveEmissionRowDAO dao = new CollectiveEmissionRowDAO();
                return dao.GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(collectiveLoadId, collectiveLoadProcessStatus, withError, withEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRows), ex);
            }
        }



        public List<AuthorizationRequest> ValidateAuthorizationPoliciesRisk(List<PoliciesAut> policiesAuthorization, MassiveLoad massiveLoad, int policyId, int riskId)
        {
            try
            {
                CollectiveEmissionDAO emissionDao = new CollectiveEmissionDAO();
                return emissionDao.ValidateAuthorizationPoliciesRisk(policiesAuthorization, massiveLoad, policyId, riskId);
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

        public List<AuthorizationRequest> ValidateAuthorizationPoliciesPolicy(List<PoliciesAut> policiesAuthorization, MassiveLoad massiveLoad, int policyId)
        {
            try
            {
                CollectiveEmissionDAO emissionDao = new CollectiveEmissionDAO();
                return emissionDao.ValidateAuthorizationPolicies(policiesAuthorization, massiveLoad, policyId);
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

        public CollectiveEmissionRow GetCollectiveEmissionRowById(int id)
        {
            try
            {
                return new CollectiveEmissionRowDAO().GetCollectiveEmissionRowById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorEmissionCollectiveRow), ex);
            }

        }

        public void UpdateCollectiveLoadAuthorization(int loadId, int temporalId, List<int> risks)
        {
            try
            {
                new CollectiveEmissionDAO().UpdateCollectiveLoadAuthorization(loadId, temporalId, risks);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""/*Errors.ErrorEmissionCollectiveRow*/), ex);
            }
        }
    }
}