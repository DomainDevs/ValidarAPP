using Sistran.Core.Application.ReversionEndorsement;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.SuretyEndorsementReversionService
{
    [ServiceContract]
    public interface ISuretyReversionService : IReversionEndorsement
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        List<UnderwritingServices.Models.Policy> CreateTemporalEndorsementReversion(Policy policy, string userName);

        [OperationContract]
        Policy CreateEndorsementReversion(Policy policy, string userName);
    }
}
