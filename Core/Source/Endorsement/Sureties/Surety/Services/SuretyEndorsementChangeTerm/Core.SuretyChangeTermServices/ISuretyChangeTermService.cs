using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.SuretyEndorsementChangeTermService
{
    [ServiceContract]
    public interface ISuretyChangeTermService
    {
        /// <summary>
        /// Tarifar Traslado de Vigencia de la Póliza
        /// Se genera un endoso de Cancelacion y uno de Traslado        
        /// </summary>
        /// <param name="policy">Póliza</param>       
        /// <returns>Riesgos</returns>
        [OperationContract]
        List<Risk> QuotateChangeTerm(Policy policy);

        /// <summary>
        /// Emitir Traslado de Vigencia de la Póliza       
        /// </summary>
        /// <param name="Id">Temporal</param>    
        /// <returns>Numero Endoso</returns>
        [OperationContract]
        Policy Execute(int Id);
    }
}
