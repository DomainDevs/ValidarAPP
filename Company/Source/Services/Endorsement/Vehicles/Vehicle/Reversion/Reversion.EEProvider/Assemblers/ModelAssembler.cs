namespace Sistran.Company.Application.VehicleReversionService.EEProvider.Assemblers
{
    using UnderwritingServices.Enums;
    using UnderwritingServices.Models;

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
