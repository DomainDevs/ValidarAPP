using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class IssueLayerDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// LayerNumber
        /// </summary>
        [DataMember]
        public int LayerNumber { get; set; }

        /// <summary>
        /// LayerPercentage
        /// </summary>
        [DataMember]
        public decimal LayerPercentage { get; set; }

        /// <summary>
        /// PremiumPercentage
        /// </summary>
        [DataMember]
        public decimal PremiumPercentage { get; set; }

        /// <summary>
        /// IssueLayerLine
        /// </summary>
        [DataMember]
        public List<IssueLayerLineDTO> IssueLayerLines { get; set; }
    }
}
