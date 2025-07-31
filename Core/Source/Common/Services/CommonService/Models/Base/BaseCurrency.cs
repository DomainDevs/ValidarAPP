using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseCurrency : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Abreviatura de la descripción
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Abreviatura 2 de la descripción
        /// </summary>
        [DataMember]
        public string TinyDescription { get; set; }
    }
}
