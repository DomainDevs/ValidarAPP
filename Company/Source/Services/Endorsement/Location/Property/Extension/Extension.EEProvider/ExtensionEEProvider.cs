using Sistran.Company.Application.ExtensionEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.PropertyEndorsementExtensionService.EEProvider.Business;
using Sistran.Company.Application.PropertyEndorsementExtensionService.EEProvider.Services;


using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.PropertytExtensionService.EEProvider
{
    public class PropertyExtensionServiceEEProvider : IPropertyExtensionServiceCia
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

        public bool ValidateDeclarativeInsuredObjects(decimal policyId)
        {
            try
            {
                ExtensionBusinessCia ExtensionBusinessCia = new ExtensionBusinessCia();
                return ExtensionBusinessCia.ValidateDeclarativeInsuredObjects(policyId);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
