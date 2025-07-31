// -----------------------------------------------------------------------
// <copyright file="ParamFinancialPlanComponent.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    /// <summary>
    /// Modelo plan financiero y componentes
    /// </summary>
    [DataContract]
    public class ParamFinancialPlanComponent
    {
        /// <summary>
        /// Obtiene o establece modelo plan fincanciero
        /// </summary>
        [DataMember]
        public ParamFinancialPlan ParamFinancialPlan { get; set; }

        /// <summary>
        /// Obtiene o establece asociacion
        /// </summary>
        [DataMember]
        public List<ParamFirstPayComponent> ParamFirstPayComponent { get; set; }
    }
}
