using AutoMapper;
using Sistran.Core.Application.OperationQuotaServices.DTOs.Consortium;
using Sistran.Core.Application.OperationQuotaServices.DTOs.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.OperationQuota;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using OQDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.OperationQuota;
using ECODTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.EconomicGroup;
using CONDTO = Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        public static IMapper CreateMapApplyReinsurance()
        {
            IMapper config = MapperCache.GetMapper<ApplyReinsurance, ApplyReinsuranceDTO>(cfg =>
            {
                cfg.CreateMap<ApplyReinsurance, ApplyReinsuranceDTO>()
                .ForMember(a => a.ContractCoverage, b => b.MapFrom(c => CreateMapCoverage().Map<List<ContractCoverage>, List<ContractCoverageDTO>>(c.ContractCoverage)));
            });
            return config;
        }

        public static IMapper CreateMapCoverage()
        {
            IMapper config = MapperCache.GetMapper<ContractCoverage, ContractCoverageDTO>(cfg =>
            {
                cfg.CreateMap<ContractCoverage, ContractCoverageDTO>();
            });
            return config;
        }

        public static IMapper CreateMapApplyEndorsement()
        {
            IMapper config = MapperCache.GetMapper<ApplyEndorsement, ApplyEndorsementDTO>(cfg =>
            {
                cfg.CreateMap<ApplyEndorsement, ApplyEndorsementDTO>();
            });
            return config;
        }
        public static IMapper CreateMapIndividualOperatingQuota()
        {
            IMapper config = MapperCache.GetMapper<IndividualOperatingQuota, IndividualOperatingQuotaDTO>(cfg =>
            {
                cfg.CreateMap<IndividualOperatingQuota, IndividualOperatingQuotaDTO>();
            });
            return config;
        }

        public static IMapper CreateMapEconomicGroupEvent()
        {
            IMapper config = MapperCache.GetMapper<EconomicGroupEvent, EconomicGroupEventDTO>(cfg =>
            {
                cfg.CreateMap<EconomicGroupEvent, EconomicGroupEventDTO>()
                .ForMember(a => a.EconomicGroupEventID, b => b.MapFrom(c => c.EconomicGroupEventId))
                .ForMember(a => a.EconomicGroupEventEventType, b => b.MapFrom(c => c.EconomicGroupEventType))
                .ForMember(a => a.EconomicGroupID, b => b.MapFrom(c => c.EconomicGroupId))
                .ForMember(a => a.economicgroupoperatingquotaDTO, b => b.MapFrom(c => CreateMapEconomicGroupoperatingQuota().Map<Economicgroupoperatingquota, EconomicgroupoperatingquotaDTO>(c.EconomicGroupOperatingQuota)))
                .ForMember(a => a.economicgrouppartnersDTO, b => b.MapFrom(c => CreateMapConsortiumpPartners().Map<Economicgrouppartners, EconomicgrouppartnersDTO>(c.EconomicGroupPartners)));
            });
            return config;
        }

        public static IMapper CreateMapEconomicGroupoperatingQuota()
        {
            IMapper config = MapperCache.GetMapper<Economicgroupoperatingquota, EconomicgroupoperatingquotaDTO>(cfg =>
            {
                cfg.CreateMap<Economicgroupoperatingquota, EconomicgroupoperatingquotaDTO>();
            });
            return config;
        }

        public static IMapper CreateMapEconomicGroupPartners()
        {
            IMapper config = MapperCache.GetMapper<Economicgrouppartners, EconomicgrouppartnersDTO>(cfg =>
            {
                cfg.CreateMap<Economicgrouppartners, EconomicgrouppartnersDTO>();
            });
            return config;
        }


        public static IMapper CreateMapConsortiumEvent()
        {
            IMapper config = MapperCache.GetMapper<ConsortiumEvent, ConsortiumEventDTO>(cfg =>
            {
                cfg.CreateMap<ConsortiumEvent, ConsortiumEventDTO>()
                .ForMember(a => a.consortiumDTO, b => b.MapFrom(c => CreateMapConsortium().Map<Consortium, ConsortiumDTO>(c.consortium)))
                .ForMember(a => a.ConsortiumpartnersDTO, b => b.MapFrom(c => CreateMapConsortiumpPartners().Map<Consortiumpartners, ConsortiumpartnersDTO>(c.Consortiumpartners)));
            });
            return config;
        }

        public static IMapper CreateMapConsortiumpPartners()
        {
            IMapper config = MapperCache.GetMapper<Consortiumpartners, ConsortiumpartnersDTO>(cfg =>
            {
                cfg.CreateMap<Consortiumpartners, ConsortiumpartnersDTO>();
            });
            return config;
        }

        public static IMapper CreateMapConsortium()
        {
            IMapper config = MapperCache.GetMapper<Consortium, ConsortiumDTO>(cfg =>
            {
                cfg.CreateMap<Consortium, ConsortiumDTO>();
            });
            return config;
        }

        public static IMapper CreateMapIndividualOperatingQuotaIntegrationDTO()
        {
            IMapper config = MapperCache.GetMapper<IndividualOperatingQuotaDTO, OQDTO.IndividualOperatingQuotaDTO>(cfg =>
            {
                cfg.CreateMap<IndividualOperatingQuotaDTO, OQDTO.IndividualOperatingQuotaDTO>();
            });
            return config;
        }
        public static IMapper CreateMapApplyEndorsementIntegrationDTO()
        {
            IMapper config = MapperCache.GetMapper<ApplyEndorsementDTO, OQDTO.ApplyEndorsementDTO>(cfg =>
            {
                cfg.CreateMap<ApplyEndorsementDTO, OQDTO.ApplyEndorsementDTO>();
            });
            return config;
        }

        public static IMapper CreateMapEconomicGroupEventIntegrationDTO()
        {
            IMapper config = MapperCache.GetMapper<EconomicGroupEventDTO, ECODTO.EconomicGroupEventDTO>(cfg =>
            {
                cfg.CreateMap<EconomicGroupEventDTO, ECODTO.EconomicGroupEventDTO>()
                .ForMember(a => a.economicgroupoperatingquotaDTO, b => b.MapFrom(c => CreateMapEconomicgroupoperatingquotaDTO().Map<EconomicgroupoperatingquotaDTO, ECODTO.EconomicgroupoperatingquotaDTO>(c.economicgroupoperatingquotaDTO)))
                .ForMember(a => a.economicgrouppartnersDTO, b => b.MapFrom(c => CreateMapEconomicgrouppartnersDTO().Map<EconomicgrouppartnersDTO, ECODTO.EconomicgrouppartnersDTO>(c.economicgrouppartnersDTO)));
            });
            return config;
        }

        public static IMapper CreateMapEconomicgroupoperatingquotaDTO()
        {
            IMapper config = MapperCache.GetMapper<EconomicgroupoperatingquotaDTO, ECODTO.EconomicgroupoperatingquotaDTO>(cfg =>
            {
                cfg.CreateMap<EconomicgroupoperatingquotaDTO, ECODTO.EconomicgroupoperatingquotaDTO>();
            });
            return config;
        }
        public static IMapper CreateMapEconomicgrouppartnersDTO()
        {
            IMapper config = MapperCache.GetMapper<EconomicgrouppartnersDTO, ECODTO.EconomicgrouppartnersDTO>(cfg =>
            {
                cfg.CreateMap<EconomicgrouppartnersDTO, ECODTO.EconomicgrouppartnersDTO>();
            });
            return config;
        }
        public static IMapper CreteMapConsortiumEventDTO()
        {
            IMapper config = MapperCache.GetMapper<ConsortiumEventDTO, CONDTO.ConsortiumEventDTO>(cfg =>
            {
                cfg.CreateMap<ConsortiumEventDTO, CONDTO.ConsortiumEventDTO>()
                .ForMember(a => a.consortiumDTO, b => b.MapFrom(c => CreateMapConsortiumDTO().Map<ConsortiumDTO, CONDTO.ConsortiumDTO>(c.consortiumDTO)))
                .ForMember(a => a.ConsortiumpartnersDTO, b => b.MapFrom(c => CreateMapConsortiumpartnersDTO().Map<ConsortiumpartnersDTO, CONDTO.ConsortiumpartnersDTO>(c.ConsortiumpartnersDTO)));
            });
            return config;
        }

        public static IMapper CreateMapConsortiumDTO()
        {
            IMapper config = MapperCache.GetMapper<ConsortiumDTO, CONDTO.ConsortiumDTO>(cfg =>
            {
                cfg.CreateMap<ConsortiumDTO, CONDTO.ConsortiumDTO>();
            });
            return config;
        }

        public static IMapper CreateMapConsortiumpartnersDTO()
        {
            IMapper config = MapperCache.GetMapper<ConsortiumpartnersDTO, CONDTO.ConsortiumpartnersDTO>(cfg =>
            {
                cfg.CreateMap<ConsortiumpartnersDTO, CONDTO.ConsortiumpartnersDTO>();
            });
            return config;
        }

        public static IMapper CreateMapConsortiumEventDTO()
        {
            IMapper config = MapperCache.GetMapper<ConsortiumEventDTO, CONDTO.ConsortiumEventDTO>(cfg =>
            {
                cfg.CreateMap<ConsortiumEventDTO, ConsortiumEventDTO>()
                .ForMember(a => a.consortiumDTO, b => b.MapFrom(c => CreateMapConsortium().Map<ConsortiumDTO, CONDTO.ConsortiumDTO>(c.consortiumDTO)))
                .ForMember(a => a.ConsortiumpartnersDTO, b => b.MapFrom(c => CreateMapConsortiumpPartners().Map<ConsortiumpartnersDTO, CONDTO.ConsortiumpartnersDTO>(c.ConsortiumpartnersDTO)));
            });
            return config;
        }

        internal static IMapper CreateMapRiskConsortiumDTO()
        {
            throw new NotImplementedException();
        }
    }
}
