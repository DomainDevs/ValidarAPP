using Sistran.Core.Integration.GeneralLedgerServices.DTOs;
using Sistran.Core.Integration.GeneralLedgerServices.DTOs.AccountingConcepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.GeneralLedgerServices
{
    [ServiceContract]
    public interface IAccountingIntegrationService
    {
        [OperationContract]
        List<ConceptSourceDTO> GetConceptSources();

        [OperationContract]
        List<MovementTypeDTO> GetMovementTypesByConceptSourceId(int conceptSourceId);

        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId);
               
        /// <summary>
        /// Obtiene el detalle del asiento contable por la transación técnica
        /// </summary>
        /// <param name="technicalTransaction"></param>
        /// <returns></returns>
        [OperationContract]
        JournalEntryDTO GetJournalEntryDetailByTechnicalTransaction(int technicalTransaction);
    }
}
