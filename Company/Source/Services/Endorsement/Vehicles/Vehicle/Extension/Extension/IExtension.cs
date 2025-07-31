using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.VehicletExtensionService
{
    [ServiceContract]
    public interface ICiaVehicleExtensionService
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);

    }
}
