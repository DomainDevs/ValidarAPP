using System.Runtime.Serialization;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.Sureties.Models
{
    public class CiaRiskSuretyGuarantee : BaseGuarantee
    {
        [DataMember]
        public GuaranteeType Type { get; set; }

        [DataMember]
        public InsuredGuarantee InsuredGuarantee { get; set; }
    }
}
