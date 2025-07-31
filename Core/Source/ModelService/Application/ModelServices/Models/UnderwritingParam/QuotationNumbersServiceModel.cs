// -----------------------------------------------------------------------
// <copyright file="QuotationNumbersServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo de servicio para partametrización de nomero de cotización
    /// </summary>
    public class QuotationNumbersServiceModel : ErrorServiceModel
    {
        /// <summary>
        /// Obtiene o establece los números de cotización (Modelo del servicio)
        /// </summary>
        [DataMember]
        public List<QuotationNumberServiceModel> QuotationNumberServiceModels { get; set; }
    }
}
