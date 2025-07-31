using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.DTOs
{
    [DataContract]
    public class AgencyDTO
    {
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string FullName { get; set; }
    }
}
