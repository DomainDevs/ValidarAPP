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

    public class ParamRateType: BaseParamRateType
    {
       
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamRateType" />
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion </param>
        /// <param name="description">Descripcion  del tipo de ejecucion</param>
        /// 
        private ParamRateType(int id, string description):
            base(id,description)
        {
        }


        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion</param>
        /// <param name="description">Descripcion del tipo de ejecucion</param>
        /// <returns>Tipo de ejecucion</returns>
        public static Result<ParamRateType, ErrorModel> GetParamRuleSet(int id, string description)
        {
            return new ResultValue<ParamRateType, ErrorModel>(new ParamRateType(id, description));
        }

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y realiza las validaciones de negocio
        /// </summary>
        /// <param name="id">Identificador del tipo de ejecucion</param>
        /// <param name="description">Descripcion del tipo de ejecucion</param>
        /// <returns>Tipo de vehiculo</returns>
        public static Result<ParamRateType, ErrorModel> CreateParamRateType(int id, string description)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                error.Add(Resources.Errors.ValidateDescriptionRateType);
            }

            if (description.Length > 50)
            {
                error.Add(Resources.Errors.ValidateSmallDescriptionRateType);
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamRateType, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamRateType, ErrorModel>(new ParamRateType(id, description));
        }
    }

}
