using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.CollectiveServices.EEProvider.Assemblers
{
    class ModelAssembler
    {
        public static CompanyAcceptCoInsuranceAgent MapCompanyAcceptCoInsuranceAgentFromAgency(IssuanceAgency issuanceAgency)
        {
            CompanyAcceptCoInsuranceAgent companyAcceptCoInsuranceAgent = new CompanyAcceptCoInsuranceAgent()
            {

                Agent = new CompanyPolicyAgent()
                {
                    IndividualId = issuanceAgency.Agent.IndividualId,
                    Id = issuanceAgency.Id,
                    FullName = issuanceAgency.Agent.FullName
                },
                ParticipationPercentage = issuanceAgency.Participation

            };
            return companyAcceptCoInsuranceAgent;
        }
        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config;
        }
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
    }
}
