using System.Runtime.Serialization;

namespace Sistran.Company.Application.ExternalProxyServices.Models
{
    [DataContract]
    public class ResponseEmail
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public int EmailTypeId { get; set; }
        [DataMember]
        public bool IsPrincipal { get; set; }
        [DataMember]
        public string UpdateDate { get; set; }
    }
}
