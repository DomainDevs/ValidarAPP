using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium
{
    [DataContract]
    public class ConsortiumDTO
    {
        [DataMember]
        public int ConsotiumId { get; set; }

        [DataMember]
        public string ConsortiumName { get; set; }

        [DataMember]
        public DateTime UpdateDate { get; set; }

        [DataMember]
        public int AssociationType { get; set; }

        [DataMember]
        public string AssociationTypeDesc { get; set; }
    }
}
