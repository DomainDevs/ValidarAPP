using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    [DataContract]
    public class AccountBank : BaseAccountBank
    {
        [DataMember]
        public Individual Individual { get; set; }
        [DataMember]
        public AccountType AccountType { get; set; }
        [DataMember]
        public Bank Bank { get; set; }
        [DataMember]
        public Currency Currency { get; set; }
        [DataMember]
        public Branch Branch { get; set; }

    }
}
