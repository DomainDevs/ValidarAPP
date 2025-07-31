#region Using

using Sistran.Core.Application.CommonService.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models
{
    /// <summary>
    ///     Asiento de Diario
    /// </summary>
    [DataContract]
    public class JournalEntry
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
        public AccountingCompany AccountingCompany { get; set; }

        /// <summary>
        ///  AccountingMovementType
        /// </summary>
        [DataMember]
        public AccountingMovementType AccountingMovementType { get; set; }

        /// <summary>
        ///  ModuleDate
        /// </summary>
        [DataMember]
        public int ModuleDateId { get; set; }

        /// <summary>
        ///  Branch
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        ///  SalePoint
        /// </summary>
        [DataMember]
        public SalePoint SalePoint { get; set; }

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
        public List<JournalEntryItem> JournalEntryItems { get; set; }
    }
}