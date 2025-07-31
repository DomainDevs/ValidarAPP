using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim
{
    [DataContract]
    public class Policy
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int BranchId { get; set; }

        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public string DocumentNumber { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public DateTime CurrentTo { get; set; }

        [DataMember]
        public int TypeId { get; set; }

        [DataMember]
        public int BusinessTypeId { get; set; }

        [DataMember]
        public int ProductId { get; set; }
    }
}
