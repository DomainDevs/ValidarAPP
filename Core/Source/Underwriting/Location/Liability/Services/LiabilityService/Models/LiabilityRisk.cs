using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.LiabilityServices.Models.Base;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Location.LiabilityServices.Models
{
    /// <summary>
    /// Riesgo RC
    /// </summary>
    [DataContract]
    public class LiabilityRisk : BaseLiabilityRisk
    {
        [DataMember]
        public Risk Risk { get; set; }

        /// <summary>
        /// Obtener o setear Tipo de nomenclatura
        /// </summary>
        [DataMember]
        public NomenclatureAddress NomenclatureAddress { get; set; }

        /// <summary>
        /// Obtener o setear Ciudad
        /// </summary>
        [DataMember]
        public City City { get; set; }
    }
}
