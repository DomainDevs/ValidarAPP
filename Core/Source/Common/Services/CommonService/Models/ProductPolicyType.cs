using Sistran.Core.Application.CommonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models
{
    [DataContract]
    public class ProductPolicyType : BaseProductPolicyType
    {
        [DataMember]
        public Prefix Prefix { get; set; }

    }
}
