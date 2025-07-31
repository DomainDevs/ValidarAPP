using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseGuarantee : BaseGeneric
    {
        /// <summary>
        /// Codigo
        /// </summary>
        [DataMember]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Guarantee"/> is apostille.
        /// </summary>
        /// <value>
        ///   <c>true</c> if apostille; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool HasApostille { get; set; }

        /// <summary>
        /// HasPromissoryNote
        /// </summary>
        [DataMember]
        public bool HasPromissoryNote { get; set; }


    }
}
