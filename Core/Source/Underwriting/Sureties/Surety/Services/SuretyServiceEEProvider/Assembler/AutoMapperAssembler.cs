using AutoMapper;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Models;
using Sistran.Core.Application.Sureties.SuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Sureties.SuretyServices.EEProvider.Assembler
{
    public class AutoMapperAssembler
    {
        public static IMapper CreateMapContract()
        {
            IMapper config = MapperCache.GetMapper<ContractMapper, Contract>(cfg =>
            {
                cfg.CreateMap<ContractMapper, Contract>()
                 .ForMember(d => d.Risk, o => o.MapFrom(c => new Risk
                 {
                     RiskId = c.entityRisk.RiskId,
                     CoveredRiskType = (CoveredRiskType)c.entityRisk.CoveredRiskTypeCode,
                     GroupCoverage = new GroupCoverage
                     {
                         Id = c.entityRisk.CoverGroupId.Value,
                         CoveredRiskType = (CoveredRiskType)c.entityRisk.CoveredRiskTypeCode
                     },
                     MainInsured = new IssuanceInsured
                     {
                         IndividualId = c.entityRisk.InsuredId,
                         CompanyName = new IssuanceCompanyName
                         {
                             NameNum = c.entityRisk.NameNum.GetValueOrDefault(),
                             Address = new IssuanceAddress
                             {
                                 Id = c.entityRisk.AddressId.GetValueOrDefault()
                             }
                         }
                     },
                     Policy = new Policy
                     {
                         Id = c.entityEndorsementRisk.PolicyId,
                         DocumentNumber = c.entityPolicy.DocumentNumber,
                         Endorsement = new Endorsement
                         {
                             Id = c.entityEndorsementRisk.EndorsementId,
                             PolicyId = c.entityPolicy.PolicyId
                         },

                     },
                     Number = c.entityEndorsementRisk.RiskNum,
                     OriginalStatus = (RiskStatusType)c.entityEndorsementRisk.RiskStatusCode,
                     Status = RiskStatusType.NotModified,
                     DynamicProperties = new List<RulesScriptsServices.Models.DynamicConcept>(),


                 }))
                 .ForMember(d => d.Value, o => o.MapFrom(c => new CommonService.Models.Amount
                 {
                     Value = c.entityRiskSurety.ContractAmount
                 }))
                .ForMember(d => d.ContractType, o => o.MapFrom(c => new ContractType
                {
                    Id = c.entityRiskSurety.SuretyContractTypeCode
                }))
                .ForMember(d => d.Isfacultative, o => o.MapFrom(c => c.entityRiskSurety.IsFacultative))
                .ForMember(d => d.Class, o => o.MapFrom(c => new ContractClass
                {
                    Id = Convert.ToInt32(c.entityRiskSurety.SuretyContractCategoriesCode)
                }))
                .ForMember(d => d.Contractor, o => o.MapFrom(c => new Contractor
                {
                    IndividualId = c.entityRiskSurety.IndividualId
                }))
                .ForMember(d => d.SettledNumber, o => o.MapFrom(c => c.entityRiskSurety.BidNumber));

            });
            return config;
        }
    }
}
