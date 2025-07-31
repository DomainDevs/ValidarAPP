using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.DTOs;
using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;

namespace Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.EEProvider
{
    public class AuthorizationPoliciesApplicationServiceProvider : IAuthorizationPoliciesApplicationService
    {
        public const int eventGroupId = 8;
        public const int eventGroupSftId = 10;

        public List<SelectDTO> GetEventGroups()
        {
            return DTOAssembler.CreateEventGroups(DelegateService.authorizationPoliciesService.GetEventGroupsByEventGroupId(eventGroupId));
        }

        public List<SelectDTO> GetSarlaftEventGroups()
        {
            return DTOAssembler.CreateEventGroups(DelegateService.authorizationPoliciesService.GetSarlaftEventGroupByEventGroupId(eventGroupSftId));
        }

        public List<SelectDTO> GetAuthorizationReasons(int eventGroupId)
        {
            return DTOAssembler.CreateAuthorizationReasons(DelegateService.authorizationPoliciesService.GetAuthorizationReasons(eventGroupId));
        }

        public List<UserDTO> GetUsersByDescription(string description)
        {
            return DTOAssembler.CreateUsers(DelegateService.authorizationPoliciesService.GetUsersByDescription(description));
        }

        public List<EventAuthorizationDTO> GetEventAuthorizationsByUserId(int? userId, int eventGroupId, DateTime startDate, DateTime finishDate, int sesionId)
        {
            return DTOAssembler.CreateEventAuthorizations(DelegateService.authorizationPoliciesService.GetEventAuthorizationsByUserId(userId, eventGroupId, startDate, finishDate, sesionId));
        }
        #region Event_Autho_WorkFlo

        public List<EventAuthorizationDTO> CreateBaseEventAuthorization(List<EventAuthorizationDTO> eventAuthorizationDTOs, string description)
        {
            return DTOAssembler.CreateEventAuthorizations(DelegateService.authorizationPoliciesService.CreateBaseEventAuthorization(ModelAssembler.CreateEventAuthorizations(eventAuthorizationDTOs), description));
        }

        public List<EventAuthorizationDTO> GetAuthorizationsByUserId(int userId, int eventId)
        {
            return DTOAssembler.CreateEventAuthorizations(DelegateService.authorizationPoliciesService.GetAuthorizationsByUserId(userId, eventId));
        }
        #endregion

    }
}
