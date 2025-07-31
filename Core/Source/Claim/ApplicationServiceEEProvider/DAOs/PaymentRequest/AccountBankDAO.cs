using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest;
using Sistran.Core.Framework.DAF.Engine;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest
{
    public class AccountBankDAO
    {
        public List<AccountBank> GetAccountBanksByIndividualId(int individualId)
        {
            List<AccountBank> accountBanks = new List<AccountBank>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.AccountBank.Properties.IndividualId, typeof(UPEN.AccountBank).Name, individualId);

            AccountBankView accountBankView = new AccountBankView();
            ViewBuilder viewBuilder = new ViewBuilder("AccountBankView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, accountBankView);

            if (accountBankView.AccountBank.Count > 0)
            {
                accountBanks = ModelAssembler.CreateAccountBanks(accountBankView.AccountBank);
            }

            return accountBanks;

        }
    }
}
