using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers
{
    public static class ModelAssembler
    {
        internal static Policy CreateClaimPolicy(PolicyDTO policy)
        {
            return new Policy
            {
                DocumentNumber = policy.DocumentNumber,
                Branch = new Branch
                {
                    Id = policy.BranchId
                },
                CurrentFrom = Convert.ToDateTime(policy.CurrentFrom),
                CurrentTo = Convert.ToDateTime(policy.CurrentTo),
                Prefix = new Prefix
                {
                    Id = policy.PrefixId
                },
                Holder = new Holder
                {
                    IndividualId = policy.HolderId
                },
                Risk = new Risk
                {
                    RiskId = policy.RiskId,
                    MainInsured = new IssuanceInsured()
                    {
                        IndividualId = policy.IndividualId
                    }
                }
            };
        }

        internal static PayerPayment CreatePayerPaymentModel(ISSEN.PayerPayment payerPayment)
        {
            return new PayerPayment()
            {
                AgtPayExpDate = payerPayment.AgtPayExpDate.HasValue ? (DateTime)payerPayment.AgtPayExpDate : DateTime.MinValue,
                Amount = payerPayment.Amount,
                EndorsementId = payerPayment.EndorsementId,
                LocalAmount = payerPayment.LocalAmount.HasValue ? (decimal)payerPayment.LocalAmount : 0,
                MainAmount = payerPayment.MainAmount.HasValue ? (decimal)payerPayment.MainAmount : 0,
                MainLocalAmount = payerPayment.MainLocalAmount.HasValue ? (decimal)payerPayment.MainLocalAmount : 0,
                PayerId = payerPayment.PayerId,
                PayerPaymentId = payerPayment.PayerPaymentId,
                PayExpDate = payerPayment.PayExpDate,
                PaymentNum = payerPayment.PaymentNum,
                PaymentPercentage = payerPayment.PaymentPercentage.HasValue ? (decimal)payerPayment.PaymentPercentage : 0,
                PaymentState = payerPayment.PaymentState.HasValue ? (int)payerPayment.PaymentState : 0,
                PolicyId = payerPayment.PolicyId
            };
        }

        internal static PayerPayment CreatePayerPaymentModel(PayerPaymentDTO payerPayment)
        {
            return new PayerPayment()
            {
                AgtPayExpDate = payerPayment.AgtPayExpDate,
                Amount = payerPayment.Amount,
                EndorsementId = payerPayment.EndorsementId,
                LocalAmount = payerPayment.LocalAmount,
                MainAmount = payerPayment.MainAmount,
                MainLocalAmount = payerPayment.MainLocalAmount,
                PayerId = payerPayment.PayerId,
                PayerPaymentId = payerPayment.PayerPaymentId,
                PayExpDate = payerPayment.PayExpDate,
                PaymentNum = payerPayment.PaymentNum,
                PaymentState = payerPayment.PaymentState,
                PolicyId = payerPayment.PolicyId
            };
        }

        internal static List<PayerPaymentComp> CreatePayerPaymentComp(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapPayerPaymentComp();
            return mapper.Map<List<ISSEN.PayerPaymentComp>, List<PayerPaymentComp>>(businessCollection.Cast<ISSEN.PayerPaymentComp>().ToList());
        }

        internal static List<PayerPaymentCompLbsb> CreatePayerPaymentCompLbsb(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapPayerPaymentComp();
            return mapper.Map<List<ISSEN.PayerPaymentCompLbsb>, List<PayerPaymentCompLbsb>>(businessCollection.Cast<ISSEN.PayerPaymentCompLbsb>().ToList());
        }

        internal static SearchPolicyPayment CreateSearchPolicyPayment(SearchPolicyPaymentDTO searchPolicyPaymentDTO)
        {
            return new SearchPolicyPayment()
            {
                InsuredId = searchPolicyPaymentDTO.InsuredId,
                PayerId = searchPolicyPaymentDTO.PayerId,
                AgentId = searchPolicyPaymentDTO.AgentId,
                BranchId = searchPolicyPaymentDTO.BranchId,
                DateFrom = searchPolicyPaymentDTO.DateFrom,
                DateTo = searchPolicyPaymentDTO.DateTo,
                EndorsementId = searchPolicyPaymentDTO.EndorsementId,
                GroupId = searchPolicyPaymentDTO.GroupId,
                InsuredDocumentNumber = searchPolicyPaymentDTO.InsuredDocumentNumber,
                PageIndex = searchPolicyPaymentDTO.PageIndex,
                PageSize = searchPolicyPaymentDTO.PageSize,
                PolicyDocumentNumber = searchPolicyPaymentDTO.PolicyDocumentNumber,
                PolicyId = searchPolicyPaymentDTO.PolicyId,
                SalesTicket = searchPolicyPaymentDTO.SalesTicket,
                PrefixId = searchPolicyPaymentDTO.PrefixId,
                EndorsementDocumentNumber = searchPolicyPaymentDTO.EndorsementDocumentNumber

            };
        }

        internal static PremiumSearchPolicyDTO CreateSearchPremiumPolicies(PremiumSearchPolicy premiumSearchPolicy)
        {
            return new PremiumSearchPolicyDTO()
            {
                BranchPrefixPolicyEndorsement = premiumSearchPolicy.BranchPrefixPolicyEndorsement,
                PolicyId = premiumSearchPolicy.PolicyId,
                PolicyDocumentNumber = premiumSearchPolicy.PolicyDocumentNumber,
                BussinessTypeDescription = premiumSearchPolicy.BussinessTypeDescription,
                BussinessTypeId = premiumSearchPolicy.BussinessTypeId,
                BranchId = premiumSearchPolicy.BranchId,
                BranchDescription = premiumSearchPolicy.BranchDescription,
                PrefixId = premiumSearchPolicy.PrefixId,
                PrefixDescription = premiumSearchPolicy.PrefixDescription,
                InsuredIndividualId = premiumSearchPolicy.InsuredIndividualId,
                InsuredDocumentNumber = premiumSearchPolicy.InsuredDocumentNumber,
                InsuredName = premiumSearchPolicy.InsuredName,
                EndorsementId = premiumSearchPolicy.EndorsementId,
                EndorsementDocumentNumber = premiumSearchPolicy.EndorsementDocumentNumber,
                EndorsementTypeId = premiumSearchPolicy.EndorsementTypeId,
                EndorsementTypeDescription = premiumSearchPolicy.EndorsementTypeDescription,
                CollectGroupId = premiumSearchPolicy.CollectGroupId,
                CollectGroupDescription = premiumSearchPolicy.CollectGroupDescription,
                PayerId = premiumSearchPolicy.PayerId,
                PayerIndividualId = premiumSearchPolicy.InsuredIndividualId,
                PayerDocumentNumber = premiumSearchPolicy.PayerDocumentNumber,
                PayerName = premiumSearchPolicy.PayerName,
                PaymentExpirationDate = premiumSearchPolicy.PaymentExpirationDate,
                Amount = premiumSearchPolicy.Amount,
                CurrencyDescription = premiumSearchPolicy.CurrencyDescription,
                ExchangeRate = premiumSearchPolicy.ExchangeRate,
                PaymentNumber = premiumSearchPolicy.PaymentNumber,
                TotalPremium = premiumSearchPolicy.Amount
            };
        }
    }
}
