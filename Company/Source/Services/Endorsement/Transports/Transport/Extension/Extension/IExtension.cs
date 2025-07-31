using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;

namespace Sistran.Company.Application.TransportExtensionService
{
    [ServiceContract]
    public interface ICiaTransportsExtensionService
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);
    }
}
