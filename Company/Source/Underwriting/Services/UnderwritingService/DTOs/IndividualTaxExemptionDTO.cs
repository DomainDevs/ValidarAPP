using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.DTOs
{
    [DataContract]
    public class IndividualTaxExemptionDTO
    {
        [DataMember]
        public int individualId { get; set; }
        
        [DataMember]
        public int taxId { get; set; }

        [DataMember]
        public int exemptionPct { get; set; }
    }
}
