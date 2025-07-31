using System.Runtime.Serialization;

namespace Sistran.Core.Application.ClaimServices.DTOs.PaymentRequest
{
    [DataContract]
    public class PersonTypeDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool Enable { get; set; }

        [DataMember]
        public bool BillEnabled { get; set; }

        [DataMember]
        public bool PaymentOrderEnable { get; set; }

        [DataMember]
        public bool PreaplicationEnables { get; set; }
        [DataMember]
        public bool ChargeRequestEnable { get; set; }
        [DataMember]
        public bool PaymentRequestEnable { get; set; }
    }
}