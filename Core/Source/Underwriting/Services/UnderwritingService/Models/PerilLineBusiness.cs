
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Amparo asociado al ramo
    /// </summary>
    [DataContract]
    public class PerilLineBusiness : BasePerilLineBusiness
    {
        /// <summary>
        /// Gets or sets the peril not assign.
        /// </summary>
        /// <value>
        /// The peril not assign.
        /// </value>
        [DataMember]
        public List<Peril> PerilNotAssign { get; set; }

        /// <summary>
        /// Gets or sets the peril assign.
        /// </summary>
        /// <value>
        /// The peril assign.
        /// </value>
        [DataMember]
        public List<Peril> PerilAssign { get; set; }

    }
}
