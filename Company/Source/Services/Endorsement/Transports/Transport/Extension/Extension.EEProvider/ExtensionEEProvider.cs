using System;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.TransportExtensionService.EEProvider;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.ExtensionEndorsement.EEProvider;
using Sistran.Company.Application.TransportExtensionService.EEProvider.Business;

namespace Sistran.Company.Application.TransportExtensionService.EEProvider
{
    public class TransportExtensionServiceEEProvider : ICiaTransportsExtensionService
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
                TransportExtensionBusinessCia ExtensionBusinessCia = new TransportExtensionBusinessCia();
                return ExtensionBusinessCia.CreateEndorsementExtension(companyEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
