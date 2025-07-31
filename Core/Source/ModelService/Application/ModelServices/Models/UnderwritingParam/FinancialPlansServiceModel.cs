// -----------------------------------------------------------------------
// <copyright file="FinancialPlansServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Clase public FinancialPlansServiceModel
    /// </summary>
    [DataContract]
    public class FinancialPlansServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de FinancialPlan
        /// </summary>
        [DataMember]
        public List<FinancialPlanServiceModel> FinancialPlanServiceModels { get; set; }
    }
}
