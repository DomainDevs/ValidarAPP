using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models.AccountingRules;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    public class JournalParameter
    {
        public List<List<Parameter>> Parameters { get; set; }

        
        public JournalEntry JournalEntry { get; set; }

        
        public int ModuleId { get; set; }

        
        public int TypeId { get; set; }

        
        public DateTime ReceiptDate { get; set; }

        
        public string ReceiptNumber { get; set; }

        
        public int IndividualId { get; set; }

        
        public int SourceCode { get; set; }

        
        public string CodeRulePackage { get; set; }

        
        public int BridgeAccounting { get; set; }

        
        public int AccountingAccountId { get; set; }

        public Receipt Receipt { get; set; }

        public ReconciliationMovementType ReconciliationMovementType { get; set; }

        public List<int> BridgeAccounts { get; set; }
        
        public OriginalGeneralLedger OriginalGeneralLedger { get; set;  }
    }
}