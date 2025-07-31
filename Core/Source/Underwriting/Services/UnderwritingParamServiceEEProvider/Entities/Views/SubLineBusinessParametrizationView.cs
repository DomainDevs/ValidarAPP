// -----------------------------------------------------------------------
// <copyright file="SubLineBusinessParametrizationView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;
   
    /// <summary>
    /// clase para la vista SubRamo Tecnico
    /// </summary>
    [Serializable]
    public class SubLineBusinessParametrizationView : BusinessView
    {
        /// <summary>
        /// Obtiene entidad SubRamo Tecnico
        /// </summary>
        public BusinessCollection SubLineBusiness
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene entidad Ramo Tecnico
        /// </summary>
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }
    }
}
