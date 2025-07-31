using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using Sistran.Company.Application.WrapperAuthorizationPoliciesService;
using Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Business;
using Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Company.Application.WrapperAuthorizationPoliciesServiceEEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class WrapperAuthorizationPoliciesServiceEEProvider : IWrapperAuthorizationPoliciesService
    {
        public WrapperAuthorizationPoliciesServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        #region Masivos
        public void UpdateCollectiveLoadAuthorization(int loadId, int temporalId, List<int> risks)
        {
            try
            {
                DelegateService.CollectiveService.UpdateCollectiveLoadAuthorization(loadId, temporalId, risks);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en UpdateCollectiveLoadAuthorization: {0}", ex.Message));
            }
        }


        public void CompanyUpdateMassiveLoadAuthorization(string loadId, List<string> temporalId)
        {
            try
            {
                DelegateService.MassiveService.CompanyUpdateMassiveLoadAuthorization(loadId, temporalId);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CompanyUpdateMassiveLoadAuthorization: {0}", ex.Message));
            }
        }
        #endregion Masivos

        #region Individuales
        public void CreatePolicyAuthorization(int TemporalId)
        {
            try
            {
                CreatePolicy createPolicy = new CreatePolicy();
                createPolicy.CreatePolicyAuthorization(TemporalId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en  CreatePolicyAuthorization: {0}", ex.Message));
            }
        }
        #endregion

        #region Claims

        public void CreateClaimAuthorization(int claimTemporalId)
        {
            try
            {
                DelegateService.claimService.CreateClaimByTemporalId(claimTemporalId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreateClaimAuthorization: {0}", ex.Message));
            }
        }

        public void CreatePaymentRequestAuthorization(int paymentRequestTemporalId)
        {
            try
            {
                DelegateService.paymentRequestService.CreatePaymentRequestByTemporalId(paymentRequestTemporalId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreatePaymentRequestAuthorization: {0}", ex.Message));
            }
        }


        public void CreateChargeRequestAuthorization(int chargeRequestTemporalId)
        {
            try
            {
                DelegateService.paymentRequestService.CreateChargeRequestByTemporalId(chargeRequestTemporalId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreateChargeRequestAuthorization: {0}", ex.Message));
            }            
        }

        public void CreateClaimNoticeAuthorization(int claimNoticeTemporalId)
        {
            try
            {
                DelegateService.claimService.CreateClaimNoticeByTemporalId(claimNoticeTemporalId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", string.Format(" Error en CreateClaimNoticeAuthorization: {0}", ex.Message));
            }
        }

        #endregion

        #region Personas
        public void CreatePersonAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreatePersonAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateAplicationPerson: {ex.Message}");
            }
        }
        
        public void CreateInsuredAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateInsuredAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateInsuredAuthorization: {ex.Message}");
            }
        }

        public void CreateProviderAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateProviderAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateProviderAuthorization: {ex.Message}");
            }
        }

        public void CreateAgentAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateAgentAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateAgentAuthorization: {ex.Message}");
            }
        }

        public void CreateReInsurerAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateReInsurerAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateReInsurerAuthorization: {ex.Message}");
            }
        }

        public void CreateQuotaAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateQuotaAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateQuotaAuthorization: {ex.Message}");
            }
        }

        public void CreateTaxAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateTaxAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateTaxAuthorization: {ex.Message}");
            }
        }

        public void CreateCoInsuredAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateCoInsuredAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateCoInsuredAuthorization: {ex.Message}");
            }
        }

        public void CreateThirdAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateThirdAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateThirdAuthorization: {ex.Message}");
            }
        }

        public void CreateEmployedAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateEmployedAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateEmployedAuthorization: {ex.Message}");
            }
        }

        public void CreatePersonalInformationAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreatePersonalInformationAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreatePersonalInformationAuthorization: {ex.Message}");
            }
        }

        public void CreatePaymentMethodsAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreatePaymentMethodsAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreatePaymentMethodsAuthorization: {ex.Message}");
            }
        }

        public void CreateBankTransfersAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateBankTransfersAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateBankTransfersAuthorization: {ex.Message}");
            }
        }

        public void CreateConsortiatesAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateConsortiatesAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateConsortiatesAuthorization: {ex.Message}");
            }
        }

        public void CreateBusinessNameAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateBusinessNameAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateBusinessNameAuthorization: {ex.Message}");
            }
        }

        public void CreateGuaranteeAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateGuaranteeAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateGuaranteeAuthorization: {ex.Message}");
            }
        }

        public void CreateBasicInfoAuthorization(int operationId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                personBusiness.CreateBasicInfoAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateBasicInfoAuthorization: {ex.Message}");
            }
        }

        #endregion

        #region Sarlaft
        public void CreateSarlaftAuthorization(int operationId)
        {
            try
            {
                SarlaftBusiness sarlaftBusiness = new SarlaftBusiness();
                sarlaftBusiness.CreateSarlaftAuthorization(operationId);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateSarlaftAuthorization: {ex.Message}");
            }
        }
        #endregion Sarlaft

        #region AutomaticQuota
        public void CreateAutomaticQuotaAuthorization(int id)
        {
            try
            {
                AutomaticQuotaBusiness automaticBusiness = new AutomaticQuotaBusiness();
                automaticBusiness.CreateAutomaticQuotaAuthorization(id);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", $" Error en CreateAplicationPerson: {ex.Message}");
            }
        }
        #endregion AutomaticQuota
    }
}