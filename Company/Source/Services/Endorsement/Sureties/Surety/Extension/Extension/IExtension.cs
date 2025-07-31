using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.SuretytExtensionService
{
    [ServiceContract]
    public interface ISuretyExtensionServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);

    }
}
