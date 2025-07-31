using Sistran.Core.Application.BaseEndorsementService;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.ChangeTermEndorsement
{
    [ServiceContract]
    public interface IChangeTermEndorsement : IBaseEndorsementService
    {
        /// <summary>
        /// Tarifar Traslado de Vigencia de la Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<Risk> QuotateChangeTerm(Policy policy);

    }
}
