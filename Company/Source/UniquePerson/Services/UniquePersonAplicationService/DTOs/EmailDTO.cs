using Sistran.Company.Application.CommonAplicationServices.Enums;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class EmailDTO
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
        public AplicationStaus AplicationStaus { get; set; }
        [DataMember]
        public string UpdateDate { get; set; }
    }
}
