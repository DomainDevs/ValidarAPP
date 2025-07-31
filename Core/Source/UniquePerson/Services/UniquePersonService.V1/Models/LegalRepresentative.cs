using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models
{
    /// <summary>
    /// Representante Legal
    /// </summary>
    [DataContract]
    public class LegalRepresentative : BaseLegalRepresentative
    {
        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        /// <value>
        /// The city.
        /// </value>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Gets or sets the identification document.
        /// </summary>
        /// <value>
        /// The identification document.
        /// </value>
        [DataMember]
        public IdentificationDocument IdentificationDocument { get; set; }

        /// <summary>
        /// Gets or sets the authorization amount.
        /// </summary>
        /// <value>
        /// The authorization amount.
        /// </value>
        [DataMember]
        public Amount AuthorizationAmount { get; set; }

    }
}
