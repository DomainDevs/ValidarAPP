// -----------------------------------------------------------------------
// <copyright file="ParamVehicleModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>John Jairo Peralta</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.Models
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.ModelServices.Models;
    using System;


    /// <summary>
    /// Parametrizacion del modelo de vehiculo
    /// </summary>
    public class ParamVehicleModel
    {
        /// <summary>
        /// Identificador del modelo de vehiculo
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripcion  corta de modelo
        /// </summary>
        private readonly string description;

        /// <summary>
        /// Descripcion larga del modelo de vehiculo
        /// </summary>
        private readonly string smallDescription;

        /// <summary>
        /// Marca y  modelo de vehiculo
        /// </summary>
        private readonly ParamVehicleMake vehicleMake;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="smallDescription"></param>
        /// <param name="vehicleMake"></param>
        public ParamVehicleModel(int id, string description, string smallDescription, ParamVehicleMake vehicleMake)
        {
            this.id = id;
            this.description = description;
            this.smallDescription = smallDescription;
            this.vehicleMake = vehicleMake;
         
        }

        /// <summary>
        /// Obtiene el identificador
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// Obtiene el codigo
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Obtiene la descripcion corta
        /// </summary>
        public string SmallDescription
        {
            get
            {
                return this.smallDescription;
            }
        }

       
        public static Result<ParamVehicleModel, ErrorModel> CreateParamVehicleModel(int id, string description, string smallDescription, VehicelMakeServiceQueryModel vehicelMakeServiceQueryModel)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Obtiene un valor que indica si se encuentra activado
        /// </summary>
        public ParamVehicleMake VehicleMake
        {
            get
            {
                return this.vehicleMake;
            }
        }

        /// <summary>
        /// </summary>

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador del modelo de vehiculo</param>
        /// <param name="description">Descripcion del modelo de vehiculo</param>
        /// <param name="smallDescription">Descripcion corta</param>
        /// <param name="vehicleMake">Indica si esta activado</param>
        /// <returns>Marca de vehiculo</returns>
        public static Result<ParamVehicleModel, ErrorModel> GetParamVehicleModel(int id, string description, string smallDescription, ParamVehicleMake vehicleMake)
        {
            return new ResultValue<ParamVehicleModel, ErrorModel>(new ParamVehicleModel(id, description, smallDescription, vehicleMake));
        }

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y realiza las validaciones de negocio
        /// </summary>
        /// <param name="id">Identificador del modelo de vehiculo</param>
        /// <param name="description">Descripcion del modelo de vehiculo</param>
        /// <param name="smallDescription">Descripcion corta</param>
        /// <param name="vehicleMake">Indica si esta activado</param>
        /// <returns>Modelo de vehiculo</returns>
        public static Result<ParamVehicleModel, ErrorModel> CreateParamVehicleModel(int id, string description, string smallDescription, ParamVehicleMake vehicleMake)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                error.Add(Resources.Errors.ErrorValidacionlDescription);
            }

            if (description.Length > 50)
            {
                error.Add(Resources.Errors.ErrorValidacionDescription);
            }

            if (string.IsNullOrEmpty(smallDescription))
            {
                error.Add(Resources.Errors.ShortDescription);
            }

            if (smallDescription.Length > 15)
            {
                error.Add(Resources.Errors.ShortDescriptionLength);
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamVehicleModel, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamVehicleModel, ErrorModel>(new ParamVehicleModel(id, description, smallDescription, vehicleMake));
        }

    }
}