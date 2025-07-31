// -----------------------------------------------------------------------
// <copyright file="ParametrizationQuotationNumberView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Manuel Méndez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Vista de número de cotizacion
    /// </summary>
    [Serializable]
    public class ParametrizationQuotationNumberView : BusinessView
    {
        /// <summary>
        /// Obtiene lista de números de cotización
        /// </summary>
        public BusinessCollection QuotationNumbers
        {
            get
            {
                return this["QuotationNumber"];
            }
        }

        /// <summary>
        /// Obtiene lista de sucursales
        /// </summary>
        public BusinessCollection Branchs
        {
            get
            {
                return this["Branch"];
            }
        }

        /// <summary>
        /// Obtiene lista de ramos
        /// </summary>
        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        /// <summary>
        /// Obtiene lista de cotizaciones
        /// </summary>
        public BusinessCollection Quotations
        {
            get
            {
                return this["Quotation"];
            }
        }
    }
}
