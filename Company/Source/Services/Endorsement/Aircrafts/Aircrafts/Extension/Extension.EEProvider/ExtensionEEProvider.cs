using System;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.AircraftExtensionService.EEProvider;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.ExtensionEndorsement.EEProvider;
using Sistran.Company.Application.AircraftExtensionService.EEProvider.Business;

namespace Sistran.Company.Application.AircraftExtensionService.EEProvider
{
    public class AircraftExtensionServiceEEProvider : ICiaAircraftsExtensionService
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
                AircraftExtensionBusinessCia ExtensionBusinessCia = new AircraftExtensionBusinessCia();
                return ExtensionBusinessCia.CreateEndorsementExtension(companyEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
