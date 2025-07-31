using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.TaxServices.Enums;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using TAXMOD = Sistran.Core.Application.TaxServices.Models;

namespace Sistran.Core.Application.TaxServices.EEProvider.Assemblers

{
    public class ModelAssembler
    {
        #region Tax

        /// <summary>
        /// Creates the tax.
        /// </summary>
        /// <returns></returns>
        public static Models.Tax CreateTax(TAXEN.Tax tax)
        {
            return new Models.Tax
            {
                Id = tax.TaxCode,
                Description = tax.Description
            };
        }

        /// <summary>
        /// Creates the tax.
        /// </summary>
        /// <returns></returns>
        public static List<Models.Tax> CreateTaxs(BusinessCollection businessCollection)
        {
            List<Models.Tax> tax = new List<Models.Tax>();

            foreach (TAXEN.Tax field in businessCollection)
            {
                tax.Add(ModelAssembler.CreateTax(field));
            }

            return tax;
        }

        #endregion

        #region TaxCondition

        /// <summary>
        /// Creates the taxcondition.
        /// </summary>
        /// <returns></returns>
        public static Models.TaxCondition CreateTaxCondition(TAXEN.TaxCondition tax)
        {
            return new Models.TaxCondition
            {
                Id = tax.TaxConditionCode,
                Description = tax.Description
            };
        }

        /// <summary>
        /// Creates the taxcondition.
        /// </summary>
        /// <returns></returns>
        public static Models.TaxCategory CreateTaxCategory(TAXEN.TaxCategory tax)
        {
            return new Models.TaxCategory
            {
                Id = tax.TaxCategoryCode,
                Description = tax.Description
            };
        }

        /// <summary>
        /// Creates the taxcondition.
        /// </summary>
        /// <returns></returns>
        public static List<Models.TaxCondition> CreateTaxConditions(BusinessCollection businessCollection)
        {
            List<Models.TaxCondition> taxCondition = new List<Models.TaxCondition>();

            foreach (TAXEN.TaxCondition field in businessCollection)
            {
                taxCondition.Add(ModelAssembler.CreateTaxCondition(field));
            }

            return taxCondition;
        }
        public static List<Models.TaxCategory> CreateTaxCategory(BusinessCollection businessCollection)
        {
            List<Models.TaxCategory> taxCategories = new List<Models.TaxCategory>();

            foreach (TAXEN.TaxCategory field in businessCollection)
            {
                taxCategories.Add(ModelAssembler.CreateTaxCategory(field));
            }

            return taxCategories;
        }

        #endregion

        internal static TaxPeriodRate CreateTaxPeriodRate(TAXEN.TaxPeriodRate entityTaxPeriodRate)
        {
            return new TaxPeriodRate
            {
                Id = entityTaxPeriodRate.TaxRateId,
                AdditionalRate = entityTaxPeriodRate.AdditionalRate,
                BaseTaxAdditional = entityTaxPeriodRate.BaseTaxIncInAdditional,
                CurrentFrom = entityTaxPeriodRate.CurrentFrom,
                CurrentTo = Convert.ToDateTime(entityTaxPeriodRate.CurrentTo),
                MinAdditionalBaseAMT = entityTaxPeriodRate.MinAdditionalBaseAmount,
                MinAdditionalTaxAMT = entityTaxPeriodRate.MinTaxAmount,
                MinBaseAMT = entityTaxPeriodRate.MinBaseAmount,
                MinTaxAMT = entityTaxPeriodRate.MinTaxAmount,
                Rate = entityTaxPeriodRate.Rate
            };
        }

        public static AccountingConceptTax CreateAccountingConceptTax(TAXEN.AccountingConceptTax entityAccountingConceptTax)
        {
            return new AccountingConceptTax
            {
                AccountingConceptTaxId = entityAccountingConceptTax.AccountingConceptTaxCode,
                AccountingConceptId = entityAccountingConceptTax.AccountingConceptCode,
                Tax = new TAXMOD.Tax
                {
                    Id = entityAccountingConceptTax.TaxCode,

                },
                TaxCategory = new TaxCategory
                {
                    Id = entityAccountingConceptTax.TaxCategoryCode
                },
                Branch = new Branch
                {
                    Id = entityAccountingConceptTax.BranchCode
                },
                EnableAddExpense = entityAccountingConceptTax.EnableAddExpense,
                EnableAutomatic = entityAccountingConceptTax.EnableAutomatic
            };
        }

        public static List<AccountingConceptTax> CreateAccountingConceptTaxes(BusinessCollection businessCollection)
        {
            List<AccountingConceptTax> paymentConceptTaxs = new List<AccountingConceptTax>();
            foreach (TAXEN.AccountingConceptTax field in businessCollection)
            {
                paymentConceptTaxs.Add(CreateAccountingConceptTax(field));
            }
            return paymentConceptTaxs;
        }

        public static AccountingConceptTax CreateAccountingConceptTax(AccountingConceptTaxDTO accountingConceptTaxDTO)
        {
            return new AccountingConceptTax
            {
                AccountingConceptTaxId = accountingConceptTaxDTO.AccountingConceptTaxId,
                AccountingConceptId = accountingConceptTaxDTO.AccountingConceptId,
                Tax = new TAXMOD.Tax
                {
                    Id = accountingConceptTaxDTO.TaxId,

                },
                Branch = new Branch
                {
                    Id = accountingConceptTaxDTO.BranchId
                },
                TaxCategory = new TaxCategory
                {
                    Id = accountingConceptTaxDTO.TaxCategoryId
                },
                EnableAddExpense = accountingConceptTaxDTO.EnableAddExpense,
                EnableAutomatic = accountingConceptTaxDTO.EnableAutomatic
            };
        }

        public static List<AccountingConceptTax> CreateAccountingConceptTaxes(List<AccountingConceptTaxDTO> accountingConceptTaxesDTO)
        {
            List<AccountingConceptTax> accountingConceptTaxes = new List<AccountingConceptTax>();

            foreach (AccountingConceptTaxDTO accountingConceptTaxDTO in accountingConceptTaxesDTO)
            {
                accountingConceptTaxes.Add(CreateAccountingConceptTax(accountingConceptTaxDTO));
            }

            return accountingConceptTaxes;
        }

        public static TaxCategoryCondition CreateTax(TaxCategoryConditionDTO taxCategoryCondition)
        {
            return new TaxCategoryCondition
            {
                Id = taxCategoryCondition.TaxId,
                Description = taxCategoryCondition.TaxDescription,
                TaxCategory = new TaxCategory
                {
                    Id = taxCategoryCondition.TaxCategoryId,
                    Description = taxCategoryCondition.TaxCategoryDescription
                },
                TaxCondition = new TaxCondition
                {
                    Id = taxCategoryCondition.TaxConditionId,
                    Description = taxCategoryCondition.TaxConditionDescription
                },
                TaxPeriodRate = new TaxPeriodRate
                {
                    Rate = taxCategoryCondition.TaxRate
                },
                BaseAmount = taxCategoryCondition.BaseAmount,
                IsRetention = taxCategoryCondition.IsRetention,
                RateType = (RateType)taxCategoryCondition.RateTypeId,
                RetentionTaxId = taxCategoryCondition.RetentionTaxId,
                BaseConditionTaxId = taxCategoryCondition.BaseConditionTaxId
            };
        }

        public static IEnumerable<TaxCategoryCondition> CreateTaxes(IEnumerable<TaxCategoryConditionDTO> taxCategoryConditionsDTO)
        {
            return taxCategoryConditionsDTO.Select(CreateTax);
        }

        public static TaxAttribute CreateTaxAttribute(TaxAttributeDTO taxAttributeDTO)
        {
            return new TaxAttribute
            {
                Id = taxAttributeDTO.Id,
                Description = taxAttributeDTO.Description,
                Value = taxAttributeDTO.Value
            };
        }

        public static IEnumerable<TaxAttribute> CreateTaxAttributes(IEnumerable<TaxAttributeDTO> taxAttributesDTO)
        {
            return taxAttributesDTO.Select(CreateTaxAttribute);
        }

        internal static TaxAttribute CreateTaxAttributes(TAXEN.TaxAttribute entityTaxAttribute)
        {
            return new TaxAttribute
            {
                Id = entityTaxAttribute.TaxAttributeTypeCode
            };
        }

        internal static IEnumerable<TaxAttribute> CreateTaxAttributes(BusinessCollection businessCollection)
        {
            foreach (TAXEN.TaxAttribute entityTaxAttribute in businessCollection)
            {
                yield return CreateTaxAttributes(entityTaxAttribute);
            }
        }

        internal static TaxAttribute CreateTaxAttributesByTaxAttributeType(TAXEN.TaxAttributeType entityTaxAttributeType)
        {
            return new TaxAttribute
            {
                Id = entityTaxAttributeType.TaxAttributeTypeCode,
                Description = entityTaxAttributeType.Description
            };
        }

        internal static IEnumerable<TaxAttribute> CreateTaxAttributesByTaxAttributeTypes(BusinessCollection businessCollection)
        {
            foreach (TAXEN.TaxAttributeType entityTaxAttributeType in businessCollection)
            {
                yield return CreateTaxAttributesByTaxAttributeType(entityTaxAttributeType);
            }
        }

        internal static IndividualTaxCategoryCondition CreateIndividualTaxCategoryCondition(TAXEN.IndividualTax entityIndividualTax)
        {
            return new IndividualTaxCategoryCondition
            {
                IndividualId = entityIndividualTax.IndividualId,
                Tax = new TaxCategoryCondition
                {
                    //Id = entityIndividualTax.TaxCode,
                    //TaxCategory = new TaxCategory
                    //{
                    //    Id = Convert.ToInt32(entityIndividualTax.TaxCategoryCode)
                    //},
                    //TaxCondition = new TaxCondition
                    //{
                    //    Id = entityIndividualTax.TaxConditionCode
                    //}
                }
            };
        }

        internal static IEnumerable<IndividualTaxCategoryCondition> CreateIndividualTaxCategoryConditions(BusinessCollection businessCollection)
        {
            foreach (TAXEN.IndividualTax entityIndividualTax in businessCollection)
            {
                yield return CreateIndividualTaxCategoryCondition(entityIndividualTax);
            }
        }

        internal static TaxCategoryCondition CreateTaxCategoryCondition(TAXEN.Tax entityTax, TAXEN.TaxCategory entityTaxCategory, TAXEN.TaxCondition entityTaxCondition)
        {
            return new TaxCategoryCondition
            {
                Id = entityTax.TaxCode,
                Description = entityTax.Description,
                IsEnable = entityTax.Enabled,
                RateType = (RateType)entityTax.RateTypeCode,
                IsRetention = entityTax.IsRetention,
                RetentionTaxId = entityTax.RetentionTaxCode,
                TaxCategory = new TaxCategory
                {
                    Id = Convert.ToInt32(entityTaxCategory?.TaxCategoryCode),
                    Description = entityTaxCategory?.Description
                },
                TaxCondition = new TaxCondition
                {
                    Id = entityTaxCondition.TaxConditionCode,
                    Description = entityTaxCondition.Description
                }
            };
        }
        internal static IndividualTaxExemptionDTO CreateIndividualTaxExemptionDTO(TAXEN.IndividualTaxExemption individualTaxExemption)
        {
            if (individualTaxExemption != null)
            {
                var mapper = AutoMapperAssembler.CreateMapCompanyIndividualTaxExemption();
                return mapper.Map<TAXEN.IndividualTaxExemption, IndividualTaxExemptionDTO>(individualTaxExemption);
            }
            else
            {
                return null;
            }
        }
        #region Activity Economic Tax
        /// <summary>
        /// Creates the EconomicActivityTax
        /// </summary>
        /// <returns></returns>
        public static List<Models.EconomicActivityTax> CreateEconomicActivitesTax(BusinessCollection businessCollection)
        {
            List<Models.EconomicActivityTax> economicActivityTax = new List<Models.EconomicActivityTax>();

            foreach (TAXEN.EconomicActivityTax field in businessCollection)
            {
                economicActivityTax.Add(ModelAssembler.CreateEconomicActivityTax(field));
            }

            return economicActivityTax;
        }
        /// <summary>
        /// Creates the EconomicActivityTax
        /// </summary>
        /// <returns></returns>
        public static Models.EconomicActivityTax CreateEconomicActivityTax(TAXEN.EconomicActivityTax economicActivityTax)
        {
            return new Models.EconomicActivityTax
            {
                CityCode = economicActivityTax?.CityCode ?? 0,
                CountryCode = economicActivityTax?.CountryCode ?? 0,
                EconomicActivityCode = economicActivityTax?.EconomicActivityCode ?? 0,
                Description = economicActivityTax.Description,
                EconomicActivityTaxId = economicActivityTax.EconomicActivityTaxId,
                StateCode = economicActivityTax?.StateCode ?? 0
            };
        }

        #endregion
        #region taxcomponent
        /// <summary>
        /// Gets the tax by tax ids.
        /// </summary>
        /// <param name="taxComponent">The tax component.</param>
        /// <returns></returns>
        internal static TaxComponentDTO CreateTaxComponent(TAXEN.TaxComponent taxComponent)
        {
            if (taxComponent != null)
            {
                var mapper = AutoMapperAssembler.CreateMapTaxComponent();
                return mapper.Map<TAXEN.TaxComponent, TaxComponentDTO>(taxComponent);
            }
            else
            {
                return null;
            }
        }
        internal static List<TaxComponentDTO> CreateTaxsComponents(List<TAXEN.TaxComponent> taxComponents)
        {
            if (taxComponents != null)
            {
                var mapper = AutoMapperAssembler.CreateMapTaxComponent();
                return mapper.Map<List<TAXEN.TaxComponent>, List<TaxComponentDTO>>(taxComponents);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}
