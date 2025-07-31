using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseIssuanceInsuredGuarantee : Extension
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Valor de la contragarantía
        /// </summary>
        [DataMember]
        public decimal? GuaranteeAmount { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Monto
        /// </summary>
        [DataMember]
        public decimal? ValueAmount { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Abierto-Cerrado
        /// </summary>
        [DataMember]
        public bool IsCloseInd { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int? Code { get; set; }

        /// <summary>
        /// Fecha de avalúo
        /// </summary>
        [DataMember]
        public DateTime? AppraisalDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ExpertName { get; set; }

        /// <summary>
        /// Valor asegurado
        /// </summary>
        [DataMember]
        public decimal? InsuranceAmount { get; set; }

        /// <summary>
        /// Número de póliza
        /// </summary>
        [DataMember]
        public string PolicyNumber { get; set; }

        /// <summary>
        /// Nombre emisor
        /// </summary>
        [DataMember]
        public string IssuerName { get; set; }

        /// <summary>
        /// Número de documento
        /// </summary>
        [DataMember]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Fecha de vencimiento
        /// </summary>
        [DataMember]
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Código línea de negocios
        /// </summary>
        [DataMember]
        public int? BusinessLineCode { get; set; }

        /// <summary>
        /// Número de registro
        /// </summary>
        [DataMember]
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Placa
        /// </summary>
        [DataMember]
        public string LicensePlate { get; set; }

        /// <summary>
        /// Motor
        /// </summary>
        [DataMember]
        public string EngineNro { get; set; }

        /// <summary>
        /// Chasis
        /// </summary>
        [DataMember]
        public string ChassisNro { get; set; }

        /// <summary>
        /// Marca del vehículo
        /// </summary>
        [DataMember]
        public short? VehicleMake { get; set; }

        /// <summary>
        /// Modelo del vehículo
        /// </summary>
        [DataMember]
        public int? VehicleModel { get; set; }

        /// <summary>
        /// Versión del vehículo
        /// </summary>
        [DataMember]
        public short? VehicleVersion { get; set; }

        /// <summary>
        /// Área construida
        /// </summary>
        [DataMember]
        public decimal? BuiltArea { get; set; }

        /// <summary>
        /// Número de escritura
        /// </summary>
        [DataMember]
        public string DeedNumber { get; set; }

        /// <summary>
        /// Monto valoración
        /// </summary>
        [DataMember]
        public decimal? AppraisalAmount { get; set; }

        /// <summary>
        /// Descripción - Otros
        /// </summary>
        [DataMember]
        public string DescriptionOthers { get; set; }

        /// <summary>
        /// Valor del documento
        /// </summary>
        [DataMember]
        public decimal? DocumentValueAmount { get; set; }

        /// <summary>
        /// Área medida
        /// </summary>
        [DataMember]
        public decimal? MeasureArea { get; set; }

        /// <summary>
        /// Fecha de constitución
        /// </summary>
        [DataMember]
        public DateTime? ConstitutionDate { get; set; }

        /// <summary>
        /// Nombre de la hipoteca
        /// </summary>
        [DataMember]
        public string MortgagerName { get; set; }

        /// <summary>
        /// Entidad
        /// </summary>
        [DataMember]
        public string DepositEntity { get; set; }

        /// <summary>
        /// Fecha de deposito
        /// </summary>
        [DataMember]
        public DateTime? DepositDate { get; set; }

        /// <summary>
        /// Depositor
        /// </summary>
        [DataMember]
        public string Depositor { get; set; }

        /// <summary>
        /// Constitución
        /// </summary>
        [DataMember]
        public string Constituent { get; set; }

        /// <summary>
        /// Número firmantes
        /// </summary>
        [DataMember]
        public int? SignatoriesNumber { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? AssetTypeCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string RealstateMatriculation { get; set; }

        /// <summary>
        /// Fecha de último cambio
        /// </summary>
        [DataMember]
        public DateTime? LastChangeDate { get; set; }

        /// <summary>
        /// Apostilla
        /// </summary>
        [DataMember]
        public bool Apostille { get; set; }
    }
}
