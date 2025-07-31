using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using PEM = Sistran.Company.Application.Location.PropertyServices.Models;

namespace Sistran.Company.Application.PropertyCancellationService
{
    [ServiceContract]
    public interface IPropertyCancellationServiceCia
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
        CompanyPolicy ExecuteThread(List<PEM.CompanyPropertyRisk> risksThread, CompanyPolicy policy, List<CompanyRisk> risks);
    }
}
