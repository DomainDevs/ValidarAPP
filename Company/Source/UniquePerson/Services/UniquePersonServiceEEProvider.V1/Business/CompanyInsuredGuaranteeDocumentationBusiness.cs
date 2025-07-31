using System.Collections.Generic;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    class CompanyInsuredGuaranteeDocumentationBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyInsuredGuaranteeDocumentationBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }


        internal CompanyInsuredGuaranteeDocumentation CreateCompanyInsuredGuaranteeDocumentation(CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation)
        {
            var imapper = ModelAssembler.CreateCompanyInsuredGuarantee();
            var guaranteeDocumentation = imapper.Map<CompanyInsuredGuaranteeDocumentation, InsuredGuaranteeDocumentation>(companyInsuredGuaranteeDocumentation);
            var result = coreProvider.CreateInsuredGuaranteeDocumentation(guaranteeDocumentation);
            return imapper.Map<InsuredGuaranteeDocumentation, CompanyInsuredGuaranteeDocumentation>(result);
        }

        internal CompanyInsuredGuaranteeDocumentation UpdateCompanyInsuredGuaranteeDocumentation(CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation)
        {
            var imapper = ModelAssembler.CreateCompanyInsuredGuarantee();
            var guaranteeDocumentation = imapper.Map<CompanyInsuredGuaranteeDocumentation, InsuredGuaranteeDocumentation>(companyInsuredGuaranteeDocumentation);
            var result = coreProvider.UpdateInsuredGuaranteeDocumentation(guaranteeDocumentation);
            return imapper.Map<InsuredGuaranteeDocumentation, CompanyInsuredGuaranteeDocumentation>(result);
        }

        internal void DeleteCompanyInsuredGuaranteeDocumentation(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            ModelAssembler.CreateCompanyInsuredGuarantee();
            coreProvider.DeleteInsuredGuaranteeDocumentation(individualId, insuredguaranteeId, guaranteeId, documentId);
        }

        internal CompanyInsuredGuaranteeDocumentation GetCompanyInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            var imapper = ModelAssembler.CreateCompanyInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(individualId, insuredguaranteeId, guaranteeId, documentId);
            return imapper.Map<InsuredGuaranteeDocumentation, CompanyInsuredGuaranteeDocumentation>(result);
        }

        internal List<CompanyInsuredGuaranteeDocumentation> GetCompanyInsuredGuaranteeDocumentation()
        {
            var imapper = ModelAssembler.CreateCompanyInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeDocumentation();
            return imapper.Map<List<InsuredGuaranteeDocumentation>, List<CompanyInsuredGuaranteeDocumentation>>(result);
        }

        internal List<CompanyInsuredGuaranteeDocumentation> GetCompanyInsuredGuaranteeDocument(int individualId, int guaranteeId)
        {
            var imapper = ModelAssembler.CreateCompanyInsuredGuarantee();
            var result = coreProvider.GetInsuredGuaranteeDocument( individualId,  guaranteeId);
            return imapper.Map<List<InsuredGuaranteeDocumentation>, List<CompanyInsuredGuaranteeDocumentation>>(result);
        }

    }
}
