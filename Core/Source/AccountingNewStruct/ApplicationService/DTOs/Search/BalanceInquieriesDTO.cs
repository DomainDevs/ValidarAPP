using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class BalanceInquieriesDTO
    {
        [DataMember]
        public int PaymentMethodTypeCode { get; set; }
        [DataMember]
        public string PaymentDescription { get; set; }
        [DataMember]
        public int CurrencyCode { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public Decimal IncomeAmount { get; set; }
        [DataMember]
        public Decimal Amount { get; set; }
        [DataMember]
        public int BranchCode { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public string StatusDescription { get; set; }//estado del pago
        [DataMember]
        public string RegisterDate { get; set; }

        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public string UserDescription{ get; set; }

        [DataMember]
        public string AccountingDate { get; set; }

        [DataMember]
        public string OpenDate { get; set; }

    }
}
