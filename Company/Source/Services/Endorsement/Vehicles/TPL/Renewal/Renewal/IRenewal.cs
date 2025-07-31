using Sistran.Company.Application.UnderwritingServices.Models;

using System.ServiceModel;

namespace Sistran.Company.Application.ThirdPartyLiabilityEndorsementRenewalService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityRenewalServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);

    }
}
