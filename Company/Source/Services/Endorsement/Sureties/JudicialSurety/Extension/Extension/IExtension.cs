using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretytExtensionService
{
    [ServiceContract]
    public interface IJudicialSuretyExtensionServiceCompany
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);

    }
}
