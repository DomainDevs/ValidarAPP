using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.Vehicles.Models
{
    /// <summary>
    /// Versión
    /// </summary>
    [DataContract]
    public class Version : BaseVersion
    {
        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public Model Model { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public Make Make { get; set; }

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
        /// propiedad IsImported
        /// </summary>
        [DataMember]
        public Engine Engine { get; set; }

        /// <summary>
        /// propiedad EngineType
        /// </summary>
        [DataMember]
        public TransmissionType TransmissionType { get; set; }

        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }


        /// <summary>
        /// Tipo de servicio
        /// </summary>
        [DataMember]
        public ServiceType ServiceType { get; set; }

        /// <summary>
        /// Carroceria
        /// </summary>
        [DataMember]
        public Body Body { get; set; }
    }
}
