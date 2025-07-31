using Sistran.Core.Application.ReversionEndorsement;
using System.ServiceModel;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.JudicialEndorsementReversionService
{
    [ServiceContract]
    public interface IJudicialReversionService : IReversionEndorsement
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        [OperationContract]

        UnderwritingServices.Models.Policy CreateEndorsementReversion(UnderwritingServices.Models.Policy policy, string userName);
    }
}
