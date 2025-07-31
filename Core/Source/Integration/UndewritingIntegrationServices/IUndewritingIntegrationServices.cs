using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Payment;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
namespace Sistran.Core.Integration.UndewritingIntegrationServices
{

    [ServiceContract]
    public interface IUndewritingIntegrationServices
    {
        [OperationContract]
        List<PolicyDTO> GetClaimPoliciesByPolicy(PolicyDTO policyDTO);

        [OperationContract]
        PolicyDTO GetClaimPolicyByEndorsementId(int endorsementId);

        [OperationContract]
        List<InsuredDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        [OperationContract]
        List<HolderDTO> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        [OperationContract]
        List<CoverageDTO> GetCoveragesByRiskId(int riskId);

        [OperationContract]
        List<CoverageDTO> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime? occurrenceDate, decimal companyParticipationPercentage);

        [OperationContract]
        DeductibleDTO GetCoverageDeductibleByCoverageId(int coverageId);

        [OperationContract]
        List<CoverageDTO> GetCoveragesByLineBusinessId(int lineBusinessId);

        [OperationContract]
        List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBussinessId, int subLineBussinessId);

        [OperationContract]
        List<DeductibleDTO> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum);

        [OperationContract]
        List<RiskCommercialClassDTO> GetRiskCommercialClasses();

        [OperationContract]
        PolicyDTO GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber);

        [OperationContract]
        List<CoInsuranceAssignedDTO> GetCoInsuranceByPolicyIdByEndorsementId(int policyId, int endorsementId);

        [OperationContract]
        List<InsuredObjectDTO> GetInsuredObjectByPrefixIdList(int prefixId);

        [OperationContract]
        PayerPaymentDTO GetPayerPaymet(int EndorsementId, int Quota);

        [OperationContract]
        void UpdateStatusPayerPayment(PayerPaymentDTO payerPayment);

        [OperationContract]
        List<PayerPaymentComponentDTO> GetPayerPaymetComponents(int PayerPaymentId);

        [OperationContract]
        List<PayerPaymentComponentDTO> GetPayerPaymetComponentsByEndorsementIdQuota(int EndorsementId, int Quota);

        [OperationContract]
        List<PayerPaymentComponentLBSBDTO> GetPayerPaymetComponentsLBSB(int PayerPaymentId);

        [OperationContract]
        List<RiskDTO> GetRisksByPolicyIdEndorsmentId(int policyId, int endorsementId);

        [OperationContract]
        RiskDTO GetRiskSuretyByRiskId(int riskId);

        [OperationContract]
        List<PremiumSearchPolicyDTO> GetPremiuPaymSearchPolicies(SearchPolicyPaymentDTO searchPolicyPaymentDTO);

        [OperationContract]
        List<CoverageDTO> GetCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId);

        [OperationContract]
        List<CoInsuranceAssignedDTO> GetCoInsuranceByEndorsementId(int endorsementId);

        [OperationContract]
        List<IssuanceAgencyDTO> GetAgentsByPolicyIdEndorsementId(int? policyId, int? endorsementId);

        [OperationContract]
        List<EndorsementBaseDTO> GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, int riskId = 0, bool isCurrent = false);
        [OperationContract]
        PolicyDTO GetPolicyByPolicyId(int policyId);


        #region Recuotificacion
        /// <summary>
        /// Obtener endosos de una poliza
        /// </summary>
        /// <param name="policyFilterDTO">The policy filter dto.</param>
        /// <returns></returns>
        [OperationContract]
        List<EndorsementBaseDTO> GetEndorsementsByPolicyFilter(PolicyFilterDTO policyFilterDTO);

        /// <summary>
        /// Gets the policies by policy filter.
        /// </summary>
        /// <param name="policyFilterDTO">The policy filter dto.</param>
        /// <returns></returns>
        [OperationContract]
        Task<List<decimal>> GetPoliciesByPolicyFilter(PolicyFilterDTO policyFilterDTO);

        /// <summary>
        /// Gets the payers by endorsement filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        List<PayerBaseDTO> GetPayersByEndorsementFilter(FilterBaseDTO filterBase);

        /// <summary>
        /// Gets the financial policy information.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        BasePaymentPlan GetFinancialPolicyInfo(FilterBaseDTO filterBase);

        [OperationContract]
        List<ComboBaseDTO> GetPaymentsScheduleByProductId(FilterBaseDTO filterBase);

        /// <summary>
        /// Obtener el plan de pagos de un endoso especifico
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        List<FinancialPlanDTO> GetFinancialPlan(FilterBaseDTO filterBase);

        /// <summary>
        /// Gets the payments schedule by shedule identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        PaymentsScheduleDTO GetPaymentsScheduleBySheduleId(FilterBaseDTO filterBase);

        [OperationContract]
        PolicyBaseDTO GetEndorsementBaseByEndorsementId(FilterBaseDTO filterBase);

        /// <summary>
        /// Gets the payments schedule by shedule identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentDistributionDTO> GetPaymentsDistributionByFilter(FilterBaseDTO filterBase);

        /// <summary>
        /// Obtener Componentes prima iva gastos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ComponentTypeDTO> GetComponentTypes();

        /// <summary>
        /// Obtener distribucion Componentes prima iva gastos
        /// </summary>
        /// <param name="paymentScheduleId">The payment schedule identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<PaymentDistributionCompDTO> GetPaymentDistributionComponents(int paymentScheduleId);

        /// <summary>
        /// Gets the quotas by endorsement identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        QuotaAll GetQuotasByEndorsementId(FilterBaseDTO filterBase);

        /// <summary>
        /// Gets the lb sb by endorsement identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        [OperationContract]
        List<ComponentLbSb> GetLbSbByEndorsementId(FilterBaseDTO filterBase);

        /// <summary>
        /// Creates the endorsement quotas.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="sequentialId">The sequential identifier.</param>
        /// <returns></returns>
        [OperationContract]
        bool CreateEndorsementQuotas(int endorsementId, int sequentialId);
        #endregion
        #region aplicacion primas
        [OperationContract]
        PaymenQuotaDTO GetPayment(FilterBaseDTO filterBase);
        [OperationContract]
        int GetPaymentQuota(FilterBaseDTO filterBase);
        [OperationContract] 
        int GetPercentageByEndorsementId(int endorsementId);
        [OperationContract]
        PersonDataDTO GetPersonByFilter(PersonRequestDTO personRequestDTO);
        
        [OperationContract]
        PolicyDTO GetPaymentQuotaData(FilterBaseDTO filterBase);

        /// <summary>
        /// Get description for payment
        /// </summary>
        /// <param name="filterBase">filter</param>
        /// <returns>Policiy</returns>
        [OperationContract]
        PolicyDTO GetPaymentQuotaDescription(FilterBaseDTO filterBase);
        #endregion
    }

}
