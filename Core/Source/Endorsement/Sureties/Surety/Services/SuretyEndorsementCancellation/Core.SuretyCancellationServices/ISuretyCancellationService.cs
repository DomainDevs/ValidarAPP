using System.ServiceModel;
using Sistran.Core.Application.CancellationEndorsement;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.SuretyEndorsementCancellationService
{
    [ServiceContract]
    public interface ISuretyCancellationService : ICancellationEndorsement
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        int CreateTemporalEndorsementCancellation(Policy policy, int cancellationFactor, string userName);

        /// <summary>
        /// Tarifar Cancelación de Póliza Cumplimiento
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="cancellationFactor">Factor de Cancelación</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<Risk> QuotateCancellationSurety(Policy policy, int cancellationFactor);
    }
}
