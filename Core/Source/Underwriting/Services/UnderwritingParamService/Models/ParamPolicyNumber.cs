// -----------------------------------------------------------------------
// <copyright file="ParamPolicyNumber.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    /// <summary>
    /// Modelo para partametrización de nomero de cotización
    /// </summary>
    public class ParamPolicyNumber: BaseParamPolicyNumber
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
