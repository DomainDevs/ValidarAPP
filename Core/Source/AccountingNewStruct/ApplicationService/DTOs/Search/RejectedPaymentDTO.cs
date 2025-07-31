using Sistran.Core.Application.AccountingServices.DTOs.Payments;
using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class RejectedPaymentDTO
    {
        [DataMember]
        public int RejectedPaymentCode { get; set; }
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string IssuingAccountNumber { get; set; }
        [DataMember]
        public string BankDescription { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public string DatePayment { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int CollectCode { get; set; }
        [DataMember]
        public int PayerId { get; set; } //pagador
        [DataMember]
        public string Name { get; set; } //nombre pagador
        [DataMember]
        public string RejectionDate { get; set; }
        [DataMember]
        public string RejectionDescription { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public int CollectConceptCode { get; set; }

        //PARA INFORMACION DE CONSULTA DE CHEQUES
        [DataMember]
        public double Comission { get; set; }
        [DataMember]
        public double TaxComission { get; set; }
    }
}
