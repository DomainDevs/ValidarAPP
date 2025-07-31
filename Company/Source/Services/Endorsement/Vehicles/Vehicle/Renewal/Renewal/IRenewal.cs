using Sistran.Company.Application.UnderwritingServices.Models;

using System.ServiceModel;

namespace Sistran.Company.Application.VehicleEndorsementRenewalService
{
    [ServiceContract]
    public interface IVehicleRenewalService
    {
        [OperationContract]
        CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy);

    }
}
