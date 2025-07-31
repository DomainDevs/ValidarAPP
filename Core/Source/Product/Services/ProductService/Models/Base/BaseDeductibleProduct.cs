using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Deducible producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseDeductibleProduct : Extension
    {
        /// <summary>
        /// Atributo para la propiedad DeductId
        /// </summary> 
        [DataMember]
        public int DeductId { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeductId
        /// </summary> 
        [DataMember]
        public int ProductId { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is selected; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsSelected { get; set; }
    }
}
