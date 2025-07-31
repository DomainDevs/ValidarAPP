using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    public class JournalEntryParameter
    {
        /// <summary>
        ///  Id
        /// </summary>
        
        public int Id { get; set; }

        /// <summary>
        ///  AccountingCompany
        /// </summary>
        
        public AccountingCompany AccountingCompany { get; set; }

        /// <summary>
        ///  AccountingMovementType
        /// </summary>
        
        public AccountingMovementType AccountingMovementType { get; set; }

        /// <summary>
        ///  ModuleDate
        /// </summary>
        
        public int ModuleDateId { get; set; }

        /// <summary>
        ///  Branch
        /// </summary>
        
        public Branch Branch { get; set; }

        /// <summary>
        ///  SalePoint
        /// </summary>
        
        public SalePoint SalePoint { get; set; }

        /// <summary>
        ///     EntryNumber
        /// </summary>
        
        public int EntryNumber { get; set; }

        /// <summary>
        ///  TransactionNumber
        /// </summary>
        
        public int TechnicalTransaction { get; set; }

        /// <summary>
        ///     Description
        /// </summary>
        
        public string Description { get; set; }

        /// <summary>
        ///     AccountingDate
        /// </summary>
        
        public DateTime AccountingDate { get; set; }

        /// <summary>
        ///     RegisterDate
        /// </summary>
        
        public DateTime RegisterDate { get; set; }

        /// <summary>
        ///  Status
        /// </summary>
        
        public int Status { get; set; }

        /// <summary>
        ///  User
        /// </summary>
        
        public int UserId { get; set; }

        /// <summary>
        ///  JournalEntryItems
        /// </summary>

        public List<JournalEntryItemParameter> JournalEntryItems { get; set; }

        /// <summary>
        ///  JournalEntryItems
        /// </summary>

        public List<JournalEntryItem> NewJournalEntryItems { get; set; }

        /// <summary>
        ///  ReconciliationMovementType
        /// </summary>

        public ReconciliationMovementType ReconciliationMovementType { get; set; }

        /// <summary>
        ///  Receipt
        /// </summary>
        
        public Receipt Receipt { get; set; }
    }
}
