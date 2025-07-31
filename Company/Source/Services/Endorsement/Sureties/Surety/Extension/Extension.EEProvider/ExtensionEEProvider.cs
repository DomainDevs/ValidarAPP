using Sistran.Company.Application.ExtensionEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyEndorsementExtensionService.EEProvider.Business;
using Sistran.Company.Application.SuretyEndorsementExtensionService.EEProvider.Services;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.SuretytExtensionService.EEProvider
{
    public class SuretyExtensionServiceEEProvider : ISuretyExtensionServiceCia
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
                ExtensionBusinessCia ExtensionBusinessCia = new ExtensionBusinessCia();
                return ExtensionBusinessCia.CreateEndorsementExtension(companyEndorsement);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
