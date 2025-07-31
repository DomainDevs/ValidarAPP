using Company.AuthorizationPoliciesServices.Models;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sistran.Core.Framework.UIF.Web.Areas.AuthorizationPolicies.Models
{
    public class ModelAssembler
    {
        #region SarlaftAuthorization

        public static CompanySarlaftEventAuthorization CreateSarlaftEventAuthorization(AuthorizationSuspectOperationModelView suspectOpModelView)
        {
            return new CompanySarlaftEventAuthorization
            {
                AuthorizationId = suspectOpModelView.AuthorizationId,
                EventGroupId = suspectOpModelView.EventGroupId,
                EventId = suspectOpModelView.EventId,
                RejectId = suspectOpModelView.RejectId,
                EventDate = suspectOpModelView.EventDate,
                Detail = suspectOpModelView.Detail,
                RequestDetail = suspectOpModelView.RequestDetail,
                Description = suspectOpModelView.Description,
                Assets = suspectOpModelView.Assets,
                AuthorizeReasonId = suspectOpModelView.AuthorizeReasonId,
                DocumentNumber = suspectOpModelView.DocumentNumber,
                FormNumber = suspectOpModelView.FormNumber,
                Year = suspectOpModelView.Year,
                IndividualId = suspectOpModelView.IndividualId,
                User = suspectOpModelView.User,
                UserId = suspectOpModelView.UserId,
                TypeId = suspectOpModelView.TypeId,
                DocumentType = suspectOpModelView.DocumentType,
                IsAuthorized = suspectOpModelView.IsAuthorized,
                IsRejected = suspectOpModelView.IsRejected,
                AuthoUserId = suspectOpModelView.AuthoUserId
            };
        }
        public static AuthorizationSuspectOperationModelView CreateSarlaftSuspectOperation(CompanySarlaftEventAuthorization companySarlaftEventAuthorization)
        {
            return new AuthorizationSuspectOperationModelView
            {
                AuthorizationId = companySarlaftEventAuthorization.AuthorizationId,
                EventGroupId = companySarlaftEventAuthorization.EventGroupId,
                EventId = companySarlaftEventAuthorization.EventId,
                RejectId = companySarlaftEventAuthorization.RejectId,
                EventDate = companySarlaftEventAuthorization.EventDate,
                Detail = companySarlaftEventAuthorization.Detail,
                RequestDetail = companySarlaftEventAuthorization.RequestDetail,
                Description = companySarlaftEventAuthorization.Description,
                Assets = companySarlaftEventAuthorization.Assets,
                AuthorizeReasonId = companySarlaftEventAuthorization.AuthorizeReasonId,
                DocumentNumber = companySarlaftEventAuthorization.DocumentNumber,
                FormNumber = companySarlaftEventAuthorization.FormNumber,
                Year = companySarlaftEventAuthorization.Year,
                IndividualId = companySarlaftEventAuthorization.IndividualId,
                User = companySarlaftEventAuthorization.User,
                UserId = companySarlaftEventAuthorization.UserId,
                TypeId = companySarlaftEventAuthorization.TypeId,
                DocumentType = companySarlaftEventAuthorization.DocumentType,
                IsAuthorized = companySarlaftEventAuthorization.IsAuthorized,
                IsRejected = companySarlaftEventAuthorization.IsRejected,
                AuthoUserId = companySarlaftEventAuthorization.AuthoUserId
            };
        }

        public static List<AuthorizationSuspectOperationModelView> CreateSarlaftSuspectOperations(List<CompanySarlaftEventAuthorization> listSarlaftEventAuthorizations)
        {
            List<AuthorizationSuspectOperationModelView> listSuspectOperationModelView = new List<AuthorizationSuspectOperationModelView>();
            foreach (CompanySarlaftEventAuthorization companySarlaftEventAuthorization in listSarlaftEventAuthorizations)
            {
                listSuspectOperationModelView.Add(CreateSarlaftSuspectOperation(companySarlaftEventAuthorization));
            }
            return listSuspectOperationModelView;
        }

        #endregion

        #region SarlaftRiskList
        public static CompanyAuthorizationRiskList CreateCompanyAuthorizationRiskList(AuthorizationRiskListModelView authorizationSuspectOperationModelView)
        {
            
            return new CompanyAuthorizationRiskList
            {
                AuthorizationId = authorizationSuspectOperationModelView.AuthorizationId,
                EventGroupId = authorizationSuspectOperationModelView.EventGroupId,
                EventId = authorizationSuspectOperationModelView.EventId,
                RejectId = authorizationSuspectOperationModelView.RejectId,
                EventDate = authorizationSuspectOperationModelView.EventDate,
                Detail = authorizationSuspectOperationModelView.Detail,
                RequestDetail = authorizationSuspectOperationModelView.RequestDetail,
                Description = authorizationSuspectOperationModelView.Description,
                AuthorizeReasonId = authorizationSuspectOperationModelView.AuthorizeReasonId,
                DocumentNumber = authorizationSuspectOperationModelView.DocumentNumber,
                DocumentType = authorizationSuspectOperationModelView.DocumentType,
                IsRejected = authorizationSuspectOperationModelView.IsRejected,
                IsAuthorized = authorizationSuspectOperationModelView.IsAuthorized
            };
        }

        public static AuthorizationRiskListModelView CreateAuthorizationRiskList(CompanyAuthorizationRiskList companyAuthorizationRiskList)
        {
            return new AuthorizationRiskListModelView
            {
                AuthorizationId = companyAuthorizationRiskList.AuthorizationId,
                EventGroupId = companyAuthorizationRiskList.EventGroupId,
                EventId = companyAuthorizationRiskList.EventId,
                RejectId = companyAuthorizationRiskList.RejectId,
                EventDate = companyAuthorizationRiskList.EventDate,
                Detail = companyAuthorizationRiskList.Detail,
                RequestDetail = companyAuthorizationRiskList.RequestDetail,
                Description = companyAuthorizationRiskList.Description,
                AuthorizeReasonId = companyAuthorizationRiskList.AuthorizeReasonId,
                DocumentNumber = companyAuthorizationRiskList.DocumentNumber,
                DocumentType = companyAuthorizationRiskList.DocumentType,
                IsRejected = companyAuthorizationRiskList.IsRejected,
                IsAuthorized = companyAuthorizationRiskList.IsAuthorized
            };
        }

        public static List<AuthorizationRiskListModelView> CreateAuthorizationRiskLists(List<CompanyAuthorizationRiskList> companyAuthorizationRiskLists)
        {
            List<AuthorizationRiskListModelView> listauthorizationAnswerModelView = new List<AuthorizationRiskListModelView>();
            foreach(CompanyAuthorizationRiskList companyRiskList in companyAuthorizationRiskLists)
            {
                listauthorizationAnswerModelView.Add(CreateAuthorizationRiskList(companyRiskList));
            }
            return listauthorizationAnswerModelView;
        }
        #endregion
    }
}