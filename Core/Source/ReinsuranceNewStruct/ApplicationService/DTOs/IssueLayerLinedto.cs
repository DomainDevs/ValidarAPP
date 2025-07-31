using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    [DataContract]
    public class IssueLayerLineDTO
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
        public LineDTO Line { get; set; }

        /// <summary>
        /// CumulusKey
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }


        /// <summary>
        /// IssueAllocations
        /// </summary>
        [DataMember]
        public List<IssueAllocationDTO> IssueAllocations { get; set; }
    }
}
