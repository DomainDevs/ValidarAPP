using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    /// <summary>
    /// Auto - Riesgo
    /// </summary>
    [DataContract]
    public class CompanyVehicle : Sistran.Core.Application.Vehicles.VehicleServices.Models.Base.BaseVehicle
    {
        public CompanyVehicle()
        {
            Risk = new CompanyRisk();
        }


        [DataMember]
        public CompanyRisk Risk { get; set; }

        [DataMember]
        public GoodExperienceYear GoodExperienceYear { get; set; }

        /// <summary>
        /// Código fasecolda
        /// </summary>
        [DataMember]
        public Fasecolda Fasecolda { get; set; }

        /// <summary>
        /// Tipo de Servicio
        /// </summary>
        [DataMember]
        public CompanyServiceType ServiceType { get; set; }

        /// <summary>
        /// Tipo de Tasa
        /// </summary>
        [DataMember]
        public int? RateType { get; set; }

        /// <summary>
        /// Fecha Actual de Movimiento
        /// </summary>
        [DataMember]
        public DateTime ActualDateMovement { get; set; }

        /// <summary>
        /// Fecha Consulta Multas
        /// </summary>
        [DataMember]
        public DateTime? FineDate { get; set; }

        /// <summary>
        /// Consultó Multas?
        /// </summary>
        [DataMember]
        public bool? IsFine { get; set; }

        /// <summary>
        /// Reserva Id Pv 2G
        /// </summary>
        [DataMember]
        public int? Policy2g { get; set; }

        /// <summary>
        /// Id Grupo de Consulta Multas
        /// </summary>
        [DataMember]
        public int? GroupFineId { get; set; }

        /// <summary>
        /// Nuevo / Renovado
        /// </summary>
        [DataMember]
        public int? NewRenovated { get; set; }

        /// <summary>
        /// Nro Renovaciones
        /// </summary>
        [DataMember]
        public int? RenewallNum { get; set; }

        [DataMember]
        /// <summary>
        /// Historial de renovación.
        /// </summary>
        public RenewallHistory RenewallHistory { get; set; }

          /// <summary>
        /// Fecha Nacimiento Hijo Mayor
        /// </summary>
        [DataMember]
        public DateTime? BirthDateEldestson { get; set; }

       
        /// <summary>
        /// Color
        /// </summary>
        [DataMember]
        public CompanyColor Color { get; set; }

        /// <summary>
        /// Uso
        /// </summary>
        [DataMember]
        public CompanyUse Use { get; set; }

        /// <summary>
        /// Accesorios
        /// </summary>
        [DataMember]
        public List<CompanyAccessory> Accesories { get; set; }

        /// <summary>
        /// Versión
        /// </summary>
        [DataMember]
        public CompanyVersion Version { get; set; }

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
        /// Multas de Transito
        /// </summary>
        [DataMember]
        public CompanyInfringementSimit InfringementSimit { get; set; }


        /// <summary>
        /// Inspección
        /// </summary>
        [DataMember]
        public CompanyInspection Inspection { get; set; }

        [DataMember]
        public bool CalculateMinPremium { get; set; }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return MemberwiseClone();
        }


        /// <summary>
        /// SinisterQuantity cantidad de siniestros - entity general
        /// </summary>
        [DataMember]
        public int SinisterQuantity { get; set; }

        /// <summary>
        /// Alerts: para las cotizaciones que se pueden guardar con advertencias
        /// </summary>
        [DataMember]
        public List<string> Alerts { get; set; }
    }
}