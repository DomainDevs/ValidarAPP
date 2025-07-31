// -----------------------------------------------------------------------
// <copyright file="ParamVehicleUse.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Tipo de carroceria del vehiculo
    /// </summary>
    public class BaseParamVehicleUse: Extension
    {
        /// <summary>
        /// Identificador del uso
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripcion del uso
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleUse" />
        /// </summary>
        /// <param name="id">Identificador del uso</param>
        /// <param name="description">Descripcion del uso</param>
        protected BaseParamVehicleUse(int id, string description)
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
