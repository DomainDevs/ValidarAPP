using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using TAXEN = Sistran.Core.Application.Tax.Entities;

namespace Sistran.Core.Application.TaxServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        internal static TAXEN.AccountingConceptTax CreateAccountingConceptTax(AccountingConceptTax accountingConceptTax)
        {
            return new TAXEN.AccountingConceptTax(accountingConceptTax.AccountingConceptTaxId)
            {
                BranchCode = accountingConceptTax.Branch.Id,
                EnableAddExpense = accountingConceptTax.EnableAddExpense,
                EnableAutomatic = accountingConceptTax.EnableAutomatic,
                AccountingConceptCode = accountingConceptTax.AccountingConceptId,
                AccountingConceptTaxCode = accountingConceptTax.AccountingConceptTaxId,
                TaxCategoryCode = accountingConceptTax.TaxCategory.Id,
                TaxCode = accountingConceptTax.Tax.Id,
            };
        }
        internal static List<TAXEN.AccountingConceptTax> CreateAccountingConceptTaxes(List<AccountingConceptTax> accountingConceptTaxes)
        {
            List<TAXEN.AccountingConceptTax> entityAccountingConceptTaxes = new List<TAXEN.AccountingConceptTax>();
            foreach (AccountingConceptTax accountingConceptTax in accountingConceptTaxes)
            {
                entityAccountingConceptTaxes.Add(CreateAccountingConceptTax(accountingConceptTax));
            }
            return entityAccountingConceptTaxes;
        }
    }
}
