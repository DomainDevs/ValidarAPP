// -----------------------------------------------------------------------
// <copyright file="ParamTechnicalPlan.cs" company="SISTRAN">
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
    public class ParamTechnicalPlan: BaseParamTechnicalPlan
    {
        
        /// <summary>
        /// Tipo de Riesgo Cubierto relacionado con el Plan Técnico.
        /// </summary>
        private readonly ParamCoveredRiskType _CoveredRiskType;
       

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamTechnicalPlan"/>.
        /// </summary>
        /// <param name="id">Identificador del Plan Técnico.</param>
        /// <param name="description">Descripción del Plan Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Plan Técnico.</param>
        /// <param name="coveredRiskType">Tipo de Riesgo Cubierto relacionado con el Plan Técnico.</param>
        /// <param name="currentFrom">Fecha de Creación del Plan Técnico.</param>
        /// <param name="currentTo">Fecha de Finalización del Plan Técnico.</param>
        private ParamTechnicalPlan(int id, string description, string smallDescription, ParamCoveredRiskType coveredRiskType, System.DateTime currentFrom, System.DateTime? currentTo):
            base(id,description,smallDescription,currentFrom,currentTo)
        {
           
            _CoveredRiskType = coveredRiskType;
           
        }

      

        /// <summary>
        /// Obtiene el Tipo de Riesgo Cubierto relacionado con el Plan Técnico.
        /// </summary>
        public ParamCoveredRiskType CoveredRiskType
        {
            get
            {
                return _CoveredRiskType;
            }
        }

        


        /// <summary>
        /// Objeto que obtiene el Plan Técnico.
        /// </summary>
        /// <param name="id">Identificador del Plan Técnico.</param>
        /// <param name="description">Descripción del Plan Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Plan Técnico.</param>
        /// <param name="coveredRiskType">Tipo de Riesgo Cubierto relacionado con el Plan Técnico.</param>
        /// <param name="currentFrom">Fecha de Creación del Plan Técnico.</param>
        /// <param name="currentTo">Fecha de Finalización del Plan Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlan, ErrorModel> GetParamTechnicalPlan(int id, string description, string smallDescription, ParamCoveredRiskType coveredRiskType, System.DateTime currentFrom, System.DateTime? currentTo)
        {
            return new ResultValue<ParamTechnicalPlan, ErrorModel>(new ParamTechnicalPlan(id, description, smallDescription, coveredRiskType, currentFrom, currentTo));
        }


        /// <summary>
        /// Objeto que crea el Plan Técnico.
        /// </summary>
        /// <param name="id">Identificador del Plan Técnico.</param>
        /// <param name="description">Descripción del Plan Técnico.</param>
        /// <param name="smallDescription">Descripción Corta del Plan Técnico.</param>
        /// <param name="coveredRiskType">Tipo de Riesgo Cubierto relacionado con el Plan Técnico.</param>
        /// <param name="currentFrom">Fecha de Creación del Plan Técnico.</param>
        /// <param name="currentTo">Fecha de Finalización del Plan Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamTechnicalPlan, ErrorModel> CreateParamTechnicalPlan(int id, string description, string smallDescription, ParamCoveredRiskType coveredRiskType, System.DateTime currentFrom, System.DateTime? currentTo)
        {
            List<string> listErrors = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                listErrors.Add("El campo (Descripción) es obligatorio");
            }
            if (string.IsNullOrEmpty(smallDescription))
            {
                listErrors.Add("El campo (Descripción Corta) es obligatorio");
            }
            if (coveredRiskType == null)
            {
                listErrors.Add("El campo (Tipo de Riesgo) es obligatorio");
            }

            if (listErrors.Count > 0)
            {
                return new ResultError<ParamTechnicalPlan, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamTechnicalPlan, ErrorModel>(new ParamTechnicalPlan(id, description, smallDescription, coveredRiskType, currentFrom, currentTo));
            }            
        }
    }
}
