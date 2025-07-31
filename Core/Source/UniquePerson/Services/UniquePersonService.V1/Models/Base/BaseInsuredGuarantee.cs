using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseInsuredGuarantee : BaseGeneric
    {

        /// <summary>
        /// IndividualId
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Dirección
        /// </summary>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Guarantee Guarantee { get; set; }


        //propiedades faltantes

        /// <summary>
        /// Sucursal
        /// </summary>
        [DataMember]
        public Branch Branch { get; set; }

        /// <summary>
        /// ciudad, estado , pais 
        /// </summary>
        [DataMember]
        public City City { get; set; }

        /// <summary>
        /// Moneda
        /// </summary>
        [DataMember]
        public Currency Currency { get; set; }

        /// <summary>
        /// Estado de la garantía (abierta o cerrada)
        /// </summary>
        [DataMember]
        public Boolean ClosedInd { get; set; }

        /// <summary>
        /// Fecha de registro
        /// </summary>
        [DataMember]
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Fecha de actualizacion
        /// </summary>
        [DataMember]
        public DateTime? LastChangeDate { get; set; }

        /// propiedades faltantes
        /// <summary>
        /// Estado de la garantia
        /// </summary>
        [DataMember]
        public GuaranteeStatus Status { get; set; }
        /// <summary>
        /// Valor de la contragarantía
        /// </summary>
        [DataMember]
        public decimal? GuaranteeAmount { get; set; }

        /// <summary>
        /// Código
        /// </summary>
        [DataMember]
        public int? Code { get; set; }


        /// <summary>
        /// Monto valoración
        /// </summary>
        [DataMember]
        public decimal? AppraisalAmount { get; set; }

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
        /// Abierto-Cerrado
        /// </summary>
        [DataMember]
        public bool IsCloseInd { get; set; }
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
        /// Número firmantes
        /// </summary>
        [DataMember]
        public int? SignatoriesNumber { get; set; }

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
        /// 
        /// </summary>
        [DataMember]
        public int? AssetTypeCode { get; set; }

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
        /// Fecha de constitución
        /// </summary>
        [DataMember]
        public DateTime? ConstitutionDate { get; set; }
    }
}
