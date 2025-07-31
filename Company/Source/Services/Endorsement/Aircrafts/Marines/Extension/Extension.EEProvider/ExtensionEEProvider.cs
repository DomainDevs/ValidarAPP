using System;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.MarineExtensionService.EEProvider;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.ExtensionEndorsement.EEProvider;
using Sistran.Company.Application.MarineExtensionService.EEProvider.Business;

namespace Sistran.Company.Application.MarineExtensionService.EEProvider
{
    public class MarineExtensionServiceEEProvider : ICiaMarinesExtensionService
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
                MarineExtensionBusinessCia ExtensionBusinessCia = new MarineExtensionBusinessCia();
                return ExtensionBusinessCia.CreateEndorsementExtension(companyEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
