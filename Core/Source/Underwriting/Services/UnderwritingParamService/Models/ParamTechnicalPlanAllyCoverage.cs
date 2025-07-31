// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlanAllyCoverage.cs" company="SISTRAN">
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
    public class ParamTechnicalPlanAllyCoverage: BaseParamTechnicalPlanAllyCoverage
    {
        /// <summary>
        /// Cobertura Aliada
        /// </summary>
        private readonly ParamAllyCoverage _AllyCoverage;
        

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlanAllyCoverage"/>.
        /// </summary>
        /// <param name="allyCoverage">Identificador de la Cobertura.</param>
        /// <param name="allyCoveragePercentage">Porcentaje de Limite de Suma.</param>    
        private ParamTechnicalPlanAllyCoverage(ParamAllyCoverage allyCoverage, decimal? allyCoveragePercentage):
            base(allyCoveragePercentage)
        {
            _AllyCoverage = allyCoverage;
        }

        /// <summary>
        /// Obtiene la Cobertura.
        /// </summary>
        public ParamAllyCoverage AllyCoverage
        {
            get { return _AllyCoverage; }
        }


        /// <summary>
        /// Objeto que obtiene la Relacion Cobertura/Porcentaje.
        /// </summary>
        /// <param name="allyCoverage">Cobertura.</param>
        /// <param name="allyCoveragePercentage">Porcentaje de Limite de Suma.</param>        
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlanAllyCoverage, ErrorModel> GetParamTechnicalPlanAllyCoverage(ParamAllyCoverage allyCoverage, decimal? allyCoveragePercentage)
        {
            return new ResultValue<ParamTechnicalPlanAllyCoverage, ErrorModel>(new ParamTechnicalPlanAllyCoverage(allyCoverage, allyCoveragePercentage));
        }


        /// <summary>
        /// Objeto que Crea la Relacion Cobertura/Porcentaje.
        /// </summary>
        /// <param name="allyCoverage">Cobertura.</param>
        /// <param name="allyCoveragePercentage">Porcentaje de Limite de Suma.</param>        
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlanAllyCoverage, ErrorModel> CreateParamTechnicalPlanAllyCoverage(ParamAllyCoverage allyCoverage, decimal? allyCoveragePercentage)
        {
            List<string> listErrors = new List<string>();
            if (allyCoverage == null)
            {
                listErrors.Add("El campo (Cobertura Aliada) es obligatorio");
            }
            if (listErrors.Count > 0)
            {
                return new ResultError<ParamTechnicalPlanAllyCoverage, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamTechnicalPlanAllyCoverage, ErrorModel>(new ParamTechnicalPlanAllyCoverage(allyCoverage, allyCoveragePercentage));
            }
        }


    }
}
