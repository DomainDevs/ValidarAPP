// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlanCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.Extensions;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    public class BaseParamTechnicalPlanCoverage: Extension
    {
        
       
        
        /// <summary>
        /// Porcentaje de Suma
        /// </summary>
        private readonly decimal? _CoveragePercentage;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlanCoverage"/>.
        /// </summary>
        /// <param name="insuredObject">Objeto del Seguro.</param>
        /// <param name="coverage">Cobertura.</param>
        /// <param name="principalCoverage">Cobertura Principal.</param>
        /// <param name="coveragePercentage">Porcentaje de limite de suma.</param>
        protected BaseParamTechnicalPlanCoverage(decimal? coveragePercentage)
        {
           
            _CoveragePercentage = coveragePercentage;
        }

        /// <summary>
        /// Obtiene el Porcentaje de limite de suma
        /// </summary>
        public decimal? CoveragePercentage
        {
            get { return _CoveragePercentage; }
        }

        
    }
}
