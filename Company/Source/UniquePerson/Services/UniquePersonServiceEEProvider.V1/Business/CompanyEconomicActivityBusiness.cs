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
    public class CompanyEconomicActivityBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyEconomicActivityBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyEconomicActivity> GetCompanyEconomicActivity()
        {
            var imapper = ModelAssembler.CreateMapperEconomicActivity();
            var result = coreProvider.GetEconomicActivities();
            return imapper.Map<List<EconomicActivity>, List<CompanyEconomicActivity>>(result);
        }
    }
}
