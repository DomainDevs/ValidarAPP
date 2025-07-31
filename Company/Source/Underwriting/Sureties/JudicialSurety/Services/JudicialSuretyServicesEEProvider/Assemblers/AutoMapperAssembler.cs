using AutoMapper;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;
using Sistran.Company.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IssuanceEntities = Sistran.Core.Application.Issuance.Entities;
using Rules = Sistran.Core.Framework.Rules;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Newtonsoft.Json;

namespace Sistran.Company.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        public static IMapper CreateMapRiskPostS()
        {
            IMapper config = MapperCache.GetMapper<CompanyJudgementMapper, CompanyJudgement>(cfg =>
            {
                cfg.CreateMap<CompanyJudgementMapper, CompanyJudgement>()
                 .ForMember(d => d.Risk, o => o.MapFrom(c => new CompanyRisk
                 {
                     RiskId = c.risk.RiskId,
                     Number = c.endorsementRisk.RiskNum,
                     IsFacultative = c.risk.IsFacultative,
                     RatingZone = new CompanyRatingZone
                     {
                         Id = c.risk.RatingZoneCode.GetValueOrDefault()
                     },
                     CoveredRiskType = (CoveredRiskType)c.risk.CoveredRiskTypeCode,
                     GroupCoverage = new GroupCoverage
                     {
                         Id = c.risk.CoverGroupId.GetValueOrDefault()
                     },
                     Status = RiskStatusType.NotModified,
                     AmountInsured = c.RiskJudicialSurety.InsuredValue.GetValueOrDefault(),
                     DynamicProperties = new List<Core.Application.RulesScriptsServices.Models.DynamicConcept>(),
                     MainInsured = new CompanyIssuanceInsured()
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
                     RiskActivity = new CompanyRiskActivity
                     {
                         Id = c.risk.RiskCommercialClassCode.GetValueOrDefault()
                     },
                     OriginalStatus = (RiskStatusType)c.endorsementRisk.RiskStatusCode,
                     SecondInsured = ValidateInsuredId(c.RiskJudicialSurety),
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
                  .ForMember(d => d.Article, o => o.MapFrom(c => new CompanyArticle
                  {
                      Id = c.RiskJudicialSurety.ArticleCode ?? 0
                  }))
                  .ForMember(d => d.Court, o => o.MapFrom(c => new CompanyCourt
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
                      InsuredToPrint = ValidateInsuredPrintName(c.RiskJudicialSurety),
                      Name = c.RiskJudicialSurety.InsuredCaution ?? string.Empty
                  }));
            });
            return config;
        }

        static IdentificationDocument ValidateIdentificationDocument(IssuanceEntities.RiskJudicialSurety riskJudicialSurety)
        {
            IdentificationDocument identificationDocument = null;
            if (riskJudicialSurety.IdentificationIdAgent != null && riskJudicialSurety.DocumentNumberAgent != null)
            {
                identificationDocument = new IdentificationDocument
                {
                    Number = riskJudicialSurety.DocumentNumberAgent,
                    DocumentType = new DocumentType
                    {
                        Id = int.Parse(riskJudicialSurety.IdentificationIdAgent)
                    }
                };
            }


            return identificationDocument;
        }
        static string ValidateIdProfessionalCard(IssuanceEntities.RiskJudicialSurety riskJudicialSurety)
        {
            string IdProfessionalCard = null;
            if (riskJudicialSurety.ProfessionalCardNum != null)
            {
                IdProfessionalCard = riskJudicialSurety.ProfessionalCardNum;
            }


            return IdProfessionalCard;
        }

        static string ValidateInsuredPrintName(IssuanceEntities.RiskJudicialSurety riskJudicialSurety)
        {
            string InsuredPrintName = null;
            if (riskJudicialSurety.InsuredPrintName != null)
            {
                InsuredPrintName = riskJudicialSurety.InsuredPrintName;
            }


            return InsuredPrintName;
        }
        static CompanyIssuanceInsured ValidateInsuredId(IssuanceEntities.RiskJudicialSurety riskJudicialSurety)
        {
            CompanyIssuanceInsured issuanceInsured = null;
            if (riskJudicialSurety.InsuredId != null && riskJudicialSurety.InsuredId.HasValue)
            {
                issuanceInsured = new CompanyIssuanceInsured
                {
                    IndividualId = riskJudicialSurety.InsuredId.Value,
                    Name = riskJudicialSurety.InsuredPrintName
                };
            }


            return issuanceInsured;
        }

        public static IMapper CreateMapBeneficiary()
        {
            IMapper config = MapperCache.GetMapper<IssuanceEntities.RiskBeneficiary, CompanyBeneficiary>(cfg =>
            {
                cfg.CreateMap<IssuanceEntities.RiskBeneficiary, CompanyBeneficiary>()
                 .ForMember(d => d.IndividualId, o => o.MapFrom(c => c.BeneficiaryId))
                 .ForMember(d => d.CustomerType, o => o.MapFrom(c => CustomerType.Individual))
                 .ForMember(d => d.BeneficiaryType, o => o.MapFrom(c => new CompanyBeneficiaryType
                 {
                     Id = c.BeneficiaryTypeCode
                 }))
                 .ForMember(d => d.Participation, o => o.MapFrom(c => c.BenefitPercentage))
                 .ForMember(d => d.CompanyName, o => o.MapFrom(c => new IssuanceCompanyName
                 {
                     NameNum = c.NameNum.GetValueOrDefault(),
                     Address = new IssuanceAddress
                     {
                         Id = c.AddressId
                     }
                 }));
            });
            return config;
        }

        public static IMapper CreateMapDynamicConcept()
        {
            IMapper config = MapperCache.GetMapper<Rules.Concept, DynamicConcept>(cfg =>
            {
                cfg.CreateMap<Rules.Concept, DynamicConcept>();
            });
            return config;
        }

        public static IMapper CreateMapJudgement()
        {
            IMapper config = MapperCache.GetMapper<ISSEN.EndorsementOperation, CompanyJudgement>(cfg =>
            {
                cfg.CreateMap<ISSEN.EndorsementOperation, CompanyJudgement>()
                    .ForMember(d => d, o => o.MapFrom(c => CreateMapJudgement(c)));

            });
            return config;
        }

        static CompanyJudgement CreateMapJudgement(ISSEN.EndorsementOperation endorsementOperation)
        {
            CompanyJudgement companyJudgement = null;
            if (!string.IsNullOrEmpty(endorsementOperation.Operation))
            {
                companyJudgement = JsonConvert.DeserializeObject<CompanyJudgement>(endorsementOperation.Operation);
                companyJudgement.Risk.Id = 0;
                companyJudgement.Risk.Number = endorsementOperation.RiskNumber.Value;
                companyJudgement.Risk.Coverages.ForEach(x => x.CoverageOriginalStatus = x.CoverStatus);
            }

            return companyJudgement;
        }
    }

}
