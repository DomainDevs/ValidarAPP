using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.TaxServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.TaxServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        internal static AccountingConceptTaxDTO CreateAccountingConceptTax(AccountingConceptTax accountingConceptTax)
        {
            return new AccountingConceptTaxDTO
            {
                AccountingConceptTaxId = accountingConceptTax.AccountingConceptTaxId,
                AccountingConceptId = accountingConceptTax.AccountingConceptId,
                TaxId = accountingConceptTax.Tax.Id,
                TaxDescription = accountingConceptTax.Tax.Description,
                TaxCategoryId = accountingConceptTax.TaxCategory.Id,
                TaxCategoryDescription = accountingConceptTax.TaxCategory.Description,
                BranchId = accountingConceptTax.Branch.Id,
                EnableAddExpense = accountingConceptTax.EnableAddExpense,
                EnableAutomatic = accountingConceptTax.EnableAutomatic,
                RateTypeId = Convert.ToInt32(accountingConceptTax.RateType),
                RateTypeDescription = accountingConceptTax.RateType.ToString()
            };
        }

        internal static List<AccountingConceptTaxDTO> CreateAccountingConceptTaxes(List<AccountingConceptTax> accountingConceptTax)
        {
            List<AccountingConceptTaxDTO> accountingConceptTaxDTOs = new List<AccountingConceptTaxDTO>();
            foreach (AccountingConceptTax acountingConceptTx in accountingConceptTax)
            {
                accountingConceptTaxDTOs.Add(CreateAccountingConceptTax(acountingConceptTx));
            }
            return accountingConceptTaxDTOs;
        }

        internal static TaxCategoryConditionDTO CreateTaxByAccountingTaxCondition(AccountingConceptTaxDTO accountingConceptTax, IndividualTaxCategoryConditionDTO individualTaxCategoryCondition, decimal amount)
        {
            return new TaxCategoryConditionDTO
            {
                TaxId = accountingConceptTax.TaxId,
                TaxDescription = accountingConceptTax.TaxDescription,
                TaxCategoryId = accountingConceptTax.TaxCategoryId,
                TaxCategoryDescription = accountingConceptTax.TaxCategoryDescription,
                RateTypeId = individualTaxCategoryCondition.RateTypeId,
                TaxConditionId = individualTaxCategoryCondition.TaxConditionId,
                TaxConditionDescription = individualTaxCategoryCondition.TaxConditionDescription,
                IsRetention = individualTaxCategoryCondition.IsRetention,
                BaseAmount = amount,
                RetentionTaxId = individualTaxCategoryCondition.RetentionTaxId,
                BaseConditionTaxId = individualTaxCategoryCondition.BaseConditionTaxId,
                EconomicActivityTaxId = individualTaxCategoryCondition.EconomicActivityTaxId
            };
        }

        internal static TaxCategoryConditionDTO CreateTax(TaxCategoryCondition taxCategoryCondition)
        {
            return new TaxCategoryConditionDTO
            {
                TaxId = taxCategoryCondition.Id,
                TaxDescription = taxCategoryCondition.Description,
                BaseAmount = taxCategoryCondition.BaseAmount,
                IsRetention = taxCategoryCondition.IsRetention,
                RateTypeId = (int)taxCategoryCondition.RateType,
                TaxCategoryId = taxCategoryCondition.TaxCategory.Id,
                TaxCategoryDescription = taxCategoryCondition.TaxCategory.Description,
                TaxConditionId = taxCategoryCondition.TaxCondition.Id,
                TaxConditionDescription = taxCategoryCondition.TaxCondition.Description,
                TaxRate = taxCategoryCondition.TaxPeriodRate.Rate,
                TaxValue = taxCategoryCondition.IsRetention ? 0 : taxCategoryCondition.Amount,
                RetentionValue = taxCategoryCondition.IsRetention ? taxCategoryCondition.Amount : 0,
                AccountingConceptId = taxCategoryCondition.AccountingConceptId,
                EconomicActivityTaxId = taxCategoryCondition.EconomicActivityTaxId
            };
        }

        internal static IEnumerable<TaxCategoryConditionDTO> CreateTaxes(IEnumerable<TaxCategoryCondition> taxCategoryConditions)
        {
            return taxCategoryConditions.Select(CreateTax);
        }

        internal static TaxAttributeDTO CreateTaxAttribute(TaxAttribute taxAttribute)
        {
            return new TaxAttributeDTO
            {
                Id = taxAttribute.Id,
                Description = taxAttribute.Description,
                Value = taxAttribute.Value
            };
        }

        internal static IEnumerable<TaxAttributeDTO> CreateTaxAttributes(IEnumerable<TaxAttribute> taxAttributes)
        {
            return taxAttributes.Select(CreateTaxAttribute);
        }

        internal static IndividualTaxCategoryConditionDTO CreateIndividualTaxCategoryCondition(IndividualTaxCategoryCondition individualTaxCategoryCondition)
        {
            return new IndividualTaxCategoryConditionDTO
            {
                IndividualId = individualTaxCategoryCondition.IndividualId,
                Enabled = individualTaxCategoryCondition.Tax.IsEnable,
                IsRetention = individualTaxCategoryCondition.Tax.IsRetention,
                TaxId = individualTaxCategoryCondition.Tax.Id,
                TaxDescription = individualTaxCategoryCondition.Tax.Description,
                RateTypeId = (int)individualTaxCategoryCondition.Tax.RateType,
                RateTypeDescription = individualTaxCategoryCondition.Tax.RateType.ToString(),
                TaxCategoryId = individualTaxCategoryCondition.Tax.TaxCategory.Id,
                TaxCategoryDescription = individualTaxCategoryCondition.Tax.TaxCategory.Description,
                TaxConditionId = individualTaxCategoryCondition.Tax.TaxCondition.Id,
                TaxConditionDescription = individualTaxCategoryCondition.Tax.TaxCondition.Description,
                RetentionTaxId = individualTaxCategoryCondition.Tax.RetentionTaxId,
                BaseConditionTaxId = individualTaxCategoryCondition.Tax.BaseConditionTaxId,
                EconomicActivityTaxId = individualTaxCategoryCondition.Tax.EconomicActivityTaxId,
                BranchId = individualTaxCategoryCondition.Tax.Branch?.Id,
                BranchDescription = individualTaxCategoryCondition.Tax.Branch.Description
            };
        }

        internal static IEnumerable<IndividualTaxCategoryConditionDTO> CreateIndividualTaxCategoryConditions(IEnumerable<IndividualTaxCategoryCondition> individualTaxCategoryConditions)
        {
            return individualTaxCategoryConditions.Select(CreateIndividualTaxCategoryCondition);
        }
    }
}
