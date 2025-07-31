using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.ThirdPartyLiabilitytExtensionService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityExtensionServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);

    }
}
