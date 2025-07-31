using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Vehicles.Models.Base;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.Models
{
    /// <summary>
    /// Versión
    /// </summary>
    [DataContract]
    public class CompanyVersion : BaseVersion
    {
        public const string isElectronicPolicy = "IsElectronicPolicy";

        /// <summary>
        /// Modelo
        /// </summary>
        [DataMember]
        public CompanyModel Model { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        [DataMember]
        public CompanyMake Make { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        [DataMember]
        public CompanyType Type { get; set; }

        /// <summary>
        /// Combustible
        /// </summary>
        [DataMember]
        public CompanyFuel Fuel { get; set; }

        /// <summary>
        /// propiedad IsImported
        /// </summary>
        [DataMember]
        public CompanyEngine Engine { get; set; }

        /// <summary>
        /// propiedad EngineType
        /// </summary>
        [DataMember]
        public CompanyTransmissionType TransmissionType { get; set; }

        /// <summary>
        /// Es importado
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }


        /// <summary>
        /// Tipo de servicio
        /// </summary>
        [DataMember]
        public CompanyServiceType ServiceType { get; set; }

        /// <summary>
        /// Carroceria
        /// </summary>
        [DataMember]
        public CompanyBody Body { get; set; }

        [DataMember]
        public bool IsElectronicPolicy
        {
            get
            {
                return GetExtendedProperty<bool>(isElectronicPolicy);
            }
            set
            {
                SetExtendedProperty(isElectronicPolicy, value);
            }
        }
    }
}
