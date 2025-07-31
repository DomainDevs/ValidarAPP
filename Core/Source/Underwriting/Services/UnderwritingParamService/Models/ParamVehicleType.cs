// -----------------------------------------------------------------------
// <copyright file="ParamVehicleType.cs" company="SISTRAN">
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
    /// Parametrizacion del tipo de vehiculo
    /// </summary>
    public class ParamVehicleType: BaseParamVehicleType
    {
        


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ParamVehicleType" />
        /// </summary>
        /// <param name="id">Identificador de tipo de vehiculo</param>
        /// <param name="description">Descripcion de tipo de vehiculo</param>
        /// <param name="smallDescription">Descripcion corta</param>
        /// <param name="isEnable">Indica si esta activado</param>
        /// <param name="isTruck">Indica si es una camioneta</param>
        private ParamVehicleType(int id, string description, string smallDescription, bool isEnable, bool isTruck) :
            base(id, description, smallDescription, isEnable, isTruck)
        {
        }

        
        /// <summary>
        /// Construye la clase para lectura
        /// </summary>
        /// <param name="id">Identificador de tipo de vehiculo</param>
        /// <param name="description">Descripcion de tipo de vehiculo</param>
        /// <param name="smallDescription">Descripcion corta</param>
        /// <param name="isEnable">Indica si esta activado</param>
        /// <param name="isTruck">Indica si es una camioneta</param>
        /// <returns>Tipo de vehiculo</returns>
        public static Result<ParamVehicleType, ErrorModel> GetParamVehicleType(int id, string description, string smallDescription, bool isEnable, bool isTruck)
        {
            return new ResultValue<ParamVehicleType, ErrorModel>(new ParamVehicleType(id, description, smallDescription, isEnable, isTruck));
        }

        /// <summary>
        /// Crea el modelo de tipo de vehiculo y realiza las validaciones de negocio
        /// </summary>
        /// <param name="id">Identificador de tipo de vehiculo</param>
        /// <param name="description">Descripcion de tipo de vehiculo</param>
        /// <param name="smallDescription">Descripcion corta</param>
        /// <param name="isEnable">Indica si esta activado</param>
        /// <param name="isTruck">Indica si es una camioneta</param>
        /// <returns>Tipo de vehiculo</returns>
        public static Result<ParamVehicleType, ErrorModel> CreateParamVehicleType(int id, string description, string smallDescription, bool isEnable, bool isTruck)
        {
            List<string> error = new List<string>();
            if (string.IsNullOrEmpty(description))
            {
                error.Add("La descripción no puede estar vacia");
            }

            if (description.Length > 50)
            {
                error.Add("La descripción no puede contener mas de 50 caracteres");
            }

            if (string.IsNullOrEmpty(smallDescription))
            {
                error.Add("La descripcion corta no puede estar vacia");
            }

            if (smallDescription.Length > 15)
            {
                error.Add("La descripcion corta no puede contener mas de 15 caracteres");
            }

            if (error.Count > 0)
            {
                return new ResultError<ParamVehicleType, ErrorModel>(ErrorModel.CreateErrorModel(error, Utilities.Enums.ErrorType.BusinessFault, null));
            }

            return new ResultValue<ParamVehicleType, ErrorModel>(new ParamVehicleType(id, description, smallDescription, isEnable, isTruck));
        }
    }
}
