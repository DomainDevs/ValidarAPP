using Sistran.Company.Application.ExtensionEndorsement;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;
using Sistran.Company.Application.Aircrafts.AircraftBusinessService.Models.Base;

namespace Sistran.Company.Application.AircraftExtensionService
{
    [ServiceContract]
    public interface ICiaAircraftsExtensionService
    {
        [OperationContract]
        CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement);
    }
}
