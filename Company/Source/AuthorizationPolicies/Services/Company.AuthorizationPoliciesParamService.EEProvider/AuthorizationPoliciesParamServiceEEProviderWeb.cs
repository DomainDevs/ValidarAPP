// -----------------------------------------------------------------------
// <copyright file="AuthorizationPoliciesParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.AuthorizationPoliciesParamService.EEProvider
{
    using global::Company.AuthorizationPoliciesParamServices.Models;
    using Sistran.Company.Application.AuthorizationPoliciesParamService;
    using System.Collections.Generic;
    using coreProvider = Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider;
    using coreModel = Sistran.Core.Application.AuthorizationPoliciesParamService.Models;
    using System.ServiceModel;
    using Sistran.Core.Application.Utilities.Error;
    using global::Company.AuthorizationPoliciesParamService.EEProvider.CoreAssemblers;
    using global::Company.AuthorizationPoliciesParamService.EEProvider.ServicesAssemblers;
    using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Company.Application.ModelServices.Enums;
    using Sistran.Company.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.EEProvider;
    using Sistran.Core.Framework.Transactions;
    using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.AuthorizationPoliciesParamService.Models;

    /// <summary>
    /// Defines the <see cref="AuthorizationPoliciesParamServiceEEProviderWeb" />
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuthorizationPoliciesParamServiceEEProviderWeb : AuthorizationPoliciesParamServiceEEProviderWebCore, IAuthorizationPoliciesParamServiceWeb
    {
        /// <summary>
        /// Obtiene lista de Declinaciones
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>
        public RejectionCausesServiceModel CompanyGetRejectionCauses()
        {
            RejectionCausesServiceModel rejectionCauseServiceModel = new RejectionCausesServiceModel();

            coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore providerCore = new coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore();
            Result<List<coreModel.ParamBaseEjectionCauses>, ErrorModel> resultCoreParamBaseEjectionCauses = providerCore.GetRejectionCausesAll();

            if (resultCoreParamBaseEjectionCauses is ResultError<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultCoreParamBaseEjectionCauses as ResultError<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>).Message;
                rejectionCauseServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                rejectionCauseServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (resultCoreParamBaseEjectionCauses is ResultValue<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>)
            {
                List<coreModel.ParamBaseEjectionCauses> ejectionCausesCore = (resultCoreParamBaseEjectionCauses as ResultValue<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>).Value;
                List<CompanyParamBaseEjectionCauses> ejectionCausesCompany = CoreCompanyAssembler.CreateCoreBaseEjectionCauses(ejectionCausesCore);
                rejectionCauseServiceModel.RejectionCauseServiceModel = ModelServiceAssember.CreateBaseEjectionCauses(ejectionCausesCompany);
                rejectionCauseServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            }

            return rejectionCauseServiceModel;
        }

        /// <summary>
        /// Obtiene archivo excel para exportar
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>        
        public ExcelFileServiceModel CompanyGenerateFileToRejectionCause(string fileName)
        {
           coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore providerCore = new coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore();
           ExcelFileServiceModel excelFileServiceModel = new ExcelFileServiceModel();

           excelFileServiceModel = CoreCompanyAssembler.ExcelFileServiceModel(providerCore.GenerateFileToRejectionCause(fileName));

            return excelFileServiceModel;
        }

        /// <summary>
        /// Obtiene lista de grupo de politicas
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>
        public GenericModelsServicesQueryModel CompanyGetGroupPolicies()
        {
            List<GenericModelsServicesQueryModel> rejectionCausesServiceModel = new List<GenericModelsServicesQueryModel>();
            GenericModelsServicesQueryModel genericModelsServicesQueryModel = new GenericModelsServicesQueryModel();

            coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore providerCore = new coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore();
            Result<List<coreModel.ParamBaseGroupPolicies>, ErrorModel> resultCoreParamBaseEjectionCauses = providerCore.GetGroupPolicies();

            if (resultCoreParamBaseEjectionCauses is ResultError<List<coreModel.ParamBaseGroupPolicies>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultCoreParamBaseEjectionCauses as ResultError<List<coreModel.ParamBaseGroupPolicies>, ErrorModel>).Message;
                genericModelsServicesQueryModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                genericModelsServicesQueryModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (resultCoreParamBaseEjectionCauses is ResultValue<List<coreModel.ParamBaseGroupPolicies>, ErrorModel>)
            {
                List<coreModel.ParamBaseGroupPolicies> ejectionCausesCore = (resultCoreParamBaseEjectionCauses as ResultValue<List<coreModel.ParamBaseGroupPolicies>, ErrorModel>).Value;
                List<CompanyParamGroupPolicies> groupsPolicies = CoreCompanyAssembler.CreateCoreGroupPolicies(ejectionCausesCore);
                genericModelsServicesQueryModel.GenericModelServicesQueryModel = ModelServiceAssember.CreateBaseGroupsPolicies(groupsPolicies);
                genericModelsServicesQueryModel.ErrorTypeService = ErrorTypeService.Ok;
            }

            return genericModelsServicesQueryModel;
        }

        /// <summary>
        /// Obtiene listado para executionOperation
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>   
        public List<RejectionCauseServiceModel> CompanyExecuteOperationsRejectionCausesServiceModel(List<RejectionCauseServiceModel> rejectionCauseServiceModel)
        {
            List<RejectionCauseServiceModel> result = new List<RejectionCauseServiceModel>();
            RejectionCausesServiceModel rejectionCausesServiceModel = new RejectionCausesServiceModel();
            foreach (var itemSM in rejectionCauseServiceModel)
            {
                CompanyParamBaseEjectionCauses companyParamBaseEjection = ServiceModelAssembler.CreateParamBaseEjectionCauses(itemSM);
                RejectionCauseServiceModel listresult = new RejectionCauseServiceModel();
                using (Transaction transaction = new Transaction())
                {
                    listresult = this.OperationBaseRejectionCause(companyParamBaseEjection, (StatusTypeService)itemSM.StatusTypeService);

                    {
                        transaction.Complete();
                    }
                }

                result.Add(listresult);
            }
            return result;
        }

        /// <summary>
        /// Operaciones en base de datos
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>   
        public RejectionCauseServiceModel OperationBaseRejectionCause(CompanyParamBaseEjectionCauses companyParamBaseEjectionCauses, StatusTypeService statusTypeService)
        {
            ParamBaseEjectionCauses coreParamBaseEjectionCauses = CompanyCoreAssemblers.CreateCoreBaseEjectionCause(companyParamBaseEjectionCauses);
            coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore providerCore = new coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore();

            RejectionCauseServiceModel itemList = new RejectionCauseServiceModel();
            Result<ParamBaseEjectionCauses, ErrorModel> result;
            switch (statusTypeService)
            {
                case StatusTypeService.Create:
                    result = providerCore.CreateBaseRejectionCause(coreParamBaseEjectionCauses);
                    break;
                case StatusTypeService.Update:
                    result = providerCore.UpdateBaseRejectionCause(coreParamBaseEjectionCauses);
                    break;
                case StatusTypeService.Delete:
                    result = providerCore.DeleteBaseRejectionCause(coreParamBaseEjectionCauses);
                    break;
                default:
                    result = providerCore.CreateBaseRejectionCause(coreParamBaseEjectionCauses);
                    break;

            }

            if (result is ResultError<ParamBaseEjectionCauses, ErrorModel>)
            {
                ErrorModel errorModelResult = (result as ResultError<ParamBaseEjectionCauses, ErrorModel>).Message;
                itemList.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorDescription = errorModelResult.ErrorDescription,
                    ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType
                };
                itemList.StatusTypeService = StatusTypeService.Error;
            }
            else if (result is ResultValue<ParamBaseEjectionCauses, ErrorModel>)
            {
                ParamBaseEjectionCauses paramBaseEjectionCauses = (result as ResultValue<ParamBaseEjectionCauses, ErrorModel>).Value;
                CompanyParamBaseEjectionCauses comParamBaseEjectionCauses = CoreCompanyAssembler.CreateCoreBaseEjectionCause(paramBaseEjectionCauses);
                itemList = ModelServiceAssember.CreateBaseEjectionCause(comParamBaseEjectionCauses);
                itemList.ErrorServiceModel = new ErrorServiceModel()
                {
                    ErrorTypeService = ErrorTypeService.Ok
                };
                itemList.StatusTypeService = StatusTypeService.Original;
            }
            return itemList;
        }

        /// <summary>
        /// Obtiene lista de Declinaciones
        /// </summary>
        /// <returns>Retorna lista de Declinaciones</returns>
        public RejectionCausesServiceModel CompanyGetRejectionCauseByDescription(string description, int groupPolicies)
        {
            RejectionCausesServiceModel rejectionCauseServiceModel = new RejectionCausesServiceModel();

            coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore providerCore = new coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore();
            Result<List<coreModel.ParamBaseEjectionCauses>, ErrorModel> resultCoreParamBaseEjectionCauses = providerCore.GetRejectionCausesByDescription(description, groupPolicies);

            if (resultCoreParamBaseEjectionCauses is ResultError<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultCoreParamBaseEjectionCauses as ResultError<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>).Message;
                rejectionCauseServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                rejectionCauseServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (resultCoreParamBaseEjectionCauses is ResultValue<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>)
            {
                List<coreModel.ParamBaseEjectionCauses> ejectionCausesCore = (resultCoreParamBaseEjectionCauses as ResultValue<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>).Value;
                List<CompanyParamBaseEjectionCauses> ejectionCausesCompany = CoreCompanyAssembler.CreateCoreBaseEjectionCauses(ejectionCausesCore);
                rejectionCauseServiceModel.RejectionCauseServiceModel = ModelServiceAssember.CreateBaseEjectionCauses(ejectionCausesCompany);
                rejectionCauseServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            }

            return rejectionCauseServiceModel;
        }

        /// <summary>
        /// Obtiene lista de motivos de rechazo por grupo de poliza
        /// </summary>
        /// <returns>Retorna lista de motivos de rechazo</returns>
        public RejectionCausesServiceModel CompanyGetRejectionCausesByGroupPolicyId(int groupPolicyId)
        {
            RejectionCausesServiceModel rejectionCauseServiceModel = new RejectionCausesServiceModel();

            coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore providerCore = new coreProvider.AuthorizationPoliciesParamServiceEEProviderWebCore();
            Result<List<coreModel.ParamBaseEjectionCauses>, ErrorModel> resultCoreParamBaseEjectionCauses = providerCore.GetRejectionCausesAll();

            if (resultCoreParamBaseEjectionCauses is ResultError<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>)
            {
                ErrorModel errorModelResult = (resultCoreParamBaseEjectionCauses as ResultError<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>).Message;
                rejectionCauseServiceModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                rejectionCauseServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (resultCoreParamBaseEjectionCauses is ResultValue<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>)
            {
                List<coreModel.ParamBaseEjectionCauses> ejectionCausesCore = (resultCoreParamBaseEjectionCauses as ResultValue<List<coreModel.ParamBaseEjectionCauses>, ErrorModel>).Value;
                ejectionCausesCore = ejectionCausesCore.FindAll(x => x.paramBaseGroupPolicies.id == groupPolicyId);
                List<CompanyParamBaseEjectionCauses> ejectionCausesCompany = CoreCompanyAssembler.CreateCoreBaseEjectionCauses(ejectionCausesCore);
                rejectionCauseServiceModel.RejectionCauseServiceModel = ModelServiceAssember.CreateBaseEjectionCauses(ejectionCausesCompany);
                rejectionCauseServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            }

            return rejectionCauseServiceModel;
        }

    }





}
