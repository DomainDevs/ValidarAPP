using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class CompanyFiscalResponsibilityBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public CompanyFiscalResponsibilityBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }

        internal List<CompanyFiscalResponsibility> GetCompanyCompanyFiscalResponsibility()
        {
            var imapper = ModelAssembler.CreateMapperCompanyFiscalResponsibility();
            var result = coreProvider.GetFiscalResponsibility();
            return imapper.Map<List<FiscalResponsibility>, List<CompanyFiscalResponsibility>>(result);
        }

        /// <summary>
        /// Delete el ramo para la persona.
        /// </summary>
        /// <returns></returns>
        public bool DeleteCompanyInsuredFiscalResponsibility(CompanyInsuredFiscalResponsibility companyFiscal)
        {
            try
            {
                var imap = ModelAssembler.CreateMapperCompanyInsuredFiscalResponsibility();
                var fiscalCore = imap.Map<CompanyInsuredFiscalResponsibility, InsuredFiscalResponsibility>(companyFiscal);
                var result = coreProvider.DeleteFiscalResponsibility(fiscalCore);
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
    
}
