
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.PrintingServices.Models
{
    using Sistran.Company.Application.UnderwritingServices.Models;

    public class ModelAssembler
    {
        #region Event Work Flow Printing

        /// <summary>
        /// Creates the EventAuthorization by CompanyPolicy
        /// </summary>
        /// <param name="policy"></param>
        /// <returns></returns>
        public static EventAuthorization CreateCompanyEventAuthorizationPrinting(PolicyInfo policy)
        {
            int eventId = 0;
            if (policy.CommonProperties.IsMassive == true)
            {
                eventId = (int)UnderwritingServices.Enums.EventTypes.PrintingMassive;
            }
            else
            {
                eventId = (int)UnderwritingServices.Enums.EventTypes.Printing;
            }
            EventAuthorization Event = new EventAuthorization();
            try
            {
                Event.OPERATION2_ID = policy.EndorsementId.ToString();
                Event.OPERATION1_ID = (policy.TicketNumber == null) ? "0" : policy.TicketNumber.ToString();
                Event.AUTHO_USER_ID = policy.CommonProperties.UserId; 
                Event.EVENT_ID = eventId;
            }
            catch (Exception ex)
            {
            }
            return Event;
        }
        #endregion
    }
}
