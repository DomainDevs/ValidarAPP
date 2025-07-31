using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class PaymentTicketBusiness
    {
        public List<PaymentTicket> GetPaymentTicketItemsByCollectId(int collectId)
        {
            PaymentTicketDAO paymentTicketDAO = new PaymentTicketDAO();
            return paymentTicketDAO.GetPaymentTicketsByCollectId(collectId);
        }

        public int GetPaymentTicketSequence()
        {
            PaymentTicketDAO paymentTicketDAO = new PaymentTicketDAO();
            var result = paymentTicketDAO.GetPaymentTicketSequence();
            if (result > 0)
                return result;
            else
                throw  new BusinessException();
        }
    }
}
