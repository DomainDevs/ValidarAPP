using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// Cumulus
    /// </summary>
    [DataContract]
    public class Cumulus
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
        public List<IssueLayer> IssueLayers { get; set; }
    }
}