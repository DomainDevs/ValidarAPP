using System.Runtime.Serialization;

namespace Sistran.Core.Integration.UndewritingIntegrationServices.DTOs
{
    [DataContract]
    public class InsuredObjectDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public bool IsDeclarative { get; set; }
        [DataMember]
        public bool? IsMandatory { get; set; }
        [DataMember]
        public decimal Premium { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public bool? IsSelected { get; set; }
        [DataMember]
        public int ParametrizationStatus { get; set; }
    }
}