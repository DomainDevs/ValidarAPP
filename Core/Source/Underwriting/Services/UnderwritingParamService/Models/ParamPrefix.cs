// -----------------------------------------------------------------------
// <copyright file="ParamPrefix.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Modelo de ramo comercial de los tipos de riesgo cubierto.
    /// </summary>
    public class ParamPrefix: BaseParamPrefix
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamPrefix"/>.
        /// </summary>
        /// <param name="prefixCode">Identificador del ramo comercial.</param>
        /// <param name="description">Descripción del ramo comercial.</param>
        /// <param name="smallDescription">Descripcion corta del ramo comercial.</param>
        public ParamPrefix(int prefixCode, string description, string smallDescription):
            base(prefixCode, description, smallDescription)
        {
        }

        /// <summary>
        /// Objeto que crea u obtiene el ramo comercial.
        /// </summary>
        /// <param name="prefixCode">Identificador del ramo comercial.</param>
        /// <param name="description">Nombre del ramo comercial.</param>
        /// <param name="smallDescription">Descripción corta del ramo comercial</param>
        /// <returns>Retorna el modelo de ramo comercial o un error.</returns>
        public static Result<ParamPrefix, ErrorModel> GetParamPrefix(int prefixCode, string description, string smallDescription)
        {
            return new ResultValue<ParamPrefix, ErrorModel>(new ParamPrefix(prefixCode, description, smallDescription));
        }
    }
}

