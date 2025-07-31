// -----------------------------------------------------------------------
// <copyright file="ParamVehicleBodyUse.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Asociasion entre los tipos de vehiculos y Los Usos
    /// </summary>
    public class ParamVehicleBodyUse 
    {
        /// <summary>
        /// Listados de tipos de Usos
        /// </summary>
        private readonly List<ParamVehicleUse> vehicleUses;

        /// <summary>
        /// Carrocería de vehículo
        /// </summary>
        private readonly ParamVehicleBody vehicleBody;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleBodyUse" />
        /// </summary>
        /// <param name="vehicleBody">Carrocería de vehículo</param>
        /// <param name="vehicleUses">Tipos de Usos</param>
        private ParamVehicleBodyUse(ParamVehicleBody vehicleBody, List<ParamVehicleUse> vehicleUses)
        {
            this.vehicleUses = vehicleUses;
            this.vehicleBody = vehicleBody;
        }

        /// <summary>
        /// Obtiene el tipo de vehiculo
        /// </summary>
        public ParamVehicleBody VehicleBody
        {
            get
            {
                return this.vehicleBody;
            }
        }

        /// <summary>
        /// Obtiene el listado de Usos
        /// </summary>
        public List<ParamVehicleUse> VehicleUses
        {
            get
            {
                return this.vehicleUses;
            }
        }

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="vehicleBody">Carrocería de vehículo</param>
        /// <param name="vehicleUses">Listado de Usos</param>
        /// <returns>Relacion de tipo de vehiculo con Usos</returns>
        public static Result<ParamVehicleBodyUse, ErrorModel> GetParamVehicleBodyUse(ParamVehicleBody vehicleBody, List<ParamVehicleUse> vehicleUses)
        {
            return new ResultValue<ParamVehicleBodyUse, ErrorModel>(new ParamVehicleBodyUse(vehicleBody, vehicleUses));
        }

        /// <summary>
        /// Crea el modelo de la asociacion de tipo de vehiculo con Los Usos
        /// </summary>
        /// <param name="vehicleBody">Entida de ParamVehicleBody</param>
        /// <param name="vehicleUses">Lista de ParamVehicleUse </param>
        /// <returns>Relacion de tipo de vehiculo con Usos</returns>
        public static Result<ParamVehicleBodyUse, ErrorModel> CreateParamVehicleBodyUse(ParamVehicleBody vehicleBody, List<ParamVehicleUse> vehicleUses)
        {
            return new ResultValue<ParamVehicleBodyUse, ErrorModel>(new ParamVehicleBodyUse(vehicleBody, vehicleUses));
        }
    }
}
