using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingClosingServices.DTOs
{
    [DataContract]
    public class ClosureTempEntryGenerationDTO
    {
        [DataMember]
        public int ProcessId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public int SalePointId { get; set; }
        [DataMember]
        public int AccountingCompanyId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int AccountingAccountId { get; set; }
        [DataMember]
        public int AccountingNature { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int AccountingMovementTypeId { get; set; }
        [DataMember]
        public int AccountingModuleId { get; set; }
        [DataMember]
        public int BankReconciliationId { get; set; }
        [DataMember]
        public int ReceiptNumber { get; set; }
        [DataMember]
        public DateTime ReceiptDate { get; set; }
        [DataMember]
        public int PaymentMovementTypeId { get; set; }
        [DataMember]
        public int EntryDestinationId { get; set; }
        [DataMember]
        public bool IsDailyEntry { get; set; }
        [DataMember]
        public int EntryNumber { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ReconciliationId { get; set; }
        [DataMember]
        public DateTime ReconciliationDate { get; set; }
        [DataMember]
        public DateTime DueDate { get; set; }

        
    }
}
