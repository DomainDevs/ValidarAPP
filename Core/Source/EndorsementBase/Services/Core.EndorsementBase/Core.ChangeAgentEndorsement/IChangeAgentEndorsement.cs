using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ChangeAgentEndorsement
{
    [ServiceContract]
    public interface IChangeAgentEndorsement : IBaseEndorsementService
    {
        /// <summary>
        /// Tarifar Traslado de Intermediarios de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<Risk> QuotateChangeAgent(Policy policy);

        /// <summary>
        /// Emitir Cambio de Intermediario de la Póliza
        /// </summary>
        /// <param name="Id">Temporal</param>
        /// <returns>Numero Endoso</returns>
        [OperationContract]
        Policy Execute(int Id);
    }
}
