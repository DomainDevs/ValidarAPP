using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.VehicleCancellationService
{
    [ServiceContract]
    public interface ICiaVehicleCancellationService : ICiaCancellationEndorsement
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        CompanyPolicy CreateTemporalEndorsementCancellation(CompanyEndorsement companyEndorsement);

        [OperationContract]
        CompanyPolicy ExecuteThread(List<VEM.CompanyVehicle> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);

        [OperationContract]
        List<CompanyRisk> CreateVehicleCancelation(CompanyPolicy companyPolicy, List<VEM.CompanyVehicle> companyVehicles, int cancellationFactor);
    }
}
