// -----------------------------------------------------------------------
// <copyright file="ParamCoveredRiskType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Modelo de negocio de los tipos de riesgo cubierto.
    /// </summary>
    public class ParamCoveredRiskType: BaseParamCoveredRiskType
    {
        

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamCoveredRiskType"/>.
        /// </summary>
        /// <param name="id">Identificador del tipo de riesgo cubierto.</param>
        /// <param name="smallDescription">Descripción del tipo de riesgo cubierto.</param>
        public ParamCoveredRiskType(int id, string smallDescription): 
            base(id,smallDescription)
        {
           
        }        

        /// <summary>
        /// Objeto que crea u obtiene el Tipo de riesgo cubierto.
        /// </summary>
        /// <param name="id">Identificador del tipo de riesgo cubierto.</param>
        /// <param name="smallDescription">Nombre del tipo de riesgo cubierto.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamCoveredRiskType, ErrorModel> GetParamCoveredRiskType(int id, string smallDescription)
        {
            return new ResultValue<ParamCoveredRiskType, ErrorModel>(new ParamCoveredRiskType(id, smallDescription));            
        }

        /// <summary>
        /// Objeto que crea u obtiene el Tipo de riesgo cubierto.
        /// </summary>
        /// <param name="id">Identificador del tipo de riesgo cubierto.</param>
        /// <param name="smallDescription">Nombre del tipo de riesgo cubierto.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamCoveredRiskType, ErrorModel> CreateParamCoveredRiskType(int id, string smallDescription)
        {
            List<string> listErrors = new List<string>();
            if (string.IsNullOrEmpty(smallDescription))
            {
                listErrors.Add("El campo (Descripción Corta) es obligatorio");
            }

            if (smallDescription.Trim().Length > 15)
            {
                listErrors.Add("El campo (Descripción Corta) es de máximo 15 Caracteres");
            }

            if (listErrors.Count > 0)
            {
                return new ResultError<ParamCoveredRiskType, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamCoveredRiskType, ErrorModel>(new ParamCoveredRiskType(id, smallDescription));
            }
        }
    }
}
