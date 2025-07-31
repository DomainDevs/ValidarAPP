using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ENUMISS = Sistran.Core.Application.UnderwritingServices.Enums;
using MODUS = Sistran.Core.Application.UnderwritingServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using TP = Sistran.Core.Application.Utilities.Utility;
using Sistran.Core.Application.TaxServices.DTOs;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    class PayerComponentDAO
    {
        /// <summary>
        /// Calcular Componentes De La Póliza
        /// </summary>
        /// <param name="policy">Póliza</param>
        /// <param name="risks">Riesgos</param>
        /// <returns>Componentes</returns>
        public List<PayerComponent> CalculateTaxComponents(int branchId, Risk risk)
        {
            int lineBusinessId = 0;
            List<PayerComponent> payerComponents = new List<PayerComponent>();
            ComponentView componentView = new ComponentView();
            ViewBuilder builder = new ViewBuilder("ComponentView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
            filter.Equal();
            filter.Constant(ENUMISS.ComponentType.Taxes);
            filter.EndList();
            builder.Filter = filter.GetPredicate();

            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, componentView);
            }

            if (!componentView.Components.Any())
            {
                return payerComponents;
            }
            List<QUOEN.Component> components = componentView.Components.Cast<QUOEN.Component>().ToList();
            List<int> taxComponents = componentView.Components.Cast<QUOEN.Component>().Where(m => m.ComponentTypeCode == (int)ENUMISS.ComponentType.Taxes).Select(u => u.ComponentCode).ToList();
            List<TaxComponentDTO> taxComponentsDtos = DelegateService.taxServiceCore.GetTaxComponentsByComponentsIds(taxComponents);
            List<TaxPayerDTO> taxPayerDTOs = DelegateService.taxServiceCore.GetTaxPayerIds(taxComponentsDtos.Select(m => m.Id).Distinct().ToList(), Convert.ToInt16(branchId), lineBusinessId);
            foreach (TaxComponentDTO taxComponent in taxComponentsDtos)
            {
                TaxPayerDTO TaxPayerDTO = taxPayerDTOs.First(a => a.Id == taxComponent.Id);
                if (TaxPayerDTO != null)
                {
                    decimal totalComponents = payerComponents.Sum(x => x.Amount);                   
                    QUOEN.Component component = components.First(x => x.ComponentCode == taxComponent.ComponentId);
                    PayerComponent payerComponentTax = new PayerComponent
                    {
                        LineBusinessId = lineBusinessId,
                        Rate = TaxPayerDTO.Rate,
                        RateType = RateType.Percentage,
                        Component = ModelAssembler.CreateComponent(component),
                        BaseAmount = decimal.Round(totalComponents, QuoteManager.DecimalRound),
                        Amount = decimal.Round(totalComponents * TaxPayerDTO.Rate / 100, QuoteManager.DecimalRound),
                        TaxId = taxComponent.Id,
                        TaxConditionId = TaxPayerDTO.TaxConditionId
                    };
                    payerComponents.Add(payerComponentTax);
                }
            }          
            return payerComponents;
        }
        public List<PayerComponent> CalculatePayerComponents(CompanyPolicy policy, List<Risk> risks, bool returnExpenses)
        {
            if (risks?.Count < 1)
            {
                throw new ArgumentException(Errors.ErrorRiskNoExist);
            }
            List<PayerComponent> payerComponents = new List<PayerComponent>();
            ComponentView componentView = new ComponentView();
            ViewBuilder builder = new ViewBuilder("ComponentView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(ENUMISS.ComponentType.Premium);
            filter.Constant(ENUMISS.ComponentType.Expenses);
            filter.Constant(ENUMISS.ComponentType.Taxes);
            filter.EndList();
            builder.Filter = filter.GetPredicate();
            using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, componentView);
            }
            if (!componentView.Components.Any())
            {
                return payerComponents;
            }
            List<PayerComponent> payerComponentExpenses = new List<PayerComponent>();
            List<QUOEN.Component> components = componentView.Components.Cast<QUOEN.Component>().ToList();
            List<QUOEN.ExpenseComponent> expenseComponents = componentView.ExpenseComponents.Cast<QUOEN.ExpenseComponent>().ToList();
            //List<TAXEN.TaxComponent> taxComponents = componentView.TaxComponents.Cast<TAXEN.TaxComponent>().ToList();
            List<int> taxComponents = componentView.Components.Cast<QUOEN.Component>().Where(m => m.ComponentTypeCode == (int)ENUMISS.ComponentType.Taxes).Select(u => u.ComponentCode).ToList();
            decimal premiumAmt = risks.SelectMany(x => x.Coverages).Where(z => z != null && z.PremiumAmount != 0).Sum(m => m.PremiumAmount);
            if (premiumAmt != 0)
            {
                foreach (QUOEN.Component component in components.Where(x => x.ComponentTypeCode == (int)ENUMISS.ComponentType.Expenses))
                {
                    PayerComponent payerComponent = new PayerComponent();
                    payerComponent.Component = ModelAssembler.CreateComponent(component);
                    payerComponent.RateType = RateType.Percentage;
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

                    payerComponentExpenses.Add(payerComponent);
                }
            }
            decimal totalInsuredAmount = risks.Sum(x => x.AmountInsured);
            Parallel.For(0, risks.Count, ParallelHelper.DebugParallelFor(), u =>
               {
                   var risk = risks[u];
                   TP.Parallel.For(0, risk.Coverages.Count, y =>
                   {
                       var coverage = risk.Coverages[y];

                       foreach (QUOEN.Component component in components.Where(x => x.ComponentTypeCode == (int)ENUMISS.ComponentType.Premium))
                       {
                           PayerComponent payerComponent = new PayerComponent();
                           payerComponent.Component = ModelAssembler.CreateComponent(component);
                           payerComponent.Rate = coverage.Rate.GetValueOrDefault();
                           payerComponent.RateType = coverage.RateType;
                           payerComponent.Amount = coverage.PremiumAmount;
                           payerComponent.CoverageId = coverage.Id;
                           payerComponent.LineBusinessId = coverage.SubLineBusiness.LineBusiness.Id;

                           if (policy.Endorsement.EndorsementType == ENUMISS.EndorsementType.Modification && risk.Status != ENUMISS.RiskStatusType.Included)
                           {
                               payerComponent.BaseAmount = decimal.Round(coverage.EndorsementLimitAmount, QuoteManager.DecimalRound);
                           }
                           else
                           {
                               payerComponent.BaseAmount = CalculatePremiumBase(coverage, 100, QuoteManager.DecimalRound);
                           }

                           if (payerComponents.Exists(x => x.Component.Id == payerComponent.Id && x.CoverageId == payerComponent.CoverageId))
                           {
                               payerComponents.First(x => x.Component.Id == payerComponent.Id && x.CoverageId == payerComponent.CoverageId).BaseAmount += payerComponent.BaseAmount;
                               payerComponents.First(x => x.Component.Id == payerComponent.Id && x.CoverageId == payerComponent.CoverageId).Amount += payerComponent.Amount;
                           }
                           else
                           {
                               payerComponents.Add(new PayerComponent
                               {
                                   Component = payerComponent.Component,
                                   CoverageId = payerComponent.CoverageId,
                                   LineBusinessId = payerComponent.LineBusinessId,
                                   BaseAmount = payerComponent.BaseAmount,
                                   Amount = payerComponent.Amount,
                                   Rate = payerComponent.Rate,
                                   RateType = payerComponent.RateType
                               });
                           }
                           if (payerComponentExpenses?.Count > 0)
                           {
                               foreach (PayerComponent item in payerComponentExpenses)
                               {
                                   PayerComponent payerComponentExpense = new PayerComponent();
                                   payerComponentExpense.Component = item.Component;
                                   payerComponentExpense.CoverageId = payerComponent.CoverageId;
                                   payerComponentExpense.LineBusinessId = payerComponent.LineBusinessId;
                                   payerComponentExpense.BaseAmount = payerComponent.BaseAmount;
                                   payerComponentExpense.Rate = item.Rate;
                                   payerComponentExpense.RateType = item.RateType;
                                   payerComponentExpense.DynamicProperties = item.DynamicProperties;
                                   if (totalInsuredAmount != 0)
                                   {
                                       payerComponentExpense.Amount = decimal.Round(((coverage.LimitAmount * payerComponentExpense.Rate) / totalInsuredAmount), QuoteManager.DecimalRound);
                                   }
                                   if (payerComponents.Exists(x => x.Component.Id == payerComponentExpense.Id && x.CoverageId == payerComponentExpense.CoverageId))
                                   {
                                       payerComponents.First(x => x.Component.Id == payerComponentExpense.Id && x.CoverageId == payerComponentExpense.CoverageId).BaseAmount += payerComponentExpense.BaseAmount;
                                       payerComponents.First(x => x.Component.Id == payerComponentExpense.Id && x.CoverageId == payerComponentExpense.CoverageId).Amount += payerComponentExpense.Amount;
                                   }
                                   else
                                   {
                                       payerComponents.Add(new PayerComponent
                                       {
                                           Component = payerComponentExpense.Component,
                                           CoverageId = payerComponent.CoverageId,
                                           LineBusinessId = payerComponent.LineBusinessId,
                                           BaseAmount = payerComponentExpense.BaseAmount,
                                           Amount = payerComponentExpense.Amount,
                                           Rate = payerComponentExpense.Rate,
                                           RateType = payerComponentExpense.RateType,
                                           DynamicProperties = payerComponentExpense.DynamicProperties
                                       });
                                   }
                               }
                           }
                       }

                   });

               });

            return payerComponents;
        }
        private decimal CalculatePremiumBase(Coverage coverage, decimal sharePercentage, int decimalsRound)
        {
            decimal premiumBase;

            if (coverage.Rate == 0)
            {
                return coverage.Rate.Value;
            }

            switch (coverage.RateType)
            {
                case RateType.FixedValue:
                    {
                        if (coverage.Rate.HasValue)
                        {
                            if (coverage.CoverStatus.HasValue && coverage.CoverStatus == CoverageStatusType.Excluded)
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
                    premiumBase = coverage.SubLimitAmount;
                    break;
                case RateType.Permilage:
                    premiumBase = coverage.SubLimitAmount;
                    break;
                default:
                    throw new BusinessException("CLCERR_INVALID_RATE_TYPE",
                        new[]
                            {
                                coverage.RateType.Value.ToString(),
                                "CoverageId: " + coverage.Id
                            });
            }
            premiumBase = decimal.Round(premiumBase * sharePercentage / 100, decimalsRound);
            return premiumBase;
        }


        /// <summary>
        /// Ejecutar reglas a nivel de componente
        /// </summary>
        /// <param name="payerComponent">Componente</param>
        /// <param name="ruleSetId">Id regla</param>
        /// <returns>Componente</returns>
        public PayerComponent RunRules(CompanyPolicy policy, PayerComponent payerComponent, int ruleSetId)
        {
            Rules.Facade facade = new Rules.Facade();
            EntityAssembler.CreateFacadeGeneral(policy, facade);
            EntityAssembler.CreateFacadeComponent(facade, payerComponent);

            facade = RulesEngineDelegate.ExecuteRules(ruleSetId, facade);

            payerComponent.Rate = facade.GetConcept<decimal>(RuleConceptComponent.Rate);
            payerComponent.RateType = (RateType)facade.GetConcept<int>(RuleConceptComponent.RateTypeCode);
            payerComponent.DynamicProperties = ModelAssembler.CreateDynamicConcepts(facade);
            return payerComponent;
        }
              

        private double CalculateTax(int rateTypeCode, double rate, double amount, decimal excemptionPct)
        {
            double factor = (double)QuoteManager.GetFactor((TaxType)rateTypeCode);
            double tax = 0;

            if (rateTypeCode == (int)RateType.FixedValue)
                tax = rate;
            else
                tax = amount * rate * factor;

            return tax - (tax * (double)excemptionPct / 100);
        }

    }
}
