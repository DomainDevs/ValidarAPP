using Sistran.Company.ExternalParamService.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SIstran.Company.ExternalParamService.Models
{
    [DataContract]
    public class QuoteParamIdentityCardClass
    {
        [DataMember]
        public string ProcessMessage { get; set; }

        [DataMember]
        public List<IdentityCardTypeClass> IdentityCardList { get; set; }
    }
}
