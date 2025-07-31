using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.ModelServices.Models;
using Sistran.Core.Application.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CommonParamService.EEProvider.Assemblers
{
    public class EntityAssembler
    {

        /// <summary>
        /// Crea el objeto CptPrefix
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="cptPrefix"></param>
        public static void CreateCptPrefix(CompanyPrefix prefix, ref CptPrefix cptPrefix)
        {
            if (prefix.AditionalInformation != null)
            {
                if (cptPrefix == null)
                    cptPrefix = new CptPrefix(prefix.Id);
                cptPrefix.EnableAlliance = prefix.AditionalInformation.IsAlliance;
                cptPrefix.IsMassive = prefix.AditionalInformation.IsMassive;
                cptPrefix.IsIssueR2 = prefix.AditionalInformation.IsIssueR2;
                cptPrefix.ValidDaysTmpPolicy = prefix.AditionalInformation.Temporal;
                cptPrefix.ValidDaysTmpQuote = prefix.AditionalInformation.Quote;
            }
        }

        /// <summary>
        /// Crea el objeto CptPrefixScore
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="cptPrefixScore"></param>
        public static void CreateCptPrefixScore(CompanyPrefix prefix, ref CptPrefixScore cptPrefixScore)
        {
            if (prefix.AditionalInformation != null)
            {
                if (cptPrefixScore == null)
                    cptPrefixScore = new CptPrefixScore(prefix.Id);
                cptPrefixScore.DayValidate = prefix.AditionalInformation.Score;
                cptPrefixScore.IsScore = prefix.AditionalInformation.IsScore;
                cptPrefixScore.UserId = prefix.UserId;
                cptPrefixScore.Date = DateTime.Now;
            }
        }

        public static Prefix CreatePrefix(CompanyPrefix Prefix)
        {
            return new Prefix(0)
            {
                PrefixCode = Prefix.Id,
                Description = Prefix.Description,
                SmallDescription = Prefix.SmallDescription,
                TinyDescription = Prefix.TinyDescription,
                PrefixTypeCode = Prefix.PrefixTypeCode,
                HasDetailCommiss = Prefix.HasDetailCommiss
            };
        }
    }
}
