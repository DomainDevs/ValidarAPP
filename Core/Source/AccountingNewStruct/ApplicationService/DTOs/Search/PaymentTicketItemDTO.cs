using System;
using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class PaymentTicketItemDTO 
    {
        [DataMember]
        public int PaymentTicketItemCode { get; set; }
        [DataMember]
        public int PaymentTicketCode { get; set; }
        [DataMember]
        public int Status { get; set; }
        [DataMember]
        public int PaymentCode { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}
