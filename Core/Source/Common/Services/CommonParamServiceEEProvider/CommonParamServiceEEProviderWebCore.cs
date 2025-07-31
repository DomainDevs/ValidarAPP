// -----------------------------------------------------------------------
// <copyright file="CommonParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService
{
    using System.Collections.Generic;
    using System.ServiceModel;
    using Sistran.Core.Application.CommonParamService.Assemblers;
    using Sistran.Core.Application.CommonParamService.DAOs;
    using Sistran.Core.Application.CommonParamService.Models;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using PARCPSM = Sistran.Core.Application.ModelServices.Models.CommonParam;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using MODSM = Sistran.Core.Application.ModelServices.Models.Param;
    using PARUPSM = ModelServices.Models.CommonParam;
    using System;
    using Framework.BAF;
    using Utilities.Managers;
    using CommonService.Models;
    using CommonParamService;
    using CommonParamService.EEProvider.DAOs;
    using Sistran.Core.Application.ModelServices.Models.Common;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using UnderwritingParamService.EEProvider.Business;
    using Framework.Transactions;
    using UniquePerson.Entities;

    /// <summary>
    /// Clase que implementa la interfaz ICommonParamServiceWeb.
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CommonParamServiceEEProviderWebCore : ICommonParamServiceWebCore
    {
        /// <summary>
        /// Obtiene lista de parametros
        /// </summary>
        /// <param name="ltsParameterSMo">Modelo ParamParameter</param>
        /// <returns>Retorna modelo ParamParameter</returns>
        public List<ParameterServiceModel> GetParameter(List<ParameterServiceModel> ltsParameterSMo)
        {
            DAOs.ParameterDAO parameterDAO = new DAOs.ParameterDAO();
            List<ParamParameter> lstParamParameter = new List<ParamParameter>();
            List<ParameterServiceModel> lstParameterServiceModel = new List<ParameterServiceModel>();
            lstParamParameter = ServicesModelsAssembler.CreateParamParameters(ltsParameterSMo);
            UTMO.Result<List<ParamParameter>, UTMO.ErrorModel> result = parameterDAO.GetParameter(lstParamParameter);
            if (result is UTMO.ResultError<List<ParamParameter>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamParameter>, UTMO.ErrorModel>).Message;
                lstParameterServiceModel[0].ParametricServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                lstParameterServiceModel[0].ParametricServiceModel.ErrorServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                List<ParamParameter> resultValue = (result as UTMO.ResultValue<List<ParamParameter>, UTMO.ErrorModel>).Value;
                lstParameterServiceModel = ModelServiceAssembler.MappParameters(resultValue).ParameterServiceModel;
                for (int i = 0; i < lstParameterServiceModel.Count; i++)
                {
                    lstParameterServiceModel[i].ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
                    lstParameterServiceModel[i].ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
                    lstParameterServiceModel[i].ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
                    lstParameterServiceModel[i].ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ENUMSM.ErrorTypeService.Ok;
                    lstParameterServiceModel[i].ParametricServiceModel.StatusTypeService = ENUMSM.StatusTypeService.Original;
                }
            }

            return lstParameterServiceModel;
        }

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="parameterServiceModel">Objeto de ParamParameter</param>
        /// <returns>Retorna objeto de ParamParameter</returns>
        public List<ParameterServiceModel> ExecuteOperationsParameterServiceModel(List<ParameterServiceModel> parameterServiceModel)
        {
            List<ParameterServiceModel> result = new List<ParameterServiceModel>();
            foreach (var itemLRS in parameterServiceModel)
            {
                UTMO.Result<ParamParameter, UTMO.ErrorModel> item = ServicesModelsAssembler.CreateParamParameter(itemLRS);
                Business.DiscontinuityLogBusiness discontinuityLogBusiness = new Business.DiscontinuityLogBusiness();
                Business.InfringementLogBusiness infringementLogBusiness = new Business.InfringementLogBusiness();

                UTMO.Result<ParamDiscontinuityLog, UTMO.ErrorModel> itemDiscontinuityLog = ServicesModelsAssembler.CreateParamDiscontinuityLog(discontinuityLogBusiness.ValidateDiscontinuityLog(itemLRS.DiscontinuityLogServiceModel));
                ParamDiscontinuityLog paramDiscontinuityLog = (itemDiscontinuityLog as UTMO.ResultValue<ParamDiscontinuityLog, UTMO.ErrorModel>).Value;

                UTMO.Result<ParamInfringementLog, UTMO.ErrorModel> itemInfringementLog = ServicesModelsAssembler.CreateParamInfringementLog(infringementLogBusiness.ValidateDiscontinuityLog(itemLRS.InfringementLogServiceModel));
                ParamInfringementLog paramInfringementLog = (itemInfringementLog as UTMO.ResultValue<ParamInfringementLog, UTMO.ErrorModel>).Value;

                ParamParameter paramParameter = (item as UTMO.ResultValue<ParamParameter, UTMO.ErrorModel>).Value;
                ParameterServiceModel itemResult = new ParameterServiceModel();
                using (Framework.Transactions.Transaction transaction = new Framework.Transactions.Transaction())
                {
                    switch (itemLRS.ParametricServiceModel.StatusTypeService)
                    {
                        case ENUMSM.StatusTypeService.Create:
                            itemResult = this.OperationParameterServiceModel(paramParameter, paramDiscontinuityLog, paramInfringementLog, ENUMSM.StatusTypeService.Create);
                            break;
                        case ENUMSM.StatusTypeService.Update:
                            itemResult = this.OperationParameterServiceModel(paramParameter, paramDiscontinuityLog, paramInfringementLog, ENUMSM.StatusTypeService.Update);
                            break;
                        default:
                            break;
                    }

                    if (itemResult.ParametricServiceModel.ErrorServiceModel.ErrorTypeService == ErrorTypeService.TechnicalFault ||
                        itemResult.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService == ErrorTypeService.TechnicalFault ||
                        itemResult.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService == ErrorTypeService.TechnicalFault)
                    {
                        transaction.Dispose();
                    }
                    else
                    {
                        transaction.Complete();
                    }
                }

                result.Add(itemResult);
            }

            return result;
        }

        /// <summary>
        /// Llamado a DAOS respectivos para operacion del CRUD y operacion con result
        /// </summary>
        /// <param name="paramParameter">Modelo paramParameter</param>
        /// <param name="paramDiscontinuityLog">Modelo ParamDiscontinuityLog</param>
        /// <param name="paramInfringementLog">Modelo ParamInfringementLog</param>
        /// <param name="statusTypeService">Estado de tipo servicio</param>
        /// <returns>Retorna modelo ParameterServiceModel</returns>
        public ParameterServiceModel OperationParameterServiceModel(ParamParameter paramParameter, ParamDiscontinuityLog paramDiscontinuityLog, ParamInfringementLog paramInfringementLog, StatusTypeService statusTypeService)
        {
            DAOs.ParameterDAO parameterDAO = new DAOs.ParameterDAO();
            DAOs.DiscontinuityLogDAO discontinuityLogDAO = new DAOs.DiscontinuityLogDAO();
            DAOs.InfringementLogDAO infringementLogDAO = new DAOs.InfringementLogDAO();
            ParameterServiceModel parameterServiceModelResult = new ParameterServiceModel();
            parameterServiceModelResult.ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
            parameterServiceModelResult.ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
            parameterServiceModelResult.ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
            parameterServiceModelResult.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            parameterServiceModelResult.DiscontinuityLogServiceModel = new DiscontinuityLogServiceModel();
            parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
            parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
            parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
            parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            parameterServiceModelResult.InfringementLogServiceModel = new InfringementLogServiceModel();
            parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
            parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
            parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
            parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            Result<ParamParameter, ErrorModel> result;
            Result<ParamDiscontinuityLog, ErrorModel> resultParamDiscontinuityLog;
            Result<ParamInfringementLog, ErrorModel> resultParamParamInfringementLog;

            result = parameterDAO.UpdateParamParameter(paramParameter);

            if (paramParameter.ParameterId == 1008)
            {
                resultParamParamInfringementLog = infringementLogDAO.CreateParamInfringementLog(paramInfringementLog);

                if (resultParamParamInfringementLog is ResultError<ParamInfringementLog, ErrorModel>)
                {
                    ErrorModel errorModelResult = (resultParamParamInfringementLog as ResultError<ParamInfringementLog, ErrorModel>).Message;

                    parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                    parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                    parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.StatusTypeService = statusTypeService;

                    return parameterServiceModelResult;
                }
            }

            if (paramParameter.ParameterId == 1009)
            {
                resultParamDiscontinuityLog = discontinuityLogDAO.CreateParamDiscontinuityLo(paramDiscontinuityLog);

                if (resultParamDiscontinuityLog is ResultError<ParamDiscontinuityLog, ErrorModel>)
                {
                    ErrorModel errorModelResult = (resultParamDiscontinuityLog as ResultError<ParamDiscontinuityLog, ErrorModel>).Message;

                    parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                    parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                    parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.StatusTypeService = statusTypeService;

                    return parameterServiceModelResult;
                }
            }

            if (result is ResultError<ParamParameter, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamParameter, ErrorModel>).Message;
                parameterServiceModelResult.ParameterId = paramParameter.ParameterId;
                parameterServiceModelResult.ParametricServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                parameterServiceModelResult.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                parameterServiceModelResult.ParametricServiceModel.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<ParamParameter, ErrorModel>)
            {
                ParamParameter paramparameterResult = (result as ResultValue<ParamParameter, ErrorModel>).Value;
                parameterServiceModelResult = ModelServiceAssembler.MappParameter(paramparameterResult);
                parameterServiceModelResult.ParametricServiceModel.StatusTypeService = StatusTypeService.Update;
                parameterServiceModelResult.DiscontinuityLogServiceModel.ParametricServiceModel.StatusTypeService = StatusTypeService.Create;
                parameterServiceModelResult.InfringementLogServiceModel.ParametricServiceModel.StatusTypeService = StatusTypeService.Create;

            }

            return parameterServiceModelResult;
        }
        #region Branches
        /// <summary>
        /// CRUD de sucursales
        /// </summary>
        /// <param name="branchServiceModel">sucursales MOD-S</param>
        /// <returns>Listado de sucursales producto de la operacion del CRUD</returns>
        public List<PARUPSM.BranchServiceModel> ExecuteOperationsBranchServiceModel(List<PARUPSM.BranchServiceModel> branchServiceModel)
        {
            BranchBusiness branchBusiness = new BranchBusiness();
            List<PARUPSM.BranchServiceModel> result = new List<PARUPSM.BranchServiceModel>();
            foreach (var itemSM in branchServiceModel)
            {
                ParamCoBranch item = ServicesModelsAssembler.CreateParamCoBranches(itemSM);

                if (itemSM.StatusTypeService == ENUMSM.StatusTypeService.Delete || branchBusiness.ValidateLengthDescription(item.Branch.Description))
                {
                    PARUPSM.BranchServiceModel itemResult = new PARUPSM.BranchServiceModel();
                    using (Transaction transaction = new Transaction())
                    {
                        switch (itemSM.StatusTypeService)
                        {
                            case ENUMSM.StatusTypeService.Create:
                                itemResult = this.ExecuteOperationsBranch(item, ENUMSM.StatusTypeService.Create);
                                break;
                            case ENUMSM.StatusTypeService.Update:
                                itemResult = this.ExecuteOperationsBranch(item, ENUMSM.StatusTypeService.Update);
                                break;
                            case ENUMSM.StatusTypeService.Delete:
                                itemResult = this.ExecuteOperationsBranch(item, ENUMSM.StatusTypeService.Delete);
                                break;
                            case ENUMSM.StatusTypeService.Original:
                                itemResult = new PARUPSM.BranchServiceModel() { StatusTypeService = ENUMSM.StatusTypeService.Original, ErrorServiceModel = new MODSM.ErrorServiceModel { ErrorTypeService = ENUMSM.ErrorTypeService.Ok } };
                                break;
                            default:
                                break;
                        }

                        if (itemResult.ErrorServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.TechnicalFault)
                        {
                            transaction.Dispose();
                        }
                        else
                        {
                            transaction.Complete();
                        }
                    }

                    result.Add(itemResult);
                }
                else
                {
                    itemSM.ErrorServiceModel = new MODSM.ErrorServiceModel()
                    {
                        ErrorTypeService = ENUMSM.ErrorTypeService.BusinessFault,
                        ErrorDescription = new List<string>() { Resources.Errors.FailedUpdatingBranchErrorBD }
                    };
                    result.Add(itemSM);
                }
            }

            return result;
        }

        /// <summary>
        /// CRUD de sucursales
        /// </summary>       
        /// <param name="branchParam">modelo de negocio de sucursales</param>
        /// <param name="statusTypeService">estatus de modelos</param>
        /// <returns>si se actualizo correctamente </returns>
        public PARUPSM.BranchServiceModel ExecuteOperationsBranch(ParamCoBranch branchParam, ENUMSM.StatusTypeService statusTypeService)
        {
            BranchDAO branchDAO = new BranchDAO();
            PARUPSM.BranchServiceModel branchsServiceModel = new PARUPSM.BranchServiceModel()
            {
                ErrorServiceModel = new MODSM.ErrorServiceModel()
                {
                    ErrorDescription = new List<string>(),
                    ErrorTypeService = ENUMSM.ErrorTypeService.Ok
                }
            };

            Result<ParamCoBranch, ErrorModel> result;
            switch (statusTypeService)
            {
                case ENUMSM.StatusTypeService.Create:
                    result = branchDAO.CreateCoBranch(branchParam);
                    break;
                case ENUMSM.StatusTypeService.Update:
                    result = branchDAO.UpdateCoBranch(branchParam);
                    break;
                case ENUMSM.StatusTypeService.Delete:
                    result = branchDAO.DeleteCoBranch(branchParam);
                    result = branchDAO.DeleteBranch(branchParam);
                    break;
                default:
                    result = branchDAO.CreateCoBranch(branchParam);
                    break;
            }

            if (result is ResultError<ParamCoBranch, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamCoBranch, ErrorModel>).Message;
                branchsServiceModel.ErrorServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                branchsServiceModel.ErrorServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                branchsServiceModel.StatusTypeService = statusTypeService;
            }
            else if (result is ResultValue<ParamCoBranch, ErrorModel>)
            {
                ParamCoBranch paraCoBranch = (result as ResultValue<ParamCoBranch, ErrorModel>).Value;
                branchsServiceModel = ModelServiceAssembler.CreateBranchServiceModel(paraCoBranch);
                branchsServiceModel.StatusTypeService = statusTypeService;

                if (branchsServiceModel.ErrorServiceModel.ErrorDescription == null)
                {
                    branchsServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
                }
            }

            return branchsServiceModel;
        }

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="paramParameter">Objeto de ParamParameter</param>
        /// <returns>Objeto de ParamParameter</returns>
        public PARCPSM.BranchesServiceQueryModel GetBranch()
        {
            BranchDAO branchDAO = new BranchDAO();
            PARCPSM.BranchesServiceQueryModel branchesServiceModel = new PARCPSM.BranchesServiceQueryModel();
            UTMO.Result<List<ParamBranch>, UTMO.ErrorModel> result = branchDAO.GetBranch();
            if (result is UTMO.ResultError<List<ParamBranch>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamBranch>, UTMO.ErrorModel>).Message;
                branchesServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                branchesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamBranch>, UTMO.ErrorModel>)
            {
                List<ParamBranch> branch = (result as UTMO.ResultValue<List<ParamBranch>, UTMO.ErrorModel>).Value;
                branchesServiceModel.BranchServiceQueryModel = ModelServiceAssembler.CreateBranchServiceQueryModels(branch);
            }

            return branchesServiceModel;
        }

        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="paramParameter">Objeto de ParamParameter</param>
        /// <returns>Objeto de ParamParameter</returns>
        public PARCPSM.BranchesServicesModel GetBranches()
        {
            BranchDAO branchDAO = new BranchDAO();
            PARCPSM.BranchesServicesModel branchesServiceModel = new PARCPSM.BranchesServicesModel();
            UTMO.Result<List<ParamCoBranch>, UTMO.ErrorModel> result = branchDAO.GetBranches();
            if (result is UTMO.ResultError<List<ParamCoBranch>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamCoBranch>, UTMO.ErrorModel>).Message;
                branchesServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                branchesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamCoBranch>, UTMO.ErrorModel>)
            {
                List<ParamCoBranch> branch = (result as UTMO.ResultValue<List<ParamCoBranch>, UTMO.ErrorModel>).Value;
                branchesServiceModel.BranchServiceModel = ModelServiceAssembler.CreateBranchesServicesModel(branch);
            }

            return branchesServiceModel;
        }

        /// <summary>
        /// Generar archivo excel de sucursal
        /// </summary>
        /// <param name="branchServiceModel">Listado de sucursal</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODSM.ExcelFileServiceModel GenerateFileToBranch(List<PARUPSM.BranchServiceQueryModel> branchServiceModel, string fileName)
        {
            BranchDAO branchDAO = new BranchDAO();
            MODSM.ExcelFileServiceModel excelFileServiceModel = new MODSM.ExcelFileServiceModel();

            UTMO.Result<string, UTMO.ErrorModel> result = branchDAO.GenerateFileToBranch(ServicesModelsAssembler.CreateParametrizationBranches(branchServiceModel), fileName);
            if (result is UTMO.ResultError<string, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<string, UTMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<string, UTMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTMO.ResultValue<string, UTMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtener lista de sucursal
        /// </summary>
        /// <param name="description">Descripcion de sucursal</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de sucursal</returns>
        public PARUPSM.BranchesServiceQueryModel GetBranchesByDescription(string description)
        {
            BranchDAO branchDAO = new BranchDAO();
            PARUPSM.BranchesServiceQueryModel branchesServiceModel = new PARUPSM.BranchesServiceQueryModel();
            UTMO.Result<List<ParamBranch>, UTMO.ErrorModel> result = branchDAO.GetBranchsByDescription(description);
            if (result is UTMO.ResultError<List<ParamBranch>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamBranch>, UTMO.ErrorModel>).Message;
                branchesServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                branchesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamBranch>, UTMO.ErrorModel>)
            {
                List<ParamBranch> branch = (result as UTMO.ResultValue<List<ParamBranch>, UTMO.ErrorModel>).Value;
                branchesServiceModel.BranchServiceQueryModel = ModelServiceAssembler.CreateBranchServiceQueryModels(branch);
            }

            return branchesServiceModel;
        }

        /// <summary>
        /// Se consultan las CO-sucursales por descripcion
        /// </summary>
        /// <param name="description">descripcion</param>
        /// <returns>retorna las CO-sucursales</returns>
        public PARCPSM.BranchesServicesModel GetCoBranchesByDescription(string description)
        {
            BranchDAO branchDAO = new BranchDAO();
            PARCPSM.BranchesServicesModel branchesServiceModel = new PARCPSM.BranchesServicesModel();
            UTMO.Result<List<ParamCoBranch>, UTMO.ErrorModel> result = branchDAO.GetBranchesByDescription(description);
            if (result is UTMO.ResultError<List<ParamCoBranch>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamCoBranch>, UTMO.ErrorModel>).Message;
                branchesServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                branchesServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamCoBranch>, UTMO.ErrorModel>)
            {
                List<ParamCoBranch> branch = (result as UTMO.ResultValue<List<ParamCoBranch>, UTMO.ErrorModel>).Value;
                branchesServiceModel.BranchServiceModel = ModelServiceAssembler.CreateBranchesServicesModel(branch);
            }

            return branchesServiceModel;
        }

        /// <summary>
        /// Validaciones dependencias entidad sucursales
        /// </summary>
        /// <param name="branchId">Codigo desucursal</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        public int ValidateBranch(int branchId)
        {
            try
            {
                BranchDAO branchDAO = new BranchDAO();
                return branchDAO.ValidateBranch(branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, CommonParamService.Resources.Errors.ErrorGettingBranch), ex);
            }
        }
        #endregion

        #region SalePoint
        /// <summary>
        /// Actualiza los parametros
        /// </summary>
        /// <param name="paramParameter">Objeto de ParamParameter</param>
        /// <returns>Objeto de ParamParameter</returns>
        public PARCPSM.SalePointsServiceModel GetSalePointes()
        {
            SalePointDAO salePointDAO = new SalePointDAO();
            PARCPSM.SalePointsServiceModel salePointsServiceModel = new PARCPSM.SalePointsServiceModel();
            UTMO.Result<List<ParamSalePoint>, UTMO.ErrorModel> result = salePointDAO.GetSalePointes();
            if (result is UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>).Message;
                salePointsServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                salePointsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>)
            {
                List<ParamSalePoint> salepoint = (result as UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>).Value;
                salePointsServiceModel.SalePointServiceModel = ModelServiceAssembler.CreateSalePointServiceModel(salepoint);
            }

            return salePointsServiceModel;
        }

        /// <summary>
        /// Generar archivo excel de puntos de venta
        /// </summary>
        /// <param name="salePointServiceModel">Listado de puntos de venta</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODSM.ExcelFileServiceModel GenerateFileToSalePoint(List<PARUPSM.SalePointServiceModel> salePointServiceModel, string fileName)
        {
            SalePointDAO salePointDAO = new SalePointDAO();
            MODSM.ExcelFileServiceModel excelFileServiceModel = new MODSM.ExcelFileServiceModel();

            UTMO.Result<string, UTMO.ErrorModel> result = salePointDAO.GenerateFileToSalePoint(ServicesModelsAssembler.CreateParametrizationSalePointes(salePointServiceModel), fileName);
            if (result is UTMO.ResultError<string, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<string, UTMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<string, UTMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTMO.ResultValue<string, UTMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtener lista de ountos de venta
        /// </summary>
        /// <param name="description">Descripcion de puntos de venta</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de los puntois de venta</returns>
        public PARUPSM.SalePointsServiceModel GetSalePointServiceModel(string description)
        {
            SalePointDAO salePointDAO = new SalePointDAO();
            PARUPSM.SalePointsServiceModel salePointsServiceModel = new PARUPSM.SalePointsServiceModel();
            UTMO.Result<List<ParamSalePoint>, UTMO.ErrorModel> result = salePointDAO.GetSalePointsByDescription(description);
            if (result is UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>).Message;
                salePointsServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                salePointsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>)
            {
                List<ParamSalePoint> salePoint = (result as UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>).Value;
                salePointsServiceModel.SalePointServiceModel = ModelServiceAssembler.CreateSalePointServiceModel(salePoint);
            }

            return salePointsServiceModel;
        }

        /// <summary>
        /// Obtener lista de ountos de venta
        /// </summary>
        /// <param name="description">Descripcion de puntos de venta</param>
        /// <returns>Plan de pago MOD-S resultado de consulta de los puntois de venta</returns>
        public PARUPSM.SalePointsServiceModel GetSalePointsByBranchCode(int branchId)
        {
            SalePointDAO salePointDAO = new SalePointDAO();
            PARUPSM.SalePointsServiceModel salePointsServiceModel = new PARUPSM.SalePointsServiceModel();
            UTMO.Result<List<ParamSalePoint>, UTMO.ErrorModel> result = salePointDAO.GetSalePointsByBranchCode(branchId);
            if (result is UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>).Message;
                salePointsServiceModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                salePointsServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>)
            {
                List<ParamSalePoint> salePoint = (result as UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>).Value;
                salePointsServiceModel.SalePointServiceModel = ModelServiceAssembler.CreateSalePointServiceModel(salePoint);
            }

            return salePointsServiceModel;
        }

        /// <summary>
        /// Validaciones dependencias entidad de puntos de venta
        /// </summary>
        /// <param name="salePointId">Codigo de punto de venta</param>
        /// <returns>1: tiene dependencias, 0: sin dependencias</returns>
        public int ValidateSalePoint(int salePointId, int branchId)
        {
            try
            {
                SalePointDAO salePointDAO = new SalePointDAO();
                return salePointDAO.ValidateSalePoint(salePointId , branchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, CommonParamService.Resources.Errors.ErrorGettingSalePoint), ex);
            }
        }
        #endregion

        #region Country State City
        /// <summary>
        /// Obtiene los paises
        /// </summary>
        /// <returns>Lista de paises</returns>
        public PARUPSM.CountriesServiceQueryModel GetCountries()
        {
            CountryDAO ratingZoneDao = new CountryDAO();
            PARUPSM.CountriesServiceQueryModel countryServiceQueryModels = new PARUPSM.CountriesServiceQueryModel();
            UTMO.Result<List<Country>, UTMO.ErrorModel> result = ratingZoneDao.GetCountries();

            if (result is UTMO.ResultError<List<Country>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<Country>, UTMO.ErrorModel>).Message;
                countryServiceQueryModels.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                countryServiceQueryModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<Country>, UTMO.ErrorModel>)
            {
                List<Country> countries = (result as UTMO.ResultValue<List<Country>, UTMO.ErrorModel>).Value;
                countryServiceQueryModels.Counties = ModelServiceAssembler.CreateCountryServiceQueryModel(countries);
            }

            return countryServiceQueryModels;
        }

        /// <summary>
        /// Obtiene los estados por pais
        /// </summary>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        public PARUPSM.StatesServiceQueryModel GetStatesByCountry(int idCountry)
        {
            CountryDAO ratingZoneDao = new CountryDAO();
            PARUPSM.StatesServiceQueryModel statesServiceQueryModel = new PARUPSM.StatesServiceQueryModel();
            UTMO.Result<List<State>, UTMO.ErrorModel> result = ratingZoneDao.GetStatesByCountry(idCountry);

            if (result is UTMO.ResultError<List<State>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<State>, UTMO.ErrorModel>).Message;
                statesServiceQueryModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                statesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<State>, UTMO.ErrorModel>)
            {
                List<State> countries = (result as UTMO.ResultValue<List<State>, UTMO.ErrorModel>).Value;
                statesServiceQueryModel.States = ModelServiceAssembler.CreateStatesServiceQueryModel(countries);
            }

            return statesServiceQueryModel;
        }

        /// <summary>
        /// Obtiene las cuidades por pais y estado
        /// </summary>
        /// <param name="idState">Identificador del estado</param>
        /// <param name="idCountry">identificador del pais</param>
        /// <returns>lista de estados</returns>
        public PARUPSM.CitiesServiceRelationModel GetCitiesByStateCountry(int idState, int idCountry)
        {
            CountryDAO ratingZoneDao = new CountryDAO();
            PARUPSM.CitiesServiceRelationModel citiesServiceRelationModel = new PARUPSM.CitiesServiceRelationModel();
            UTMO.Result<List<City>, UTMO.ErrorModel> result = ratingZoneDao.GetCitiesByStateCountry(idState, idCountry);

            if (result is UTMO.ResultError<List<City>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<City>, UTMO.ErrorModel>).Message;
                citiesServiceRelationModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                citiesServiceRelationModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<City>, UTMO.ErrorModel>)
            {
                List<City> cities = (result as UTMO.ResultValue<List<City>, UTMO.ErrorModel>).Value;
                citiesServiceRelationModel.Cities = ModelServiceAssembler.CreateCityServiceRelationModels(cities);
            }

            return citiesServiceRelationModel;
        }
        #endregion

        #region Common data
        /// <summary>
        /// Obtiene los tipo de teléfono
        /// </summary>
        /// <returns>Lista de tipo de teléfono</returns>
        public PARUPSM.PhonesTypesServiceQueryModel GetPhoneType()
        {
            CommonDataDAO commonDataDAO = new CommonDataDAO();
            PARUPSM.PhonesTypesServiceQueryModel phonesTypesServiceQueryModel = new PARUPSM.PhonesTypesServiceQueryModel();
            UTMO.Result<List<PhoneType>, UTMO.ErrorModel> result = commonDataDAO.GetPhoneType();

            if (result is UTMO.ResultError<List<PhoneType>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<PhoneType>, UTMO.ErrorModel>).Message;
                phonesTypesServiceQueryModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                phonesTypesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<PhoneType>, UTMO.ErrorModel>)
            {
                List<PhoneType> phoneType = (result as UTMO.ResultValue<List<PhoneType>, UTMO.ErrorModel>).Value;
                phonesTypesServiceQueryModel.PhonesTypes = ModelServiceAssembler.CreatePhoneTypeServiceQueryModel(phoneType);
            }

            return phonesTypesServiceQueryModel;
        }

        /// <summary>
        /// Obtiene los tipos de direcciones
        /// </summary>
        /// <returns>Lista de tipos de direcciones</returns>
        public PARUPSM.AddressTypesServiceQueryModel GetAddressType()
        {
            CommonDataDAO commonDataDAO = new CommonDataDAO();
            PARUPSM.AddressTypesServiceQueryModel addressTypesServiceQueryModel = new PARUPSM.AddressTypesServiceQueryModel();
            UTMO.Result<List<AddressType>, UTMO.ErrorModel> result = commonDataDAO.GetAddressType();

            if (result is UTMO.ResultError<List<AddressType>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<AddressType>, UTMO.ErrorModel>).Message;
                addressTypesServiceQueryModel.ErrorTypeService = (ENUMSM.ErrorTypeService)errorModelResult.ErrorType;
                addressTypesServiceQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<AddressType>, UTMO.ErrorModel>)
            {
                List<AddressType> addressType = (result as UTMO.ResultValue<List<AddressType>, UTMO.ErrorModel>).Value;
                addressTypesServiceQueryModel.AddressTypesService = ModelServiceAssembler.CreateAddressTypeServiceQueryModel(addressType);
            }

            return addressTypesServiceQueryModel;
        }
        #endregion

        #region vehicle concessionaire
        public List<ParamVehicleConcessionaire> GetVehicleConcessionaires()
        {
            VehicleConcessionaireDAO concessionaireDAO = new VehicleConcessionaireDAO();
            return concessionaireDAO.GetVehicleConcessionaires();
        }
        #endregion
    }
}
