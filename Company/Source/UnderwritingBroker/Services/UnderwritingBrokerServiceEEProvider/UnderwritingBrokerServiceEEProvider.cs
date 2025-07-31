using Sistran.Company.Application.UnderwritingBrokerService;
using Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Business;
using Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Resources;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.ServiceModel;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Company.Application.UnderwritingBrokerServiceEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UnderwritingBrokerServiceEEProvider : IUnderwritingBrokerService
    {
        public UnderwritingBrokerServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public int CreatePendingOperation(string businessCollection)
        {
            try
            {
                BusinessPendingOperation businessPendingOperation = new BusinessPendingOperation();
                return businessPendingOperation.CreatePendingOperation(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatingPendingOperation), ex);
            }
        }

        public int CreatePendingOperationWithParent(string businessCollection)
        {
            try
            {
                BusinessPendingOperation businessPendingOperation = new BusinessPendingOperation();
                return businessPendingOperation.CreatePendingOperationWithParent(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreatingPendingOperation), ex);
            }
        }

        public int UpdatePendingOperation(string businessCollection)
        {
            try
            {
                BusinessPendingOperation businessPendingOperation = new BusinessPendingOperation();
                return businessPendingOperation.UpdatePendingOperation(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdatingPendingOperation), ex);
            }
        }

        public void ProcessResponseFromExperienceServiceHistoricPolicies(string businessCollection)
        {
            try
            {
                BusinessExternalServices businessExternalServices = new BusinessExternalServices();
                businessExternalServices.ProcessResponseFromExperienceServiceHistoricPolicies(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorProcessingResponseFromExperienceServiceHistoricPolicies), ex);
            }
        }


        public void ProcessResponseFromExperienceServiceHistoricSinister(string businessCollection)
        {
            try
            {
                BusinessExternalServices businessExternalServices = new BusinessExternalServices();
                businessExternalServices.ProcessResponseFromExperienceServiceHistoricSinister(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorProcessingResponseFromExperienceServiceHistoricSinister), ex);
            }
        }

        public void ProcessResponseFromScoreService(string businessCollection)
        {
            try
            {
                BusinessExternalServices businessExternalServices = new BusinessExternalServices();
                businessExternalServices.ProcessResponseFromScoreService(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorProcessingResponseFromScoreService), ex);
            }
        }

        public void ProcessResponseFromSimitService(string businessCollection)
        {
            try
            {
                BusinessExternalServices businessExternalServices = new BusinessExternalServices();
                businessExternalServices.ProcessResponseFromSimitService(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorProcessingResponseFromSimitService), ex);
            }
        }

        public string ExecuteCreatePolicy(string businessCollection)
        {
            try
            {
                BusinessIssuance businessIssuance = new BusinessIssuance();
                return businessIssuance.ExecuteCreatePolicy(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingPolicy), ex);
            }
        }

        public void UpdatePOAndRecordEndorsementOperation(string businessCollection)
        {
            try
            {
                BusinessIssuance businessIssuance = new BusinessIssuance();
                businessIssuance.UpdatePOAndRecordEndorsementOperation(businessCollection);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingPolicy), ex);
            }
        }

        public void UpdateMassiveEmissionRows(string businessCollection, string error)
        {
            try
            {
                BusinessMassiveRows businessMassiveRows = new BusinessMassiveRows();
                businessMassiveRows.UpdateMassiveEmissionRows(businessCollection, error);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateRows), ex);
            }
        }

    }
}