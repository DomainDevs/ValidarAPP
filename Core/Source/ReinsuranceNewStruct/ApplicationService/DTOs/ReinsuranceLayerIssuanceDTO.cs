using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class ReinsuranceLayerIssuanceDTO : ReinsuranceLayerDTO
    {
        /// <summary>
        /// Porcentaje de capa o de suma
        /// </summary>
        [DataMember]
        public new decimal LayerPercentage { get; set; }

        /// <summary>
        /// monto de capa o de suma
        /// </summary>
        [DataMember]
        public decimal LayerAmount { get; set; }

        /// <summary>
        /// Porcentaje de la Prima
        /// </summary>
        [DataMember]
        public new decimal PremiumPercentage { get; set; }

        /// <summary>
        /// Monto de la Prima
        /// </summary>
        [DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Líneas
        /// </summary>
        [DataMember]
        public List<ReinsuranceLineDTO> Lines { get; set; }

        /// <summary>
        /// TemporaryIssueId
        /// </summary>
        [DataMember]
        public int TemporaryIssueId { get; set; }
    }
}
