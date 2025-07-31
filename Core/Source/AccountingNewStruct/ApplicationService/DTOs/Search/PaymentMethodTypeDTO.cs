using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.Search
{
    public class PaymentMethodTypeDTO 
    {
        [DataMember]
        public int PaymentTypeCode { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? EnabledTicket { get; set; }
        [DataMember]
        public int? CollectEnabled { get; set; }
        [DataMember]
        public int? EnabledPaymentOrder { get; set; }
        [DataMember]
        public int? EnabledPaymentRequest { get; set; }
    }
}
