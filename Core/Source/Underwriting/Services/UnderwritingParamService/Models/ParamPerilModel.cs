// -----------------------------------------------------------------------
// <copyright file="ParamPerilModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Contiene las propiedades del Amparo
    /// </summary>
    public class ParamPerilModel: BaseParamPerilModel
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
        private ParamPerilModel(int id, string description):
            base(id, description)
        {
        }

        /// <summary>
        /// Objeto que obtiene del Amparo.
        /// </summary>
        /// <param name="id">Id del Amparo.</param>
        /// <param name="description">Descripción del Amparo.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamPerilModel, ErrorModel> GetParamPerilModel(int id, string description)
        {
            return new ResultValue<ParamPerilModel, ErrorModel>(new ParamPerilModel(id, description));
        }

        /// <summary>
        /// Objeto que crea del Amparo.
        /// </summary>
        /// <param name="id">Id del Amparo.</param>
        /// <param name="description">Descripción del Amparo.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamPerilModel, ErrorModel> CreateParamPerilModel(int id, string description)
        {
            return new ResultValue<ParamPerilModel, ErrorModel>(new ParamPerilModel(id, description));
        }
    }
}