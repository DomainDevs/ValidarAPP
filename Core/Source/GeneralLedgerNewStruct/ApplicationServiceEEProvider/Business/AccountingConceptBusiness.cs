using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Enums;
using Sistran.Core.Application.GeneralLedgerServices.EEProvider.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Data;
using GENERALLEDGEREN = Sistran.Core.Application.GeneralLedger.Entities;

namespace Sistran.Core.Application.GeneralLedgerServices.EEProvider.Business
{
    internal class AccountingConceptBusiness
    {
        public AccountingAccount GetAccountingNumberByAccountingConcept(string accountingConcept, int? branchId, int? prefixId)
        {
            AccountingAccount accountingAccount = new AccountingAccount();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(GENERALLEDGEREN.AccountingConcept.Properties.AccountingConceptCode, "AccountingConcept").Equal().Constant(Convert.ToInt32(accountingConcept));

            SelectQuery SelectQuery = new SelectQuery();

            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, "AccountingAccount")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountNumber, "AccountingAccount")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountingNature, "AccountingAccount")));
            SelectQuery.AddSelectValue(new SelectValue(new Column(GENERALLEDGEREN.AccountingAccount.Properties.AccountName, "AccountingAccount")));

            Join join = new Join(new ClassNameTable(typeof(GENERALLEDGEREN.AccountingConcept), "AccountingConcept"), new ClassNameTable(typeof(GENERALLEDGEREN.AccountingAccount), "AccountingAccount"), JoinType.Left)
            {
                Criteria = new ObjectCriteriaBuilder()
               .Property(GENERALLEDGEREN.AccountingConcept.Properties.AccountingAccountId, "AccountingConcept")
               .Equal()
               .Property(GENERALLEDGEREN.AccountingAccount.Properties.AccountingAccountId, "AccountingAccount")
               .GetPredicate()
            };

            SelectQuery.Table = join;
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
