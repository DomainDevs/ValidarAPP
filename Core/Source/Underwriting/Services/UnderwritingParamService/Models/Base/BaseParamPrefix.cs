// -----------------------------------------------------------------------
// <copyright file="ParamPrefix.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de ramo comercial de los tipos de riesgo cubierto.
    /// </summary>
    public class BaseParamPrefix: Extension
    {
        /// <summary>
        /// Id del ramo comercial.
        /// </summary>
        private readonly int prefixCode;

        /// <summary>
        /// Descripción del ramo comercial.
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Descripcion corta del ramo comercial.
        /// </summary>
        private readonly string smallDescription;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamPrefix"/>.
        /// </summary>
        /// <param name="prefixCode">Identificador del ramo comercial.</param>
        /// <param name="description">Descripción del ramo comercial.</param>
        /// <param name="smallDescription">Descripcion corta del ramo comercial.</param>
        protected BaseParamPrefix(int prefixCode, string description, string smallDescription)
        {
            this.prefixCode = prefixCode;
            this.description = description;
            this.smallDescription = smallDescription;
        }

        /// <summary>
        /// Obtiene el Id del ramo comercial.
        /// </summary>
        public int PrefixCode
        {
            get
            {
                return this.prefixCode;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del ramo comercial.
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene la descripción corta del ramo comercial.
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }

        
    }
}

