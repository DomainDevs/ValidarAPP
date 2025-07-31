// -----------------------------------------------------------------------
// <copyright file="ParamVehicleUse.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.UnderwritingParamService.Models.Base;
    using Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Tipo de carroceria del vehiculo
    /// </summary>
    public class ParamVehicleUse: BaseParamVehicleUse
    {
        

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleUse" />
        /// </summary>
        /// <param name="id">Identificador del uso</param>
        /// <param name="description">Descripcion del uso</param>
        private ParamVehicleUse(int id, string description):
            base(id, description)
        {
            
        }

       

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador de la carroceria</param>
        /// <param name="description">Descripcion de la carroceria</param>
        /// <returns>Modelo de solo lectura de la carroceria</returns>
        public static Result<ParamVehicleUse, ErrorModel> GetParamVehicleUse(int id, string description)
        {
            return new ResultValue<ParamVehicleUse, ErrorModel>(new ParamVehicleUse(id, description));
        }

        /// <summary>
        /// Crea el modelo el uso y realiza las validaciones
        /// </summary>
        /// <param name="id">Identificador de la carroceria</param>
        /// <param name="description">Descripcion de la carroceria</param>
        /// <returns>Modelo de solo lectura de la carroceria</returns>
        public static Result<ParamVehicleUse, ErrorModel> CreateParamVehicleUse(int id, string description)
        {
            return new ResultValue<ParamVehicleUse, ErrorModel>(new ParamVehicleUse(id, description));
        }
    }
}
