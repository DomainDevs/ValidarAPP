using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class PaymentTypeDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
