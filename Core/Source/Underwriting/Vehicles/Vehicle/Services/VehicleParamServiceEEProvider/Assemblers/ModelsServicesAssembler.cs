// -----------------------------------------------------------------------
// <copyright file="ModelServiceAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider.Assemblers
{
    using ModelServices.Models.VehicleParam;
    using VehicleParamService.Models;
    using System.Collections.Generic;
    using ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.VehicleParamService.EEProvider.Resources;
    using System;

    public class ModelsServicesAssembler
    {
        public static InfringementGroupsServiceModel MappInfringementGroup(List<InfringementGroup> paramInfrigementGroup)
        {
            InfringementGroupsServiceModel paramInfringementGroupServiceModel = new InfringementGroupsServiceModel();
            List<InfringementGroupServiceModel> listInfringementGroupServiceModel = new List<InfringementGroupServiceModel>();
            foreach (InfringementGroup paramInfringementGroupBusinessModel in paramInfrigementGroup)
            {
                InfringementGroupServiceModel itemInfringementGroupServiceModel = new InfringementGroupServiceModel();
                itemInfringementGroupServiceModel.InfringementGroupCode = paramInfringementGroupBusinessModel.InfringementGroupCode;
                itemInfringementGroupServiceModel.Description = paramInfringementGroupBusinessModel.Description;
                itemInfringementGroupServiceModel.InfrigementOneYear = paramInfringementGroupBusinessModel.InfringementOneYear;
                itemInfringementGroupServiceModel.InfrigementThreeYear = paramInfringementGroupBusinessModel.InfringementThreeYear;
                itemInfringementGroupServiceModel.IsActive = paramInfringementGroupBusinessModel.IsActive;
                itemInfringementGroupServiceModel.StatusTypeService = StatusTypeService.Original;
                listInfringementGroupServiceModel.Add(itemInfringementGroupServiceModel);
            }

            paramInfringementGroupServiceModel.ErrorDescription = new List<string>();
            paramInfringementGroupServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramInfringementGroupServiceModel.InfringementGroupServiceModel = listInfringementGroupServiceModel;

            return paramInfringementGroupServiceModel;
        }

        public static InfringementGroupServiceModel CreateInfringementGroupServiceModel(InfringementGroup infrigementGroupResult)
        {
            return new InfringementGroupServiceModel()
            {
                InfringementGroupCode = infrigementGroupResult.InfringementGroupCode,
                Description = infrigementGroupResult.Description,
                InfrigementOneYear = infrigementGroupResult.InfringementOneYear,
                InfrigementThreeYear = infrigementGroupResult.InfringementThreeYear
            };
        }

        public static InfringementsServiceModel MappInfringement(List<Infringement> paramInfrigement, List<InfringementGroup> resultValueGroups)
        {
            InfringementsServiceModel paramInfringementServiceModel = new InfringementsServiceModel();
            List<InfringementServiceModel> listInfringementServiceModel = new List<InfringementServiceModel>();
            foreach (Infringement paramInfringementBusinessModel in paramInfrigement)
            {
                InfringementServiceModel itemInfringementServiceModel = new InfringementServiceModel();
                itemInfringementServiceModel.InfringementCode = paramInfringementBusinessModel.InfringementCode;
                itemInfringementServiceModel.InfringementPreviousCode = paramInfringementBusinessModel.InfrigementPreviousCode;
                itemInfringementServiceModel.InfringementDescription = paramInfringementBusinessModel.Description;
                itemInfringementServiceModel.InfringementGroupCode = paramInfringementBusinessModel.InfrigemenGroupCode;
                if (resultValueGroups != null && paramInfringementBusinessModel.InfrigemenGroupCode != null)
                {
                    itemInfringementServiceModel.InfringementGroupDescription = resultValueGroups.Find(g => g.InfringementGroupCode == paramInfringementBusinessModel.InfrigemenGroupCode).Description;
                }
                else
                {
                    itemInfringementServiceModel.InfringementGroupDescription = String.Empty;
                }                
                itemInfringementServiceModel.StatusTypeService = StatusTypeService.Original;
                listInfringementServiceModel.Add(itemInfringementServiceModel);
            }

            paramInfringementServiceModel.ErrorDescription = new List<string>();
            paramInfringementServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramInfringementServiceModel.InfringementServiceModel = listInfringementServiceModel;

            return paramInfringementServiceModel;
        }

        public static InfringementServiceModel CreateInfringementServiceModel(Infringement infrigementResult)
        {
            return new InfringementServiceModel()
            {
                InfringementCode = infrigementResult.InfringementCode,
                InfringementDescription = infrigementResult.Description,
                InfringementPreviousCode = infrigementResult.InfrigementPreviousCode,
                InfringementGroupCode = infrigementResult.InfrigemenGroupCode
            };
        }

        public static InfringementStatesServiceModel MappInfringementState(List<InfringementState> paramInfrigementState)
        {
            InfringementStatesServiceModel paramInfringementServiceModel = new InfringementStatesServiceModel();
            List<InfringementStateServiceModel> listInfringementServiceModel = new List<InfringementStateServiceModel>();
            foreach (InfringementState paramInfringementStateBusinessModel in paramInfrigementState)
            {
                InfringementStateServiceModel itemInfringementServiceModel = new InfringementStateServiceModel();
                itemInfringementServiceModel.InfringementStateCode = paramInfringementStateBusinessModel.InfringementStateCode;
                itemInfringementServiceModel.InfringementStateDescription = paramInfringementStateBusinessModel.Description;
                itemInfringementServiceModel.StatusTypeService = StatusTypeService.Original;
                listInfringementServiceModel.Add(itemInfringementServiceModel);
            }

            paramInfringementServiceModel.ErrorDescription = new List<string>();
            paramInfringementServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramInfringementServiceModel.InfringementStateServiceModel = listInfringementServiceModel;

            return paramInfringementServiceModel;
        }

        public static InfringementStateServiceModel CreateInfringementStateServiceModel(InfringementState infrigementStateResult)
        {
            return new InfringementStateServiceModel()
            {
                InfringementStateCode = infrigementStateResult.InfringementStateCode,
                InfringementStateDescription = infrigementStateResult.Description
            };
        }

        public static InfringementGroupsTypeServiceModel MappInfringementGroupType(List<InfringementGroup> paramInfrigementGroup)
        {
            InfringementGroupsTypeServiceModel paramInfringementGroupServiceModel = new InfringementGroupsTypeServiceModel();
            List<InfringementGroupTypeServiceModel> listInfringementGroupServiceModel = new List<InfringementGroupTypeServiceModel>();
            foreach (InfringementGroup paramInfringementGroupBusinessModel in paramInfrigementGroup)
            {
                InfringementGroupTypeServiceModel itemInfringementGroupServiceModel = new InfringementGroupTypeServiceModel();
                itemInfringementGroupServiceModel.InfringementGroupCode = paramInfringementGroupBusinessModel.InfringementGroupCode;
                itemInfringementGroupServiceModel.InfringementGroupDescription = paramInfringementGroupBusinessModel.Description;
                itemInfringementGroupServiceModel.StatusTypeService = StatusTypeService.Original;
                listInfringementGroupServiceModel.Add(itemInfringementGroupServiceModel);
            }
            paramInfringementGroupServiceModel.ErrorDescription = new List<string>();
            paramInfringementGroupServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramInfringementGroupServiceModel.InfringementGroupServiceModel = listInfringementGroupServiceModel;
            return paramInfringementGroupServiceModel;
        }
        
        #region MakeModel
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paramVehicleMake"></param>
        /// <returns></returns>

        public static VehicleModelsServiceModel MappVehicleModel(List<ParamVehicleModel> paramVehicleModel)
        {
            VehicleModelsServiceModel paramVehicleModelServiceModel = new VehicleModelsServiceModel();
            List<VehicleModelServiceModel> listVehicleModelServiceModel = new List<VehicleModelServiceModel>();
            foreach (ParamVehicleModel paramVehicleModelBusinessModel in paramVehicleModel)
            {
                VehicleModelServiceModel itemVehicleModelServiceModel = new VehicleModelServiceModel();
                itemVehicleModelServiceModel.Id = paramVehicleModelBusinessModel.Id;
                itemVehicleModelServiceModel.StatusTypeService = StatusTypeService.Original;
                itemVehicleModelServiceModel.Description = paramVehicleModelBusinessModel.Description;
                itemVehicleModelServiceModel.SmallDescription = paramVehicleModelBusinessModel.SmallDescription;
                itemVehicleModelServiceModel.VehicelMakeServiceQueryModel = new ModelServices.Models.VehicelMakeServiceQueryModel() { Id = paramVehicleModelBusinessModel.VehicleMake.Id, Description = paramVehicleModelBusinessModel.VehicleMake.Description };
                listVehicleModelServiceModel.Add(itemVehicleModelServiceModel);

            }

            paramVehicleModelServiceModel.ErrorDescription = new List<string>();
            paramVehicleModelServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramVehicleModelServiceModel.VehicleModelServiceModel = listVehicleModelServiceModel;


            return paramVehicleModelServiceModel;
        }
        public static VehicleModelServiceModel CreateVehicleModelServiceModel(ParamVehicleModel paramVehicleModelResult)
        {
            return new VehicleModelServiceModel()
            {
                Description = paramVehicleModelResult.Description,
                SmallDescription = paramVehicleModelResult.SmallDescription,
                Id = paramVehicleModelResult.Id,
                VehicelMakeServiceQueryModel = new VehicelMakeServiceQueryModel()
                {
                    Id = paramVehicleModelResult.VehicleMake.Id,
                    Description = paramVehicleModelResult.VehicleMake.Description

                }
            };
        }
        public static List<VehicelMakeServiceQueryModel> CreateVehicelMakeServiceQueryModel (List<ParamVehicleMake> paramVehicleMake)
        
        {
            List<VehicelMakeServiceQueryModel> listResult = new List<VehicelMakeServiceQueryModel>();
            foreach (ParamVehicleMake i in paramVehicleMake)
            {
                VehicelMakeServiceQueryModel model = new VehicelMakeServiceQueryModel();
                model.Id = i.Id;
                model.Description = i.Description;

                listResult.Add(model);
            }

            return listResult;
        }

        #endregion


        #region Fasecolda

        /// <summary>
        /// Mapeo de Modelo
        /// </summary>
        /// <param name="paramMakes"></param>
        /// <returns></returns>
        public static MakesServiceModel MappMakes(List<ParamVehicleMake> paramMakes)
        {
            MakesServiceModel makesServiceModel = new MakesServiceModel();
            List<MakeServiceModel> listMakeServiceModel = new List<MakeServiceModel>();
            foreach (ParamVehicleMake makeBusinessModel in paramMakes)
            {
                MakeServiceModel itemMakeServiceModel = new MakeServiceModel();
                itemMakeServiceModel.Id = makeBusinessModel.Id;
                itemMakeServiceModel.Description = makeBusinessModel.Description;
                itemMakeServiceModel.StatusTypeService = StatusTypeService.Original;
                listMakeServiceModel.Add(itemMakeServiceModel);
            }

            makesServiceModel.ErrorDescription = new List<string>();
            makesServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            makesServiceModel.ListMakesServiceModel = listMakeServiceModel;

            return makesServiceModel;
        }

        /// <summary>
        /// Mapear modelos de negocio ParamModel a modelos de Servicio ModelsServiceModel.
        /// </summary>
        /// <param name="paramModels"></param>
        /// <returns></returns>
        public static ModelsServiceModel MappModels(List<ParamVehicleModel> paramModels)
        {
            ModelsServiceModel modelsServiceModel = new ModelsServiceModel();
            List<ModelServiceModel> listModelServiceModel = new List<ModelServiceModel>();
            foreach (ParamVehicleModel modelBusinessModel in paramModels)
            {
                ModelServiceModel itemModelServiceModel = new ModelServiceModel();
                //itemModelServiceModel.Id = modelBusinessModel.Id;
                itemModelServiceModel.Id = modelBusinessModel.Id;
                itemModelServiceModel.Description = modelBusinessModel.Description;
                itemModelServiceModel.StatusTypeService = StatusTypeService.Original;
                listModelServiceModel.Add(itemModelServiceModel);
            }

            modelsServiceModel.ErrorDescription = new List<string>();
            modelsServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            modelsServiceModel.ListModelServiceModel = listModelServiceModel;

            return modelsServiceModel;
        }


        public static VersionsServiceModel MappVersions(List<ParamVehicleVersion> paramVersions)
        {
            VersionsServiceModel versionsServiceModel = new VersionsServiceModel();
            List<VersionServiceModel> listVersionServiceModel = new List<VersionServiceModel>();
            foreach (ParamVehicleVersion versionBusinessModel in paramVersions)
            {
                VersionServiceModel itemVersionServiceModel = new VersionServiceModel();
                itemVersionServiceModel.Id = versionBusinessModel.Id;
                itemVersionServiceModel.Description = versionBusinessModel.Description;
                itemVersionServiceModel.StatusTypeService = StatusTypeService.Original;
                listVersionServiceModel.Add(itemVersionServiceModel);
            }

            versionsServiceModel.ErrorDescription = new List<string>();
            versionsServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            versionsServiceModel.ListVersionServiceModel = listVersionServiceModel;

            return versionsServiceModel;
        }

        public static VersionVehicleFasecoldasServiceModel MappVVersionVehicleFasecolda(List<ParamVersionVehicleFasecolda> paramVersionVehicleFasecolda)
        {
            VersionVehicleFasecoldasServiceModel versionVehicleFasecolda = new VersionVehicleFasecoldasServiceModel();
            List<VersionVehicleFasecoldaServiceModel> listVersionVehicleFasecoldaServiceModel = new List<VersionVehicleFasecoldaServiceModel>();
            foreach (ParamVersionVehicleFasecolda versionVehicleFasecoldaBusinessModel in paramVersionVehicleFasecolda)
            {
                VersionVehicleFasecoldaServiceModel itemVersionVehicleFasecoldaServiceModel= new VersionVehicleFasecoldaServiceModel();
                itemVersionVehicleFasecoldaServiceModel.VersionId = versionVehicleFasecoldaBusinessModel.versionId;
                itemVersionVehicleFasecoldaServiceModel.ModelId = versionVehicleFasecoldaBusinessModel.modelId;
                itemVersionVehicleFasecoldaServiceModel.MakeId = versionVehicleFasecoldaBusinessModel.makeId;
                itemVersionVehicleFasecoldaServiceModel.FasecoldaModelId= versionVehicleFasecoldaBusinessModel.fasecoldaModelId;
                itemVersionVehicleFasecoldaServiceModel.FasecoldaMakeId = versionVehicleFasecoldaBusinessModel.fasecoldaMakeId;
                
                itemVersionVehicleFasecoldaServiceModel.StatusTypeService = StatusTypeService.Original;
                listVersionVehicleFasecoldaServiceModel.Add(itemVersionVehicleFasecoldaServiceModel);
            }

            versionVehicleFasecolda.ErrorDescription = new List<string>();
            versionVehicleFasecolda.ErrorTypeService = ErrorTypeService.Ok;

            versionVehicleFasecolda.ListVersionVehicleFasecoldaModelService = listVersionVehicleFasecoldaServiceModel;

            return versionVehicleFasecolda;
        }

        public static FasecoldasServiceModel MappVersionVehicleFasecolda(List<ParamFasecolda> paramFasecolda)
        {
            FasecoldasServiceModel fasecoldasServiceModel = new FasecoldasServiceModel();
            List<FasecoldaServiceModel> listFasecoldaServiceModel = new List<FasecoldaServiceModel>();
            foreach (ParamFasecolda fasecoldaBusinessModel in paramFasecolda)
            {
                FasecoldaServiceModel itemFasecoldaServiceModel = new FasecoldaServiceModel();
                if (fasecoldaBusinessModel != null)
                {
                    itemFasecoldaServiceModel.Version = new VersionServiceModel();
                    if (fasecoldaBusinessModel.version.Id > 0)
                    {                    
                        itemFasecoldaServiceModel.Version.Id = fasecoldaBusinessModel.version.Id;
                        itemFasecoldaServiceModel.Version.Description = fasecoldaBusinessModel.version.Description;
                    }


                    if (fasecoldaBusinessModel.model.Id > 0 )
                    {                        
                        itemFasecoldaServiceModel.Model.Id = fasecoldaBusinessModel.model.Id;
                        itemFasecoldaServiceModel.Model.Description = fasecoldaBusinessModel.model.Description;
                    }
                    itemFasecoldaServiceModel.Make = new MakeServiceModel();
                    if (fasecoldaBusinessModel.make.Id > 0 )
                    {                        
                        itemFasecoldaServiceModel.Make.Id = fasecoldaBusinessModel.make.Id;
                        itemFasecoldaServiceModel.Make.Description = fasecoldaBusinessModel.make.Description;
                    }
                    
                    itemFasecoldaServiceModel.FasecoldaModelId = fasecoldaBusinessModel.fasecoldaModelId;
                    itemFasecoldaServiceModel.FasecoldaMakeId = fasecoldaBusinessModel.fasecoldaMakeId;

                    
                    itemFasecoldaServiceModel.StatusTypeService = StatusTypeService.Original;
                    listFasecoldaServiceModel.Add(itemFasecoldaServiceModel);
                }                
            }

            fasecoldasServiceModel.ErrorDescription = new List<string>();
            fasecoldasServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            fasecoldasServiceModel.ListFasecoldaModelService = listFasecoldaServiceModel;

            return fasecoldasServiceModel;
        }

        /// <summary>
        /// Crea el modelo
        /// </summary>
        /// <param name="serviceModel">Modelo de servicio</param>
        /// <returns>Modelo lectura</returns>
        public static Result<ParamVersionVehicleFasecolda, ErrorModel> CreateFasecolda(VersionVehicleFasecoldaServiceModel serviceModel)
        {
            List<string> errorCreateModel = new List<string>();
            Result<ParamVersionVehicleFasecolda, ErrorModel> versionVehicleFasecolda = ParamVersionVehicleFasecolda.CreateParamVersionVehicleFasecolda(serviceModel.VersionId,serviceModel.ModelId,serviceModel.MakeId,serviceModel.FasecoldaModelId,serviceModel.FasecoldaMakeId);
            if (versionVehicleFasecolda is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
            {
                errorCreateModel.AddRange(((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)versionVehicleFasecolda).Message.ErrorDescription);
            }
            
            if (errorCreateModel.Count > 0)
            {
                return new ResultError<ParamVersionVehicleFasecolda, ErrorModel>(ErrorModel.CreateErrorModel(errorCreateModel, Utilities.Enums.ErrorType.BusinessFault, null));
            }
            return ParamVersionVehicleFasecolda.CreateParamVersionVehicleFasecolda(((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)versionVehicleFasecolda).Value.versionId, ((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)versionVehicleFasecolda).Value.modelId, ((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)versionVehicleFasecolda).Value.makeId, ((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)versionVehicleFasecolda).Value.fasecoldaModelId, ((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)versionVehicleFasecolda).Value.fasecoldaMakeId);
        }
        #endregion

        #region VehicleVersionYearServiceModel
        /// <summary>
        /// valores de vehiculo por año
        /// </summary>
        /// <param name="paramVehicleVersionYear">Modelo de negocio valores de vehiculos por año</param>
        /// <returns>modelo de servicio valores de vehiculos por año</returns>
        public static VehicleVersionYearServiceModel CreateVehicleVersionYearServiceModel(ParamVehicleVersionYear paramVehicleVersionYear)
        {
            return new VehicleVersionYearServiceModel()
            {
                CurrencyServiceQueryModel = new ModelServices.Models.UnderwritingParam.CurrencyServiceQueryModel() //moneda
                {
                    Id = paramVehicleVersionYear.ParamCurrency.Id,
                    Description = paramVehicleVersionYear.ParamCurrency.Description
                },
                VehicleMakeServiceQueryModel = new VehicleMakeServiceQueryModel() //marca 
                {
                    Id = paramVehicleVersionYear.ParamVehicleMake.Id,
                    Description = paramVehicleVersionYear.ParamVehicleMake.Description
                },
                VehicleModelServiceQueryModel = new VehicleModelServiceQueryModel() //modelo
                {
                    Id = paramVehicleVersionYear.ParamVehicleModel.Id,
                    Description = paramVehicleVersionYear.ParamVehicleModel.Description
                },
                VehicleVersionServiceQueryModel = new VehicleVersionServiceQueryModel()
                {
                    Id = paramVehicleVersionYear.ParamVehicleVersion.Id,
                    Description = paramVehicleVersionYear.ParamVehicleVersion.Description
                },
                Price = paramVehicleVersionYear.Price,
                Year = paramVehicleVersionYear.Year,
                StatusTypeService = StatusTypeService.Original
            };
        }

        /// <summary>
        /// Listado de valores de vehiculos por año
        /// </summary>
        /// <param name="paramVehicleVersionYears">Modelos de negocio valores de vehiculos por año</param>
        /// <returns>modelos de servicio valores de vehiculos por año</returns>
        public static List<VehicleVersionYearServiceModel> CreateVehicleVersionYearServiceModels(List<ParamVehicleVersionYear> paramVehicleVersionYears)
        {
            List<VehicleVersionYearServiceModel> vehicleVersionYearServiceModel = new List<VehicleVersionYearServiceModel>();
            foreach (var item in paramVehicleVersionYears)
            {
                vehicleVersionYearServiceModel.Add(CreateVehicleVersionYearServiceModel(item));
            }
            return vehicleVersionYearServiceModel;
        }
        #endregion


        #region VehicleVersion
        public static Result<ParamVehicleVersion, ErrorModel> CreateParamVehicleVersion(VehicleVersionServiceModel serviceModel)
        {
            List<string> errorCreateModel = new List<string>();
            if (serviceModel.VehicleModelServiceQueryModel ==null)
            {
                errorCreateModel.Add(Errors.ErrorValidacionParamVehicleModel);
            }
            if (serviceModel.VehicleBodyServiceQueryModel == null)
            {
                errorCreateModel.Add(Errors.ErrorValidacionParamVehicleBody);
            }
            if (serviceModel.VehicleTypeServiceQueryModel == null)
            {
                errorCreateModel.Add(Errors.ErrorValidacionParamVehicleType);
            }
            if (serviceModel.VehicleMakeServiceQueryModel == null)
            {
                errorCreateModel.Add(Errors.ErrorValidacionParamVehicleMake);
            }
            if (serviceModel.CurrencyServiceQueryModel == null)
            {
                errorCreateModel.Add(Errors.ErrorValidacionParamCurrency);
            }
            if (serviceModel.Price == null)
            {
                errorCreateModel.Add(Errors.ErrorValidacionPrice);
            }
            if (errorCreateModel.Count > 0)
            {
                return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(errorCreateModel, Utilities.Enums.ErrorType.BusinessFault, null));
            }
            Result<ParamVehicleVersion, ErrorModel> paramVehicleVersion = ParamVehicleVersion.CreateParamVehicleVersion
                (
                serviceModel.Id,
                serviceModel.Description,
                ((ResultValue<ParamVehicleModel, ErrorModel>)ParamVehicleModel.GetParamVehicleModel(serviceModel.VehicleModelServiceQueryModel.Id, serviceModel.VehicleModelServiceQueryModel.Description, null, null)).Value,
                serviceModel.EngineQuantity,
                ((ResultValue<ParamVehicleMake, ErrorModel>)ParamVehicleMake.GetParamVehicleMake(serviceModel.VehicleMakeServiceQueryModel.Id, serviceModel.VehicleMakeServiceQueryModel.Description)).Value,
                serviceModel.HorsePower,
                serviceModel.Weight,
                serviceModel.TonsQuantity,
                serviceModel.PassengerQuantity,
                serviceModel.VehicleFuelServiceQueryModel==null?null:((ResultValue<ParamVehicleFuel, ErrorModel>)ParamVehicleFuel.GetParamVehicleFuel(serviceModel.VehicleFuelServiceQueryModel.Id, null)).Value,
                ((ResultValue<ParamVehicleBody, ErrorModel>)ParamVehicleBody.GetParamVehicleBody(serviceModel.VehicleBodyServiceQueryModel.Id, null)).Value,
                ((ResultValue<ParamVehicleType, ErrorModel>)ParamVehicleType.GetParamVehicleType(serviceModel.VehicleTypeServiceQueryModel.Id, null)).Value,
                serviceModel.VehicleTransmissionTypeServiceQueryModel == null ? null : ((ResultValue<ParamVehicleTransmissionType, ErrorModel>)ParamVehicleTransmissionType.GetParamVehicleTransmissionType(serviceModel.VehicleTransmissionTypeServiceQueryModel.Id, null)).Value,
                serviceModel.MaxSpeedQuantity,
                serviceModel.DoorQuantity,
                serviceModel.Price,
                serviceModel.IsImported,
                serviceModel.LastModel,
                ((ResultValue<ParamCurrency, ErrorModel>)ParamCurrency.GetParamCurrency(serviceModel.CurrencyServiceQueryModel.Id, null)).Value//serviceModel.CurrencyServiceQueryModel
                );
            if (paramVehicleVersion is ResultError<ParamVehicleVersion, ErrorModel>)
            {
                errorCreateModel.AddRange(((ResultError<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Message.ErrorDescription);
            }
            if (errorCreateModel.Count > 0)
            {
                return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(errorCreateModel, Utilities.Enums.ErrorType.BusinessFault, null));
            }
            return paramVehicleVersion;
        }



        #endregion

    }
        
        }
  
    

        

