using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseRiskPayer:Extension
    {
        /// <summary>
        /// Atributo para la propiedad TemporalId
        /// </summary>
        [DataMember]
        public int TemporalId { get; set; }

        /// <summary>
        /// Atributo para la propiedad RiskId
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Atributo para la propiedad PayerId
        /// </summary>
        [DataMember]
        public int PayerId { get; set; }

        /// <summary>
        /// Atributo para la propiedad CustomerTypeCd
        /// </summary>
        [DataMember]
        public int CustomerTypeCd { get; set; }

        /// <summary>
        /// Atributo para la propiedad PayerNum
        /// </summary>
        [DataMember]
        public int PayerNum { get; set; }

        /// <summary>
        /// Atributo para la propiedad PremiumPartPct
        /// </summary>
        [DataMember]
        public decimal PremiumPartPct { get; set; }

        /// <summary>
        /// Atributo para la propiedad EndorsementId
        /// </summary>
        [DataMember]
        public int EndorsementId { get; set; }
    }
}
