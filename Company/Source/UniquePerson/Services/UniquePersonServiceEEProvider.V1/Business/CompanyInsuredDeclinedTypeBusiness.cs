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
    public class CompanyInsuredDeclinedTypeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyInsuredDeclinedTypeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyInsuredDeclinedType> GetCompanyInsuredDeclinedType()
        {
            var imapper = ModelAssembler.CreateMapperInsuredDeclinedType();
            var result = coreProvider.GetInsuredDeclinedTypes();
            return imapper.Map<List<InsuredDeclinedType>, List<CompanyInsuredDeclinedType>>(result);
        }
    }
}
