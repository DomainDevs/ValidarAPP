using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.VehicleTextService
{
    [ServiceContract]
    public interface IVehicleTextService
    {

        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyPolicyResult CreateTexts(CompanyEndorsement companyEndorsement);

    }
}
