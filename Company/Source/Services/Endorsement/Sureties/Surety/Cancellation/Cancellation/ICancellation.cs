using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using SEM = Sistran.Company.Application.Sureties.SuretyServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.SuretyCancellationService
{
    [ServiceContract]
    public interface ISuretyCancellationServiceCia : ICiaCancellationEndorsement
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
        CompanyPolicy ExecuteThread(List<SEM.CompanyContract> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);
    }
}
