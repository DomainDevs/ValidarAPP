#region Using

using System.Collections.Generic;
using System.Runtime.Serialization;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance
{
    /// <summary>
    /// IssueLayerLine
    /// </summary>
    [DataContract]
    public class IssueLayerLine
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
        public Line Line { get; set; }

        /// <summary>
        /// CumulusKey
        /// </summary>
        [DataMember]
        public string CumulusKey { get; set; }


        /// <summary>
        /// IssueAllocations
        /// </summary>
        [DataMember]
        public List<IssueAllocation> IssueAllocations { get; set; }
    }
}