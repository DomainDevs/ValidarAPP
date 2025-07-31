// -----------------------------------------------------------------------
// <copyright file="ParamCoveredRiskType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Carlos Eduardo Rodríguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;

    /// <summary>
    /// Modelo de negocio de los tipos de riesgo cubierto.
    /// </summary>
    public class BaseParamCoveredRiskType: Extension
    {
        /// <summary>
        /// Id del tipo de riesgo cubierto.
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripción del tipo de riesgo cubierto.
        /// </summary>
        private readonly string smallDescription;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamCoveredRiskType"/>.
        /// </summary>
        /// <param name="id">Identificador del tipo de riesgo cubierto.</param>
        /// <param name="smallDescription">Descripción del tipo de riesgo cubierto.</param>
        public BaseParamCoveredRiskType(int id, string smallDescription)
        {
            this.id = id;
            this.smallDescription = smallDescription;
        }        

        /// <summary>
        /// Obtiene el Id del tipo de riesgo cubierto.
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del tipo de riesgo cubierto.
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }

    }
}
