// -----------------------------------------------------------------------
// <copyright file="Infringement.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using System.Collections.Generic;

    public class ParamVehicleTransmissionType
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int id;
        /// <summary>
        /// 
        /// </summary>
        private readonly string description;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        private ParamVehicleTransmissionType(int id,string description)
        {
            this.id = id;
            this.description = description;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        public static ResultValue<ParamVehicleTransmissionType, ErrorModel> GetParamVehicleTransmissionType(int id, string description)
        {
            return new ResultValue<ParamVehicleTransmissionType, ErrorModel>(new ParamVehicleTransmissionType(id,description));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static Result<ParamVehicleTransmissionType, ErrorModel> CreateParamVehicleTransmissionType(int id, string description)
        {
            return new ResultValue<ParamVehicleTransmissionType, ErrorModel>(new ParamVehicleTransmissionType(id, description));
        }
    }
}
