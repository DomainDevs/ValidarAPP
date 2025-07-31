using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models
{
    /// <summary>
    /// Zona de Tarifacion 
    /// </summary>
    [DataContract]
    public class RatingZone : BaseRatingZone
    {
        /// <summary>
        /// Ramo comercial
        /// </summary>
        [DataMember]
        public Prefix Prefix { get; set; }
    }
}
