using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.Models.Base;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.ThirdPartyLiabilityService.Models
{
    /// <summary>
    /// 
    /// riesgo en RCP
    /// </summary>
    [DataContract]
    public class CompanyTplRisk : BaseTplRisk
    {
        public CompanyTplRisk()
        {
            Risk = new CompanyRisk();
        }

        [DataMember]
        public CompanyRisk Risk { get; set; }

        [DataMember]
        public CompanyDeductible Deductible { get; set; }

        [DataMember]
        public CompanyColor Color { get; set; }

        [DataMember]
        public CompanyBody Body { get; set; }

        [DataMember]
        public CompanyFuel Fuel { get; set; }

        [DataMember]
        public CompanyType Type { get; set; }

        [DataMember]
        public CompanyVersion Version { get; set; }

        [DataMember]
        public CompanyModel Model { get; set; }

        [DataMember]
        public CompanyMake Make { get; set; }

        [DataMember]
        public CompanyShuttle Shuttle { get; set; }

        [DataMember]
        public CompanyServiceType ServiceType { get; set; }

        [DataMember]
        public bool IsNew { get; set; }

        /// <summary>
        /// Código fasecolda
        /// </summary>
        [DataMember]
        public Fasecolda Fasecolda { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsFacultative { get; set; }

        /// <summary>
        /// Precio Estandar
        /// </summary>
        [DataMember]
        public decimal StandardPrice { get; set; }

        /// <summary>
        /// Precio Nuevo
        /// </summary>
        [DataMember]
        public decimal NewPrice { get; set; }

        [DataMember]
        public CompanyUse Use { get; set; }

        [DataMember]
        public decimal PercentageOfClaims { get; set; }

        [DataMember]
        public int TypeCargoId { get; set; }
        [DataMember]
        public string TypeCargodescription { get; set; }
        [DataMember]
        public int Tons { get; set; }
        [DataMember]
        public int TrailerQuantity { get; set; }
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        ///Capacidad de Galones 
        /// </summary>
        [DataMember]
        public int GallonTankCapacity { get; set; }

        /// <summary>
        /// Vehículo Repotenciado
        /// </summary>
        [DataMember]
        public bool RePoweredVehicle { get; set; }
        /// <summary>
        /// Año repotenciado
        /// </summary>
        [DataMember]
        public int RepoweringYear { get; set; }
        /// <summary>
        /// Año Modelo
        /// </summary>
        [DataMember]
        public int YearModel { get; set; }
        [DataMember]
        public List<string> Alerts { get; set; }
    }
}