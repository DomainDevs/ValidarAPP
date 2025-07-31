// -----------------------------------------------------------------------
// <copyright file="ParamInsuredObjectModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Contiene las propiedades de el Objetos del Seguro
    /// </summary>
    public class BaseParamInsuredObjectModel: Extension
    {
        /// <summary>
        /// ID de Objetos del Seguro.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripción de Objetos del Seguro.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamInsuredObjectModel"/>.
        /// </summary>
        /// <param name="id">Id de Objetos del Seguro.</param>
        /// <param name="description">Descripción de Objetos del Seguro.</param>
        protected BaseParamInsuredObjectModel(int id, string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// Obtiene el Id de Objetos del Seguro.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción de Objetos del Seguro.
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