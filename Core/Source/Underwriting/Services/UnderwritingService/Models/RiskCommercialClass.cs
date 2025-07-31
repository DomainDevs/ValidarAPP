using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Beneficiario
    /// </summary>
    [DataContract]
    public class RiskCommercialClass
    {
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad SmallDescription.
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Atributo para la propiedad Enabled.
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Atributo para la propiedad riskCommercialClassCode.
        /// </summary>
        [DataMember]
        public int RiskCommercialClassCode { get; set; }
    }
}
