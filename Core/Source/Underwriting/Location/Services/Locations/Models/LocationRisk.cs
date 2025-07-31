using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Locations.Models.Base;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.Locations.Models
{
    [DataContract]
   public class LocationRisk: BaseLocation
        
    {
        public LocationRisk()
        {
            Risk = new Risk();
        }

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

        /// <summary>
        /// Tipo de construccion
        /// </summary>
        [DataMember]
        public ConstructionType ConstructionType { get; set; }
      

        /// <summary>
        /// Tipo de riesgo
        /// </summary>
        [DataMember]
        public RiskType RiskType { get; set; }

        /// <summary>
        /// Obtener o setear Uso del riesgo
        /// </summary>
        [DataMember]
        public RiskUse RiskUse { get; set; }        
      

    }
}
