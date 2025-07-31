using Sistran.Co.Application.Data;
using Sistran.Company.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Managers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using CUNMO = Sistran.Core.Application.UnderwritingServices.Models;
using UNMOCORE = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PayerPaymentDAO
    {
        private const int parameterAssistanceCoverId = 525;
        private const int parameterAssistanceMultiCoverId = 528;
        private const int prefixCDAut = 10;
        public List<PayerPayment> Calculate(CompanyPolicy companyPolicy, bool RequestIsOpen, DateTime RequestFrom, DateTime RequestTo)
        {
            PaymentScheduleDAO paymentScheduleDAO = new PaymentScheduleDAO();
            CompanyPaymentSchedule paymentSchedule = paymentScheduleDAO.GetPaymentScheduleByPaymentPlanId(companyPolicy.PaymentPlan.Id);
            List<PayerPayment> payerPayments =
                CalculateDatesQuotes(
                    RequestIsOpen,
                    RequestFrom,
                    RequestTo,
                    companyPolicy.IssueDate,
                    companyPolicy.CurrentFrom,
                    companyPolicy.CurrentTo,
                    paymentSchedule.Quantity,
                    paymentSchedule.FirstPayerQuantity,
                    paymentSchedule.CalculationType);

            int assistanceCoverId = (int)DelegateService.commonService.GetParameterByParameterId(parameterAssistanceCoverId).NumberParameter;
            int assistanceCoverMultiId = (int)DelegateService.commonService.GetParameterByParameterId(parameterAssistanceMultiCoverId).NumberParameter;

            payerPayments = CalculateValues(payerPayments, companyPolicy, assistanceCoverId, assistanceCoverMultiId);
            return payerPayments;
        }

        /// <summary>
        /// Calcula el valor de las cuotas para cada componente
        /// </summary>
        /// <param name="payerPaymentsDate"></param>
        /// <param name="companyPolicy"></param>
        /// <returns></returns>
        public List<PayerPayment> CalculateValues(List<PayerPayment> payerPaymentsDate,
            CompanyPolicy companyPolicy,
            int assistanceCoverId, int assistanceCoverMultiId)
        {
            decimal taxesAmount = companyPolicy.Summary.Taxes;
            decimal premiumAmount = companyPolicy.Summary.Premium;
            decimal expensesAmount = companyPolicy.Summary.Expenses;

            decimal fullPremiumAmount = companyPolicy.Summary.FullPremium;
            decimal expensesCoverageAmount = companyPolicy.PayerComponents.Where(p => p.CoverageId == assistanceCoverId || p.CoverageId == assistanceCoverMultiId).Sum(p => p.Amount); ;
            premiumAmount = premiumAmount - expensesCoverageAmount;


            //Valor de la cuota de Prima
            decimal fullPremiumQuote = premiumAmount / payerPaymentsDate.Count;
            //Valor de la cuota de gastos de coverturas
            decimal fullExpenseQuote = expensesCoverageAmount / payerPaymentsDate.Count;

            List<PayerPayment> payerPayments = new List<PayerPayment>();

            //Valor del gasto de expedicion
            decimal expenseEmisionAmount = expensesAmount;

            //Acumulados de los valores de gastos de expedicion, covertura y prima
            decimal acumExpenseEmision = expensesAmount;
            decimal acumPremium = 0;
            decimal acumExpenseCoverage = 0;

            //Valor total de la cuota (gastos + prima) / Nro de cuotas
            decimal premiumQuote = (premiumAmount + expensesCoverageAmount + expensesAmount) / payerPaymentsDate.Count;
            foreach (PayerPayment item in payerPaymentsDate.OrderBy(p => p.PaymentNumber))
            {
                //Nro de cuotas actuales
                int totalQuotes = (payerPaymentsDate.Count - (item.PaymentNumber - 1));

                //Arma la cuota de los impuestos
                PayerPayment payerPaymentTaxes = new PayerPayment()
                {
                    Amount = taxesAmount / payerPaymentsDate.Count,
                    ComponentType = ComponentTypePayerPayment.Taxes,
                    PaymentDate = item.PaymentDate,
                    PaymentNumber = item.PaymentNumber
                };

                //Arama la cuota de los gastos sin el valor
                PayerPayment payerPaymentExpenses = new PayerPayment()
                {
                    ComponentType = ComponentTypePayerPayment.Expenses,
                    PaymentDate = item.PaymentDate,
                    PaymentNumber = item.PaymentNumber
                };

                //Arma la cuota de la prima sin los gatos
                PayerPayment payerPaymentPremium = new PayerPayment()
                {
                    ComponentType = ComponentTypePayerPayment.Premium,
                    PaymentDate = item.PaymentDate,
                    PaymentNumber = item.PaymentNumber
                };

                //Indica si hay un acumulado de prima para ser poder ser recalculado de acuerdo al numero de cuotas actual
                if (acumPremium > 0)
                {
                    fullPremiumQuote = fullPremiumQuote + (acumPremium / totalQuotes);
                    acumPremium = 0;
                }
                //Indica si hay un acumulado de gastos de covertura para poder ser recalculado de acuerdo al numero de cuotas actual
                if (acumExpenseCoverage > 0)
                {
                    fullExpenseQuote = fullExpenseQuote + (acumExpenseCoverage / totalQuotes);
                    acumExpenseCoverage = 0;
                }
                payerPaymentPremium.Amount = fullPremiumQuote;
                payerPaymentExpenses.Amount = fullExpenseQuote;

                /*Realiza el calculo de las cuotas de prima y gastos teniendo en cuenta que 
                 el valor de los gastos de emision se distribuyen en las primeras cuotas
                */
                if (acumExpenseEmision > 0 && companyPolicy.Summary.CoveredRiskType == Core.Application.CommonService.Enums.CoveredRiskType.Vehicle)
                {
                    payerPaymentExpenses.Amount = fullExpenseQuote + acumExpenseEmision;
                    if (payerPaymentExpenses.Amount > premiumQuote)
                        payerPaymentExpenses.Amount = expensesAmount + (premiumQuote - expensesAmount);
                    payerPaymentPremium.Amount = premiumQuote - payerPaymentExpenses.Amount;

                    acumPremium = Math.Abs(payerPaymentPremium.Amount - fullPremiumQuote);
                    acumExpenseCoverage = fullExpenseQuote - (payerPaymentExpenses.Amount - acumExpenseEmision);
                    if ((payerPaymentExpenses.Amount - acumExpenseEmision) < 0)
                        acumExpenseCoverage = fullExpenseQuote;
                    if (acumExpenseEmision < 0)
                        acumExpenseEmision = 0;
                    acumExpenseEmision = acumExpenseEmision - payerPaymentExpenses.Amount;

                }
                //Calcula los porcentajes
                if (companyPolicy.Summary.Premium > 0)
                {
                    payerPaymentPremium.Porcentage = (payerPaymentPremium.Amount * 100) / companyPolicy.Summary.Premium;
                }
                decimal totalExpenses = expensesCoverageAmount + expensesAmount;
                if (totalExpenses > 0)
                {
                    payerPaymentExpenses.Porcentage = (payerPaymentExpenses.Amount * 100) / totalExpenses;
                }
                if (companyPolicy.Summary.Taxes > 0)
                {
                    payerPaymentTaxes.Porcentage = (payerPaymentTaxes.Amount * 100) / companyPolicy.Summary.Taxes;
                }

                //Redonde la cuota a 2 decimales
                payerPaymentPremium.Amount = decimal.Round(payerPaymentPremium.Amount, QuoteManager.DecimalRound);
                payerPaymentExpenses.Amount = decimal.Round(payerPaymentExpenses.Amount, QuoteManager.DecimalRound);
                payerPaymentTaxes.Amount = decimal.Round(payerPaymentTaxes.Amount, QuoteManager.DecimalRound);

                //Ajusta por redondeo de la ultima cuota para que el valor final sea exacto
                if (totalQuotes == 1)
                {
                    payerPaymentPremium.Amount = RoundValue(payerPaymentPremium, payerPayments, premiumAmount);
                    payerPaymentExpenses.Amount = RoundValue(payerPaymentExpenses, payerPayments, (expensesCoverageAmount + expensesAmount));
                    payerPaymentTaxes.Amount = RoundValue(payerPaymentTaxes, payerPayments, taxesAmount);
                }

                payerPayments.Add(payerPaymentPremium);
                payerPayments.Add(payerPaymentExpenses);
                payerPayments.Add(payerPaymentTaxes);
            }
            return payerPayments;
        }

        private decimal RoundValue(PayerPayment payerPayment, List<PayerPayment> payerPayments, decimal total)
        {
            List<decimal> payerPaymentsAmount = payerPayments.Where(p => p.ComponentType == payerPayment.ComponentType).Select(p => p.Amount).ToList();
            payerPaymentsAmount.Add(payerPayment.Amount);
            if (payerPaymentsAmount.Sum() != total)
            {
                decimal[] amountValues = QuoteManager.RoundCollection(payerPaymentsAmount.ToArray(), total, 2);
                return amountValues.Last();
            }
            return payerPayment.Amount;
        }

        public List<PayerPayment> CalculateDatesQuotes(bool requestIsOpen, DateTime requestFrom, DateTime requestTo, DateTime issuseDate, DateTime policyFrom, DateTime PolicyTo, int totalQuotes, int daysToFirstPay, PaymentCalculationType typeCalculation)
        {
            DateTime date = (issuseDate > policyFrom) ? issuseDate : policyFrom;
            return GetDateQuotes(date, totalQuotes, daysToFirstPay, PolicyTo);
        }

        /// <summary>
        /// Construye las cuotas con sus fechas
        /// </summary>
        /// <param name="date"></param>
        /// <param name="totalQuotes"></param>
        /// <param name="daysOrMonths"></param>
        /// <param name="type"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        private List<PayerPayment> GetDateQuotes(DateTime date, int totalQuotes, int initialDays, DateTime currentTo)
        {
            DateTime tempDate = date;
            // Se agregan los días configurados para la primera cuota
            tempDate = tempDate.AddDays(initialDays);

            List<PayerPayment> payerPayments = new List<PayerPayment>();
            // Se agregan cuotas hasta completar el total de cuotas o hasta cuando el valor de la fecha de la cuota 
            // sea menor que la fecha final de la póliza
            for (int i = 1; i <= totalQuotes; i++)
            {
                if (tempDate > currentTo)
                {
                    break;
                }
                payerPayments.Add(new PayerPayment()
                {
                    PaymentNumber = i,
                    PaymentDate = tempDate
                });

                tempDate = tempDate.AddMonths(1);
            }

            if (payerPayments.Count == 0)
            {
                payerPayments.Add(new PayerPayment()
                {
                    PaymentNumber = 1,
                    PaymentDate = currentTo
                });
            }
            return payerPayments;
        }

        public void CreateCompanyPolicyPayer(CompanyPolicy companyPolicy)
        {
            NameValue[] parameters = new NameValue[9];
            parameters[0] = new NameValue("@ENDORSEMENT_ID", companyPolicy.Endorsement.Id);
            parameters[1] = new NameValue("@POLICY_ID", companyPolicy.Endorsement.PolicyId);
            parameters[2] = new NameValue("@POLICYHOLDER_ID", companyPolicy.Holder.IndividualId);
            parameters[3] = new NameValue("@PAYMENT_SCHEDULE_ID", companyPolicy.PaymentPlan.Id);
            parameters[4] = new NameValue("@PAYMENT_METHOD_CD", companyPolicy.Holder.PaymentMethod.Id);
            parameters[5] = new NameValue("@MAIL_ADDRESS_ID", companyPolicy.Holder.CompanyName.Address.Id);
            parameters[6] = new NameValue("@PAYMENT_ID", companyPolicy.Holder.PaymentMethod.PaymentId);

            #region PayerPayment
            DataTable dtQuotas = new DataTable("PARAM_TEMP_PAYER_PAYMENT");
            dtQuotas.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtQuotas.Columns.Add("PAYER_ID", typeof(int));
            dtQuotas.Columns.Add("PAYMENT_NUM", typeof(int));
            dtQuotas.Columns.Add("PAY_EXP_DATE", typeof(DateTime));
            dtQuotas.Columns.Add("PAYMENT_PCT", typeof(decimal));
            dtQuotas.Columns.Add("AMOUNT", typeof(decimal));
            dtQuotas.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtQuotas.Columns.Add("PREFIX_CD", typeof(int));
            dtQuotas.Columns.Add("AGT_PAY_EXP_DATE", typeof(DateTime));
            if (companyPolicy.PaymentPlan.Quotas != null)
            {
                bool prvCredi = false;
                if (companyPolicy.PaymentPlan.Id == 38 || companyPolicy.PaymentPlan.Id == 39 || companyPolicy.PaymentPlan.Id == 40)
                {
                    prvCredi = true;
                }

                if (companyPolicy.PaymentPlan.PremiumFinance == null || prvCredi)
                {
                    foreach (CUNMO.Quota quota in companyPolicy.PaymentPlan.Quotas)
                    {
                        DataRow dataRowQuota = dtQuotas.NewRow();
                        dataRowQuota["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                        dataRowQuota["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                        dataRowQuota["PAYMENT_NUM"] = quota.Number;
                        dataRowQuota["PAY_EXP_DATE"] = quota.ExpirationDate;
                        dataRowQuota["PAYMENT_PCT"] = quota.Percentage;
                        dataRowQuota["AMOUNT"] = quota.Amount;
                        dataRowQuota["PREFIX_CD"] = companyPolicy.Prefix.Id;
                        dataRowQuota["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;                        
                        dtQuotas.Rows.Add(dataRowQuota);
                    }
                }
                else
                {
                    foreach (CUNMO.Quota quota in companyPolicy.PaymentPlan.Quotas)
                    {
                        decimal? Percentage = companyPolicy.PaymentPlan.PremiumFinance.PercentagetoFinance;
                        decimal? Amount = companyPolicy.PaymentPlan.PremiumFinance.FinanceToValue;
                        if (quota.Number == 2)
                        {
                            Percentage = 100 - companyPolicy.PaymentPlan.PremiumFinance.PercentagetoFinance;
                            Amount = companyPolicy.PaymentPlan.PremiumFinance.PremiumValue - companyPolicy.PaymentPlan.PremiumFinance.FinanceToValue;
                        }
                        DataRow dataRowQuota = dtQuotas.NewRow();
                        dataRowQuota["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                        dataRowQuota["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                        dataRowQuota["PAYMENT_NUM"] = quota.Number;
                        dataRowQuota["PAY_EXP_DATE"] = quota.ExpirationDate;
                        dataRowQuota["PAYMENT_PCT"] = Percentage;
                        dataRowQuota["AMOUNT"] = Amount;
                        dataRowQuota["PREFIX_CD"] = companyPolicy.Prefix.Id;
                        dataRowQuota["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
                        dtQuotas.Rows.Add(dataRowQuota);
                    }
                }

            }
            parameters[7] = new NameValue("@INSERT_TEMP_PAYER_PAYMENT", dtQuotas);
            #endregion PayerPayment

            #region FirstPayerCompoment

            DataTable dtParamFirstPayer = new DataTable("PARAM_TEMP_FIRST_PAY_COMP");
            dtParamFirstPayer.Columns.Add("CUSTOMER_TYPE_CD", typeof(int));
            dtParamFirstPayer.Columns.Add("PAYER_ID", typeof(int));
            dtParamFirstPayer.Columns.Add("COMPONENT_CD", typeof(int));
            dtParamFirstPayer.Columns.Add("ENDORSEMENT_ID", typeof(int));
            dtParamFirstPayer.Columns.Add("PREFIX_CD", typeof(int));
            if (companyPolicy.PayerComponents.FirstOrDefault(x => x != null && x.Component != null && x.Component.ComponentType == ComponentType.Taxes) != null)
            {
                if (companyPolicy.PayerComponents != null && companyPolicy.PayerComponents.Count > 0 && companyPolicy.BusinessType != BusinessType.Accepted)
                {
                    DataRow rwFirstPayComponent = dtParamFirstPayer.NewRow();
                    rwFirstPayComponent["CUSTOMER_TYPE_CD"] = companyPolicy.Holder.CustomerType;
                    rwFirstPayComponent["PAYER_ID"] = companyPolicy.Holder.IndividualId;
                    rwFirstPayComponent["COMPONENT_CD"] = companyPolicy.PayerComponents.First(x => x.Component.ComponentType == ComponentType.Taxes).Component.Id;
                    rwFirstPayComponent["ENDORSEMENT_ID"] = companyPolicy.Endorsement.Id;
                    rwFirstPayComponent["PREFIX_CD"] = DBNull.Value;
                    dtParamFirstPayer.Rows.Add(rwFirstPayComponent);
                }
            }
            parameters[8] = new NameValue("@INSERT_PARAM_TEMP_FIRST_PAY_COMP", dtParamFirstPayer);

            #endregion

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteSPNonQuery("ISS.RECORD_POLICY_PAYER", parameters);
            }
        }

        public List<UNMOCORE.Quota> CalculateQuotasWithrequestGroupig(List<PayerPayment> payerPayments)
        {
            List<UNMOCORE.Quota> quotas = new List<UNMOCORE.Quota>();
            for (int i = 1; i <= payerPayments.OrderByDescending(x => x.PaymentNumber).First().PaymentNumber; i++)
            {
                PayerPayment payerPayment = payerPayments.Where(x => x.PaymentNumber == i).First();
                UNMOCORE.Quota quota = new UNMOCORE.Quota();
                quota.Amount = payerPayments.Where(x => x.PaymentNumber == i).Sum(p => p.Amount);
                quota.ExpirationDate = payerPayment.PaymentDate;
                quota.Number = i;
                quota.Percentage = payerPayment.Porcentage;
                quotas.Add(quota);
            }
            return quotas;
        }
    }

    public static class HelperPayerPayment
    {
        public static decimal GetFullQuote(this IEnumerable<PayerPayment> payerPayments, int numberPayerPayment)
        {
            return payerPayments.Where(p => p.PaymentNumber == numberPayerPayment).Sum(p => p.Amount);
        }
    }

    public static class HelperDateTime
    {
        public static DateTime AddMonthsOrDays(this DateTime dateTime, int value, PaymentCalculationType type)
        {
            switch (type)
            {
                case PaymentCalculationType.Day:
                    return dateTime.AddDays(value);
                case PaymentCalculationType.Fortnight:
                    return dateTime.AddDays(value * 15);
                case PaymentCalculationType.Month:
                    return dateTime.AddMonths(value);
            }
            return dateTime;
        }
    }
}
