// -----------------------------------------------------------------------
// <copyright file="StatesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using Param;

    /// <summary>
    /// Modelo de estados
    /// </summary>
    public class StatesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de estados
        /// </summary>
        public List<StateServiceQueryModel> States { get; set; }
    }
}