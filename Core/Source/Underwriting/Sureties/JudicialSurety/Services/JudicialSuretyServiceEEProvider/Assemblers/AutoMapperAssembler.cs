using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        public static IMapper CreateMapCourt()
        {
            IMapper config = MapperCache.GetMapper<COMMEN.Court, Court>(cfg =>
            {
                cfg.CreateMap<COMMEN.Court, Court>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.CourtCode))
                 .ForMember(d => d.Description, o => o.MapFrom(c => c.DescriptionCode))
                 .ForMember(d => d.Enabled, o => o.MapFrom(c => c.Enabled))
                 .ForMember(d => d.SmallDescription, o => o.MapFrom(c => c.SmallDescription));


            });
            return config;
        }

        public static IMapper CreateMapArticle()
        {
            IMapper config = MapperCache.GetMapper<COMMEN.Article, Article>(cfg =>
            {
                cfg.CreateMap<COMMEN.Article, Article>()
                 .ForMember(d => d.Id, o => o.MapFrom(c => c.ArticleCode));

            });
            return config;
        }

        public static IMapper CreateMapArticleLine()
        {
            IMapper config = MapperCache.GetMapper<COMMEN.ArticleLine, ArticleLine>(cfg =>
            {
                cfg.CreateMap<COMMEN.ArticleLine, ArticleLine>()
                 .ForMember(d => d.ArticleLineCd, o => o.MapFrom(c => c.ArticleLineCode))
                 .ForMember(d => d.Description, o => o.MapFrom(c => c.Description))
                 .ForMember(d => d.Enabled, o => o.MapFrom(c => c.Enabled))
                 .ForMember(d => d.SmallDescription, o => o.MapFrom(c => c.SmallDescription));

            });
            return config;
        }
        public static IMapper CreateMapJudicialSurety()
        {

            IMapper config = MapperCache.GetMapper<JudicialSuretyMapper, Judgement>(cfg =>
            {
                cfg.CreateMap<JudicialSuretyMapper, Judgement>()
                 .ForMember(d => d.Risk, o => o.MapFrom(c => new Risk
                 {
                     RiskId = c.risk.RiskId,
                     Number = c.endorsementRisk.RiskNum,
                     RatingZone = new RatingZone
                     {
                         Id = c.risk.RatingZoneCode.GetValueOrDefault()
                     },
                     CoveredRiskType = (CoveredRiskType)c.risk.CoveredRiskTypeCode,
                     GroupCoverage = new GroupCoverage
                     {
                         Id = c.risk.CoverGroupId.GetValueOrDefault()
                     },
                     Status = UnderwritingServices.Enums.RiskStatusType.NotModified,
                     AmountInsured = c.RiskJudicialSurety.InsuredValue.GetValueOrDefault(),
                     DynamicProperties = new List<DynamicConcept>(),
                     MainInsured = new IssuanceInsured()
                     {
                         IndividualId = c.risk.InsuredId,
                         CompanyName = new IssuanceCompanyName
                         {
                             NameNum = c.risk.NameNum.GetValueOrDefault(),
                             Address = new IssuanceAddress
                             {
                                 Id = c.risk.AddressId.GetValueOrDefault()
                             }
                         }
                     },
                     Policy = new Policy
                     {
                         Id = c.endorsementRisk.PolicyId,
                         DocumentNumber = c.policy.DocumentNumber,
                         Endorsement = new Endorsement
                         {
                             Id = c.endorsementRisk.EndorsementId,
                             PolicyId = c.policy.PolicyId
                         },

                     },
                     OriginalStatus = (RiskStatusType)c.endorsementRisk.RiskStatusCode,
                     SecondInsured = ValidateInsuredId(c.RiskJudicialSurety)

                 }))
                   .ForMember(d => d.City, o => o.MapFrom(c => new City
                   {
                       Id = c.RiskJudicialSurety.CityCode.GetValueOrDefault(),
                       State = new State
                       {
                           Id = c.RiskJudicialSurety.StateCode.GetValueOrDefault(),
                           Country = new Country
                           {
                               Id = c.RiskJudicialSurety.CountryCode.GetValueOrDefault()
                           }
                       }

                   }))
                   .ForMember(d => d.Article, o => o.MapFrom(c => new Article
                   {
                       Id = c.article.ArticleCode,
                       Description = c.article.Description
                   }))
                   .ForMember(d => d.Court, o => o.MapFrom(c => new Court
                   {
                       Id = c.RiskJudicialSurety.CourtCode ?? 0
                   }))
                    .ForMember(d => d.SettledNumber, o => o.MapFrom(c => c.RiskJudicialSurety.BidNumber))
                    .ForMember(d => d.InsuredValue, o => o.MapFrom(c => c.RiskJudicialSurety.InsuredValue.GetValueOrDefault()))
                    .ForMember(d => d.HolderActAs, o => o.MapFrom(c => c.endorsement.CapacityOfCode == null ? 0 : (CapacityOf)c.endorsement.CapacityOfCode))
                    .ForMember(d => d.InsuredActAs, o => o.MapFrom(c => c.RiskJudicialSurety.CapacityOfCode == null ? 0 : (CapacityOf)c.RiskJudicialSurety.CapacityOfCode))
                    .ForMember(d => d.Attorney, o => o.MapFrom(c => new Attorney()
                    {
                        IdentificationDocument = ValidateIdentificationDocument(c.RiskJudicialSurety),
                        IdProfessionalCard = ValidateIdProfessionalCard(c.RiskJudicialSurety),
                        Name = ValidateInsuredPrintName(c.RiskJudicialSurety)
                    }));

            });
            return config;
        }

        static IdentificationDocument ValidateIdentificationDocument(ISSEN.RiskJudicialSurety riskJudicialSurety)
        {
            IdentificationDocument identificationDocument = null;
            if (riskJudicialSurety.IdCardNo != null && riskJudicialSurety.IdCardTypeCode.HasValue)
            {
                identificationDocument = new IdentificationDocument
                {
                    Number = riskJudicialSurety.IdCardNo,
                    DocumentType = new DocumentType
                    {
                        Id = riskJudicialSurety.IdCardTypeCode.Value
                    }
                };
            }


            return identificationDocument;
        }

        static string ValidateIdProfessionalCard(ISSEN.RiskJudicialSurety riskJudicialSurety)
        {
            string IdProfessionalCard = null;
            if (riskJudicialSurety.ProfessionalCardNum != null)
            {
                IdProfessionalCard = riskJudicialSurety.ProfessionalCardNum;
            }


            return IdProfessionalCard;
        }

        static string ValidateInsuredPrintName(ISSEN.RiskJudicialSurety riskJudicialSurety)
        {
            string InsuredPrintName = null;
            if (riskJudicialSurety.InsuredPrintName != null)
            {
                InsuredPrintName = riskJudicialSurety.InsuredPrintName;
            }


            return InsuredPrintName;
        }
        static IssuanceInsured ValidateInsuredId(ISSEN.RiskJudicialSurety riskJudicialSurety)
        {
            IssuanceInsured issuanceInsured = null;
            if (riskJudicialSurety.InsuredId != null && riskJudicialSurety.InsuredId.HasValue)
            {
                issuanceInsured = new IssuanceInsured
                {
                    IndividualId = riskJudicialSurety.InsuredId.Value,
                    Name = riskJudicialSurety.InsuredPrintName
                };
            }


            return issuanceInsured;
        }
    }
}