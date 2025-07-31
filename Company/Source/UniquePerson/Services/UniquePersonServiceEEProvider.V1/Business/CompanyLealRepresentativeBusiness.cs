using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyLealRepresentativeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyLealRepresentativeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal CompanyLegalRepresentative CreateLegalRepresentative(CompanyLegalRepresentative legalRepresent, int individualId)
        {
            var imapper = ModelAssembler.CreateMapperLegalRepresentative();
            var legalRepresentcore = imapper.Map<CompanyLegalRepresentative, LegalRepresentative>(legalRepresent);

            LegalRepresentative result;
            if (GetLegalRepresentativeByIndividualId(individualId) != null)
            {
                result = coreProvider.UpdateLegalRepresent(legalRepresentcore, individualId);
            }
            else
            {
                result = coreProvider.CreateLegalRepresent(legalRepresentcore, individualId);
            }
            return imapper.Map<LegalRepresentative, CompanyLegalRepresentative>(result);
        }

        public CompanyLegalRepresentative GetLegalRepresentativeByIndividualId(int individualId)
        {
            var imapper = ModelAssembler.CreateMapperLegalRepresentative();
            var result = coreProvider.GetLegalRepresentByIndividualId(individualId);
            return imapper.Map<LegalRepresentative, CompanyLegalRepresentative>(result);
        }
    }
}
