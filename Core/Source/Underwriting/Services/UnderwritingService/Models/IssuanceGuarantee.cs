using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    [DataContract]
    public class IssuanceGuarantee : BaseIssuanceGuarantee
    {
        /// <summary>
        /// Tipo de Garantia
        /// </summary>
        [DataMember]
        public IssuanceGuaranteeType GuaranteeType { get; set; }
        
        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public IssuanceInsuredGuarantee InsuredGuarantee { get; set; }
    }
}
