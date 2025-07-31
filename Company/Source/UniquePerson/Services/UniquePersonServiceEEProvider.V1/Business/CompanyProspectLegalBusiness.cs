using System;
using System.Collections.Generic;
using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyProspectLegalBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyProspectLegalBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal CompanyProspectNatural GetProspectPersonLegal(int individualId)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();
            var result = coreProvider.GetProspectLegalByIndividualId(individualId);
            return imapper.Map<ProspectNatural, CompanyProspectNatural>(result);
        }

        internal CompanyProspectNatural CreateProspectPersonLegal(CompanyProspectNatural companyProspectNatural)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();


            var listresult = coreProvider.GetProspectByDocumentNum(companyProspectNatural.TributaryIdNumber, (int)IndividualSearchType.ProspectusPerson);

            if (listresult.Count == 0)
            {
                var ProspectNaturalcore = imapper.Map<CompanyProspectNatural, ProspectNatural>(companyProspectNatural);
                var result = coreProvider.CreateProspectLegal(ProspectNaturalcore);
                return imapper.Map<ProspectNatural, CompanyProspectNatural>(result);
            }
            else
            {
                return null;
            }

        }

        internal CompanyProspectNatural UpdateProspectPersonLegal(CompanyProspectNatural companyProspectNatural)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();
            var ProspectNaturalcore = imapper.Map<CompanyProspectNatural, ProspectNatural>(companyProspectNatural);
            var result = coreProvider.UpdateProspectLegal(ProspectNaturalcore);
            return imapper.Map<ProspectNatural, CompanyProspectNatural>(result);
        }

        internal CompanyProspectNatural GetProspectLegalByDocumentNumber(string documentNum)
        {
            var imapper = ModelAssembler.CreateMapperPersonNatural();

            List<ProspectNatural> result = coreProvider.GetProspectLegalByDocument(documentNum);

            if (result.Count > 0)
            {
                if(result[0].CountryCode > 0 && result[0].StateCode >0 && result[0].CityCode > 0)
                {
                    City city = DelegateService.commonService.GetCityByCountryIdByStateIdByCityId((int)result[0].CountryCode, (int)result[0].StateCode, (int)result[0].CityCode);
                    result[0].City = city;
                }

                return imapper.Map<ProspectNatural, CompanyProspectNatural>(result[0]);
            }
            else
            {
                return new CompanyProspectNatural();
            }
        }
    }
}
