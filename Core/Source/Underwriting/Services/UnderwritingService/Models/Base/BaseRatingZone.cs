using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Zona de Tarifacion 
    /// </summary>
    [DataContract]
    public class BaseRatingZone : Extension
    {
        /// <summary>
        /// Identificador Zona de tarifacion
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion Zona de tarifacion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Minima Descripcion Zona de tarifacion
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }
    }
}
