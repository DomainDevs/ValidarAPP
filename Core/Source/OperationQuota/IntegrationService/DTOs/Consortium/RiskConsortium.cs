using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium
{   
    [DataContract]
    public class RiskConsortiumDTO
    {
        [DataMember]
        public int PolicyId { get; set; }
        [DataMember]
        public int EndorsementId { get; set; }
        [DataMember]
        public int RiskId { get; set; }
        [DataMember]
        public int ConsortiumId { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
        [DataMember]
        public decimal PjePart { get; set; }
    }
}
