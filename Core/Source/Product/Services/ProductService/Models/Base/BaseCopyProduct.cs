using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Modelo Copiar un producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseCopyProduct : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del Producto
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Nombre Corto del Producto
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
