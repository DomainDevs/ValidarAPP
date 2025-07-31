using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// Modelo de Contrato
    /// </summary>
    [DataContract]
    public class ContractDTO
    {
        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [DataMember]
        public int ContractId { get; set; }

        /// <summary>
        /// Breve descripción del contrato
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Descripción del contrato
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Año del contrato
        /// </summary>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Fecha inicio del contrato
        /// </summary>
        [DataMember]
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Fecha hasta del contrato
        /// </summary>
        [DataMember]
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Moneda del contrato
        /// </summary>
        [DataMember]
        public CurrencyDTO Currency { get; set; }

        /// <summary>
        /// Modelo del tipo de contrato
        /// </summary>
        [DataMember]
        public ContractTypeDTO ContractType { get; set; }

        /// <summary>
        /// Listado de niveles de contrato
        /// </summary>
        [DataMember]
        public List<LevelDTO> ContractLevels { get; set; }

        /// <summary>
        /// Meses de Liberación de Reservas
        /// </summary>
        [DataMember]
        public int? ReleaseTimeReserve { get; set; }

        /// <summary>
        /// Grupo de Contrato
        /// </summary>
        [DataMember]
        public string GroupContract { get; set; }

        /// <summary>
        /// Procentage CoAseguro
        /// </summary>
        [DataMember]
        public decimal CoInsurancePercentage { get; set; }

        /// <summary>
        /// Habilitado
        /// </summary>
        [DataMember]
        public bool Enabled { get; set; }

        /// <summary>
        /// Numero de Riesgos
        /// </summary>
        [DataMember]
        public int RisksNumber { get; set; }

        /// <summary>
        /// Monto de Prima
        /// </summary>
        [DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Fecha Estimada
        /// </summary>
        [DataMember]
        public DateTime EstimatedDate { get; set; }

        /// <summary>
        /// EPIType
        /// </summary>
        [DataMember]
        public EPITypeDTO EPIType { get; set; }


        /// <summary>
        /// Afectacion
        /// </summary>
        [DataMember]
        public AffectationTypeDTO AffectationType { get; set; }

        /// <summary>
        /// Restablecimiento
        /// </summary>
        [DataMember]
        public ResettlementTypeDTO ResettlementType { get; set; }

        /// <summary>
        /// Procentage de facultativo (de la cabecera)
        /// </summary>
        [DataMember]
        public decimal FacultativePercentage { get; set; }

        /// <summary>
        /// Procentage de prima facultativo (de la cabecera)
        /// </summary>
        [DataMember]
        public decimal FacultativePremiumPercentage { get; set; }

        /// <summary>
        /// Identificador único del modelo
        /// </summary>
        [DataMember]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Porcentaje
        /// </summary>
        [DataMember]
        public decimal DepositPercentageAmount { get; set; }

        /// <summary>
        /// Importe
        /// </summary>
        [DataMember]
        public decimal DepositPremiumAmount { get; set; }
    }
}