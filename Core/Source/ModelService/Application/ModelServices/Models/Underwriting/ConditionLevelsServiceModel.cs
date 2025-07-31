// -----------------------------------------------------------------------
// <copyright file="ConditionLevelsServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.Underwriting
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo lista de niveles
    /// </summary>
    [DataContract]
    public class ConditionLevelsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de niveles
        /// </summary>
        [DataMember]
        public List<ConditionLevelServiceModel> ConditionLevelsServiceModels { get; set; }
    }
}
