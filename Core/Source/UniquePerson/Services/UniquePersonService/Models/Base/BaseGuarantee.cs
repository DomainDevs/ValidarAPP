using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseGuarantee : Extension
    {
        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Guarantee"/> is apostille.
        /// </summary>
        /// <value>
        ///   <c>true</c> if apostille; otherwise, <c>false</c>.
        /// </value>
		[DataMember]
        public bool Apostille { get; set; }
    }
}
