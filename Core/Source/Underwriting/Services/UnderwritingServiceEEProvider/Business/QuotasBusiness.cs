using Sistran.Core.Application.UnderwritingServices.Constants;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Distribution;
using Sistran.Core.Application.Utilities.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Business
{
    public class QuotasBusiness
    {
        //PaymentPlan
        public static List<Quota> CalculateQuotas(FinancialPaymentPlan paymentPlan, List<PaymentDistribution> paymentDistribution)
        {
            List<Quota> quotas = new List<Quota>();
            DateTime dateStartQuota = new DateTime();
            if (paymentPlan.PaymentDistribution?.Count > 0)
            {
                if (paymentPlan != null)
                {
                    if (paymentPlan.IsGreaterDate)
                    {
                        if (DateTime.Compare(paymentPlan.IssueDate, paymentPlan.CurrentFrom) < 0)
                        {
                            dateStartQuota = paymentPlan.CurrentFrom;
                        }
                        else
                        {
                            dateStartQuota = paymentPlan.IssueDate;
                        }
                    }
                    else if (paymentPlan.IsIssueDate)
                    {
                        dateStartQuota = paymentPlan.IssueDate;
                    }
                    else
                    {
                        dateStartQuota = paymentPlan.CurrentFrom;
                    }

                    ValidateQuotas(paymentPlan.PaymentDistribution.SelectMany(m => m.PaymentDistributionComp).ToList());


                    foreach (PaymentDistributionPlan distribution in paymentPlan.PaymentDistribution)
                    {
                        foreach (PaymentDistribution pDistribution in paymentDistribution)
                        {
                            if (distribution.Number == pDistribution.Number)
                            {
                                Quota quota = CalculateQuota(distribution, paymentPlan, dateStartQuota, pDistribution.CalculationQuantity == null ? 0 : (int)pDistribution.CalculationQuantity);
                                quotas.Add(quota);
                                dateStartQuota = quota.ExpirationDate;
                            }
                           
                        }
                        
                    }
                    decimal premiumTotal = decimal.Round(paymentPlan.ComponentValue.Premium + paymentPlan.ComponentValue.Expenses + paymentPlan.ComponentValue.Tax + paymentPlan.ComponentValue.Surcharges + paymentPlan.ComponentValue.Discounts, QuoteManager.DecimalRound);
                    if (premiumTotal != quotas.Sum(q => q.Amount))
                    {
                        decimal[] quotasRound = QuoteManager.RoundCollection(quotas.Select(x => x.Amount).ToArray(), premiumTotal, 2);
                        quotas[quotas.Count - 1].Amount = quotasRound[quotas.Count - 1];
                    }
                }
                else
                {
                    throw new Exception(Resources.Errors.ErrorPaymentPlanEmpty);
                }
            }
            else
            {
                throw new Exception(Resources.Errors.ErrorPaymentPlanEmpty);
            }
            return quotas;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="distribution"></param>
        /// <param name="paymentPlan"></param>
        /// <returns></returns>
        private static Quota CalculateQuota(PaymentDistributionPlan distribution, FinancialPaymentPlan paymentPlan, DateTime dateStartQuota, int pDistribution)
        {
            Quota quota = new Quota();
            ComponentValue ComponentValue = new ComponentValue();
            ComponentValue PercentageComponentValue = new ComponentValue();
            decimal premiumTotal = decimal.Round(paymentPlan.ComponentValue.Premium + paymentPlan.ComponentValue.Expenses + paymentPlan.ComponentValue.Tax + paymentPlan.ComponentValue.Surcharges + paymentPlan.ComponentValue.Discounts, QuoteManager.DecimalRound);
            if (premiumTotal != 0)
            {
                PercentageComponentValue.Premium = distribution.PaymentDistributionComp.Where(m => m.ComponentId == (int)ComponentType.Premium).Sum(z => z.Percentage);
                PercentageComponentValue.Tax = distribution.PaymentDistributionComp.Where(m => m.ComponentId == (int)ComponentType.Taxes).Sum(z => z.Percentage);
                PercentageComponentValue.Expenses = distribution.PaymentDistributionComp.Where(m => m.ComponentId == (int)ComponentType.Expenses).Sum(z => z.Percentage);
                PercentageComponentValue.Surcharges = distribution.PaymentDistributionComp.Where(m => m.ComponentId == (int)ComponentType.Surcharges).Sum(z => z.Percentage);
                PercentageComponentValue.Discounts = distribution.PaymentDistributionComp.Where(m => m.ComponentId == (int)ComponentType.Discounts).Sum(z => z.Percentage);
                ComponentValue.Premium = decimal.Round(((paymentPlan.ComponentValue.Premium * PercentageComponentValue.Premium) / 100), QuoteManager.DecimalRound);
                ComponentValue.Expenses = decimal.Round(((paymentPlan.ComponentValue.Expenses * PercentageComponentValue.Expenses) / 100), QuoteManager.DecimalRound);
                ComponentValue.Tax = decimal.Round(((paymentPlan.ComponentValue.Tax * PercentageComponentValue.Tax) / 100), QuoteManager.DecimalRound);
                ComponentValue.Surcharges = decimal.Round(((paymentPlan.ComponentValue.Surcharges * PercentageComponentValue.Surcharges) / 100), QuoteManager.DecimalRound);
                ComponentValue.Discounts = decimal.Round(((paymentPlan.ComponentValue.Discounts * PercentageComponentValue.Discounts) / 100), QuoteManager.DecimalRound);
                ComponentValue.Total = decimal.Round(ComponentValue.Premium + ComponentValue.Expenses + ComponentValue.Tax + ComponentValue.Surcharges + ComponentValue.Discounts, QuoteManager.DecimalRound);
            }
            quota.Number = distribution.Number;
                if (quota.Number == 1)
                {
                    quota.ExpirationDate = CalculateExpirationDate(PaymentCalculationType.Day, dateStartQuota, paymentPlan.Quantity);
                }
                else
                {
                    quota.ExpirationDate = CalculateExpirationDate((PaymentCalculationType)paymentPlan.CalculationType, dateStartQuota, pDistribution);
                }
            
            if (premiumTotal != 0)
            {
                quota.Percentage = decimal.Round((ComponentValue.Total / premiumTotal) * 100, 2);
            }
            else
            {
                quota.Percentage = 0;
            }
            quota.Amount = ComponentValue.Total;
            return quota;
        }

        /// <summary>
        /// Calcular Fecha De Pago
        /// </summary>
        /// <param name="gapUnitType">Tipo De Unidad</param>
        /// <param name="dateStart">Fecha Inicial</param>
        /// <param name="quantityAdd">Cantidad A Incrementar</param>
        /// <returns>Fecha De Pago</returns>
        private static DateTime CalculateExpirationDate(PaymentCalculationType calculationType, DateTime dateStart, int quantityAdd)
        {
            quantityAdd = quantityAdd == 0 ? 1 : quantityAdd;
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
        private static void ValidateQuotas(List<PaymentDistributionComp> PaymentDistributionComp)
        {
            int value = 0;
            var result = PaymentDistributionComp.GroupBy(m => m.ComponentId);
            foreach (IGrouping<int, PaymentDistributionComp> distribution in result)
            {
                if (distribution.Sum(a => a.Percentage) != 0)
                {
                    if (distribution.Sum(a => a.Percentage) != Convert.ToDecimal(UnderwritingConstant.Percent))
                    {
                        throw new Exception(string.Format(Resources.Errors.ErrorPorcentageQuotas, distribution.Sum(a => a.Percentage)));

                    }
                }
            }
        }

    }
}
