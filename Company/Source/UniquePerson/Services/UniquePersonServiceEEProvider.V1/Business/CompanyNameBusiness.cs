using Sistran.Company.Application.UniquePersonServices.V1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyNameBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;
        public CompanyCoCompanyName CreateCompanyIndividualTax(CompanyCoCompanyName companyIndividualTaxExeption)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperBusinessName();

                var IndividualTaxExeption = imapper.Map<CompanyCoCompanyName, CompanyName>(companyIndividualTaxExeption);
                var result = coreProvider.CreateBusinessName(IndividualTaxExeption);
                return imapper.Map<CompanyName, CompanyCoCompanyName>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
