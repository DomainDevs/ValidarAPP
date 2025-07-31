using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.ModelsAutoMapper;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Enums;
using Sistran.Core.Application.Sureties.JudicialSuretyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Sureties.JudicialSuretyServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        /// <summary>
        /// Creates the court.
        /// </summary>
        /// <param name="court">The court.</param>
        /// <returns></returns>
        public static Court CreateCourt(COMMEN.Court court)
        {
            var config = AutoMapperAssembler.CreateMapCourt();
            return config.Map<COMMEN.Court, Court>(court);
        }

        public static List<Court> CreateCourt(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapCourt();
            return mapper.Map<List<COMMEN.Court>, List<Court>>(businessCollection.Cast<COMMEN.Court>().ToList());
        }

        public static Article CreateArticle(COMMEN.Article article)
        {
            var config = AutoMapperAssembler.CreateMapArticle();
            return config.Map<COMMEN.Article, Article>(article);
        }

        public static List<Article> CreateArticle(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapArticle();
            return mapper.Map<List<COMMEN.Article>, List<Article>>(businessCollection.Cast<COMMEN.Article>().ToList());
        }

        public static List<ArticleLine> CreateArticleLine(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapArticleLine();
            return mapper.Map<List<COMMEN.ArticleLine>, List<ArticleLine>>(businessCollection.Cast<COMMEN.ArticleLine>().ToList());
        }

        public static Judgement CreateJudgement(Judgement riskJudgementModel)
        {
            return new Judgement
            {


            };
        }

        //public static Judgement CreateJudicialSurety(ISSEN.Risk risk, ISSEN.RiskJudicialSurety RiskJudicialSurety, ISSEN.EndorsementRisk endorsementRisk, ISSEN.Endorsement endorsement)
        //{

        //    Judgement companyJudgement = new Judgement
        //    {
        //        Risk = new Risk
        //        {
        //            RiskId = risk.RiskId,
        //            Number = endorsementRisk.RiskNum,
        //            RatingZone = new RatingZone
        //            {
        //                Id = risk.RatingZoneCode.GetValueOrDefault()
        //            },
        //            CoveredRiskType = (CoveredRiskType)risk.CoveredRiskTypeCode,
        //            GroupCoverage = new GroupCoverage
        //            {
        //                Id = risk.CoverGroupId.GetValueOrDefault()
        //            },
        //            Status = RiskStatusType.NotModified,
        //            AmountInsured = RiskJudicialSurety.InsuredValue.GetValueOrDefault(),
        //            DynamicProperties = new List<DynamicConcept>()
        //        }
        //    };

        //    companyJudgement.Risk.MainInsured = new IssuanceInsured
        //    {
        //        IndividualId = risk.InsuredId,
        //        CompanyName = new IssuanceCompanyName
        //        {
        //            NameNum = risk.NameNum.GetValueOrDefault(),
        //            Address = new IssuanceAddress
        //            {
        //                Id = risk.AddressId.GetValueOrDefault()
        //            }
        //        }
        //    };

        //    var country = new Country
        //    {
        //        Id = RiskJudicialSurety.CountryCode.GetValueOrDefault()
        //    };
        //    var state = new State
        //    {
        //        Id = RiskJudicialSurety.StateCode.GetValueOrDefault(),
        //        Country = country
        //    };
        //    companyJudgement.City = new City
        //    {
        //        Id = RiskJudicialSurety.CityCode.GetValueOrDefault(),
        //        State = state
        //    };
        //    companyJudgement.Article = new Article
        //    {
        //        Id = RiskJudicialSurety.ArticleCode ?? 0
        //    };
        //    companyJudgement.Court = new Court
        //    {
        //        Id = RiskJudicialSurety.CourtCode ?? 0
        //    };
        //    companyJudgement.SettledNumber = RiskJudicialSurety.BidNumber;
        //    companyJudgement.InsuredValue = RiskJudicialSurety.InsuredValue.GetValueOrDefault();
        //    companyJudgement.HolderActAs = endorsement.CapacityOfCode == null ? 0 : (CapacityOf)endorsement.CapacityOfCode;
        //    companyJudgement.InsuredActAs = RiskJudicialSurety.CapacityOfCode == null ? 0 : (CapacityOf)RiskJudicialSurety.CapacityOfCode;
        //    companyJudgement.Risk.OriginalStatus = (RiskStatusType)endorsementRisk.RiskStatusCode;
        //    companyJudgement.Attorney = new Attorney();
        //    if (RiskJudicialSurety.IdCardNo != null && RiskJudicialSurety.IdCardTypeCode.HasValue)
        //    {
        //        companyJudgement.Attorney.IdentificationDocument = new IdentificationDocument { Number = RiskJudicialSurety.IdCardNo, DocumentType = new DocumentType { Id = RiskJudicialSurety.IdCardTypeCode.Value } };
        //    }
        //    if (RiskJudicialSurety.ProfessionalCardNum != null)
        //    {
        //        companyJudgement.Attorney.IdProfessionalCard = RiskJudicialSurety.ProfessionalCardNum;
        //    }
        //    if (RiskJudicialSurety.InsuredPrintName != null)
        //    {
        //        companyJudgement.Attorney.Name = RiskJudicialSurety.InsuredPrintName;
        //    }
        //    if (RiskJudicialSurety.InsuredId != null && RiskJudicialSurety.InsuredId.HasValue)
        //    {
        //        companyJudgement.Risk.SecondInsured = new IssuanceInsured { IndividualId = RiskJudicialSurety.InsuredId.Value, Name = RiskJudicialSurety.InsuredPrintName };

        //    }
        //    if (risk.DynamicProperties != null && risk.DynamicProperties.Count > 0)
        //    {
        //        foreach (DynamicProperty item in risk.DynamicProperties)
        //        {
        //            DynamicProperty dynamicProperty = (DynamicProperty)item.Value;
        //            DynamicConcept dynamicConcept = new DynamicConcept();
        //            dynamicConcept.Id = dynamicProperty.Id;
        //            dynamicConcept.Value = dynamicProperty.Value;
        //            companyJudgement.Risk.DynamicProperties.Add(dynamicConcept);
        //        }
        //    }
        //    return companyJudgement;
        //}

        public static Judgement CreateJudicialSurety(JudicialSuretyMapper judicialSuretyMapper)
        {
            var mapper = AutoMapperAssembler.CreateMapJudicialSurety();
            var mapperJudgement = mapper.Map<JudicialSuretyMapper, Judgement>(judicialSuretyMapper);

            foreach (DynamicConcept item in mapperJudgement.Risk.DynamicProperties)
            {
                DynamicConcept dynamicConcept = new DynamicConcept();
                dynamicConcept.Id = item.Id;
                dynamicConcept.Value = item.Value;
                mapperJudgement.Risk.DynamicProperties.Add(dynamicConcept);
            }

            return mapperJudgement;
        }

    }
}
