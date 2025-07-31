using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs.AccountsPayables
{
    [DataContract]
    public class PaymentRequestTypeDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
