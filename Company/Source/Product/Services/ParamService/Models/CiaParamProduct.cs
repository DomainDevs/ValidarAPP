// -----------------------------------------------------------------------
// <copyright file="ParamProduct.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ProductParamService.Models
{
    using Sistran.Core.Application.ProductParamService.Models.Base;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo company de producto.
    /// </summary>
    [DataContract]
    public class CiaParamProduct: BaseParamProduct
    {
        /// <summary>
        /// Obtiene o establece la propiedad IsPolitical
        /// </summary>
        public bool IsPolitical { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IncentiveAmount
        /// </summary>
        public decimal IncentiveAmount { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si Habilitar Incentivo esta activo
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsScore indica DataCrédito
        /// </summary>
        public bool? IsScore { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsFine indica Multas de Transito
        /// </summary>
        public bool? IsFine { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsFasecolda 
        /// </summary>        
        public bool? IsFasecolda { get; set; }

        /// <summary>
        /// Obtiene o establece los dias de vigencia de la cotizacion
        /// </summary>
        public int? ValidDaysTempQuote { get; set; }

        /// <summary>
        /// Obtiene o establece los dias de vigencia de la temporal
        /// </summary>
        public int? ValidDaysTempPolicy { get; set; }

        /// <summary>
        /// Obtiene o establece la propiedad IsRcAdditional 
        /// </summary>        
        public bool? IsRcAdditional { get; set; }



        /// <summary>
        /// Obtiene o establece el ramo Asociado
        /// </summary>
        public CiaParamPrefix Prefix { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamCurrency> Currency { get; set; }

        /// <summary>
        /// Obtiene o establece el Producto 2G Asociado
        /// </summary>
        public CiaParamProduct2G Product2G { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de póliza Asociadas
        /// </summary>
        public List<CiaParamPolicyType> PolicyType { get; set; }

        /// <summary>
        /// Obtiene o establece los tipos de asistencia Asociadas
        /// </summary>
        public List<CiaParamAssistanceType> AssistanceType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamRiskType> RiskTypes { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CiaParamFinancialPlan> FinancialPlan { get; set; }
    }
}
