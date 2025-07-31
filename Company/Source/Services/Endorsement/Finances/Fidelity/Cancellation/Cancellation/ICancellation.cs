using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using LEM = Sistran.Company.Application.Finances.FidelityServices.Models;

namespace Sistran.Company.Application.FidelityCancellationService
{
    [ServiceContract]
    public interface IFidelityCancellationServiceCia
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
        CompanyPolicy ExecuteThread(List<LEM.CompanyFidelityRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);
    }
}
