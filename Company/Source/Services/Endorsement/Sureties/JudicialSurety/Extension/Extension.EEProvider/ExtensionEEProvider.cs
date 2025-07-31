using Sistran.Company.Application.JudicialSuretytExtensionService;
using Sistran.Company.Application.JudicialSuretyEndorsementExtensionService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.JudicialSuretytExtensionService.EEProvider
{
    public class JudicialSuretyExtensionServiceEEProvider : IJudicialSuretyExtensionServiceCompany
    {
        /// <summary>
        /// Creacion prorroga
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateExtension(CompanyPolicy companyEndorsement)
        {
            try
            {
                ExtensionBusinessCompany ExtensionBusinessCompany = new ExtensionBusinessCompany();
                return ExtensionBusinessCompany.CreateEndorsementExtension(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
