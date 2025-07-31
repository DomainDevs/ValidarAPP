using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.ThirdPartyLiabilityClauseService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityClauseServiceCia
    {
        /// <summary>
        /// Creacion Temporal, endoso Modificacion
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive);
        [OperationContract]
        CompanyPolicyResult CreateClauses(CompanyPolicy companyEndorsement);

    }
}
