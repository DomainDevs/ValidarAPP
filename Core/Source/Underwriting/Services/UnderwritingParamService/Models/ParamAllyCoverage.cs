// -----------------------------------------------------------------------
// <copyright file="ParamAllyCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.UnderwritingParamService.Models.Base;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    public class ParamAllyCoverage: BaseParamAllyCoverage
    {
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamAllyCoverage"/>.
        /// </summary>
        /// <param name="id">Identificador de la Cobertura.</param>
        /// <param name="description">Descripción de la Cobertura.</param>    
        /// <param name="coveragePercentage">Porcentaje de la Cobertura Aliada.</param>
        private ParamAllyCoverage(int id, string description, decimal? coveragePercentage):
            base(id, description, coveragePercentage)
        {
           
        }

        /// <summary>
        /// Objeto que obtiene la Cobertura.
        /// </summary>
        /// <param name="id">Identificador de la Cobertura.</param>
        /// <param name="description">Descripción de la Cobertura.</param>        
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamAllyCoverage, ErrorModel> GetParamAllyCoverage(int id, string description, decimal? coveragePercentage)
        {
            return new ResultValue<ParamAllyCoverage, ErrorModel>(new ParamAllyCoverage(id, description, coveragePercentage));
        }


        /// <summary>
        /// Objeto que crea la Cobertura.
        /// </summary>
        /// <param name="id">Identificador de la Cobertura.</param>
        /// <param name="description">Descripción de la Cobertura.</param>        
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamAllyCoverage, ErrorModel> CreateParamAllyCoverage(int id, string description, decimal? coveragePercentage)
        {
            List<string> listErrors = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                listErrors.Add("El campo (Descripción) es obligatorio");
            }            
            if (listErrors.Count > 0)
            {
                return new ResultError<ParamAllyCoverage, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamAllyCoverage, ErrorModel>(new ParamAllyCoverage(id, description, coveragePercentage));
            }
        }

    }
}
