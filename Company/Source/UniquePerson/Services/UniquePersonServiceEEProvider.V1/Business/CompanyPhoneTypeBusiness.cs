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
    public class CompanyPhoneTypeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyPhoneTypeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyPhoneType> GetCompanyPhoneType()
        {
            var imapper = ModelAssembler.CreateMapperPhoneType();
            var result = coreProvider.GetPhoneTypes();
            return imapper.Map<List<PhoneType>, List<CompanyPhoneType>>(result);
        }
    }
}
