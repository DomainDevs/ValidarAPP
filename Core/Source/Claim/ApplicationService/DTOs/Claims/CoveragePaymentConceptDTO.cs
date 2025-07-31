using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.DTOs.Claims
{
    [DataContract]
    public class CoveragePaymentConceptDTO
    {
        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int EstimationTypeId { get; set; }

        [DataMember]
        public int ConceptId { get; set; }
    }
}
