using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.PropertyChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.PropertyChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.PropertyChangeAgentService.EEProvider
{
    public class PropertyChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, IPropertyChangeAgentServiceCia
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                PropertyChangeAgentBusinessCia propertyChangeAgentBusinessCia = new PropertyChangeAgentBusinessCia();
                return propertyChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentProperty);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {

                PropertyChangeAgentBusinessCia propertyChangeAgentBusinessCia = new PropertyChangeAgentBusinessCia();
                return propertyChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentProperty);
            }
        }

    }
}
