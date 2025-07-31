// -----------------------------------------------------------------------
// <copyright file="CiaProductServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models.Product
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// Modelo de servicio company product
    /// </summary>
    [DataContract]
    public class CiaParamProductServiceModel : ParametricServiceModel
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? SurchargeCommissionPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? AdditDisCommissPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? StandardCommissionPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? StdDiscountCommPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? AdditionalCommissionPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsRequest { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsFlatRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsCollective { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? DecrementCommisionAdjustFactorPercentage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsGreen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsUse { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? CurrentTo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime CurrentFrom { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? PreRuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? RuleSetId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int? ScriptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool CalculateMinPremium { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public decimal? IncrementCommisionAdjustFactorPercentage { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsPolitical
        /// </summary>
        //[DataMember]
        //public bool IsPolitical { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IncentiveAmount
        /// </summary>
        //[DataMember]
        //public decimal IncentiveAmount { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si Habilitar Incentivo esta activo
        /// </summary>
        //[DataMember]
        //public bool IsEnabled { get; set; }

        ///// <summary>
        ///// Obtiene o establece la propiedad IsScore indica DataCrédito
        ///// </summary>
        //[DataMember]
        //public bool? IsScore { get; set; }

        ///// <summary>
        ///// Obtiene o establece la propiedad IsFine indica Multas de Transito
        ///// </summary>
        //[DataMember]
        //public bool? IsFine { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsFasecolda 
        /// </summary>
        //[DataMember]
        //public bool? IsFasecolda { get; set; }

        /// <summary>
        /// Obtiene o establece los dias de vigencia de la cotizacion
        /// </summary>
        //[DataMember]
        //public int? ValidDaysTempQuote { get; set; }

        /// <summary>
        /// Obtiene o establece los dias de vigencia de la temporal
        /// </summary>
        //[DataMember]
        //public int? ValidDaysTempPolicy { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsRcAdditional 
        /// </summary>
        //[DataMember]
        //public bool? IsRcAdditional { get; set; }

        /// <summary>
        /// Obtiene o establece el ramo Asociado
        /// </summary>
        [DataMember]
        public CiaParamPrefixServiceModel Prefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamCurrencyServiceModel> Currency { get; set; }

        /// <summary>
        /// Obtiene o establece el Producto 2G Asociado
        /// </summary>
        [DataMember]
        public CiaParamProduct2GServiceModel Product2G { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de póliza Asociadas
        /// </summary>
        [DataMember]
        public List<CiaParamPolicyTypeServiceModel> PolicyType { get; set; }

        ///// <summary>
        ///// Obtiene o establece los tipos de asistencia Asociadas
        ///// </summary>
        //[DataMember]
        //public List<CiaParamAssistanceTypeServiceModel> AssistanceType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamRiskTypeServiceModel> RiskTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public List<CiaParamFinancialPlanServiceModel> FinancialPlan { get; set; }

        /// <summary>
        /// Atributo para la propiedad Recargo Comision
        /// </summary> 
        [DataMember]
        public decimal? AdditCommissPercentage { get; set; }

        /// <summary>
        /// Marca para proceso interactivo
        /// </summary>
        [DataMember]
        public bool IsInteractive { get; set; }

        /// <summary>
        /// Marca para proceso Masivo
        /// </summary>
        [DataMember]
        public bool IsMassive { get; set; }
    }
}
