using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;
using ProductModel = Sistran.Core.Application.ProductServices.Models;
namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Limites de Rc Asociados al ramo producto
    /// </summary>
    [DataContract]
    public class LimitRCRelation : BaseLimitRCRelation
    {

        /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public virtual Prefix Prefix { get; set; }

      
        ///// <summary>
        ///// Producto
        ///// </summary>
        //[DataMember]
        public virtual ProductModel.Product Product { get; set; }

        /// <summary>
        /// Tipo de poliza
        /// </summary>
        [DataMember]
        public virtual PolicyType PolicyType { get; set; }

    }
}
