// -----------------------------------------------------------------------
// <copyright file="BranchesServicesModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Param;

    /// <summary>
    /// Unidad de sucursales
    /// </summary>
    [DataContract]
    public class BranchesServicesModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo sucursales
        /// </summary>
        [DataMember]
        public List<BranchServiceModel> BranchServiceModel { get; set; }
    }
}
