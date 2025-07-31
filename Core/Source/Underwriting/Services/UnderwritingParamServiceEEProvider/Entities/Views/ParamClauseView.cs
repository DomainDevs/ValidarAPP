// -----------------------------------------------------------------------
// <copyright file="ParamClauseView.cs" company="SISTRAN">
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
    /// View de clausula relacionada con clauselevel
    /// </summary>
    [Serializable]
    public class ParamClauseView : BusinessView
    {
        /// <summary>
        /// Obtiene la coleccion de clausulas
        /// </summary>
        public BusinessCollection Clauses
        {
            get
            {
                return this["Clause"];
            }
        }

        /// <summary>
        /// Obtiene la colecciones de clauseleves
        /// </summary>
        public BusinessCollection ClauseLevels
        {
            get
            {
                return this["ClauseLevel"];
            }
        }        
    }
}
