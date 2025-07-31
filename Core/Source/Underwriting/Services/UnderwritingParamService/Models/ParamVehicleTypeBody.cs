// -----------------------------------------------------------------------
// <copyright file="ParamVehicleTypeBody.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Julian Ospina</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;
    /// <summary>
    /// Asociasion entre los tipos de vehiculos y las carrocerias
    /// </summary>
    public class ParamVehicleTypeBody 
    {

        /// <summary>
        /// Listados de tipos de carrocerias
        /// </summary>
        private readonly List<ParamVehicleBody> vehicleBodies;

        /// <summary>
        /// Tipo de vehiculo
        /// </summary>
        private readonly ParamVehicleType vehicleType;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleTypeBody" />
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <param name="vehicleBodies">Tipos de carrocerias</param>
        private ParamVehicleTypeBody(ParamVehicleType vehicleType, List<ParamVehicleBody> vehicleBodies)
        {
            this.vehicleBodies = vehicleBodies;
            this.vehicleType = vehicleType;
        }

        /// <summary>
        /// Obtiene el tipo de vehiculo
        /// </summary>
        public ParamVehicleType VehicleType
        {
            get
            {
                return this.vehicleType;
            }
        }

        /// <summary>
        /// Obtiene el listado de carrocerias
        /// </summary>
        public List<ParamVehicleBody> VehicleBodies
        {
            get
            {
                return this.vehicleBodies;
            }
        }

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="vehicleType">Tipo de vehiculo</param>
        /// <param name="vehicleBodies">Listado de carrocerias</param>
        /// <returns>Relacion de tipo de vehiculo con carrocerias</returns>
        public static Result<ParamVehicleTypeBody, ErrorModel> GetParamVehicleTypBody(ParamVehicleType vehicleType, List<ParamVehicleBody> vehicleBodies)
        {
            return new ResultValue<ParamVehicleTypeBody, ErrorModel>(new ParamVehicleTypeBody(vehicleType, vehicleBodies));
        }

        /// <summary>
        /// Crea el modelo de la asociacion de tipo de vehiculo con las carrocerias
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <param name="vehicleBodies"></param>
        /// <returns>Relacion de tipo de vehiculo con carrocerias</returns>
        public static Result<ParamVehicleTypeBody, ErrorModel> CreateParamVehicleBody(ParamVehicleType vehicleType, List<ParamVehicleBody> vehicleBodies)
        {
            return new ResultValue<ParamVehicleTypeBody, ErrorModel>(new ParamVehicleTypeBody(vehicleType, vehicleBodies));
        }
    }
}
