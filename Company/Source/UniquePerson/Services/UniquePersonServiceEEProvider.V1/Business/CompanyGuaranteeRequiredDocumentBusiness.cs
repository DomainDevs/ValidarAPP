using System.Collections.Generic;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;


namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    class CompanyGuaranteeRequiredDocumentBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyGuaranteeRequiredDocumentBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyGuaranteeRequiredDocument> GetCompanyInsuredGuaranteeRequiredDocumentation(int guaranteeId)
        {
            var imapper = ModelAssembler.CreateGuaranteeRequiredDocument();
            var result = coreProvider.GetDocumentationReceivedByGuaranteeId(guaranteeId);
            return imapper.Map<List<GuaranteeRequiredDocument>, List<CompanyGuaranteeRequiredDocument>>(result);
        }
    }
}
