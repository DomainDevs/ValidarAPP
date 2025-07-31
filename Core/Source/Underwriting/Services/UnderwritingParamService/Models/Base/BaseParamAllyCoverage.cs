// -----------------------------------------------------------------------
// <copyright file="ParamAllyCoverage.cs" company="SISTRAN">
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
    public class BaseParamAllyCoverage: Extension
    {
        /// <summary>
        /// Identificador de la Cobertura.
        /// </summary>
        private readonly int _Id;
        /// <summary>
        /// Descripción de la Cobertura.
        /// </summary>
        private readonly string _Description;

        /// <summary>
        /// Porcentaje de la cobertura aliada
        /// </summary>
        private readonly decimal? _CoveragePercentage;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamAllyCoverage"/>.
        /// </summary>
        /// <param name="id">Identificador de la Cobertura.</param>
        /// <param name="description">Descripción de la Cobertura.</param>    
        /// <param name="coveragePercentage">Porcentaje de la Cobertura Aliada.</param>
        protected BaseParamAllyCoverage(int id, string description, decimal? coveragePercentage)
        {
            _Id = id;
            _Description = description;
            _CoveragePercentage = coveragePercentage;
        }

        /// <summary>
        /// Obtiene el Id de la Cobertura.
        /// </summary>
        public int Id
        {
            get
            {
                return _Id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción de la Cobertura.
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }
        }

        /// <summary>
        /// Obtiene el Porcentaje de la Cobertura Aliada.
        /// </summary>
        public decimal? CoveragePercentage
        {
            get
            {
                return _CoveragePercentage;
            }
        }

        

    }
}
