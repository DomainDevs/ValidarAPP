using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    [DataContract]
    public class CiaDocumentTypeRange
    {
        [DataMember]
        public int IndividualTypeId { get; set; }

        [DataMember]
        public int DocumentTypeRange { get; set; }
    }
}
