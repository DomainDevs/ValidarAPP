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
    public class CompanyMaritalStatusBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyMaritalStatusBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyMaritalStatus> GetCompanyMaritalStatus()
        {
            var imapper = ModelAssembler.CreateMapperMaritalStatus();
            var result = coreProvider.GetMaritalStatus();
            return imapper.Map<List<MaritalStatus>, List<CompanyMaritalStatus>>(result);
        }
    }
}
