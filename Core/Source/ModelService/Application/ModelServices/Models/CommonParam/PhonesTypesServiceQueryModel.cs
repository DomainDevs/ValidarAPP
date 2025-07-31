// -----------------------------------------------------------------------
// <copyright file="PhonesTypesServiceQueryModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.CommonParam
{
    using System.Collections.Generic;
    using Param;

    /// <summary>
    /// modelo tipos de teléfonos
    /// </summary>
    public class PhonesTypesServiceQueryModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece la lista de paises
        /// </summary>
        public List<PhoneTypeServiceQueryModel> PhonesTypes { get; set; }
    }
}
