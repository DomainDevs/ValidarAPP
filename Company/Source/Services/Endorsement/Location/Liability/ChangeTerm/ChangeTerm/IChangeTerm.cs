using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.LiabilityChangeTermService
{
    /// <summary>
    /// Cambio de terminos
    /// </summary>
    [ServiceContract]
    public interface ILiabilityChangeTermServiceCompany
    {
        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="clearPolicies">If validate Policies</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateChangeTerms(CompanyEndorsement companyEndorsement, bool clearPolicies = false);

        /// <summary>
        /// Creates the temporal.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false);

        /// <summary>
        /// Creates the endorsement change term.
        /// </summary>
        /// <param name="companyPolicy">The company policy.</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyPolicy> CreateEndorsementChangeTerm(CompanyEndorsement companyPolicy, bool clearPolicies = false);
    }
}
