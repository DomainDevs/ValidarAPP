using Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService.DTOs;
using Sistran.Company.Application.AuthorizationPoliciesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Sistran.Company.Application.AuthorizationPolicies.AuthorizationPolicyApplicationService
{
    [ServiceContract]
    public interface IAuthorizationPoliciesApplicationService
    {
        [OperationContract]
        List<SelectDTO> GetEventGroups();

        [OperationContract]
        List<UserDTO> GetUsersByDescription(string description);

        [OperationContract]
        List<EventAuthorizationDTO> GetEventAuthorizationsByUserId(int? userId, int eventGroupId, DateTime startDate, DateTime finishDate, int sesionId);

        //[OperationContract]
        //List<EventAuthorizationDTO> CreateEventAuthorizations(List<EventAuthorizationDTO> eventAuthorizationDTOs, EventTypes eventType, string urlAccess, int userId, string description);

        [OperationContract]
        List<EventAuthorizationDTO> CreateBaseEventAuthorization(List<EventAuthorizationDTO> eventAuthorizationDTOs, string description);

        [OperationContract]
        List<SelectDTO> GetSarlaftEventGroups();

        [OperationContract]
        List<SelectDTO> GetAuthorizationReasons(int eventGroupId);

        [OperationContract]
        List<EventAuthorizationDTO> GetAuthorizationsByUserId(int userId, int eventId);

    }
}
