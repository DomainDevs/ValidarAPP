using AutoMapper;
using Sistran.Company.Application.UnderwritingBusinessService.Model;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Company.Application.CommonServices.Models;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using PAREN = Sistran.Core.Application.Parameters.Entities;
using MODEN = Sistran.Core.Application.ModelServices.Enums;

namespace Sistran.Company.Application.UnderwritingBusinessServiceProvider.Assemblers
{
    public class ModelAssembler
    {
        #region CompanyPaymentPlan

        public static void CreateMapCompanyPaymentPlan()
        {
            Mapper.CreateMap<PaymentPlan, CompanyPolicyPaymentPlan>();
            Mapper.CreateMap<Quota, CompanyQuota>();
        }

        #endregion


        #region CompanyClause


        public static void CreateMapCompanyClause()
        {
            Mapper.CreateMap<Clause, CompanyClause>();
        }

        #endregion


        #region Holder


        public static CompanyHolder CreateMapCompanyHolder(Holder hoter)
        {
            CompanyHolder companyHolder = new CompanyHolder();
            companyHolder.BirthDate = hoter.BirthDate;
            companyHolder.IndividualId = hoter.IndividualId;
            companyHolder.CustomerTypeDescription = hoter.CustomerTypeDescription;
            companyHolder.InsuredId = hoter.InsuredId;
            companyHolder.OwnerRoleCode = hoter.OwnerRoleCode;
            companyHolder.Name = hoter.Name;
            companyHolder.Surname = hoter.Surname;
            companyHolder.SecondSurname = hoter.SecondSurname;
            companyHolder.Gender = hoter.Gender;
            companyHolder.IndividualType = hoter.IndividualType;
            companyHolder.CustomerType = hoter.CustomerType;
            companyHolder.DeclinedDate = hoter.DeclinedDate;

            if (hoter.PaymentMethod != null)
            {
                companyHolder.PaymentMethod = new CompanyIssuancePaymentMethod
                {
                    Id = hoter.PaymentMethod.Id,
                    PaymentId = hoter.PaymentMethod.PaymentId,
                    Description = hoter.PaymentMethod.Description
                };
            }

            if (hoter.EconomicActivity != null)
            {
                companyHolder.EconomicActivity = new CompanyIssuanceEconomicActivity
                {
                    Id = hoter.EconomicActivity.Id,
                    Description = hoter.EconomicActivity.Description
                };
            }


            if (hoter.IdentificationDocument != null)
            {
                companyHolder.IdentificationDocument = new CompanyIssuanceIdentificationDocument
                {
                    ExpeditionDate = hoter.IdentificationDocument.ExpeditionDate,
                    Number = hoter.IdentificationDocument.Number,
                    DocumentType = new CompanyIssuanceDocumentType
                    {
                        Id = hoter.IdentificationDocument.DocumentType.Id,
                        Description = hoter.IdentificationDocument.DocumentType.Description,
                        SmallDescription = hoter.IdentificationDocument.DocumentType.SmallDescription

                    }
                                      
                };

              }

            if(hoter.CompanyName !=null)
            {
                companyHolder.CompanyName = new CompanyPolicyIssuanceCompanyName
                {
                    NameNum = hoter.CompanyName.NameNum,
                    TradeName = hoter.CompanyName.TradeName,
                    IsMain = hoter.CompanyName.IsMain,
                    Address = new CompanyPolicyIssuanceAddress
                    {
                         Id= hoter.CompanyName.Address.Id,
                         Description= hoter.CompanyName.Address.Description,
                         City = new CompanyPolicyCity
                         {
                             Id = hoter.CompanyName.Address.City.Id,
                             Description = hoter.CompanyName.Address.City.Description,
                             State = new CompanyPolicyState
                             {
                                 Id = hoter.CompanyName.Address.City.State.Id,
                                 Description = hoter.CompanyName.Address.City.State.Description,
                                 Country = new CompanyPolicyCountry
                                 {
                                      Id = hoter.CompanyName.Address.City.State.Country.Id,
                                      Description = hoter.CompanyName.Address.City.State.Country.Description
                                 }
                             }
                         }

                    },
                    Phone = new CompanyPolicyIssuancePhone
                    {
                         Id = hoter.CompanyName.Phone.Id,
                         Description = hoter.CompanyName.Phone.Description
                       
                    },
                    Email = new CompanyPolicyIssuanceEmail
                    {
                         Id = hoter.CompanyName.Email.Id,
                         Description = hoter.CompanyName.Email.Description
                    }

                };
            }

            return companyHolder;
        }

        #endregion

        #region Product


        /// <summary>
        /// Creates the company product.
        /// </summary>
        /// <param name="coreProduct">The core product.</param>
        /// <returns></returns>


        public static CompanyPolicyProduct CreateMapCompanyProduct(CompanyProduct companyProduct)
        {
            CompanyPolicyProduct companyProductNew = new CompanyPolicyProduct();
            companyProductNew.Id = companyProduct.Id;
            companyProductNew.Description = companyProduct.Description;
            companyProductNew.SmallDescription = companyProduct.SmallDescription;
            companyProductNew.PreRuleSetId = companyProduct.PreRuleSetId;
            companyProductNew.RuleSetId = companyProduct.RuleSetId;
            companyProductNew.ScriptId = companyProduct.ScriptId;
            companyProductNew.AdditDisCommissPercentage = companyProduct.AdditDisCommissPercentage;
            companyProductNew.StandardCommissionPercentage = companyProduct.StandardCommissionPercentage;
            companyProductNew.IsGreen = companyProduct.IsGreen;
            companyProductNew.IsRequest = companyProduct.IsRequest;
            companyProductNew.IsFlatRate = companyProduct.IsFlatRate;
            companyProductNew.IsCollective = companyProduct.IsCollective;
            companyProductNew.IsScore = companyProduct.IsScore;
            companyProductNew.IsFine = companyProduct.IsFine;
            companyProductNew.IsFasecolda = companyProduct.IsFasecolda;
            companyProductNew.CurrentTo = companyProduct.CurrentTo;
            companyProductNew.CalculateMinPremium = companyProduct.CalculateMinPremium;
            if (companyProduct.CoveredRisk != null)
            {
                companyProductNew.CoveredRisk = new CompanyPolicyCoveredRisk
                {
                    Id = companyProduct.CoveredRisk.Id,
                    Description = companyProduct.CoveredRisk.Description,
                    CoveredRiskType = companyProduct.CoveredRisk.CoveredRiskType,
                    PreRuleSetId = companyProduct.CoveredRisk.PreRuleSetId,
                    RuleSetId = companyProduct.CoveredRisk.RuleSetId,
                    ScriptId = companyProduct.CoveredRisk.ScriptId,
                    SubCoveredRiskType = companyProduct.CoveredRisk.SubCoveredRiskType,
                    MaxRiskQuantity = companyProduct.CoveredRisk.MaxRiskQuantity,
                };
            }
            return companyProductNew;
        }

        #endregion


        #region Policy


        public static CompanyPolicy CreateCompanyPolicy(Policy policy)
        {
            CompanyPolicy companyPolicy = new CompanyPolicy();
            CreateMapCompanyPolicy();
            return Mapper.Map(policy, companyPolicy);

        }

        public static void CreateMapCompanyPolicy()
        {
            Mapper.CreateMap<Product, CompanyPolicyProduct>();
            Mapper.CreateMap<CoveredRisk, CompanyPolicyCoveredRisk>();
            Mapper.CreateMap<Prefix, CompanyPolicyPrefix>();
            Mapper.CreateMap<Summary, CompanySummary>();
            Mapper.CreateMap<Risk, CompanyPolicyRisk>();
            Mapper.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
            Mapper.CreateMap<Core.Application.UniqueUserServices.Models.User, CompanyPolicyUser>();
            CreateMapCoverage();
            Mapper.CreateMap<Component, CompanyComponent>();
            Mapper.CreateMap<PayerComponent, CompanyPayerComponent>();
            Mapper.CreateMap<Coverage, CompanyCoverage>();
            Mapper.CreateMap<Beneficiary, CompanyBeneficiary>();
            Mapper.CreateMap<Clause, CompanyClause>();
            Mapper.CreateMap<BillingGroup, CompanyBillingGroup>();
            Mapper.CreateMap<PolicyType, CommonServices.Models.CompanyPolicyType>();
            Mapper.CreateMap<Endorsement, CompanyEndorsement>();
            Mapper.CreateMap<Branch, CompanyPolicyBranch>();
            Mapper.CreateMap<SalePoint, CompanyPolicySalesPoint>();
            Mapper.CreateMap<Text, CompanyText>();
            Mapper.CreateMap<Policy, CompanyPolicy>();
            Mapper.CreateMap<PaymentPlan, CompanyPolicyPaymentPlan>();
        }

        #endregion

        #region Surcharge
        public static CompanySurchargeComponent CreateCompanySurcharge(QUOEN.Component component, QUOEN.SurchargeComponent surchargeComponent, PAREN.RateType rateType)
        {
            return new CompanySurchargeComponent
            {
                Rate = surchargeComponent.Rate,
                Description = component.SmallDescription,
                RateType = (rateType != null) ? CreateCompanyRateType(rateType): null ,
                Id = component.ComponentCode
            };
        }
        #endregion

        #region RateType
        public static CompanyRateType CreateCompanyRateType(PAREN.RateType rateType)
        {
            return new CompanyRateType
            {
                Description = rateType.Description,
                Id = rateType.RateTypeCode
            };
        }
        #endregion

        #region Coverage

        /// <summary>
        /// Creates the map company coverage.
        /// </summary>
        public static void CreateMapCoverage()
        {
            Mapper.CreateMap<Text, CompanyText>();
            Mapper.CreateMap<Clause, CompanyClause>();
            Mapper.CreateMap<Deductible, CompanyDeductible>();
            Mapper.CreateMap<InsuredObject, CompanyInsuredObject>();
            Mapper.CreateMap<LineBusiness, CompanyLineBusiness>();
            Mapper.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
            Mapper.CreateMap<Coverage, CompanyCoverage>();

        }

        internal static CompanyCoverage CreateMapCompanyCoverage(Coverage coverage)
        {
            CreateMapCoverage();
            return Mapper.Map<Coverage, CompanyCoverage>(coverage);
        }


        internal static Coverage CreateMapCoreCoverage(CompanyCoverage companyCoverage)
        {
            CreateMapCoverage();
            return Mapper.Map<CompanyCoverage, Coverage>(companyCoverage);
        }


        #endregion

    }
}
