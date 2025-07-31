// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlansCoverage.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------

using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    public class ParamTechnicalPlansCoverage
    {
        /// <summary>
        /// Cobertura
        /// </summary>
        private readonly ParamTechnicalPlanCoverage _TechnicalPlanCoverage;
        /// <summary>
        /// Lista de Coberturas Aliadas
        /// </summary>
        private readonly List<ParamAllyCoverage> _TechnicalPlanAllyCoverages;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlansCoverage"/>.
        /// </summary>
        /// <param name="technicalPlanCoverage">Cobertura.</param>
        /// <param name="technicalPlanAllyCoverages">lista de Coberturas Aliadas.</param>
        private ParamTechnicalPlansCoverage(ParamTechnicalPlanCoverage technicalPlanCoverage, List<ParamAllyCoverage> technicalPlanAllyCoverages)
        {
            _TechnicalPlanCoverage = technicalPlanCoverage;
            _TechnicalPlanAllyCoverages = technicalPlanAllyCoverages;
        }

        /// <summary>
        /// Obtiene la Cobertura.
        /// </summary>
        public ParamTechnicalPlanCoverage TechnicalPlanCoverage
        {
            get { return _TechnicalPlanCoverage; }
        }

        /// <summary>
        /// Obtiene la Lista de Coberturas Aliadas.
        /// </summary>
        public List<ParamAllyCoverage> TechnicalPlanAllyCoverages
        {
            get { return _TechnicalPlanAllyCoverages; }
        }

        /// <summary>
        /// Objeto que obtiene el Modelo.
        /// </summary>
        /// <param name="technicalPlanCoverage">Cobertura.</param>
        /// <param name="technicalPlanAllyCoverages">lista de Coberturas Aliadas.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlansCoverage, ErrorModel> GetParamTechnicalPlansCoverage(ParamTechnicalPlanCoverage technicalPlanCoverage, List<ParamAllyCoverage> technicalPlanAllyCoverages)
        {
            return new ResultValue<ParamTechnicalPlansCoverage, ErrorModel>(new ParamTechnicalPlansCoverage(technicalPlanCoverage, technicalPlanAllyCoverages));
        }


        /// <summary>
        /// Objeto que crea el Modelo.
        /// </summary>
        /// <param name="technicalPlanCoverage">Cobertura.</param>
        /// <param name="technicalPlanAllyCoverages">lista de Coberturas Aliadas.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlansCoverage, ErrorModel> CreateParamTechnicalPlansCoverage(ParamTechnicalPlanCoverage technicalPlanCoverage, List<ParamAllyCoverage> technicalPlanAllyCoverages)
        {
            List<string> listErrors = new List<string>();            
            if (technicalPlanCoverage == null)
            {
                listErrors.Add("El campo (Cobertura) es obligatorio");
            }
            if (technicalPlanAllyCoverages == null)
            {
                listErrors.Add("El campo (Coberturas Aliadas) es obligatorio");
            }
            if (listErrors.Count > 0)
            {
                return new ResultError<ParamTechnicalPlansCoverage, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamTechnicalPlansCoverage, ErrorModel>(new ParamTechnicalPlansCoverage(technicalPlanCoverage, technicalPlanAllyCoverages));
            }
        }
    }
}
