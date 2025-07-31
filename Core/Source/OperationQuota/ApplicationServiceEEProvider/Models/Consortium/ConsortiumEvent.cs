using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium
{
    [DataContract]
    public class ConsortiumEvent
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
        public bool IsConsortium { get; set; }

        [DataMember]
        public DeclineInsured declineInsured { get; set; }

        [DataMember]
        public Consortium consortium { get; set; }

        [DataMember]
        public Consortiumpartners Consortiumpartners { get; set; }
    }
}
