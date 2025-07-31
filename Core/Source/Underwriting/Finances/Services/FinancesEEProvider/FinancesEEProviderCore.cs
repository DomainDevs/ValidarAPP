using Sistran.Core.Application.Finances.EEProvider.Business;
using Sistran.Core.Application.Finances.EEProvider.Resources;
using Sistran.Core.Application.Finances.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Application.Finances.EEProvider
{
    public class FinancesEEProviderCore : IFinancesCore
    {
        public List<IssuanceOccupation> GetIssuanceOccupations()
        {
            try
            {
                FinancesBusiness financesBusiness = new FinancesBusiness();
                return financesBusiness.GetOccupations();
            }
            catch(Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetIssuanceOccupations), ex); ;
            }
        }
    }
}

