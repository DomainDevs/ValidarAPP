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
    public class CompanyDocumentTypeBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyDocumentTypeBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyDocumentType> GetCompanyDocumentTypes(int typeDocument)
        {
            var imapper = ModelAssembler.CreateMapperDocumentType();
            var result = coreProvider.GetDocumentTypes(typeDocument);
            return imapper.Map<List<DocumentType>, List<CompanyDocumentType>>(result);
        }
    }
}
