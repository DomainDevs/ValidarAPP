using System.Runtime.Serialization;


namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class SaveBillParametersDTO
    {
        [DataMember]
        public CollectDTO Collect { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public int TechnicalTransaction { get; set; }

        [DataMember]
        public int PaymentCode { get; set; }

        [DataMember]
        public int BridgeAccoutingId { get; set; }

        [DataMember]
        public string BridgePackageCode { get; set; }
    }
}
