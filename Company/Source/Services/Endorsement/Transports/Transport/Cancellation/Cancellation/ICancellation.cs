using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.TransportCancellationService
{
    [ServiceContract]
    public interface ICiaTransportCancellationService : ICiaCancellationEndorsement
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
        CompanyPolicy ExecuteThread(List<CompanyTransport> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);
    }
}
