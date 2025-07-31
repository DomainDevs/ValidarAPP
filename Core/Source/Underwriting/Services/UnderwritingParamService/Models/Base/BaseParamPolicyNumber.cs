// -----------------------------------------------------------------------
// <copyright file="ParamPolicyNumber.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using System;

    /// <summary>
    /// Modelo para partametrización de nomero de cotización
    /// </summary>
    public class BaseParamPolicyNumber: Extension
    {
        /// <summary>
        /// Obtiene o establece el número de póliza
        /// </summary>
        public decimal PolicyNumber { get; set; }      

        /// <summary>
        /// Obtiene o establece la fecha de la última póliza
        /// </summary>
        public DateTime LastPolicyDate { get; set; }            

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene cotización
        /// </summary>
        public bool HasPolicy { get; set; }
        
    }
}
