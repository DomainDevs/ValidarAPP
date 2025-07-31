using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.FidelitytExtensionService
{
    [ServiceContract]
    public interface IFidelityExtensionServiceCia
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);

    }
}
