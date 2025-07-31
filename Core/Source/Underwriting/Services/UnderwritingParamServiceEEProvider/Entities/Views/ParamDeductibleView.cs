// -----------------------------------------------------------------------
// <copyright file="ParamDeductibleView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;
    
    /// <summary>
    /// Vista de deducible
    /// </summary>
    [Serializable()]
    public class ParamDeductibleView : BusinessView
    {
        /// <summary>
        /// Obtiene listado deducibles
        /// </summary>
        public BusinessCollection Deductibles
        {
            get
            {
                return this["Deductible"];
            }
        }

        /// <summary>
        /// Obtiene listado ramos
        /// </summary>
        public BusinessCollection LineBusinesses
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene listado unidades
        /// </summary>
        public BusinessCollection DeductibleUnits
        {
            get
            {
                return this["DeductibleUnit"];
            }
        }

        /// <summary>
        /// Obtiene listado unidades
        /// </summary>
        public BusinessCollection MinimumDeductibleUnits
        {
            get
            {
                return this["MinimumDeductibleUnit"];
            }
        }

        /// <summary>
        /// Obtiene listado unidades
        /// </summary>
        public BusinessCollection MaximumDeductibleUnits
        {
            get
            {
                return this["MaximumDeductibleUnit"];
            }
        }

        /// <summary>
        /// Obtiene listado aplica
        /// </summary>
        public BusinessCollection DeductibleSubjects
        {
            get
            {
                return this["DeductibleSubject"];
            }
        }

        /// <summary>
        /// Obtiene listado de aplica
        /// </summary>
        public BusinessCollection MinimumDeductibleSubjects
        {
            get
            {
                return this["MinimumDeductibleSubject"];
            }
        }

        /// <summary>
        /// Obtiene listado de aplica
        /// </summary>
        public BusinessCollection MaximumDeductibleSubjects
        {
            get
            {
                return this["MaximumDeductibleSubject"];
            }
        }

        /// <summary>
        /// Obtiene listado monedas
        /// </summary>
        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }
    }
}
