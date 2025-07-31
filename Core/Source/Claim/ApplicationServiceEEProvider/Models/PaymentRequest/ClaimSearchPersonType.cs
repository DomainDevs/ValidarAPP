
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest
{
    [DataContract]
    public class ClaimSearchPersonType
    {
        [DataMember]
        public int PersonTypeId { get; set; }
        [DataMember]
        public int PrefixId { get; set; }

        [DataMember]
        public int SearchType { get; set; }

        [DataMember]
        public List<ClaimPersonType> ClaimPersonType { get; set; }
    }
}