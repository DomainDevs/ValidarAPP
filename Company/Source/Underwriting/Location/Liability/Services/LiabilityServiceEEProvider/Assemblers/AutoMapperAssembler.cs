using AutoMapper;
using Sistran.Company.Application.Location.LiabilityServices.EEProvider.Entities;
using Sistran.Company.Application.Location.LiabilityServices.Models;
using Sistran.Company.Application.Locations.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.Location.LiabilityServices.Models;
using Sistran.Core.Application.Locations.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.LiabilityServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region asegurado
        public static IMapper CreateMapCompanyInsured()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CompanyIssuanceInsured, CompanyInsured>();
            });
            return config.CreateMapper();
        }
        #endregion asegurado

        #region beneficiario
        public static IMapper CreateMapCompanyBeneficiary()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Beneficiary, CompanyBeneficiary>();
                cfg.CreateMap<BeneficiaryType, CompanyBeneficiaryType>();
                cfg.CreateMap<CompanyIssuanceInsured, CompanyBeneficiary>();
            });
            return config.CreateMapper();
        }
        #endregion beneficiario

        #region risk
        public static IMapper CreateMapLiabilityRisk()
        {
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<CompanyLiabilityRisk, LiabilityRisk>();
                cfg.CreateMap<CompanyRisk, Risk>();
                cfg.CreateMap<CompanyRatingZone, RatingZone>();
                cfg.CreateMap<CompanyClause, Clause>();
                cfg.CreateMap<CompanyRiskActivity, RiskActivity>();
                cfg.CreateMap<CompanyLimitRc, LimitRc>();
                cfg.CreateMap<CompanyBeneficiary, Beneficiary>();
                cfg.CreateMap<CompanyBeneficiaryType, BeneficiaryType>();
                cfg.CreateMap<CompanyCoverage, Coverage>();
                cfg.CreateMap<CompanyIssuanceInsured, IssuanceInsured>();
                cfg.CreateMap<CompanyPolicy, CompanyPolicy>();
                cfg.CreateMap<CompanyPayerComponent, PayerComponent>();
                cfg.CreateMap<CompanyText, Text>();
                cfg.CreateMap<CompanySummary, Summary>();
                cfg.CreateMap<CompanyPayerComponent, PayerComponent>();
                cfg.CreateMap<CompanyEndorsement, Endorsement>();
                cfg.CreateMap<CompanyPaymentPlan, PaymentPlan>();
                cfg.CreateMap<CompanyRiskUse, RiskUse>();
                cfg.CreateMap<CompanyDeductible, Deductible>();
                cfg.CreateMap<CompanyInsuredObject, InsuredObject>();
                cfg.CreateMap<CompanyComponent, Component>();
                cfg.CreateMap<CompanyBillingGroup, BillingGroup>();

            });

            return config.CreateMapper();
        }
        #endregion risk

        #region Deductible
        public static IMapper CreateMapCompanyDeductible()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Deductible, CompanyDeductible>();
            });
            return config.CreateMapper();
        }

        #endregion Deductible

        #region RisksubActivities
        public static IMapper CreateMapRisksubActivities()
        {
            IMapper config = MapperCache.GetMapper<RiskCommercialType, CompanyRiskSubActivity>(cfg =>
            {
                cfg.CreateMap<RiskCommercialType, CompanyRiskSubActivity>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.RiskCommercialTypeCode));
            });
            return config;
        }
        #endregion RisksubActivities

        #region AssuranceMode
        public static IMapper CreateMapAssuranceMode()
        {
            IMapper config = MapperCache.GetMapper<InsuranceMode, CompanyAssuranceMode>(cfg =>
            {
                cfg.CreateMap<InsuranceMode, CompanyAssuranceMode>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.InsuranceModeCode));
            });
            return config;
        }
        #endregion AssuranceMode

    }
}
