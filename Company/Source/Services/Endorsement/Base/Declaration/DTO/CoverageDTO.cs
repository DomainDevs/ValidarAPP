using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Company.Application.Declaration.DTO
{
    /// <summary>
    /// Información de cobertura
    /// </summary>
    [DataContract]
    public class CoverageDTO
    {
        /// <summary>
        /// constructor de la clase 
        /// </summary>
        public CoverageDTO()
        {

        }

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Identificador de cobertura
        /// </summary>
        [DataMember]
        public int CoverageId { get; set; }

        /// <summary>
        /// Fecha de finalización de cobertura
        /// </summary>
        [DataMember]
        public DateTime? CurrentTo { get; set; }

        /// <summary>
        /// Fecha inicio de cobertura
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// Valor declarado
        /// </summary>
        [DataMember]
        public decimal DeclaredAmount { get; set; }

        /// <summary>
        /// Límite asewgurado
        /// </summary>
        [DataMember]
        public decimal LimitAmount { get; set; }

        /// <summary>
        /// Sublímite
        /// </summary>
        [DataMember]
        public decimal SubLimitAmount { get; set; }

        /// <summary>
        /// Tipo de tasa
        /// </summary>
        [DataMember]
        public int? RateTypeId { get; set; }

        /// <summary>
        /// Tasa
        /// </summary>
        [DataMember]
        public decimal? Rate { get; set; }

        /// <summary>
        /// Indica si es declarativa
        /// </summary>
        [DataMember]
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Prima
        /// </summary>
        [DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Identificador de deducible
        /// </summary>
        [DataMember]
        public int DeductibleId { get; set; }

        /// <summary>
        /// Porcentaje de prima de depósito
        /// </summary>
        [DataMember]
        public decimal DepositPremiumPercent { get; set; }

        /// <summary>
        /// Responsabilidad máxima
        /// </summary>
        [DataMember]
        public decimal MaxLiabilityAmount { get; set; }

        /// <summary>
        /// Límite por reclamante
        /// </summary>
        [DataMember]
        public decimal LimitClaimantAmount { get; set; }

        /// <summary>
        /// Límite por ocurrencia
        /// </summary>
        [DataMember]
        public decimal LimitOccurrenceAmount { get; set; }

        /// <summary>
        /// Tipo de cálculo
        /// </summary>
        [DataMember]
        public int? CalculationTypeId { get; set; }

        /// <summary>
        /// Descripción de la cobertura
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es obligatorio
        /// </summary>
        [DataMember]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Indica si es obligatoria por defecto
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Indica si es seleccionada por defecto
        /// </summary>
        [DataMember]
        public bool IsSelected { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int CoverStatus { get; set; }

        /// <summary>
        /// Descripcion del estado de la cobertura
        /// </summary>
        [DataMember]
        public string CoverStatusName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Number { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public SubLineBusiness SubLineBusiness { get; set; }

        /// <summary>
        /// Objeto del seguro
        /// </summary>
        [DataMember]
        public InsuredObjectDTO InsuredObject{ get; set; }

        /// <summary>
        ///  Atributo para la propiedad IsMinPremiumDeposit
        /// </summary>
        [DataMember]
        public bool IsMinPremiumDeposit { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [DataMember]
        public TextDTO Text { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [DataMember]
        public List<ClauseDTO> Clauses { get; set; }

		/// <summary>
        /// Sublimite asegurado, valor original
        /// </summary>
        [DataMember]
        public decimal OriginalSubLimitAmount { get; set; }

        /// <summary>
        /// Límite asegurado, valor original
        /// </summary>
        [DataMember]
        public decimal OriginalLimitAmount { get; set; }

        /// <summary>
        /// Fecha final de vigencia, valor original
        /// </summary>
        [DataMember]
        public DateTime CurrentToOriginal { get; set; }

        /// <summary>
        /// Fecha de inicio de vigencia, valor original
        /// </summary>
        [DataMember]
        public DateTime CurrentFromOriginal { get; set; }

        /// <summary>
        /// Id Regla Pre
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// Id Regla post
        /// </summary>
        [DataMember]
        public int? PosRuleSetId { get; set; }

    }
}