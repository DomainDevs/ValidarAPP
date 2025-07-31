using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Integration.UnderwritingOperatingQuotaServices.DTOs.Consortium
{
    [DataContract]
    public class ConsortiumEventDTO
    {
        [DataMember]
        public int ConsortiumEventID { get; set; }

        [DataMember]
        public int ConsortiumEventEventType { get; set; }

        [DataMember]
        public int IndividualConsortiumID { get; set; }

        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public DateTime IssueDate { get; set; }

        [DataMember]
        public string payload { get; set; }

        [DataMember]
        public decimal OperationQuotaConsortium { get; set; }

        [DataMember]
        public ConsortiumDTO consortiumDTO { get; set; }

        [DataMember]
        public ConsortiumpartnersDTO ConsortiumpartnersDTO { get; set; }
    }
}
