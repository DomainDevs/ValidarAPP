// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlanDTO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Alberto Sánchez Lesmes</author>
// -----------------------------------------------------------------------
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    public class ParamTechnicalPlanDTO
    {
        /// <summary>
        /// Plan Técnico
        /// </summary>
        private readonly ParamTechnicalPlan _TechnicalPlan;
        /// <summary>
        /// Lista de Coberturas Asociadas
        /// </summary>
        private readonly List<ParamTechnicalPlansCoverage> _TechnicalPlanCoverages;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlanDTO"/>.
        /// </summary>
        /// <param name="technicalPlan">Plan Técnico.</param>
        /// <param name="technicalPlanCoverages">Lista de Coberturas Asociadas.</param> 
        public ParamTechnicalPlanDTO(ParamTechnicalPlan technicalPlan, List<ParamTechnicalPlansCoverage> technicalPlanCoverages)
        {
            _TechnicalPlan = technicalPlan;
            _TechnicalPlanCoverages = technicalPlanCoverages;
        }

        /// <summary>
        /// Obtiene el Plan Técnico.
        /// </summary>
        public ParamTechnicalPlan TechnicalPlan
        {
            get { return _TechnicalPlan; }
        }

        /// <summary>
        /// Obtiene la lista de Coberturas Asociadas.
        /// </summary>
        public List<ParamTechnicalPlansCoverage> TechnicalPlanCoverages
        {
            get { return _TechnicalPlanCoverages; }
        }

        /// <summary>
        /// Objeto que obtiene el modelo.
        /// </summary>
        /// <param name="technicalPlan">Plan Técnico.</param>
        /// <param name="technicalPlanCoverages">Lista de Coberturas Asociadas.</param>   
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlanDTO, ErrorModel> GetParamTechnicalPlanDTO(ParamTechnicalPlan technicalPlan, List<ParamTechnicalPlansCoverage> technicalPlanCoverages)
        {
            return new ResultValue<ParamTechnicalPlanDTO, ErrorModel>(new ParamTechnicalPlanDTO(technicalPlan, technicalPlanCoverages));
        }


        /// <summary>
        /// Objeto que crea el modelo.
        /// </summary>
        /// <param name="technicalPlan">Plan Técnico.</param>
        /// <param name="technicalPlanCoverages">Lista de Coberturas Asociadas.</param>        
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlanDTO, ErrorModel> CreateParamTechnicalPlanDTO(ParamTechnicalPlan technicalPlan, List<ParamTechnicalPlansCoverage> technicalPlanCoverages)
        {
            List<string> listErrors = new List<string>();
            if (technicalPlan == null)
            {
                listErrors.Add("El campo (Plan Técnico) es obligatorio");
            }
            if (technicalPlanCoverages == null)
            {
                listErrors.Add("El campo (Lista de Coberturas) es obligatorio");
            }
            if (listErrors.Count > 0)
            {
                return new ResultError<ParamTechnicalPlanDTO, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamTechnicalPlanDTO, ErrorModel>(new ParamTechnicalPlanDTO(technicalPlan, technicalPlanCoverages));
            }
        }

    }
}
