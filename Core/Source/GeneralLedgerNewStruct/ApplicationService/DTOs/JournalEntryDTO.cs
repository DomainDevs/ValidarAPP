#region Using

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.DTOs
{
    /// <summary>
    ///     Asiento de Diario
    /// </summary>
    [DataContract]
    public class JournalEntryDTO
    {

        /// <summary>
        ///  Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        ///  AccountingCompany
        /// </summary>
        [DataMember]
        public AccountingCompanyDTO AccountingCompany { get; set; }

        /// <summary>
        ///  AccountingMovementType
        /// </summary>
        [DataMember]
        public AccountingMovementTypeDTO AccountingMovementType { get; set; }

        /// <summary>
        ///  ModuleDate
        /// </summary>
        [DataMember]
        public int ModuleDateId { get; set; }

        /// <summary>
        ///  Branch
        /// </summary>
        [DataMember]
        public BranchDTO Branch { get; set; }

        /// <summary>
        ///  SalePoint
        /// </summary>
        [DataMember]
        public SalePointDTO SalePoint { get; set; }

        /// <summary>
        ///     EntryNumber
        /// </summary>
        [DataMember]
        public int EntryNumber { get; set; }

        /// <summary>
        ///  TransactionNumber
        /// </summary>
        [DataMember]
        public int TechnicalTransaction { get; set; }
        
        /// <summary>
        ///     Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        ///     AccountingDate
        /// </summary>
        [DataMember]
        public DateTime AccountingDate { get; set; }

        /// <summary>
        ///     RegisterDate
        /// </summary>
        [DataMember]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        ///  Status
        /// </summary>
        [DataMember]
        public int Status { get; set; }

        /// <summary>
        ///  User
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        ///  JournalEntryItems
        /// </summary>
        [DataMember]
        public List<JournalEntryItemDTO> JournalEntryItems { get; set; }

        /// <summary>
        ///  ReconciliationMovementType
        /// </summary>
        [DataMember]
        public ReconciliationMovementTypeDTO ReconciliationMovementType { get; set; }

        /// <summary>
        ///  Receipt
        /// </summary>
        [DataMember]
        public ReceiptDTO Receipt { get; set; }
    }
}