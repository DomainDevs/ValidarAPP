// -----------------------------------------------------------------------
// <copyright file="ParamAssistanceType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models.Base
{
    using Sistran.Core.Application.Extensions;
    using Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Modelo de ramo comercial de los tipos de riesgo cubierto.
    /// </summary>
    public class BaseParamAssistanceType: Extension
    {
        /// <summary>
        /// Id del ramo comercial.
        /// </summary>
        private readonly int assistanceCode;

        /// <summary>
        /// Descripción del ramo comercial.
        /// </summary>
        private readonly string assistanceDescription;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamAssistanceType"/>.
        /// </summary>
        /// <param name="assistanceCode">Identificador del ramo comercial.</param>
        /// <param name="assistanceDescription">Descripción del ramo comercial.</param>
        protected BaseParamAssistanceType(int assistanceCode, string assistanceDescription)
        {
            this.assistanceCode = assistanceCode;
            this.assistanceDescription = assistanceDescription;
        }

        /// <summary>
        /// Obtiene el Id del ramo comercial.
        /// </summary>
        public int AssistanceCode
        {
            get
            {
                return this.assistanceCode;
            }
        }

        /// <summary>
        /// Obtiene la Descripción del ramo comercial.
        /// </summary>
        public string AssistanceDescription
        {
            get
            {
                return this.assistanceDescription;
            }
        }

       
    }
}

