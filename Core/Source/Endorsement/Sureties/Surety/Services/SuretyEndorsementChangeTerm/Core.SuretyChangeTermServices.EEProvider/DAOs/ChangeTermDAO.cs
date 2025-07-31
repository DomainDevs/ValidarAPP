using Sistran.Core.Application.ChangeTermEndorsement.EEProvider;
using System.Collections.Generic;

namespace Sistran.Core.Application.SuretyEndorsementChangeTermService.EEProvider.DAOs
{
    public class ChangeTermDAO
    {
        /// <summary>
        /// Emitir Traslado de Vigencia de la Póliza       
        /// </summary>
        /// <param name="Id">Temporal</param>    
        /// <returns>Numero Endoso</returns>
        public UnderwritingServices.Models.Policy Execute(int Id)
        {
            var result = DelegateService.baseEndorsementService.CreateEndorsement(Id);
            return result;
        }
    }
}
