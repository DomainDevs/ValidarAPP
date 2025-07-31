using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    [DataContract]
    public class CollectItemWithoutPaymentTicketDTO
    {

        [DataMember]
        public string User { get; set; }
        [DataMember]
        public DateTime AccountingDate { get; set; }
        [DataMember]
        public DateTime OpenDate { get; set; }
        [DataMember]
        public int BranchId { get; set; }
        [DataMember]
        public string BranchDescription { get; set; }
        [DataMember]
        public int TechnicalTransaction { get; set; }
        [DataMember]
        public string PaymentMethod { get; set; }
        [DataMember]
        public string CurrencyDescription { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public int Rows { get; set; }
    }
}
