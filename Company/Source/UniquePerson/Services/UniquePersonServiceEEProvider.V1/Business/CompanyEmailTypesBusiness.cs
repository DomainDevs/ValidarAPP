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
    public class CompanyEmailTypesBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyEmailTypesBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyEmailType> GetCompanyEmailTypes()
        {
            var imapper = ModelAssembler.CreateMapperEmailType();
            var result = coreProvider.GetEmailTypes();
            return imapper.Map<List<EmailType>, List<CompanyEmailType>>(result);
        }
    }
}
