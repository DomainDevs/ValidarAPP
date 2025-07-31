// -----------------------------------------------------------------------
// <copyright file="SubLineBranchsServiceModel.cs" company="SISTRAN">
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
    /// Modelo para la lista de SubRamo Tecnico
    /// </summary>
    [DataContract]
    public class SubLineBranchsServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece lista de SubRamo Tecnico
        /// </summary>
        [DataMember]
        public List<SubLineBranchServiceModel> SubLineBranchService { get; set; }
    }
}
