using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices
{
    [ServiceContract]
    public interface ICreditNoteBusinessService
    {
        /// <summary>
        /// Obtiener la lista de tipos de TIPOS DE ENDOSOS QUE TARIFAN
        /// </summary>
        /// <returns>Lista de Endosos</returns>
        [OperationContract]
        List<EndorsementType> GetEndorsmenteTypesHasQuotation();
    }
}
