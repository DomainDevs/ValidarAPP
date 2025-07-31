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
    public class CompanyGuarantorBusiness
    {

        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyGuarantorBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyGuarantor> GetCompanyGuarantorByindividualIdByguaranteeId(int individualId, int guaranteeId)
        {
            var imapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var result = coreProvider.GetGuarantorByIndividualIdByGuaranteeId(individualId, guaranteeId);
            return imapper.Map<List<Guarantor>, List<CompanyGuarantor>>(result);
        }

        internal CompanyGuarantor CreateCompanyGuarator(CompanyGuarantor companyGuarantor)
        {
            var imap = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var CoreinsuredGuaranteeLog = imap.Map<CompanyGuarantor, Guarantor>(companyGuarantor);
            var result = coreProvider.CreateGuatantor(CoreinsuredGuaranteeLog);
            return imap.Map<Guarantor, CompanyGuarantor>(result);
        }

        internal CompanyGuarantor UpdateCompanyGuarantor(CompanyGuarantor companyGuarantor)
        {
            var mapper = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var CoreinsuredGuaranteeLog = mapper.Map<CompanyGuarantor, Guarantor>(companyGuarantor);
            var result = coreProvider.UpdateGuarantor(CoreinsuredGuaranteeLog);
            return mapper.Map<Guarantor, CompanyGuarantor>(result);
        }

        internal void DeleteCompanyGuarantor(CompanyGuarantor companyGuarantor)
        {
            var map = ModelAssembler.CreateMapperGuaranteeInsuredGuarantee();
            var CoreinsuredGuaranteeLog = map.Map<CompanyGuarantor, Guarantor>(companyGuarantor);
            coreProvider.DeleteGuarantor(CoreinsuredGuaranteeLog);
        }

    }
}
