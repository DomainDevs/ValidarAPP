// -----------------------------------------------------------------------
// <copyright file="InsuredObjectLineBusinessView.cs" company="SISTRAN">
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
    /// Objetos del seguro relacionados con el ramo tecnico
    /// </summary>
    [Serializable]
    public class InsuredObjectLineBusinessView : BusinessView
    {
        /// <summary>
        /// Obtiene Coleccion de ramos tecnicos
        /// </summary>
        public BusinessCollection LineBusiness
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        /// <summary>
        /// Obtiene la coleccion de objetos del seguro
        /// </summary>
        public BusinessCollection InsuredObjects
        {
            get
            {
                return this["InsuredObject"];
            }
        }        
    }
}
