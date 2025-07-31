// -----------------------------------------------------------------------
// <copyright file="QuotationNumberServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

using Sistran.Core.Application.ModelServices.Models.CommonParam;
using Sistran.Core.Application.ModelServices.Models.Underwriting;

namespace Sistran.Core.Application.ModelServices.Models.UnderwritingParam
{
    /// <summary>
    /// Modelo de servicio para partametrización de nomero de cotización
    /// </summary>
    public class QuotationNumberServiceModel
    {
        /// <summary>
        /// Obtiene o establece el número de cotización
        /// </summary>
        public int QuotNumber { get; set; }

        /// <summary>
        /// indica si tiene cotización
        /// </summary>
        public bool HasQuotation { get; set; }

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
