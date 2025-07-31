using Sistran.Core.Application.AccountingServices.DTOs.Imputations;
using Sistran.Core.Application.AccountingServices.DTOs.Search;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.AccountingServices.EEProvider
{
    public class AccountingComponentDistributionServiceEEProvider : IAccountingComponentDistributionService
    {
        readonly PolicyComponentDistributionDAO _policyComponentDistributionDAO = new PolicyComponentDistributionDAO();
        public void CreateApplicationPremiumComponent(ParamApplicationPremiumComponent paramApplicationPremiumComponent)
        {
            try
            {
                var payerPaymentDTOs = DelegateService.integrationUnderwritingService.GetPayerPaymet(paramApplicationPremiumComponent.EndorsementId, paramApplicationPremiumComponent.QuotaNumber);
                var payerPaymentComponentDTOs = DelegateService.integrationUnderwritingService.GetPayerPaymetComponents(payerPaymentDTOs.PayerPaymentId);
                var payerPaymentComponenTLBSBDTOs = DelegateService.integrationUnderwritingService.GetPayerPaymetComponentsLBSB(payerPaymentDTOs.PayerPaymentId);
                var tempApplicationPremiumComponents = new AccountingApplicationServiceEEProvider().GetTempApplicationPremiumComponentsByTemApp(paramApplicationPremiumComponent.TempApplicationPremiumCode);
                var tempApplicationPremiumCommisionDTOs = new AccountingApplicationServiceEEProvider().GetTempApplicationPremiumCommissByTempAppId(string.Empty,string.Empty,paramApplicationPremiumComponent.TempApplicationPremiumCode);//comisiones temporales a reales
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
                    var payerPaymentComponenTLBSBByComponentDTOs = payerPaymentComponenTLBSBDTOs.Where(m => m.ComponentId == PayerPaymentComponent.ComponentId);
                    var total = payerPaymentComponenTLBSBByComponentDTOs.Sum(m => m.Amount);
                    var diffAmount = total;
                    var diffLocalAmount = payerPaymentComponenTLBSBByComponentDTOs.Sum(m => m.LocalAmount);
                    var diffMainAmount = payerPaymentComponenTLBSBByComponentDTOs.Sum(m => m.MainAmount);
                    var diffMainLocalAmount = payerPaymentComponenTLBSBByComponentDTOs.Sum(m => m.MainLocalAmount);

                    var last = payerPaymentComponenTLBSBByComponentDTOs.Count();
                    int index = 0;
                    if (newPayerPayment.ComponentId == 1)
                    {
                        foreach (DiscountedCommissionDTO tempApplicationPremiumCommisionDTO in tempApplicationPremiumCommisionDTOs)
                        {
                            ApplicationPremiumCommision applicationPremiumCommiss = new ApplicationPremiumCommision
                            {
                                AgentAgencyId = tempApplicationPremiumCommisionDTO.AgentAgencyId,
                                AgentIndividualId = tempApplicationPremiumCommisionDTO.AgentId,
                                AgentTypeId = tempApplicationPremiumCommisionDTO.AgentTypeCode,
                                Amount = tempApplicationPremiumCommisionDTO.Amount,
                                AppComponentId = newPayerPayment.AppComponentId,
                                CommissionType = tempApplicationPremiumCommisionDTO.CommissionType,
                                CurrencyId = tempApplicationPremiumCommisionDTO.CurrencyId,
                                LocalAmount = tempApplicationPremiumCommisionDTO.LocalAmount,
                                ExchangeRate = tempApplicationPremiumCommisionDTO.ExchangeRate
                            };
                            var newPremiumCommission = new AccountingApplicationServiceEEProvider().SavePremiumCommission(applicationPremiumCommiss);
                        }
                    }
                    
                    foreach (PayerPaymentComponentLBSBDTO payerPaymentComponentLBSB in payerPaymentComponenTLBSBByComponentDTOs)
                    {
                        payerPaymentComponentLBSB.Percentage = total == 0 ? 0 : payerPaymentComponentLBSB.Amount / total;
                        ApplicationPremiumComponentLBSB applicationPremiumComponentLBSB = new ApplicationPremiumComponentLBSB
                        { 
                            Amount = tempApplicationPremiumComponent.Amount * payerPaymentComponentLBSB.Percentage,
                            ApplicationComponentId = newPayerPayment.AppComponentId,
                            CurrencyId = tempApplicationPremiumComponent.ComponentCurrencyCode,
                            ExchangeRateId = tempApplicationPremiumComponent.ExchangeRate,
                            LineBussinesId = payerPaymentComponentLBSB.LineBusiness,
                            SubLineBussinesId = payerPaymentComponentLBSB.SubLineBusiness,
                            LocalAmount = tempApplicationPremiumComponent.LocalAmount * payerPaymentComponentLBSB.Percentage,
                            MainAmount = tempApplicationPremiumComponent.MainAmount * payerPaymentComponentLBSB.Percentage,
                            MainLocalAmount = tempApplicationPremiumComponent.MainLocalAmount * payerPaymentComponentLBSB.Percentage
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
            catch
            {
                throw;
            }
        }
    }
}
