using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentOrderDTO 
    {
        [DataMember]
        public string PaymentOrderCode { get; set; }

        [DataMember]
        public int PaymentMethodCode { get; set; }

        [DataMember]
        public string PaymentMethodName { get; set; }

        [DataMember]
        public decimal ExchangeRate { get; set; }

        [DataMember]
        public decimal IncomeAmount { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public string BranchName { get; set; }

        [DataMember]
        public int BranchPayCode { get; set; }

        [DataMember]
        public string BranchPayName { get; set; }

        [DataMember]
        public string AdmissionDate { get; set; }

        [DataMember]
        public string IndividualId { get; set; }

        [DataMember]
        public string PayerName { get; set; }

        [DataMember]
        public string BeneficiaryDocumentNumber { get; set; }

        [DataMember]
        public string BeneficiaryName { get; set; }

        [DataMember]
        public int PaymentSourceCode { get; set; }

        [DataMember]
        public string PaymentSourceName { get; set; }

        [DataMember]
        public int CompanyCode { get; set; }

        [DataMember]
        public string CompanyName { get; set; }

        [DataMember]
        public int CurrencyCode { get; set; }

        [DataMember]
        public string CurrencyName { get; set; }

        [DataMember]
        public int PersonTypeCode { get; set; }

        [DataMember]
        public string PersonTypeName { get; set; }

        [DataMember]
        public string EstimatedPaymentDate { get; set; }

        [DataMember]
        public int AccountBankCode { get; set; }

        [DataMember]
        public string BankAccountNumber { get; set; }

        [DataMember]
        public string BankName { get; set; }

        [DataMember]
        public string AccountingDate { get; set; }

        [DataMember]
        public string CancellationDate { get; set; }

        [DataMember]
        public int TempImputationCode { get; set; }

        [DataMember]
        public string CheckNumber { get; set; }
		
        [DataMember]
        public int Status { get; set; }
		
        [DataMember]
        public string StatusDescription { get; set; }
		
        [DataMember]
        public string PayTo { get; set; }

        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public int BankCode { get; set; }

        [DataMember]
        public string SectorName { get; set; }

        [DataMember]
        public string TechnicalAuthorization { get; set; }
		
        [DataMember]
        public string TransferAccount { get; set; }
		
        [DataMember]
        public int TransferNumber { get; set; }
		
        [DataMember]
        public DateTime DeliveryDate { get; set; }
		
        [DataMember]
        public int ImputationId { get; set; }
		
        [DataMember]
        public string Comments { get; set; }
		
        [DataMember]
        public int CancellationNumber { get; set; }

        [DataMember]
        public string Observation { get; set; }

        [DataMember]
        public int IndividualTypeId { get; set; }

        [DataMember]
        public string BankAccountNumberPerson { get; set; }

        [DataMember]
        public string BankNamePerson { get; set; }
    }
}
