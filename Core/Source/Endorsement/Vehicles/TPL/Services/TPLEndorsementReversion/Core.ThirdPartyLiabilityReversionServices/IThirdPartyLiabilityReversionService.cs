using Sistran.Core.Application.ReversionEndorsement;
using System.ServiceModel;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.ThirdPartyLiabilityEndorsementReversionService
{
    [ServiceContract]
    public interface IThirdPartyLiabilityReversionService : IReversionEndorsement
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        [OperationContract]
        Policy CreateEndorsementReversion(Policy policy, string userName);
    }
}
