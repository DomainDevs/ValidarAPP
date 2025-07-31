// -----------------------------------------------------------------------
// <copyright file="ParamLineBusinessCoveredRiskType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres F. Gonzalez R.</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Contiene las propiedades de los Tipos de Riesgo Cubierto por Ramo Técnico
    /// </summary>
    public class ParamLineBusinessCoveredRiskType
    {
        /// <summary>
        /// Ramo Técnico de los Tipos de Riesgo Cubierto por Ramo Técnico.
        /// </summary>
        private readonly ParamLineBusinessModel paramLineBusinessModel;

        /// <summary>
        /// Tipos de Riesgo Cubierto de los Tipos de Riesgo Cubierto por Ramo Técnico.
        /// </summary>
        private readonly List<ParamCoveredRiskType> paramCoveredRiskType;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamLineBusinessCoveredRiskType"/>.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Tipos de Riesgo Cubierto por Ramo Técnico.</param>
        /// <param name="paramCoveredRiskType">Tipos de Riesgo Cubierto de los Tipos de Riesgo Cubierto por Ramo Técnico.</param>
        private ParamLineBusinessCoveredRiskType(ParamLineBusinessModel paramLineBusinessModel, List<ParamCoveredRiskType> paramCoveredRiskType)
        {
            this.paramLineBusinessModel = paramLineBusinessModel;
            this.paramCoveredRiskType = paramCoveredRiskType;
        }

        /// <summary>
        /// Obtiene el Ramo Técnico de los Tipos de Riesgo Cubierto por Ramo Técnico.
        /// </summary>
        public ParamLineBusinessModel ParamLineBusinessModel
        {
            get
            {
                return this.paramLineBusinessModel;
            }
        }

        /// <summary>
        /// Obtiene los Tipos de Riesgo Cubierto de los Tipos de Riesgo Cubierto por Ramo Técnico.
        /// </summary>
        public List<ParamCoveredRiskType> ParamCoveredRiskType
        {
            get
            {
                return this.paramCoveredRiskType;
            }
        }

        /// <summary>
        /// Objeto que obtiene los Tipos de Riesgo Cubierto por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Tipos de Riesgo Cubierto por Ramo Técnico.</param>
        /// <param name="paramCoveredRiskType">Tipos de Riesgo Cubierto de los Tipos de Riesgo Cubierto por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessCoveredRiskType, ErrorModel> GetParamLineBusinessCoveredRiskType(ParamLineBusinessModel paramLineBusinessModel, List<ParamCoveredRiskType> paramCoveredRiskType)
        {
            return new ResultValue<ParamLineBusinessCoveredRiskType, ErrorModel>(new ParamLineBusinessCoveredRiskType(paramLineBusinessModel, paramCoveredRiskType));
        }

        /// <summary>
        /// Objeto que crea los Tipos de Riesgo Cubierto por Ramo Técnico.
        /// </summary>
        /// <param name="paramLineBusinessModel">Ramo Técnico de los Tipos de Riesgo Cubierto por Ramo Técnico.</param>
        /// <param name="paramCoveredRiskType">Tipos de Riesgo Cubierto de los Tipos de Riesgo Cubierto por Ramo Técnico.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamLineBusinessCoveredRiskType, ErrorModel> CreateParamLineBusinessCoveredRiskType(ParamLineBusinessModel paramLineBusinessModel, List<ParamCoveredRiskType> paramCoveredRiskType)
        {
            return new ResultValue<ParamLineBusinessCoveredRiskType, ErrorModel>(new ParamLineBusinessCoveredRiskType(paramLineBusinessModel, paramCoveredRiskType));
        }
    }
}