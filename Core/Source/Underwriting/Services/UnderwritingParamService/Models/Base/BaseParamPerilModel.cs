// -----------------------------------------------------------------------
// <copyright file="ParamPerilModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Contiene las propiedades del Amparo
    /// </summary>
    public class BaseParamPerilModel: Extension
    {
        /// <summary>
        /// ID del Amparo.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripción del Amparo.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamPerilModel"/>.
        /// </summary>
        /// <param name="id">Id del Amparo.</param>
        /// <param name="description">Descripción del Amparo.</param>
        protected BaseParamPerilModel(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Obtiene el Id del Amparo.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del Amparo.
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