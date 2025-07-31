// -----------------------------------------------------------------------
// <copyright file="ServicesModelsAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.Assemblers
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using Sistran.Core.Application.VehicleParamService.Models;
    using Utilities.Error;

    public class ServicesModelsAssembler
    {
        public static InfringementGroup CreateInfringementGroup(InfringementGroupServiceModel itemSM)
        {
            List<InfringementGroup> cptGroupInfringement = new List<InfringementGroup>();
            List<string> errorModelListDescription = new List<string>();
            Result<InfringementGroup, ErrorModel> result;
            result = InfringementGroup.GetInfringementGroup(itemSM.InfringementGroupCode, itemSM.Description, itemSM.InfrigementOneYear, itemSM.InfrigementThreeYear, itemSM.IsActive);
            return (result as ResultValue<InfringementGroup, ErrorModel>).Value;
        }

        public static Infringement CreateInfringement(InfringementServiceModel itemSM)
        {
            List<Infringement> cptGroupInfringement = new List<Infringement>();
            List<string> errorModelListDescription = new List<string>();
            Result<Infringement, ErrorModel> result;
            result = Infringement.GetInfringement(itemSM.InfringementCode, itemSM.InfringementPreviousCode, itemSM.InfringementDescription, itemSM.InfringementGroupCode, itemSM.InfringementGroupDescription);
            return (result as ResultValue<Infringement, ErrorModel>).Value;
        }

        public static InfringementState CreateInfringementState(InfringementStateServiceModel itemSM)
        {
            List<InfringementState> cptGroupInfringement = new List<InfringementState>();
            List<string> errorModelListDescription = new List<string>();
            Result<InfringementState, ErrorModel> result;
            result = InfringementState.GetInfringementState(itemSM.InfringementStateCode, itemSM.InfringementStateDescription);
            return (result as ResultValue<InfringementState, ErrorModel>).Value;
        }

        internal static ParamFasecolda CreateFasecolda(FasecoldaServiceModel fasecoldaServiceModel)
        {
            ParamVehicleMake paramVehicleMake = ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(fasecoldaServiceModel.Make.Id, fasecoldaServiceModel.Make.Description)).Value;
            //ParamVehicleModel paramVehicleModel = new ParamVehicleModel(fasecoldaServiceModel.Model.Id, fasecoldaServiceModel.Model.Id, fasecoldaServiceModel.Model.Description, string.Empty, paramVehicleMake);
            ParamVehicleModel paramVehicleModel = ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(fasecoldaServiceModel.Model.Id, fasecoldaServiceModel.Model.Description, string.Empty, paramVehicleMake)).Value;
            ParamVehicleFuel paramVehicleFuel = ((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(0, string.Empty)).Value;
            ParamVehicleBody paramVehicleBody = ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(0, string.Empty)).Value;
            ParamVehicleType paramVehicleType = ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(0, string.Empty)).Value;
            ParamCurrency paramCurrency = ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(0, string.Empty)).Value;
            ParamVehicleTransmissionType paramVehicleTransmissionType = ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(0, string.Empty)).Value;
            ParamVehicleVersion paramVersion = ((ResultValue<ParamVehicleVersion, ErrorModel>)ParamVehicleVersion.GetParamVehicleVersion(fasecoldaServiceModel.Version.Id, fasecoldaServiceModel.Version.Description, paramVehicleModel,
                0, paramVehicleMake, 0, 0, 0, 0, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, 0, 0, 0, false, false, paramCurrency)).Value;
            ParamFasecolda paramFasecolda = ((ResultValue<ParamFasecolda, ErrorModel>)ParamFasecolda.GetVersionVehicleFasecolda(paramVersion, paramVehicleModel, paramVehicleMake, fasecoldaServiceModel.FasecoldaModelId, fasecoldaServiceModel.FasecoldaMakeId)).Value;
            return paramFasecolda;
        }
        
         #region Makemodel

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleModelServiceModel"></param>
        /// <returns></returns>
        public static ParamVehicleMake CreateParamVehicleMake(VehicelMakeServiceQueryModel vehicelMakeServiceQuery)
        {
            Result<ParamVehicleMake, ErrorModel> result;
            result = ParamVehicleMake.CreateParamVehicleMake(vehicelMakeServiceQuery.Id, vehicelMakeServiceQuery.Description);
            return (result as ResultValue<ParamVehicleMake, ErrorModel>).Value;
           
        }

        public static ParamVehicleModel CreateParamVehicleModel(VehicleModelServiceModel vehicleModelService)
        {
            ParamVehicleMake paramVehicle = new ParamVehicleMake(vehicleModelService.VehicelMakeServiceQueryModel.Id, vehicleModelService.VehicelMakeServiceQueryModel.Description);
            Result<ParamVehicleModel, ErrorModel> result;
            result = ParamVehicleModel.CreateParamVehicleModel(vehicleModelService.Id, vehicleModelService.Description, vehicleModelService.SmallDescription, paramVehicle);
            return (result as ResultValue<ParamVehicleModel, ErrorModel>).Value;

        }
        #endregion

        #region VehicleVersion
        public static VehicleVersionServiceModel CreateVehicleVersionServiceModel(ParamVehicleVersion paramVehicleVersion)
        {
            return new VehicleVersionServiceModel()
            {
                CurrencyServiceQueryModel= paramVehicleVersion.ParamCurrency==null?null:new CurrencyServiceQueryModel() {Id= paramVehicleVersion.ParamCurrency.Id },
                StatusTypeService = StatusTypeService.Original,
                Id = paramVehicleVersion.Id,
                LastModel = paramVehicleVersion.LastModel,
                IsImported = paramVehicleVersion.IsImported,
                Price = paramVehicleVersion.Price,
                DoorQuantity = paramVehicleVersion.DoorQuantity,
                MaxSpeedQuantity = paramVehicleVersion.MaxSpeedQuantity,
                Description = paramVehicleVersion.Description,
                EngineQuantity = paramVehicleVersion.EngineQuantity,
                HorsePower = paramVehicleVersion.HorsePower,
                PassengerQuantity = paramVehicleVersion.PassengerQuantity,
                TonsQuantity = paramVehicleVersion.TonsQuantity,
                Weight = paramVehicleVersion.Weight,
                VehicleBodyServiceQueryModel = paramVehicleVersion.ParamVehicleBody==null?null: new ModelServices.Models.UnderwritingParam.VehicleBodyServiceQueryModel() {StatusTypeService= StatusTypeService.Original,ErrorServiceModel=new ModelServices.Models.Param.ErrorServiceModel() {ErrorDescription=new List<string> (),ErrorTypeService= ErrorTypeService .Ok}, Description = paramVehicleVersion.ParamVehicleBody.Description, Id = paramVehicleVersion.ParamVehicleBody.Id },
                VehicleFuelServiceQueryModel = paramVehicleVersion.ParamVehicleFuel == null ? null : new VehicleFuelServiceQueryModel { Id = paramVehicleVersion.ParamVehicleFuel.Id, Description = paramVehicleVersion.ParamVehicleFuel.Description },
                VehicleMakeServiceQueryModel = paramVehicleVersion.ParamVehicleMake == null ? null : new VehicleMakeServiceQueryModel() { Id = paramVehicleVersion.ParamVehicleMake.Id, Description = paramVehicleVersion.ParamVehicleMake.Description },
                VehicleModelServiceQueryModel = paramVehicleVersion.ParamVehicleModel == null ? null : new VehicleModelServiceQueryModel() { Description = paramVehicleVersion.ParamVehicleModel.Description, Id = paramVehicleVersion.ParamVehicleModel.Id },
                VehicleTransmissionTypeServiceQueryModel = paramVehicleVersion.ParamVehicleTransmissionType == null ? null : new VehicleTransmissionTypeServiceQueryModel() { Id = paramVehicleVersion.ParamVehicleTransmissionType.Id, Description = paramVehicleVersion.ParamVehicleTransmissionType.Description },
                VehicleTypeServiceQueryModel = paramVehicleVersion.ParamVehicleType == null ? null : new VehicleTypeServiceQueryModel() { Id = paramVehicleVersion.ParamVehicleType.Id, Description = paramVehicleVersion.ParamVehicleType.Description }
            };
        }
        public static List<VehicleVersionServiceModel>  CreateVehicleVersionsServiceModel(List<ParamVehicleVersion > paramVehicleVersion)
        {
            List<VehicleVersionServiceModel> result = new List<VehicleVersionServiceModel>();
            foreach (var item in paramVehicleVersion)
            {
                result.Add(CreateVehicleVersionServiceModel(item));
            }
            return result;
        }
        #endregion
        #region VehicleTransmissionTypeQueryModelServiceModel
        public static List<VehicleTransmissionTypeServiceQueryModel> CreateVehicleTransmissionTypeQueryModelServiceModel(List<ParamVehicleTransmissionType> paramTransmission)
        {
            List<VehicleTransmissionTypeServiceQueryModel> listServiceModel = new List<VehicleTransmissionTypeServiceQueryModel>();
            foreach (ParamVehicleTransmissionType modelBusinessModel in paramTransmission)
            {
                listServiceModel.Add(new VehicleTransmissionTypeServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description
                });
            }
            return listServiceModel;
        }
        #endregion
        #region VehicleBodyQueryModelServiceModel
        public static List<VehicleBodyServiceQueryModel> CreateVehicleBodyQueryModelServiceModel(List<ParamVehicleBody> paramBody)
        {
            List<VehicleBodyServiceQueryModel> listServiceModel = new List<VehicleBodyServiceQueryModel>();
            foreach (ParamVehicleBody modelBusinessModel in paramBody)
            {
                listServiceModel.Add(new VehicleBodyServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description,
                    StatusTypeService=ModelServices.Enums.StatusTypeService.Original
                    
                });
            }
            return listServiceModel;
        }
        #endregion
        #region CurrencyQueryModelServiceModel
        public static List<CurrencyServiceQueryModel> CreateCurrencyQueryModelServiceModel(List<ParamCurrency> paramBody)
        {
            List<CurrencyServiceQueryModel> listServiceModel = new List<CurrencyServiceQueryModel>();
            foreach (ParamCurrency modelBusinessModel in paramBody)
            {
                listServiceModel.Add(new CurrencyServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description
                });
            }
            return listServiceModel;
        }
        #endregion
        #region VehicleTypeQueryModelServiceModel
        public static List<VehicleTypeServiceQueryModel> CreateVehicleTypeQueryModelServiceModel(List<ParamVehicleType> paramType)
        {
            List<VehicleTypeServiceQueryModel> listServiceModel = new List<VehicleTypeServiceQueryModel>();
            foreach (ParamVehicleType modelBusinessModel in paramType)
            {
                listServiceModel.Add(new VehicleTypeServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description
                });
            }
            return listServiceModel;
        }
        #endregion
        #region VehicleFuelQueryModelServiceModel
        public static List<VehicleFuelServiceQueryModel> CreateVehicleFuelQueryModelServiceModel(List<ParamVehicleFuel> paramFuel)
        {
            List<VehicleFuelServiceQueryModel> listServiceModel = new List<VehicleFuelServiceQueryModel>();
            foreach (ParamVehicleFuel modelBusinessModel in paramFuel)
            {
                listServiceModel.Add(new VehicleFuelServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description
                });
            }
            return listServiceModel;
        }
        #endregion
        #region VehicleMakeServiceQueryModel
        public static List<VehicleMakeServiceQueryModel> CreateVehicleMakeServiceQueryModel(List<ParamVehicleMake> paramMake)
        {
            List<VehicleMakeServiceQueryModel> listServiceModel = new List<VehicleMakeServiceQueryModel>();
            foreach (ParamVehicleMake modelBusinessModel in paramMake)
            {
                listServiceModel.Add(new VehicleMakeServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description
                });
            }
            return listServiceModel;
        }
        #endregion

        #region VehicleModelServiceQueryModel
        public static List<VehicleModelServiceQueryModel> CreateVehicleModelServiceQueryModel(List<ParamVehicleModel> paramModel)
        {
            List<VehicleModelServiceQueryModel> listService = new List<VehicleModelServiceQueryModel>();
            foreach (ParamVehicleModel modelBusinessModel in paramModel)
            {
                listService.Add(new VehicleModelServiceQueryModel
                {
                    Id = modelBusinessModel.Id,
                    Description = modelBusinessModel.Description
                });
            }
            return listService;
        }
        #endregion

    }
}