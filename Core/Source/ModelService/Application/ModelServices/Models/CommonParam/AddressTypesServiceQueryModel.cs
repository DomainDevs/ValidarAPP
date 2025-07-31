// -----------------------------------------------------------------------
// <copyright file="AddressTypesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using Param;

    /// <summary>
    /// modelo de tipos de direcciones
    /// </summary>
    public class AddressTypesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de tipods de dirección
        /// </summary>
        public List<AddressTypeServiceQueryModel> AddressTypesService { get; set; }
    }
}
