using Sistran.Company.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static EventAuthorization CreateCompanyEventAuthorization(CompanyPolicy companyPolicy)
        {
            return new EventAuthorization
            {
                OPERATION1_ID = companyPolicy.Endorsement.TicketNumber.ToString(),
                OPERATION2_ID = companyPolicy.Endorsement.Id.ToString(),
                AUTHO_USER_ID = companyPolicy.UserId,
                EVENT_ID = (int)EventTypes.Endorsement
            };
        }
    }
}
