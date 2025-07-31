using AutoMapper;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM = Sistran.Company.Application.UnderwritingServices.Models;
using RM = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.SuretyChangeAgentService.EEProvider.Assemblers
{
    public class ModelAssembler
    {

        public static List<CRM.CompanyRisk> CreateCompanyRisk(List<RM.Risk> risk)
        {
            if (risk == null)
            {
                throw new ArgumentNullException(nameof(risk));
            }
            var config = MapperCache.GetMapper<RM.Risk, CRM.CompanyRisk>(cfg =>
            {

                cfg.CreateMap<RM.Risk, CRM.CompanyRisk>();
                cfg.CreateMap<RM.LimitRc, CRM.CompanyLimitRc>();
                cfg.CreateMap<RM.RatingZone, CRM.CompanyRatingZone>();
                cfg.CreateMap<RM.Coverage, CRM.CompanyCoverage>();
                cfg.CreateMap<Insured, CRM.CompanyIssuanceInsured>();
                cfg.CreateMap<RM.Policy, CRM.CompanyPolicy>();
                cfg.CreateMap<RM.RiskActivity, CRM.CompanyRiskActivity>();
                cfg.CreateMap<RM.Text, CRM.CompanyText>();
                cfg.CreateMap<RM.Clause, CRM.CompanyClause>();
                cfg.CreateMap<RM.Beneficiary, CRM.CompanyBeneficiary>();
            });
            return config.Map<List<RM.Risk>, List<CRM.CompanyRisk>>(risk);
        }
    }
}
