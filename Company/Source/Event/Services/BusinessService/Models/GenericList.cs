using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.BusinessService.Models
{
    [DataContract]
    public class GenericList
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
