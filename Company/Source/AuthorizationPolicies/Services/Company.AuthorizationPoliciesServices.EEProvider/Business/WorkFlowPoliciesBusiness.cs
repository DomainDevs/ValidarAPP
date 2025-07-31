using Sistran.Company.Application.UniqueUserServices.Models;
using USEN = Sistran.Core.Application.UniqueUser.Entities;
using EVEN = Sistran.Core.Application.Events.Entities;
using AUEN = Sistran.Core.Application.AuthorizationPolicies.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.AuthorizationPoliciesServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs;

namespace Sistran.Company.AuthorizationPoliciesServices.EEProvider.Business
{
    public class WorkFlowPoliciesBusiness
    {

        public List<CompanyEventAuthorization> GetAuthorizationsByUserId(int userId,int eventId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EVEN.CoEventAuthorization.Properties.EventId, typeof(EVEN.CoEventAuthorization).Name);
            filter.Equal();
            filter.Constant(eventId);
            filter.And();
            filter.Property(EVEN.CoEventAuthorization.Properties.AuthoUserId, typeof(EVEN.CoEventAuthorization).Name);
            filter.Equal();
            filter.Constant(userId);
            filter.And();
            filter.Property(EVEN.CoEventAuthorization.Properties.AuthorizedInd, typeof(EVEN.CoEventAuthorization).Name);
            filter.Equal();
            filter.Constant(0);
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(EVEN.CoEventAuthorization), filter.GetPredicate());

            return ModelAssembler.CreateCompanyEventAuthorization(businessCollection);
        }
        public List<CompanyEventGroup> GetEventGroupsByEventGroupId(int eventGroupId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(EVEN.CoEventGroup.Properties.GroupEventId, typeof(EVEN.CoEventGroup).Name);
            filter.Equal();
            filter.Constant(eventGroupId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(EVEN.CoEventGroup), filter.GetPredicate());

            return ModelAssembler.CreateCompanyEventGroups(businessCollection);
        }

        public List<CompanyUser> GetUsersByDescription(string description)
        {
            List<CompanyUser> companyUsers = new List<CompanyUser>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(USEN.UniqueUsers.Properties.AccountName, typeof(USEN.UniqueUsers).Name);
            filter.Like();
            filter.Constant("%" + description + "%");

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(USEN.UniqueUsers), filter.GetPredicate());

            return ModelAssembler.CreateCompanyUsers(businessCollection);
        }

        public List<CompanyEventAuthorization> GetEventAuthorizationsByUserId(int? userId, int eventGroupId, DateTime startDate, DateTime finishDate, int groupId)
        {
            List<CompanyEventAuthorization> companyEventAuthorizations = new List<CompanyEventAuthorization>();
            WorkFlowPoliciesDAO workFlowPoliciesDAO = new WorkFlowPoliciesDAO();
            companyEventAuthorizations = workFlowPoliciesDAO.GetEventAuthorizationsByUserId(userId, eventGroupId, startDate, finishDate, groupId);

            if (companyEventAuthorizations.Count > 0)
            {
                foreach (CompanyEventAuthorization companyEventAuthorization in companyEventAuthorizations)
                {
                    if (companyEventAuthorization.User != null)
                    {
                        companyEventAuthorization.User = GetUserByUserId(companyEventAuthorization.User.Id);
                    }

                    if (companyEventAuthorization.Policy.Id > 0)
                    {
                        companyEventAuthorization.Policy.Branch = GetBranchByBranchId(companyEventAuthorization.Policy.Branch.Id);
                        companyEventAuthorization.Policy.Prefix = GetPrefixByPrefixId(companyEventAuthorization.Policy.Prefix.Id);
                    }
                };

                return companyEventAuthorizations;
            }
            else
            {
                return companyEventAuthorizations;
            }
        }
        
        //public List<CompanyEventAuthorization> CreateEventAuthorizations(List<CompanyEventAuthorization> companyEventAuthorizations, EventTypes eventTypes, string urlAccess, int userId, string description)
        //{
        //    WorkFlowPoliciesDAO workFlowPoliciesDAO = new WorkFlowPoliciesDAO();

        //    foreach (CompanyEventAuthorization companyEventAuthorization in companyEventAuthorizations)
        //    {
        //        companyEventAuthorization.Description = description;
        //        companyEventAuthorization.AuthorizationId = workFlowPoliciesDAO.CreateEventAuthorization(
        //            workFlowPoliciesDAO.MapperFromPolicyToEventAuthorization(companyEventAuthorization, eventTypes, urlAccess, userId)
        //        ).AuthorizationId;
        //    }

        //    return companyEventAuthorizations;
        //}

        public List<CompanyEventAuthorization> CreateEventAuthorization(List<CompanyEventAuthorization> companyEventAuthorizations,  string description)
        {
            WorkFlowPoliciesDAO workFlowPoliciesDAO = new WorkFlowPoliciesDAO();

            foreach (CompanyEventAuthorization companyEventAuthorization in companyEventAuthorizations)
            {
                companyEventAuthorization.Description = description;
                companyEventAuthorization.AuthorizationId = workFlowPoliciesDAO.CreateBaseEventAuthorization(
                    ModelAssembler.CreateCompanyEventAuthorizationRelease(companyEventAuthorization)
                ).AuthorizationId;
            }

            return companyEventAuthorizations;
        }


        public CompanyWorkFlowUser GetUserByUserId(int userId)
        {
            PrimaryKey primaryKey = USEN.UniqueUsers.CreatePrimaryKey(userId);
            CompanyWorkFlowUser companyWorkFlowUser = new CompanyWorkFlowUser();
            companyWorkFlowUser = ModelAssembler.CreateCompanyWorkFlowUser((USEN.UniqueUsers)DataFacadeManager.GetObject(primaryKey));
            return companyWorkFlowUser;
        }

        public CompanyWorkFlowBranch GetBranchByBranchId(int branchId)
        {
            PrimaryKey primaryKey = COMMEN.Branch.CreatePrimaryKey(branchId);
            CompanyWorkFlowBranch companyWorkFlowBranch = new CompanyWorkFlowBranch();
            companyWorkFlowBranch = ModelAssembler.CreateCompanyWorkFlowBranch((COMMEN.Branch)DataFacadeManager.GetObject(primaryKey));
            return companyWorkFlowBranch;
        }

        public CompanyWorkFlowPrefix GetPrefixByPrefixId(int prefixId)
        {
            PrimaryKey primaryKey = COMMEN.Prefix.CreatePrimaryKey(prefixId);
            CompanyWorkFlowPrefix companyWorkFlowPrefix = new CompanyWorkFlowPrefix();
            companyWorkFlowPrefix = ModelAssembler.CreateCompanyWorkFlowPrefix((COMMEN.Prefix)DataFacadeManager.GetObject(primaryKey));
            return companyWorkFlowPrefix;
        }

        public int GetGroupIdByUserId(int userId)
        {
            var groupId = 0;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(USEN.UserGroup.Properties.UserId, typeof(USEN.UserGroup).Name);
            filter.Equal();
            filter.Constant(userId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(USEN.UserGroup), filter.GetPredicate());
            if (businessCollection.Count > 0)
            {
                groupId = ((USEN.UserGroup)businessCollection[0]).GroupCode;
            }
            else
            {
                groupId = -1;
            }
            return groupId;
        }

    }
}
