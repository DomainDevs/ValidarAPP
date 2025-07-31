using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.WrapperAccountingService.DTOs
{
    [DataContract]
    public class ApplicationRequest
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public DateTime AccountingDate { get; set; }
        [DataMember]
        public decimal ExchangeRate { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
        [DataMember]
        public short BranchId { get; set; }
        [DataMember]
        public List<RequestApplication> RequestApplication { get; set; }
        [DataMember]
        public VoucherRequest VoucherRequest { get; set; }
        [DataMember]
        public int CurrencyId { get; set; }


    }
}
