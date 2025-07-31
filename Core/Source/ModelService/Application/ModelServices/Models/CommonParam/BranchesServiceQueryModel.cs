// -----------------------------------------------------------------------
// <copyright file="BranchesServiceQueryModel.cs" company="SISTRAN">
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
    /// Clase pública BranchesServiceQueryModel
    /// </summary>
    [DataContract]
    public class BranchesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece el modelo sucursales
        /// </summary>
        [DataMember]
        public List<BranchServiceQueryModel> BranchServiceQueryModel { get; set; }
    }
}