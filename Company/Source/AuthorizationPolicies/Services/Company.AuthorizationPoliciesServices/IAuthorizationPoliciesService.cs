// -----------------------------------------------------------------------
// <copyright file="AuthorizationPoliciesParamServiceEEProviderWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>SISTRAN\Stiveen Niño Gutierrez</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.AuthorizationPoliciesServices
{
    using Sistran.Company.Application.ModelServices.Models.AuthorizationPolicies;
    using Sistran.Company.Application.ModelServices.Models.Reports;    
    using Sistran.Core.Application.AuthorizationPoliciesServices;
    using System.ServiceModel; 
    using Sistran.Company.Application.Utilities.DTO;
    using Sistran.Company.Application.AuthorizationPoliciesServices.Models;
    using System.Collections.Generic;
    using Sistran.Company.Application.UniqueUserServices.Models;
    using System;
    using Sistran.Company.AuthorizationPoliciesServices.Models;
    using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
    using global::Company.AuthorizationPoliciesServices.Models;

    [ServiceContract]
    public interface IAuthorizationPoliciesService : IAuthorizationPoliciesServiceCore
    {
        /// <summary>
        /// Obtiene lista de status
        /// </summary>
        /// <returns>Retorna lista de modulos <see cref="StatusServicesModel"/></returns>   
        [OperationContract]
        StatusServicesModel GetAllStates();

        [OperationContract]
        PoliciesServicesModel GetPolicies(Models.CompanyPolicyValid parampolicies);

        /// <summary>
        /// GenerateFileToCity:Llamado a metodo para generar el archivo excel con las ciudades registradas en BD
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [OperationContract]
        ExcelFileDTO GenerateFileToReportAuthorizationPolicies(string fileName, CompanyPolicyValid cpv);

        [OperationContract]
        List<CompanyUser> GetUsersByDescription(string description);

        [OperationContract]
        List<CompanyEventGroup> GetEventGroupsByEventGroupId(int eventGroupId);

        [OperationContract]
        List<CompanyEventGroup> GetSarlaftEventGroupByEventGroupId(int eventGroupSftId);

        [OperationContract]
        List<CompanySarlaftEventAuthorization> SearchSarlaftSuspectOperations(CompanySarlaftEventAuthorization sarlaftSuspectOperation);

        [OperationContract]
        List<CompanyEventAuthorization> GetEventAuthorizationsByUserId(int? userId, int eventId, DateTime startDate, DateTime finishDate, int sesionId);

        [OperationContract]
        List<CompanyEventAuthorization> GetAuthorizationsByUserId(int userId, int eventId);

        //[OperationContract]
        //List<CompanyEventAuthorization> CreateEventAuthorizations(List<CompanyEventAuthorization> companyEventAuthorizations, EventTypes eventTypes, string urlAccess, int userId, string description);

        [OperationContract]
        List<CompanyEventAuthorization> CreateBaseEventAuthorization(List<CompanyEventAuthorization> companyEventAuthorizations, string description);

        [OperationContract]
        List<CompanySarlaftAuthorizationReason> GetAuthorizationReasons(int eventGroupId);

        [OperationContract]
        CompanySarlaftEventAuthorization AuthorizeSuspectOperation(CompanySarlaftEventAuthorization companyEventAuthorization);

        [OperationContract]
        List<CompanyAuthorizationRiskList> SearchAuthorizationRiskList(CompanyAuthorizationRiskList companyAuthorizationRiskList, int userId);

        [OperationContract]
        CompanyAuthorizationRiskList AuthorizeRiskListOperation(CompanyAuthorizationRiskList companyAuthorizationRiskList);


      
    }
}
