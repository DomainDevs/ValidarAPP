using AutoMapper;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM = Sistran.Company.Application.UnderwritingServices.Models;
using RM = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.TransportChangeAgentService.EEProvider.Assemblers
{
    public class ModelAssembler
    {

        public static List<CRM.CompanyRisk> CreateCompanyRisk(List<RM.Risk> risk)
        {
            if (risk == null)
            {
                throw new ArgumentNullException(nameof(risk));
            }

            var immaper = CreateMapCompanyRisk();
            return immaper.Map<List<RM.Risk>, List<CRM.CompanyRisk>>(risk);
        }
        #region AUTOMAPPER

        #region Riesgo
        public static IMapper CreateMapCompanyRisk()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Risk, CompanyRisk>();
                cfg.CreateMap<Coverage, CompanyCoverage>();
                cfg.CreateMap<SubLineBusiness, CompanySubLineBusiness>();
                cfg.CreateMap<LineBusiness, CompanyLineBusiness>();
                cfg.CreateMap<Deductible, CompanyDeductible>();
                cfg.CreateMap<InsuredObject, CompanyInsuredObject>();
                cfg.CreateMap<IssuanceInsured, CompanyIssuanceInsured>();
                cfg.CreateMap<IndividualPaymentMethod, CiaIndividualPaymentMethod>();
                #region poliza
                cfg.CreateMap<Policy, CompanyPolicy>();
                cfg.CreateMap<Product, CompanyProduct>();
                cfg.CreateMap<CoveredRisk, CompanyCoveredRisk>();
                cfg.CreateMap<Prefix, CompanyPrefix>();
                cfg.CreateMap<Summary, CompanySummary>();
                cfg.CreateMap<PrefixType, CompanyPrefixType>();
                cfg.CreateMap<PayerComponent, CompanyPayerComponent>();
                cfg.CreateMap<BillingGroup, CompanyBillingGroup>();
                cfg.CreateMap<Branch, CompanyBranch>();
                cfg.CreateMap<SalePoint, CompanySalesPoint>();
                cfg.CreateMap<PolicyType, CompanyPolicyType>();
                cfg.CreateMap<Endorsement, CompanyEndorsement>();
                #endregion
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
                cfg.CreateMap<LimitRc, CompanyLimitRc>();
                cfg.CreateMap<RiskActivity, CompanyRiskActivity>();
                cfg.CreateMap<Clause, CompanyClause>();
                cfg.CreateMap<RatingZone, CompanyRatingZone>();
                cfg.CreateMap<Text, CompanyText>();
            });
            return config.CreateMapper();
        }
        #endregion Riesgo

        #endregion AUTOMAPPER
    }
}
