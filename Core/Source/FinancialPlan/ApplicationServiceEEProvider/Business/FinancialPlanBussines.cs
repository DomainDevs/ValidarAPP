using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Assemblers;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.DAOs;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Models;
using Sistran.Core.Application.FinancialPlanServices.Enums;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Business
{
    /// <summary>
    /// Plan Financiero
    /// </summary>
    public class FinancialPlanBussines
    {
        /// <summary>
        /// Creates the financial plan.
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <returns></returns>
        public static FinancialDTO CreateFinancialPlan(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            FinancialDTO financialDTO = new FinancialDTO();
            ValidatePaymentPlan(filterFinancialPlanDTO);
            List<FinancialPlanDTO> financialPlanDTOs = DelegateService.integrationUnderwritingService.GetFinancialPlan(new FilterBaseDTO { Id = filterFinancialPlanDTO.EndorsementId });
            //PaymentsScheduleDTO paymentsScheduleDTO = DelegateService.integrationUnderwritingService.GetPaymentsScheduleBySheduleId(new FilterBaseDTO { Id = filterFinancialPlanDTO.PaymentPlanId });
            List<PaymentDistributionDTO> paymentDistributionDTOs = DelegateService.integrationUnderwritingService.GetPaymentsDistributionByFilter(new FilterBaseDTO { Id = filterFinancialPlanDTO.PaymentPlanId });
            List<QuotaComponentsDTO> AplicationPayment = DelegateService.accountingIntegrationService.GetApplicationComponentByEndorsementId(filterFinancialPlanDTO.EndorsementId);
            financialDTO.EndorsementId = filterFinancialPlanDTO.EndorsementId;
            financialDTO.Quotas = CreatePaymentPlan(paymentDistributionDTOs, financialPlanDTOs, AplicationPayment, filterFinancialPlanDTO.AccountDate, filterFinancialPlanDTO.PaymentPlanId);
            financialDTO.Premium = decimal.Round(financialDTO.Quotas.Where(a => a.StateQuota != (short)QuotaState.Complete).Sum(m => m.Premium), QuoteManager.DecimalRound);
            financialDTO.Expenses = decimal.Round(financialDTO.Quotas.Where(a => a.StateQuota != (short)QuotaState.Complete).Sum(m => m.Expenses), QuoteManager.DecimalRound);
            financialDTO.Tax = decimal.Round(financialDTO.Quotas.Where(a => a.StateQuota != (short)QuotaState.Complete).Sum(m => m.Tax), QuoteManager.DecimalRound);
            financialDTO.Total = financialDTO.Premium + financialDTO.Expenses + financialDTO.Tax;
            return financialDTO;
        }

        /// <summary>
        /// Validates the payment plan.
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <exception cref="System.Exception"></exception>
        private static void ValidatePaymentPlan(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            PolicyBaseDTO policyBaseDTO = DelegateService.integrationUnderwritingService.GetEndorsementBaseByEndorsementId(new FilterBaseDTO { Id = filterFinancialPlanDTO.EndorsementId });
            DateTime accountDate = DelegateService.commonServices.GetModuleDateIssue((int)ModuleType.Emision, DateTime.Now);
            if (filterFinancialPlanDTO.AccountDate >= policyBaseDTO.ToDate)
            {
                throw new Exception(Resources.Resources.ErrorCurrentTo);
            }
            else if (filterFinancialPlanDTO.AccountDate < policyBaseDTO.FromDate)
            {
                throw new Exception(Resources.Resources.ErrorCurrentTo);
            }
            else if (filterFinancialPlanDTO.AccountDate < accountDate)
            {
                throw new Exception(String.Format(Resources.Resources.ErrorAccountDate, accountDate));
            }

        }

        /// <summary>
        /// Creates the payment plan.
        /// </summary>
        /// <param name="NumberQuotas">The number quotas.</param>
        /// <param name="financialPlanDTOs">The financial plan dt os.</param>
        /// <param name="AccountDate">The account date.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No hay cuotas pendientes par Recuotificar</exception>
        private static List<QuotaPlanDTO> CreatePaymentPlan(List<PaymentDistributionDTO> paymentDistributionDTOs, List<FinancialPlanDTO> financialPlanDTOs, List<QuotaComponentsDTO> AplicationPayment, DateTime AccountDate, int paymentPlanId, bool showAll = true)
        {
            ComponentBaseDTO componentBaseDTO = new ComponentBaseDTO();

            if (financialPlanDTOs.Where(a => a.StateQuota != (short)QuotaState.Complete).Count() <= 0)
            {
                throw new System.Exception(Resources.Resources.NotQuotaPending);
            }
            componentBaseDTO.Total = financialPlanDTOs.Where(a => a.StateQuota != (int)QuotaState.Complete).Sum(a => a.AmountPending);
            componentBaseDTO.Premium = financialPlanDTOs.Where(a => a.StateQuota != (int)QuotaState.Complete).Sum(a => a.Premium);
            componentBaseDTO.Tax = financialPlanDTOs.Where(a => a.StateQuota != (int)QuotaState.Complete).Sum(a => a.Tax);
            componentBaseDTO.Expenses = financialPlanDTOs.Where(a => a.StateQuota != (int)QuotaState.Complete).Sum(a => a.Expenses);

            List<QuotaPlanDTO> quotaNew = DtoAssembler.GetFinancialPlan(financialPlanDTOs);
            List<QuotaPlanDTO> QuotaPaid = quotaNew.Where(a => a.StateQuota == (short)QuotaState.Complete).ToList();
            List<QuotaPlanDTO> QuotaPending = quotaNew.Where(a => a.StateQuota != (short)QuotaState.Complete && a.AmountPending != a.Amount).ToList();
            List<ComponentTypeDTO> componentTypes = DelegateService.integrationUnderwritingService.GetComponentTypes();
            //quotaNew = ComponentsCalculate(quotaNew, AplicationPayment, componentTypes);
            object lockobj = new object();
            QuotaPending.AsParallel().ForAll(a =>
            {
                a.StateQuota = (short)QuotaState.Complete;
                a.OriginalValue = a.Amount;
                a.Amount = a.Amount - a.AmountPending;
                a.AmountPending = 0;
                a.Premium = decimal.Round(AplicationPayment.Where(m => m.QuotaNumber == a.Number && componentTypes.Where(u => u.Id == (int)ComponentType.Premium).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                a.Expenses = decimal.Round(AplicationPayment.Where(m => m.QuotaNumber == a.Number && componentTypes.Where(u => u.Id == (int)ComponentType.Expenses).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                a.Tax = decimal.Round(AplicationPayment.Where(m => m.QuotaNumber == a.Number && componentTypes.Where(u => u.Id == (int)ComponentType.Taxes).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
            });

            componentBaseDTO.Premium = decimal.Round(componentBaseDTO.Premium - QuotaPending.Sum(a => a.Premium), QuoteManager.DecimalRound);
            componentBaseDTO.Tax = decimal.Round(componentBaseDTO.Tax - QuotaPending.Sum(a => a.Tax), QuoteManager.DecimalRound);
            componentBaseDTO.Expenses = decimal.Round(componentBaseDTO.Expenses - QuotaPending.Sum(a => a.Expenses), QuoteManager.DecimalRound);

            List<QuotaPlanDTO> newPlan = QuotaPaid;
            newPlan.AddRange(QuotaPending);
            int newCuota = newPlan.Count() + 1;
            QuotaBase QuotaBase = new QuotaBase { InitialFee = newCuota, StartDate = AccountDate, PaymentPlanId = paymentPlanId, ComponentBaseDTO = componentBaseDTO, ComponentTypes = componentTypes, PaymentDistributions = paymentDistributionDTOs, Quotas = newPlan };
            List<QuotaPlanDTO> quotaPlan = CalculateQuotas(QuotaBase);
            quotaPlan = CalculateDiff(quotaPlan, componentBaseDTO.Total);
            if (showAll)
                return quotaPlan;
            else
                return quotaPlan.Where(m => m.StateQuota != (short)QuotaState.Complete).ToList();
        }

        /// <summary>
        /// Componentses the calculate.
        /// </summary>
        /// <param name="quotaNew">The quota new.</param>
        /// <param name="quotaComp">The quota comp.</param>
        /// <returns></returns>
        private static List<QuotaPlanDTO> ComponentsCalculate(List<QuotaPlanDTO> quotaNew, List<QuotaComponentsDTO> quotaComp, List<ComponentTypeDTO> componentTypes)
        {

            //Prima iva gastos
            quotaNew.Where(a => a.StateQuota != (short)QuotaState.Complete).AsParallel().ForAll(a =>
            {
                a.Premium = decimal.Round(a.Premium - quotaComp.Where(m => m.QuotaNumber == a.Number && componentTypes.Where(u => u.Id == (int)ComponentType.Premium).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                a.Expenses = decimal.Round(a.Expenses - quotaComp.Where(m => m.QuotaNumber == a.Number && componentTypes.Where(u => u.Id == (int)ComponentType.Expenses).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
                a.Tax = decimal.Round(a.Tax - quotaComp.Where(m => m.QuotaNumber == a.Number && componentTypes.Where(u => u.Id == (int)ComponentType.Taxes).Select(p => p.ComponentId).Contains(m.ComponentId)).Sum(z => z.Amount), QuoteManager.DecimalRound);
            });
            return quotaNew;
        }

        /// <summary>
        /// Calculates the quotas.
        /// </summary>
        /// <param name="quotas">The quotas.</param>
        /// <param name="paymentDistributionDTOs">The payment distribution dt os.</param>
        /// <param name="componentBaseDTO">The component base dto.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="quotasBase">The quotas base.</param>
        /// <returns></returns>
        internal static List<QuotaPlanDTO> CalculateQuotas(QuotaBase quotaBase)
        {
            DateTime ExpirationDate;
            ComponentBaseDTO componentQuota = new ComponentBaseDTO();
            int coutasNumber = quotaBase.InitialFee;
            decimal PercentagePremium = decimal.Zero;
            decimal PercentageTax = decimal.Zero;
            decimal PercentageExpenses = decimal.Zero;
            List<PaymentDistributionCompDTO> paymentDistributionCompDTO = DelegateService.integrationUnderwritingService.GetPaymentDistributionComponents(quotaBase.PaymentPlanId);
            foreach (PaymentDistributionDTO distribution in quotaBase.PaymentDistributions)
            {
                PercentagePremium = paymentDistributionCompDTO.Where(m => m.Id == distribution.Number && m.ComponentId == (int)ComponentType.Premium).Sum(z => z.Value);
                PercentageTax = paymentDistributionCompDTO.Where(m => m.Id == distribution.Number && m.ComponentId == (int)ComponentType.Taxes).Sum(z => z.Value);
                PercentageExpenses = paymentDistributionCompDTO.Where(m => m.Id == distribution.Number && m.ComponentId == (int)ComponentType.Expenses).Sum(z => z.Value);
                componentQuota = new ComponentBaseDTO();
                componentQuota.Premium = decimal.Round(((quotaBase.ComponentBaseDTO.Premium * PercentagePremium) / 100), QuoteManager.DecimalRound);
                componentQuota.Expenses = decimal.Round(((quotaBase.ComponentBaseDTO.Expenses * PercentageExpenses) / 100), QuoteManager.DecimalRound);
                componentQuota.Tax = decimal.Round(((quotaBase.ComponentBaseDTO.Tax * PercentageTax) / 100), QuoteManager.DecimalRound);
                componentQuota.Total = decimal.Round(componentQuota.Premium + componentQuota.Expenses + componentQuota.Tax, QuoteManager.DecimalRound);
                ExpirationDate = CalculateExpirationDate((PaymentCalculationType)distribution.CalculationUnit, quotaBase.StartDate, distribution.CalculationQuantity == 0 ? 1 : distribution.CalculationQuantity);
                quotaBase.Quotas.Add(CreateFinancialPlanQuota(coutasNumber, componentQuota, ExpirationDate));
                quotaBase.StartDate = ExpirationDate;
                coutasNumber = coutasNumber + 1;
            }
            return quotaBase.Quotas;
        }

        /// <summary>
        /// Calculates the expiration date.
        /// </summary>
        /// <param name="calculationType">Type of the calculation.</param>
        /// <param name="dateStart">The date start.</param>
        /// <param name="quantityAdd">The quantity add.</param>
        /// <returns></returns>
        private static DateTime CalculateExpirationDate(PaymentCalculationType calculationType, DateTime dateStart, int quantityAdd)
        {
            switch (calculationType)
            {
                case PaymentCalculationType.Day:
                    dateStart = dateStart.AddDays(quantityAdd);
                    break;
                case PaymentCalculationType.Fortnight:
                    dateStart = dateStart.AddDays(quantityAdd * 15);
                    break;
                case PaymentCalculationType.Month:
                    dateStart = dateStart.AddMonths(quantityAdd);
                    break;
                default:
                    dateStart = dateStart.AddMonths(quantityAdd);
                    break;
            }

            return dateStart;
        }

        /// <summary>
        /// Creates the financial plan quota.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="value">The value.</param>
        /// <param name="ExpirationDate">The expiration date.</param>
        /// <returns></returns>
        private static QuotaPlanDTO CreateFinancialPlanQuota(int number, ComponentBaseDTO componentBaseDTO, DateTime ExpirationDate)
        {
            QuotaPlanDTO QuotaPlanDTO = new QuotaPlanDTO();
            QuotaPlanDTO.Number = number;
            QuotaPlanDTO.Amount = componentBaseDTO.Total;
            QuotaPlanDTO.AmountPending = componentBaseDTO.Total;
            QuotaPlanDTO.Premium = componentBaseDTO.Premium;
            QuotaPlanDTO.Tax = componentBaseDTO.Tax;
            QuotaPlanDTO.Expenses = componentBaseDTO.Expenses;
            QuotaPlanDTO.ExpirationDate = ExpirationDate;
            QuotaPlanDTO.StateQuota = (short)QuotaState.Pending;
            return QuotaPlanDTO;
        }

        /// <summary>
        /// Calculates the difference.
        /// </summary>
        /// <param name="QuotaPlanDTOs">The quota dt os.</param>
        /// <param name="premium">The premium.</param>
        /// <returns></returns>
        private static List<QuotaPlanDTO> CalculateDiff(List<QuotaPlanDTO> QuotaPlanDTOs, decimal premium)
        {
            decimal totalPremium = QuotaPlanDTOs.Where(a => a.StateQuota != (short)QuotaState.Complete).Sum(m => m.Amount);
            decimal diff = premium - totalPremium;
            if (diff != 0)
            {
                QuotaPlanDTOs.OrderByDescending(x => x.Number).FirstOrDefault().Amount += diff;
                QuotaPlanDTOs.OrderByDescending(x => x.Number).FirstOrDefault().AmountPending += diff;
            }
            return QuotaPlanDTOs;
        }

        /// <summary>
        /// Crear Plan de Pago
        /// </summary>
        /// <param name="filterFinancialPlanDTO">The filter financial plan dto.</param>
        /// <returns></returns>
        public static bool CreateQuotas(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            PolicyBaseDTO policyBaseDTO = DelegateService.integrationUnderwritingService.GetEndorsementBaseByEndorsementId(new FilterBaseDTO { Id = filterFinancialPlanDTO.EndorsementId });
            List<PremiumBaseDTO> PremiumBaseDTOs = DelegateService.accountingIntegrationService.GetApplicationPremiumBaseByEndorsementId(filterFinancialPlanDTO.EndorsementId);
            FinancialDTO financialDTO = CreateFinancialPlan(filterFinancialPlanDTO);
            FinanPlanModel finanPlanModel = new FinanPlanModel { endorsementId = filterFinancialPlanDTO.EndorsementId, IndividualId = filterFinancialPlanDTO.PayerId, AccountingDate = filterFinancialPlanDTO.AccountDate, isByPaymentUpdate = true, PaymentId = 1, PaymentPlanId = filterFinancialPlanDTO.PaymentPlanId, PaymentMethodId = filterFinancialPlanDTO.PaymentMethodId, ReasonforChange = filterFinancialPlanDTO.ReasonforChange };
            finanPlanModel.ExchangeRate = policyBaseDTO.ExchangeRate;
            finanPlanModel.CoinsurancePct = policyBaseDTO.CoinsurancePct;
            finanPlanModel.QuotaPlan = DtoAssembler.CreatePayerPayment(financialDTO.Quotas);
            decimal amount = decimal.Round(finanPlanModel.QuotaPlan.Sum(a => a.Amount), QuoteManager.DecimalRound);
            finanPlanModel.QuotaPlan.AsParallel().ForAll(m => m.PaymentPct = decimal.Round((m.Amount / amount) * 100));
            finanPlanModel.QuotaPlan = UpdatePaymentQuotas(finanPlanModel.QuotaPlan, policyBaseDTO);
            finanPlanModel.QuotaPlanComponents = CreateComponentsQuotas(financialDTO, finanPlanModel);
            finanPlanModel.QuotaPlanComponents.AsParallel().ForAll(m => m.PaymentPct = decimal.Round((m.Amount / amount) * 100));
            return QuotaDAO.CreateQuotas(finanPlanModel);

        }

        /// <summary>
        /// Updates the payment quota.
        /// </summary>
        /// <param name="finPayerPaymentModels">The fin payer payment models.</param>
        /// <param name="policyBaseDTO">The policy base dto.</param>
        /// <returns></returns>
        private static List<FinPayerPaymentModel> UpdatePaymentQuotas(List<FinPayerPaymentModel> finPayerPaymentModels, PolicyBaseDTO policyBaseDTO)
        {
            finPayerPaymentModels.AsParallel().ForAll(reg =>
            {
                reg.LocalAmount = decimal.Round(reg.Amount * policyBaseDTO.ExchangeRate, QuoteManager.DecimalRound);
                reg.MainAmount = decimal.Round(reg.Amount * (policyBaseDTO.CoinsurancePct / 100), QuoteManager.DecimalRound);
                reg.MainLocalAmount = decimal.Round(reg.Amount * policyBaseDTO.ExchangeRate * (policyBaseDTO.CoinsurancePct / 100), QuoteManager.DecimalRound);
            });
            return finPayerPaymentModels;
        }

        /// <summary>
        /// Creates the components quotas.
        /// </summary>
        /// <param name="financialDTO">The financial dto.</param>
        /// <param name="finanPlanModel">The finan plan model.</param>
        /// <returns></returns>
        public static List<FinPayerPaymentCompModel> CreateComponentsQuotas(FinancialDTO financialDTO, FinanPlanModel finanPlanModel)
        {

            List<ComponentLbSb> ComponentLbSbs = DelegateService.integrationUnderwritingService.GetLbSbByEndorsementId(new FilterBaseDTO { Id = financialDTO.EndorsementId });
            List<ComponentTypeDTO> componentTypes = DelegateService.integrationUnderwritingService.GetComponentTypes();
            List<int> components = new List<int> { (int)ComponentType.Premium, (int)ComponentType.Expenses, (int)ComponentType.Taxes, (int)ComponentType.Surcharges, (int)ComponentType.Discounts };
            List<FinPayerPaymentCompModel> finPayerPaymentCompModels = new List<FinPayerPaymentCompModel>();
            FinPayerPaymentCompModel finPayerPaymentCompModel = null;
            foreach (var z in finanPlanModel.QuotaPlan)
            {
                var q = financialDTO.Quotas.Where(m => m.Number == z.PaymentNumber).FirstOrDefault();
                if (q != null)
                {
                    foreach (var componentType in components)
                    {
                        foreach (var id in componentTypes.Where(a => a.Id == componentType))
                        {
                            finPayerPaymentCompModel = new FinPayerPaymentCompModel();
                            finPayerPaymentCompModel.ComponentId = id.ComponentId;
                            if (componentType == (int)ComponentType.Premium)
                            {
                                finPayerPaymentCompModel.Amount = q.Premium;
                            }
                            else if (componentType == (int)ComponentType.Expenses)
                            {
                                finPayerPaymentCompModel.Amount = q.Expenses;
                            }
                            else if (componentType == (int)ComponentType.Taxes)
                            {
                                finPayerPaymentCompModel.Amount = q.Tax;
                            }
                            else if (componentType == (int)ComponentType.Discounts)
                            {
                                finPayerPaymentCompModel.Amount = q.ExpensesOther;
                            }
                            else if (componentType == (int)ComponentType.Surcharges)
                            {
                                finPayerPaymentCompModel.Amount = q.ExpensesOther;
                            }
                            finPayerPaymentCompModel.Number = z.PaymentNumber;
                            finPayerPaymentCompModel.LocalAmount = decimal.Round(finPayerPaymentCompModel.Amount * finanPlanModel.ExchangeRate, QuoteManager.DecimalRound);
                            finPayerPaymentCompModel.MainAmount = decimal.Round(finPayerPaymentCompModel.Amount * (finanPlanModel.CoinsurancePct / 100), QuoteManager.DecimalRound);
                            finPayerPaymentCompModel.MainLocalAmount = decimal.Round(finPayerPaymentCompModel.Amount * finanPlanModel.ExchangeRate * (finanPlanModel.CoinsurancePct / 100), QuoteManager.DecimalRound);
                            finPayerPaymentCompModels.Add(finPayerPaymentCompModel);

                            finPayerPaymentCompModel.FinPayerPaymentLbSbModel = CreateComponentsLbSbQuotas(finPayerPaymentCompModel, finanPlanModel, ComponentLbSbs);
                        }
                    }

                }

            }
            return finPayerPaymentCompModels;
        }

        /// <summary>
        /// Creates the components lb sb quotas.
        /// </summary>
        /// <param name="finPayerPaymentCompModel">The fin payer payment comp model.</param>
        /// <param name="finanPlanModel">The finan plan model.</param>
        /// <param name="ComponentLbSbs">The component lb SBS.</param>
        /// <returns></returns>
        public static List<FinPayerPaymentLbSbModel> CreateComponentsLbSbQuotas(FinPayerPaymentCompModel finPayerPaymentCompModel, FinanPlanModel finanPlanModel, List<ComponentLbSb> ComponentLbSbs)
        {
            List<FinPayerPaymentLbSbModel> finPayerPaymentLbSbModels = new List<FinPayerPaymentLbSbModel>();
            FinPayerPaymentLbSbModel finPayerPaymentLbSbModel = new FinPayerPaymentLbSbModel();

            foreach (var compLbSb in ComponentLbSbs)
            {
                decimal premiumBase = decimal.Round(ComponentLbSbs.Sum(a => a.Amount), QuoteManager.DecimalRound);
                finPayerPaymentLbSbModel = new FinPayerPaymentLbSbModel();
                finPayerPaymentLbSbModel.ComponentId = finPayerPaymentCompModel.ComponentId;
                finPayerPaymentLbSbModel.Amount = decimal.Round(finPayerPaymentCompModel.Amount * (compLbSb.Amount / premiumBase), QuoteManager.DecimalRound);
                finPayerPaymentLbSbModel.LocalAmount = decimal.Round(finPayerPaymentLbSbModel.Amount * finanPlanModel.ExchangeRate, QuoteManager.DecimalRound);
                finPayerPaymentLbSbModel.MainAmount = decimal.Round(finPayerPaymentLbSbModel.Amount * (finanPlanModel.CoinsurancePct / 100), QuoteManager.DecimalRound);
                finPayerPaymentLbSbModel.MainLocalAmount = decimal.Round(finPayerPaymentLbSbModel.Amount * finanPlanModel.ExchangeRate * (finanPlanModel.CoinsurancePct / 100), QuoteManager.DecimalRound);
                finPayerPaymentLbSbModel.LineBussinesId = compLbSb.LineBusiness;
                finPayerPaymentLbSbModel.SubLineBussinesId = compLbSb.SubLineBusiness;
                finPayerPaymentLbSbModels.Add(finPayerPaymentLbSbModel);
            }

            return finPayerPaymentLbSbModels;
        }
        private static bool CreateModelQuotas(FilterFinancialPlanDTO filterFinancialPlanDTO)
        {
            List<FinPayerPaymentCompModel> finPayerPaymentCompModels = new List<FinPayerPaymentCompModel>();
            var quotasAplications = DelegateService.accountingIntegrationService.GetApplicationPremiumBaseByEndorsementId(filterFinancialPlanDTO.EndorsementId);
            var quotas = DelegateService.integrationUnderwritingService.GetQuotasByEndorsementId(new FilterBaseDTO { Id = filterFinancialPlanDTO.EndorsementId });
            FinPayerPaymentCompModel finPayerPaymentCompModel = null;
            foreach (var z in quotas.FinancialPlanDTOs)
            {
                if (z.StateQuota == (int)QuotaState.Complete)
                {
                    var a = quotas.QuotaComponentDTOs.Where(o => o.Id == z.PayerPaymentId);
                    finPayerPaymentCompModels.Add(finPayerPaymentCompModel);
                }

            }
            return false;

        }
    }
}
