// -----------------------------------------------------------------------
// <copyright file="ParamClauseModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Contiene las propiedades de la Cláusula
    /// </summary>
    public class ParamClauseModel: BaseParamClauseModel
    {
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamClauseModel"/>.
        /// </summary>
        /// <param name="id">Id de Cláusula.</param>
        /// <param name="description">Descripción de Cláusula.</param>
        /// <param name="isMandatory">Obligatorio de Cláusula.</param>
        private ParamClauseModel(int id, string description, bool isMandatory): 
            base(id, description, isMandatory)
        {
            
        }

        /// <summary>
        /// Objeto que obtiene la Cláusula.
        /// </summary>
        /// <param name="id">Id de Cláusula.</param>
        /// <param name="description">Descripción de Cláusula.</param>
        /// <param name="isMandatory">Obligatorio de Cláusula.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamClauseModel, ErrorModel> GetParamClauseModel(int id, string description, bool isMandatory)
        {
            return new ResultValue<ParamClauseModel, ErrorModel>(new ParamClauseModel(id, description, isMandatory));
        }

        /// <summary>
        /// Objeto que crea la Cláusula.
        /// </summary>
        /// <param name="id">Id de Cláusula.</param>
        /// <param name="description">Descripción de Cláusula.</param>
        /// <param name="isMandatory">Obligatorio de Cláusula.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamClauseModel, ErrorModel> CreateParamClauseModel(int id, string description, bool isMandatory)
        {
            return new ResultValue<ParamClauseModel, ErrorModel>(new ParamClauseModel(id, description, isMandatory));
        }
    }
}