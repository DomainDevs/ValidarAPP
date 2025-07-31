using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// Planes Tecnicos
    /// </summary>
    [DataContract]
    public class BaseTechnicalPlan:Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int TechnicalPlanId { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Descripcion Corta
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Codigo Tipo de Riesgo Cubierto
        /// </summary>
        [DataMember]
        public int CoveredRiskTypeCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad CurrentFrom
        /// </summary> 
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Atributo para la propiedad CurrentTo
        /// </summary> 
        [DataMember]
        public DateTime? CurrentTo { get; set; }
    }
}
