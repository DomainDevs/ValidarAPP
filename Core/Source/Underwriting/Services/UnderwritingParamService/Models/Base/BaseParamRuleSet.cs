// -----------------------------------------------------------------------
// <copyright file="ParamRuleSet.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// BaseParametrizacion del tipo de ejecucion 
    /// </summary>

    public class BaseParamRuleSet: Extension
    {
        /// <summary>
        /// Identificador del tipo de ejecucion
        /// </summary>
        private readonly int? id;

        /// <summary>
        /// Descripcion del tipo de ejecucion
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamRuleSet" />
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion </param>
        /// <param name="description">Descripcion  del tipo de ejecucion</param>
        /// 
        protected BaseParamRuleSet(int? id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Obtiene el identificador
        /// </summary>
        public int? Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la descripcion
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        
    }

}
