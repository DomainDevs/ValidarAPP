// -----------------------------------------------------------------------
// <copyright file="ParamVersionVehicleFasecolda.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Eder Camilo Ramirez</author>
// -----------------------------------------------------------------------

using Sistran.Core.Application.Utilities.Error;

namespace Sistran.Core.Application.VehicleParamService.Models
{
    /// <summary>
    /// Modelo de Negocio para Fasecolda
    /// </summary>
    public class ParamFasecolda
    {
        /// <summary>
        /// Version
        /// </summary>
        public readonly ParamVehicleVersion version;

        /// <summary>
        /// modelo
        /// </summary>
        public readonly ParamVehicleModel model;

        /// <summary>
        /// marca
        /// </summary>
        public readonly ParamVehicleMake make;

        /// <summary>
        /// codigo de modelo de fasecolda
        /// </summary>
        public readonly string fasecoldaModelId;

        /// <summary>
        /// codigo de marca de fasecolda
        /// </summary>
        public readonly string fasecoldaMakeId;

        private ParamFasecolda(ParamVehicleVersion version, ParamVehicleModel model, ParamVehicleMake make,string fasecoldaModelId,string fasecoldaMakeId)
        {
            this.version = version;
            this.model = model;
            this.make = make;
            this.fasecoldaModelId= fasecoldaModelId;
            this.fasecoldaMakeId = fasecoldaMakeId;
        }

        public ParamVehicleVersion Version
        {
            get
            {
                return this.version;
            }
        }
        public ParamVehicleModel Model
        {
            get
            {
                return this.model;
            }
        }
        public ParamVehicleMake Make
        {
            get
            {
                return this.make;
            }
        }
        public string FasecoldaModelId
        {
            get
            {
                return this.fasecoldaModelId;
            }
        }
        public string FasecoldaMakeId
        {
            get
            {
                return this.fasecoldaMakeId;
            }
        }

        public static Result<ParamFasecolda, ErrorModel> GetVersionVehicleFasecolda(ParamVehicleVersion version,ParamVehicleModel model,ParamVehicleMake make,string fasecoldaModelId, string fasecoldaMakeId)
        {
            return new ResultValue<ParamFasecolda, ErrorModel>(new ParamFasecolda(version,model,make,fasecoldaModelId, fasecoldaMakeId));
        }

        public static Result<ParamFasecolda, ErrorModel> CreateFasecolda(ParamVehicleVersion version, ParamVehicleModel model, ParamVehicleMake make, string fasecoldaModelId, string fasecoldaMakeId)
        {
            return new ResultValue<ParamFasecolda, ErrorModel>(new ParamFasecolda(version, model, make, fasecoldaModelId, fasecoldaMakeId));
        }        
    }
}
