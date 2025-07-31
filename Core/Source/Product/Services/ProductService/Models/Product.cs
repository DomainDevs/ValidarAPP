
namespace Sistran.Core.Application.ProductServices.Models
{
    using Sistran.Core.Application.ProductServices.Models.Base;
    using System;
    using System.Runtime.Serialization;
    using CommonModel = CommonService.Models;
    /// <summary>
    /// Modelo Para el Guardado de Productos
    /// </summary>
    /// <seealso cref="System.ICloneable" />
    [DataContract]
    public class Product : BaseProduct, ICloneable
    {
        /// <summary>
        /// Atributo para la propiedad CoveredRisk
        /// </summary> 
        [DataMember]
        public CoveredRisk CoveredRisk { get; set; }

        /// <summary>
        /// Atributo para la propiedad Ramo
        /// </summary> 
        [DataMember]
        public CommonModel.Prefix Prefix { get; set; }

        /// <summary>
        /// Crea un nuevo objeto copiado de la instancia actual.
        /// </summary>
        /// <returns>
        /// Nuevo objeto que es copia de esta instancia.
        /// </returns>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}