using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium
{
    [DataContract]
    public class Consortium
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
