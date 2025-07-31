using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.ProductServices.Models.Base
{
    /// <summary>
    /// Formularios asociados al producto
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseProductForm : Extension
    {
        /// <summary>
        /// Atributo para la propiedad CurrentFrom.
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Atributo para la propiedad CurrentFrom.
        /// </summary>
        [DataMember]
        public string StrCurrentFrom { get; set; }

        /// <summary>
        /// Atributo para la propiedad FormNumber.
        /// </summary>
        [DataMember]
        public string FormNumber { get; set; }

        /// <summary>
        /// Atributo para la propiedad FormId.
        /// </summary>
        [DataMember]
        public int FormId { get; set; }


        /// <summary>
        /// Atributo para la propiedad CoverGroupId.
        /// </summary>
        [DataMember]
        public int CoverGroupId { get; set; }

        /// <summary>
        /// Atributo para la propiedad ProductId.
        /// </summary>
        [DataMember]
        public int ProductId { get; set; }
    }
}
