using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    [DataContract]
    public class AccountingParameterDTO
    {
        [DataMember]
        public List<List<ParameterDTO>> Parameters { get; set; }

        [DataMember]
        public JournalEntryDTO JournalEntry { get; set; }

        [DataMember]
        public int ModuleId { get; set; }

        [DataMember]
        public CheckBallotAccountingParameterDTO checkBallotAccountingParameter { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public string ReceiptNumber { get; set; }

        [DataMember]
        public DateTime ReceiptDate { get; set; }

        [DataMember]
        public string CodeRulePackage { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int SourceCode { get; set; }
        
        [DataMember]
        public int BridgeAccounting { get; set; }

        [DataMember]
        public int AccountingAccountId { get; set; }

        [DataMember]
        public ReceiptDTO Receipt { get; set; }

        [DataMember]
        public ReconciliationMovementTypeDTO ReconciliationMovementType { get; set; }

        [DataMember]
        public OriginalGeneralLedgerDTO OriginalGeneralLedger { get; set; }
    }
}
