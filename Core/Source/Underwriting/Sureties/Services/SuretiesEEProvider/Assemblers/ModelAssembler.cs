using AutoMapper;
using Sistran.Core.Application.Sureties.Models;
using Sistran.Core.Application.Utilities.Cache;
using System.Collections.Generic;
using Entities = Sistran.Core.Application.Issuance.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.SuretiesEEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region IssuanceInsuredGuarantee

        public static IssuanceInsuredGuarantee CreateIssuanceInsuredGuarantee(UPEN.InsuredGuarantee entityInsuredGuarantee)
        {
            var imapper = CreateMapIssuanceInsuredGuarantee();
            return imapper.Map<UPEN.InsuredGuarantee, IssuanceInsuredGuarantee>(entityInsuredGuarantee);
        }

        public static List<IssuanceInsuredGuarantee> CreateIssuanceInsuredGuarantees(List<UPEN.InsuredGuarantee> entityInsuredGuarantees)
        {
            var mapper = CreateMapIssuanceInsuredGuarantee();
            return mapper.Map<List<UPEN.InsuredGuarantee>, List<IssuanceInsuredGuarantee>>(entityInsuredGuarantees);
        }

        #endregion

        #region riskGuarantee
        /// <summary>
        /// Crear Contragarantia
        /// </summary>
        /// <param name="riskSuretyGuarantee">Entidad RiskSuretyGuarantee </param>
        /// <returns></returns>
        public static RiskSuretyGuarantee CreateRiskSuretyGuarantee(Entities.RiskSuretyGuarantee riskSuretyGuarantee)
        {
            var imapper = RiskMapSuretyGuarantee();
            return imapper.Map<Entities.RiskSuretyGuarantee, RiskSuretyGuarantee>(riskSuretyGuarantee);
        }
        /// <summary>
        /// Crear listado de contragarantias
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>listado de contragarantias</returns>
        public static List<RiskSuretyGuarantee> CreateRiskSuretyGuarantees(List<Entities.RiskSuretyGuarantee> riskSuretyGuarantees)
        {
            var mapper = RiskMapSuretyGuarantee();
            return mapper.Map<List<Entities.RiskSuretyGuarantee>, List<RiskSuretyGuarantee>>(riskSuretyGuarantees);
        }
        #endregion 
        #region PolicyRiskDTO
        public static UNDTO.PolicyRiskDTO CreatePolicyRiskDTOs(ISSEN.Policy policy)
        {
            var imapper = MapPolicyRisk();
            return imapper.Map<ISSEN.Policy, UNDTO.PolicyRiskDTO>(policy);
        }
        public static List<UNDTO.PolicyRiskDTO> CreatePolicyRiskDTOs(List<ISSEN.Policy> policies)
        {
            var imapper = MapPolicyRisk();
            return imapper.Map<List<ISSEN.Policy>, List<UNDTO.PolicyRiskDTO>>(policies);
        }
        #endregion
        #region Autommaper

        public static IMapper RiskMapSuretyGuarantee()
        {
            var config = MapperCache.GetMapper<Entities.RiskSuretyGuarantee, RiskSuretyGuarantee>(cfg =>
            {
                cfg.CreateMap<Entities.RiskSuretyGuarantee, RiskSuretyGuarantee>();
            });
            return config;



        }
        public static IMapper MapPolicyRisk()
        {
            var config = MapperCache.GetMapper<ISSEN.Policy, UNDTO.PolicyRiskDTO>(cfg =>
            {
                cfg.CreateMap<ISSEN.Policy, UNDTO.PolicyRiskDTO>()
               .ForMember(dest => dest.PrefixId, scr => scr.MapFrom(ori => ori.PrefixCode))
               .ForMember(dest => dest.BranchId, scr => scr.MapFrom(ori => ori.BranchCode));
            });
            return config;

        }

        public static IMapper CreateMapIssuanceInsuredGuarantee()
        {
            var config = MapperCache.GetMapper<UPEN.InsuredGuarantee, IssuanceInsuredGuarantee>(cfg =>
            {
                cfg.CreateMap<UPEN.InsuredGuarantee, IssuanceInsuredGuarantee>()
                .ForMember(dest => dest.Id, scr => scr.MapFrom(ori => ori.GuaranteeCode))
                .ForMember(dest => dest.GuaranteeStatus, scr => scr.MapFrom(ori => new IssuanceGuaranteeStatus
                {
                    Code = ori.GuaranteeStatusCode ?? 0
                }));
            });
            return config;

        }
        #endregion Autommaper
    }
}
