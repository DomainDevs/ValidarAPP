using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyPrvCoverage
    {
        [DataMember]
        public int CoverageId { get; set; }

        [DataMember]
        public int CoverageNum { get; set; }

        [DataMember]
        public bool IsPost { get; set; }

        [DataMember]
        public DateTime? BeginDate { get; set; }
    }
}
