using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Event.ApplicationService.DTOs
{
    [DataContract]
    public class GenericListDTO
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
