using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;

namespace Sistran.Company.Application.MarineExtensionService
{
    [ServiceContract]
    public interface ICiaMarinesExtensionService
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);
    }
}
