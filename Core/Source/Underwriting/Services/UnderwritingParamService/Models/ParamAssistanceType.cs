// -----------------------------------------------------------------------
// <copyright file="ParamAssistanceType.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Cristyan Fernando Ballesteros</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Modelo de ramo comercial de los tipos de riesgo cubierto.
    /// </summary>
    public class ParamAssistanceType: BaseParamAssistanceType
    {
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamAssistanceType"/>.
        /// </summary>
        /// <param name="assistanceCode">Identificador del ramo comercial.</param>
        /// <param name="assistanceDescription">Descripción del ramo comercial.</param>
        private ParamAssistanceType(int assistanceCode, string assistanceDescription): 
            base(assistanceCode,assistanceDescription)
        {
           
        }

        
        /// <summary>
        /// Objeto que crea u obtiene el ramo comercial.
        /// </summary>
        /// <param name="assistanceCode">Identificador del ramo comercial.</param>
        /// <param name="assistanceDescription">Nombre del ramo comercial.</param>
        /// <returns>Retorna el modelo de ramo comercial o un error.</returns>
        public static Result<ParamAssistanceType, ErrorModel> GetParamAssistanceType(int assistanceCode, string assistanceDescription)
        {
            return new ResultValue<ParamAssistanceType, ErrorModel>(new ParamAssistanceType(assistanceCode, assistanceDescription));
        }
    }
}

