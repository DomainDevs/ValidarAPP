using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UniqueUserServices.DTOs
{
    public class IndividualRelationAppDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public int RelationTypeId { get; set; }
    }
}
