using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Company.Application.WrapperServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using System.Linq;
using CVEMO = Sistran.Company.Application.Vehicles.Models;
using CiaUnderwritingModel = Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.WrapperServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Coverages

        internal static Coverage CreateCoverageByQuoteCoverage(QuoteCoverage quoteCoverage)
        {
            var imapper = CreateMapCoverageByQuoteCoverage();
            return imapper.Map<QuoteCoverage, Coverage>(quoteCoverage);

        }

        internal static List<Coverage> CreateCoveragesByQuoteCoverages(List<QuoteCoverage> quoteCoverages)
        {
            return quoteCoverages.Select(CreateCoverageByQuoteCoverage).ToList();
        }

        #endregion
        #region AutoMapper
        #region CoverageByQuoteCoverage
        public static IMapper CreateMapCoverageByQuoteCoverage()
        {
            var config = MapperCache.GetMapper<QuoteCoverage, Coverage>(cfg =>
            {
                cfg.CreateMap<QuoteCoverage, Coverage>();
            });

            return config;
        }
        #endregion CoverageByQuoteCoverage
        #region policy
        public static IMapper CreateMapCompanyPolicy()
        {
            var config = MapperCache.GetMapper<Endorsement, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<CompanyPrefix, Prefix>();
                cfg.CreateMap<CompanyPrefixType, PrefixType>();
                cfg.CreateMap<CompanyLineBusiness, LineBusiness>();
                cfg.CreateMap<CompanyBranch, Branch>();
                cfg.CreateMap<CompanySalesPoint, SalePoint>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
            });
            return config;
        }
        #endregion policy
        #endregion AutoMapper
        #region GroupCoverages

        internal static GroupCoverage CreateGroupCoverageByQuoteGroupCoverage(QuoteGroupCoverage quoteGroupCoverage)
        {

            GroupCoverage groupCoverage = new GroupCoverage
            {
                Id = quoteGroupCoverage.Id,
                Description = quoteGroupCoverage.Description,
                InsuredObjects = quoteGroupCoverage.InsuredObjects,
                IsMandatory = quoteGroupCoverage.IsMandatory,
                IsSelected = quoteGroupCoverage.IsSelected,
                MainCoverageId = quoteGroupCoverage.MainCoverageId,
                Number = quoteGroupCoverage.Number,
                PosRuleSetId = quoteGroupCoverage.PosRuleSetId,
                RuleSetId = quoteGroupCoverage.RuleSetId,
                ScriptId = quoteGroupCoverage.ScriptId,
                SublimitPercentage = quoteGroupCoverage.SublimitPercentage
            };
            groupCoverage.Coverages = CreateCoveragesByQuoteCoverages(quoteGroupCoverage.Coverages);
            return groupCoverage;

        }
        internal static List<GroupCoverage> CreateGroupCoveragesByQuoteCoverages(List<QuoteGroupCoverage> quoteGroupCoverages)
        {
            return quoteGroupCoverages.Select(CreateGroupCoverageByQuoteGroupCoverage).ToList();
        }

        #endregion
        #region CompanyProduct
        internal static CompanyPolicy CreateCompanyPolicyByQuotePolicy(QuotePolicy quotePolicy)
        {

            CompanyPolicy companyPolicy = new CompanyPolicy();
            companyPolicy.Product = new CompanyProduct
            {
                AdditDisCommissPercentage = quotePolicy.QuoteProduct.AdditDisCommissPercentage,
                AdditionalCommissionPercentage = quotePolicy.QuoteProduct.AdditionalCommissionPercentage,
                CurrentFrom = quotePolicy.QuoteProduct.CurrentFrom,
                CurrentTo = quotePolicy.QuoteProduct.CurrentTo,
                DecrementCommisionAdjustFactorPercentage = quotePolicy.QuoteProduct.DecrementCommisionAdjustFactorPercentage,
                Description = quotePolicy.QuoteProduct.Description,
                Id = quotePolicy.QuoteProduct.Id,
                IncrementCommisionAdjustFactorPercentage = quotePolicy.QuoteProduct.IncrementCommisionAdjustFactorPercentage,
                IsCollective = quotePolicy.QuoteProduct.IsCollective,
                IsFlatRate = quotePolicy.QuoteProduct.IsFlatRate,
                IsGreen = quotePolicy.QuoteProduct.IsGreen,
                IsRequest = quotePolicy.QuoteProduct.IsRequest,
                PreRuleSetId = quotePolicy.QuoteProduct.PreRuleSetId,
                RuleSetId = quotePolicy.QuoteProduct.RuleSetId,
                ScriptId = quotePolicy.QuoteProduct.ScriptId,
                SmallDescription = quotePolicy.QuoteProduct.SmallDescription,
                StandardCommissionPercentage = quotePolicy.QuoteProduct.StandardCommissionPercentage,
                StdDiscountCommPercentage = quotePolicy.QuoteProduct.StdDiscountCommPercentage,
                SurchargeCommissionPercentage = quotePolicy.QuoteProduct.SurchargeCommissionPercentage,
                Version = quotePolicy.QuoteProduct.Version,

            };

            return companyPolicy;

        }
        #endregion
        #region Vehicles

        internal static CompanyVehicle CreateCompanyVehicleFromRequestQuoteVehicle(QuoteVehicleRequest quoteVehicleRequest)
        {
            CompanyVehicle companyVehicle = new CompanyVehicle
            {
                Risk = new CompanyRisk
                {
                    Policy = new CompanyPolicy
                    {
                        Id = quoteVehicleRequest.TemporalId,
                        Holder = new Holder
                        {
                            IndividualId = quoteVehicleRequest.CustomerId,
                            CustomerType = (CustomerType)quoteVehicleRequest.CustomerTypeId
                        },
                        Prefix = new CompanyPrefix
                        {
                            Id = quoteVehicleRequest.PrefixId
                        },
                        Product = new CompanyProduct
                        {
                            Id = quoteVehicleRequest.ProductId
                        },
                        Endorsement = new CompanyEndorsement
                        {
                            QuotationId = quoteVehicleRequest.QuotationId,
                            QuotationVersion = 1
                        },
                        UserId = quoteVehicleRequest.UserId
                    },

                    RatingZone = new CompanyRatingZone
                    {
                        Id = quoteVehicleRequest.RatingZoneId
                    },
                    MainInsured = new CiaUnderwritingModel.CompanyIssuanceInsured
                    {
                        IndividualId = quoteVehicleRequest.CustomerId,
                        CustomerType = (CustomerType)quoteVehicleRequest.CustomerTypeId
                    },
                    GroupCoverage = new GroupCoverage
                    {
                        Id = quoteVehicleRequest.GroupCoverageId
                    },
                    Description = quoteVehicleRequest.Plate
                },
                LicensePlate = quoteVehicleRequest.Plate,
                Make = new CVEMO.CompanyMake
                {
                    Id = quoteVehicleRequest.MakeId
                },
                Model = new CVEMO.CompanyModel
                {
                    Id = quoteVehicleRequest.ModelId
                },
                Version = new CVEMO.CompanyVersion
                {
                    Id = quoteVehicleRequest.VersionId,
                    Type = new CVEMO.CompanyType
                    {
                        Id = quoteVehicleRequest.TypeId
                    },
                    Fuel = new CVEMO.CompanyFuel
                    {
                        Id = 1
                    },
                    Body = new CVEMO.CompanyBody
                    {
                        Id = 1
                    },
                    ServiceType = new CVEMO.CompanyServiceType
                    {
                        Id = 1
                    }
                },
                Fasecolda = new Fasecolda
                {
                    Description = quoteVehicleRequest.FasecoldaCode
                },
                ServiceType = new CVEMO.CompanyServiceType
                {
                    Id = 1
                },
                Use = new CompanyUse
                {
                    Id = quoteVehicleRequest.UseId
                },
                Year = quoteVehicleRequest.Year,
                Price = quoteVehicleRequest.Price,
                Rate = quoteVehicleRequest.Rate
            };

            return companyVehicle;
        }

        internal static QuoteVehicleResponse CreateQuoteVehicleResponseFromCompanyVehicle(CompanyVehicle companyVehicle)
        {
            return new QuoteVehicleResponse
            {
                TemporalId = companyVehicle.Risk.Policy.Id,
                QuotationId = companyVehicle.Risk.Policy.Endorsement.QuotationId,
                Premium = companyVehicle.Risk.Policy.Summary.Premium,
                Taxes = companyVehicle.Risk.Policy.Summary.Taxes,
                Expenses = companyVehicle.Risk.Policy.Summary.Expenses
            };
        }

        #endregion
    }
}