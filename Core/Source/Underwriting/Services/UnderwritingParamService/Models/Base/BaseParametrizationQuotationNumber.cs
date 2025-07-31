// -----------------------------------------------------------------------
// <copyright file="ParametrizationQuotationNumber.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;

    /// <summary>
    /// Modelo para partametrización de nomero de cotización
    /// </summary>
    public class BaseParametrizationQuotationNumber: Extension
    {
        /// <summary>
        /// Obtiene o establece el número de cotización
        /// </summary>
        public int QuotNumber { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene cotización
        /// </summary>
        public bool HasQuotation { get; set; }

       
    }
}
