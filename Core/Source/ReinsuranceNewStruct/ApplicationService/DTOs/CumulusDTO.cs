using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class CumulusDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// MovementId
        /// </summary>
        [DataMember]
        public int MovementId { get; set; }

        /// <summary>
        /// IssueLayers
        /// </summary>
        [DataMember]
        public List<IssueLayerDTO> IssueLayers { get; set; }
    }
}
