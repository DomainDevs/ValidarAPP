using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Data;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Business
{
    public class AccountingAccountBusiness
    {
        public AccountingAccount GetAccountingAccountByAccountingAccountId(int accountingAccountId)
        {
            AccountingAccount accountingAccount = new AccountingAccount();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, "AccountingAccount").Equal().Constant(Convert.ToInt32(accountingAccountId));

            SelectQuery SelectQuery = new SelectQuery();

            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, "AccountingAccount")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber, "AccountingAccount")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountingNature, "AccountingAccount")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountName, "AccountingAccount")));


            SelectQuery.Table = new ClassNameTable(typeof(GENERALLEDGEREN.AccountingAccount), "AccountingAccount");
            SelectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    accountingAccount.AccountingAccountId = (int)reader["AccountingAccountId"];
                    accountingAccount.Number = (string)reader["AccountNumber"];
                    accountingAccount.AccountingNature = (AccountingNatures)reader["AccountingNature"];
                    accountingAccount.Description = (string)reader["AccountName"];
                }
            }
            return accountingAccount;
        }
    }
}
