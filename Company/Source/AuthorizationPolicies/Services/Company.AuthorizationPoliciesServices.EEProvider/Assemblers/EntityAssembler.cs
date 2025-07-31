using Sistran.Company.AuthorizationPoliciesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EVEN = Sistran.Core.Application.Events.Entities;

namespace Company.AuthorizationPoliciesServices.EEProvider.Assemblers
{
    public static class EntityAssembler
    {
        public static EVEN.CoEventAuthorization CreateEventAuthorization(CompanySarlaftEventAuthorization companySarlaftEventAuthorization)
        {
            return new EVEN.CoEventAuthorization(companySarlaftEventAuthorization.AuthorizationId)
            {
                Description = companySarlaftEventAuthorization.Detail,
                DescriptionErrorMessage = companySarlaftEventAuthorization.RequestDetail,
                EventDate = companySarlaftEventAuthorization.EventDate,
                RejectId = companySarlaftEventAuthorization.RejectId,
                AuthorizationDescription = companySarlaftEventAuthorization.Description,
                EventId = companySarlaftEventAuthorization.EventId,
                GroupEventId = companySarlaftEventAuthorization.EventGroupId,
                AuthorizationReasonCode = companySarlaftEventAuthorization.AuthorizeReasonId,
                EntityDescriptionValues = companySarlaftEventAuthorization.Assets,
            };
        }
    }
}
