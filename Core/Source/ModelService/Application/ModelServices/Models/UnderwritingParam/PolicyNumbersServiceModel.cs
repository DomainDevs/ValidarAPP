// -----------------------------------------------------------------------
// <copyright file="PolicyNumbersServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para partametrización de número de pólizas
    /// </summary>
    public class PolicyNumbersServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los números de pólizas (Modelo del servicio)
        /// </summary>
        [DataMember]
        public List<PolicyNumberServiceModel> PolicyNumberServiceModels { get; set; }
    }
}
