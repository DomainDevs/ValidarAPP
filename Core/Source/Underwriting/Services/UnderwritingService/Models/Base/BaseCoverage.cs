using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    /// <summary>
    /// COberturas
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.CommonService.Models.Extension" />
    [DataContract]
    public class BaseCoverage : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Es obilgatoria?
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Seleccionada por defecto
        /// </summary>
        [DataMember]
        public bool IsSelected { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        [DataMember]
        public int Number { get; set; }

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

        /// <summary>
        /// Atributo para la propiedad Rate
        /// </summary> 
        [DataMember]
        public decimal? Rate { get; set; }

         /// <summary>
        /// Atributo para la propiedad Tasa Original
        /// </summary> 
        [DataMember]
        public decimal? OriginalRate { get; set; }

        /// <summary>
        /// Atributo para la propiedad LimitAmount
        /// </summary> 
        [DataMember]
        public decimal LimitAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad SubLimitAmount
        /// </summary> 
        [DataMember]
        public decimal SubLimitAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad PremiumAmount
        /// </summary> 
        [DataMember]
        public decimal PremiumAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad AccumulatedPremiumAmount
        /// </summary> 
        [DataMember]
        public decimal AccumulatedPremiumAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad EndorsementLimitAmount
        /// </summary> 
        [DataMember]
        public decimal EndorsementLimitAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad EndorsementSublimitAmount
        /// </summary> 
        [DataMember]
        public decimal EndorsementSublimitAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsDeclarative
        /// </summary> 
        [DataMember]
        public bool IsDeclarative { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsMinPremiumDeposit
        /// </summary> 
        [DataMember]
        public bool IsMinPremiumDeposit { get; set; }
        /// <summary>
        /// Atributo para la propiedad CoverStatusName
        /// </summary> 
        [DataMember]
        public string CoverStatusName { get; set; }

        /// <summary>
        /// Atributo para la propiedad LimitOccurrenceAmount
        /// </summary> 
        [DataMember]
        public decimal LimitOccurrenceAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad AccumulatedSubLimitAmount
        /// </summary> 
        [DataMember]
        public decimal AccumulatedSubLimitAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad MainCoverageId
        /// </summary> 
        [DataMember]
        public int? MainCoverageId { get; set; }

        /// <summary>
        /// Atributo para la propiedad IsSublimit
        /// </summary> 
        [DataMember]
        public bool IsSublimit { get; set; }

        /// <summary>
        /// Atributo para la propiedad MainCoveragePercentage
        /// </summary> 
        [DataMember]
        public decimal? MainCoveragePercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad CoverNum
        /// </summary> 
        [DataMember]
        public int CoverNum { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeclaredAmount
        /// </summary> 
        [DataMember]
        public decimal DeclaredAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad ShortTermPercentage
        /// </summary> 
        [DataMember]
        public decimal ShortTermPercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad AccumulatedDeductAmount
        /// </summary> 
        [DataMember]
        public decimal AccumulatedDeductAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad AccumulatedLimitAmount
        /// </summary> 
        [DataMember]
        public decimal AccumulatedLimitAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad ContractAmountPercentage
        /// </summary> 
        [DataMember]
        public decimal ContractAmountPercentage { get; set; }

        /// <summary>
        /// Atributo para la propiedad CurrencyCode
        /// </summary> 
        [DataMember]
        public int CurrencyCode { get; set; }

        /// <summary>
        /// Obtiene o setea El Limite Declarado del Siniestro
        /// </summary>
        /// <value>
        ///  Limite Declarado del Siniestro
        /// </value>
        [DataMember]
        public decimal LimitClaimantAmount { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador del paquete de reglas Pre
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// Obtiene o establece el Identificador del paquete de reglas Pos
        /// </summary>
        [DataMember]
        public int? PosRuleSetId { get; set; }

        /// <summary>
        /// Es visible?
        /// </summary>
        [DataMember]
        public bool IsVisible { get; set; }

        /// <summary>
        /// Obtiene o setea el porcentaje del contrato
        /// </summary>
        /// <value>
        /// Porcentaje del Contrato.
        /// </value>
        [DataMember]
        public decimal PercentageContract { get; set; }

        /// <summary>
        /// Obtiene o setea ExcessLimit
        /// </summary>
        /// <value>
        /// Porcentaje del Contrato.
        /// </value>
        [DataMember]
        public decimal ExcessLimit { get; set; }

        /// <summary>
        /// Obtiene o setea MaxLiabilityAmount
        /// </summary>
        /// <value>
        /// Responsabilidad maxima.
        /// </value>
        [DataMember]
        public decimal MaxLiabilityAmount { get; set; }

        /// <summary>
        /// Obtiene o setea el porcentage del sublimite
        /// </summary>
        /// <value>
        /// porcentaje del sublimite cobertura aliada.
        /// </value>
        [DataMember]
        public decimal? SublimitPercentage { get; set; }

        /// <summary>
        /// Porcentaje de tasa
        /// </summary>
        [DataMember]
        public decimal FlatRatePorcentage { get; set; }

        /// <summary>
        /// Dias vigencia
        /// </summary>
        [DataMember]
        public int Days { get; set; }

        /// <summary>
        /// Id cobertura asociada al riesgo
        /// </summary>
        [DataMember]
        public int RiskCoverageId { get; set; }

        /// <summary>
        /// Valor subimite original
        /// </summary>   
        [DataMember]
        public decimal OriginalSubLimitAmount { get; set; }

        /// <summary>
        /// Valor limite original
        /// </summary>   
        [DataMember]
        public decimal OriginalLimitAmount { get; set; }
        /// <summary>
        /// Obntiene o setea la diferencia de prima Minima
        /// </summary>
        /// <value>
        /// The difference minimum premium amount.
        /// </value>
        [DataMember]
        public decimal? DiffMinPremiumAmount { get; set; }

        /// <summary>
        /// Gets or sets the script identifier.
        /// </summary>
        /// <value>
        /// The script identifier.
        /// </value>
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// Indicador Cobertura Principal
        /// </summary>
        [DataMember]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Indicador para propiedad endorsementId
        /// </summary>
        [DataMember]
        public int? EndorsementId { get; set; }

        [DataMember]
        public bool IsImpression { get; set; }

        [DataMember]
        public string PrintDescription { get; set; }

        [DataMember]
        public string PrintDescriptionLimit { get; set; }

        [DataMember]
        public bool IsChild { get; set; }

        [DataMember]
        public bool IsAccMinPremium { get; set; }

        [DataMember]
        public bool IsAssistance { get; set; }

        [DataMember]
        public decimal? MinimumPremiumCoverage { get; set; }

        /// <summary>
        /// Atributo para la propiedad RateType
        /// </summary> 
        [DataMember]
        public RateType? RateType { get; set; }

        /// <summary>
        /// Atributo para la propiedad CalculationType
        /// </summary> 
        [DataMember]
        public Services.UtilitiesServices.Enums.CalculationType? CalculationType { get; set; }

        /// <summary>
        /// Atributo para la propiedad EndorsementType
        /// </summary> 
        [DataMember]
        public EndorsementType? EndorsementType { get; set; }


        /// <summary>
        /// Atributo para la propiedad CoverStatus
        /// </summary> 
        [DataMember]
        public CoverageStatusType? CoverStatus { get; set; }

        /// <summary>
        /// Atributo para la propiedad CoverageOriginalStatus
        /// </summary> 
        [DataMember]
        public CoverageStatusType? CoverageOriginalStatus { get; set; }

        /// <summary>
        /// Obtiene o setea FirstRiskType
        /// </summary>
        /// <value>
        /// Porcentaje del Contrato.
        /// </value>
        [DataMember]
        public Enums.FirstRiskType? FirstRiskType { get; set; }
		
		/// <summary>
        /// Atributo para la propiedad Original CurrentFrom
        /// </summary> 
        [DataMember]
        public DateTime CurrentFromOriginal { get; set; }
		
		/// <summary>
        /// Atributo para la propiedad Original CurrentToOriginal
        /// </summary> 
        [DataMember]
        public DateTime CurrentToOriginal { get; set; }
        /// <summary>
        /// Atributo para la propiedad Original DepositPremiumPercent
        /// </summary> 
        [DataMember]
        public decimal DepositPremiumPercent { get; set; }
		

        /// Tipo de Riesgo auto fianza ubicacion
        /// </summary>
        [DataMember]
        public int CoveredRiskType { get; set; }

        /// <summary>
        /// Seriedad de oferta
        /// </summary>
        [DataMember]
        public bool IsSeriousOffer { get; set; }

        /// <summary>
        /// Tipo de Modificacion
        /// </summary>
        [DataMember]
        public int ModificationTypeId { get; set; }
    }
}
