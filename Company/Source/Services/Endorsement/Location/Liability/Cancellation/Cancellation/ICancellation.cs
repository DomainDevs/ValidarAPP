using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using LEM = Sistran.Company.Application.Location.LiabilityServices.Models;

namespace Sistran.Company.Application.LiabilityCancellationService
{
    [ServiceContract]
    public interface ILiabilityCancellationServiceCia
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
        CompanyPolicy ExecuteThread(List<LEM.CompanyLiabilityRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);
    }
}
