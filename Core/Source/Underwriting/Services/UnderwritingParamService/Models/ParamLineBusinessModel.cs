// -----------------------------------------------------------------------
// <copyright file="ParamLineBusinessModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Contiene las propiedades del Ramo Técnico
    /// </summary>
    public class ParamLineBusinessModel: BaseParamLineBusinessModel
    {
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLineBusinessModel"/>.
        /// </summary>
        /// <param name="id">Id del Ramo Técnico.</param>
        /// <param name="description">Descripción del Ramo Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Ramo Técnico.</param>
        /// <param name="tinyDescription">Abreviatura del Ramo Técnico.</param>
        private ParamLineBusinessModel(int id, string description, string smallDescription, string tinyDescription):
            base(id,description, smallDescription, tinyDescription)
        {
            
        }

        /// <summary>
        /// Objeto que obtiene del Ramo Técnico.
        /// </summary>
        /// <param name="id">Id del Ramo Técnico.</param>
        /// <param name="description">Descripción del Ramo Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Ramo Técnico.</param>
        /// <param name="tinyDescription">Abreviatura del Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessModel, ErrorModel> GetParamLineBusinessModel(int id, string description, string smallDescription, string tinyDescription)
        {
            return new ResultValue<ParamLineBusinessModel, ErrorModel>(new ParamLineBusinessModel(id, description, smallDescription, tinyDescription));
        }

        /// <summary>
        /// Objeto que crea del Ramo Técnico.
        /// </summary>
        /// <param name="id">Id del Ramo Técnico.</param>
        /// <param name="description">Descripción del Ramo Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Ramo Técnico.</param>
        /// <param name="tinyDescription">Abreviatura del Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessModel, ErrorModel> CreateParamLineBusinessModel(int id, string description, string smallDescription, string tinyDescription)
        {
            return new ResultValue<ParamLineBusinessModel, ErrorModel>(new ParamLineBusinessModel(id, description, smallDescription, tinyDescription));
        }
    }
}