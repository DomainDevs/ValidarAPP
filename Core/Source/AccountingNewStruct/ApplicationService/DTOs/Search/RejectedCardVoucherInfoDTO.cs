using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    public class RejectedCardVoucherInfoDTO : RejectedPaymentDTO
    {
        [DataMember]
        public int BranchCode { get; set; }

        [DataMember]
        public DateTime CardDate { get; set; }

        [DataMember]
        public string CardDescription { get; set; }

        [DataMember]
        public int CreditCardTypeCode { get; set; }

        [DataMember]
        public string Description { get; set; } //BranchDescription
        
        [DataMember]
        public double Tax { get; set; }

        [DataMember]
        public double TaxBase { get; set; }

        [DataMember]
        public string Voucher { get; set; }
    }
}
