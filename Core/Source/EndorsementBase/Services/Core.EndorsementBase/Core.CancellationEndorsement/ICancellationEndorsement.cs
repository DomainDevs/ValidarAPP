using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.CancellationEndorsement
{
    [ServiceContract]
    public interface ICancellationEndorsement
    {
        /// <summary>
        /// Tarifar Cancelación de Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<Risk> QuotateCancellation(Policy policy, int cancellationFactor);
    }
}
