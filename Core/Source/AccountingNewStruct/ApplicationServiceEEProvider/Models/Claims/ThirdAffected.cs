using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims
{
    [DataContract]
    public class ThirdAffected
    {
        [DataMember]
        public int ClaimCoverageId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public string FullName { get; set; }
    }
}
