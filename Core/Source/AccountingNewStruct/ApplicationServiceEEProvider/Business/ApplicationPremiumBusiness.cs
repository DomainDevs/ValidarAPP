using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using Sistran.Core.Application.AccountingServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class ApplicationPremiumBusiness
    {
        /// <summary>
        /// Calculates the premium.
        /// </summary>
        /// <param name="moduleId">The module identifier.</param>
        /// <param name="applicationPremiumTransactionItems">The application premium transaction items.</param>
        /// <returns></returns>
        public static DebitCreditDTO CalculatePremium(int moduleId, List<ApplicationPremiumTransactionItem> applicationPremiumTransactionItems)
        {
            DebitCreditDTO debitCreditDTO = new DebitCreditDTO();
            if (moduleId == Convert.ToInt32(ImputationItemTypes.PremiumsReceivable))
            {
                debitCreditDTO.Credit = applicationPremiumTransactionItems.Where(s => s.Amount.Value > 0).Sum(m => m.Amount.Value);
                debitCreditDTO.Debit = applicationPremiumTransactionItems.Where(s => s.Amount.Value < 0).Sum(m => m.Amount.Value);
            }
            if (moduleId == Convert.ToInt32(ImputationItemTypes.CommissionRetained))
            {
                debitCreditDTO.Debit = applicationPremiumTransactionItems.Where(s => s.Amount.Value > 0).Sum(m => m.DeductCommission.Value);
                debitCreditDTO.Credit = applicationPremiumTransactionItems.Where(s => s.Amount.Value < 0).Sum(m => m.DeductCommission.Value);
            }
            return debitCreditDTO;
        }

        /// <summary>
        /// Creates the premium component.
        /// </summary>
        /// <param name="PaymentComponentModel">The payment component model.</param>
        /// <returns></returns>
        public static List<TempApplicationPremiumComponent> CreatePremiumComponent(PaymentComponentModel PaymentComponentModel)
        {
            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = new List<TempApplicationPremiumComponent>();
            decimal newExchangeRate = 1;
            decimal localAmount = decimal.Zero;
            int decimalPlaces = 2;
            decimal percentage;
            foreach (Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PayerPaymentComponentDTO payerPaymentComponentDTO in PaymentComponentModel.payerPaymentComponentDTOs)
            {
                // Se calcula el porcentaje de coaseguro sólo para el componente de prima
                if (payerPaymentComponentDTO.TinyDescription == ComponentTypes.P.ToString())
                {
                    percentage = PaymentComponentModel.PercentageCoinsurance / 100;
                    newExchangeRate = PaymentComponentModel.ExchangeRate;
                }
                else
                {
                    percentage = 1;
                    newExchangeRate = payerPaymentComponentDTO.ExchangeRate;
                }
                localAmount = Math.Round(payerPaymentComponentDTO.Amount * newExchangeRate, decimalPlaces);

                TempApplicationPremiumComponent tempApplicationPremiumComponent = new TempApplicationPremiumComponent
                {
                    TempApplicationPremiumCode = PaymentComponentModel.AppId,
                    ComponentCode = payerPaymentComponentDTO.ComponentId,
                    ComponentCurrencyCode = PaymentComponentModel.CurrencyId,
                    ExchangeRate = newExchangeRate,
                    Amount = payerPaymentComponentDTO.Amount,
                    LocalAmount = localAmount,
                    MainAmount = Math.Round(percentage * payerPaymentComponentDTO.Amount, decimalPlaces),
                    MainLocalAmount = Math.Round(percentage * localAmount, decimalPlaces)
                };
                tempApplicationPremiumComponents.Add(tempApplicationPremiumComponent);
            }
            return tempApplicationPremiumComponents;
        }

        /// <summary>
        /// Creates the premium component.
        /// </summary>
        /// <param name="PaymentComponentModel">The payment component model.</param>
        /// <returns></returns>
        public static List<TempApplicationPremiumComponent> CreatePremiumComponentForLocalAmount(PaymentComponentModel PaymentComponentModel)
        {
            List<TempApplicationPremiumComponent> tempApplicationPremiumComponents = new List<TempApplicationPremiumComponent>();
            decimal newExchangeRate = 1;
            decimal amount = decimal.Zero;
            int decimalPlaces = 2;
            decimal percentage;
            foreach (Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.PayerPaymentComponentDTO payerPaymentComponentDTO in PaymentComponentModel.payerPaymentComponentDTOs)
            {
                // Se calcula el porcentaje de coaseguro sólo para el componente de prima
                if (payerPaymentComponentDTO.TinyDescription == ComponentTypes.P.ToString())
                {
                    percentage = PaymentComponentModel.PercentageCoinsurance / 100;
                    newExchangeRate = PaymentComponentModel.ExchangeRate;
                }
                else
                {
                    percentage = 1;
                    newExchangeRate = payerPaymentComponentDTO.ExchangeRate;
                }
                amount = Math.Round(payerPaymentComponentDTO.LocalAmount / newExchangeRate, decimalPlaces);

                TempApplicationPremiumComponent tempApplicationPremiumComponent = new TempApplicationPremiumComponent
                {
                    TempApplicationPremiumCode = PaymentComponentModel.AppId,
                    ComponentCode = payerPaymentComponentDTO.ComponentId,
                    ComponentCurrencyCode = PaymentComponentModel.CurrencyId,
                    ExchangeRate = newExchangeRate,
                    Amount = amount,
                    LocalAmount = payerPaymentComponentDTO.LocalAmount,
                    MainAmount = Math.Round(percentage * amount, decimalPlaces),
                    MainLocalAmount = Math.Round(percentage * payerPaymentComponentDTO.LocalAmount, decimalPlaces)
                };
                tempApplicationPremiumComponents.Add(tempApplicationPremiumComponent);
            }
            return tempApplicationPremiumComponents;
        }

        public void SaveApplicationPremiumComponents(ParamApplicationPremiumComponent paramApplicationPremiumComponent)
        {
            ApplicationBusiness applicationBusiness = new ApplicationBusiness();
            ApplicationPremiumCommisionDAO applicationPremiumCommisionDAO = new ApplicationPremiumCommisionDAO();
            PolicyComponentDistributionDAO _policyComponentDistributionDAO = new PolicyComponentDistributionDAO();
            TempApplicationPremiumComponentDAO tempApplicationPremiumComponentDAO = new TempApplicationPremiumComponentDAO();

            var payerPaymentDTOs = DelegateService.integrationUnderwritingService.GetPayerPaymet(paramApplicationPremiumComponent.EndorsementId, paramApplicationPremiumComponent.QuotaNumber);

            var payerPaymentComponentDTOs = applicationBusiness.GetPayerPaymentComponentsByEndorsementIdQuotaNumber(
                paramApplicationPremiumComponent.EndorsementId, paramApplicationPremiumComponent.QuotaNumber);
            var payerPaymentComponentsLBSBDTOs = applicationBusiness.GetPayerPaymentComponentsLSBSByPayerPaymentIdEndorsementIdQuotaNumber(payerPaymentDTOs.PayerPaymentId,
                paramApplicationPremiumComponent.EndorsementId, paramApplicationPremiumComponent.QuotaNumber);
            var tempApplicationPremiumComponents = tempApplicationPremiumComponentDAO.GetTempApplicationPremiumComponentsByTemAppPremium(paramApplicationPremiumComponent.TempApplicationPremiumCode);
            var tempApplicationPremiumCommisions = applicationPremiumCommisionDAO.GetTempApplicationPremiumCommissByTempAppId(paramApplicationPremiumComponent.TempApplicationPremiumCode);

            foreach (PayerPaymentComponentDTO PayerPaymentComponent in payerPaymentComponentDTOs)
            {
                var tempApplicationPremiumComponent = tempApplicationPremiumComponents.Where(m => m.ComponentCode == PayerPaymentComponent.ComponentId).FirstOrDefault();
                if (tempApplicationPremiumComponent == null)
                    continue;

                ApplicationPremiumComponent applicationPremiumComponent = new ApplicationPremiumComponent
                {
                    PremiumId = paramApplicationPremiumComponent.PremiumId,
                    ComponentId = PayerPaymentComponent.ComponentId,
                    CurrencyId = tempApplicationPremiumComponent.ComponentCurrencyCode,
                    ExchangeRate = tempApplicationPremiumComponent.ExchangeRate,
                    Amount = tempApplicationPremiumComponent.Amount,
                    LocalAmount = tempApplicationPremiumComponent.LocalAmount,
                    MainAmount = tempApplicationPremiumComponent.MainAmount,
                    MainLocalAmount = tempApplicationPremiumComponent.MainLocalAmount,
                };

                var newPayerPayment = _policyComponentDistributionDAO.saveApplicationPremiumComponent(applicationPremiumComponent);

                var payerPaymentComponenTLBSBByComponentDTOs = payerPaymentComponentsLBSBDTOs.
                    Where(m => m.ComponentId == PayerPaymentComponent.ComponentId).ToList();

                var componentsLBSBTotal = payerPaymentComponenTLBSBByComponentDTOs.Sum(m => m.Amount);
                var diffAmount = tempApplicationPremiumComponent.Amount;
                var diffLocalAmount = tempApplicationPremiumComponent.LocalAmount;
                var diffMainAmount = tempApplicationPremiumComponent.MainAmount;
                var diffMainLocalAmount = tempApplicationPremiumComponent.MainLocalAmount;

                var last = payerPaymentComponenTLBSBByComponentDTOs.Count();
                int index = 1;
                if (newPayerPayment.ComponentId == 1)
                {
                    foreach (ApplicationPremiumCommision tempApplicationPremiumCommision in tempApplicationPremiumCommisions)
                    {
                        ApplicationPremiumCommision applicationPremiumCommiss = new ApplicationPremiumCommision
                        {
                            AgentAgencyId = tempApplicationPremiumCommision.AgentAgencyId,
                            AgentIndividualId = tempApplicationPremiumCommision.AgentIndividualId,
                            AgentTypeId = tempApplicationPremiumCommision.AgentTypeId,
                            Amount = tempApplicationPremiumCommision.Amount,
                            AppComponentId = newPayerPayment.AppComponentId,
                            CommissionType = tempApplicationPremiumCommision.CommissionType,
                            CurrencyId = tempApplicationPremiumCommision.CurrencyId,
                            LocalAmount = tempApplicationPremiumCommision.LocalAmount,
                            ExchangeRate = tempApplicationPremiumCommision.ExchangeRate
                        };
                        applicationPremiumCommisionDAO.CreateApplicationPremiumCommision(applicationPremiumCommiss);
                    }
                }

                foreach (PayerPaymentComponentLBSBDTO payerPaymentComponentLBSB in payerPaymentComponenTLBSBByComponentDTOs)
                {
                    var percentage = componentsLBSBTotal == 0 ? 0 : payerPaymentComponentLBSB.Amount / componentsLBSBTotal;
                    ApplicationPremiumComponentLBSB applicationPremiumComponentLBSB = new ApplicationPremiumComponentLBSB
                    {
                        ApplicationComponentId = newPayerPayment.AppComponentId,
                        CurrencyId = tempApplicationPremiumComponent.ComponentCurrencyCode,
                        ExchangeRateId = tempApplicationPremiumComponent.ExchangeRate,
                        LineBussinesId = payerPaymentComponentLBSB.LineBusiness,
                        SubLineBussinesId = payerPaymentComponentLBSB.SubLineBusiness,
                        Amount = tempApplicationPremiumComponent.Amount * percentage,
                        LocalAmount = tempApplicationPremiumComponent.LocalAmount * percentage,
                        MainAmount = tempApplicationPremiumComponent.MainAmount * percentage,
                        MainLocalAmount = tempApplicationPremiumComponent.MainLocalAmount * percentage
                    };

                    if (index++ == last)
                    {
                        applicationPremiumComponentLBSB.Amount = diffAmount;
                        applicationPremiumComponentLBSB.LocalAmount = diffLocalAmount;
                        applicationPremiumComponentLBSB.MainAmount = diffMainAmount;
                        applicationPremiumComponentLBSB.MainLocalAmount = diffMainLocalAmount;
                    }
                    else
                    {
                        diffAmount -= applicationPremiumComponentLBSB.Amount;
                        diffLocalAmount -= applicationPremiumComponentLBSB.LocalAmount;
                        diffMainAmount -= applicationPremiumComponentLBSB.MainAmount;
                        diffMainLocalAmount -= applicationPremiumComponentLBSB.MainLocalAmount;
                    }
                    var newPayerPaymentComponentLBSB = _policyComponentDistributionDAO.saveApplicationPremiumComponentLBSB(applicationPremiumComponentLBSB);
                }
            }

            bool updateState = false;
            if (payerPaymentDTOs.Amount < 0)
            {
                if (paramApplicationPremiumComponent.ApplicationAmount < 0 && Math.Abs(paramApplicationPremiumComponent.ApplicationAmount) >= Math.Abs(payerPaymentDTOs.Amount))
                    updateState = true;
            }
            else
            {
                if (paramApplicationPremiumComponent.ApplicationAmount >= payerPaymentDTOs.Amount)
                    updateState = true;
            }
            if (updateState)
            {
                payerPaymentDTOs.PaymentState = 2;
                DelegateService.integrationUnderwritingService.UpdateStatusPayerPayment(payerPaymentDTOs);
            }
        }
    }
}
