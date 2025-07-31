using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.ModelServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using commonEntitiesCore = Sistran.Core.Application.Common.Entities;
namespace Sistran.Company.Application.CommonParamService.EEProvider.Assemblers
{
    public static class ModelAssembler
    {

        public static List<LineBusiness> CreateLinesBusiness(BusinessCollection businessCollection)
        {
            List<LineBusiness> linesBusiness = new List<LineBusiness>();

            foreach (commonEntitiesCore.LineBusiness entity in businessCollection)
            {
                linesBusiness.Add(ModelAssembler.CreateLineBusiness(entity));
            }

            return linesBusiness;
        }

        public static LineBusiness CreateLineBusiness(commonEntitiesCore.LineBusiness lineBusiness)
        {
            return new LineBusiness
            {
                Id = lineBusiness.LineBusinessCode,
                Description = lineBusiness.Description,
                ShortDescription = lineBusiness.SmallDescription,
                TyniDescription = lineBusiness.TinyDescription,
                ReportLineBusiness = int.Parse(lineBusiness.ReportLineBusinessCode)
            };
        }

        public static CompanyPrefix CreatePrefix(commonEntitiesCore.Prefix entityPrefix)
        {
            return new CompanyPrefix
            {
                Id = entityPrefix.PrefixCode,
                Description = entityPrefix.Description,
                SmallDescription = entityPrefix.SmallDescription,
                TinyDescription = entityPrefix.TinyDescription,
                PrefixType = new PrefixType { Id = entityPrefix.PrefixTypeCode },
                PrefixTypeCode = entityPrefix.PrefixTypeCode,
                HasDetailCommiss = entityPrefix.HasDetailCommiss

            };
        }

        public static CompanyAditionalInformationPrefix CreateAditionalInformationPrefix(CptPrefixScore entityPrefixScore, CptPrefix entityPrefix)
        {
            CompanyAditionalInformationPrefix aditionalInformationPrefix = null;
            if (entityPrefixScore != null)
            {
                aditionalInformationPrefix = new CompanyAditionalInformationPrefix();
                aditionalInformationPrefix.IsScore = entityPrefixScore.IsScore;
                aditionalInformationPrefix.Score = entityPrefixScore.DayValidate;
            }
            if (entityPrefix != null)
            {
                if (aditionalInformationPrefix == null)
                    aditionalInformationPrefix = new CompanyAditionalInformationPrefix();
                aditionalInformationPrefix.IsAlliance = entityPrefix.EnableAlliance;
                aditionalInformationPrefix.IsMassive = entityPrefix.IsMassive;
                aditionalInformationPrefix.Quote = entityPrefix.ValidDaysTmpQuote;
                aditionalInformationPrefix.Temporal = entityPrefix.ValidDaysTmpPolicy;
            }
            return aditionalInformationPrefix;
        }
    }
}
