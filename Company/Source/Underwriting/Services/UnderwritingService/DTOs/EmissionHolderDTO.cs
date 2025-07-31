using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class EmissionHolderDTO
    {
        [DataMember]
        int IndividualId { get; set; }

        [DataMember]
        string Name { get; set; }

        [DataMember]
        string Document { get; set; }

        [DataMember]
        string DocumentType { get; set; }

        [DataMember]
        string ClientType { get; set; }
    }
}
