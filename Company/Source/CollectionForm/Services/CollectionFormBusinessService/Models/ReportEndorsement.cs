using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportEndorsement
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public List<ReportPayer> Payers { get; set; }

        [DataMember]
        public Int32 EndorsementNumber { get; set; }

        [DataMember]
        public Int32 BranchId { get; set; }

        [DataMember]
        public Int32 PrefixId { get; set; }
        
        [DataMember]
        public String DocumentNumber { get; set; }

        [DataMember]
        public List<PolicyQuote> Quotes { get; set; }

        [DataMember]
        public String FailureText { get; set; }
    }
}
