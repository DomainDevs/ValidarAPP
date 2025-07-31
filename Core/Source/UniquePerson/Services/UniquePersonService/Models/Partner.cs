using Sistran.Core.Application.UniquePersonService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    /// <summary>
    /// Asociados
    /// </summary>
    [DataContract]
    public class Partner : BasePartner
    {
        /// <summary>
        /// Gets or sets the identification document.
        /// </summary>
        /// <value>
        /// The identification document.
        /// </value>
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }

    }
}
