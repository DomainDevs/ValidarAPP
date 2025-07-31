// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.Assemblers
{
    using Sistran.Core.Application.VehicleParamService.Models;
    using Sistran.Core.Application.Common.Entities;




    public class EntityAssembler
    {
        public static CoCptGroupInfringement CreateInfringementGroup(InfringementGroup group)
        {
            return new CoCptGroupInfringement()
            {
                GroupInfringementCode = group.InfringementGroupCode,
                Description = group.Description,
                InfringementOneYear = group.InfringementOneYear,
                InfringementThreeYears = group.InfringementThreeYear,
                SnActive = group.IsActive
            };
        }

        public static CoInfringement CreateInfringement(Infringement infringement)
        {
            return new CoInfringement()
            {
                InfringementCode = 0,
                InfringementNewCode = infringement.InfringementCode,
                InfringementPreviousCode = infringement.InfrigementPreviousCode,
                Description = infringement.Description,
                GroupInfringementCode = infringement.InfrigemenGroupCode
            };
        }

        public static CoInfringementState CreateInfringementState(InfringementState infringementState)
        {
            return new CoInfringementState(infringementState.InfringementStateCode)
            {
                Description = infringementState.Description
            };
        }

        /// <summary>
        /// Crea la entidad a partir del modelo 
        /// </summary>
        /// <param name="vehicleType">tipo de vehiculo</param>
        /// <returns>Entidad de tipo de vehiculo</returns>
        public static CoVehicleVersionFasecolda CreateCoVehicleVersionFasecolda(ParamVersionVehicleFasecolda versionVehicleFasecolda)
        {
            return new CoVehicleVersionFasecolda(versionVehicleFasecolda.versionId, versionVehicleFasecolda.modelId, versionVehicleFasecolda.makeId)
            {
                FasecoldaModelId = versionVehicleFasecolda.fasecoldaModelId.ToString(),
                FasecoldaMakeId = versionVehicleFasecolda.fasecoldaMakeId.ToString()
            };
        }



        #region VehicleVersion
        public static VehicleVersion CreateVehicleVersion(ParamVehicleVersion ParamVehicleVersion)
        {
            return new VehicleVersion(ParamVehicleVersion.Id, ParamVehicleVersion.ParamVehicleModel.Id, ParamVehicleVersion.ParamVehicleMake.Id)
            {
                VehicleVersionCode = ParamVehicleVersion.Id,
                Description = ParamVehicleVersion.Description,
                VehicleModelCode = ParamVehicleVersion.ParamVehicleModel.Id,
                EngineCc = ParamVehicleVersion.EngineQuantity,
                //EngineCylQuantity= ParamVehicleVersion.EngineQuantity,
                VehicleMakeCode = ParamVehicleVersion.ParamVehicleMake.Id,
                Horsepower = ParamVehicleVersion.HorsePower,
                Weight = ParamVehicleVersion.Weight,
                TonsQuantity = ParamVehicleVersion.TonsQuantity,
                PassengerQuantity = ParamVehicleVersion.PassengerQuantity,
                VehicleFuelCode = (ParamVehicleVersion.ParamVehicleFuel == null ? null : (int?)ParamVehicleVersion.ParamVehicleFuel.Id),
                VehicleBodyCode = ParamVehicleVersion.ParamVehicleBody.Id,
                VehicleTypeCode = ParamVehicleVersion.ParamVehicleType.Id,
                TransmissionTypeCode = (ParamVehicleVersion.ParamVehicleTransmissionType == null ? null : (int?)ParamVehicleVersion.ParamVehicleTransmissionType.Id),
                TopSpeed = ParamVehicleVersion.MaxSpeedQuantity,
                DoorQuantity = ParamVehicleVersion.DoorQuantity,
                NewVehiclePrice = ParamVehicleVersion.Price,
                IsImported = ParamVehicleVersion.IsImported,
                LastModel = ParamVehicleVersion.LastModel,
                CurrencyCode = ParamVehicleVersion.ParamCurrency.Id,
                ///<summary>
                ///Se adiciona la propiedad AirConditioning y se inicializa en false, con el objetivo de llenar este campo que era solicitado 
                ///al momento de crear una versión de vehículo, pero que en el formulario actual no era solicitada en el formulario.
                ///</summary>
                ///<author>Diego Leon</author>
                ///<date>10/07/2018</date>
                ///<purpose>REQ_#079</purpose>
                ///<returns></returns>        
                AirConditioning = false
                //IsElectronicPolicy = false
                
                
        };
    }
    #endregion
    #region MakeModel 

    /// <summary>
    /// Construye la entidad de plan de modelo (VehicleMake)
    /// </summary>
    /// <param name="vehicleMake">VehicleMake</param>
    /// <returns>VehicleMake</returns>

    public static VehicleMake CreateVehicleMake(ParamVehicleMake vehicleMake)
    {
        return new VehicleMake(vehicleMake.Id)
        {

            SmallDescription = vehicleMake.Description
        };
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="vehicleModel"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    /// 

    public static VehicleModel CreateVehicleModel(ParamVehicleModel paramVehicleModel)
    {
        return new VehicleModel(paramVehicleModel.Id, paramVehicleModel.VehicleMake.Id)
        {

            Description = paramVehicleModel.Description,
            SmallDescription = paramVehicleModel.SmallDescription

        };
    }


    #endregion



}
}
