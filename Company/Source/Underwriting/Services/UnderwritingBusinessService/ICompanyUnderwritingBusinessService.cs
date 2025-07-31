using Sistran.Company.Application.UnderwritingBusinessService.Model;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.ServiceModel;


namespace Sistran.Company.Application.UnderwritingBusinessService
{
    [ServiceContract]
    public interface ICompanyUnderwritingBusinessService
    {
        /// <summary>
        /// Guardar Temporal de la Poliza
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        [OperationContract]
        CompanyPolicy CompanySavePolicyTemporal(CompanyPolicy policy, bool isMasive);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPolicies(CompanyPolicy policy);

        [OperationContract]
        CompanyPolicy CompanyGetTemporalByIdTemporalType(int id, TemporalType temporalType);

        [OperationContract]
        CompanyPolicy RunRulesCompanyPolicyPre(CompanyPolicy policy);

        [OperationContract]
        List<CompanyPolicy> GetCompanyPoliciesByQuotationIdVersionPrefixId(int quotationId, int version, int prefixId, int branchId);

        [OperationContract]
        CompanyPolicy CompanyGetPolicyByTemporalId(int temporalId, bool isMasive);

        [OperationContract]
        CompanyIssuanceInsured GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        [OperationContract]
        CompanyCoverage QuotateCompanyCoverage(CompanyCoverage companyCoverage, int policyId, int riskId);

        #region Surcharges
        [OperationContract]
        List<CompanySurchargeComponent> GetCompanySurcharges();

        List<CompanySurchargeComponent> ExecuteOperationCompanySurcharges(List<CompanySurchargeComponent> surcharges);
        #endregion
    }
}
