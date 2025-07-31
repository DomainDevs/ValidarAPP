// -----------------------------------------------------------------------
// <copyright file="ParamVehicleBody.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Tipo de carroceria del vehiculo
    /// </summary>
    public class ParamVehicleBody: BaseParamVehicleBody
    {
        
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleBody" />
        /// </summary>
        /// <param name="id">Identificador del tipo de carroceria</param>
        /// <param name="description">Descripcion del tipo de carroceria</param>
        private ParamVehicleBody(int id, string description):
            base(id, description)
        {
        }

        

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador de la carroceria</param>
        /// <param name="description">Descripcion de la carroceria</param>
        /// <returns>Modelo de solo lectura de la carroceria</returns>
        public static Result<ParamVehicleBody, ErrorModel> GetParamVehicleBody(int id, string description)
        {
            return new ResultValue<ParamVehicleBody, ErrorModel>(new ParamVehicleBody(id, description));
        }

        /// <summary>
        /// Crea el modelo el tipo de carroceria y realiza las validaciones
        /// </summary>
        /// <param name="id">Identificador de la carroceria</param>
        /// <param name="description">Descripcion de la carroceria</param>
        /// <returns>Modelo de solo lectura de la carroceria</returns>
        public static Result<ParamVehicleBody, ErrorModel> CreateParamVehicleBody(int id, string description)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                error.Add("La descripcion corta no puede estar vacia");
            }

            if (description.Length > 15)
            {
                error.Add("La descripcion corta no puede contener mas de 15 caracteres");
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamVehicleBody, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamVehicleBody, ErrorModel>(new ParamVehicleBody(id, description));
        }
    }
}
