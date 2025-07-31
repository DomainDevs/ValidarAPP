// -----------------------------------------------------------------------
// <copyright file="ParamVehicleBody.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;
    
    /// <summary>
    /// Tipo de carroceria del vehiculo
    /// </summary>
    public class BaseParamVehicleBody: Extension
    {
        /// <summary>
        /// Identificador del tipo de carroceria
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripcion del tipo de carroceria
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleBody" />
        /// </summary>
        /// <param name="id">Identificador del tipo de carroceria</param>
        /// <param name="description">Descripcion del tipo de carroceria</param>
        protected BaseParamVehicleBody(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Obtiene el identificador de carroceria
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la descripcion de la carroceria
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
