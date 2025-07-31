using Sistran.Core.Application.TaxServices.DAOs;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.TaxServices.EEProvider.Assemblers;
using Sistran.Core.Application.TaxServices.EEProvider.DAOs;
using Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.TaxServices.EEProvider.Business
{
    public class TaxBusiness
    {
        internal static TaxCategoryCondition CalculateTax(TaxCategoryCondition taxCategoryCondition)
        {
            if (!taxCategoryCondition.IsRetention)
            {
                if (taxCategoryCondition.RateType == Enums.RateType.Amount)
                {
                    taxCategoryCondition.Amount = taxCategoryCondition.TaxPeriodRate.Rate;
                }
                else
                {
                    taxCategoryCondition.Amount = taxCategoryCondition.BaseAmount * taxCategoryCondition.TaxPeriodRate.Rate * GetFactor((int)taxCategoryCondition.RateType);
                }
            }

            return taxCategoryCondition;
        }

        public static Func<TaxCategoryCondition, TaxCategoryCondition> CalculateRetention(List<TaxCategoryCondition> taxes)
        {
            return (TaxCategoryCondition taxCategoryCondition) =>
            {
                if (taxCategoryCondition.IsRetention)
                {
                    if (taxCategoryCondition.BaseConditionTaxId > 0)
                    {
                        taxCategoryCondition.BaseAmount = Convert.ToDecimal(taxes.Find(x => x.Id == taxCategoryCondition.BaseConditionTaxId)?.Amount);
                    }

                    if (taxCategoryCondition.RateType == Enums.RateType.Amount)
                    {
                        taxCategoryCondition.Amount = taxCategoryCondition.TaxPeriodRate.Rate * -1;
                    }
                    else
                    {
                        taxCategoryCondition.Amount = taxCategoryCondition.BaseAmount * taxCategoryCondition.TaxPeriodRate.Rate * GetFactor((int)taxCategoryCondition.RateType) * -1;
                    }
                }

                return taxCategoryCondition;
            };
        }

        /// <summary>
        /// Gets the factor.
        /// </summary>
        /// <param name="rateTypeCode">The rate type code.</param>
        /// <returns></returns>
        private static decimal GetFactor(int rateTypeCode)
        {
            decimal factor = new decimal(1);
            decimal divider = new decimal(1);
            switch (rateTypeCode)
            {
                case 1:
                    divider = new decimal(100);
                    break;
                case 2:
                    divider = new decimal(1000);
                    break;
                case 3:
                    divider = new decimal(1);
                    break;
            }

            return factor / divider;
        }

        internal static IndividualTaxExemptionDTO GetIndividualTaxExemptionByIndividualId(int individualId, int taxCode, DateTime currentFrom)
        {
            TaxDAO taxDAO = new TaxDAO();
            return taxDAO.GetIndividualTaxExemptionByIndividualId(individualId, taxCode, currentFrom);
        }

        /// <summary>
        /// Calculates the payment taxes by individual identifier accounting concept identifier tax attributes amount.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <param name="accontingConceptId">The acconting concept identifier.</param>
        /// <param name="taxAttributes">The tax attributes.</param>
        /// <param name="amount">The amount.</param>
        /// <returns></returns>
        internal static List<TaxCategoryConditionDTO> CalculatePaymentTaxesByIndividualIdAccountingConceptIdTaxAttributesAmount(int individualId, int accontingConceptId, List<TaxAttributeDTO> taxAttributes, decimal amount)
        {
            TaxDAO taxDAO = new TaxDAO();
            AccountingConceptTaxDAO accountingConceptTaxDAO = new AccountingConceptTaxDAO();
            int branchId = Convert.ToInt32(taxAttributes.Find(x => x.Description.Equals("BRANCH_CODE"))?.Value);
            int roleId = Convert.ToInt32(taxAttributes.Find(x => x.Description.Equals("ROLE_CODE"))?.Value);

            List<AccountingConceptTaxDTO> accountingConceptTaxes = DTOAssembler.CreateAccountingConceptTaxes(accountingConceptTaxDAO.GetAccountingConceptTaxByAccountingConceptIdBranchId(accontingConceptId, branchId));
            List<IndividualTaxCategoryConditionDTO> taxConditions = GetIndividualTaxCategoryCondition(individualId, roleId);

            IEnumerable<TaxCategoryConditionDTO> taxes = from accountingTax in accountingConceptTaxes
                                                         join condition in taxConditions
                                                         on new { accountingTax.TaxId, accountingTax.TaxCategoryId } equals new { condition.TaxId, condition.TaxCategoryId }
                                                         where !condition.IsRetention
                                                         select DTOAssembler.CreateTaxByAccountingTaxCondition(accountingTax, condition, amount);

            IEnumerable<TaxCategoryConditionDTO> retentions = from accountingTax in accountingConceptTaxes
                                                              join condition in taxConditions
                                                              on new { accountingTax.TaxId, accountingTax.TaxCategoryId } equals new { condition.TaxId, condition.TaxCategoryId }
                                                              where condition.IsRetention
                                                              select DTOAssembler.CreateTaxByAccountingTaxCondition(accountingTax, condition, amount);

            List<TaxCategoryCondition> taxesCalculate = taxDAO.GetTaxPeriodRatesByTaxesTaxAttributes(ModelAssembler.CreateTaxes(taxes), ModelAssembler.CreateTaxAttributes(taxAttributes)).Select(CalculateTax).ToList();
            List<TaxCategoryCondition> retentionsCalculate = taxDAO.GetTaxPeriodRatesByTaxesTaxAttributes(ModelAssembler.CreateTaxes(retentions), ModelAssembler.CreateTaxAttributes(taxAttributes)).Select(CalculateRetention(taxesCalculate)).ToList();

            return DTOAssembler.CreateTaxes(taxesCalculate.Concat(retentionsCalculate)).DistinctBy(x => new { x.TaxId, x.TaxCategoryId, x.TaxConditionId}).ToList();
        }

        /// <summary>
        /// GetIndividualTaxCategoryCondition
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns>List<IndividualTaxCategoryConditionDTO></returns>
        internal static List<IndividualTaxCategoryConditionDTO> GetIndividualTaxCategoryCondition(int individualId, int? roleId = 0)
        {
            TaxDAO taxDAO = new TaxDAO();
            return DTOAssembler.CreateIndividualTaxCategoryConditions(taxDAO.GetIndividualTaxCategoryConditionByIndividualIdRoleId(individualId, roleId)).ToList();
        }

        #region tax components
        /// <summary>
        /// Gets the tax Componentes por id
        /// </summary>
        /// <param name="taxId">The tax identifier.</param>
        /// <returns></returns>
        internal static TaxComponentDTO GetTaxComponentByTaxId(int taxId)
        {
            TaxDAO taxDAO = new TaxDAO();
            return taxDAO.GetTaxComponentByTaxId(taxId);
        }

        /// <summary>
        /// Gets the tax components by tax ids.
        /// </summary>
        /// <param name="taxIds">The tax ids.</param>
        /// <returns></returns>
        internal static List<TaxComponentDTO> GetTaxComponentsByTaxIds(List<int> taxIds)
        {
            TaxDAO taxDAO = new TaxDAO();
            return taxDAO.GetTaxComponentsByTaxIds(taxIds);
        }

        /// <summary>
        /// Gets the tax components by conmponents ids.
        /// </summary>
        /// <param name="ComponentsIds">The components ids.</param>
        /// <returns></returns>
        internal static List<TaxComponentDTO> GetTaxComponentsByComponentsIds(List<int> ComponentsIds)
        {
            TaxDAO taxDAO = new TaxDAO();
            return taxDAO.GetTaxComponentsByComponentsIds(ComponentsIds);
        }

        /// <summary>
        ///Obtiene la tasa de los impuestos.
        /// </summary>
        /// <param name="taxId">identificador Impuesto Iva</param>
        /// <param name="branchId">Sucursal</param>
        /// <param name="lineBusinessId">RamoTecnico</param>
        /// <returns></returns>
        internal static List<TaxPayerDTO> GetTaxPayerIds(List<int> taxIds, Int16 branchId, int lineBusinessId)
        {
            TaxDAO taxDAO = new TaxDAO();
            return taxDAO.GetTaxPayerIds(taxIds, branchId, lineBusinessId);
        }
        #endregion tax components
    }
}
