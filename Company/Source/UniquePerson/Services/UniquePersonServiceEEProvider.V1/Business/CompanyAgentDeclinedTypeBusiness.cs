using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyAgentDeclinedTypeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyAgentDeclinedTypeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyAgentDeclinedType> GetCompanyAgentDeclinedTypes()
        {
            var imapper = ModelAssembler.CreateMapperAgentDeclinedType();
            var result = coreProvider.GetAgentDeclinedTypes();
            return imapper.Map<List<AgentDeclinedType>, List<CompanyAgentDeclinedType>>(result);
        }
    }
}
