using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectionFormBusinessService.Models
{
    [DataContract]
    public class ReportPolicy
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int EndorsementId { get; set; }

        [DataMember]
        public int DocumentNumber { get; set; }

        [DataMember]
        public int userId { get; set; }

        [DataMember]
        public List<ReportEndorsement> Endorsements { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string Textbody { get; set; }
    }
}
