using Sistran.Company.Application.ReversionEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.VehicleReversionService
{
    [ServiceContract]
    public interface IVehicleReversionService : ICiaReversionEndorsement
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="clearPolicies"></param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy, bool clearPolicies);
    }
}