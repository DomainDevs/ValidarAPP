using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.CancellationEndorsement
{
    [ServiceContract]
    public interface ICiaCancellationEndorsement
    {
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<CompanyRisk> QuotateCiaCancellation(CompanyPolicy policy, int cancellationFactor);
    }
}
