using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BasePartner : Extension
    {
        /// <summary>
        /// Gets or sets the partner identifier.
        /// </summary>
        /// <value>
        /// The partner identifier.
        /// </value>
        [DataMember]
        public int PartnerId { get; set; }

        /// <summary>
        /// Gets or sets the name of the trade.
        /// </summary>
        /// <value>
        /// The name of the trade.
        /// </value>
        [DataMember]
        public string TradeName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Partner"/> is active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if active; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Active { get; set; }
        [DataMember]
        public int IndividualId { get; set; }
    }
}
