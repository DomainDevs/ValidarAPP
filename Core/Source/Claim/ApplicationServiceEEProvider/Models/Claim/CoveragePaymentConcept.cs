using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class CoveragePaymentConcept
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public int ConceptId { get; set; }
    }
}
