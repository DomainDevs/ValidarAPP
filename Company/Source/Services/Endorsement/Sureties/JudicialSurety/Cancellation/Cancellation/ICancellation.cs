using Sistran.Company.Application.CancellationEndorsement;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using JSEM = Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.JudicialSuretyCancellationService
{
    [ServiceContract]
    public interface IJudicialSuretyCancellationServiceCompany : ICiaCancellationEndorsement
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
        CompanyPolicy ExecuteThread(List<JSEM.CompanyJudgement> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);
    }
}
