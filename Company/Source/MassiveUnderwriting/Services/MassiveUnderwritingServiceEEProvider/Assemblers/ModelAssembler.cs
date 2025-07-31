using AutoMapper;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Assemblers
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

        #region Clausulas
        public static IMapper CreateMapClause()
        {
            var config = MapperCache.GetMapper<CompanyClause, Clause>(cfg =>
            {
                cfg.CreateMap<CompanyClause, Clause>();
            });
            return config;
        }
        public static IMapper CreateMapCompanyClause()
        {
            var config = MapperCache.GetMapper<Clause, CompanyClause>(cfg =>
            {
                cfg.CreateMap<Clause, CompanyClause>();
            });
            return config;
        }
        #endregion

    }
}
