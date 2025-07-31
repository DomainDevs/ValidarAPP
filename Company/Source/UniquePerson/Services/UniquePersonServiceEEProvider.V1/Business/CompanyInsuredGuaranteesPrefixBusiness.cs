using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    class CompanyInsuredGuaranteesPrefixBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyInsuredGuaranteesPrefixBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }


        internal CompanyInsuredGuaranteePrefix CreateCompanyCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix companyInsuredGuaranteePrefix)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteePrefix = imapper.Map<CompanyInsuredGuaranteePrefix, InsuredGuaranteePrefix>(companyInsuredGuaranteePrefix);
            var result = coreProvider.CreateInsuredGuaranteePrefix(insuredGuaranteePrefix);
            return imapper.Map<InsuredGuaranteePrefix, CompanyInsuredGuaranteePrefix>(result);
        }

        internal CompanyInsuredGuaranteePrefix UpdateCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix companyInsuredGuaranteePrefix)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var insuredGuaranteePrefix = imapper.Map<CompanyInsuredGuaranteePrefix, InsuredGuaranteePrefix>(companyInsuredGuaranteePrefix);
            var result = coreProvider.UpdateInsuredGuaranteePrefix(insuredGuaranteePrefix);
            return imapper.Map<InsuredGuaranteePrefix, CompanyInsuredGuaranteePrefix>(result);
        }

        internal void DeleteCompanyInsuredGuaranteePrefix(int individualId, int guaranteeId, int prefixId)
        {
            ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            coreProvider.DeleteInsuredGuaranteePrefix(individualId, guaranteeId, prefixId);
        }

        internal List<CompanyInsuredGuaranteePrefix> GetCompanyInsuredGuaranteePrefix(int individualId, int guaranteeId)
        {
            var imapper = ModelAssembler.CreateCompanyInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteePrefix(individualId, guaranteeId);
            return imapper.Map<List<InsuredGuaranteePrefix>, List<CompanyInsuredGuaranteePrefix>>(result);
        }
    }
}
