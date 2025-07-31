using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Contragarantias Asegurado Asociadas al Ramo
    /// </summary>
    [DataContract]
    public class InsuredGuaranteePrefix : Extension
    {
        /// <summary>
        /// Gets or sets the individual identifier.
        /// </summary>
        /// <value>
        /// The individual identifier.
        /// </value>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Gets or sets the guarantee identifier.
        /// </summary>
        /// <value>
        /// The guarantee identifier.
        /// </value>
        [DataMember]
        public int GuaranteeId { get; set; }

        /// <summary>
        /// Gets or sets the prefix code.
        /// </summary>
        /// <value>
        /// The prefix code.
        /// </value>
        [DataMember]
        public int PrefixCode { get; set; }
    }
}
