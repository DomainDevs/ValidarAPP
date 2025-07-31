// -----------------------------------------------------------------------
// <copyright file="AuthorizationPoliciesParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider
{
    using Company.Application.AuthorizationPoliciesServices;
    using global::Company.AuthorizationPoliciesServices.EEProvider.Business;
    using global::Company.AuthorizationPoliciesServices.Models;
    using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
    using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Business;
    using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs;
    using Sistran.Company.Application.ModelServices.Enums;
    using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Company.Application.ModelServices.Models.Reports;
    using Sistran.Company.Application.UniqueUserServices.Models;
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.Utilities.Error;
    using Sistran.Company.AuthorizationPoliciesServices.EEProvider.Business;
    using Sistran.Company.AuthorizationPoliciesServices.Models;
    using Sistran.Core.Framework.BAF;
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using COREEE = Sistran.Core.Application.AuthorizationPoliciesServices.EEProvider;
    using Models = Sistran.Company.Application.AuthorizationPoliciesServices.Models;
    using UTMO = Sistran.Company.Application.Utilities.Error;

    /// <summary>
    /// Defines the <see cref="AuthorizationPoliciesServiceEEProvider" />
    /// </summary>
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class AuthorizationPoliciesServiceEEProvider : COREEE.AuthorizationPoliciesServiceEEProvider, IAuthorizationPoliciesService
    {
        public StatusServicesModel GetAllStates()
        {
            StatusServicesModel Status = new StatusServicesModel();
            ReportAuthorizationpoliciesDao ReportDao = new ReportAuthorizationpoliciesDao();
            UTMO.Result<List<Models.Status>, UTMO.ErrorModel> result = ReportDao.GetAllStatus();
            if (result is UTMO.ResultError<List<Models.Status>, UTMO.ErrorModel>)
            {
                UTMO.ErrorModel errorModelResult = (result as UTMO.ResultError<List<Models.Status>, UTMO.ErrorModel>).Message;
                Status.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
                Status.ErrorDescription = errorModelResult.ErrorDescription;
            }
            else if (result is UTMO.ResultValue<List<Models.Status>, UTMO.ErrorModel>)
            {
                List<Models.Status> StatusModel = (result as UTMO.ResultValue<List<Models.Status>, UTMO.ErrorModel>).Value;

                Status.StatusServicemodel = Assemblers.ModelAssembler.CreateStatusServiceModels(StatusModel);
                Status.ErrorTypeService = ErrorTypeService.Ok;
            }
            return Status;
        }

        public PoliciesServicesModel GetPolicies(Models.CompanyPolicyValid parampolicies)
        {
            ReportsPoliciesDAO reportsDAO = new ReportsPoliciesDAO();
            List<PoliciesServiceModel> vehicleParameters = new List<PoliciesServiceModel>();

            Result<Models.CompanyReportPolicies, ErrorModel> Result = reportsDAO.GetPolicies(parampolicies);

            PoliciesServicesModel policiesServicesModel = new PoliciesServicesModel();
            if (Result is ResultError<Models.CompanyReportPolicies, ErrorModel>)
            {
                ErrorModel errorModelResult = (Result as ResultError<Models.CompanyReportPolicies, ErrorModel>).Message;
                policiesServicesModel.ErrorDescription = errorModelResult.ErrorDescription;
                policiesServicesModel.ErrorTypeService = (ErrorTypeService)errorModelResult.ErrorType;
            }
            else
            {
                Models.CompanyReportPolicies resultValue = (Result as ResultValue<Models.CompanyReportPolicies, ErrorModel>).Value;

                policiesServicesModel = ModelAssembler.GetCompanyReportPolicies(resultValue);
            }

            return policiesServicesModel;

        }

        public ExcelFileDTO GenerateFileToReportAuthorizationPolicies(string fileName, Models.CompanyPolicyValid cpv)
        {
            try
            {
                ReportsPoliciesBusiness policies = new ReportsPoliciesBusiness();
                return new ExcelFileDTO { File = policies.GenerateFileToReportAuthorizationPolicies(fileName, cpv) };
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyUser> GetUsersByDescription(string description)
        {
            try
            {
                WorkFlowPoliciesBusiness workFlowPoliciesBusiness = new WorkFlowPoliciesBusiness();
                return workFlowPoliciesBusiness.GetUsersByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEventGroup> GetEventGroupsByEventGroupId(int eventGroupId)
        {
            try
            {
                WorkFlowPoliciesBusiness workFlowPoliciesBusiness = new WorkFlowPoliciesBusiness();
                return workFlowPoliciesBusiness.GetEventGroupsByEventGroupId(eventGroupId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEventAuthorization> GetEventAuthorizationsByUserId(int? userId, int eventId, DateTime startDate, DateTime finishDate, int sesionId)
        {
            try
            {
                WorkFlowPoliciesBusiness workFlowPoliciesBusiness = new WorkFlowPoliciesBusiness();
                int groupId = workFlowPoliciesBusiness.GetGroupIdByUserId(sesionId);
                return workFlowPoliciesBusiness.GetEventAuthorizationsByUserId(userId, eventId, startDate, finishDate, groupId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEventAuthorization> GetAuthorizationsByUserId(int userId, int eventId)
        {
            try
            {
                WorkFlowPoliciesBusiness workFlowPoliciesBusiness = new WorkFlowPoliciesBusiness();
                return workFlowPoliciesBusiness.GetAuthorizationsByUserId(userId, eventId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region AuthorizationRiskList
        public List<CompanyEventGroup> GetSarlaftEventGroupByEventGroupId(int eventGroupSftId)
        {
            try
            {
                AuthorizationPersonRiskListBusiness authorizationPersonRiskListBusiness = new AuthorizationPersonRiskListBusiness();
                return authorizationPersonRiskListBusiness.GetSarlaftEventGroupByEventGroupId(eventGroupSftId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        public List<CompanyEventAuthorization> CreateBaseEventAuthorization(List<CompanyEventAuthorization> companyEventAuthorizations, string description)
        {
            try
            {
                WorkFlowPoliciesBusiness workFlowPoliciesBusiness = new WorkFlowPoliciesBusiness();
                return workFlowPoliciesBusiness.CreateEventAuthorization(companyEventAuthorizations, description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region SarlaftAuthorization
        public List<CompanySarlaftEventAuthorization> SearchSarlaftSuspectOperations(CompanySarlaftEventAuthorization sarlaftSuspectOperation)
        {
            try
            {
                AuthorizationSuspectOperationsBusiness suspectOperationsBusiness = new AuthorizationSuspectOperationsBusiness();
                return suspectOperationsBusiness.SearchSarlaftSuspectOperations(sarlaftSuspectOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanySarlaftAuthorizationReason> GetAuthorizationReasons(int eventGroupId)
        {
            try
            {
                AuthorizationSuspectOperationsBusiness suspectOperationsBusiness = new AuthorizationSuspectOperationsBusiness();
                return suspectOperationsBusiness.GetAuthorizationReasons(eventGroupId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanySarlaftEventAuthorization AuthorizeSuspectOperation(CompanySarlaftEventAuthorization companyEventAuthorization)
        {
            try
            {
                AuthorizationSuspectOperationsBusiness suspectOperationsBusiness = new AuthorizationSuspectOperationsBusiness();
                return suspectOperationsBusiness.AuthorizeSupectOperation(companyEventAuthorization);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #endregion

        #region RiskListAuthorization
        public List<CompanyAuthorizationRiskList> SearchAuthorizationRiskList(CompanyAuthorizationRiskList companyAuthorizationRiskList, int userId)
        {
            try
            {
                AuthorizationPersonRiskListBusiness authorizationPersonRiskListBusiness = new AuthorizationPersonRiskListBusiness();
                return authorizationPersonRiskListBusiness.SearchSarlaftRiskList(companyAuthorizationRiskList, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }


        }

        public CompanyAuthorizationRiskList AuthorizeRiskListOperation(CompanyAuthorizationRiskList companyAuthorizationRiskList)
        {
            try
            {
                AuthorizationPersonRiskListBusiness authorizationPersonRiskListBusiness = new AuthorizationPersonRiskListBusiness();
                return authorizationPersonRiskListBusiness.AuthorizeRiskListOperation(companyAuthorizationRiskList);
            }
            catch(Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion
    }
}
