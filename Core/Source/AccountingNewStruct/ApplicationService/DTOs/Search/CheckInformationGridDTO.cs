using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CheckInformationGridDTO 
    {
        [DataMember]
        public string DatePayment { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string Holder { get; set; }
        [DataMember]
        public int ReceivingBankCode { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public int IssuingBankCode { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public string ReceivingAccountNumber { get; set; }
        [DataMember]
        public double Comission { get; set; }
        [DataMember]
        public double TaxComission { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public string RejectionDescription { get; set; }
        [DataMember]
        public int PayerId { get; set; }
        //Voucher
        [DataMember]
        public string CreditCardDescription { get; set; }
        [DataMember]
        public string Voucher { get; set; }
        [DataMember]
        public int CreditCardTypeCode { get; set; }
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }


        
    }
}
