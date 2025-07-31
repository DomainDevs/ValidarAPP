
// -----------------------------------------------------------------------
// <copyright file="ParamVehicleMake.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>John Jairo Peralta</author>
// -----------------------------------------------------------------------


namespace Sistran.Core.Application.VehicleParamService.Models
{
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Error;
    public class ParamVehicleMake
    {
        /// <summary>
        /// Identificador del marca de vehiculo
        /// </summary>
        private readonly int id;

        /// <summary>
        /// Descripcion  corta de marca de vehiculo
        /// </summary>
        private readonly string description;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public ParamVehicleMake(int id, string description)
        {
            this.id = id;
            this.description = description;
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
        /// Obtiene la descripcion
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }

        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador del modelo de vehiculo</param>
        /// <param name="description">Descripcion del modelo de vehiculo</param>

        public static Result<ParamVehicleMake, ErrorModel> GetParamVehicleMake(int id, string description)
        {
            return new ResultValue<ParamVehicleMake, ErrorModel>(new ParamVehicleMake(id, description));


        }

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y realiza las validaciones de negocio
        /// </summary>
        /// <param name="id">Identificador del modelo de vehiculo</param>
        /// <param name="description">Descripcion del modelo de vehiculo</param>
        /// <returns>Modelo de vehiculo</returns>
        public static Result<ParamVehicleMake, ErrorModel> CreateParamVehicleMake(int id, string description)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                error.Add(Resources.Errors.ErrorValidacionDescription);
            }

            if (description.Length > 50)
            {

                error.Add(Resources.Errors.ErrorValidacionDescription);
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamVehicleMake, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamVehicleMake, ErrorModel>(new ParamVehicleMake(id, description));
        }
    }

}
