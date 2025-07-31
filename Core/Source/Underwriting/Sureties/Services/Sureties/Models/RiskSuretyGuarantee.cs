using Sistran.Core.Application.Sureties.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Sureties.Models
{
    [DataContract]
    public class RiskSuretyGuarantee: BaseRiskSuretyGuarantee
    {
        /// <summary>
        /// Id Riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Id contragarantia
        /// </summary>
        [DataMember]
        public int GuaranteeId { get; set; }
    }
}
