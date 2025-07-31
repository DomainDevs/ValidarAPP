using Sistran.Core.Application.GeneralLedgerServices.Assemblers;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.DAOs;
using Sistran.Core.Integration.GeneralLedgerServices;
using Sistran.Core.Integration.GeneralLedgerServices.DTOs;
using Sistran.Core.Integration.GeneralLedgerServices.DTOs.AccountingConcepts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.IntegrationProviders
{
    public class AccountingIntegrationServiceEEProvider : IAccountingIntegrationService
    {
        public List<ConceptSourceDTO> GetConceptSources()
        {
            ConceptSourceDAO conceptSourceDAO = new ConceptSourceDAO();
            return DTOAssembler.ToDTOsInt(conceptSourceDAO.GetConceptSources()).ToList();
        }

        public List<MovementTypeDTO> GetMovementTypesByConceptSourceId(int conceptSourceId)
        {
            MovementTypeDAO movementTypeDAO = new MovementTypeDAO();
            return DTOAssembler.ToDTOsInt(movementTypeDAO.GetMovementTypesByConceptSourceId(conceptSourceId)).ToList();
        }

        public List<AccountingConceptDTO> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId)
        {
            AccountingConceptDAO accountingConceptDAO = new AccountingConceptDAO();
            return DTOAssembler.ToDTOsInt(accountingConceptDAO.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(branchId, movementTypeId, personTypeId, individualId)).ToList();
        }

        public JournalEntryDTO GetJournalEntryDetailByTechnicalTransaction(int technicalTransaction)
        {
            JournalEntryDAO journalEntryDAO = new JournalEntryDAO();
            return DTOAssembler.ToDTOInt(journalEntryDAO.GetJournalEntryDetailByTechnicalTransaction(technicalTransaction));
        }
    }
}
