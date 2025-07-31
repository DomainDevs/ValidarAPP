// -----------------------------------------------------------------------
// <copyright file="ParamRuleSet.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Stiveen Niño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Parametrizacion del tipo de ejecucion 
    /// </summary>

    public class ParamRuleSet: BaseParamRuleSet
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamRuleSet" />
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion </param>
        /// <param name="description">Descripcion  del tipo de ejecucion</param>
        /// 
        private ParamRuleSet(int? id, string description):
            base(id, description)
        {
        }

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion</param>
        /// <param name="description">Descripcion del tipo de ejecucion</param>
        /// <returns>Tipo de ejecucion</returns>
        public static Result<ParamRuleSet, ErrorModel> GetParamRuleSet(int? id, string description)
        {
            return new ResultValue<ParamRuleSet, ErrorModel>(new ParamRuleSet(id, description));
        }

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y realiza las validaciones de negocio
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion</param>
        /// <param name="description">Descripcion del tipo de ejecucion</param>
        /// <returns>Tipo de vehiculo</returns>
        public static Result<ParamRuleSet, ErrorModel> CreateParamRuleSet(int? id, string description)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                error.Add(Resources.Errors.ValidateDescriptionRuleSet);
            }

            if (description.Length > 50)
            {
                error.Add(Resources.Errors.ValidateSmallDescriptionRuleSet);
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamRuleSet, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamRuleSet, ErrorModel>(new ParamRuleSet(id, description));
        }
    }

}
