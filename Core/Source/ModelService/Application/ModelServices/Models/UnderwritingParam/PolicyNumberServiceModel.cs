// -----------------------------------------------------------------------
// <copyright file="PolicyNumberServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    using Sistran.Core.Application.ModelServices.Models.CommonParam;
    using Sistran.Core.Application.ModelServices.Models.Underwriting;
    using System;

    /// <summary>
    /// Modelo de servicio para partametrización de nomero de pólizas
    /// </summary>
    public class PolicyNumberServiceModel
    {
        /// <summary>
        /// Obtiene o establece el número de pólizas
        /// </summary>
        public decimal PolicyLastNumber { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha de la última póliza
        /// </summary>
        public DateTime LastPolicyDate { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si tiene pólizas
        /// </summary>
        public bool HasPolicy { get; set; }

        /// <summary>
        /// Obtiene o establece el ramo
        /// </summary>
        public PrefixServiceQueryModel Prefix { get; set; }

        /// <summary>
        /// Obtiene o establece la sucursal
        /// </summary>
        public BranchServiceQueryModel Branch { get; set; }
    }
}
