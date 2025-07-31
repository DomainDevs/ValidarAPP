// -----------------------------------------------------------------------
// <copyright file="ParamInsuredObjectModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Contiene las propiedades de el Objetos del Seguro
    /// </summary>
    public class ParamInsuredObjectModel: BaseParamInsuredObjectModel
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamInsuredObjectModel"/>.
        /// </summary>
        /// <param name="id">Id de Objetos del Seguro.</param>
        /// <param name="description">Descripción de Objetos del Seguro.</param>
        private ParamInsuredObjectModel(int id, string description):
            base(id, description)
        {
        }

        /// <summary>
        /// Objeto que obtiene el Objetos del Seguro.
        /// </summary>
        /// <param name="id">Id de Objetos del Seguro.</param>
        /// <param name="description">Descripción de Objetos del Seguro.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamInsuredObjectModel, ErrorModel> GetParamInsuredObjectModel(int id, string description)
        {
            return new ResultValue<ParamInsuredObjectModel, ErrorModel>(new ParamInsuredObjectModel(id, description));
        }

        /// <summary>
        /// Objeto que crea el Objetos del Seguro.
        /// </summary>
        /// <param name="id">Id de Objetos del Seguro.</param>
        /// <param name="description">Descripción de Objetos del Seguro.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamInsuredObjectModel, ErrorModel> CreateParamInsuredObjectModel(int id, string description)
        {
            return new ResultValue<ParamInsuredObjectModel, ErrorModel>(new ParamInsuredObjectModel(id, description));
        }
    }
}