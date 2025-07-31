using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo -> Producto
    /// </summary>
    [DataContract]
    public class ByPrefixProductDTO : LineAssociationTypeDTO
    {
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public PrefixDTO Prefix { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        //public Product Product { get; set; } TODO: DGUERRON No se localiza el modelo
        public List<ProductDTO> Products { get; set; }
    }
}