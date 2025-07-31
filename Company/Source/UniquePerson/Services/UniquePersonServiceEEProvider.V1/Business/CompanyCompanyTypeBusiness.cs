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
    public class CompanyCompanyTypeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyCompanyTypeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyCompanyType> GetCompanyCompanyType()
        {
            var imapper = ModelAssembler.CreateMapperCompanyType();
            var result = coreProvider.GetCompanyTypes();
            return imapper.Map<List<CompanyType>, List<CompanyCompanyType>>(result);
        }
        
    }
}
