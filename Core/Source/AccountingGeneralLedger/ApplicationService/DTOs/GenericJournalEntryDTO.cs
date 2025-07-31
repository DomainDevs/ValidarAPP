using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingGeneralLedgerServices.DTOs
{
    public class GenericJournalEntryDTO
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
        public int AccountingCompanyId { get; set; }

        /// <summary>
        ///  AccountingMovementType
        /// </summary>
        [DataMember]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        ///  ModuleDate
        /// </summary>
        [DataMember]
        public int ModuleId { get; set; }

        /// <summary>
        ///  Branch
        /// </summary>
        [DataMember]
        public int BranchId { get; set; }

        /// <summary>
        ///  SalePoint
        /// </summary>
        [DataMember]
        public int SalePointId { get; set; }

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
        /// Bridge Account Identifier
        /// </summary>
        [DataMember]
        public int? BridgeAccountId { get; set; }

        /// <summary>
        /// Package Rule code
        /// </summary>
        [DataMember]
        public int? PackageRuleCode { get; set; }

        /// <summary>
        /// Accounting type
        /// </summary>
        [DataMember]
        public int AccountingTypeId { get; set; }

        /// <summary>
        /// Bank identifier
        /// </summary>
        [DataMember]
        public int BankId { get; set; }

        /// <summary>
        /// Accounting number
        /// </summary>
        [DataMember]
        public string AccountingNumber { get; set; }

        /// <summary>
        /// Accounting account id
        /// </summary>
        [DataMember]
        public int AccountingAccountId { get; set; }

        /// <summary>
        ///  JournalEntryItems
        /// </summary>
        [DataMember]
        public List<JournalEntryListParametersDTO> JournalEntryItems { get; set; }

        /// <summary>
        /// Número de recibo
        /// </summary>
        [DataMember]
        public int? RecieptNumber { get; set; }

        /// <summary>
        /// Fecha de recibo
        /// </summary>
        [DataMember]
        public DateTime? RecieptDate { get; set; }

        /// <summary>
        /// Identificador del banco de conciliación
        /// </summary>
        [DataMember]
        public int? BankReconciliationId { get; set; }

        /// <summary>
        /// Original general ledger
        /// </summary>
        [DataMember]
        public OriginalGeneralLedgerDTO OriginalGeneralLedger { get; set; }
    }
}
