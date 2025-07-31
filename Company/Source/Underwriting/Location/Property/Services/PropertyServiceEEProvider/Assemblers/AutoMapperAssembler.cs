using AutoMapper;
using PARAM = Sistran.Company.Application.Location.PropertyServices.EEProvider.Entities;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.CommonService.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region SuBActivities
        public static IMapper CreateMapSuBActivities()
        {
            IMapper config = MapperCache.GetMapper<PARAM.RiskCommercialType, CompanyRiskSubActivity>(cfg =>
            {
                cfg.CreateMap<PARAM.RiskCommercialType, CompanyRiskSubActivity>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.RiskCommercialTypeCode));
            });
            return config;
        }
        #endregion SuBActivities

        #region AssuranceMode
        public static IMapper CreateMapAssuranceMode()
        {
            IMapper config = MapperCache.GetMapper<PARAM.InsuranceMode, CompanyAssuranceMode>(cfg =>
            {
                cfg.CreateMap<PARAM.InsuranceMode, CompanyAssuranceMode>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.InsuranceModeCode));
            });
            return config;
        }
        #endregion AssuranceMode

        #region CompanyLocationsByRiskLocation
        public static IMapper CreateMapCompanyLocationsByRiskLocation()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.RiskLocation, CompanyRiskLocation>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskLocation, CompanyRiskLocation>()
                .ForMember(d => d.Risk, o => o.MapFrom(c => new Risk
                {
                    RiskId = c.RiskId
                }))
                .ForMember(d => d.Street, o => o.MapFrom(c => c.Street));
            });
            return config;
        }

        public static IMapper CreateMapClaimCompanyLocationsByRiskLocation()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.RiskLocation, CompanyRiskLocation>(cfg =>
            {
                cfg.CreateMap<ISSEN.RiskLocation, CompanyRiskLocation>()
                .ForMember(d => d.Risk, o => o.MapFrom(c => new Risk
                {
                    RiskId = c.RiskId
                }))
                .ForMember(d => d.City, o => o.MapFrom(c => new City
                {
                    Id = c.CityCode.GetValueOrDefault()
                }))
                .ForMember(d => d.Risk, o => o.MapFrom(c => new Country
                {
                    Id = c.CountryCode
                }))
                .ForMember(d => d.Risk, o => o.MapFrom(c => new State
                {
                    Id = c.StateCode
                }));
            });
            return config;
        }
        #endregion CompanyLocationsByRiskLocation

        #region DeclarationPeriodType
        public static IMapper CreateMapDeclarationPeriodType()
        {
            IMapper config = MapperCache.GetMapper<PARAMEN.DeclarationPeriod, CompanyDeclarationPeriod>(cfg =>
            {
                cfg.CreateMap<PARAMEN.DeclarationPeriod, CompanyDeclarationPeriod>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.DeclarationPeriodCode));
            });
            return config;
        }
        #endregion DeclarationPeriodType

        #region AdjustPeriod
        public static IMapper CreateMapAdjustPeriod()
        {
            IMapper config = MapperCache.GetMapper<PARAMEN.BillingPeriod, CompanyAdjustPeriod>(cfg =>
            {
                cfg.CreateMap<PARAMEN.BillingPeriod, CompanyAdjustPeriod>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.BillingPeriodCode));
            });
            return config;
        }
        #endregion AdjustPeriod

        #region CompanyEndorsement
        public static IMapper CreateMapCompanyEndorsement()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.Endorsement, CompanyEndorsement>(cfg =>
            {
                cfg.CreateMap<ISSEN.Endorsement, CompanyEndorsement>()
                .ForMember(d => d.Id, o => o.MapFrom(c => c.EndorsementId))
                .ForMember(d => d.EndorsementType, o => o.MapFrom(c => (EndorsementType)c.EndoTypeCode))
                .ForMember(d => d.CurrentTo, o => o.MapFrom(c => c.CurrentTo ?? DateTime.Now));
            });
            return config;
        }
        #endregion CompanyEndorsement

       
    }
}
