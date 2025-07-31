using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.Business;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Integration.UndewritingIntegrationServices;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Payment;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider
{
    public class UnderwritingIntegrationServiceEEProvider : IUndewritingIntegrationServices
    {
        public List<PolicyDTO> GetClaimPoliciesByPolicy(PolicyDTO policyDTO)
        {
            return DTOAssembler.CreateClaimPolicies(DelegateService.underwritingServiceCore.GetPoliciesByPolicyPersonTypeIdModuleType(ModelAssembler.CreateClaimPolicy(policyDTO), policyDTO.PersonTypeId, ModuleType.Claim));
        }

        public PolicyDTO GetClaimPolicyByEndorsementId(int endorsementId)
        {
            return DTOAssembler.CreatePolicy(DelegateService.underwritingServiceCore.GetClaimPolicyByEndorsementId(endorsementId));
        }

        public List<InsuredDTO> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            return DTOAssembler.CreateInsureds(DelegateService.underwritingServiceCore.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public List<HolderDTO> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            return DTOAssembler.CreateHolders(DelegateService.underwritingServiceCore.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType));
        }

        public List<CoverageDTO> GetCoveragesByRiskId(int riskId)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingServiceCore.GetCoveragesByRiskId(riskId));
        }

        public List<CoverageDTO> GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(int riskId, DateTime? occurrenceDate, decimal companyParticipationPercentage)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingServiceCore.GetCoveragesByRiskIdOccurrenceDateCompanyParticipationPercentage(riskId, occurrenceDate, companyParticipationPercentage));
        }

        public DeductibleDTO GetCoverageDeductibleByCoverageId(int coverageId)
        {
            return DTOAssembler.CreateDeductible(DelegateService.underwritingServiceCore.GetCoverageDeductibleByCoverageId(coverageId));
        }

        public List<CoverageDTO> GetCoveragesByLineBusinessId(int lineBusinessId)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingServiceCore.GetCoveragesByLineBusinessId(lineBusinessId));
        }

        public List<CoverageDTO> GetCoveragesByLineBusinessIdSubLineBusinessId(int lineBussinessId, int subLineBussinessId)
        {
            return DTOAssembler.CreateCoverages(DelegateService.underwritingServiceCore.GetCoveragesByLineBusinessIdSubLineBusinessId(lineBussinessId, subLineBussinessId));
        }

        public List<DeductibleDTO> GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(int policyId, int riskNum, int coverageId, int coverNum)
        {
            return DTOAssembler.CreateDeductibles(DelegateService.underwritingServiceCore.GetDeductiblesByPolicyIdRiskNumCoverageIdCoverageNumber(policyId, riskNum, coverageId, coverNum));
        }

        public List<RiskCommercialClassDTO> GetRiskCommercialClasses()
        {
            return DTOAssembler.CreateRiskCommercialClasses(DelegateService.underwritingServiceCore.GetRiskCommercialClass());
        }

        public PolicyDTO GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            return DTOAssembler.CreatePolicyByPrefixIdBranchIdPolicyNumber(DelegateService.underwritingServiceCore.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber));
        }
        public PolicyDTO GetPolicyByPolicyId(int policyId)
        {
            return DTOAssembler.GetPolicyByPolicyId(DelegateService.underwritingServiceCore.GetPolicyByPolicyId(policyId));
        }
        public List<CoInsuranceAssignedDTO> GetCoInsuranceByPolicyIdByEndorsementId(int policyId, int endorsementId)
        {
            return DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingServiceCore.GetCoInsuranceByPolicyIdByEndorsementId(policyId, endorsementId));
        }

        public List<InsuredObjectDTO> GetInsuredObjectByPrefixIdList(int prefixId)
        {
            return DTOAssembler.CreateGetInsuredObjectByPrefixId(DelegateService.underwritingServiceCore.GetInsuredObjectByPrefixIdList(prefixId));
        }

        public PayerPaymentDTO GetPayerPaymet(int EndorsementId, int Quota)
        {
            try
            {
                PolicyPaymentComponentDistributionBusiness payerPayment = new PolicyPaymentComponentDistributionBusiness();
                return payerPayment.GetPayment(EndorsementId, Quota);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public void UpdateStatusPayerPayment(PayerPaymentDTO payerPayment)
        {
            try
            {
                PolicyPaymentComponentDistributionBusiness payerPaymentbusiness = new PolicyPaymentComponentDistributionBusiness();
                payerPaymentbusiness.UpdateStatusPayerPayment(payerPayment);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
        public List<PayerPaymentComponentDTO> GetPayerPaymetComponents(int PayerPaymentId)
        {
            try
            {
                PolicyPaymentComponentDistributionBusiness payerPayment = new PolicyPaymentComponentDistributionBusiness();
                return payerPayment.GetPaymentComp(PayerPaymentId);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public List<PayerPaymentComponentLBSBDTO> GetPayerPaymetComponentsLBSB(int PayerPaymentId)
        {
            try
            {
                PolicyPaymentComponentDistributionBusiness payerPayment = new PolicyPaymentComponentDistributionBusiness();
                return payerPayment.GetPaymentCompLbsb(PayerPaymentId);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public List<PayerPaymentComponentDTO> GetPayerPaymetComponentsByEndorsementIdQuota(int EndorsementId, int Quota)
        {
            try
            {
                PolicyPaymentComponentDistributionBusiness payerPayment = new PolicyPaymentComponentDistributionBusiness();
                var payment = payerPayment.GetPayment(EndorsementId, Quota);
                return payerPayment.GetPaymentComp(payment.PayerPaymentId);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public List<RiskDTO> GetRisksByPolicyIdEndorsmentId(int policyId, int endorsementId)
        {
            try
            {
                return DTOAssembler.CreateRiskDTOs(DelegateService.underwritingServiceCore.GetRisksByPolicyIdEndorsmentId(policyId, endorsementId));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetRisksByPolicyEndorsment), ex);
            }
        }

        public RiskDTO GetRiskSuretyByRiskId(int riskId)
        {
            try
            {
                return DTOAssembler.CreateRiskSuretyDTO(DelegateService.underwritingServiceCore.GetRiskSuretyByRiskId(riskId));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetRiskSuretyByRisk), ex);
            }
        }

        public List<PremiumSearchPolicyDTO> GetPremiuPaymSearchPolicies(SearchPolicyPaymentDTO searchPolicyPaymentDTO)
        {
            try
            {
                PolicyPaymentComponentDistributionBusiness policyBusinessPayment = new PolicyPaymentComponentDistributionBusiness();
                var premiumSearchPolicyDTOs = policyBusinessPayment.GetPremiumSearchPolicies(searchPolicyPaymentDTO);
                return premiumSearchPolicyDTOs;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public List<CoverageDTO> GetCoveragesByPolicyIdEndorsementIdRiskId(int policyId, int endorsementId, int riskId)
        {
            try
            {
                return DTOAssembler.CreateCoverages(DelegateService.underwritingServiceCore.GetCoveragesByPolicyIdEndorsementIdRiskId(policyId, endorsementId, riskId));
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetCoveragesByPolicyEndorsementRisk), ex);
            }
        }

        public List<CoInsuranceAssignedDTO> GetCoInsuranceByEndorsementId(int endorsementId)
        {
            try
            {
                int policyId = DelegateService.underwritingServiceCore.GetPolicyIdByEndormestId(endorsementId);
                return DTOAssembler.CreateCoInsuranceAssigneds(DelegateService.underwritingServiceCore.GetCoInsuranceByPolicyIdByEndorsementId(policyId, endorsementId));
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        public List<IssuanceAgencyDTO> GetAgentsByPolicyIdEndorsementId(int? policyId, int? endorsementId)
        {
            try
            {
                var issAgency = DelegateService.underwritingServiceCore.GetAgentsByPolicyIdEndorsementId(policyId, endorsementId);
                if (issAgency != null && issAgency.Count > 0)
                {
                    return DTOAssembler.CreateIssuanceAgencys(issAgency, DelegateService.personService.GetAgentByIndividualId(issAgency.FirstOrDefault().Agent.IndividualId).CommissionDiscountAgreement);
                }
                else
                    return new List<IssuanceAgencyDTO>();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<EndorsementBaseDTO> GetEndorsementsByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber, int riskId = 0, bool isCurrent = false)
        {
            return DTOAssembler.CreateEndorsementBaseDTOs(DelegateService.underwritingServiceCore.GetEndorsementsByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber, riskId, isCurrent));
        }


        #region recoutificacion
        /// <summary>
        /// Gets the policies by policy filter.
        /// </summary>
        /// <param name="policyFilterDTO">The policy filter dto.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<List<decimal>> GetPoliciesByPolicyFilter(PolicyFilterDTO policyFilterDTO)
        {
            try
            {
                var result = await TP.Task.Run(() => PolicyDAO.GetPoliciesByPolicyFilter(policyFilterDTO));
                return result;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the endorsements by policy filter.
        /// </summary>
        /// <param name="policyFilterDTO">The policy filter dto.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<EndorsementBaseDTO> GetEndorsementsByPolicyFilter(PolicyFilterDTO policyFilterDTO)
        {
            try
            {
                return PolicyDAO.GetEndorsementsByPolicyFilter(policyFilterDTO);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the payers by endorsement filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<PayerBaseDTO> GetPayersByEndorsementFilter(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerDAO.GetPayersByEndorsementFilter(filterBase);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the financial policy information.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public BasePaymentPlan GetFinancialPolicyInfo(FilterBaseDTO filterBase)
        {
            try
            {

                var quotas = TP.Task.Run(() => DelegateService.accountingIntegrationService.GetQuotasByEndorsementId(filterBase.Id));
                BasePaymentPlan basePaymentPlan = new BasePaymentPlan();
                var summary = TP.Task.Run(() => PolicyDAO.GetSummaryByPolicyFilter(filterBase));
                var payerpayment = TP.Task.Run(() => PayerDAO.GetPayerPaymentByEndorsementFilter(filterBase));
                Task.WaitAll(quotas, summary, payerpayment);
                basePaymentPlan.SummaryDTO = summary.Result;
                basePaymentPlan.FinancialPlanDTO = payerpayment.Result;
                payerpayment.Result.ForEach((quota) =>
                {
                    quota.AmountPending = decimal.Round(quota.Amount - (quotas.Result.Quotas.FirstOrDefault(m => m.Id == quota.Id && m.Number == quota.Number)?.Amount ?? Decimal.Zero), QuoteManager.DecimalRound);
                });
                return basePaymentPlan;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the payments schedule by product identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        public List<ComboBaseDTO> GetPaymentsScheduleByProductId(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPaymentsScheduleByProductId(filterBase);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
        public List<FinancialPlanDTO> GetFinancialPlan(FilterBaseDTO filterBase)
        {
            try
            {
                var quotas = TP.Task.Run(() => DelegateService.accountingIntegrationService.GetQuotasByEndorsementId(filterBase.Id));
                BasePaymentPlan basePaymentPlan = new BasePaymentPlan();
                var payerpayment = TP.Task.Run(() => PayerDAO.GetPayerPaymentByEndorsementFilter(filterBase));
                Task.WaitAll(quotas, payerpayment);
                payerpayment.Result.ForEach((quota) =>
                {
                    quota.AmountPending = decimal.Round(quota.Amount - (quotas.Result.Quotas.FirstOrDefault(m => m.Id == quota.Id && m.Number == quota.Number)?.Amount ?? Decimal.Zero), QuoteManager.DecimalRound);
                });
                return payerpayment.Result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public PaymentsScheduleDTO GetPaymentsScheduleBySheduleId(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPaymentsScheduleBySheduleId(filterBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Gets the dates by endorsement identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public PolicyBaseDTO GetEndorsementBaseByEndorsementId(FilterBaseDTO filterBase)
        {
            try
            {
                return PolicyDAO.GetEndorsementBaseByEndorsementId(filterBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the payments distribution by filter.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<PaymentDistributionDTO> GetPaymentsDistributionByFilter(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPaymentsDistribution(filterBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the component types.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<ComponentTypeDTO> GetComponentTypes()
        {
            try
            {
                return PayerComponentDAO.GetComponentTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the payment distribution components.
        /// </summary>
        /// <param name="paymentScheduleId">The payment schedule identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<PaymentDistributionCompDTO> GetPaymentDistributionComponents(int paymentScheduleId)
        {
            try
            {
                return PaymentDistribution.GetPaymentDistributionComponents(paymentScheduleId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the quotas by endorsement identifier.
        /// </summary>
        /// <param name="filterBase">The filter base.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public QuotaAll GetQuotasByEndorsementId(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerDAO.GetQuotasByEndorsementId(filterBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public List<ComponentLbSb> GetLbSbByEndorsementId(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerDAO.GetLbSbByEndorsementId(filterBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Creates the endorsement quotas.
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <param name="sequentialId">The sequential identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public bool CreateEndorsementQuotas(int endorsementId, int sequentialId)
        {
            try
            {
                return PayerDAO.CreateEndorsementQuotas(endorsementId, sequentialId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion recoutificacion
        #region aplicacion primas
        public PaymenQuotaDTO GetPayment(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPayment(filterBase);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public int GetPaymentQuota(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPaymentQuota(filterBase);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Porcentaje participacion directo de la compañia
        /// </summary>
        /// <param name="endorsementId">The endorsement identifier.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public int GetPercentageByEndorsementId(int endorsementId)
        {
            try
            {
                return IntCouinsuranceDAO.GetPercentageByEndorsementId(endorsementId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public PersonDataDTO GetPersonByFilter(PersonRequestDTO personRequestDTO)
        {
            try
            {
                return PersonDAO.GetPersonByFilter(personRequestDTO);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public PolicyDTO GetPaymentQuotaData(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPaymentQuotaData(filterBase);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Errors.ErrorGetPaymentQuotaData);
            }
        }

        public PolicyDTO GetPaymentQuotaDescription(FilterBaseDTO filterBase)
        {
            try
            {
                return PayerPaymentDAO.GetPaymentQuotaDescription(filterBase);
            }
            catch (Exception)
            {
                throw new BusinessException(Resources.Errors.ErrorGetPaymentQuotaDescription);
            }
        }
        #endregion
    }
}
