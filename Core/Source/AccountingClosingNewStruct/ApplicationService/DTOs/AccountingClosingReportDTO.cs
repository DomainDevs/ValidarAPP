using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    [DataContract]
    public class AccountingClosingReportDTO 
    {
        /// <summary>
        /// BrachCod
        /// </summary>
        [DataMember]
        public int BrachCd { get; set; }

        /// <summary>
        /// BranchDescription
        /// </summary>
        [DataMember]
        public string BranchDescription { get; set; }

        /// <summary>
        /// PrefixCod
        /// </summary>
        [DataMember]
        public int PrefixCd { get; set; }

        /// <summary>
        /// PrefixDescription
        /// </summary>
        [DataMember]
        public string PrefixDescription { get; set; }

        /// <summary>
        /// CurrencyCod
        /// </summary>
        [DataMember]
        public int CurrencyCd { get; set; }

        /// <summary>
        /// CurrencyDescription
        /// </summary>
        [DataMember]
        public string CurrencyDescription { get; set; }

        /// <summary>
        /// AccountNatureCod
        /// </summary>
        [DataMember]
        public string AccountNatureCd { get; set; }

        /// <summary>
        /// AccountNatureDescription
        /// </summary>
        [DataMember]
        public string AccountNatureDescription { get; set; }

        /// <summary>
        /// AccountingAccountCod
        /// </summary>
        [DataMember]
        public string AccountingAccountCd { get; set; }

        /// <summary>
        /// AccountingAccountDescription
        /// </summary>
        [DataMember]
        public string AccountingAccountDescription { get; set; }

        /// <summary>
        /// EntryNumber
        /// </summary>
        [DataMember]
        public int EntryNumber { get; set; }

        /// <summary>
        /// LocalAmountValue
        /// </summary>
        [DataMember]
        public decimal LocalAmountValue { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }
        
        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int PolicyId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public decimal ExchangeRate { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int EndorsementTypeId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int BusinessTypeId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int ComponentId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public string ComponentSmallDescription { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int PayerId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public string PolicyDocumentNumber { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public string EndorsementDocumentNumber { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int ContractTypeId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int CompanyTypeId { get; set; }

        /// <summary>
        /// Campo para EE
        /// </summary>
        [DataMember]
        public int ConceptId { get; set; }

        //Alejandro Villagran
        //Campos para procesos de pre-cierre.

        /// <summary>
        /// SalePointId
        /// </summary>
        [DataMember]
        public int SalePointId { get; set; }

        /// <summary>
        /// AccountingCompanyId
        /// </summary>
        [DataMember]
        public int AccountingCompanyId { get; set; }

        /// <summary>
        /// Date
        /// </summary>
        [DataMember]
        public DateTime Date { get; set; }

        /// <summary>
        /// AccountingMovementTypeId
        /// </summary>
        [DataMember]
        public int AccountingMovementTypeId { get; set; }

        /// <summary>
        /// ModuleId
        /// </summary>
        [DataMember]
        public int ModuleId { get; set; }

        /// <summary>
        /// BankReconciliationId
        /// </summary>
        [DataMember]
        public int BankReconciliationId { get; set; }

        /// <summary>
        /// ReceiptNumber
        /// </summary>
        [DataMember]
        public int ReceiptNumber { get; set; }

        /// <summary>
        /// ReceiptDate
        /// </summary>
        [DataMember]
        public DateTime? ReceiptDate { get; set; }

        /// <summary>
        /// PaymentMovementTypeId
        /// </summary>
        [DataMember]
        public int PaymentMovementTypeId { get; set; }

        /// <summary>
        /// EntryDestination
        /// </summary>
        [DataMember]
        public int EntryDestinationId { get; set; }

        /// <summary>
        /// IsDailyEntry
        /// </summary>
        [DataMember]
        public int IsDailyEntry { get; set; }

        /// <summary>
        /// UserId
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// ConciliationId
        /// </summary>
        [DataMember]
        public int ConciliationId { get; set; }

        /// <summary>
        /// ConciliationDate
        /// </summary>
        [DataMember]
        public decimal Amount { get; set; }

        /// <summary>
        /// ConciliationDate
        /// </summary>
        [DataMember]
        public DateTime? ConciliationDate { get; set; }

        /// <summary>
        /// DueDate
        /// </summary>
        [DataMember]
        public DateTime? DueDate { get; set; }
    }
}
