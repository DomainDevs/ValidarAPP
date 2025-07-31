using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;



namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class TempDailyAccountingDTO 
    {
        [DataMember]
        public int TempDailyAccountingId { get; set; }
        [DataMember]
        public int TempImputationId { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string Branch { get; set; }
        [DataMember]
        public int SalePointId { get; set; }
        [DataMember]
        public string SalePoint { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public string Company { get; set; }
        [DataMember]
        public string ConceptId { get; set; }
        [DataMember]
        public string ConceptDescription { get; set; }
        [DataMember]
        public string ConceptCodeDescription { get; set; } //campo compuesto
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public string AccountNumber { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string AccountingNumberAccount { get; set; } //campo compuesto
        [DataMember]
        public int NatureId { get; set; }
        [DataMember]
        public string Nature { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public decimal IncomeAmount { get; set; }
        [DataMember]
        public decimal Exchange { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal LocalAmount { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int BankReconciliationId { get; set; }
        [DataMember]
        public string BankReconciliation { get; set; }
        [DataMember]
        public string ReceiptNumber { get; set; }
        [DataMember]
        public string ReceiptDate { get; set; }
        [DataMember]
        public decimal PostdatedAmount { get; set; }
        [DataMember]
        public int BeneficiaryId { get; set; }
        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }
        [DataMember]
        public string BeneficiaryName { get; set; }
        [DataMember]
        public string BeneficiaryNameDocumentNumber { get; set; } //campo compuesto
        [DataMember]
        public int Rows { get; set; }
    }
}
