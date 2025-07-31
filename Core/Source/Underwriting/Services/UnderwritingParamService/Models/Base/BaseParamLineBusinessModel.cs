// -----------------------------------------------------------------------
// <copyright file="ParamLineBusinessModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Contiene las propiedades del Ramo Técnico
    /// </summary>
    public class BaseParamLineBusinessModel: Extension
    {
        /// <summary>
        /// ID del Ramo Técnico.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripción del Ramo Técnico.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Descripción Corta del Ramo Técnico.
        /// </summary>
        private readonly string smallDescription;

        /// <summary>
        /// Abreviatura del Ramo Técnico.
        /// </summary>
        private readonly string tinyDescription;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLineBusinessModel"/>.
        /// </summary>
        /// <param name="id">Id del Ramo Técnico.</param>
        /// <param name="description">Descripción del Ramo Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Ramo Técnico.</param>
        /// <param name="tinyDescription">Abreviatura del Ramo Técnico.</param>
        protected BaseParamLineBusinessModel(int id, string description, string smallDescription, string tinyDescription)
        {
            this.id = id;
            this.description = description;
            this.smallDescription = smallDescription;
            this.tinyDescription = tinyDescription;
        }

        /// <summary>
        /// Obtiene el Id del Ramo Técnico.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del Ramo Técnico.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene la Descripción Corta del Ramo Técnico.
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }

        /// <summary>
        /// Obtiene la Abreviatura del Ramo Técnico.
        /// </summary>
        public string TinyDescription
        {
            get
            {
                return this.tinyDescription;
            }
        }

        
    }
}