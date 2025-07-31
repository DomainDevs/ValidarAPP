// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlanAllyCoverage.cs" company="SISTRAN">
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
    public class BaseParamTechnicalPlanAllyCoverage: Extension
    {
        
        /// <summary>
        /// Porcentaje de Limite de Suma
        /// </summary>
        private readonly decimal? _AllyCoveragePercentage;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlanAllyCoverage"/>.
        /// </summary>
        /// <param name="allyCoverage">Identificador de la Cobertura.</param>
        /// <param name="allyCoveragePercentage">Porcentaje de Limite de Suma.</param>    
        protected BaseParamTechnicalPlanAllyCoverage(decimal? allyCoveragePercentage)
        {
           
            _AllyCoveragePercentage = allyCoveragePercentage; 
        }

       

        /// <summary>
        /// Obtiene el Porcentaje de Limite de Suma.
        /// </summary>
        public decimal? AllyCoveragePercentage
        {
            get { return _AllyCoveragePercentage; }
        }

        
    }
}
