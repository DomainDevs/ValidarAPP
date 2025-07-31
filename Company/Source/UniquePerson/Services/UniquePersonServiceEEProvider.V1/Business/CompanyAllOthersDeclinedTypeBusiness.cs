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
    public class CompanyAllOthersDeclinedTypeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyAllOthersDeclinedTypeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyAllOthersDeclinedType> GetCompanyAllOthersDeclinedTypes()
        {
            var imapper = ModelAssembler.CreateMapperAllOthersDeclinedType();
            var result = coreProvider.GetAllOthersDeclinedTypes();
            return imapper.Map<List<AllOthersDeclinedType>, List<CompanyAllOthersDeclinedType>>(result);
        }
    }
}
