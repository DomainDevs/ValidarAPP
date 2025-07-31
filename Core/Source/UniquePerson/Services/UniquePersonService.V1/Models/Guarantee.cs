using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Garantias
    /// </summary>
    [DataContract]
    public class Guarantee : BaseGuarantee
    {
        /// <summary>
        /// Tipo de Garantia
        /// </summary>
        [DataMember]
        public GuaranteeType Type { get; set; }

        /// <summary>
        /// Asegurado
        /// </summary>
        [DataMember]
        public InsuredGuarantee InsuredGuarantee { get; set; }
    }
}
