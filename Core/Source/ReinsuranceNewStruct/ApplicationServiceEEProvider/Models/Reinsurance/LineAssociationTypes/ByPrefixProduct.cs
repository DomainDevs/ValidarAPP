#region Using

using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;

#endregion

namespace Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes
{
    /// <summary>
    /// Tipo de asociación de lineas por Ramo -> Producto
    /// </summary>
    [DataContract]
    public class ByPrefixProduct : LineAssociationType
    {
        /// <summary>
        /// Ramo
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }

        /// <summary>
        /// Producto
        /// </summary>
        [DataMember]
        //public Product Product { get; set; } TODO: DGUERRON No se localiza el modelo
        public string Product { get; set; }


    }
}