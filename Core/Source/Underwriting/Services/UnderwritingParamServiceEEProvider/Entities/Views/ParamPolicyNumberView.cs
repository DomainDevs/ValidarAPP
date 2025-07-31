// -----------------------------------------------------------------------
// <copyright file="ParamPolicyNumberView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// Vista de número de pólizas
    /// </summary>
    [Serializable]
    public class ParamPolicyNumberView : BusinessView
    {
        /// <summary>
        /// Obtiene lista de números de pólizas
        /// </summary>
        public BusinessCollection PolicyNumbers
        {
            get
            {
                return this["PolicyNumber"];
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
        /// Obtiene lista de pólizas
        /// </summary>
        public BusinessCollection Policys
        {
            get
            {
                return this["Policy"];
            }
        }
    }
}
