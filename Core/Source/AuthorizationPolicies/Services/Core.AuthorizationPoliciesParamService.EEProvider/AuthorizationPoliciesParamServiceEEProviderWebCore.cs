// -----------------------------------------------------------------------
// <copyright file="AuthorizationPoliciesParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Camila Vergara</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider
{
    using Sistran.Core.Application.AuthorizationPoliciesParamService;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider.DAOs;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.Transactions;
    using System.Collections.Generic;
    using System.ServiceModel;
    using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
    using MODUD = Sistran.Core.Application.ModelServices.Models.AuthorizationPolicies;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="AuthorizationPoliciesParamServiceEEProviderWebCore" />
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuthorizationPoliciesParamServiceEEProviderWebCore : IAuthorizationPoliciesParamServiceWebCore
    {
        /// <summary>
        /// CRUD delegaciones
        /// </summary>
        /// <param name="delegationServiceModel">Delegacion MOD-S</param>
        /// <returns>Listado delegaciones de la operacion del CRUD</returns>
        /// <summary>
        public List<MODUD.HierarchyAssociationServiceModel> ExecuteOperationsDelegationServiceModel(List<MODUD.HierarchyAssociationServiceModel> delegationServiceModel)
        {
            List<MODUD.HierarchyAssociationServiceModel> result = new List<MODUD.HierarchyAssociationServiceModel>();
            foreach (var itemSM in delegationServiceModel)
            {
                ParamHierarchyAssociation item = ServicesModelsAssembler.CreateParametrizationDelegation(itemSM);
                MODUD.HierarchyAssociationServiceModel itemResult = new MODUD.HierarchyAssociationServiceModel();
                using (Transaction transaction = new Transaction())
                {
                    switch (itemSM.StatusTypeService)
                    {
                        case MODEN.StatusTypeService.Create:
                            itemResult = this.OperationDelegationServiceModel(item, MODEN.StatusTypeService.Create);
                            break;
                        case MODEN.StatusTypeService.Update:
                            itemResult = this.OperationDelegationServiceModel(item, MODEN.StatusTypeService.Update);
                            break;
                        default:
                            break;
                    }

                    if (itemResult.ErrorServiceModel.ErrorTypeService == MODEN.ErrorTypeService.TechnicalFault)
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
        /// Generar archivo excel de delegaciones
        /// </summary>
        /// <param name="paymentPlans">Listado de delegaciones</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Archivo de excel</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToDelegation(List<MODUD.HierarchyAssociationServiceModel> paymentPlans, string fileName)
        {
            DelegationDAO delegationDAO = new DelegationDAO();
            MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();

            UTMO.Result<string, UTMO.ErrorModel> result = delegationDAO.GenerateFileToDelegation(ServicesModelsAssembler.CreateParametrizationDelegations(paymentPlans), fileName);
            if (result is UTMO.ResultError<string, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<string, UTMO.ErrorModel>).Message;
                excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<string, UTMO.ErrorModel>)
            {
                excelFileServiceModel.FileData = (result as UTMO.ResultValue<string, UTMO.ErrorModel>).Value;
            }

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtener o establece lista de delegados por nombre
        /// </summary>
        /// <returns>Retorna delegados</returns>
        public MODUD.HierarchiesAssociationServiceModel GetDelegationByNameServiceModel(string description)
        {
            DelegationDAO delegationDAO = new DelegationDAO();
            MODUD.HierarchiesAssociationServiceModel delegationServiceModels = new MODUD.HierarchiesAssociationServiceModel();
            UTMO.Result<List<ParamHierarchyAssociation>, UTMO.ErrorModel> result = delegationDAO.GetDelegationByName(description);
            if (result is UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>).Message;
                delegationServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                delegationServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamHierarchyAssociation>, UTMO.ErrorModel>)
            {
                List<ParamHierarchyAssociation> parametrizationDelegation = (result as UTMO.ResultValue<List<ParamHierarchyAssociation>, UTMO.ErrorModel>).Value;

                delegationServiceModels.HierarchyAssociationServiceModel = ModelsServicesAssembler.CreateDeleationsServiceModels(parametrizationDelegation);
                delegationServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return delegationServiceModels;
        }

        /// <summary>
        /// Obtener o establece lista de delegados
        /// </summary>
        /// <returns>Retorna delegados</returns>
        public MODUD.HierarchiesAssociationServiceModel GetDelegationServiceModel()
        {
            DelegationDAO delegationDAO = new DelegationDAO();
            MODUD.HierarchiesAssociationServiceModel delegationServiceModels = new MODUD.HierarchiesAssociationServiceModel();
            UTMO.Result<List<ParamHierarchyAssociation>, UTMO.ErrorModel> result = delegationDAO.GetDelegationAll();
            if (result is UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamHierarchyAssociation>, UTMO.ErrorModel>).Message;
                delegationServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                delegationServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamHierarchyAssociation>, UTMO.ErrorModel>)
            {
                List<ParamHierarchyAssociation> parametrizationDelegation = (result as UTMO.ResultValue<List<ParamHierarchyAssociation>, UTMO.ErrorModel>).Value;

                delegationServiceModels.HierarchyAssociationServiceModel = ModelsServicesAssembler.CreateDeleationsServiceModels(parametrizationDelegation);
                delegationServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return delegationServiceModels;
        }

        /// <summary>
        /// Obtiene lista de jerarquias
        /// </summary>
        /// <returns>Retorna lista de jerarquias</returns>
        public MODUD.HierarchiesServiceQueryModel GetHierarchyServiceModel()
        {
            HierarchyDAO hierarchyDAO = new HierarchyDAO();
            MODUD.HierarchiesServiceQueryModel hierarchiesServiceModels = new MODUD.HierarchiesServiceQueryModel();
            UTMO.Result<System.Collections.Generic.List<Models.ParamHierarchy>, UTMO.ErrorModel> result = hierarchyDAO.GetParametrizationHierarchy();
            if (result is UTMO.ResultError<List<ParamHierarchy>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamHierarchy>, UTMO.ErrorModel>).Message;
                hierarchiesServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                hierarchiesServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamHierarchy>, UTMO.ErrorModel>)
            {
                List<ParamHierarchy> parametrizationHierarchies = (result as UTMO.ResultValue<List<ParamHierarchy>, UTMO.ErrorModel>).Value;
                hierarchiesServiceModels.HierarchyServiceQueryModels = ModelsServicesAssembler.CreateHierarchiesServiceModels(parametrizationHierarchies);
                hierarchiesServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return hierarchiesServiceModels;
        }

        /// <summary>
        /// Obtiene lista de modulos
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public MODUD.ModuleSubmoduleServicesQueryModel GetModuleServiceModel()
        {
            ModuleDAO moduleDAO = new ModuleDAO();
            MODUD.ModuleSubmoduleServicesQueryModel modulesServiceModels = new MODUD.ModuleSubmoduleServicesQueryModel();
            UTMO.Result<System.Collections.Generic.List<Models.ParamModule>, UTMO.ErrorModel> result = moduleDAO.GetParametrizationModules();
            if (result is UTMO.ResultError<List<ParamModule>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamModule>, UTMO.ErrorModel>).Message;
                modulesServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                modulesServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamModule>, UTMO.ErrorModel>)
            {
                List<ParamModule> parametrizationModules = (result as UTMO.ResultValue<List<ParamModule>, UTMO.ErrorModel>).Value;
                modulesServiceModels.ModuleSubModuleQueryModel = ModelsServicesAssembler.CreateModuleSubModuleServiceModels(parametrizationModules);
                modulesServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return modulesServiceModels;
        }

        /// <summary>
        /// Obtiene lista de modulos
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public MODUD.SubModulesServiceQueryModel GetSubModuleForItemIdModuleServiceModel(int idModule)
        {
            SubModuleDAO subModuleDAO = new SubModuleDAO();
            MODUD.SubModulesServiceQueryModel subModulesServiceModels = new MODUD.SubModulesServiceQueryModel();
            UTMO.Result<System.Collections.Generic.List<Models.ParamSubModule>, UTMO.ErrorModel> result = subModuleDAO.GetParametrizationSubModulesForItem(idModule);
            if (result is UTMO.ResultError<List<ParamSubModule>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamSubModule>, UTMO.ErrorModel>).Message;
                subModulesServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                subModulesServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamSubModule>, UTMO.ErrorModel>)
            {
                List<ParamSubModule> parametrizationSubModules = (result as UTMO.ResultValue<List<ParamSubModule>, UTMO.ErrorModel>).Value;
                subModulesServiceModels.SubModuleServiceQueryModels = ModelsServicesAssembler.CreateSubModuleServiceModels(parametrizationSubModules);
                subModulesServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return subModulesServiceModels;
        }

        /// <summary>
        /// Obtiene lista de modulos
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public MODUD.SubModulesServiceQueryModel GetSubModuleServiceModel()
        {
            SubModuleDAO subModuleDAO = new SubModuleDAO();
            MODUD.SubModulesServiceQueryModel subModulesServiceModels = new MODUD.SubModulesServiceQueryModel();
            UTMO.Result<System.Collections.Generic.List<Models.ParamSubModule>, UTMO.ErrorModel> result = subModuleDAO.GetParametrizationSubModules();
            if (result is UTMO.ResultError<List<ParamSubModule>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<ParamSubModule>, UTMO.ErrorModel>).Message;
                subModulesServiceModels.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                subModulesServiceModels.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<ParamSubModule>, UTMO.ErrorModel>)
            {
                List<ParamSubModule> parametrizationSubModules = (result as UTMO.ResultValue<List<ParamSubModule>, UTMO.ErrorModel>).Value;
                subModulesServiceModels.SubModuleServiceQueryModels = ModelsServicesAssembler.CreateSubModuleServiceModels(parametrizationSubModules);
                subModulesServiceModels.ErrorTypeService = MODEN.ErrorTypeService.Ok;
            }

            return subModulesServiceModels;
        }

        /// <summary>
        /// Metodo que obtiene las operaciones a realizar para delegaciones
        /// </summary>
        /// <param name="parametrizationDelegation">Recibe parametrizationDelegation</param>
        /// <param name="statusTypeService">Recibe statusTypeService</param>
        /// <param name="deleteIsPrincipal">Recibe deleteIsPrincipal</param>
        /// <returns>Retorna clauseServiceModelResult</returns>
        public MODUD.HierarchyAssociationServiceModel OperationDelegationServiceModel(ParamHierarchyAssociation parametrizationDelegation, MODEN.StatusTypeService statusTypeService, bool deleteIsPrincipal = false)
        {
            DelegationDAO delagationDAO = new DelegationDAO();
            MODUD.HierarchyAssociationServiceModel delegationServiceModelResult = new MODUD.HierarchyAssociationServiceModel();
            UTMO.Result<ParamHierarchyAssociation, UTMO.ErrorModel> result;
            switch (statusTypeService)
            {
                case MODEN.StatusTypeService.Create:
                    result = delagationDAO.CreateParametrizationDelegation(parametrizationDelegation);
                    break;
                case MODEN.StatusTypeService.Update:
                    result = delagationDAO.UpdateParametrizationDelegation(parametrizationDelegation);
                    break;
                default:
                    result = delagationDAO.CreateParametrizationDelegation(parametrizationDelegation);
                    break;
            }

            if (result is UTMO.ResultError<ParamHierarchyAssociation, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<ParamHierarchyAssociation, UTMO.ErrorModel>).Message;
                delegationServiceModelResult.ErrorServiceModel = new MODPA.ErrorServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType
                };
                delegationServiceModelResult.StatusTypeService = statusTypeService;
            }
            else if (result is UTMO.ResultValue<ParamHierarchyAssociation, UTMO.ErrorModel>)
            {
                ParamHierarchyAssociation parametrizationDelegationResult = (result as UTMO.ResultValue<ParamHierarchyAssociation, UTMO.ErrorModel>).Value;
                delegationServiceModelResult = ModelsServicesAssembler.CreateDelegationServiceModel(parametrizationDelegationResult);

                delegationServiceModelResult.StatusTypeService = statusTypeService;
            }
            return delegationServiceModelResult;
        }

        /// <summary>
        /// The Prueba
        /// </summary>
        /// <returns>The <see cref="string"/></returns>
        public string Prueba()
        {
            return "Hola";
        }

        #region BaseRejectionCauses
        
        /// <summary>
        /// Obtiene lista de modulos
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public Result<List<ParamBaseEjectionCauses>, UTMO.ErrorModel> GetRejectionCausesAll()
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();
            UTMO.Result<System.Collections.Generic.List<Models.ParamBaseEjectionCauses>, UTMO.ErrorModel> result = rejectionCausesDAO.GetRejectionCausesAll();
            
            return result;
        }       

        /// <summary>
        /// Obtiene lista de Declinaciones
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>
        public MODPA.ExcelFileServiceModel GenerateFileToRejectionCause(string fileName)
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();

            Result<List<ParamBaseEjectionCauses>, UTMO.ErrorModel> listRejection = rejectionCausesDAO.GetRejectionCausesAll();
            List<ParamBaseEjectionCauses> paramBaseEjectionCauses = (listRejection as ResultValue<List<ParamBaseEjectionCauses>, ErrorModel>).Value;
            MODPA.ExcelFileServiceModel excelRejectionCause = new MODPA.ExcelFileServiceModel();
            Result<string, UTMO.ErrorModel> result = rejectionCausesDAO.GenerateFileToRejectionCauses(paramBaseEjectionCauses, fileName);
            if (result is UTMO.ResultError<string, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<string, UTMO.ErrorModel>).Message;
                excelRejectionCause.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                excelRejectionCause.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<string, UTMO.ErrorModel>)
            {
                excelRejectionCause.FileData = (result as UTMO.ResultValue<string, UTMO.ErrorModel>).Value;
            }
            return excelRejectionCause;
        }

        /// <summary>
        /// CRUD motivos de rechazo
        /// </summary>
        /// <param name="RejectionCause">Motivos de rechazo MOD-S</param>
        /// <returns>Listado motivos de rechazo  operacion del CRUD</returns>
        /// <summary>
        public Result<List<ParamBaseEjectionCauses>, UTMO.ErrorModel> ExecuteOperationsRejectionCausesServiceModel(List<ParamBaseEjectionCauses> paramBaseEjectionCauses)
        {
            List<ParamBaseEjectionCauses> result = new List<ParamBaseEjectionCauses>();

            return new UTMO.ResultValue<List<ParamBaseEjectionCauses>, UTMO.ErrorModel>(result);
        }

        /// <summary>
        /// Obtiene lista de grupo de politicas
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public Result<List<ParamBaseGroupPolicies>, UTMO.ErrorModel> GetGroupPolicies()
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();
            UTMO.Result<System.Collections.Generic.List<Models.ParamBaseGroupPolicies>, UTMO.ErrorModel> result = rejectionCausesDAO.GetBaseGroupPolicies();           

            return result;
        }

        /// <summary>
        /// Obtiene modelo de negocio para crear Rejection Cause
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public Result<ParamBaseEjectionCauses, UTMO.ErrorModel> CreateBaseRejectionCause(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();
            UTMO.Result<Models.ParamBaseEjectionCauses, UTMO.ErrorModel> result = rejectionCausesDAO.CreateRejectionCause(paramBaseEjectionCauses);

            return result;
        }

        /// <summary>
        /// Obtiene modelo de negocio para crear Rejection Cause
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public Result<ParamBaseEjectionCauses, UTMO.ErrorModel> UpdateBaseRejectionCause(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();
            UTMO.Result<Models.ParamBaseEjectionCauses, UTMO.ErrorModel> result = rejectionCausesDAO.UpdateRejectionCause(paramBaseEjectionCauses);

            return result;
        }

        /// <summary>
        /// Obtiene modelo de negocio para crear Rejection Cause
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public Result<ParamBaseEjectionCauses, UTMO.ErrorModel> DeleteBaseRejectionCause(ParamBaseEjectionCauses paramBaseEjectionCauses)
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();
            UTMO.Result<Models.ParamBaseEjectionCauses, UTMO.ErrorModel> result = rejectionCausesDAO.DeleteRejectionCause(paramBaseEjectionCauses);

            return result;
        }

        /// <summary>
        /// Obtiene lista de modulos
        /// </summary>
        /// <returns>Retorna lista de modulos</returns>
        public Result<List<ParamBaseEjectionCauses>, UTMO.ErrorModel> GetRejectionCausesByDescription(string description , int groupPolicie)
        {
            RejectionCausesDAO rejectionCausesDAO = new RejectionCausesDAO();
            UTMO.Result<System.Collections.Generic.List<Models.ParamBaseEjectionCauses>, UTMO.ErrorModel> result = rejectionCausesDAO.GetBaseGroupPoliciesByDescription(description,groupPolicie);

            return result;   
            
        }


        #endregion



    }
}
