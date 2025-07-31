using System.Runtime.Serialization;

namespace Sistran.Core.Integration.TempCommonService.DTOs
{
    [DataContract]
    public class IndividualDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int IndividualTypeId { get; set; }
        [DataMember]
        public string DocumentNumber { get; set; }
        [DataMember]
        public int DocumentTypeId { get; set; }
        [DataMember]
        public string Name { get; set; }
    }
}