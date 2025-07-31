using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices.DTOs;
using Sistran.Company.Application.UnderwritingServices.EEProvider.ModelsMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using CIAISS = Sistran.Company.Application.Issuance.Entities;
using COISSEN = Sistran.Company.Application.Issuance.Entities;
using COMMENT = Sistran.Core.Application.Common.Entities;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using ENTI = Sistran.Core.Application.UniquePerson.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PROEN = Sistran.Core.Application.Product.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TMPEN = Sistran.Core.Application.Temporary.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region Automapper
        #region payercomponent
        public static IMapper CreateMapCompanyPayerComponent()
        {
            var config = MapperCache.GetMapper<PayerComponent, CompanyPayerComponent>(cfg =>
           {
               cfg.CreateMap<Component, CompanyComponent>();
               cfg.CreateMap<Coverage, CompanyCoverage>();
               cfg.CreateMap<PayerComponent, CompanyPayerComponent>();
               cfg.CreateMap<Text, CompanyText>();
               cfg.CreateMap<Clause, CompanyClause>();
               cfg.CreateMap<Deductible, CompanyDeductible>();
               cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
           });
            return config;
        }
        #endregion payercomponent

        #region endorsement
        public static IMapper CreateMapEndorsement()
        {
            var config = MapperCache.GetMapper<CompanyEndorsement, Endorsement>(cfg =>
            {
                cfg.CreateMap<CompanyEndorsement, Endorsement>();
                cfg.CreateMap<CompanyText, Text>();
            });
            return config;
        }
        public static IMapper CreateMapCompanyEndorsement()
        {
            var config = MapperCache.GetMapper<Endorsement, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                cfg.CreateMap<Text, CompanyText>();
            });
            return config;
        }

        public static IMapper CreateMapEndorsementCompany()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.Endorsement, EndorsementCompanyDTO>(cfg =>
            {
                cfg.CreateMap<ISSEN.Endorsement, EndorsementCompanyDTO>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.EndorsementId))
                 .ForMember(d => d.EndorsementNumber, o => o.MapFrom(c => c.DocumentNum))
                 .ForMember(d => d.EmissionDate, o => o.MapFrom(c => c.IssueDate))
                .ForMember(d => d.EndorsementType, o => o.MapFrom(c => c.EndoTypeCode));

            });
            return config;
        }

        #endregion endorsement

        #region deductible
        public static IMapper CreateMapDeducible()
        {
            var config = MapperCache.GetMapper<Deductible, CompanyDeductible>(cfg =>
            {
                cfg.CreateMap<Deductible, CompanyDeductible>();
            });
            return config;
        }
        #endregion deductible
        #region Policy
        public static IMapper CreateMapCompanyPolicy()
        {
            IMapper config = MapperCache.GetMapper<CompanyPolicyMapper, CompanyPolicy>(cfg =>
            {
                cfg.CreateMap<CompanyPolicyMapper, CompanyPolicy>()
                .ForMember(d => d.CurrentFrom, o => o.MapFrom(c => c.endorsement.CurrentFrom))
                 .ForMember(d => d.CurrentTo, o => o.MapFrom(c => c.endorsement.CurrentTo.Value))
                 .ForMember(d => d.BeginDate, o => o.MapFrom(c => DateTime.Now))
                 .ForMember(d => d.DocumentNumber, o => o.MapFrom(c => c.policy.DocumentNumber))
                 .ForMember(d => d.Prefix, o => o.MapFrom(c => new CompanyPrefix
                 {
                     Id = c.policy.PrefixCode
                 }))
                 .ForMember(d => d.Endorsement, o => o.MapFrom(c => new CompanyEndorsement
                 {
                     Id = c.endorsement.EndorsementId,
                     PolicyId = c.policy.PolicyId,
                     CurrentFrom = c.policy.CurrentFrom,
                     CurrentTo = c.policy.CurrentTo.Value,
                     EndorsementType = (EndorsementType)c.endorsement.EndoTypeCode,
                     Number = c.endorsement.DocumentNum,
                     AppRelation = c.tempPolicyControl.AppVersionId
                 }))
                 .ForMember(d => d.Product, o => o.MapFrom(c => new CompanyProduct
                 {
                     Id = c.policy.ProductId.Value
                 }))
                  .ForMember(d => d.Branch, o => o.MapFrom(c => new CompanyBranch
                  {
                      Id = c.policy.BranchCode,
                      SalePoints = c.policy.SalePointCode.HasValue ? new System.Collections.Generic.List<CompanySalesPoint>
                      {
                          new CompanySalesPoint
                          {
                              Id = (int)c.policy.SalePointCode
                          }
                      } : null
                  }))
                  .ForMember(d => d.BusinessType, o => o.MapFrom(c => (BusinessType)c.policy.BusinessTypeCode))
                  .ForMember(d => d.PolicyType, o => o.MapFrom(c => new CompanyPolicyType
                  {
                      Id = c.coPolicy.PolicyTypeCode
                  }))
                   .ForMember(d => d.PaymentPlan, o => o.MapFrom(c => new CompanyPaymentPlan
                   {
                       Id = c.endorsementPayer.PaymentScheduleId
                   }))
                    .ForMember(d => d.Holder, o => o.MapFrom(c => new Holder
                    {
                        IndividualId = c.policy.PolicyholderId.Value,
                        CompanyName = new IssuanceCompanyName
                        {
                            NameNum = c.policy.NameNum.GetValueOrDefault()
                        }
                    }))
                    .ForMember(d => d.ExchangeRate, o => o.MapFrom(c => new CommonModel.ExchangeRate
                    {
                        BuyAmount = c.endorsement.ExchangeRate,
                        SellAmount = c.endorsement.ExchangeRate,
                        Currency = new CommonModel.Currency
                        {
                            Id = c.policy.CurrencyCode
                        }
                    }))
                    .ForMember(d => d.DynamicProperties, o => o.MapFrom(c => new List<DynamicConcept>()))
                    .ForMember(d => d.CalculateMinPremium, o => o.MapFrom(c => c.policy.CalculateMinPremium))
                    .ForMember(d => d.IssueDate, o => o.MapFrom(c => c.endorsement.IssueDate))
                    .ForMember(d => d.TemporalType, o => o.MapFrom(c => TemporalType.Policy))
                    .ForMember(d => d.Summary, o => o.MapFrom(c => new CompanySummary()))
                    .ForMember(d => d.PolicyOrigin, o => o.MapFrom(c => (PolicyOrigin)c.tempPolicyControl.PolicyOrigin))
                    .ForMember(d => d.CoInsuranceCompanies, o => o.MapFrom(c => new List<CompanyIssuanceCoInsuranceCompany>()));
            });
            return config;
        }

        public static IMapper CreateMapCompanyClaimPolicy()
        {
            IMapper config = MapperCache.GetMapper<ClaimPolicyMapper, CompanyPolicy>(cfg =>
            {
                cfg.CreateMap<ClaimPolicyMapper, CompanyPolicy>()
                .ForMember(d => d.CurrentFrom, o => o.MapFrom(c => c.policy.CurrentFrom))
                 .ForMember(d => d.CurrentTo, o => o.MapFrom(c => c.policy.CurrentTo.Value))
                 .ForMember(d => d.DocumentNumber, o => o.MapFrom(c => c.policy.DocumentNumber))
                 .ForMember(d => d.Prefix, o => o.MapFrom(c => new CompanyPrefix
                 {
                     Id = c.policy.PrefixCode
                 }))
                 .ForMember(d => d.Endorsement, o => o.MapFrom(c => new CompanyEndorsement
                 {
                     Id = c.endorsement.EndorsementId,
                     PolicyId = c.policy.PolicyId,
                     CurrentFrom = c.endorsement.CurrentFrom,
                     CurrentTo = c.endorsement.CurrentTo.Value,
                     EndorsementType = (EndorsementType)c.endorsement.EndoTypeCode,
                     EndorsementTypeDescription = c.entityEndorsementType.Description,
                     Number = c.endorsement.DocumentNum,
                 }))
                 .ForMember(d => d.Product, o => o.MapFrom(c => new CompanyProduct
                 {
                     Id = c.policy.ProductId.Value
                 }))
                  .ForMember(d => d.Branch, o => o.MapFrom(c => new CompanyBranch
                  {
                      Id = c.policy.BranchCode,
                      SalePoints = c.policy.SalePointCode.HasValue ? new System.Collections.Generic.List<CompanySalesPoint>
                      {
                          new CompanySalesPoint
                          {
                              Id = (int)c.policy.SalePointCode
                          }
                      } : null
                  }))
                  .ForMember(d => d.BusinessType, o => o.MapFrom(c => (BusinessType)c.policy.BusinessTypeCode))
                  .ForMember(d => d.BusinessTypeDescription, o => o.MapFrom(c => c.businessType.SmallDescription))
                  .ForMember(d => d.PolicyType, o => o.MapFrom(c => new CompanyPolicyType
                  {
                      Id = c.coPolicy.PolicyTypeCode,
                      Description = c.entityCoPolicyType.Description
                  }))
                    .ForMember(d => d.Holder, o => o.MapFrom(c => new Holder
                    {
                        IndividualId = c.policy.PolicyholderId.Value,

                    }))
                    .ForMember(d => d.ExchangeRate, o => o.MapFrom(c => new CommonModel.ExchangeRate
                    {
                        BuyAmount = c.endorsement.ExchangeRate,
                        Currency = new CommonModel.Currency
                        {
                            Id = c.policy.CurrencyCode,
                            Description = c.entityCurrency.Description
                        }
                    }))
                    .ForMember(d => d.IssueDate, o => o.MapFrom(c => c.endorsement.IssueDate))
                    .ForMember(d => d.Summary, o => o.MapFrom(c => new CompanySummary()))
                    .ForMember(d => d.CoInsuranceCompanies, o => o.MapFrom(c => new List<CompanyIssuanceCoInsuranceCompany>()))
                    .ForMember(d => d.Agencies, o => o.MapFrom(c => new List<IssuanceAgency>()));
            });
            return config;
        }

        public static IMapper CreateMapDynamicConcept()
        {
            IMapper config = MapperCache.GetMapper<Rules.Concept, DynamicConcept>(cfg =>
            {
                cfg.CreateMap<Rules.Concept, DynamicConcept>();
            });
            return config;
        }

        public static IMapper CreateMapCompanySurcharge()
        {
            IMapper config = MapperCache.GetMapper<CompanySurchargeComponentMapper, CompanySurchargeComponent>(cfg =>
        {
            cfg.CreateMap<CompanySurchargeComponentMapper, CompanySurchargeComponent>()
            .ForMember(d => d.Rate, o => o.MapFrom(c => c.surchargeComponent.Rate))
            .ForMember(d => d.Description, o => o.MapFrom(c => c.component.SmallDescription))
            .ForMember(d => d.RateType, o => o.MapFrom(c => c.rateType != null ? new CompanyRateType
            {
                Description = c.rateType.Description,
                Id = c.rateType.RateTypeCode
            } : null))
            .ForMember(d => d.Id, o => o.MapFrom(c => c.component.ComponentCode));
        });
            return config;
        }
        #endregion
        #region Agency
        public static IMapper CreateMapAgency()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.PolicyAgent, IssuanceAgency>(cfg =>
            {
                cfg.CreateMap<ISSEN.PolicyAgent, IssuanceAgency>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.AgentAgencyId))
                .ForMember(d => d.Agent, o => o.MapFrom(c => new IssuanceAgent
                {
                    IndividualId = c.IndividualId
                }))
                .ForMember(d => d.IsPrincipal, o => o.MapFrom(c => c.IsPrimary))
                .ForMember(d => d.Commissions, o => o.MapFrom(c => new List<IssuanceCommission>()));
            });
            return config;
        }

        public static IMapper CreateMapCommission()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.CommissAgent, IssuanceCommission>(cfg =>
            {
                cfg.CreateMap<ISSEN.CommissAgent, IssuanceCommission>()
                .ForMember(d => d.Percentage, o => o.MapFrom(c => c.SchCommissPercentage))
                .ForMember(d => d.PercentageAdditional, o => o.MapFrom(c => c.AdditCommissPercentage.Value))
                .ForMember(d => d.SubLineBusiness, o => o.MapFrom(c => new CommonModel.SubLineBusiness
                {
                    Id = c.SubLineBusinessCode.Value,
                    LineBusiness = new CommonModel.LineBusiness
                    {
                        Id = c.LineBusinessCode.Value
                    }
                }));
            });
            return config;
        }
        #endregion

        #region PayerComponent
        public static IMapper CreateMapPayerComponent()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.PayerComp, PayerComponent>(cfg =>
            {
                cfg.CreateMap<ISSEN.PayerComp, PayerComponent>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.PayerCompId))
                .ForMember(d => d.Component, o => o.MapFrom(c => new Component
                {
                    Id = c.ComponentCode
                }))
                .ForMember(d => d.RateType, o => o.MapFrom(c => (RateType)c.RateTypeCode))
                .ForMember(d => d.BaseAmount, o => o.MapFrom(c => c.CalcBaseAmount))
                .ForMember(d => d.CoverageId, o => o.MapFrom(c => c.CoverageId ?? 0))
                .ForMember(d => d.LineBusinessId, o => o.MapFrom(c => c.LineBusinessCode ?? 0))
                .ForMember(d => d.Amount, o => o.MapFrom(c => c.ComponentAmount))
                .ForMember(d => d.TaxId, o => o.MapFrom(c => c.TaxCode))
                .ForMember(d => d.TaxConditionId, o => o.MapFrom(c => c.TaxConditionCode));
            });
            return config;
        }
        #endregion
        #region TemporalPolicy
        public static IMapper CreateMapTemporalPolicy()
        {
            IMapper config = MapperCache.GetMapper<TemporalPolicyMapper, CompanyPolicy>(cfg =>
            {
                cfg.CreateMap<TemporalPolicyMapper, CompanyPolicy>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.tempSubscription.OperationId.GetValueOrDefault()))
                .ForMember(d => d.Holder, o => o.MapFrom(c => new Holder
                {
                    IndividualId = c.tempSubscription.PolicyHolderId,
                    CustomerType = (CustomerType)c.tempSubscription.CustomerTypeCode,
                    CompanyName = new IssuanceCompanyName
                    {
                        Address = new IssuanceAddress
                        {
                            Id = c.tempSubscription.MailAddressId ?? 0
                        }
                    },
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        Id = c.tempSubscriptionPayer.PaymentMethodCode,
                        PaymentId = c.tempSubscriptionPayer.PaymentId ?? 0
                    }
                }))
                .ForMember(d => d.PaymentPlan, o => o.MapFrom(c => new CompanyPaymentPlan
                {
                    Id = c.tempSubscriptionPayer.PaymentScheduleId
                }))
                .ForMember(d => d.Prefix, o => o.MapFrom(c => new CompanyPrefix
                {
                    Id = c.tempSubscription.PrefixCode
                }))
                .ForMember(d => d.Branch, o => o.MapFrom(c => new CompanyBranch
                {
                    Id = c.tempSubscription.BranchCode,
                    SalePoints = c.tempSubscription.SalePointCode.HasValue ?
                    new List<CompanySalesPoint>
                    {
                        new CompanySalesPoint
                        {
                            Id =(int)c.tempSubscription.SalePointCode
                        }
                    } : null
                }))
                .ForMember(d => d.Endorsement, o => o.MapFrom(c => new CompanyEndorsement
                {
                    EndorsementType = (EndorsementType)c.tempSubscription.EndorsementTypeCode,
                    TemporalId = c.tempSubscription.TempId
                }))
                .ForMember(d => d.ExchangeRate, o => o.MapFrom(c => new CommonModel.ExchangeRate
                {
                    BuyAmount = c.tempSubscription.ExchangeRate,
                    Currency = new CommonModel.Currency
                    {
                        Id = c.tempSubscription.CurrencyCode
                    }
                }))
                .ForMember(d => d.IssueDate, o => o.MapFrom(c => c.tempSubscription.IssueDate))
                .ForMember(d => d.CurrentFrom, o => o.MapFrom(c => c.tempSubscription.CurrentFrom))
                .ForMember(d => d.CurrentTo, o => o.MapFrom(c => c.tempSubscription.CurrentTo.Value))
                .ForMember(d => d.BeginDate, o => o.MapFrom(c => c.tempSubscription.BeginDate))
                .ForMember(d => d.Product, o => o.MapFrom(c => new CompanyProduct
                {
                    Id = c.tempSubscription.ProductId.Value
                }))
                .ForMember(d => d.TemporalType, o => o.MapFrom(c => (TemporalType)c.tempSubscription.TemporalTypeCode))
                .ForMember(d => d.BusinessType, o => o.MapFrom(c => (BusinessType)c.tempSubscription.BusinessTypeCode))
                .ForMember(d => d.UserId, o => o.MapFrom(c => c.tempSubscription.UserId))
                .ForMember(d => d.CoInsuranceCompanies, o => o.MapFrom(c => new List<CompanyIssuanceCoInsuranceCompany>()))
                .ForMember(d => d.Text, o => o.MapFrom(c => new CompanyText
                {
                    TextBody = c.tempSubscription.ConditionText,
                    Observations = c.tempSubscription.Annotations
                }));


            });
            return config;
        }
        #endregion
        #region TemporalAgency
        public static IMapper CreateMapTemporalAgency()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempSubscriptionAgent, IssuanceAgency>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempSubscriptionAgent, IssuanceAgency>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.AgentAgencyId))
                .ForMember(d => d.Agent, o => o.MapFrom(c => new IssuanceAgent
                {
                    IndividualId = c.IndividualId
                }))
                .ForMember(d => d.IsPrincipal, o => o.MapFrom(c => c.IsPrimary));
            });
            return config;
        }

        public static IMapper CreateMapTemporalAgencyCommission()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempCommissAgent, IssuanceCommission>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempCommissAgent, IssuanceCommission>()
                .ForMember(d => d.Percentage, o => o.MapFrom(c => c.StCommissPercentage))
                .ForMember(d => d.SubLineBusiness, o => o.MapFrom(c => new CommonModel.SubLineBusiness
                {
                    Id = c.SubLineBusinessCode.Value,
                    LineBusiness = new CommonModel.LineBusiness
                    {
                        Id = c.LineBusinessCode.Value
                    }
                }));
            });
            return config;
        }
        #endregion

        #region TemporalQuota
        public static IMapper CreateMapTemporalQuota()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempPayerPayment, Quota>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempPayerPayment, Quota>()
                .ForMember(d => d.Number, o => o.MapFrom(c => c.PaymentNum))
                .ForMember(d => d.ExpirationDate, o => o.MapFrom(c => c.PayExpDate))
                .ForMember(d => d.Percentage, o => o.MapFrom(c => c.PaymentPercentage.GetValueOrDefault()));
            });
            return config;
        }
        #endregion
        #region TemporalPayerComponent
        public static IMapper CreateMapTemporalPayerComponent()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempPayerComponent, PayerComponent>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempPayerComponent, PayerComponent>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.ComponentCode))
                .ForMember(d => d.RateType, o => o.MapFrom(c => (RateType)c.RateTypeCode))
                .ForMember(d => d.BaseAmount, o => o.MapFrom(c => c.CalcBaseAmount))
                .ForMember(d => d.Amount, o => o.MapFrom(c => c.ComponentAmount))
                .ForMember(d => d.CoverageId, o => o.MapFrom(c => c.CoverageId ?? 0))
                .ForMember(d => d.LineBusinessId, o => o.MapFrom(c => c.CoverageId ?? 0))
                .ForMember(d => d.TaxId, o => o.MapFrom(c => c.TaxCode))
                .ForMember(d => d.TaxConditionId, o => o.MapFrom(c => c.TaxConditionCode));
            });
            return config;
        }

        #endregion
        #region TemporalClause
        public static IMapper CreateMapTemporalClause()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempClause, Clause>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempClause, Clause>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.ClauseId));
            });
            return config;
        }

        #endregion

        #region TemporalCoverage
        public static IMapper CreateMapTemporalCompanyCoverage()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempRiskCoverage, CompanyCoverage>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempRiskCoverage, CompanyCoverage>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.CoverageId))
                .ForMember(d => d.IsMinPremiumDeposit, o => o.MapFrom(c => c.IsMinimumPremiumDeposit))
                .ForMember(d => d.FirstRiskType, o => o.MapFrom(c => (FirstRiskType)c.FirstRiskTypeCode))
                .ForMember(d => d.CalculationType, o => o.MapFrom(c => (Core.Services.UtilitiesServices.Enums.CalculationType)c.CalculationTypeCode))
                .ForMember(d => d.ExcessLimit, o => o.MapFrom(c => c.LimitInExcess))
                .ForMember(d => d.RateType, o => o.MapFrom(c => (RateType)c.RateTypeCode))
                .ForMember(d => d.Number, o => o.MapFrom(c => c.CoverageNumber))
                .ForMember(d => d.CoverStatus, o => o.MapFrom(c => (CoverageStatusType)c.CoverageStatusCode))
                .ForMember(d => d.FlatRatePorcentage, o => o.MapFrom(c => c.FlatRatePercentage.HasValue ? c.FlatRatePercentage.Value : 0));
            });
            return config;
        }
        #endregion
        #region TemporalDeductible
        public static IMapper CreateMapTemporalDeductible()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempRiskCoverDeduct, CompanyDeductible>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempRiskCoverDeduct, CompanyDeductible>()
                .ForMember(d => d.Rate, o => o.MapFrom(c => c.Rate.GetValueOrDefault()))
                .ForMember(d => d.RateType, o => o.MapFrom(c => (RateType)c.RateTypeCode))
                .ForMember(d => d.DeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.DeductUnitCode
                }))
                .ForMember(d => d.DeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                {
                    Id = c.DeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MinDeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.MinDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MinDeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                {
                    Id = c.MinDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MaxDeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.MaxDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MaxDeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                {
                    Id = c.MaxDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(d => d.Currency, o => o.MapFrom(c => new CommonModel.Currency
                {
                    Id = c.CurrencyCode.GetValueOrDefault()
                }));
            });
            return config;
        }
        #endregion
        #region SubLineBusiness
        public static IMapper CreateMapSubLineBusiness()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.Coverage, CompanySubLineBusiness>(cfg =>
            {
                cfg.CreateMap<QUOEN.Coverage, CompanySubLineBusiness>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.SubLineBusinessCode))
                .ForMember(d => d.LineBusiness, o => o.MapFrom(c => new CompanyLineBusiness
                {
                    Id = c.LineBusinessCode
                }));
            });
            return config;
        }
        #endregion

        #region Coinsurance
        public static IMapper CreateMapCoInsurancesAssigned()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.CoinsuranceAssigned, CompanyIssuanceCoInsuranceCompany>(cfg =>
            {
                cfg.CreateMap<ISSEN.CoinsuranceAssigned, CompanyIssuanceCoInsuranceCompany>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.InsuranceCompanyId))
                .ForMember(d => d.ParticipationPercentage, o => o.MapFrom(c => c.PartCiaPercentage))
                .ForMember(d => d.EndorsementNumber, o => o.MapFrom(c => c.CompanyNum.ToString()));
            });
            return config;
        }

        public static IMapper CreateMapCoInsuranceAccepted()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.CoinsuranceAccepted, CompanyIssuanceCoInsuranceCompany>(cfg =>
            {
                cfg.CreateMap<ISSEN.CoinsuranceAccepted, CompanyIssuanceCoInsuranceCompany>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.InsuranceCompanyId))
                .ForMember(d => d.ParticipationPercentage, o => o.MapFrom(c => c.PartCiaPercentage))
                .ForMember(d => d.EndorsementNumber, o => o.MapFrom(c => c.AnnexNumMain))
                .ForMember(d => d.ParticipationPercentageOwn, o => o.MapFrom(c => c.PartMainPercentage))
                .ForMember(d => d.PolicyNumber, o => o.MapFrom(c => c.PolicyNumMain));
            });
            return config;
        }

        public static IMapper CreateMapCiaCoInsuranceAccepted()
        {
            IMapper config = MapperCache.GetMapper<CIAISS.CiaCoinsuranceAccepted, CompanyAcceptCoInsuranceAgent>(cfg =>
            {
                cfg.CreateMap<CIAISS.CiaCoinsuranceAccepted, CompanyAcceptCoInsuranceAgent>()
                .ForPath(d => d.Agent.IndividualId, o => o.MapFrom(c => c.IndividualId))
                .ForMember(d => d.ParticipationPercentage, o => o.MapFrom(c => c.PartCiaPercentage));
            });
            return config;
        }
        #endregion

        #region Clauses
        public static IMapper CreateMapClauses()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.Clause, Clause>(cfg =>
            {
                cfg.CreateMap<QUOEN.Clause, Clause>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.ClauseId))
                .ForMember(d => d.Name, o => o.MapFrom(c => c.ClauseName))
                .ForMember(d => d.Title, o => o.MapFrom(c => c.ClauseTitle.Trim()))
                .ForMember(d => d.Text, o => o.MapFrom(c => c.ClauseText.Trim()));
            });
            return config;
        }
        #endregion

        #region objeto del seguro
        public static IMapper CreateMapCompanyInsuredObject()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.InsuredObject, CompanyInsuredObject>(cfg =>
            {
                cfg.CreateMap<QUOEN.InsuredObject, CompanyInsuredObject>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.InsuredObjectId));
            });
            return config;
        }
        #endregion
        #region DetailTypes
        public static IMapper CreateMapCoverDetailType()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.CoverDetailType, CoverDetailType>(cfg =>
            {
                cfg.CreateMap<QUOEN.CoverDetailType, CoverDetailType>()
                .ForMember(d => d.Code, o => o.MapFrom(c => c.DetailTypeCode));
            });
            return config;
        }
        #endregion
        #region zona tarifacion
        public static IMapper CreateMapCiaRatingZoneBranch()
        {
            IMapper config = MapperCache.GetMapper<COMMENT.CiaRatingZoneBranch, CiaRatingZoneBranch>(cfg =>
            {
                cfg.CreateMap<COMMENT.CiaRatingZoneBranch, CiaRatingZoneBranch>()
                .ForMember(d => d.Branch, o => o.MapFrom(c => new CompanyBranch
                {
                    Id = c.BranchCode
                }))
                 .ForMember(d => d.RatingZone, o => o.MapFrom(c => new CompanyRatingZone
                 {
                     Id = c.RatingZoneCode
                 }));
            });
            return config;
        }

        public static IMapper CreateMapCompanyRatingZones()
        {
            IMapper config = MapperCache.GetMapper<COMMENT.Prefix, CompanyPrefix>(cfg =>
            {
                cfg.CreateMap<COMMENT.Prefix, CompanyPrefix>();
            });
            return config;
        }
        #endregion
        #region Personas
        #region Beneficiarios
        public static IMapper CreateMapCompanyBeneficiaryType()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.BeneficiaryType, CompanyBeneficiaryType>(cfg =>
            {
                cfg.CreateMap<QUOEN.BeneficiaryType, CompanyBeneficiaryType>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.BeneficiaryTypeCode));
            });
            return config;
        }
        #endregion
        #endregion
        #region PaymentSchedule
        public static IMapper CreateMapPaymentSchedule()
        {
            IMapper config = MapperCache.GetMapper<Core.Application.Product.Entities.PaymentSchedule, CompanyPaymentSchedule>(cfg =>
            {
                cfg.CreateMap<Core.Application.Product.Entities.PaymentSchedule, CompanyPaymentSchedule>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.PaymentScheduleId))
                 .ForMember(d => d.FirstPayerQuantity, o => o.MapFrom(c => c.FirstPayQuantity))
                 .ForMember(d => d.Quantity, o => o.MapFrom(c => c.PaymentQuantity))
                 .ForMember(d => d.CalculationType, o => o.MapFrom(c => (PaymentCalculationType)c.GapUnitCode.GetValueOrDefault(3)))
                 .ForMember(d => d.CalculationQuantity, o => o.MapFrom(c => c.GapQuantity.HasValue ? c.GapQuantity.Value : 0))
                 .ForMember(d => d.LastPayerQuantity, o => o.MapFrom(c => c.LastPayQuantity.HasValue ? c.LastPayQuantity.Value : 0));
            });
            return config;
        }
        #endregion
        #region FirstPayComponent
        public static IMapper CreateMapFirstPayComponent()
        {
            IMapper config = MapperCache.GetMapper<COISSEN.FirstPayComponent, FirstPayComponent>(cfg =>
            {
                cfg.CreateMap<COISSEN.FirstPayComponent, FirstPayComponent>()
                .ForMember(d => d.FinancialPlan, o => o.MapFrom(c => new FinancialPlan
                {
                    Id = c.FinancialPlanId
                }))
                .ForMember(d => d.Component, o => o.MapFrom(c => new Component
                {
                    Id = c.ComponentCode
                }));
            });
            return config;
        }
        #endregion
        #region Component
        public static IMapper CreateMapComponent()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.Component, Component>(cfg =>
            {
                cfg.CreateMap<QUOEN.Component, Component>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.ComponentCode))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.SmallDescription))
                .ForMember(d => d.ComponentType, o => o.MapFrom(c => (ComponentType)c.ComponentTypeCode))
                .ForMember(d => d.ComponentClass, o => o.MapFrom(c => (ComponentClassType)c.ComponentClassCode));
            });
            return config;
        }
        #endregion
        #region Deductible
        public static IMapper CreateMapCoverageDeductible()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.RiskCoverDeduct, Deductible>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskCoverDeduct, Deductible>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.DeductId.GetValueOrDefault()))
               .ForMember(d => d.Rate, o => o.MapFrom(c => c.Rate))
                .ForMember(d => d.RateType, o => o.MapFrom(c => (RateType)c.RateTypeCode))
                .ForMember(d => d.DeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.DeductUnitCode
                }))
                .ForMember(d => d.MinDeductValue, o => o.MapFrom(c => c.MinDeductValue.GetValueOrDefault()))
                .ForMember(d => d.MinDeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.MinDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MinDeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                {
                    Id = c.MinDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MaxDeductValue, o => o.MapFrom(c => c.MaxDeductValue.GetValueOrDefault()))

                .ForMember(d => d.MaxDeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.MaxDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(d => d.MaxDeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                {
                    Id = c.MaxDeductSubjectCode.GetValueOrDefault()
                }))
                  .ForMember(d => d.DeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                  {
                      Id = c.DeductSubjectCode.GetValueOrDefault()
                  }))
                .ForMember(d => d.Currency, o => o.MapFrom(c => new CommonModel.Currency
                {
                    Id = c.CurrencyCode.GetValueOrDefault()
                }));
            });
            return config;
        }

        public static IMapper CreateMapDeductible()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.Deductible, Deductible>(cfg =>
            {
                cfg.CreateMap<QUOEN.Deductible, Deductible>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.DeductId))
                .ForMember(d => d.RateType, o => o.MapFrom(c => (RateType)c.RateTypeCode))
                .ForMember(d => d.DeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.DeductUnitCode
                }))
                .ForMember(d => d.MinDeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                {
                    Id = c.MinDeductSubjectCode.GetValueOrDefault()
                }))
                 .ForMember(d => d.MinDeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                 {
                     Id = c.MinDeductSubjectCode.GetValueOrDefault()
                 }))
                   .ForMember(d => d.MaxDeductibleUnit, o => o.MapFrom(c => new DeductibleUnit
                   {
                       Id = c.MaxDeductUnitCode.GetValueOrDefault()
                   }))
                   .ForMember(d => d.MaxDeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                   {
                       Id = c.MaxDeductSubjectCode.GetValueOrDefault()
                   }))
                   .ForMember(d => d.DeductibleSubject, o => o.MapFrom(c => new DeductibleSubject
                   {
                       Id = c.DeductSubjectCode.GetValueOrDefault()
                   }))
                  .ForMember(d => d.Currency, o => o.MapFrom(c => new CommonModel.Currency
                  {
                      Id = c.CurrencyCode.GetValueOrDefault()
                  }))
                    .ForMember(d => d.LineBusiness, o => o.MapFrom(c => new CommonModel.LineBusiness
                    {
                        Id = c.LineBusinessCode
                    }));
            });
            return config;
        }
        #endregion
        #region Holder details
        public static IMapper CreateMapPhone()
        {
            IMapper config = MapperCache.GetMapper<ENTI.Phone, IssuancePhone>(cfg =>
            {
                cfg.CreateMap<ENTI.Phone, IssuancePhone>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.DataId))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.PhoneNumber.ToString()));
            });
            return config;
        }

        public static IMapper CreateMapEmail()
        {
            IMapper config = MapperCache.GetMapper<ENTI.Email, IssuanceEmail>(cfg =>
            {
                cfg.CreateMap<ENTI.Email, IssuanceEmail>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.DataId))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.Address));
            });
            return config;
        }

        public static IMapper CreateMapAddress()
        {
            IMapper config = MapperCache.GetMapper<ENTI.Address, IssuanceAddress>(cfg =>
            {
                cfg.CreateMap<ENTI.Address, IssuanceAddress>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.DataId))
                .ForMember(d => d.Description, o => o.MapFrom(c => c.Street));
            });
            return config;
        }
        #endregion
        #region PaymentPlan
        public static IMapper CreateMapPaymentPlan()
        {
            IMapper config = MapperCache.GetMapper<PROEN.PaymentSchedule, CompanyPaymentPlan>(cfg =>
            {
                cfg.CreateMap<PROEN.PaymentSchedule, CompanyPaymentPlan>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.PaymentScheduleId));
            });
            return config;
        }
        #endregion
        #region Quota
        public static IMapper CreateMapFinancialQuotaPlan()
        {
            IMapper config = MapperCache.GetMapper<Policy, QuotaFilterDTO>(cfg =>
            {
                cfg.CreateMap<Policy, QuotaFilterDTO>()
                .ForMember(d => d.PlanId, o => o.MapFrom(c => c.PaymentPlan.Id))
                .ForMember(d => d.CurrentFrom, o => o.MapFrom(c => c.CurrentFrom))
                .ForMember(d => d.IssueDate, o => o.MapFrom(c => c.IssueDate))
                .ForMember(d => d.ComponentValueDTO, o => o.MapFrom(c => c.Summary));
            });
            return config;
        }
        #endregion
        #region Componentes
        public static IMapper CreateMapCompanyComponentValueDTO()
        {
            IMapper config = MapperCache.GetMapper<CompanySummary, ComponentValueDTO>(cfg =>
            {
                cfg.CreateMap<CompanySummary, ComponentValueDTO>();
            });
            return config;
        }
        #endregion Componentes
        #endregion Automapper
    }
}
