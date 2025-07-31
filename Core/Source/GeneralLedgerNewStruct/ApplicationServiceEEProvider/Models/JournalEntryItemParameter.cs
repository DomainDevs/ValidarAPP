using Sistran.Core.Application.CommonService.Models;
using System.Collections.Generic;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    public class JournalEntryItemParameter
    {

        /// <summary>
        ///  Id
        /// </summary>
        
        public int Id { get; set; }

        /// <summary>
        ///  Currency
        /// </summary>
        
        public Currency Currency { get; set; }

        /// <summary>
        ///  ExchangeRate
        /// </summary>
        
        public ExchangeRate ExchangeRate { get; set; }

        /// <summary>
        ///  AccountingAccount
        /// </summary>
        
        public AccountingAccount AccountingAccount { get; set; }

        /// <summary>
        ///  ReconciliationMovementType
        ///  
        /// </summary>
        
        public ReconciliationMovementType ReconciliationMovementType { get; set; }

        /// <summary>
        ///  Receipt
        /// </summary>
        
        public Receipt Receipt { get; set; }


        /// <summary>
        ///  AccountingNature
        /// </summary>
        
        public int AccountingNature { get; set; }

        /// <summary>
        ///     Description
        /// </summary>
        
        public string Description { get; set; }

        /// <summary>
        ///     Amount
        /// </summary>
        
        public Amount Amount { get; set; }

        /// <summary>
        ///     LocalAmount
        /// </summary>
        
        public Amount LocalAmount { get; set; }

        /// <summary>
        ///     Individual
        /// </summary>
        
        public UniquePersonService.V1.Models.Individual Individual { get; set; }

        /// <summary>
        ///     EntryType
        /// </summary>
        
        public EntryType EntryType { get; set; }

        /// <summary>
        ///     CostCenter
        /// </summary>
        
        public List<CostCenter> CostCenters { get; set; }

        /// <summary>
        ///     Analysis
        /// </summary>
        
        public List<Analysis> Analysis { get; set; }

        /// <summary>
        ///     PostDated
        /// </summary>
        
        public List<PostDated> PostDated { get; set; }

        /// <summary>
        /// SourceCode
        /// </summary>
        
        public int SourceCode { get; set; }

        /// <summary>
        ///  AccountingConcept
        /// </summary>
        
        public int AccountingConcept { get; set; }

        /// <summary>
        ///  Branch
        /// </summary>
        
        public Branch Branch { get; set; }

        /// <summary>
        ///  SalePoint
        /// </summary>
        
        public SalePoint SalePoint { get; set; }

        /// <summary>
        /// Código del paquete de reglas a buscar
        /// </summary>
        
        public string PackageRuleCodeId { get; set; }

        /// <summary>
        /// Identificador de la cuenta puente
        /// </summary>
        
        public int BridgeAccountId { get; set; }
    }
}
