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
    /// Modelo de Negocio para VersionVehicleFasecolda
    /// </summary>
    public class ParamVersionVehicleFasecolda
    {
        /// <summary>
        /// Id de la Version
        /// </summary>
        public readonly int versionId;

        /// <summary>
        /// id del modelo
        /// </summary>
        public readonly int modelId;

        /// <summary>
        /// Ide de la marca
        /// </summary>
        public readonly int makeId;

        /// <summary>
        /// codigo de modelo de fasecolda
        /// </summary>
        public readonly string fasecoldaModelId;

        /// <summary>
        /// codigo de marca de fasecolda
        /// </summary>
        public readonly string fasecoldaMakeId;

        private ParamVersionVehicleFasecolda(int versionId, int modelId, int makeId,string fasecoldaModelId,string fasecoldaMakeId)
        {
            this.versionId = versionId;
            this.modelId = modelId;
            this.makeId = makeId;
            this.fasecoldaModelId= fasecoldaModelId;
            this.fasecoldaMakeId = fasecoldaMakeId;
        }

        public int VersionId
        {
            get
            {
                return this.versionId;
            }
        }
        public int ModelId
        {
            get
            {
                return this.modelId;
            }
        }
        public int MakeId
        {
            get
            {
                return this.makeId;
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

        public static Result<ParamVersionVehicleFasecolda, ErrorModel> GetVersionVehicleFasecolda(int versionId,int modelId,int makeId,string fasecoldaModelId, string fasecoldaMakeId)
        {
            return new ResultValue<ParamVersionVehicleFasecolda, ErrorModel>(new ParamVersionVehicleFasecolda(versionId,modelId,makeId,fasecoldaModelId, fasecoldaMakeId));
        }

        public static Result<ParamVersionVehicleFasecolda, ErrorModel> CreateParamVersionVehicleFasecolda(int versionId,int modelId, int makeId, string fasecoldaModelId, string fasecoldaMakeId)
        {
            return new ResultValue<ParamVersionVehicleFasecolda, ErrorModel>(new ParamVersionVehicleFasecolda(versionId,modelId,makeId,fasecoldaModelId,fasecoldaMakeId));
        }
    }
}
