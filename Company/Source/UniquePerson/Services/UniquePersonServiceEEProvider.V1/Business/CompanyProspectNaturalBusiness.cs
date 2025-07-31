using System;
using System.Collections.Generic;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyProspectNaturalBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyProspectNaturalBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal CompanyProspectNatural GetProspectPersonNatural(int individualId)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();
            var result = coreProvider.GetProspectNaturalByIndividualId(individualId);
            return imapper.Map<ProspectNatural, CompanyProspectNatural>(result);
        }

        internal CompanyProspectNatural CreateProspectPersonNatural(CompanyProspectNatural companyProspectNatural)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();

            var listresult = coreProvider.GetProspectByDocumentNum(companyProspectNatural.IdCardNo, (int)IndividualSearchType.ProspectusPerson);

            if (listresult.Count == 0)
            {
                var ProspectNaturalcore = imapper.Map<CompanyProspectNatural, ProspectNatural>(companyProspectNatural);
                var result = coreProvider.CreateProspectNatural(ProspectNaturalcore);
                return imapper.Map<ProspectNatural, CompanyProspectNatural>(result);
            }
            else
            {
                return null;
            }
        }

        internal CompanyProspectNatural UpdateProspectPersonNatural(CompanyProspectNatural companyProspectNatural)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();
            var ProspectNaturalcore = imapper.Map<CompanyProspectNatural, ProspectNatural>(companyProspectNatural);
            var result = coreProvider.UpdateProspectNatural(ProspectNaturalcore);
            return imapper.Map<ProspectNatural, CompanyProspectNatural>(result);
        }

        internal CompanyProspectNatural GetProspectByDocumentNumber(string documentNum, int searchType)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();

            var result = coreProvider.GetProspectByDocumentNum(documentNum, searchType);

            if (result.Count > 0)
            {
                return imapper.Map<ProspectNatural, CompanyProspectNatural>(result[0]);
            }
            else
            {
                return new CompanyProspectNatural();
            }

        }

        internal CompanyProspectNatural GetProspectNaturalByDocumentNumber(string documentNum)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();

            var result = coreProvider.GetProspectNaturalByDocument(documentNum);

            if (result.Count > 0)
            {
                return imapper.Map<ProspectNatural, CompanyProspectNatural>(result[0]);
            }
            else
            {
                return new CompanyProspectNatural();
            }
        }
    }
}
