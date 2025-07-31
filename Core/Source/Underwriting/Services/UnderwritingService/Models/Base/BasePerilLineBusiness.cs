using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Amparo asociado al ramo
    /// </summary>
    [DataContract]
    public class BasePerilLineBusiness : Extension
    {
        /// <summary>
        /// Gets or sets the identifier line business.
        /// </summary>
        /// <value>
        /// The identifier line business.
        /// </value>
        [DataMember]
        public int IdLineBusiness { get; set; }

        /// <summary>
        /// Gets or sets the identifier peril.
        /// </summary>
        /// <value>
        /// The identifier peril.
        /// </value>
        [DataMember]
        public int IdPeril { get; set; }
    }
}
