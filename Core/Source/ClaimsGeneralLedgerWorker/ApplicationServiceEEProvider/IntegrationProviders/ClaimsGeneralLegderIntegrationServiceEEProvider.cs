using Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Business;
using Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.DTOs.AccountingRules;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs;
using Sistran.Core.Integration.ClaimsGeneralLedgerWorkerServices.DTOs.AccountingConcept;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.IntegrationProviders
{
    public class ClaimsGeneralLegderIntegrationServiceEEProvider : IClaimsGeneralLedgerWorkerIntegrationService
    {
        public void AccountingPaymentRequest(AccountingPaymentRequestDTO accountingPaymentRequest)
        {            
            int moduleId = Convert.ToInt32(Convert.ToDecimal(EnumHelper.GetEnumParameterValue<ClaimsGeneralLedgerWorkerKey>(ClaimsGeneralLedgerWorkerKey.CLM_CLAIMS_MODULE), new System.Globalization.CultureInfo("en-US")));
            ClaimsGeneralLedgerWorkerBusiness.GetAccountingAccounts(accountingPaymentRequest, moduleId);

            DelegateService.generalLedgerService.SaveJournalEntry(DTOAssembler.CreateJournalEntry(accountingPaymentRequest, moduleId));                     
        }

        public AccountingPaymentRequestDTO LoadAccountingPaymentRequest(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            int moduleId = Convert.ToInt32(Convert.ToDecimal(EnumHelper.GetEnumParameterValue<ClaimsGeneralLedgerWorkerKey>(ClaimsGeneralLedgerWorkerKey.CLM_CLAIMS_MODULE), new System.Globalization.CultureInfo("en-US")));

            ClaimsGeneralLedgerWorkerBusiness.GetAccountingAccounts(accountingPaymentRequest, moduleId);

            return accountingPaymentRequest;
        }

        public void SaveAccountingPaymentRequest(AccountingPaymentRequestDTO accountingPaymentRequest)
        {
            int moduleId = Convert.ToInt32(Convert.ToDecimal(EnumHelper.GetEnumParameterValue<ClaimsGeneralLedgerWorkerKey>(ClaimsGeneralLedgerWorkerKey.CLM_CLAIMS_MODULE), new System.Globalization.CultureInfo("en-US")));

            DelegateService.generalLedgerService.SaveJournalEntry(DTOAssembler.CreateJournalEntry(accountingPaymentRequest, moduleId));
        }

        public List<AccountingConceptDTO> GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(int branchId, int movementTypeId, int personTypeId, int individualId)
        {
            return DTOAssembler.ToDTOsInt(DelegateService.generalLedgerIntegrationService.GetAccountingConceptsByBranchIdMovementTypeIdPersonTypeIdIndividualId(branchId, movementTypeId, personTypeId, individualId)).ToList();
        }

        public List<ConceptSourceDTO> GetConceptSources()
        {
            return DTOAssembler.ToDTOsInt(DelegateService.generalLedgerIntegrationService.GetConceptSources()).ToList();
        }

        public List<MovementTypeDTO> GetMovementTypesByConceptSourceId(int conceptSourceId)
        {
            return DTOAssembler.ToDTOsInt(DelegateService.generalLedgerIntegrationService.GetMovementTypesByConceptSourceId(conceptSourceId)).ToList();
        }

        public JournalEntryDTO GetJournalEntryDetailByTechnicalTransaction(int technicalTransaction)
        {
            return DTOAssembler.ToDTOInt(DelegateService.generalLedgerIntegrationService.GetJournalEntryDetailByTechnicalTransaction(technicalTransaction));
        }
    }
}
