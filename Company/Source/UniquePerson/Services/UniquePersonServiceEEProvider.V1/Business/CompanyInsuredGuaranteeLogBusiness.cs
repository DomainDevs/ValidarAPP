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
    public class CompanyInsuredGuaranteeLogBusiness
    {

        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyInsuredGuaranteeLogBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyInsuredGuaranteeLog> GetCompanyInsuredGuaranteeLogsByindividualIdByguaranteeId(int individualId, int guaranteeId)
        {
            var imapper = ModelAssembler.CreateMapperInsuredGuaranteelog();
            var result = coreProvider.GetInsuredGuaranteeLogByIndividualIdByGuaranteeId(individualId, guaranteeId);
            return imapper.Map<List<InsuredGuaranteeLog>, List<CompanyInsuredGuaranteeLog>>(result);
        }

        internal CompanyInsuredGuaranteeLog CreateCompanyInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog)
        {
            var imapper = ModelAssembler.CreateMapperInsuredGuaranteelog();
            var CoreinsuredGuaranteeLog = imapper.Map<CompanyInsuredGuaranteeLog, InsuredGuaranteeLog>(insuredGuaranteeLog);
            var result = coreProvider.CreateInsuredGuaranteeLog(CoreinsuredGuaranteeLog);
            return imapper.Map<InsuredGuaranteeLog, CompanyInsuredGuaranteeLog>(result);
        }

        internal CompanyInsuredGuaranteeLog UpdateCompanyInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog)
        {
            var imapper = ModelAssembler.CreateMapperInsuredGuaranteelog();
            var CoreinsuredGuaranteeLog = imapper.Map<CompanyInsuredGuaranteeLog, InsuredGuaranteeLog>(insuredGuaranteeLog);
            var result = coreProvider.UpdateInsuredGuaranteeLog(CoreinsuredGuaranteeLog);
            return imapper.Map<InsuredGuaranteeLog, CompanyInsuredGuaranteeLog>(result);
        }


    }
}
