using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Location.PropertyServices.Models.Base;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Location.PropertyServices.Models
{
    /// <summary>
    /// Riesgo de hogar
    /// </summary>
    [DataContract]
    public class PropertyRisk : BasePropertyRisk
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

        [DataMember]
        public List<InsuredObject> InsuredObjects { get; set; }
    }
}
