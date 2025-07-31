using AutoMapper;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyCompanyBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreCompany;

        public CompanyCompanyBusiness()
        {
            coreCompany = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        
        public CompanyCompany CreateCompanyCompany(CompanyCompany companyCompany)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperCompany();
                var resultMapper = imapper.Map<CompanyCompany, Core.Application.UniquePersonService.V1.Models.Company>(companyCompany);
                var result = coreCompany.CreateCompany(resultMapper);
                return imapper.Map<Core.Application.UniquePersonService.V1.Models.Company, CompanyCompany>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

    }
}
