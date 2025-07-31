// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlanCoverage.cs" company="SISTRAN">
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
    public class ParamTechnicalPlanCoverage: BaseParamTechnicalPlanCoverage
    {
        /// <summary>
        /// Objeto del Seguro
        /// </summary>
        private readonly ParamInsuredObject _InsuredObject;
        /// <summary>
        /// Cobertura
        /// </summary>
        private readonly ParamCoverage _Coverage;
        /// <summary>
        /// Cobertura Básica
        /// </summary>
        private readonly ParamCoverage _PrincipalCoverage;
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlanCoverage"/>.
        /// </summary>
        /// <param name="insuredObject">Objeto del Seguro.</param>
        /// <param name="coverage">Cobertura.</param>
        /// <param name="principalCoverage">Cobertura Principal.</param>
        /// <param name="coveragePercentage">Porcentaje de limite de suma.</param>
        private ParamTechnicalPlanCoverage(ParamInsuredObject insuredObject, ParamCoverage coverage, ParamCoverage principalCoverage, decimal? coveragePercentage):
            base(coveragePercentage)
        {
            _InsuredObject = insuredObject;
            _Coverage = coverage;
            _PrincipalCoverage = principalCoverage;
          
        }

        /// <summary>
        /// Obtiene el Objeto del Seguro
        /// </summary>
        public ParamInsuredObject InsuredObject
        {
            get { return _InsuredObject; }
        }

        /// <summary>
        /// Obtiene la Cobertura
        /// </summary>
        public ParamCoverage Coverage
        {
            get { return _Coverage; }
        }

        /// <summary>
        /// Obtiene la Cobertura Principal
        /// </summary>
        public ParamCoverage PrincipalCoverage
        {
            get { return _PrincipalCoverage; }
        }
        

        /// <summary>
        /// Objeto que obtiene la Cobertua.
        /// </summary>
        /// <param name="insuredObject">Objeto del Seguro.</param>
        /// <param name="coverage">Cobertura.</param>
        /// <param name="principalCoverage">Cobertura Principal.</param>
        /// <param name="CoveragePercentage">Porcentaje de limite de suma.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlanCoverage, ErrorModel> GetParamTechnicalPlanCoverage(ParamInsuredObject insuredObject, ParamCoverage coverage, ParamCoverage principalCoverage, decimal? CoveragePercentage)
        {
            return new ResultValue<ParamTechnicalPlanCoverage, ErrorModel>(new ParamTechnicalPlanCoverage(insuredObject, coverage, principalCoverage, CoveragePercentage));
        }


        /// <summary>
        /// Objeto que crea la Cobertua.
        /// </summary>
        /// <param name="insuredObject">Objeto del Seguro.</param>
        /// <param name="coverage">Cobertura.</param>
        /// <param name="principalCoverage">Cobertura Principal.</param>
        /// <param name="CoveragePercentage">Porcentaje de limite de suma.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlanCoverage, ErrorModel> CreateParamTechnicalPlanCoverage(ParamInsuredObject insuredObject, ParamCoverage coverage, ParamCoverage principalCoverage, decimal? CoveragePercentage)
        {
            List<string> listErrors = new List<string>();
            if (insuredObject == null)
            {
                listErrors.Add("El campo (Objeto del Seguro) es obligatorio");
            }
            if (coverage == null)
            {
                listErrors.Add("El campo (Cobertura) es obligatorio");
            }

            if (listErrors.Count > 0)
            {
                return new ResultError<ParamTechnicalPlanCoverage, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamTechnicalPlanCoverage, ErrorModel>(new ParamTechnicalPlanCoverage(insuredObject, coverage, principalCoverage, CoveragePercentage));
            }
        }
    }
}
