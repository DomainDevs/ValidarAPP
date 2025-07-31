using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class CollectPaymentBusiness
    {
        public List<Payment> GetPaymentsByCollectId(int collectId)
        {
            var paymentDAO = new PaymentDAO();
            return paymentDAO.GetCollectPaymentsByCollectId(collectId);
        }
    }
}
