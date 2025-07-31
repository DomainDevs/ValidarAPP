using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs.AccountingConcept;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices
{
    [ServiceContract]
    public interface IClaimsGeneralLedgerWorkerIntegrationService
    {
        [OperationContract]
        void AccountingPaymentRequest(AccountingPaymentRequestDTO accountingPaymentRequest);

        [OperationContract]
        AccountingPaymentRequestDTO LoadAccountingPaymentRequest(AccountingPaymentRequestDTO accountingPaymentRequest);

        [OperationContract]
        void SaveAccountingPaymentRequest(AccountingPaymentRequestDTO accountingPaymentRequest);

        [OperationContract]
        List<ConceptSourceDTO> GetConceptSources();

        [OperationContract]
        List<MovementTypeDTO> GetMovementTypesByConceptSourceId(int conceptSourceId);

        [OperationContract]
        List<AccountingConceptDTO> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId);

        [OperationContract]
        JournalEntryDTO GetJournalEntryDetailByTechnicalTransaction(int technicalTransaction);
    }
}
