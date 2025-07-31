// -----------------------------------------------------------------------
// <copyright file="PerilLineBusinessView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views
{
    using System;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Views;

    /// <summary>
    /// View de amparos relacionados con el ramo tecnico
    /// </summary>
    [Serializable]
    public class PerilLineBusinessView : BusinessView
    {
        /// <summary>
        /// Obtiene coleccion de ramos tecnicos
        /// </summary>
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene relacion de amparos
        /// </summary>
        public BusinessCollection Perils
        {
            get
            {
                return this["Peril"];
            }
        }        
    }
}
