using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.DAOs;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using ENUMUN = Sistran.Core.Application.UnderwritingServices.Enums;
using Model = Sistran.Core.Application.UnderwritingServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PayerComponentDAO
    {
        /// <summary>
        /// Calcular Componentes De La Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        public List<Model.PayerComponent> CalculatePayerComponents(Model.Policy policy, List<Model.Risk> risks)
        {
            bool isExpenses = false;
            if (risks?.Count < 1)
            {
                throw new Exception(Errors.ErrorRiskNotFound);
            }
            ConcurrentBag<Model.PayerComponent> payerComponents = new ConcurrentBag<Model.PayerComponent>();
            ComponentView componentView = new ComponentView();
            ViewBuilder builder = new ViewBuilder("ComponentView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(Enums.ComponentType.Premium);
            filter.Constant(Enums.ComponentType.Expenses);
            filter.Constant(Enums.ComponentType.Taxes);
            filter.EndList();
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, componentView);
            }
            if (!componentView.Components.Any())
            {
                return payerComponents.ToList();
            }
            ConcurrentBag<Model.PayerComponent> payerComponentExpenses = new ConcurrentBag<Model.PayerComponent>();
            List<QUOEN.Component> components = componentView.Components.Cast<QUOEN.Component>().ToList();
            if (components?.Count < 1)
            {
                throw new Exception(Errors.ErrorPayerComponents);
            }
            List<QUOEN.ExpenseComponent> expenseComponents = componentView.ExpenseComponents.Cast<QUOEN.ExpenseComponent>().ToList();
            List<int> taxComponents = componentView.Components.Cast<QUOEN.Component>().Where(m => m.ComponentTypeCode == (int)Enums.ComponentType.Taxes).Select(u => u.ComponentCode).ToList();
            var premiumAmt = risks.SelectMany(x => x.Coverages).Where(z => z != null && z.PremiumAmount != 0).Sum(m => m.PremiumAmount);
            decimal baseExpense = 0;
            decimal rateExpense = 0;
            if (premiumAmt != 0)
            {
                isExpenses = IsExpenses();
                foreach (QUOEN.Component component in components.Where(x => x.ComponentTypeCode == (int)Enums.ComponentType.Expenses))
                {
                    Model.PayerComponent payerComponent = new Model.PayerComponent();
                    payerComponent.Component = ModelAssembler.CreateComponent(component);
                    payerComponent.RateType = RateType.FixedValue;
                    QUOEN.ExpenseComponent expenseComponent = expenseComponents.First(x => x.ComponentCode == component.ComponentCode);
                    if (expenseComponent.RuleSetId.GetValueOrDefault() > 0)
                    {
                        payerComponent.BaseAmount = risks.Sum(x => x.Premium);
                        payerComponent = RunRules(policy, payerComponent, expenseComponent.RuleSetId.Value);
                    }
                    else
                    {
                        payerComponent.RateType = (RateType)expenseComponent.RateTypeCode;
                        payerComponent.Rate = expenseComponent.Rate;
                    }
                    rateExpense += rateExpense + Math.Round(payerComponent.Rate, 2);
                    payerComponentExpenses.Add(payerComponent);
                }
                baseExpense = payerComponentExpenses.FirstOrDefault()?.BaseAmount == null ? 0 : payerComponentExpenses.First().BaseAmount;
            }

            decimal totalInsuredAmount = risks.Sum(x => x.AmountInsured);

            ConcurrentBag<string> errorsData = new ConcurrentBag<string>();

            if (components?.Count > 0)
            {
                var coverageAll = risks.SelectMany(x => x.Coverages).ToList();
                var coverageBase = coverageAll.GroupBy(x => x.Id).ToList();
                object objLock = new object();
                List<QUOEN.Component> componentsPremium = components.Where(x => x.ComponentTypeCode == (int)Enums.ComponentType.Premium).ToList();
                Parallel.For(0, coverageBase.Count, ParallelHelper.DebugParallelFor(), u =>
                {
                    try
                    {
                        var coverageList = new List<Model.Coverage>();
                        lock (objLock)
                        {
                            coverageList = (coverageBase[u] as IEnumerable<Model.Coverage>).ToList();
                        }
                        Debug.WriteLine($"{coverageBase[u].FirstOrDefault().Description} : {coverageList.Sum(a => a.PremiumAmount)}");
                        foreach (QUOEN.Component component in componentsPremium)
                        {
                            decimal baseAmount = 0;
                            Model.PayerComponent payerComponent = new Model.PayerComponent();
                            payerComponent.Component = ModelAssembler.CreateComponent(component);
                            payerComponent.Rate = coverageList[0]?.Rate ?? 0;
                            payerComponent.RateType = coverageList[0].RateType;
                            payerComponent.Amount = coverageList.Where(x => x != null).Sum(z => z.PremiumAmount);
                            payerComponent.CoverageId = coverageList[0].Id;
                            payerComponent.LineBusinessId = coverageList[0].SubLineBusiness.LineBusiness.Id;
                            payerComponent.Amount = coverageList.Where(m => m != null).Sum(a => a.PremiumAmount);
                            payerComponent.AmountLocal = decimal.Round(coverageList.Where(m => m != null).Sum(a => a.PremiumAmount) * policy.ExchangeRate.SellAmount, QuoteManager.DecimalRound);
                            object objLockPremium = new object();
                            Parallel.For(0, coverageList.Count, ParallelHelper.DebugParallelFor(), m =>
                            {
                                try
                                {
                                    var coverage = new Model.Coverage();
                                    lock (objLock)
                                    {
                                        coverage = coverageList[m];
                                    }

                                    payerComponent.Rate = coverage.Rate.GetValueOrDefault();


                                    if (policy.Endorsement.EndorsementType == Enums.EndorsementType.Modification && coverage.CoverStatus != Enums.CoverageStatusType.Included)
                                    {
                                        lock (objLockPremium)
                                        {
                                            baseAmount += decimal.Round(coverage.EndorsementLimitAmount, QuoteManager.DecimalRound);
                                        }
                                    }
                                    else
                                    {
                                        lock (objLockPremium)
                                        {
                                            baseAmount += CalculatePremiumBase(coverage, QuoteManager.DecimalRound);
                                        }
                                    }


                                }
                                catch (Exception ex)
                                {
                                    errorsData.Add($"{Errors.ErrorPayerComponents} - {ex.GetBaseException().Message}");
                                }
                            });
                            lock (objLock)
                            {
                                payerComponent.BaseAmount = baseAmount;
                            }
                            payerComponents.Add(new Models.PayerComponent
                            {
                                Component = payerComponent.Component,
                                CoverageId = payerComponent.CoverageId,
                                LineBusinessId = coverageList[0].SubLineBusiness.LineBusiness.Id,
                                BaseAmount = payerComponent.BaseAmount,
                                Amount = payerComponent.Amount,
                                Rate = payerComponent.Rate,
                                RateType = payerComponent.RateType,
                                AmountLocal = payerComponent.AmountLocal
                            });
                            Debug.WriteLine($"{u.ToString()} {coverageBase[u].FirstOrDefault().Description} : {payerComponent.Amount}");
                        }

                        //gastos
                        if (payerComponentExpenses?.Count > 0)
                        {
                            decimal baseExpensesAmount = coverageList.Sum(x => x.PremiumAmount);
                            decimal amount = 0;
                            decimal amountLocal = 0;
                            decimal amountExpense = 0;
                            Model.PayerComponent payerComponentExpense = new Model.PayerComponent();
                            foreach (Model.PayerComponent item in payerComponentExpenses)
                            {
                                IEnumerable<ENUMUN.EndorsementType?> changeEndosements = new List<EndorsementType?> {
                                    ENUMUN.EndorsementType.ChangeAgentEndorsement,
                                    ENUMUN.EndorsementType.ChangePolicyHolderEndorsement,
                                    ENUMUN.EndorsementType.ChangeTermEndorsement,
                                    ENUMUN.EndorsementType.ChangeConsolidationEndorsement,
                                    ENUMUN.EndorsementType.ChangeCoinsuranceEndorsement
                                };

                                if (policy.Endorsement.EndorsementType == ENUMUN.EndorsementType.Cancellation && policy.Endorsement.CancellationTypeId == 1)
                                {
                                    payerComponentExpense = CalculateExpensesAmountByCoverage(item, policy, coverageList, isExpenses);
                                }
                                else if (changeEndosements.Contains(policy.Endorsement.EndorsementType) && policy.Endorsement.CancellationTypeId == 1)
                                {
                                    payerComponentExpense = CalculateExpensesAmountByCoverage(item, policy, coverageList, isExpenses, 1);
                                }
                                else
                                {
                                    amount = 0;
                                    amountLocal = 0;
                                    payerComponentExpense.Component = item.Component;
                                    payerComponentExpense.CoverageId = coverageList[0].Id;
                                    payerComponentExpense.LineBusinessId = coverageList[0].SubLineBusiness.LineBusiness.Id;
                                    payerComponentExpense.Rate = item.Rate;
                                    payerComponentExpense.RateType = item.RateType;
                                    payerComponentExpense.DynamicProperties = item.DynamicProperties;
                                    object objLockExpenses = new object();
                                    Parallel.For(0, coverageList.Count, ParallelHelper.DebugParallelFor(), m =>
                                    {
                                        try
                                        {
                                            var coverage = coverageList[m];
                                            if (totalInsuredAmount != 0)
                                            {
                                                if (coverage.LimitAmount != 0)
                                                {
                                                    lock (objLockExpenses)
                                                    {
                                                        if (isExpenses)
                                                        {
                                                            amountLocal += decimal.Round((((coverage.LimitAmount * policy.ExchangeRate.SellAmount) * payerComponentExpense.Rate) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);
                                                            amount += decimal.Round(((coverage.LimitAmount * (payerComponentExpense.Rate / policy.ExchangeRate.SellAmount)) / totalInsuredAmount), QuoteManager.DecimalRound);
                                                        }
                                                        else
                                                        {
                                                            if ((PrefixRc)policy.Prefix.Id == PrefixRc.Liability)
                                                            {
                                                                if (coverage.IsPrimary == true)
                                                                {
                                                                    amountExpense += ((coverage.LimitAmount * payerComponentExpense.Rate) / totalInsuredAmount);
                                                                    amount += decimal.Round(((coverage.LimitAmount * payerComponentExpense.Rate) / totalInsuredAmount), QuoteManager.DecimalRound);
                                                                    amountLocal += decimal.Round((((coverage.LimitAmount * policy.ExchangeRate.SellAmount) * (payerComponentExpense.Rate * policy.ExchangeRate.SellAmount)) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);
                                                                }

                                                            }
                                                            else
                                                            {
                                                                amountExpense += ((coverage.LimitAmount * payerComponentExpense.Rate) / totalInsuredAmount);
                                                                amount += decimal.Round(((coverage.LimitAmount * payerComponentExpense.Rate) / totalInsuredAmount), QuoteManager.DecimalRound);
                                                                amountLocal += decimal.Round((((coverage.LimitAmount * policy.ExchangeRate.SellAmount) * (payerComponentExpense.Rate * policy.ExchangeRate.SellAmount)) / (totalInsuredAmount * policy.ExchangeRate.SellAmount)), QuoteManager.DecimalRound);

                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            errorsData.Add($"{Errors.ErrorPayerComponents} - {ex.GetBaseException().Message}");
                                        }

                                    });

                                    lock (objLock)
                                    {
                                        payerComponentExpense.AmountExpense += amountExpense;
                                        payerComponentExpense.Amount += amount;
                                        payerComponentExpense.AmountLocal += amountLocal;
                                        payerComponentExpense.BaseAmount = baseExpensesAmount;
                                    }
                                }
                                payerComponents.Add(new Models.PayerComponent
                                {
                                    Component = payerComponentExpense.Component,
                                    CoverageId = payerComponentExpense.CoverageId,
                                    LineBusinessId = payerComponentExpense.LineBusinessId,
                                    BaseAmount = payerComponentExpense.BaseAmount,
                                    Amount = payerComponentExpense.Amount,
                                    AmountExpense = payerComponentExpense.AmountExpense,
                                    Rate = payerComponentExpense.Rate,
                                    RateType = payerComponentExpense.RateType,
                                    DynamicProperties = payerComponentExpense.DynamicProperties,
                                    AmountLocal = payerComponentExpense.AmountLocal
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        errorsData.Add($"{Errors.ErrorPayerComponents} - {ex.GetBaseException().Message}");
                    }

                });
            }

            if (errorsData?.Count > 0)
            {
                throw new System.Exception(string.Join(Environment.NewLine, errorsData.ToList()));
            }

            if (isExpenses)
            {
                if (rateExpense != 0)
                {
                    decimal rateExp = payerComponents.Where(a => a.Component != null && a.Component?.ComponentType.Value == ENUMUN.ComponentType.Expenses).ToList().Sum(m => m.AmountLocal);
                    decimal rateDifference = rateExpense - rateExp;
                    if (rateDifference != 0)
                    {
                        payerComponents.OrderByDescending(x => x.Id).FirstOrDefault(a => a.Component != null && a.Component?.ComponentType.Value == ENUMUN.ComponentType.Expenses).AmountLocal += rateDifference;
                    }

                }
            }

            if (payerComponents.Count > 0 && taxComponents.Count > 0 && policy.BusinessType != Enums.BusinessType.Accepted)
            {
                return AddTaxes(payerComponents.ToList(), taxComponents, policy, components);
            }

            return payerComponents.ToList();
        }

        private decimal CalculatePremiumBase(Model.Coverage coverage, int decimalsRound)
        {
            decimal premiumBase;

            if (coverage.Rate == 0)
            {
                return 0;
            }

            switch (coverage.RateType)
            {
                case RateType.FixedValue:
                    {
                        if (coverage.Rate.HasValue)
                        {
                            if (coverage.CoverStatus.HasValue && coverage.CoverStatus == ENUMUN.CoverageStatusType.Excluded)
                            {
                                premiumBase = 0;
                            }
                            else
                            {
                                premiumBase = coverage.PremiumAmount;
                            }
                        }
                        else
                        {
                            premiumBase = coverage.PremiumAmount;
                        }

                        break;
                    }

                case RateType.Percentage:
                case RateType.Permilage:
                    premiumBase = coverage.SubLimitAmount;
                    break;
                default:
                    premiumBase = 0;
                    break;
            }

            premiumBase = decimal.Round(premiumBase, decimalsRound);

            return premiumBase;
        }

        /// <summary>
        /// Adds the taxes.
        /// </summary>
        /// <param name="payerComponents">The payer components.</param>
        /// <param name="taxComponents">The tax components.</param>
        /// <param name="policy">The policy.</param>
        /// <param name="components">The components.</param>
        /// <returns></returns>
        private List<Model.PayerComponent> AddTaxes(List<Model.PayerComponent> payerComponents, List<int> taxComponents, Model.Policy policy, List<QUOEN.Component> components)
        {
            int exemptionPct = 0;
            IndividualTaxExemptionDTO individualTaxExemptionDTO = null;
            List<TaxComponentDTO> taxComponentsDto = DelegateService.taxServiceCore.GetTaxComponentsByComponentsIds(taxComponents);
            individualTaxExemptionDTO = DelegateService.taxServiceCore.GetIndividualTaxExemptionByIndividualId(policy.Holder.IndividualId, taxComponentsDto.First().Id, policy.CurrentFrom);
            if (individualTaxExemptionDTO != null)
            {
                exemptionPct = individualTaxExemptionDTO.ExemptionPercentage;
            }
            var groupedComponents = payerComponents
                .Where(x => x.CoverageId > 0 && x.LineBusinessId > 0)
                .GroupBy(x => x.LineBusinessId, x => x, (key, g) => new { LineBusinessId = key, PayerComponents = g.ToList() }).ToList();
            foreach (var group in groupedComponents)
            {
                AddTaxes(group.LineBusinessId, group.PayerComponents, taxComponentsDto, Convert.ToInt16(policy.Branch.Id), components, exemptionPct, policy.ExchangeRate.SellAmount);
            }
            payerComponents.Clear();
            payerComponents.AddRange(groupedComponents.SelectMany(x => x.PayerComponents));
            return payerComponents;
        }

        private void AddTaxes(int lineBusinessId, List<Model.PayerComponent> payerComponents, List<TaxComponentDTO> taxComponents, short branchId, List<QUOEN.Component> components, int exemptionPct, decimal sellAmount)
        {

            List<TaxPayerDTO> taxPayerDTOs = DelegateService.taxServiceCore.GetTaxPayerIds(taxComponents.Select(m => m.Id).Distinct().ToList(), branchId, lineBusinessId);
            if (taxPayerDTOs != null && taxPayerDTOs.Any())
            {
                foreach (TaxComponentDTO taxComponent in taxComponents)
                {
                    TaxPayerDTO TaxPayerDTO = taxPayerDTOs.First(a => a.Id == taxComponent.Id);
                    if (TaxPayerDTO != null)
                    {
                        decimal totalComponents = payerComponents.Sum(x => x.Amount);
                        decimal total = CalculateTax((int)RateType.Percentage, TaxPayerDTO.Rate, totalComponents, exemptionPct);
                        QUOEN.Component component = components.First(x => x.ComponentCode == taxComponent.ComponentId);
                        Model.PayerComponent payerComponentTax = new Model.PayerComponent
                        {
                            LineBusinessId = lineBusinessId,
                            Rate = total == 0 ? 0 : TaxPayerDTO.Rate,
                            RateType = RateType.Percentage,
                            Component = ModelAssembler.CreateComponent(component),
                            BaseAmount = decimal.Round(totalComponents, QuoteManager.DecimalRound),
                            Amount = total,
                            TaxId = taxComponent.Id,
                            TaxConditionId = TaxPayerDTO.TaxConditionId,
                            AmountLocal = decimal.Round(total * sellAmount, QuoteManager.DecimalRound)
                        };
                        if (payerComponents.Exists(x => x.Component.Id == payerComponentTax.Component.Id))
                        {
                            payerComponents.First(x => x.Component.Id == payerComponentTax.Component.Id).BaseAmount += payerComponentTax.BaseAmount;
                            payerComponents.First(x => x.Component.Id == payerComponentTax.Component.Id).Amount += payerComponentTax.Amount;
                        }
                        else
                        {
                            payerComponents.Add(payerComponentTax);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Ejecutar reglas a nivel de componente
        /// </summary>
        /// <param name="payerComponent">Componente</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns>Componente</returns>
        public Model.PayerComponent RunRules(Model.Policy policy, Model.PayerComponent payerComponent, int ruleSetId)
        {
            Rules.Facade facade = new Rules.Facade();

            EntityAssembler.CreateFacadeGeneral(facade, policy);
            EntityAssembler.CreateFacadeComponent(facade, payerComponent);

            facade = RulesEngineDelegate.ExecuteRules(ruleSetId, facade);

            payerComponent.Rate = facade.GetConcept<decimal>(RuleConceptComponent.Rate);
            payerComponent.RateType = (RateType)facade.GetConcept<int>(RuleConceptComponent.RateTypeCode);
            payerComponent.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);

            return payerComponent;
        }



        private decimal CalculateTax(int rateTypeCode, decimal rate, decimal amount, decimal excemptionPct)
        {
            decimal factor = QuoteManager.GetFactor((TaxType)rateTypeCode);
            decimal tax = 0;

            if (rateTypeCode == (int)RateType.FixedValue)
            {
                tax = rate;
            }
            else
            {
                tax = amount * rate * factor;
            }

            return decimal.Round((tax - (tax * (excemptionPct / 100))), QuoteManager.DecimalRound);
        }

        private bool IsExpenses()
        {
            Parameter param = DelegateService.commonServiceCore.GetParameterByParameterId(ParameterNumber.IsExpensesLocal);
            return (bool)param.BoolParameter;
        }

        private decimal CalculateExpensesDifference(decimal premium, decimal premiumBase, int round)
        {
            return decimal.Round((premiumBase + premiumBase), round);
        }

        private decimal GetExpensByPolicyId(int policyId)
        {
            return ExpenseDAO.GetExpensByPolicyId(policyId);
        }

        public Model.PayerComponent CalculateExpensesAmount(Model.PayerComponent item, Model.Policy policy, List<Model.Coverage> coverages, bool isExpenses)
        {
            ExpenseDAO expenseDAO = new ExpenseDAO();
            return expenseDAO.CalculateExpensesAmount(item, policy, coverages, isExpenses);
        }

        public Model.PayerComponent CalculateExpensesAmountByCoverage(Model.PayerComponent item, Model.Policy policy, List<Model.Coverage> coverages, bool isExpenses, int factor = -1)
        {
            ExpenseDAO expenseDAO = new ExpenseDAO();
            return expenseDAO.CalculateExpensesAmountByCoverage(item, policy, coverages, isExpenses, factor);
        }
    }
}