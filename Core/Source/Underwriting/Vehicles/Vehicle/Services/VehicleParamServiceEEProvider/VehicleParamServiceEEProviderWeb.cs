// -----------------------------------------------------------------------
// <copyright file="VehicleParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.EEProvider
{
    using Assemblers;
    using DAOs;
    using ModelServices.Enums;
    using ModelServices.Models.VehicleParam;
    using Sistran.Core.Application.ModelServices.Models;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using Utilities.Error;
    using VehicleParamService.Models;
    using ENUMSM = ModelServices.Enums;

    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class VehicleParamServiceEEProviderWeb : IVehicleParamServiceWeb
    {
        #region INFRAGEMENT GROUPS
        /// <summary>
        /// Consultar todos los grupos de infracciones
        /// </summary>
        /// <returns>Listado de grupo de infraciones (Modelo de servicio)</returns>
        public InfringementGroupsServiceModel GetInfringementGroup()
        {
            InfringementGroupDAO infringementGroupDAO = new InfringementGroupDAO();
            InfringementGroupsServiceModel infringementGroupServiceModel = new InfringementGroupsServiceModel();
            Result<List<InfringementGroup>, ErrorModel> resultGetInfringementGroup = infringementGroupDAO.GetInfringementGroup();
            if (resultGetInfringementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfringementGroup as ResultError<List<InfringementGroup>, ErrorModel>).Message;
                infringementGroupServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementGroupServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<InfringementGroup> resultValue = (resultGetInfringementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;
                resultValue.RemoveAt(0);
                infringementGroupServiceModel = ModelsServicesAssembler.MappInfringementGroup(resultValue);
            }
            return infringementGroupServiceModel;
        }

        public InfringementGroupsServiceModel GetInfringementGroupsByDescription(string description)
        {
            InfringementGroupDAO infringementGroupDAO = new InfringementGroupDAO();
            InfringementGroupsServiceModel infringementGroupServiceModel = new InfringementGroupsServiceModel();
            Result<List<InfringementGroup>, ErrorModel> resultGetInfringementGroup = infringementGroupDAO.GetInfringementGroupsByDescription(description);
            if (resultGetInfringementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfringementGroup as ResultError<List<InfringementGroup>, ErrorModel>).Message;
                infringementGroupServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementGroupServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<InfringementGroup> resultValue = (resultGetInfringementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;
                infringementGroupServiceModel = ModelsServicesAssembler.MappInfringementGroup(resultValue);
            }
            return infringementGroupServiceModel;
        }

        public List<InfringementGroupServiceModel> ExecuteOperationsInfringementGroups(List<InfringementGroupServiceModel> infringementGroupsServiceModel)
        {
            List<InfringementGroupServiceModel> result = new List<InfringementGroupServiceModel>();
            foreach (InfringementGroupServiceModel itemSM in infringementGroupsServiceModel)
            {
                InfringementGroup item = ServicesModelsAssembler.CreateInfringementGroup(itemSM);
                InfringementGroupServiceModel itemResult = new InfringementGroupServiceModel();
                itemResult = this.OperationInfringementGroupServiceModel(item, itemSM.StatusTypeService);
                result.Add(itemResult);
            }
            return result;
        }

        public InfringementGroupServiceModel OperationInfringementGroupServiceModel(InfringementGroup parametrizationPaymentPlan, StatusTypeService statusTypeService, bool deleteIsPrincipal = false)
        {
            InfringementGroupDAO infrigementGroupDAO = new InfringementGroupDAO();
            InfringementGroupServiceModel infrigementGroupServiceModelResult = new InfringementGroupServiceModel()
            {
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                }
            };
            Result<InfringementGroup, ErrorModel> result;
            switch (statusTypeService)
            {
                case StatusTypeService.Create:
                    result = infrigementGroupDAO.CreateInfringementGroup(parametrizationPaymentPlan);
                    break;
                case StatusTypeService.Update:
                    result = infrigementGroupDAO.UpdateInfringementGroup(parametrizationPaymentPlan);
                    break;
                case StatusTypeService.Delete:
                    result = null;
                    // Para el caso de eliminar se utiliza la misma logica, retornando Quotas en 0
                    //result = paymentPlanDAO.DeleteInfringementGroup(parametrizationPaymentPlan, deleteIsPrincipal);
                    break;
                default:
                    result = infrigementGroupDAO.CreateInfringementGroup(parametrizationPaymentPlan);
                    break;
            }

            if (result is ResultError<InfringementGroup, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<InfringementGroup, ErrorModel>).Message;
                infrigementGroupServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infrigementGroupServiceModelResult.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                infrigementGroupServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<InfringementGroup, ErrorModel>)
            {
                InfringementGroup infrigementGroupResult = (result as ResultValue<InfringementGroup, ErrorModel>).Value;
                infrigementGroupServiceModelResult = ModelsServicesAssembler.CreateInfringementGroupServiceModel(infrigementGroupResult);
                infrigementGroupServiceModelResult.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                };
                infrigementGroupServiceModelResult.StatusTypeService = statusTypeService;
            }
            return infrigementGroupServiceModelResult;
        }

        public ExcelFileServiceModel GenerateFileToInfringementGroup(List<InfringementGroupServiceModel> lstInfringementGroupSM, string fileName)
        {
            InfringementGroupDAO fileDAO = new InfringementGroupDAO();
            ExcelFileServiceModel infrigementState = new ExcelFileServiceModel()
            {
                ErrorDescription = new List<string>(),
                ErrorTypeService = ErrorTypeService.Ok
            };
            List<InfringementGroup> lstInfringementGroup = new List<InfringementGroup>();
            foreach (InfringementGroupServiceModel itemSM in lstInfringementGroupSM)
            {
                lstInfringementGroup.Add(ServicesModelsAssembler.CreateInfringementGroup(itemSM));
            }
            Result<string, ErrorModel> result = fileDAO.GenerateFileToInfringementGroup(lstInfringementGroup, fileName);
            if (result is ResultError<string, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<string, ErrorModel>).Message;
                infrigementState.ErrorDescription = errorModelResult.ErrorDescription;
                infrigementState.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else if (result is ResultValue<string, ErrorModel>)
            {
                infrigementState.FileData = (result as ResultValue<string, ErrorModel>).Value;
            }
            return infrigementState;
        }
        #endregion

        #region INFRAGEMENT
        /// <summary>
        /// Consultar todos las infracciones
        /// </summary>
        /// <returns>Listado de infraciones (Modelo de servicio)</returns>
        public InfringementsServiceModel GetInfringement()
        {
            InfringementDAO infringementDAO = new InfringementDAO();
            InfringementsServiceModel infringementServiceModel = new InfringementsServiceModel();
            Result<List<Infringement>, ErrorModel> resultGetInfrigement = infringementDAO.GetInfringement();
            if (resultGetInfrigement is ResultError<List<Infringement>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfrigement as ResultError<List<Infringement>, ErrorModel>).Message;
                infringementServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                InfringementGroupDAO infringementGroupDAO = new InfringementGroupDAO();
                List<InfringementGroup> resultValueGroups = new List<InfringementGroup>();
                Result<List<InfringementGroup>, ErrorModel> resultGetInfrigementGroup = infringementGroupDAO.GetInfringementGroup();
                if (resultGetInfrigementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
                {
                    ErrorModel errorModelResult = (resultGetInfrigementGroup as ResultError<List<InfringementGroup>, ErrorModel>).Message;
                    infringementServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                    infringementServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                }
                else
                {
                    resultValueGroups = (resultGetInfrigementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;
                }
                List<Infringement> resultValue = (resultGetInfrigement as ResultValue<List<Infringement>, ErrorModel>).Value;
                infringementServiceModel = ModelsServicesAssembler.MappInfringement(resultValue, resultValueGroups);
            }
            return infringementServiceModel;
        }

        public InfringementsServiceModel GetInfringementByDescription(string description, string code, int? group)
        {
            InfringementDAO infringementGroupDAO = new InfringementDAO();
            InfringementsServiceModel infringementServiceModel = new InfringementsServiceModel();
            Result<List<Infringement>, ErrorModel> resultGetInfrigement = infringementGroupDAO.GetInfringementByDescription(description, code, group);
            if (resultGetInfrigement is ResultError<List<Infringement>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfrigement as ResultError<List<Infringement>, ErrorModel>).Message;
                infringementServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<Infringement> resultValue = (resultGetInfrigement as ResultValue<List<Infringement>, ErrorModel>).Value;
                infringementServiceModel = ModelsServicesAssembler.MappInfringement(resultValue, null);
            }
            return infringementServiceModel;
        }

        public InfringementGroupsTypeServiceModel GetInfringementGroupType()
        {
            InfringementGroupDAO infringementGroupDAO = new InfringementGroupDAO();
            InfringementGroupsTypeServiceModel infringementGroupServiceModel = new InfringementGroupsTypeServiceModel();
            Result<List<InfringementGroup>, ErrorModel> resultGetInfringementGroup = infringementGroupDAO.GetInfringementGroup();
            if (resultGetInfringementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfringementGroup as ResultError<List<InfringementGroup>, ErrorModel>).Message;
                infringementGroupServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementGroupServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<InfringementGroup> resultValue = (resultGetInfringementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;
                resultValue.RemoveAt(0);
                infringementGroupServiceModel = ModelsServicesAssembler.MappInfringementGroupType(resultValue);
            }
            return infringementGroupServiceModel;
        }

        public InfringementGroupsTypeServiceModel GetInfringementGroupTypeActive()
        {
            InfringementGroupDAO infringementGroupDAO = new InfringementGroupDAO();
            InfringementGroupsTypeServiceModel infringementGroupServiceModel = new InfringementGroupsTypeServiceModel();
            Result<List<InfringementGroup>, ErrorModel> resultGetInfringementGroup = infringementGroupDAO.GetInfringementGroupActive();
            if (resultGetInfringementGroup is ResultError<List<InfringementGroup>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfringementGroup as ResultError<List<InfringementGroup>, ErrorModel>).Message;
                infringementGroupServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementGroupServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<InfringementGroup> resultValue = (resultGetInfringementGroup as ResultValue<List<InfringementGroup>, ErrorModel>).Value;
                //resultValue.RemoveAt(0);
                infringementGroupServiceModel = ModelsServicesAssembler.MappInfringementGroupType(resultValue);
            }
            return infringementGroupServiceModel;
        }

        public List<InfringementServiceModel> ExecuteOperationsInfringement(List<InfringementServiceModel> infrigementsServiceModel)
        {
            List<InfringementServiceModel> result = new List<InfringementServiceModel>();
            foreach (InfringementServiceModel itemSM in infrigementsServiceModel)
            {
                Infringement item = ServicesModelsAssembler.CreateInfringement(itemSM);
                InfringementServiceModel itemResult = new InfringementServiceModel();
                itemResult = this.OperationInfringementServiceModel(item, itemSM.StatusTypeService);
                result.Add(itemResult);
            }
            return result;
        }

        public InfringementServiceModel OperationInfringementServiceModel(Infringement infrigement, StatusTypeService statusTypeService, bool deleteIsPrincipal = false)
        {
            InfringementDAO infrigementDAO = new InfringementDAO();
            InfringementServiceModel infrigementServiceModelResult = new InfringementServiceModel()
            {
                ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                }
            };
            Result<Infringement, ErrorModel> result;
            switch (statusTypeService)
            {
                case StatusTypeService.Create:
                    result = infrigementDAO.CreateInfringement(infrigement);
                    break;
                case StatusTypeService.Update:
                    result = infrigementDAO.UpdateInfringement(infrigement);
                    break;
                case StatusTypeService.Delete:
                    result = null;
                    // Para el caso de eliminar se utiliza la misma logica, retornando Quotas en 0
                    //result = paymentPlanDAO.DeleteInfringementGroup(parametrizationPaymentPlan, deleteIsPrincipal);
                    break;
                default:
                    result = infrigementDAO.CreateInfringement(infrigement);
                    break;
            }

            if (result is ResultError<Infringement, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<Infringement, ErrorModel>).Message;
                infrigementServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infrigementServiceModelResult.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                infrigementServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<Infringement, ErrorModel>)
            {
                Infringement infrigementResult = (result as ResultValue<Infringement, ErrorModel>).Value;
                infrigementServiceModelResult = ModelsServicesAssembler.CreateInfringementServiceModel(infrigementResult);
                infrigementServiceModelResult.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                };
                infrigementServiceModelResult.StatusTypeService = statusTypeService;
            }
            return infrigementServiceModelResult;
        }

        public ExcelFileServiceModel GenerateFileToInfringement(List<InfringementServiceModel> lstInfringementSM, string fileName)
        {
            InfringementDAO fileDAO = new InfringementDAO();
            ExcelFileServiceModel infrigementState = new ExcelFileServiceModel()
            {
                ErrorDescription = new List<string>(),
                ErrorTypeService = ErrorTypeService.Ok
            };
            List<Infringement> lstInfringement = new List<Infringement>();
            foreach (InfringementServiceModel itemSM in lstInfringementSM)
            {
                lstInfringement.Add(ServicesModelsAssembler.CreateInfringement(itemSM));
            }
            Result<string, ErrorModel> result = fileDAO.GenerateFileToInfringement(lstInfringement, fileName);
            if (result is ResultError<string, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<string, ErrorModel>).Message;
                infrigementState.ErrorDescription = errorModelResult.ErrorDescription;
                infrigementState.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else if (result is ResultValue<string, ErrorModel>)
            {
                infrigementState.FileData = (result as ResultValue<string, ErrorModel>).Value;
            }
            return infrigementState;
        }
        #endregion

        #region INFRAGEMENT STATES
        /// <summary>
        /// Consultar todos los grupos de infracciones
        /// </summary>
        /// <returns>Listado de grupo de infraciones (Modelo de servicio)</returns>
        public InfringementStatesServiceModel GetInfringementState()
        {
            InfringementStateDAO infringementStateDAO = new InfringementStateDAO();
            InfringementStatesServiceModel infringementStateServiceModel = new InfringementStatesServiceModel();
            Result<List<InfringementState>, ErrorModel> resultGetInfringementState = infringementStateDAO.GetInfringementState();
            if (resultGetInfringementState is ResultError<List<InfringementState>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfringementState as ResultError<List<InfringementState>, ErrorModel>).Message;
                infringementStateServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementStateServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<InfringementState> resultValue = (resultGetInfringementState as ResultValue<List<InfringementState>, ErrorModel>).Value;
                infringementStateServiceModel = ModelsServicesAssembler.MappInfringementState(resultValue);
            }
            return infringementStateServiceModel;
        }

        public InfringementStatesServiceModel GetInfringementStateByDescription(string description)
        {
            InfringementStateDAO infringementStateDAO = new InfringementStateDAO();
            InfringementStatesServiceModel infringementStateServiceModel = new InfringementStatesServiceModel();
            Result<List<InfringementState>, ErrorModel> resultGetInfringementState = infringementStateDAO.GetInfringementStateByDescription(description);
            if (resultGetInfringementState is ResultError<List<InfringementState>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetInfringementState as ResultError<List<InfringementState>, ErrorModel>).Message;
                infringementStateServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infringementStateServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<InfringementState> resultValue = (resultGetInfringementState as ResultValue<List<InfringementState>, ErrorModel>).Value;
                infringementStateServiceModel = ModelsServicesAssembler.MappInfringementState(resultValue);
            }
            return infringementStateServiceModel;
        }

        public List<InfringementStateServiceModel> ExecuteOperationsInfringementState(List<InfringementStateServiceModel> infringementStatesServiceModel)
        {
            List<InfringementStateServiceModel> result = new List<InfringementStateServiceModel>();
            foreach (InfringementStateServiceModel itemSM in infringementStatesServiceModel)
            {
                InfringementState item = ServicesModelsAssembler.CreateInfringementState(itemSM);
                InfringementStateServiceModel itemResult = new InfringementStateServiceModel();
                itemResult = this.OperationInfringementStateServiceModel(item, itemSM.StatusTypeService);
                result.Add(itemResult);
            }
            return result;
        }

        public InfringementStateServiceModel OperationInfringementStateServiceModel(InfringementState infringementState, StatusTypeService statusTypeService, bool deleteIsPrincipal = false)
        {
            InfringementStateDAO infrigementStateDAO = new InfringementStateDAO();
            InfringementStateServiceModel infrigementStateServiceModelResult = new InfringementStateServiceModel()
            {
                ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                }
            };
            Result<InfringementState, ErrorModel> result;
            switch (statusTypeService)
            {
                case StatusTypeService.Create:
                    result = infrigementStateDAO.CreateInfringementState(infringementState);
                    break;
                case StatusTypeService.Update:
                    result = infrigementStateDAO.UpdateInfringementState(infringementState);
                    break;
                case StatusTypeService.Delete:
                    result = null;
                    // Para el caso de eliminar se utiliza la misma logica, retornando Quotas en 0
                    //result = paymentPlanDAO.DeleteInfringementGroup(parametrizationPaymentPlan, deleteIsPrincipal);
                    break;
                default:
                    result = infrigementStateDAO.CreateInfringementState(infringementState);
                    break;
            }

            if (result is ResultError<InfringementState, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<InfringementState, ErrorModel>).Message;
                infrigementStateServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                infrigementStateServiceModelResult.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                infrigementStateServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<InfringementState, ErrorModel>)
            {
                InfringementState infrigementStateResult = (result as ResultValue<InfringementState, ErrorModel>).Value;
                infrigementStateServiceModelResult = ModelsServicesAssembler.CreateInfringementStateServiceModel(infrigementStateResult);
                infrigementStateServiceModelResult.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                };
                infrigementStateServiceModelResult.StatusTypeService = statusTypeService;
            }
            return infrigementStateServiceModelResult;
        }

        public ExcelFileServiceModel GenerateFileToInfringementState(List<InfringementStateServiceModel> lstInfringementStateSM, string fileName)
        {
            InfringementStateDAO fileDAO = new InfringementStateDAO();
            ExcelFileServiceModel infrigementState = new ExcelFileServiceModel()
            {
                ErrorDescription = new List<string>(),
                ErrorTypeService = ErrorTypeService.Ok
            };
            List<InfringementState> lstInfringementState = new List<InfringementState>();
            foreach (InfringementStateServiceModel itemSM in lstInfringementStateSM)
            {
                lstInfringementState.Add(ServicesModelsAssembler.CreateInfringementState(itemSM));
            }
            Result<string, ErrorModel> result = fileDAO.GenerateFileToInfringementState(lstInfringementState, fileName);
            if (result is ResultError<string, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<string, ErrorModel>).Message;
                infrigementState.ErrorDescription = errorModelResult.ErrorDescription;
                infrigementState.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else if (result is ResultValue<string, ErrorModel>)
            {
                infrigementState.FileData = (result as ResultValue<string, ErrorModel>).Value;
            }
            return infrigementState;
        }

        #endregion

        /// <summary>
        /// Obtiene Listado de Modelos por Marca
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public ModelsServiceModel GetModelsByMakeId(int makeId)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            ModelsServiceModel modelsServiceModel = new ModelsServiceModel();
            Result<List<ParamVehicleModel>, ErrorModel> resultGetModels = vehicleDAO.GetModelsByMakeId(makeId);
            if (resultGetModels is ResultError<List<ParamVehicleModel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetModels as ResultError<List<ParamVehicleModel>, ErrorModel>).Message;
                modelsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                modelsServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleModel> resultValue = (resultGetModels as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                modelsServiceModel = ModelsServicesAssembler.MappModels(resultValue);

            }

            return modelsServiceModel;
        }

        /// <summary>
        /// Obtiene el listado de Marcas
        /// </summary>
        /// <returns>Modelo de servicio de tipo Marca</returns>
        public MakesServiceModel GetMakes()
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            MakesServiceModel makesServiceModel = new MakesServiceModel();
            Result<List<ParamVehicleMake>, ErrorModel> resultGetMakes = vehicleDAO.GetMakes();
            if (resultGetMakes is ResultError<List<ParamVehicleMake>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetMakes as ResultError<List<ParamVehicleMake>, ErrorModel>).Message;
                makesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                makesServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleMake> resultValue = (resultGetMakes as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;
                makesServiceModel = ModelsServicesAssembler.MappMakes(resultValue);
            }
            return makesServiceModel;
        }

        /// <summary>
        /// Obtiene lista de versiones por marca y modelo
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public VersionsServiceModel GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            VersionsServiceModel versionServiceModel = new VersionsServiceModel();
            Result<List<ParamVehicleVersion>, ErrorModel> resultGetVersions = vehicleDAO.GetVersionsByMakeIdModelId(makeId,modelId);
            if (resultGetVersions is ResultError<List<ParamVehicleVersion>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetVersions as ResultError<List<ParamVehicleVersion>, ErrorModel>).Message;
                versionServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                versionServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleVersion> resultValue = (resultGetVersions as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                versionServiceModel = ModelsServicesAssembler.MappVersions(resultValue);

            }

            return versionServiceModel;
        }
        /// <summary>
        /// Obtiene el listado de versiones de vehiculo por codigo fasecolda
        /// </summary>
        /// <param name="fasecoldaId"></param>
        /// <returns></returns>
        public VersionVehicleFasecoldasServiceModel GetVersionVehicleFasecoldaByFasecoldaId(string fasecoldaId)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            VersionVehicleFasecoldasServiceModel VersionVehicleFasecoldaServiceModel = new VersionVehicleFasecoldasServiceModel();
            string fasecoldaMakeId = ((fasecoldaId.ToString().Length > 0) ? fasecoldaId.Substring(0, 3) : string.Empty);
            string fasecoldaModelId =((fasecoldaId.ToString().Length > 0) ? fasecoldaId.Substring(3) : string.Empty);
            Result<List<ParamVersionVehicleFasecolda>, ErrorModel> resultGetVersionVehicleFasecolda = vehicleDAO.GetVersionVehicleFasecoldaByFasecoldaMakeIdByFasecoldaModelId(fasecoldaMakeId, fasecoldaModelId);
            if (resultGetVersionVehicleFasecolda is ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetVersionVehicleFasecolda as ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>).Message;
                VersionVehicleFasecoldaServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                VersionVehicleFasecoldaServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVersionVehicleFasecolda> resultValue = (resultGetVersionVehicleFasecolda as ResultValue<List<ParamVersionVehicleFasecolda>, ErrorModel>).Value;
                VersionVehicleFasecoldaServiceModel = ModelsServicesAssembler.MappVVersionVehicleFasecolda(resultValue);
            }
            return VersionVehicleFasecoldaServiceModel;
        }
        /// <summary>
        /// Obtiene el listado de versiones de vehiculo  fasecolda
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="modelId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public VersionVehicleFasecoldasServiceModel GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            VersionVehicleFasecoldasServiceModel VersionVehicleFasecoldaServiceModel = new VersionVehicleFasecoldasServiceModel();
            Result<List<ParamVersionVehicleFasecolda>, ErrorModel> resultGetVersionVehicleFasecolda = vehicleDAO.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(versionId, modelId, makeId, fasecoldaMakeId, fasecoldaModelId);
            if (resultGetVersionVehicleFasecolda is ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetVersionVehicleFasecolda as ResultError<List<ParamVersionVehicleFasecolda>, ErrorModel>).Message;
                VersionVehicleFasecoldaServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                VersionVehicleFasecoldaServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVersionVehicleFasecolda> resultValue = (resultGetVersionVehicleFasecolda as ResultValue<List<ParamVersionVehicleFasecolda>, ErrorModel>).Value;
                VersionVehicleFasecoldaServiceModel = ModelsServicesAssembler.MappVVersionVehicleFasecolda(resultValue);

            }

            return VersionVehicleFasecoldaServiceModel;
        }

        /// <summary>
        /// Obtiene el listado de versiones de vehiculo  fasecolda
        /// </summary>
        /// <param name="versionId"></param>
        /// <param name="modelId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public FasecoldasServiceModel GetAllVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            FasecoldasServiceModel fasecoldaServiceModel = new FasecoldasServiceModel();
            //Result<List<ParamVersionVehicleFasecolda>, ErrorModel> resultGetVersionVehicleFasecolda = vehicleDAO.GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(versionId, modelId, makeId, fasecoldaMakeId, fasecoldaModelId);
            Result<List<ParamFasecolda>, ErrorModel> resultGetVersionVehicleFasecolda = vehicleDAO.GetAllVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(versionId, modelId, makeId, fasecoldaMakeId, fasecoldaModelId);
            if (resultGetVersionVehicleFasecolda is ResultError<List<ParamFasecolda>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetVersionVehicleFasecolda as ResultError<List<ParamFasecolda>, ErrorModel>).Message;
                fasecoldaServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                fasecoldaServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamFasecolda> resultValue = (resultGetVersionVehicleFasecolda as ResultValue<List<ParamFasecolda>, ErrorModel>).Value;
                fasecoldaServiceModel = ModelsServicesAssembler.MappVersionVehicleFasecolda(resultValue);

            }

            return fasecoldaServiceModel;
        }

        public List<VersionVehicleFasecoldaServiceModel> ExecuteOperationsFasecolda(List<VersionVehicleFasecoldaServiceModel> versionVehicleFasecolda)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            foreach (VersionVehicleFasecoldaServiceModel versionVehicleFasecoldaServiceModelCreate in versionVehicleFasecolda.Where(p => p.StatusTypeService == ENUMSM.StatusTypeService.Create))
            {
                Result<ParamVersionVehicleFasecolda, ErrorModel> paramVersionVehicleFasecolda = ModelsServicesAssembler.CreateFasecolda(versionVehicleFasecoldaServiceModelCreate);
                if (paramVersionVehicleFasecolda is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                {
                    versionVehicleFasecoldaServiceModelCreate.ErrorServiceModel = new ErrorServiceModel()
                    {
                        ErrorDescription = ((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorDescription.Select(p => versionVehicleFasecoldaServiceModelCreate.ToString() + p).ToList(),
                        ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorType
                    };
                }
                else
                {
                    paramVersionVehicleFasecolda = vehicleDAO.Insert((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda);
                    if (paramVersionVehicleFasecolda is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                    {
                        versionVehicleFasecoldaServiceModelCreate.ErrorServiceModel = new ErrorServiceModel()
                        {
                            ErrorDescription = ((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorDescription.Select(p => versionVehicleFasecoldaServiceModelCreate.ToString() + p).ToList(),
                            ErrorTypeService = (ErrorTypeService)((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorType
                        };
                    }
                }
            }

            foreach (VersionVehicleFasecoldaServiceModel versionVehicleFasecoldaServiceModelUpdate in versionVehicleFasecolda.Where(p => p.StatusTypeService == ENUMSM.StatusTypeService.Update))
            {
                Result<ParamVersionVehicleFasecolda, ErrorModel> paramVersionVehicleFasecolda = ModelsServicesAssembler.CreateFasecolda(versionVehicleFasecoldaServiceModelUpdate);
                if (paramVersionVehicleFasecolda is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                {
                    versionVehicleFasecoldaServiceModelUpdate.ErrorServiceModel = new ErrorServiceModel()
                    {
                        ErrorDescription = ((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorDescription.Select(p => versionVehicleFasecoldaServiceModelUpdate.ToString() + p).ToList(),
                        ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorType
                    };
                }
                else
                {
                    paramVersionVehicleFasecolda = vehicleDAO.Update((ResultValue<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda);
                    if (paramVersionVehicleFasecolda is ResultError<ParamVersionVehicleFasecolda, ErrorModel>)
                    {
                        versionVehicleFasecoldaServiceModelUpdate.ErrorServiceModel = new ErrorServiceModel()
                        {
                            ErrorDescription = ((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorDescription.Select(p => versionVehicleFasecoldaServiceModelUpdate.ToString() + p).ToList(),
                            ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<ParamVersionVehicleFasecolda, ErrorModel>)paramVersionVehicleFasecolda).Message.ErrorType
                        };
                    }
                }
            }

            foreach (VersionVehicleFasecoldaServiceModel versionVehicleFasecoldaServiceModelDelete in versionVehicleFasecolda.Where(p => p.StatusTypeService == ENUMSM.StatusTypeService.Delete))
            {
                Result<int, ErrorModel> resultDelete = vehicleDAO.Delete(versionVehicleFasecoldaServiceModelDelete.VersionId);
                if (resultDelete is ResultError<int, ErrorModel>)
                {
                    versionVehicleFasecoldaServiceModelDelete.ErrorServiceModel = new ErrorServiceModel()
                    {
                        ErrorDescription = ((ResultError<int, ErrorModel>)resultDelete).Message.ErrorDescription.Select(p => versionVehicleFasecoldaServiceModelDelete.ToString() + p).ToList(),
                        ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<int, ErrorModel>)resultDelete).Message.ErrorType
                    };
                }
            }

            return versionVehicleFasecolda;
        }

        /// <summary>
        /// Genera el archivo para Fasecolda
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta archivo</returns>
        public ExcelFileServiceModel GenerateFileToVehicleType(string fileName)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            Result<string, ErrorModel> generatedFile = vehicleDAO.GenerateFileToVehicleType(fileName);
            if (generatedFile is ResultError<string, ErrorModel>)
            {
                return new ExcelFileServiceModel()
                {
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<string, ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((ResultError<string, ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new ExcelFileServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok,
                FileData = ((ResultValue<string, ErrorModel>)generatedFile).Value
            };
        }

        /// <summary>
        /// Genera el archivo para Fasecolda
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta archivo</returns>
        public ExcelFileServiceModel GenerateFileToFasecolda(string fileName)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            //Result<string, ErrorModel> generatedFile = vehicleDAO.GenerateFileToVehicleType(fileName);
            Result<string, ErrorModel> generatedFile = vehicleDAO.GenerateFileToFasecolda(fileName);
            if (generatedFile is ResultError<string, ErrorModel>)
            {
                return new ExcelFileServiceModel()
                {
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<string, ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((ResultError<string, ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new ExcelFileServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok,
                FileData = ((ResultValue<string, ErrorModel>)generatedFile).Value
            };
        }

        /// <summary>
        /// Obtiene el valor de los vehiculos 
        /// </summary>
        /// <param name="makeId">id de la marca</param>
        /// <param name="modelId">id del modelo</param>
        /// <param name="versionId">id de la version</param>
        /// <param name="year">año</param>
        /// <returns>listado de valor de vehiculos con id de caracteristicas</returns>
        public VehicleVersionYearsServiceModel GetVehicleVersionYearsSMByMakeIdModelIdVersionIdYear(int? makeId, int? modelId, int? versionId, int? year)
        {
            VehicleVersionYearsServiceModel vehicleVersionYearServiceModels = new VehicleVersionYearsServiceModel();
            VehicleDAO vehicleDAO = new VehicleDAO();
            Result<List<ParamVehicleVersionYear>, ErrorModel> result = vehicleDAO.GetParamVehicleVersionYearsByMakeIdModelIdVersionIdYear(makeId, modelId, versionId, year);
            if (result is ResultError<List<ParamVehicleVersionYear>, ErrorModel>)
            {
                ErrorModel errorModel = (result as ResultError<List<ParamVehicleVersionYear>, ErrorModel>).Message;
                vehicleVersionYearServiceModels.ErrorDescription = errorModel.ErrorDescription;
                vehicleVersionYearServiceModels.ErrorTypeService = (ErrorTypeService)errorModel.ErrorType;
            }
            else if (result is ResultValue<List<ParamVehicleVersionYear>, ErrorModel>)
            {
                vehicleVersionYearServiceModels.VehicleVersionYearServiceModels = ModelsServicesAssembler.CreateVehicleVersionYearServiceModels((result as ResultValue<List<ParamVehicleVersionYear>, ErrorModel>).Value);
                vehicleVersionYearServiceModels.ErrorTypeService = ErrorTypeService.Ok;                
            }
            return vehicleVersionYearServiceModels;
        }

        /// <summary>
        /// Genera archivo excel de valor de vehiculo por año
        /// </summary>
        /// <param name="makeId">id de marca</param>
        /// <param name="modelId">id de modelo</param>
        /// <param name="versionId">id de version</param>
        /// <returns>ruta de excel a exportar</returns>
        public ExcelFileServiceModel GenerateFileToVehicleVersionYear(int makeId, int modelId, int versionId)
        {
            VehicleDAO vehicleDAO = new VehicleDAO();
            ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel();            
            Result<string, ErrorModel> result = vehicleDAO.GenerateFileToVehicleVersionYear(makeId, modelId, versionId);
            if (result is ResultError<string, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<string, ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is ResultValue<string, ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as ResultValue<string, ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }


        #region VehicleVersion
        public VehicleVersionServiceModel ExecuteOperationVehicleVersion(VehicleVersionServiceModel VehicleVersionServiceModel)
        {
            VehicleVersionDAO vehicleVersionDAO = new VehicleVersionDAO();
            
            StatusTypeService Action = VehicleVersionServiceModel.StatusTypeService;
            if (StatusTypeService.Create== VehicleVersionServiceModel.StatusTypeService)
            {
                Result<List<ParamVehicleVersion>, ErrorModel> ResultGetID = vehicleVersionDAO.GetParamVehicleVersion();
                if (ResultGetID is ResultError<List<ParamVehicleVersion>, ErrorModel>)
                {
                    ErrorModel errorModelResult = (ResultGetID as ResultError<List<ParamVehicleVersion>, ErrorModel>).Message;
                    VehicleVersionServiceModel.ErrorServiceModel = new ErrorServiceModel();
                    VehicleVersionServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                    VehicleVersionServiceModel.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                    VehicleVersionServiceModel.StatusTypeService = StatusTypeService.Error;
                    return VehicleVersionServiceModel;
                }
                else
                {
                    VehicleVersionServiceModel.Id = (ResultGetID as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value.Max(p => p.Id) + 1;
                }
            }
            Result<ParamVehicleVersion, ErrorModel> paramVehicleVersion = ModelsServicesAssembler.CreateParamVehicleVersion(VehicleVersionServiceModel);
            if (paramVehicleVersion is ResultError<ParamVehicleVersion, ErrorModel>)
            {
                VehicleVersionServiceModel.StatusTypeService = StatusTypeService.Error;
                VehicleVersionServiceModel.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = ((ResultError<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Message.ErrorDescription.Select(p => VehicleVersionServiceModel.ToString() + p).ToList(),
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Message.ErrorType
                };
                return VehicleVersionServiceModel;
            }
            Result<ParamVehicleVersion, ErrorModel> result;

            switch (VehicleVersionServiceModel.StatusTypeService)
            {
                case StatusTypeService.Create:
                    result = vehicleVersionDAO.CreateParamVehicleVersion(((ResultValue<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Value);
                    break;
                case StatusTypeService.Update:
                    result = vehicleVersionDAO.UpdateParamVehicleVersion(((ResultValue<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Value);
                    break;
                default:
                    result = vehicleVersionDAO.CreateParamVehicleVersion(((ResultValue<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Value);
                    break;
            }
            if (result is ResultError<ParamVehicleVersion, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamVehicleVersion, ErrorModel>).Message;
                VehicleVersionServiceModel.ErrorServiceModel = new ErrorServiceModel();
                VehicleVersionServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                VehicleVersionServiceModel.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                VehicleVersionServiceModel.StatusTypeService = StatusTypeService.Error;
            }
            else
            {
                VehicleVersionServiceModel = ServicesModelsAssembler.CreateVehicleVersionServiceModel(((ResultValue<ParamVehicleVersion, ErrorModel>)result).Value);
                VehicleVersionServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                };
                VehicleVersionServiceModel.StatusTypeService = Action;
            }
            return VehicleVersionServiceModel;
        }
        public VehicleVersionsServiceModel GetVehicleVersionByDescription(string description)
        {
            VehicleVersionsServiceModel result = new VehicleVersionsServiceModel();
            VehicleVersionDAO vehicleVersionDAO = new VehicleVersionDAO();
            Result<List<ParamVehicleVersion>, ErrorModel> ResultDAO = vehicleVersionDAO.GetParamVehicleVersionByDescription(description);
            if (ResultDAO is ResultError<List<ParamVehicleVersion>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleVersion>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleVersion> resultValue = (ResultDAO as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                result.VehicleVersionServiceModel = ServicesModelsAssembler.CreateVehicleVersionsServiceModel(resultValue);
                result.ErrorTypeService = ErrorTypeService.Ok;
            }
            return result;
        }      
        public VehicleVersionServiceModel DeleteVehicleVersion(VehicleVersionServiceModel VehicleVersionServiceModel)
        {
            VehicleVersionDAO vehicleVersionDAO = new VehicleVersionDAO();

            Result<ParamVehicleVersion, ErrorModel> paramVehicleVersion = ModelsServicesAssembler.CreateParamVehicleVersion(VehicleVersionServiceModel);
            if (paramVehicleVersion is ResultError<ParamVehicleVersion, ErrorModel>)
            {
                VehicleVersionServiceModel.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = ((ResultError<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Message.ErrorDescription.Select(p => p).ToList(),
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Message.ErrorType
                };
                return VehicleVersionServiceModel;
            }
            Result<int, ErrorModel> result = vehicleVersionDAO.DeleteParamVehicleVersion(((ResultValue<ParamVehicleVersion, ErrorModel>)paramVehicleVersion).Value);
            if (result is ResultError<int, ErrorModel>)
            {
                VehicleVersionServiceModel.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = ((ResultError<int, ErrorModel>)result).Message.ErrorDescription.ToList(),
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<int, ErrorModel>)result).Message.ErrorType
                };
                VehicleVersionServiceModel.StatusTypeService = StatusTypeService.Error;
                return VehicleVersionServiceModel;
            }
            return VehicleVersionServiceModel;
        }       
        public VehicleMakesServiceQueryModel GetVehicleMake()
        {
            VehicleMakeDAO makeDAO = new VehicleMakeDAO();
            Result<List<ParamVehicleMake>, ErrorModel> ResultDAO = makeDAO.GetVehicleMakes();
            VehicleMakesServiceQueryModel result = new VehicleMakesServiceQueryModel();
            if (ResultDAO is ResultError<List<ParamVehicleMake>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleMake>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleMake> resultValue = (ResultDAO as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;
                result.VehicleMakeServiceQueryModel = ServicesModelsAssembler.CreateVehicleMakeServiceQueryModel(resultValue);
            }

            return result;
        }
        public VehicleModelsServiceQueryModel GetVehicleModelByMake(int MakeID)
        {
            VehicleModelDAO modelDAO = new VehicleModelDAO();
            VehicleModelsServiceQueryModel result = new VehicleModelsServiceQueryModel();
            Result<List<ParamVehicleModel>, ErrorModel> ResultDAO = modelDAO.GetParamVehicleModelsByMake(MakeID);
            if (ResultDAO is ResultError<List<ParamVehicleModel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleModel>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleModel> resultValue = (ResultDAO as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                result.VehicleModelServiceQueryModel = ServicesModelsAssembler.CreateVehicleModelServiceQueryModel(resultValue);
            }
            return result;
        }
        public VehicleFuelsServiceQueryModel GetVehicleFuel()
        {
            VehicleFuelDAO fuelDAO = new VehicleFuelDAO();
            VehicleFuelsServiceQueryModel result = new VehicleFuelsServiceQueryModel();
            Result<List<ParamVehicleFuel>, ErrorModel> ResultDAO = fuelDAO.GetParamVehicleFuel();
            if (ResultDAO is ResultError<List<ParamVehicleFuel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleFuel>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleFuel> resultValue = (ResultDAO as ResultValue<List<ParamVehicleFuel>, ErrorModel>).Value;
                result.VehicleFuelServiceQueryModel = ServicesModelsAssembler.CreateVehicleFuelQueryModelServiceModel(resultValue);
            }
            return result;
        }
        public VehicleTypesServiceQueryModel GetVehicleType()
        {
            VehicleTypeDAO typeDAO = new VehicleTypeDAO();
            VehicleTypesServiceQueryModel result = new VehicleTypesServiceQueryModel();
            Result<List<ParamVehicleType>, ErrorModel> ResultDAO = typeDAO.GetParamVehicleType();
            if (ResultDAO is ResultError<List<ParamVehicleType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleType>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleType> resultValue = (ResultDAO as ResultValue<List<ParamVehicleType>, ErrorModel>).Value;
                result.VehicleTypeServiceQueryModel = ServicesModelsAssembler.CreateVehicleTypeQueryModelServiceModel(resultValue);
            }
            return result;
        }
        public VehicleBodysServiceQueryModel GetVehicleBody()
        {
            VehicleBodyDAO bodyDAO = new VehicleBodyDAO();
            VehicleBodysServiceQueryModel result = new VehicleBodysServiceQueryModel();
            Result<List<ParamVehicleBody>, ErrorModel> ResultDAO = bodyDAO.GetParamVehicleBody();
            if (ResultDAO is ResultError<List<ParamVehicleBody>, ErrorModel>)
            {
            }
            else
            {
                List<ParamVehicleBody> resultValue = (ResultDAO as ResultValue<List<ParamVehicleBody>, ErrorModel>).Value;
                result.VehicleBodyServiceQueryModel = ServicesModelsAssembler.CreateVehicleBodyQueryModelServiceModel(resultValue);
                result.ErrorTypeService = ErrorTypeService.Ok;
            }
            return result;
        }
        public VehicleTransmissionTypesServiceQueryModel GetVehicleTransmissionType()
        {
            VehicleTransmissionTypeDAO transmissionDAO = new VehicleTransmissionTypeDAO();
            VehicleTransmissionTypesServiceQueryModel result = new VehicleTransmissionTypesServiceQueryModel();
            Result<List<ParamVehicleTransmissionType>, ErrorModel> ResultDAO = transmissionDAO.GetParamVehicleTransmissionType();
            if (ResultDAO is ResultError<List<ParamVehicleTransmissionType>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleTransmissionType>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleTransmissionType> resultValue = (ResultDAO as ResultValue<List<ParamVehicleTransmissionType>, ErrorModel>).Value;
                result.VehicleTransmissionTypeServiceQueryModel = ServicesModelsAssembler.CreateVehicleTransmissionTypeQueryModelServiceModel(resultValue);
                result.ErrorTypeService = ErrorTypeService.Ok;
            }
            return result;
        }
        public CurrenciesServiceQueryModel GetCurreny()
        {
            CurrencyDAO currencyDAO = new CurrencyDAO();
            CurrenciesServiceQueryModel result = new CurrenciesServiceQueryModel();
            Result<List<ParamCurrency>, ErrorModel> ResultDAO = currencyDAO.GetParamCurrency();
            if (ResultDAO is ResultError<List<ParamCurrency>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamCurrency>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamCurrency> resultValue = (ResultDAO as ResultValue<List<ParamCurrency>, ErrorModel>).Value;
                result.CurrencyServiceModel = ServicesModelsAssembler.CreateCurrencyQueryModelServiceModel(resultValue);
                result.ErrorTypeService = ErrorTypeService.Ok;
            }
            return result;
        }
        public VehicleVersionsServiceModel GetAdvanzedSearchVehicleVersion(VehicleVersionServiceModel VehicleVersionServiceModel)
        {
            VehicleVersionsServiceModel result = new VehicleVersionsServiceModel();
            VehicleVersionDAO vehicleDAO = new VehicleVersionDAO();
            Result<List<ParamVehicleVersion>, ErrorModel> ResultDAO = vehicleDAO.GetParamVehicleVersionByMakeModelVersion(VehicleVersionServiceModel.VehicleMakeServiceQueryModel.Id, VehicleVersionServiceModel.VehicleModelServiceQueryModel.Id, VehicleVersionServiceModel.Description);
            if (ResultDAO is ResultError<List<ParamVehicleVersion>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleVersion>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleVersion> resultValue = (ResultDAO as ResultValue<List<ParamVehicleVersion>, ErrorModel>).Value;
                result.VehicleVersionServiceModel = ServicesModelsAssembler.CreateVehicleVersionsServiceModel(resultValue);
                result.ErrorTypeService = ErrorTypeService.Ok;
            }
            return result;
        }
        public VehicleModelsServiceQueryModel GetVehicleModel()
        {
            VehicleModelDAO modelDAO = new VehicleModelDAO();
            VehicleModelsServiceQueryModel result = new VehicleModelsServiceQueryModel();
            Result<List<ParamVehicleModel>, ErrorModel> ResultDAO = modelDAO.GetParamVehicleModels();
            if (ResultDAO is ResultError<List<ParamVehicleModel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (ResultDAO as ResultError<List<ParamVehicleModel>, ErrorModel>).Message;
                result.ErrorDescription = errorModelResult.ErrorDescription;
                result.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleModel> resultValue = (ResultDAO as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                result.VehicleModelServiceQueryModel = ServicesModelsAssembler.CreateVehicleModelServiceQueryModel(resultValue);
            }
            return result;
        }
        public ExcelFileServiceModel GenerateFileToVehicleVersion(string fileName,int? makeCode, int? modelCode)
        {
            VehicleVersionDAO vehicleDAO = new VehicleVersionDAO();
            Result<string, ErrorModel> generatedFile = vehicleDAO.GenerateFileToVehicleVersion(fileName, makeCode, modelCode);
            if (generatedFile is ResultError<string, ErrorModel>)
            {
                return new ExcelFileServiceModel()
                {
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<string, ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((ResultError<string, ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new ExcelFileServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok,
                FileData = ((ResultValue<string, ErrorModel>)generatedFile).Value
            };
        }
        public ExcelFileServiceModel GenerateFileToVehicleVersion(string fileName)
        {
            VehicleVersionDAO vehicleDAO = new VehicleVersionDAO();
            Result<string, ErrorModel> generatedFile = vehicleDAO.GenerateFileToVehicleVersion(fileName);
            if (generatedFile is ResultError<string, ErrorModel>)
            {
                return new ExcelFileServiceModel()
                {
                    ErrorTypeService = (ENUMSM.ErrorTypeService)((ResultError<string, ErrorModel>)generatedFile).Message.ErrorType,
                    ErrorDescription = ((ResultError<string, ErrorModel>)generatedFile).Message.ErrorDescription
                };
            }

            return new ExcelFileServiceModel()
            {
                ErrorTypeService = ENUMSM.ErrorTypeService.Ok,
                FileData = ((ResultValue<string, ErrorModel>)generatedFile).Value
            };
        }
        #endregion

        #region MAKEMODEL

        public List<VehicleModelServiceModel> ExecuteOperationVehicleModel(List<VehicleModelServiceModel> VehicleModelServiceModel)
        {
            List<VehicleModelServiceModel> result = new List<VehicleModelServiceModel>();
            foreach (VehicleModelServiceModel itemSM in VehicleModelServiceModel)
            {
                ParamVehicleModel item = ServicesModelsAssembler.CreateParamVehicleModel(itemSM);
                VehicleModelServiceModel itemResult = new VehicleModelServiceModel();
                itemResult = this.OperationVehicleModelServiceModel(item, itemSM.StatusTypeService);
                result.Add(itemResult);
            }
            return result;
        }

        public List<VehicelMakeServiceQueryModel> GetVehicelMake()
        {
            VehicleMakeDAO vehicleMakeTypeDAO = new VehicleMakeDAO();
            List<VehicelMakeServiceQueryModel> vehicleMakesServiceModel = new List<VehicelMakeServiceQueryModel>();
            Result<List<ParamVehicleMake>, ErrorModel> resultGetParamVehicleMakeMethod = vehicleMakeTypeDAO.GetVehicleMakes();
            if (resultGetParamVehicleMakeMethod is ResultError<List<ParamVehicleMake>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetParamVehicleMakeMethod as ResultError<List<ParamVehicleMake>, ErrorModel>).Message;
            }
            else
            {
                List<ParamVehicleMake> resultValue = (resultGetParamVehicleMakeMethod as ResultValue<List<ParamVehicleMake>, ErrorModel>).Value;


                vehicleMakesServiceModel = ModelsServicesAssembler.CreateVehicelMakeServiceQueryModel(resultValue);
            }
            return vehicleMakesServiceModel;
        }
        public VehicleModelsServiceModel GetInVehicleModelByDescription(string description, int makeid)
        {
            VehicleModelDAO VehiculoModelGroupDAO = new VehicleModelDAO();
            VehicleModelsServiceModel vehicleModelServiceModel = new VehicleModelsServiceModel();
            Result<List<ParamVehicleModel>, ErrorModel> resultGetVehiculoModel = VehiculoModelGroupDAO.GetVehicleModelByDescription(description, makeid);
            if (resultGetVehiculoModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetVehiculoModel as ResultError<List<ParamVehicleModel>, ErrorModel>).Message;
                vehicleModelServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                vehicleModelServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleModel> resultValue = (resultGetVehiculoModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                vehicleModelServiceModel = ModelsServicesAssembler.MappVehicleModel(resultValue);
            }
            return vehicleModelServiceModel;
        }
        private VehicleModelServiceModel OperationVehicleModelServiceModel(ParamVehicleModel paramVehicleModelMethod, StatusTypeService statusTypeService)
        {
            VehicleModelDAO paymentMethodDAO = new VehicleModelDAO();
            VehicleModelServiceModel methodServiceModelResult = new VehicleModelServiceModel()
            {
                ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ErrorTypeService.Ok
                }
            };

            Result<ParamVehicleModel, ErrorModel> result;
            switch (statusTypeService)
            {
                case StatusTypeService.Create:
                    result = paymentMethodDAO.ParamVehicleModelCreate(paramVehicleModelMethod);
                    break;
                case StatusTypeService.Update:
                    result = paymentMethodDAO.paramVehicleModelUpdate(paramVehicleModelMethod);
                    break;
                case StatusTypeService.Delete:
                    result = paymentMethodDAO.paramVehicleModelDelete(paramVehicleModelMethod);
                    break;
                default:
                    result = paymentMethodDAO.ParamVehicleModelCreate(paramVehicleModelMethod);
                    break;
            }

            if (result is ResultError<ParamVehicleModel, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamVehicleModel, ErrorModel>).Message;
                methodServiceModelResult.Description = paramVehicleModelMethod.Description;
                methodServiceModelResult.Id = paramVehicleModelMethod.Id;
                methodServiceModelResult.SmallDescription = paramVehicleModelMethod.SmallDescription;
                methodServiceModelResult.VehicelMakeServiceQueryModel = new VehicelMakeServiceQueryModel() { Id = paramVehicleModelMethod.Id };
                methodServiceModelResult.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                methodServiceModelResult.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                methodServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<ParamVehicleModel, ErrorModel>)
            {
                ParamVehicleModel parametrizationModelVehicleResult = (result as ResultValue<ParamVehicleModel, ErrorModel>).Value;
                methodServiceModelResult = ModelsServicesAssembler.CreateVehicleModelServiceModel(parametrizationModelVehicleResult);
                methodServiceModelResult.StatusTypeService = statusTypeService;

            }

            return methodServiceModelResult;
        }
        public VehicleModelsServiceModel GetAdvanASearchVehicleModel(VehicleModelServiceModel paramVehicleModel)
        {
            VehicleModelDAO vehicleModelDAO = new VehicleModelDAO();
            VehicleModelsServiceModel ModelServiceModel = new VehicleModelsServiceModel();
            Result<List<ParamVehicleModel>, ErrorModel> resultList = vehicleModelDAO.GetVehicleModelByDescription(paramVehicleModel.Description, paramVehicleModel.VehicelMakeServiceQueryModel.Id);
            if (resultList is ResultError<List<ParamVehicleModel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultList as ResultError<List<ParamVehicleModel>, ErrorModel>).Message;
                ModelServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                ModelServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleModel> resultValue = (resultList as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                ModelServiceModel = ModelsServicesAssembler.MappVehicleModel(resultValue);
            }

            return ModelServiceModel;
        }
    
    
        public VehicleModelServiceModel DeleteVehicleModel(VehicleModelServiceModel vehicleModelServiceModel)
        {
            VehicleModelDAO VehiculoModelGroupDAO = new VehicleModelDAO();
            ParamVehicleModel paramVehicleModel = ServicesModelsAssembler.CreateParamVehicleModel(vehicleModelServiceModel);
            

            Result<ParamVehicleModel, ErrorModel> result = VehiculoModelGroupDAO.paramVehicleModelDelete(paramVehicleModel);
            if (result is ResultError<ParamVehicleModel, ErrorModel>)
            {
                vehicleModelServiceModel.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = ((ResultError<ParamVehicleModel, ErrorModel>)result).Message.ErrorDescription.ToList(),
                    ErrorTypeService = (ErrorTypeService)((ResultError<ParamVehicleModel, ErrorModel>)result).Message.ErrorType
                };
                vehicleModelServiceModel.StatusTypeService = StatusTypeService.Error;
                return vehicleModelServiceModel;
            }
            return vehicleModelServiceModel;            
        }



        

        public ExcelFileServiceModel GenerateFileToVehicleModel(List<VehicleModelServiceModel> VehicleModels, string fileName)
        {
            VehicleModelDAO vehicleModelDAO = new VehicleModelDAO();
            ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel();
            List<ParamVehicleModel> list = new List<ParamVehicleModel>();
            foreach(VehicleModelServiceModel ITEM in  VehicleModels)
            {
                list.Add(ServicesModelsAssembler.CreateParamVehicleModel(ITEM));
            }

            Result<string, ErrorModel> result = vehicleModelDAO.GenerateFileToVehicleModel(list, fileName);
            if (result is ResultError<string, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<string, ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is ResultValue<string, ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as ResultValue<string, ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }


        public VehicleModelsServiceModel GetInVehicleModels()
        {
            VehicleModelDAO VehiculoModelGroupDAO = new VehicleModelDAO();
            VehicleModelsServiceModel vehicleModelServiceModel = new VehicleModelsServiceModel();
            Result<List<ParamVehicleModel>, ErrorModel> resultGetVehiculoModel = VehiculoModelGroupDAO.GetParamVehicleModels();
            if (resultGetVehiculoModel is ResultError<List<ParamVehicleModel>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultGetVehiculoModel as ResultError<List<ParamVehicleModel>, ErrorModel>).Message;
                vehicleModelServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                vehicleModelServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamVehicleModel> resultValue = (resultGetVehiculoModel as ResultValue<List<ParamVehicleModel>, ErrorModel>).Value;
                vehicleModelServiceModel = ModelsServicesAssembler.MappVehicleModel(resultValue);
            }
            return vehicleModelServiceModel;
        }



        #endregion
    }
}