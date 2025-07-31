using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.DTOs;
using Sistran.Company.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.EEProvider.Assemblers
{
    using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
    public class ModelAssembler
    {
        public static CompanyEventAuthorization CreateEventAuthorization(EventAuthorizationDTO eventAuthorizationDTO)
        {
            return new CompanyEventAuthorization
            {
                AuthorizationId = eventAuthorizationDTO.AuthorizationId,
                GroupEvendId = eventAuthorizationDTO.GroupEvendId,
                EventId = eventAuthorizationDTO.EventId,
                AccessId = eventAuthorizationDTO.AccessId,
                UrlAccess = eventAuthorizationDTO.UrlAccess,
                HierarchyCode = eventAuthorizationDTO.HierarchyCode,
                AuthoUserId = eventAuthorizationDTO.AuthoUserId,
                Operation1Id = eventAuthorizationDTO.Operation1Id,
                Endorsement = new CompanyWorkFlowEndorsement
                {
                    Id = Convert.ToInt32(eventAuthorizationDTO.Operation2Id),
                    DocumentNumber = eventAuthorizationDTO.EndorsementNumber
                },
                RejectInd = eventAuthorizationDTO.RejectInd,
                AuthorizedInd = eventAuthorizationDTO.AuthorizedInd,
                EventDate = eventAuthorizationDTO.EventDate,
                Description = eventAuthorizationDTO.Description,
                User = new CompanyWorkFlowUser
                {
                    Id = eventAuthorizationDTO.UserId,
                    AccountName = eventAuthorizationDTO.UserAccountName
                },
                Policy = new CompanyWorkFlowPolicy
                {
                    Id = eventAuthorizationDTO.PolicyId,
                    DocumentNumber = eventAuthorizationDTO.PolicyNumber,
                    Prefix = new CompanyWorkFlowPrefix
                    {
                        Id = eventAuthorizationDTO.PrefixId,
                        Description = eventAuthorizationDTO.Description
                    },
                    Branch = new CompanyWorkFlowBranch
                    {
                        Id = eventAuthorizationDTO.BranchId,
                        Description = eventAuthorizationDTO.BranchDescription
                    }
                }
            };
        }

        public static List<CompanyEventAuthorization> CreateEventAuthorizations(List<EventAuthorizationDTO> eventAuthorizationDTOs)
        {
            List<CompanyEventAuthorization> companyEventAuthorizations = new List<CompanyEventAuthorization>();
            foreach (EventAuthorizationDTO eventAuthorizationDTO in eventAuthorizationDTOs)
            {
                companyEventAuthorizations.Add(CreateEventAuthorization(eventAuthorizationDTO));
            }

            return companyEventAuthorizations;
        }

       
    }
}