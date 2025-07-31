using System.ServiceModel;
using Sistran.Core.Application.CancellationEndorsement;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.PropertyEndorsementCancellationService
{
    [ServiceContract]
    public interface IPropertyCancellationService : ICancellationEndorsement
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        int CreateTemporalEndorsementCancellation(Policy policy, int cancellationFactor, string userName);
    }
}
