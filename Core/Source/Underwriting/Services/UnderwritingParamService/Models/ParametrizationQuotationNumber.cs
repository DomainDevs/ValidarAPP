// -----------------------------------------------------------------------
// <copyright file="ParametrizationQuotationNumber.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    /// <summary>
    /// Modelo para partametrización de nomero de cotización
    /// </summary>
    public class ParametrizationQuotationNumber: BaseParametrizationQuotationNumber
    {
        /// <summary>
        /// Obtiene o establece el ramo
        /// </summary>
        public ParamPrefix Prefix { get; set; }

        /// <summary>
        /// Obtiene o establece la sucursal
        /// </summary>
        public ParamBranch Branch { get; set; }
    }
}
