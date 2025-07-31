using Sistran.Core.Application.TaxServices.EEProvider.Assemblers;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.TaxServices.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.TaxServices.Enums;

namespace Sistran.Core.Application.TaxServices.EEProvider.DAOs
{
    public class AccountingConceptTaxDAO
    {
        public List<AccountingConceptTax> CreateAccountingConceptTaxes(List<AccountingConceptTax> accountingConceptTax)
        {
            List<AccountingConceptTax> accountingConceptTaxes = new List<AccountingConceptTax>();
            List<TAXEN.AccountingConceptTax> entityAccountingConceptTaxes = EntityAssembler.CreateAccountingConceptTaxes(accountingConceptTax);
            foreach (TAXEN.AccountingConceptTax entityAccountingConceptTax in entityAccountingConceptTaxes)
            {
                DataFacadeManager.Insert(entityAccountingConceptTax);
                accountingConceptTaxes = GetAccountingConceptTaxByAccountingConceptIdBranchId(entityAccountingConceptTax.AccountingConceptCode, entityAccountingConceptTax.BranchCode);
            }
            return accountingConceptTaxes;
        }

        public void DeleteAccountingConceptTax(int accountingConceptTaxId)
        {
            PrimaryKey primaryKey = TAXEN.AccountingConceptTax.CreatePrimaryKey(accountingConceptTaxId);
            DataFacadeManager.Delete(primaryKey);
        }


        public List<AccountingConceptTax> GetAccountingConceptTaxByAccountingConceptIdBranchId(int accountingConceptId, int branchId)
        {
            List<AccountingConceptTax> accountingConceptTaxes = new List<AccountingConceptTax>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(TAXEN.AccountingConceptTax.Properties.AccountingConceptCode, typeof(TAXEN.AccountingConceptTax).Name, accountingConceptId);
            filter.And();
            filter.PropertyEquals(TAXEN.AccountingConceptTax.Properties.BranchCode, typeof(TAXEN.AccountingConceptTax).Name, branchId);

            AccountingConceptTaxView accountingConceptTaxView = new AccountingConceptTaxView();
            ViewBuilder viewBuilder = new ViewBuilder("AccountingConceptTaxView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, accountingConceptTaxView);

            if (accountingConceptTaxView.AccountingConceptTaxes.Count > 0)
            {
                accountingConceptTaxes = ModelAssembler.CreateAccountingConceptTaxes(accountingConceptTaxView.AccountingConceptTaxes);
                foreach (AccountingConceptTax accountingConceptTax in accountingConceptTaxes)
                {
                    accountingConceptTax.Tax.Description = accountingConceptTaxView.Taxes.Cast<TAXEN.Tax>().FirstOrDefault(x => x.TaxCode == accountingConceptTax.Tax.Id)?.Description;
                    accountingConceptTax.TaxCategory.Description = accountingConceptTaxView.TaxCategories.Cast<TAXEN.TaxCategory>().FirstOrDefault(x => x.TaxCategoryCode == accountingConceptTax.TaxCategory.Id)?.Description;
                }
            }
            return accountingConceptTaxes;
        }

        //public List<AccountingConceptTax> GetAccountingConceptTaxesByBranchId(int branchId)
        //{
        //    List<AccountingConceptTax> accountingConceptTaxes = new List<AccountingConceptTax>();

        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(TAXEN.AccountingConceptTax.Properties.BranchCode, typeof(TAXEN.AccountingConceptTax).Name);
        //    filter.Equal();
        //    filter.Constant(branchId);
            
        //    AccountingConceptTaxView accountingConceptTaxView = new AccountingConceptTaxView();
        //    ViewBuilder viewBuilder = new ViewBuilder("AccountingConceptTaxView");
        //    viewBuilder.Filter = filter.GetPredicate();

        //    DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, accountingConceptTaxView);

        //    if (accountingConceptTaxView.AccountingConceptTaxes.Count > 0)
        //    {
        //        accountingConceptTaxes = ModelAssembler.CreateAccountingConceptTaxes(accountingConceptTaxView.AccountingConceptTaxes);

        //        foreach (AccountingConceptTax accountingConceptTax in accountingConceptTaxes)
        //        {
        //            TAXEN.Tax entityTax = accountingConceptTaxView.Taxes.Cast<TAXEN.Tax>().FirstOrDefault(x => x.TaxCode == accountingConceptTax.Tax.Id);
        //            TAXEN.TaxCategory entityTaxCategory = accountingConceptTaxView.TaxCategories.Cast<TAXEN.TaxCategory>().FirstOrDefault(x => x.TaxCode == accountingConceptTax.Tax.Id && x.TaxCategoryCode == accountingConceptTax.TaxCategory.Id);
        //            PARAMEN.RateType entityRateType = accountingConceptTaxView.RateTypes.Cast<PARAMEN.RateType>().FirstOrDefault(x => x.RateTypeCode == entityTax.RateTypeCode);

        //            accountingConceptTax.Tax.Description = entityTax.Description;
        //            accountingConceptTax.TaxCategory.Description = entityTaxCategory.Description;
        //            accountingConceptTax.RateType = (RateType)entityRateType.RateTypeCode;                    
        //        }
        //    }
        //    return accountingConceptTaxes;
        //}
    }
}
