using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models
{
    /// <summary>
    /// Responsabilidad Civil - Riesgo
    /// Tpl = ThirdPartyLiability
    /// </summary>
    [DataContract]
    public class TplRisk : BaseTplRisk
    {
        [DataMember]
        public Risk Risk { get; set; }
        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public Make Make { get; set; }

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public Model Model { get; set; }

        /// <summary>
        /// Versión
        /// </summary>
        [DataMember]
        public Vehicles.Models.Version Version { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        [DataMember]
        public Vehicles.Models.Type Type { get; set; }


        /// <summary>
        /// Combustible
        /// </summary>
        [DataMember]
        public Fuel Fuel { get; set; }

        /// <summary>		
        /// Carroceria		
        /// </summary>		
        [DataMember]
        public Body Body { get; set; }

        /// <summary>
        /// Color
        /// </summary>
        [DataMember]
        public Color Color { get; set; }

        /// <summary>
        /// Deducible
        /// </summary>
        [DataMember]
        public Deductible Deductible { get; set; }


        /// <summary>
        /// Trayecto
        /// </summary>
        [DataMember]
        public Shuttle Shuttle { get; set; }

        /// <summary>
        /// Tipo de Servicio
        /// </summary>
        [DataMember]
        public ServiceType ServiceType { get; set; }
    }
}
