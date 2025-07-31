using AutoMapper;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models.Distribution;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using PRODEN = Sistran.Core.Application.Product.Entities;
using ProductModel = Sistran.Core.Application.ProductServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Rules = Sistran.Core.Framework.Rules;
using TAXMO = Sistran.Core.Application.TaxServices.Models;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using UE = Sistran.Core.Services.UtilitiesServices.Enums;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using UPMO = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region  Clause
        public static IMapper CreateMapClause()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.Clause, Clause>(cfg =>
            {
                cfg.CreateMap<QUOEN.Clause, Clause>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.ClauseId))
                 .ForMember(d => d.Name, o => o.MapFrom(c => c.ClauseName))
                 .ForMember(d => d.Title, o => o.MapFrom(c => c.ClauseTitle))
                .ForMember(d => d.Text, o => o.MapFrom(c => c.ClauseText));

            });
            return config;
        }
        #endregion
        #region Text
        public static IMapper CreateMapTexts()
        {
            IMapper config = MapperCache.GetMapper<QUOEN.ConditionText, Text>(cfg =>
            {
                cfg.CreateMap<QUOEN.ConditionText, Text>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.ConditionTextId));

            });
            return config;
        }
        public static IMapper CreateMapGroupCoverageByPrefixs()
        {
            IMapper config = MapperCache.GetMapper<PRODEN.ProductGroupCover, GroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductGroupCover, GroupCoverage>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.CoverGroupId))
                 .ForMember(d => d.Description, o => o.MapFrom(c => c.SmallDescription))
                 .ForMember(d => d.CoveredRiskType, o => o.MapFrom(c => c.CoveredRiskTypeCode))

                 .ForMember(d => d.Product, o => o.MapFrom(c => new ProductModel.Product
                 {
                     Id = c.ProductId,
                     Prefix = new CommonService.Models.Prefix
                     {
                         Id = (int)c.PrefixCode
                     }
                 }));

            });
            return config;
        }

        #endregion
        #region Temporales
        public static IMapper CreateMapperTempPolicy()
        {
            IMapper config = MapperCache.GetMapper<TMPEN.TempSubscription, Policy>(cfg =>
            {
                cfg.CreateMap<TMPEN.TempSubscription, Policy>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.OperationId))
                .ForMember(d => d.Agencies, o => o.MapFrom(c => new IssuanceAgency()))
                .ForMember(d => d.Agencies, o => o.MapFrom(c => new Endorsement
                {
                    EndorsementType = (Enums.EndorsementType)c.EndorsementTypeCode
                }))
                .ForMember(d => d.Branch, o => o.MapFrom(c => new CommonService.Models.Branch
                {
                    Id = c.BranchCode,
                    SalePoints = c.SalePointCode.HasValue ? new List<CommonService.Models.SalePoint>
                    {
                        new CommonService.Models.SalePoint
                        {
                            Id = c.SalePointCode.Value
                        }
                    } : null
                }))
                .ForMember(d => d.ExchangeRate, o => o.MapFrom(c => new CommonService.Models.ExchangeRate
                {
                    Currency = new CommonService.Models.Currency
                    {
                        Id = c.CurrencyCode
                    },
                    SellAmount = c.ExchangeRate
                }))
                .ForMember(d => d.Prefix, o => o.MapFrom(c => new CommonService.Models.Prefix
                {
                    Id = c.PrefixCode
                }))
                .ForMember(d => d.Product, o => o.MapFrom(c => new ProductModel.Product
                {
                    Id = c.ProductId != null ? (int)c.ProductId : 0
                }))
                .ForMember(d => d.TemporalType, o => o.MapFrom(c => (TemporalType)c.TemporalTypeCode))
                .ForMember(d => d.Holder, o => o.MapFrom(c => new Holder
                {
                    IndividualId = c.PolicyHolderId,
                    CustomerType = (Services.UtilitiesServices.Enums.CustomerType)c.CustomerTypeCode,
                    CompanyName = new IssuanceCompanyName
                    {
                        Address = new IssuanceAddress
                        {
                            Id = c.MailAddressId ?? 0
                        }
                    }
                }));
            });
            return config;
        }
        #endregion


        #region BeneficiaryPersonsAndCompanies

        public static IMapper CreateMapBeneficiaryPerson()
        {
            var config = MapperCache.GetMapper<UPEN.Person, Beneficiary>(cfg =>
            {
                cfg.CreateMap<UPEN.Person, Beneficiary>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => MotherLastName(src)))
                 .ForMember(dest => dest.BeneficiaryType, opt => opt.MapFrom(src => new BeneficiaryType
                 {
                     Id = Utilities.Configuration.KeySettings.OnerousBeneficiaryTypeId
                 }));
            });
            return config;
        }
        static string MotherLastName(UPEN.Person person)
        {
            string beneficiary = string.Empty;
            if (person.MotherLastName != null)
            {
                beneficiary += " " + person.MotherLastName;
            }
            beneficiary += " " + person.Name;

            return beneficiary;
        }

        public static IMapper CreateMapBenefeciaryCompany()
        {
            var config = MapperCache.GetMapper<UPEN.Company, Beneficiary>(cfg =>
            {
                cfg.CreateMap<UPEN.Company, Beneficiary>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.TradeName))
                .ForMember(dest => dest.BeneficiaryType, opt => opt.MapFrom(src => new BeneficiaryType
                {
                    Id = Utilities.Configuration.KeySettings.OnerousBeneficiaryTypeId
                }));
            });
            return config;
        }
        #endregion
        #region PaymentPlant


        public static IMapper CreateMapPaymentPlan()
        {
            var config = MapperCache.GetMapper<PRODEN.PaymentSchedule, PaymentPlan>(cfg =>
            {
                cfg.CreateMap<PRODEN.PaymentSchedule, PaymentPlan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentScheduleId));
            });
            return config;
        }

        public static IMapper CreateMapFinancialPlan()
        {
            var config = MapperCache.GetMapper<PRODEN.PaymentSchedule, FinancialPlan>(cfg =>
            {
                cfg.CreateMap<PRODEN.FinancialPlan, FinancialPlan>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentScheduleId))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => new Currency
                {
                    Id = src.CurrencyCode
                }))
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => new PaymentMethod
                {
                    Id = src.PaymentMethodCode
                }))
                .ForMember(dest => dest.PaymentSchedule, opt => opt.MapFrom(src => new PaymentSchedule
                {
                    Id = src.PaymentScheduleId
                }));
            });
            return config;
        }
        public static IMapper CreateMapPaymentDistribution()
        {
            var config = MapperCache.GetMapper<PRODEN.PaymentDistribution, PaymentDistribution>(cfg =>
            {
                cfg.CreateMap<PRODEN.PaymentDistribution, PaymentDistribution>()
                .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (PaymentCalculationType?)src.GapUnitCode))
                .ForMember(dest => dest.CalculationQuantity, opt => opt.MapFrom(src => src.GapQuantity))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.PaymentPercentage.Value))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.PaymentNumber));
            });
            return config;
        }
        #endregion
        #region LimitRC
        public static IMapper CreateMapLimitRC()
        {
            var config = MapperCache.GetMapper<COMMEN.CoLimitsRc, LimitRc>(cfg =>
            {
                cfg.CreateMap<COMMEN.CoLimitsRc, LimitRc>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LimitRcCode))
                .ForMember(dest => dest.Limit1, opt => opt.MapFrom(src => src.Limit1.GetValueOrDefault()))
                .ForMember(dest => dest.Limit2, opt => opt.MapFrom(src => src.Limit2.GetValueOrDefault()))
                .ForMember(dest => dest.Limit3, opt => opt.MapFrom(src => src.Limit3.GetValueOrDefault()))
                .ForMember(dest => dest.LimitSum, opt => opt.MapFrom(src => src.Limit1.GetValueOrDefault() + src.Limit2.GetValueOrDefault() + src.Limit3.GetValueOrDefault()));
            });
            return config;
        }
        #endregion
        #region LimitRCRelation
        public static IMapper CreateMapLimitRCRelation()
        {
            var config = MapperCache.GetMapper<COMMEN.CoLimitsRcRel, LimitRCRelation>(cfg =>
            {
                cfg.CreateMap<COMMEN.CoLimitsRcRel, LimitRCRelation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LimitRcCode))
                .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new Prefix
                {
                    Id = src.PrefixCode
                }))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductModel.Product
                {
                    Id = src.ProductId
                }))
                .ForMember(dest => dest.PolicyType, opt => opt.MapFrom(src => new PolicyType
                {
                    Id = src.PolicyTypeCode
                }));

            });
            return config;
        }
        #endregion
        #region GroupCoverage
        public static IMapper CreateMapGroupCoverageByProduct()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductGroupCover, GroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductGroupCover, GroupCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverGroupId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (CoveredRiskType)src.CoveredRiskTypeCode))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductModel.Product
                {
                    Id = src.ProductId
                }));


            });
            return config;
        }

        public static IMapper CreateMapGroupCoverageByCoverage()
        {
            var config = MapperCache.GetMapper<PRODEN.GroupCoverage, GroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.GroupCoverage, GroupCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverGroupId))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductModel.Product
                {
                    Id = src.ProductId
                }))
                .ForMember(dest => dest.Coverage, opt => opt.MapFrom(src => new Coverage
                {
                    Id = src.CoverageId
                }))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.CoverNum))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => "--"))
                .ForMember(dest => dest.MainCoverageId, opt => opt.MapFrom(src => src.MainCoverageId == null ? 0 : src.MainCoverageId.Value))
                .ForMember(dest => dest.SublimitPercentage, opt => opt.MapFrom(src => src.MainCoveragePercentage == null ? 0 : src.MainCoveragePercentage.Value));


            });
            return config;
        }

        public static IMapper CreateMapInsuredObjectByGroupInsuredObject()
        {
            var config = MapperCache.GetMapper<QUOEN.InsuredObject, InsuredObject>(cfg =>
            {
                cfg.CreateMap<QUOEN.InsuredObject, InsuredObject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuredObjectId));
                cfg.CreateMap<PRODEN.GroupInsuredObject, InsuredObject>();

            });
            return config;
        }

        public static IMapper CreateMapPayerComponent()
        {
            var config = MapperCache.GetMapper<QUOEN.Component, PayerComponent>(cfg =>
            {
                cfg.CreateMap<QUOEN.Component, PayerComponent>()
                .ForMember(dest => dest.Component, opt => opt.MapFrom(src => new Component
                {
                    Id = src.ComponentCode,
                    Description = src.SmallDescription,
                    ComponentType = (Enums.ComponentType)src.ComponentTypeCode
                }));


            });
            return config;
        }

        public static IMapper CreateMapGroupCoverages()
        {
            var config = MapperCache.GetMapper<PRODEN.CoverGroupRiskType, GroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.CoverGroupRiskType, GroupCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverageGroupCode))
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (CoveredRiskType)src.CoveredRiskTypeCode));


            });
            return config;
        }


        #endregion
        #region Coverage
        public static IMapper CreateMapCoverage()
        {
            var config = MapperCache.GetMapper<QUOEN.Coverage, Coverage>(cfg =>
            {
                cfg.CreateMap<QUOEN.Coverage, Coverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PrintDescription))
                .ForMember(dest => dest.SubLineBusiness, opt => opt.MapFrom(src => new SubLineBusiness
                {
                    Id = src.SubLineBusinessCode,
                    LineBusiness = new LineBusiness
                    {
                        Id = src.LineBusinessCode
                    }
                }))
                .ForMember(dest => dest.InsuredObject, opt => opt.MapFrom(src => new InsuredObject
                {
                    Id = src.InsuredObjectId
                }))
                .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => (UE.RateType?)UE.RateType.Percentage))
                .ForMember(dest => dest.CoverStatus, opt => opt.MapFrom(src => (CoverageStatusType?)CoverageStatusType.Original))
                .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (Services.UtilitiesServices.Enums.CalculationType?)Services.UtilitiesServices.Enums.CalculationType.Prorate))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => (decimal?)Decimal.Zero))
                .ForMember(dest => dest.LimitAmount, opt => opt.MapFrom(src => Decimal.Zero))
                .ForMember(dest => dest.SubLimitAmount, opt => opt.MapFrom(src => Decimal.Zero));
            });
            return config;
        }
        public static IMapper CreateMapCoverDetailType()
        {
            var config = MapperCache.GetMapper<QUOEN.CoverDetailType, CoverDetailType>(cfg =>
            {
                cfg.CreateMap<QUOEN.CoverDetailType, CoverDetailType>()
                .ForMember(dest => dest.CoverageId, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.DetailTypeCode));
            });
            return config;
        }
        #endregion
        #region Component
        public static IMapper CreateMapComponent()
        {
            var config = MapperCache.GetMapper<QUOEN.Component, Component>(cfg =>
            {
                cfg.CreateMap<QUOEN.Component, Component>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ComponentCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.ComponentType, opt => opt.MapFrom(src => (Enums.ComponentType)src.ComponentTypeCode))
                .ForMember(dest => dest.ComponentClass, opt => opt.MapFrom(src => (Enums.ComponentClassType)src.ComponentClassCode));
            });
            return config;
        }
        #endregion
        #region CoveredRisk
        public static IMapper CreateMapCoveredRisk()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductCoverRiskType, ProductModel.CoveredRisk>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductCoverRiskType, ProductModel.CoveredRisk>()
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (CoveredRiskType)src.CoveredRiskTypeCode));
            });
            return config;
        }
        #endregion
        #region InsuredObject
        public static IMapper CreateMapInsuresObject()
        {
            var config = MapperCache.GetMapper<QUOEN.InsuredObject, InsuredObject>(cfg =>
            {
                cfg.CreateMap<QUOEN.InsuredObject, InsuredObject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuredObjectId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.IsDeclarative, opt => opt.MapFrom(src => src.IsDeclarative))
                .ForMember(dest => dest.IsDeclarative, opt => opt.MapFrom(src => src.IsDeclarative));
            });
            return config;
        }

        public static IMapper CreateMapInsuredObject()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskInsuredObject, InsuredObject>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskInsuredObject, InsuredObject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuredObjectId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.InsuredValue.HasValue ? src.InsuredValue.Value : 0));

            });
            return config;
        }

        #endregion InsuredObject
        #region InsObjLineBusiness
        public static IMapper CreateMapInsObjLineBusiness()
        {
            var config = MapperCache.GetMapper<QUOEN.InsObjLineBusiness, InsObjLineBusiness>(cfg =>
            {
                cfg.CreateMap<QUOEN.InsObjLineBusiness, InsObjLineBusiness>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuredObjectId))
                .ForMember(dest => dest.LineBusinessCd, opt => opt.MapFrom(src => src.LineBusinessCode));

            });
            return config;
        }
        #endregion
        #region Deductible
        public static IMapper CreateMapCoverageDeductible()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskCoverDeduct, Deductible>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskCoverDeduct, Deductible>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeductId == null || src.DeductId == 0 ? -1 : src.DeductId.GetValueOrDefault()))
                .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => (RateType)src.RateTypeCode))
                .ForMember(dest => dest.DeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.DeductUnitCode
                }))
                .ForMember(dest => dest.MinDeductValue, opt => opt.MapFrom(src => src.MinDeductValue.GetValueOrDefault()))
                .ForMember(dest => dest.MinDeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.MinDeductUnitCode.GetValueOrDefault()
                }))
               .ForMember(dest => dest.MinDeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
               {
                   Id = src.MinDeductSubjectCode.GetValueOrDefault()
               }))
                .ForMember(dest => dest.MaxDeductValue, opt => opt.MapFrom(src => src.MaxDeductValue.GetValueOrDefault()))
                .ForMember(dest => dest.MaxDeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.MaxDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MaxDeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.MaxDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.DeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.DeductSubjectCode.GetValueOrDefault()
                }))
                 .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => new Currency
                 {
                     Id = src.CurrencyCode.GetValueOrDefault()
                 }));
            });
            return config;
        }

        public static IMapper CreateMapDeductible()
        {
            var config = MapperCache.GetMapper<QUOEN.Deductible, Deductible>(cfg =>
            {
                cfg.CreateMap<QUOEN.Deductible, Deductible>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeductId))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => (RateType)src.RateTypeCode))
                .ForMember(dest => dest.DeductibleUnit, opt => opt.MapFrom(src => src.DeductUnitCode == 0 ? null : new DeductibleUnit
                {
                    Id = src.DeductUnitCode
                }))
                .ForMember(dest => dest.MinDeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.MinDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MaxDeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.MaxDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MaxDeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.MaxDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.DeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.DeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MinDeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.MinDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => Currency(src)))
                .ForMember(dest => dest.LineBusiness, opt => opt.MapFrom(src => new LineBusiness
                {
                    Id = src.LineBusinessCode
                }))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description ?? ""))
                .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => (RateType)src.RateTypeCode));
            });
            return config;
        }


        public static IMapper CreateMapDeductibleByRiskCoverDeduct()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskCoverDeduct, Deductible>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskCoverDeduct, Deductible>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DeductId))
                .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => (RateType)src.RateTypeCode))
                .ForMember(dest => dest.DeductibleUnit, opt => opt.MapFrom(src => src.DeductUnitCode == 0 ? null : new DeductibleUnit
                {
                    Id = src.DeductUnitCode
                }))
                .ForMember(dest => dest.MinDeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.MinDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MaxDeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.MaxDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MaxDeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.MaxDeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.DeductibleSubject, opt => opt.MapFrom(src => new DeductibleSubject
                {
                    Id = src.DeductSubjectCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.MinDeductibleUnit, opt => opt.MapFrom(src => new DeductibleUnit
                {
                    Id = src.MinDeductUnitCode.GetValueOrDefault()
                }))
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => new Currency { Id = src.CurrencyCode.GetValueOrDefault() }))
                .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => (RateType)src.RateTypeCode));
            });
            return config;
        }


        static Currency Currency(QUOEN.Deductible deductible)
        {
            Currency currency = null;
            if (deductible.CurrencyCode != null)
            {
                currency = new Currency
                {
                    Id = deductible.CurrencyCode.GetValueOrDefault()
                };
            }


            return currency;
        }
        #endregion

        #region BillingGroup
        public static IMapper CreateMapBillingGroup()
        {
            var config = MapperCache.GetMapper<ISSEN.BillingGroup, BillingGroup>(cfg =>
            {
                cfg.CreateMap<ISSEN.BillingGroup, BillingGroup>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BillingGroupCode));

            });
            return config;
        }
        #endregion
        #region TemporalQuota
        public static IMapper CreateMapTemporalQuota()
        {
            var config = MapperCache.GetMapper<ISSEN.PayerPayment, Quota>(cfg =>
            {
                cfg.CreateMap<ISSEN.PayerPayment, Quota>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.PaymentNum))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.PayExpDate))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.PaymentPercentage.Value));
            });
            return config;
        }
        #endregion
        #region GroupInsuredObjects
        public static IMapper CreateMapGroupInsuredObject()
        {
            var config = MapperCache.GetMapper<PRODEN.GroupInsuredObject, InsuredObject>(cfg =>
            {
                cfg.CreateMap<PRODEN.GroupInsuredObject, InsuredObject>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuredObject));

            });
            return config;
        }
        #endregion
        #region RiskCommercialClass
        public static IMapper CreateMapRiskCommercialClass()
        {
            var config = MapperCache.GetMapper<PARAMEN.RiskCommercialClass, RiskCommercialClass>(cfg =>
            {
                cfg.CreateMap<PARAMEN.RiskCommercialClass, RiskCommercialClass>();

            });
            return config;
        }
        #endregion
        #region PolicyCoverage
        public static IMapper CreateMapPolicyCoverages()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskCoverage, Coverage>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskCoverage, Coverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverageId))
                .ForMember(dest => dest.RiskCoverageId, opt => opt.MapFrom(src => src.RiskCoverId))
                .ForMember(dest => dest.AccumulatedPremiumAmount, opt => opt.MapFrom(src => src.PremiumAmount))
                .ForMember(dest => dest.MaxLiabilityAmount, opt => opt.MapFrom(src => src.MaxLiabilityAmount == null ? 0 : src.MaxLiabilityAmount.Value))
                .ForMember(dest => dest.ExcessLimit, opt => opt.MapFrom(src => src.LimitInExcess))
                .ForMember(dest => dest.LimitOccurrenceAmount, opt => opt.MapFrom(src => src.LimitAmount))
                .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => (RateType)src.RateTypeCode))
                .ForMember(dest => dest.OriginalLimitAmount, opt => opt.MapFrom(src => src.LimitAmount))
                .ForMember(dest => dest.OriginalSubLimitAmount, opt => opt.MapFrom(src => src.SublimitAmount))
                .ForMember(dest => dest.ContractAmountPercentage, opt => opt.MapFrom(src => src.ContractAmountPercentage.HasValue ? src.ContractAmountPercentage.Value : 0))
                .ForMember(dest => dest.CurrentFrom, opt => opt.MapFrom(src => (DateTime)src.CurrentFrom))
                .ForMember(dest => dest.CalculationType, opt => opt.MapFrom(src => (Services.UtilitiesServices.Enums.CalculationType)src.CalculationTypeCode))
                .ForMember(dest => dest.EndorsementLimitAmount, opt => opt.MapFrom(src => src.EndorsementLimitAmount == null ? 0 : (decimal)src.EndorsementLimitAmount))
                .ForMember(dest => dest.EndorsementSublimitAmount, opt => opt.MapFrom(src => src.EndorsementSublimitAmount == null ? 0 : (decimal)src.EndorsementSublimitAmount))
                .ForMember(dest => dest.FlatRatePorcentage, opt => opt.MapFrom(src => src.FlatRatePercentage.HasValue ? src.FlatRatePercentage.Value : 0))
                .ForMember(dest => dest.DynamicProperties, opt => opt.MapFrom(src => new List<DynamicConcept>()));

            });
            return config;


        }

        //static DynamicProperty DynamicProperty(ISSEN.RiskCoverage riskCoverage)
        //{
        //    Currency currency = null;
        //    if (deductible.CurrencyCode != null)
        //    {
        //        currency = new Currency
        //        {
        //            Id = deductible.CurrencyCode.GetValueOrDefault()
        //        };
        //    }


        //    return currency;
        //}
        #endregion
        #region TechnicalPlan
        public static IMapper CreateMapTechnicalPlan()
        {
            var config = MapperCache.GetMapper<PRODEN.TechnicalPlan, TechnicalPlan>(cfg =>
            {
                cfg.CreateMap<PRODEN.TechnicalPlan, TechnicalPlan>();

            });
            return config;
        }

        public static IMapper CreateMapTechnicalPlanCoverage()
        {
            var config = MapperCache.GetMapper<PRODEN.TechnicalPlanCoverage, TechnicalPlanCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.TechnicalPlanCoverage, TechnicalPlanCoverage>();

            });
            return config;
        }
        #endregion
        #region PolicyType
        public static IMapper CreateMapPolicyType()
        {
            var config = MapperCache.GetMapper<COMMEN.CoPolicyType, PolicyType>(cfg =>
            {
                cfg.CreateMap<COMMEN.CoPolicyType, PolicyType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PolicyTypeCode))
                .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new Prefix
                {
                    Id = src.PrefixCode
                }));

            });
            return config;
        }
        #endregion
        #region poliza producto
        public static IMapper CreateMapProductCoverageGroupRisk()
        {
            var config = MapperCache.GetMapper<PRODEN.ProductGroupCover, GroupCoverage>(cfg =>
            {
                cfg.CreateMap<PRODEN.ProductGroupCover, GroupCoverage>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoverGroupId))
                .ForMember(dest => dest.CoveredRiskType, opt => opt.MapFrom(src => (CoveredRiskType)src.CoveredRiskTypeCode));

            });
            return config;
        }
        #endregion
        #region Conceptos Dinamicos
        public static IMapper CreateMapDynamicConcept()
        {
            var config = MapperCache.GetMapper<Rules.Concept, DynamicConcept>(cfg =>
            {
                cfg.CreateMap<Rules.Concept, DynamicConcept>();
            });
            return config;
        }
        #endregion
        #region riskLocation
        public static IMapper CreateMapRiskLocation()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskLocation, RiskLocation>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskLocation, RiskLocation>()
                .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => new Risk
                {
                    Id = src.RiskId
                }))
                .ForMember(dest => dest.ConstructionCategoryId, opt => opt.MapFrom(src => src.ConstructionCategoryCode))
                .ForMember(dest => dest.RiskDangerousId, opt => opt.MapFrom(src => src.RiskDangerousnessCode))
                .ForMember(dest => dest.EmlPtc, opt => opt.MapFrom(src => Convert.ToDecimal(src.EmlPercentage)))
                .ForMember(dest => dest.AddressType, opt => opt.MapFrom(src => src.AddressTypeCode))
                .ForMember(dest => dest.StreetType, opt => opt.MapFrom(src => src.StreetTypeCode))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => new Country
                {
                    Id = src.CountryCode
                }))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => new State
                {
                    Id = src.StateCode
                }))
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => new City
                 {
                     Id = (int)src.CityCode
                 }))
                 .ForMember(dest => dest.CrestaZoneId, opt => opt.MapFrom(src => Convert.ToInt32(src.CrestaZoneCode)))
                 .ForMember(dest => dest.EconomicActivity, opt => opt.MapFrom(src => Convert.ToInt32(src.EconomicActivityCode)))
                 .ForMember(dest => dest.HoustingTypeId, opt => opt.MapFrom(src => Convert.ToInt32(src.HouseNumber)))
                 .ForMember(dest => dest.OccupationType, opt => opt.MapFrom(src => Convert.ToInt32(src.OccupationTypeCode)))
                 .ForMember(dest => dest.CommRiskClass, opt => opt.MapFrom(src => Convert.ToInt32(src.CommRiskClassCode)))
                 .ForMember(dest => dest.RiskCommercialtype, opt => opt.MapFrom(src => Convert.ToInt32(src.RiskCommercialTypeCode)))
                 .ForMember(dest => dest.RiskCommSubtypeId, opt => opt.MapFrom(src => Convert.ToInt32(src.RiskCommSubtypeCode)))
                 .ForMember(dest => dest.AditionalStreet, opt => opt.MapFrom(src => src.AdditionalStreet))
                 .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => Convert.ToInt32(src.LocationCode)))
                 .ForMember(dest => dest.RiskType, opt => opt.MapFrom(src => new RiskType
                 {
                     Id = Convert.ToInt32(src.RiskTypeCode)
                 }))
                 .ForMember(dest => dest.RiskAge, opt => opt.MapFrom(src => Convert.ToInt32(src.RiskAge)))
                 .ForMember(dest => dest.DeclarativePeriodId, opt => opt.MapFrom(src => Convert.ToInt32(src.DeclarativePeriodCode)))
                 .ForMember(dest => dest.IsRetention, opt => opt.MapFrom(src => Convert.ToBoolean(src.IsRetention)))
                 .ForMember(dest => dest.InspectionRecomendation, opt => opt.MapFrom(src => Convert.ToBoolean(src.InspectionRecomendation)))
                 .ForMember(dest => dest.PremiumAdjustmentPeriodId, opt => opt.MapFrom(src => Convert.ToInt32(src.PremiumAdjustmentPeriodCode)))
                 .ForMember(dest => dest.InsuranceModeId, opt => opt.MapFrom(src => Convert.ToInt32(src.InsuranceModeCode)))
                 .ForMember(dest => dest.LongitudeEarthquake, opt => opt.MapFrom(src => Convert.ToInt32(src.LongitudeEarthquake)))
                 .ForMember(dest => dest.LatitudEarthquake, opt => opt.MapFrom(src => Convert.ToInt32(src.LatitudeEarthquake)))
                 .ForMember(dest => dest.ConstructionYearEarthquake, opt => opt.MapFrom(src => Convert.ToInt32(src.ConstructionYearEarthquake)))
                 .ForMember(dest => dest.FloorNumberEarthquake, opt => opt.MapFrom(src => Convert.ToInt32(src.FloorNumberEarthquake)));

            });
            return config;
        }
        #endregion
        #region riskVehicle
        public static IMapper CreateMapRiskVehicle()
        {
            var config = MapperCache.GetMapper<ISSEN.RiskVehicle, RiskVehicle>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskVehicle, RiskVehicle>()
                .ForMember(dest => dest.VehicleYear, opt => opt.MapFrom(src => Convert.ToInt32(src.VehicleYear)))
                .ForMember(dest => dest.VehiclePrice, opt => opt.MapFrom(src => Convert.ToDecimal(src.VehiclePrice)))
                .ForMember(dest => dest.IsNew, opt => opt.MapFrom(src => Convert.ToBoolean(src.VehiclePrice)))
                .ForMember(dest => dest.EngineNumber, opt => opt.MapFrom(src => src.EngineSerNo))
                .ForMember(dest => dest.ChassisNumber, opt => opt.MapFrom(src => src.ChassisSerNo))
                .ForMember(dest => dest.LoadTypeId, opt => opt.MapFrom(src => src.LoadTypeCode))
                .ForMember(dest => dest.TrailersQuantity, opt => opt.MapFrom(src => Convert.ToInt32(src.TrailersQuantity)))
                .ForMember(dest => dest.PassengersQuantity, opt => opt.MapFrom(src => Convert.ToInt32(src.PassengerQuantity)))
                .ForMember(dest => dest.NewVehiclePrice, opt => opt.MapFrom(src => Convert.ToDecimal(src.NewVehiclePrice)))
                .ForMember(dest => dest.StandardVehiclePrice, opt => opt.MapFrom(src => Convert.ToDecimal(src.StdVehiclePrice)))
                .ForMember(dest => dest.VehicleVersion, opt => opt.MapFrom(src => new VehicleVersion
                {
                    Id = Convert.ToInt32(src.VehicleVersionCode)
                }))
                .ForMember(dest => dest.VehicleModel, opt => opt.MapFrom(src => new VehicleModel
                {
                    Id = Convert.ToInt32(src.VehicleModelCode)
                }))
                .ForMember(dest => dest.VehicleMake, opt => opt.MapFrom(src => new VehicleMake
                {
                    Id = Convert.ToInt32(src.VehicleModelCode)
                }))
                .ForMember(dest => dest.Risk, opt => opt.MapFrom(src => new Risk
                {
                    Id = Convert.ToInt32(src.RiskId),
                    MainInsured = new IssuanceInsured
                    {
                    }
                }))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => new VehicleType
                {
                    Id = Convert.ToInt32(src.VehicleTypeCode)
                }))
                .ForMember(dest => dest.VehicleUse, opt => opt.MapFrom(src => new VehicleUse
                {
                    Id = Convert.ToInt32(src.VehicleUseCode)
                }))
                 .ForMember(dest => dest.VehicleBody, opt => opt.MapFrom(src => new VehicleBody
                 {
                     Id = Convert.ToInt32(src.VehicleBodyCode)
                 }))
                 .ForMember(dest => dest.VehicleColor, opt => opt.MapFrom(src => new VehicleColor
                 {
                     Id = Convert.ToInt32(src.VehicleColorCode)
                 }))
                 .ForMember(dest => dest.VehicleFuel, opt => opt.MapFrom(src => new VehicleFuel
                 {
                     Id = Convert.ToInt32(src.VehicleFuelCode)
                 }));



            });
            return config;
        }
        #endregion
        #region RiskActivity
        public static IMapper CreateMapRiskActivity()
        {
            var config = MapperCache.GetMapper<PARAMEN.RiskCommercialClass, RiskActivity>(cfg =>
            {
                cfg.CreateMap<PARAMEN.RiskCommercialClass, RiskActivity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RiskCommercialClassCode));
            });
            return config;
        }

        public static IMapper CreateMapRiskActivityType()
        {
            var config = MapperCache.GetMapper<PARAMEN.RiskCommercialType, RiskActivity>(cfg =>
            {
                cfg.CreateMap<PARAMEN.RiskCommercialType, RiskActivity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RiskCommercialClassCode))
                 .ForMember(dest => dest.RiskActivityTypeId, opt => opt.MapFrom(src => src.RiskCommercialTypeCode));
            });
            return config;
        }
        #endregion
        #region SuretyContractCategories
        public static IMapper CreateMapSuretyContractCategory()
        {
            var config = MapperCache.GetMapper<COMMEN.SuretyContractCategories, SuretyContractCategories>(cfg =>
            {
                cfg.CreateMap<COMMEN.SuretyContractCategories, SuretyContractCategories>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SuretyContractCategoriesCode));
            });
            return config;
        }
        #endregion
        #region SuretyContractType
        public static IMapper CreateMapSuretyContractType()
        {
            var config = MapperCache.GetMapper<COMMEN.SuretyContractType, SuretyContractType>(cfg =>
            {
                cfg.CreateMap<COMMEN.SuretyContractType, SuretyContractType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SuretyContractTypeCode));
            });
            return config;
        }
        #endregion
        #region HouseType
        public static IMapper CreateMapHouseType()
        {
            var config = MapperCache.GetMapper<COMMEN.HouseType, UPMO.HouseType>(cfg =>
            {
                cfg.CreateMap<COMMEN.HouseType, UPMO.HouseType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.HouseTypeCode));
            });
            return config;
        }
        #endregion
        #region PrefixEndoTypeEnabled
        public static IMapper CreateMapPrefixEndoTypeEnabled()
        {
            var config = MapperCache.GetMapper<PARAMEN.PrefixEndoTypeEnabled, PrefixEndoTypeEnabled>(cfg =>
            {
                cfg.CreateMap<PARAMEN.PrefixEndoTypeEnabled, PrefixEndoTypeEnabled>()
                .ForMember(dest => dest.EndorsementCode, opt => opt.MapFrom(src => src.EndoTypeCode));
            });
            return config;
        }
        #endregion
        #region RiskType
        public static IMapper CreateMapRiskType()
        {
            var config = MapperCache.GetMapper<PARAMEN.CoveredRiskType, RiskType>(cfg =>
            {
                cfg.CreateMap<PARAMEN.CoveredRiskType, RiskType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CoveredRiskTypeCode))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.SmallDescription));
            });
            return config;
        }
        #endregion
        #region RatingZone
        public static IMapper CreateMapRatingZone()
        {
            var config = MapperCache.GetMapper<COMMEN.RatingZone, RatingZone>(cfg =>
            {
                cfg.CreateMap<COMMEN.RatingZone, RatingZone>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RatingZoneCode))
                .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new Prefix
                {
                    Id = src.PrefixCode
                }));
            });
            return config;
        }
        #endregion
        #region Issuance -- Falta CreateAgency

        public static IMapper CreateMapAgencyIssuance()
        {
            var config = MapperCache.GetMapper<AgentAgency, IssuanceAgency>(cfg =>
            {
                cfg.CreateMap<AgentAgency, IssuanceAgency>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AgentAgencyId))
                 .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AgentCode));
            });
            return config;
        }

        public static IMapper CreateMapPhone()
        {
            var config = MapperCache.GetMapper<Phone, IssuancePhone>(cfg =>
            {
                cfg.CreateMap<Phone, IssuancePhone>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DataId))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.PhoneNumber.ToString()));
            });
            return config;
        }

        public static IMapper CreateMapEmail()
        {
            var config = MapperCache.GetMapper<Email, IssuanceEmail>(cfg =>
            {
                cfg.CreateMap<Email, IssuanceEmail>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DataId))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Address));
            });
            return config;
        }

        public static IMapper CreateMapAddress()
        {
            var config = MapperCache.GetMapper<Address, IssuanceAddress>(cfg =>
            {
                cfg.CreateMap<Address, IssuanceAddress>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.DataId))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Street))
                 .ForMember(dest => dest.City, opt => opt.MapFrom(src => new City
                 {
                     Id = src.CityCode.HasValue ? src.CityCode.Value : 1,
                     State = new State
                     {
                         Id = src.StateCode.HasValue ? src.StateCode.Value : 1,
                         Country = new Country
                         {
                             Id = src.CountryCode.HasValue ? src.CountryCode.Value : 1
                         }
                     }
                 }));
            });
            return config;
        }

        public static IMapper CreateMapPaymentMethod()
        {
            var config = MapperCache.GetMapper<IndividualPaymentMethod, IssuancePaymentMethod>(cfg =>
            {
                cfg.CreateMap<IndividualPaymentMethod, IssuancePaymentMethod>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentMethodCode));
            });
            return config;
        }

        public static IMapper CreateMapCoInsuranceCompany()
        {
            var config = MapperCache.GetMapper<COMMEN.CoInsuranceCompany, IssuanceCoInsuranceCompany>(cfg =>
            {
                cfg.CreateMap<COMMEN.CoInsuranceCompany, IssuanceCoInsuranceCompany>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuranceCompanyId));
            });
            return config;
        }

        public static IMapper CreateMapCoinsuranceAssigned()
        {
            var config = MapperCache.GetMapper<ISSEN.CoinsuranceAssigned, CoInsuranceAssigned>(cfg =>
            {
                cfg.CreateMap<ISSEN.CoinsuranceAssigned, CoInsuranceAssigned>()
                .ForMember(dest => dest.InsuranceCompanyId, opt => opt.MapFrom(src => Convert.ToInt32(src.InsuranceCompanyId)));
            });
            return config;
        }
        #endregion
        #region VehicleType
        public static IMapper CreateMapVehicleType()
        {
            var config = MapperCache.GetMapper<COMMEN.VehicleType, VehicleType>(cfg =>
            {
                cfg.CreateMap<COMMEN.VehicleType, VehicleType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VehicleTypeCode))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.Enabled))
                .ForMember(dest => dest.ExtendedProperties, opt => opt.MapFrom(src => CreateExtendedProperties(src.ExtendedProperties)));
            });
            return config;
        }
        private static List<Extensions.ExtendedProperty> CreateExtendedProperties(List<ExtendedProperty> extendedProperties)
        {
            List<Extensions.ExtendedProperty> modelExtendedProperties = new List<Extensions.ExtendedProperty>();

            if (extendedProperties != null)
            {
                foreach (ExtendedProperty extendedProperty in extendedProperties)
                {
                    modelExtendedProperties.Add(new Extensions.ExtendedProperty
                    {
                        Name = extendedProperty.Name,
                        Value = extendedProperty.Value
                    });
                }
            }

            return modelExtendedProperties;
        }
        #endregion
        #region VehicleBody
        public static IMapper CreateMapVehicleBody()
        {
            var config = MapperCache.GetMapper<COMMEN.VehicleBody, VehicleBody>(cfg =>
            {
                cfg.CreateMap<COMMEN.VehicleBody, VehicleBody>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VehicleBodyCode));
            });
            return config;
        }
        #endregion
        #region VehicleUse
        public static IMapper CreateMapVehicleUse()
        {
            var config = MapperCache.GetMapper<COMMEN.VehicleUse, VehicleUse>(cfg =>
            {
                cfg.CreateMap<COMMEN.VehicleUse, VehicleUse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.VehicleUseCode))
                .ForMember(dest => dest.PrefixId, opt => opt.MapFrom(src => src.PrefixTypeCode));
            });
            return config;
        }
        #endregion
        #region SubLineBusiness
        public static IMapper CreateMapSubLineBusiness()
        {
            var config = MapperCache.GetMapper<COMMEN.SubLineBusiness, SubLineBusiness>(cfg =>
            {
                cfg.CreateMap<COMMEN.SubLineBusiness, SubLineBusiness>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LineBusinessCode))
                .ForMember(dest => dest.LineBusiness, opt => opt.MapFrom(src => new LineBusiness
                {
                    Id = src.LineBusinessCode
                }))
                .ForMember(dest => dest.LineBusinessId, opt => opt.MapFrom(src => src.LineBusinessCode));
            });
            return config;
        }
        #endregion
        #region CoCoverage --por resolver

        #endregion
        #region Tax
        #region Tax Methods
        public static IMapper CreateMapParamTax()
        {
            var config = MapperCache.GetMapper<Tax.Entities.Tax, ParamTax>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.Tax, ParamTax>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaxCode))
                .ForMember(dest => dest.TinyDescription, opt => opt.MapFrom(src => src.SmallDescription))
                .ForMember(dest => dest.RateType, opt => opt.MapFrom(src => new RateTypeTax
                {
                    Id = src.RateTypeCode
                }))
                .ForMember(dest => dest.RetentionTax, opt => opt.MapFrom(src => new RetentionTax
                {
                    Id = (src.RetentionTaxCode > 0 || src.RetentionTaxCode != null) ? src.RetentionTaxCode.Value : 0
                }))
                .ForMember(dest => dest.BaseConditionTax, opt => opt.MapFrom(src => new BaseConditionTax
                {
                    Id = (src.BaseConditionTaxCode > 0 || src.BaseConditionTaxCode != null) ? src.BaseConditionTaxCode.Value : 0
                }))
                .ForMember(dest => dest.AdditionalRateType, opt => opt.MapFrom(src => new AdditionalRateType
                {
                    Id = (src.AdditionalRateTypeCode > 0 || src.AdditionalRateTypeCode != null) ? src.AdditionalRateTypeCode.Value : 0
                }));


            });
            return config;
        }
        #endregion
        #region TaxRole Methods
        public static IMapper CreateMapParamTaxRole()
        {
            var config = MapperCache.GetMapper<Tax.Entities.TaxRole, TaxRole>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.TaxRole, TaxRole>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleCode));
            });
            return config;
        }
        #endregion
        #region TaxAttribute Methods
        public static IMapper CreateMapParamTaxAttribute()
        {
            var config = MapperCache.GetMapper<Tax.Entities.TaxAttribute, TaxAttribute>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.TaxAttribute, TaxAttribute>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaxAttributeTypeCode));
            });
            return config;
        }
        #endregion
        #region TaxRate Methods
        public static IMapper CreateMapParamTaxRate()
        {
            var config = MapperCache.GetMapper<Tax.Entities.TaxRate, ParamTax>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.TaxRate, ParamTaxRate>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaxRateId))
                .ForMember(dest => dest.IdTax, opt => opt.MapFrom(src => src.TaxCode))
                .ForMember(dest => dest.TaxCondition, opt => opt.MapFrom(src => new TAXMO.TaxCondition
                {
                    Id = (src.TaxConditionCode > 0 || src.TaxConditionCode != null) ? src.TaxConditionCode.Value : 0
                }))
                .ForMember(dest => dest.TaxCategory, opt => opt.MapFrom(src => new TAXMO.TaxCategory
                {
                    Id = (src.TaxCategoryCode > 0 || src.TaxCategoryCode != null) ? src.TaxCategoryCode.Value : 0
                }))
                .ForMember(dest => dest.LineBusiness, opt => opt.MapFrom(src => new LineBusiness
                {
                    Id = (src.LineBusinessCode > 0 || src.LineBusinessCode != null) ? src.LineBusinessCode.Value : 0
                }))
                .ForMember(dest => dest.TaxState, opt => opt.MapFrom(src => new TAXMO.TaxState
                {
                    IdState = (src.StateCode > 0 || src.StateCode != null) ? src.StateCode.Value : 0,
                    IdCountry = (src.CountryCode > 0 || src.CountryCode != null) ? src.CountryCode.Value : 0,
                    IdCity = (src.CityCode > 0 || src.CityCode != null) ? src.CityCode.Value : 0
                }))
                .ForMember(dest => dest.EconomicActivity, opt => opt.MapFrom(src => new EconomicActivity
                {
                    Id = (src.EconomicActivityTaxCode > 0 || src.EconomicActivityTaxCode != null) ? src.EconomicActivityTaxCode.Value : 0
                }))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new Branch
                {
                    Id = (src.BranchCode > 0 || src.BranchCode != null) ? src.BranchCode.Value : 0
                }))
                 .ForMember(dest => dest.Coverage, opt => opt.MapFrom(src => new Coverage
                 {
                     Id = (src.CoverageId > 0 || src.CoverageId != null) ? src.CoverageId.Value : 0
                 }))
                  .ForMember(dest => dest.TaxPeriodRate, opt => opt.MapFrom(src => new TAXMO.TaxPeriodRate
                  {

                  }))
                ;

            });
            return config;
        }
        #endregion
        #region TaxPeriodRate Methods
        public static IMapper CreateMapParamTaxPeriodRate()
        {
            var config = MapperCache.GetMapper<Tax.Entities.TaxPeriodRate, TAXMO.TaxPeriodRate>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.TaxPeriodRate, TAXMO.TaxPeriodRate>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaxRateId))
                .ForMember(dest => dest.BaseTaxAdditional, opt => opt.MapFrom(src => src.BaseTaxIncInAdditional))
                .ForMember(dest => dest.MinBaseAMT, opt => opt.MapFrom(src => src.MinBaseAmount))
                .ForMember(dest => dest.MinAdditionalBaseAMT, opt => opt.MapFrom(src => src.MinAdditionalBaseAmount))
                .ForMember(dest => dest.MinTaxAMT, opt => opt.MapFrom(src => src.MinTaxAmount))
                .ForMember(dest => dest.MinAdditionalTaxAMT, opt => opt.MapFrom(src => src.MinAdditionalTaxAmount));
            });
            return config;
        }
        #endregion
        #region TaxCategory Methods
        public static IMapper CreateMapParamTaxCategory()
        {
            var config = MapperCache.GetMapper<Tax.Entities.TaxCategory, ParamTaxCategory>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.TaxCategory, ParamTaxCategory>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaxCategoryCode))
                .ForMember(dest => dest.IdTax, opt => opt.MapFrom(src => src.TaxCode));
            });
            return config;
        }
        #endregion
        #region TaxCondition Methods
        public static IMapper CreateMapParamTaxCondition()
        {
            var config = MapperCache.GetMapper<Tax.Entities.TaxCondition, ParamTaxCondition>(cfg =>
            {
                cfg.CreateMap<Tax.Entities.TaxCondition, ParamTaxCondition>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TaxConditionCode))
                .ForMember(dest => dest.IdTax, opt => opt.MapFrom(src => src.TaxCode));
            });
            return config;
        }
        #endregion
        #endregion
        #region  generic excel export
        public static IMapper CreateMapFile()
        {
            var config = MapperCache.GetMapper<COMMEN.File, File>(cfg =>
            {
                cfg.CreateMap<COMMEN.File, File>().ForMember(dest => dest.FileType, opt => opt.MapFrom(src => (UE.FileType)src.FileTypeId));
            });
            return config;
        }
        #endregion
        #region Agency
        public static IMapper CreateMapAgency()
        {
            var config = MapperCache.GetMapper<AgentAgency, IssuanceAgency>(cfg =>
            {
                cfg.CreateMap<AgentAgency, IssuanceAgency>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AgentAgencyId))
                .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AgentCode))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new Branch
                {
                    Id = src.BranchCode
                }))
                .ForMember(dest => dest.Agent, opt => opt.MapFrom(src => new IssuanceAgent
                {
                    IndividualId = src.IndividualId
                }))
                .ForMember(dest => dest.AgentType, opt => opt.MapFrom(src => new IssuanceAgentType
                {
                    Id = src.AgentTypeCode
                }));
            });
            return config;
        }
        #endregion
        #region IdentityCardType
        public static IMapper CreateMapIdentityCardTyp()
        {
            IMapper config = MapperCache.GetMapper<IdentityCardType, IssuanceDocumentType>(cfg =>
            {
                cfg.CreateMap<IdentityCardType, IssuanceDocumentType>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.IdCardTypeCode));

            });
            return config;
        }
        #endregion
        #region DocumenType
        public static IMapper CreateMapDocumentType()
        {
            var config = MapperCache.GetMapper<DocumentType, IssuanceDocumentType>(cfg =>
            {
                cfg.CreateMap<DocumentType, IssuanceDocumentType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdDocumentType));
            });
            return config;
        }
        #endregion
        #region TributaryIdentityType
        public static IMapper CreateMapTributaryIdentityType()
        {
            var config = MapperCache.GetMapper<TributaryIdentityType, IssuanceDocumentType>(cfg =>
            {
                cfg.CreateMap<TributaryIdentityType, IssuanceDocumentType>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TributaryIdTypeCode));
            });
            return config;
        }
        #endregion
        #region Claim
        public static IMapper CreateMapClaimPolicy()
        {
            var config = MapperCache.GetMapper<ISSEN.Policy, Policy>(cfg =>
            {
                cfg.CreateMap<ISSEN.Policy, Policy>()
                .ForMember(dest => dest.CurrentTo, opt => opt.MapFrom(src => src.CurrentTo.Value))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PolicyId))
                .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new Prefix
                {
                    Id = src.PrefixCode
                }))
                .ForMember(dest => dest.Endorsement, opt => opt.MapFrom(src => new Endorsement
                {
                    PolicyId = src.PolicyId
                }))
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => new ProductModel.Product
                {
                    Id = src.ProductId.Value
                }))
                 .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new Branch
                 {
                     Id = src.BranchCode,
                     SalePoints = src.SalePointCode.HasValue ? new List<SalePoint>
                     {
                         new SalePoint
                         {
                             Id = (int)src.SalePointCode
                         }
                     } : new List<SalePoint>()
                 }))
                 .ForMember(dest => dest.BusinessType, opt => opt.MapFrom(src => (BusinessType)src.BusinessTypeCode))
                  .ForMember(dest => dest.Holder, opt => opt.MapFrom(src => new Holder
                  {
                      IndividualId = src.PolicyholderId.Value
                  }))
                   .ForMember(dest => dest.ExchangeRate, opt => opt.MapFrom(src => new ExchangeRate
                   {
                       Currency = new Currency
                       {
                           Id = src.CurrencyCode
                       }
                   }));
            });
            return config;
        }
        public static IMapper CreateMapCoInsurancesAssigned()
        {
            var config = MapperCache.GetMapper<ISSEN.CoinsuranceAssigned, IssuanceCoInsuranceCompany>(cfg =>
            {
                cfg.CreateMap<ISSEN.CoinsuranceAssigned, IssuanceCoInsuranceCompany>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuranceCompanyId))
                .ForMember(dest => dest.ParticipationPercentage, opt => opt.MapFrom(src => src.PartCiaPercentage))
                .ForMember(dest => dest.EndorsementNumber, opt => opt.MapFrom(src => src.CompanyNum.ToString()));
            });
            return config;
        }

        public static IMapper CreateMapCoInsuranceAccepted()
        {
            var config = MapperCache.GetMapper<ISSEN.CoinsuranceAccepted, IssuanceCoInsuranceCompany>(cfg =>
            {
                cfg.CreateMap<ISSEN.CoinsuranceAccepted, IssuanceCoInsuranceCompany>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.InsuranceCompanyId))
                .ForMember(dest => dest.ParticipationPercentage, opt => opt.MapFrom(src => src.PartCiaPercentage))
                .ForMember(dest => dest.EndorsementNumber, opt => opt.MapFrom(src => src.AnnexNumMain))
                .ForMember(dest => dest.ParticipationPercentageOwn, opt => opt.MapFrom(src => src.PartMainPercentage))
                .ForMember(dest => dest.PolicyNumber, opt => opt.MapFrom(src => src.PolicyNumMain));
            });
            return config;
        }
        #endregion

        //Integración con Previsora
        #region ContractType
        public static IMapper CreateMapContratType()
        {
            var config = MapperCache.GetMapper<SuretyContractType, ComboDTO>(cfg =>
            {
                cfg.CreateMap<SuretyContractType, ComboDTO>();

            });
            return config;
        }
        #endregion ContractType

        public static IMapper CreateMapContractCategories()
        {
            var config = MapperCache.GetMapper<SuretyContractCategories, ComboDTO>(cfg =>
            {
                cfg.CreateMap<SuretyContractCategories, ComboDTO>();

            });
            return config;
        }
        #region Componentes
        public static IMapper CreateMapPaymentDistributionComponents()
        {
            IMapper config = MapperCache.GetMapper<PRODEN.CoPaymentDistributionComponent, PaymentDistributionComp>(cfg =>
            {
                cfg.CreateMap<PRODEN.CoPaymentDistributionComponent, PaymentDistributionComp>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PaymentNumber))
                .ForMember(dest => dest.ComponentId, opt => opt.MapFrom(src => src.ComponentCode));
            });
            return config;
        }
        public static IMapper CreateMapFinancialPaymentPlan()
        {
            IMapper config = MapperCache.GetMapper<FinancialPaymentSchedule, FinancialPaymentPlan>(cfg =>
            {
                cfg.CreateMap<FinancialPaymentSchedule, FinancialPaymentPlan>();
            });
            return config;
        }
        public static IMapper CreateMapComponentValueDTO()
        {
            IMapper config = MapperCache.GetMapper<ComponentValueDTO, ComponentValue>(cfg =>
            {
                cfg.CreateMap<ComponentValueDTO, ComponentValue>()
               .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Taxes));
            });
            return config;
        }
        #endregion
    }

}
