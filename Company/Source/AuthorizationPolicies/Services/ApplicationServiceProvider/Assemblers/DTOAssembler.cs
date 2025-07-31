using Company.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.DTOs;
using Sistran.Company.Application.UniqueUserServices.Models;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        internal static SelectDTO CreateEventGroup(CompanyEventGroup companyEvent)
        {
            return new SelectDTO
            {
                Id = companyEvent.Id,
                Description = companyEvent.Description
            };
        }

        internal static List<SelectDTO> CreateEventGroups(List<CompanyEventGroup> companyEventGroups)
        {
            List<SelectDTO> events = new List<SelectDTO>();
            foreach (CompanyEventGroup companyEventGroup in companyEventGroups)
            {
                events.Add(CreateEventGroup(companyEventGroup));
            }

            return events;
        }

        internal static SelectDTO CreateAuthorizationReason(CompanySarlaftAuthorizationReason companySarlaftAuthorizationReason)
        {
            return new SelectDTO
            {
                Id = companySarlaftAuthorizationReason.Id,
                Description = companySarlaftAuthorizationReason.Description
            };
        }

        internal static List<SelectDTO> CreateAuthorizationReasons(List<CompanySarlaftAuthorizationReason> companySarlaftAuthorizationReasons)
        {
            List<SelectDTO> events = new List<SelectDTO>();
            foreach (CompanySarlaftAuthorizationReason SarlaftAuthorizationReason in companySarlaftAuthorizationReasons)
            {
                events.Add(CreateAuthorizationReason(SarlaftAuthorizationReason));
            }

            return events;
        }
        internal static UserDTO CreateUser(CompanyUser companyUser)
        {
            return new UserDTO
            {
                Id = companyUser.UserId,
                AccountName = companyUser.AccountName
            };
        }

        internal static List<UserDTO> CreateUsers(List<CompanyUser> companyUsers)
        {
            List<UserDTO> usersDTO = new List<UserDTO>();
            foreach (CompanyUser companyUser in companyUsers)
            {
                usersDTO.Add(CreateUser(companyUser));
            }

            return usersDTO;
        }

        internal static EventAuthorizationDTO CreateEventAuthorization(CompanyEventAuthorization companyEventAuthorization)
        {
            return new EventAuthorizationDTO
            {
                AuthorizationId = companyEventAuthorization.AuthorizationId,
                GroupEvendId = companyEventAuthorization.GroupEvendId,
                EventId = companyEventAuthorization.EventId,
                AccessId = companyEventAuthorization.AccessId,
                HierarchyCode = companyEventAuthorization.HierarchyCode,
                UserId = companyEventAuthorization.User.Id,
                UserAccountName = companyEventAuthorization.User.AccountName,
                AuthoUserId = companyEventAuthorization.AuthoUserId,
                Operation1Id = companyEventAuthorization.Operation1Id,
                Operation2Id = companyEventAuthorization.Endorsement.Id.ToString(),
                EndorsementNumber = companyEventAuthorization.Endorsement.DocumentNumber,
                RejectInd = companyEventAuthorization.RejectInd,
                AuthorizedInd = companyEventAuthorization.AuthorizedInd,
                EventDate = companyEventAuthorization.EventDate,
                PolicyId = companyEventAuthorization.Policy.Id,
                PolicyNumber = companyEventAuthorization.Policy.DocumentNumber,
                BranchId = companyEventAuthorization.Policy.Branch.Id,
                BranchDescription = companyEventAuthorization.Policy?.Branch.Description,
                PrefixId = companyEventAuthorization.Policy.Prefix.Id,
                PrefixDescription = companyEventAuthorization.Policy?.Prefix.Description
            };
        }

        internal static List<EventAuthorizationDTO> CreateEventAuthorizations(List<CompanyEventAuthorization> companyEventAuthorizations)
        {
            List<EventAuthorizationDTO> eventAuthorizationsDTO = new List<EventAuthorizationDTO>();
            foreach (CompanyEventAuthorization companyEventAuthorization in companyEventAuthorizations)
            {
                eventAuthorizationsDTO.Add(CreateEventAuthorization(companyEventAuthorization));
            }

            return eventAuthorizationsDTO;
        }

    }
}