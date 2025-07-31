using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using PRODEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers
{
    public static class DTOAssembler
    {
        internal static PolicyDTO CreateClaimPolicy(Policy policy)
        {
            return new PolicyDTO
            {
                Id = policy.Id,
                DocumentNumber = policy.DocumentNumber,
                RiskId = policy.Risk.Id,
                RiskDescription = policy.Risk.Description,
                BranchId = policy.Branch.Id,
                BusinessTypeId = (int)policy.BusinessType,
                BusinessType = policy.BusinessTypeDescription,
                ClaimsQuantity = policy.SinisterQuantity.ToString(),
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.CurrentTo,
                EndorsementId = policy.Endorsement.Id,
                HolderId = Convert.ToInt32(policy.Holder.IndividualId),
                HolderDocumentNumber = policy.Holder.IdentificationDocument.Number,
                HolderName = policy.Holder.Name,
                IssueDate = policy.IssueDate.ToString(),
                PrefixId = policy.Prefix.Id,
                IndividualId = policy.Risk.MainInsured.IndividualId,
                ProductDescription = policy.Product.Description
            };
        }

        internal static PolicyDTO CreatePolicy(Policy policy)
        {
            return new PolicyDTO
            {
                Id = policy.Endorsement.PolicyId,
                DocumentNumber = policy.DocumentNumber,
                EndorsementId = policy.Endorsement.Id,
                BranchId = policy.Branch.Id,
                BranchDescription = policy.Branch.Description,
                PrefixId = policy.Prefix.Id,
                PrefixDescription = policy.Prefix.Description,
                CurrencyId = policy.ExchangeRate.Currency.Id,
                CurrencyDescription = policy.ExchangeRate.Currency.Description,
                BusinessType = policy.BusinessTypeDescription,
                BusinessTypeId = (int)policy.BusinessType,
                ClaimsQuantity = "0",
                EndorsementType = policy.Endorsement.EndorsementTypeDescription,
                EndorsementTypeId = (int)policy.Endorsement.EndorsementType,
                Inforce = 1,
                PendingDebt = "0",
                IssueDate = policy.IssueDate.ToShortDateString(),
                Premium = policy.Summary.Premium,
                FullPremium = policy.Summary.FullPremium,
                PolicyTypeId = policy.PolicyType.Id,
                PolicyType = policy.PolicyType.Description,
                TaxExpenses = policy.Summary.Taxes + policy.Summary.Expenses,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.Endorsement.CurrentTo,
                Agent = policy.Agencies.FirstOrDefault().Agent.FullName,
                CoInsurance = CreateCoInsurances(policy.CoInsuranceCompanies, policy.Summary),
                EndorsementDocumentNum = policy.Endorsement.Number,
                ProductId = policy.Product.Id,
                ExpensesLocal = policy.Summary.ExpensesLocal,
                HolderId = Convert.ToInt32(policy.Holder?.IndividualId),
                HolderDocumentNumber = policy.Holder?.IdentificationDocument?.Number,
                HolderName = policy.Holder?.Name,
                SalePointId = Convert.ToInt32(policy.Branch.SalePoints?.FirstOrDefault()?.Id)
            };
        }

        internal static List<PolicyDTO> CreateClaimPolicies(List<Policy> policies)
        {
            List<PolicyDTO> policiesDTO = new List<PolicyDTO>();
            foreach (Policy policy in policies)
            {
                policiesDTO.Add(CreateClaimPolicy(policy));
            }

            return policiesDTO;
        }

        internal static List<CoInsuranceAssignedDTO> CreateCoInsuranceAssigneds(List<CoInsuranceAssigned> coInsuranceAssigneds)
        {
            List<CoInsuranceAssignedDTO> coInsuranceAssignedDTOs = new List<CoInsuranceAssignedDTO>();
            foreach (CoInsuranceAssigned coInsuranceAssigned in coInsuranceAssigneds)
            {
                coInsuranceAssignedDTOs.Add(CreateCoInsuranceAssigned(coInsuranceAssigned));
            }

            return coInsuranceAssignedDTOs;
        }

        internal static CoInsuranceAssignedDTO CreateCoInsuranceAssigned(CoInsuranceAssigned coInsuranceAssigned)
        {
            return new CoInsuranceAssignedDTO
            {
                EndorsementId = coInsuranceAssigned.EndorsementId,
                PolicyId = coInsuranceAssigned.PolicyId,
                CompanyNum = coInsuranceAssigned.CompanyNum,
                ExpensesPercentage = coInsuranceAssigned.ExpensesPercentage,
                InsuranceCompanyId = coInsuranceAssigned.InsuranceCompanyId,
                PartCiaPercentage = coInsuranceAssigned.PartCiaPercentage
            };
        }

        internal static CoInsuranceDTO CreateCoInsurance(IssuanceCoInsuranceCompany coInsurance, Summary summary)
        {
            return new CoInsuranceDTO
            {
                CompanyId = Convert.ToInt32(coInsurance.Id),
                Company = coInsurance.Description,
                Participation = coInsurance.ParticipationPercentage,
                SumAssuredAmount = Convert.ToDecimal(summary == null ? 0 : (summary.AmountInsured * (coInsurance.ParticipationPercentage / 100))),
                PremiumAmount = Convert.ToDecimal(summary == null ? 0 : (summary.Premium * (coInsurance.ParticipationPercentage / 100))),
                ParticipationOwn = coInsurance.ParticipationPercentageOwn
            };
        }

        internal static List<CoInsuranceDTO> CreateCoInsurances(List<IssuanceCoInsuranceCompany> coInsurances, Summary summary)
        {
            List<CoInsuranceDTO> coInsurancesDTO = new List<CoInsuranceDTO>();
            foreach (IssuanceCoInsuranceCompany issuanceCoInsuranceCompany in coInsurances)
            {
                coInsurancesDTO.Add(CreateCoInsurance(issuanceCoInsuranceCompany, summary));
            }

            return coInsurancesDTO;
        }

        internal static InsuredDTO CreateInsured(IssuanceInsured insured)
        {
            return new InsuredDTO
            {
                Id = insured.InsuredId,
                IndividualId = insured.IndividualId,
                DocumentNumber = insured.IdentificationDocument.Number,
                FullName = insured.IndividualType == Services.UtilitiesServices.Enums.IndividualType.Person ? (
                            insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name
                            ) : insured.Name,
                DocumentTypeId = insured.IdentificationDocument.DocumentType.Id
            };
        }

        internal static List<InsuredDTO> CreateInsureds(List<IssuanceInsured> insureds)
        {
            List<InsuredDTO> insuredDTOs = new List<InsuredDTO>();

            foreach (IssuanceInsured insured in insureds)
            {
                insuredDTOs.Add(CreateInsured(insured));
            }

            return insuredDTOs;
        }

        internal static HolderDTO CreateHolder(Holder holder)
        {
            return new HolderDTO
            {
                Id = holder.InsuredId,
                IndividualId = holder.IndividualId,
                DocumentNumber = holder.IdentificationDocument.Number,                
                FullName = holder.Name,
                DocumentTypeId = holder.IdentificationDocument.DocumentType.Id
            };
        }

        internal static List<HolderDTO> CreateHolders(List<Holder> holders)
        {
            if (holders == null)
                return new List<HolderDTO>();
                
            List<HolderDTO> holdersDTO = new List<HolderDTO>();
            foreach (Holder holder in holders)
            {
                holdersDTO.Add(CreateHolder(holder));
            }

            return holdersDTO;
        }

        internal static CoverageDTO CreateCoverage(Coverage coverage)
        {
            return new CoverageDTO
            {
                Id = coverage.Id,
                Description = coverage.Description,
                InsuredAmountTotal = coverage.SubLimitAmount,
                InsurableAmount = coverage.DeclaredAmount,
                OcurrencyLimit = coverage.LimitOccurrenceAmount,
                PersonLimit = coverage.LimitClaimantAmount,
                AggregateLimit = coverage.DeclaredAmount,
                CoverageId = coverage.Id,
                RiskCoverageId = coverage.RiskCoverageId,
                SubLineBusinessCode = coverage.SubLineBusiness.Id,
                LineBusinessCode = coverage.SubLineBusiness.LineBusinessId,
                SubLineBusinessDescription = coverage.SubLineBusiness.Description,
                InsuredObjectId = coverage.InsuredObject.Id,
                InsuredObjectDescription = coverage.InsuredObject.Description,
                Number = coverage.Number,
                LimitOccurrenceAmount = coverage.LimitOccurrenceAmount
            };
        }

        internal static List<CoverageDTO> CreateCoverages(List<Coverage> coverages)
        {
            List<CoverageDTO> coveragesDTO = new List<CoverageDTO>();

            foreach (Coverage coverage in coverages)
            {
                coveragesDTO.Add(CreateCoverage(coverage));
            }

            return coveragesDTO.OrderBy(x => x.Description).ToList();
        }

        internal static DeductibleDTO CreateDeductible(Deductible deductible)
        {
            if (deductible == null)
            {
                return null;
            }

            return new DeductibleDTO
            {
                Code = deductible.Id,
                Description = deductible.Description
            };
        }

        internal static List<DeductibleDTO> CreateDeductibles(List<Deductible> deductibles)
        {
            List<DeductibleDTO> deductiblesDTO = new List<DeductibleDTO>();

            foreach (Deductible deductible in deductibles)
            {
                deductiblesDTO.Add(CreateDeductible(deductible));
            }

            return deductiblesDTO;
        }

        internal static RiskCommercialClassDTO CreateRiskCommercialClass(RiskCommercialClass riskCommercialClass)
        {
            if (riskCommercialClass == null)
                return null;

            return new RiskCommercialClassDTO
            {
                Id = riskCommercialClass.RiskCommercialClassCode,
                Description = riskCommercialClass.Description
            };
        }

        internal static List<RiskCommercialClassDTO> CreateRiskCommercialClasses(List<RiskCommercialClass> riskCommercialClasses)
        {
            List<RiskCommercialClassDTO> riskCommercialClassesDTO = new List<RiskCommercialClassDTO>();

            foreach (RiskCommercialClass riskCommercialClass in riskCommercialClasses)
            {
                riskCommercialClassesDTO.Add(CreateRiskCommercialClass(riskCommercialClass));
            }

            return riskCommercialClassesDTO;
        }

        internal static PolicyDTO CreatePolicyByPrefixIdBranchIdPolicyNumber(Policy policy)
        {
            return new PolicyDTO
            {
                Id = policy.Endorsement.PolicyId,
                DocumentNumber = policy.DocumentNumber,
                EndorsementId = policy.Endorsement.Id,
                BranchId = policy.Branch.Id,
                PrefixId = policy.Prefix.Id,
                EndorsementType = policy.Endorsement.EndorsementTypeDescription,
                CurrentFrom = policy.CurrentFrom,
                CurrentTo = policy.CurrentTo,
                EndorsementDocumentNum = policy.Endorsement.Number
            };
        }

        internal static PolicyDTO GetPolicyByPolicyId(Policy policy)
        {
            return new PolicyDTO
            {
                Id = policy.Id,
                DocumentNumber = policy.DocumentNumber,
                BranchId = policy.Branch.Id,
                PrefixId = policy.Prefix.Id,
            };
        }

        internal static List<InsuredObjectDTO> CreateGetInsuredObjectByPrefixId(List<InsuredObject> insuredObjects)
        {
            List<InsuredObjectDTO> insuredObjectDTOs = new List<InsuredObjectDTO>();

            foreach (InsuredObject insuredObject in insuredObjects)
            {
                insuredObjectDTOs.Add(new InsuredObjectDTO
                {
                    Id = insuredObject.Id,
                    Description = insuredObject.Description,
                    SmallDescription = insuredObject.SmallDescription,
                    IsDeclarative = insuredObject.IsDeclarative,
                    IsMandatory = insuredObject.IsMandatory,
                    Premium = insuredObject.Premium,
                    Amount = insuredObject.Amount,
                    IsSelected = insuredObject.IsSelected
                });
            }
            return insuredObjectDTOs;
        }

        internal static PayerPaymentDTO CreatePayerPaymentDTO(PayerPayment payerPayment)
        {
            return new PayerPaymentDTO()
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
                PaymentPct = payerPayment.PaymentPercentage,
                PaymentState = payerPayment.PaymentState,
                PolicyId = payerPayment.PolicyId
            };
        }

        internal static List<PayerPaymentComponentDTO> CreatePayerPaymentCompsDTO(List<PayerPaymentComp> payerPaymentComps)
        {
            List<PayerPaymentComponentDTO> payerPaymentComp = new List<PayerPaymentComponentDTO>();

            foreach (PayerPaymentComp ppComp in payerPaymentComps)
            {
                payerPaymentComp.Add(new PayerPaymentComponentDTO
                {
                    Amount = ppComp.Amount,
                    ComponentId = ppComp.ComponentCode,
                    LocalAmount = ppComp.LocalAmount,
                    MainAmount = ppComp.MainAmount,
                    MainLocalAmount = ppComp.MainLocalAmount,
                    PayerPaymentComponentId = ppComp.PayerPaymentCompId,
                    PayerPaymentId = ppComp.PayerPaymentId,
                    PaymentPct = ppComp.PaymentPercentage,
                    TinyDescription = ppComp.TinyDescription,
                    ExchangeRate = ppComp.ExchangeRate
                });
            }
            return payerPaymentComp;
        }
        internal static List<PremiumSearchPolicyDTO> CreateSearchPremiumPolicies(List<PremiumSearchPolicy> premiumSearchPolicyItems)
        {
            List<PremiumSearchPolicyDTO> premiumSearchPolicyDTOs = new List<PremiumSearchPolicyDTO>();
            foreach (PremiumSearchPolicy premiumSearchPolicy in premiumSearchPolicyItems)
            {
                premiumSearchPolicyDTOs.Add(new PremiumSearchPolicyDTO
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
                    TotalPremium = premiumSearchPolicy.Amount,
                    CurrencyId = premiumSearchPolicy.CurrencyId,
                    PrefixTinyDescription = premiumSearchPolicy.PrefixTinyDescription
                });
            }
            return premiumSearchPolicyDTOs;

        }

        internal static List<PayerPaymentComponentLBSBDTO> CreatePayerPaymentCompsDTO(List<PayerPaymentCompLbsb> payerPaymentCompLbsbs)
        {
            List<PayerPaymentComponentLBSBDTO> payerPaymentCompLbsb = new List<PayerPaymentComponentLBSBDTO>();

            foreach (PayerPaymentCompLbsb ppCompL in payerPaymentCompLbsbs)
            {
                payerPaymentCompLbsb.Add(new PayerPaymentComponentLBSBDTO
                {
                    Amount = ppCompL.Amount,
                    ComponentId = ppCompL.ComponentCode,
                    LineBusiness = ppCompL.LineBusinessCode,
                    LocalAmount = ppCompL.LocalAmount,
                    MainAmount = ppCompL.MainAmount,
                    MainLocalAmount = ppCompL.MainLocalAmount,
                    PayerPaymentComponentLBSBId = ppCompL.PayerPaymentCompLbsbId,
                    PayerPaymentId = ppCompL.PayerPaymentId,
                    SubLineBusiness = ppCompL.SubLineBusinessCode
                });
            }
            return payerPaymentCompLbsb;
        }

        internal static List<RiskDTO> CreateRiskDTOs(List<Risk> risks)
        {
            List<RiskDTO> riskDTOs = new List<RiskDTO>();
            foreach (Risk risk in risks)
            {
                riskDTOs.Add(CreateRiskDTO(risk));
            }

            return riskDTOs;
        }

        internal static RiskDTO CreateRiskDTO(Risk risk)
        {
            return new RiskDTO
            {
                Id = risk.RiskId
            };
        }

        internal static RiskDTO CreateRiskSuretyDTO(Risk risk)
        {
            return new RiskDTO
            {
                Id = risk.RiskId,
                IndividualId = risk.SecondInsured.IndividualId
            };
        }

        internal static IssuanceAgencyDTO CreateIssuanceAgency(IssuanceAgency issuance, bool canUsedCommission)
        {
            return new IssuanceAgencyDTO
            {
                AgentIndividualId = issuance.Agent.IndividualId,
                AgentTypeCode = issuance.Agent.AgentType.Id,
                BaseAmount = issuance.Commissions.FirstOrDefault().Amount,
                CommissionPercentage = issuance.Commissions.FirstOrDefault().Percentage,
                AgentPercentageParticipation = issuance.Participation,
                AgentName = issuance.Agent.FullName,
                IsUsedCommission = canUsedCommission,
                AgentAgencyId = issuance.Id

            };
        }

        internal static List<IssuanceAgencyDTO> CreateIssuanceAgencys(List<IssuanceAgency> issuanceAgency, bool canUsedCommission)
        {
            List<IssuanceAgencyDTO> issuanceAgencyDTOs = new List<IssuanceAgencyDTO>();

            foreach (IssuanceAgency issuance in issuanceAgency)
            {
                issuanceAgencyDTOs.Add(CreateIssuanceAgency(issuance, canUsedCommission));
            }

            return issuanceAgencyDTOs;
        }

        /// <summary>
        /// Creates the components.
        /// </summary>
        /// <param name="components">The components.</param>
        /// <returns></returns>
        public static List<ComponentTypeDTO> CreateComponents(List<QUOEN.Component> components)
        {
            var mapper = AutoMapperAssembler.CreateMapComponentType();
            return mapper.Map<List<QUOEN.Component>, List<ComponentTypeDTO>>(components);
        }

        /// <summary>
        /// Creates the payment distribution components.
        /// </summary>
        /// <param name="paymentDistributionComponents">The payment distribution components.</param>
        /// <returns></returns>
        public static List<PaymentDistributionCompDTO> CreatePaymentDistributionComponents(List<PRODEN.CoPaymentDistributionComponent> paymentDistributionComponents)
        {
            var mapper = AutoMapperAssembler.CreateMapPaymentDistributionComponents();
            return mapper.Map<List<PRODEN.CoPaymentDistributionComponent>, List<PaymentDistributionCompDTO>>(paymentDistributionComponents);
        }

        internal static List<EndorsementBaseDTO> CreateEndorsementBaseDTOs (List<Endorsement> endorsements)
        {
            List<EndorsementBaseDTO> endorsementBaseDTOs = new List<EndorsementBaseDTO>();
            foreach (Endorsement endorsement in endorsements)
            {
                endorsementBaseDTOs.Add(CreateEndorsementDTO(endorsement));
            }

            return endorsementBaseDTOs;
        }

        internal static EndorsementBaseDTO CreateEndorsementDTO(Endorsement endorsement)
        {
            return new EndorsementBaseDTO
            {
                Id = endorsement.Id,
                PolicyId = endorsement.PolicyId
            };
        }

    }
}
