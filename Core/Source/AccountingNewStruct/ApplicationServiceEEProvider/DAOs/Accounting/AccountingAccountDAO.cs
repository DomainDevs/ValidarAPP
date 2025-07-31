using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Framework.Queries;
using System;
using System.Linq;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Co.Application.Data;
using System.Data;


namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    internal class AccountingAccountDAO
    {
        public int GetAccountingAccountIdByConceptId(int conceptId)
        {
            int accountingAccountId = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AccountingAccountConcept.Properties.AccountingConceptCode, conceptId);//PaymentConceptCode

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.AccountingAccountConcept), criteriaBuilder.GetPredicate());

            if (businessCollection.Count > 0)
            {
                foreach (ACCOUNTINGEN.AccountingAccountConcept accountingAccountConceptEntity in businessCollection.OfType<ACCOUNTINGEN.AccountingAccountConcept>())
                {
                    accountingAccountId = accountingAccountConceptEntity.AccountingAccountId != null ? Convert.ToInt32(accountingAccountConceptEntity.AccountingAccountId) : 0;
                }
            }
            return accountingAccountId;
        }

        public int GetConceptIdByAccoutingAccountId(int accountingAccountId)
        {
            int conceptId = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.AccountingAccountConcept.Properties.AccountingAccountId, accountingAccountId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.AccountingAccountConcept), criteriaBuilder.GetPredicate());

            if (businessCollection.Count > 0)
            {
                foreach (ACCOUNTINGEN.AccountingAccountConcept accountingAccountConceptEntity in businessCollection.OfType<ACCOUNTINGEN.AccountingAccountConcept>())
                {
                    conceptId = accountingAccountConceptEntity.AccountingConceptCode > 0 ? Convert.ToInt32(accountingAccountConceptEntity.AccountingAccountId) : conceptId;
                }
            }
            return conceptId;
        }
        // AContreras 
        public int ValidateApplicationJournalEntry(int transactionId, int Code)
        {
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("@TECHNICAL_TRANSACTION", transactionId);

            ApplicationDAO applicationDAO = new ApplicationDAO();
            
            DataTable dataTable;

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("ACC.INTEGRATION_VALIDATE", parameters);
            }

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                return Code;
            }
            return -2;
        }
    }
}

    
