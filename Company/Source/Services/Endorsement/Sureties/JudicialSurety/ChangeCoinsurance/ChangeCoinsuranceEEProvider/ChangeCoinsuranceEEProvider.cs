using Sistran.Company.Application.JudicialSuretyChangeCoinsuranceService.EEProvider.Business;
using Sistran.Company.Application.JudicialSuretyChangeCoinsuranceService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.JudicialSuretyChangeCoinsuranceService.EEProvider
{
    public class JudicialSuretyChangeCoinsuranceServiceEEProvider : IJudicialSuretyChangeCoinsuranceServiceCompany
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                JudicialSuretyChangeCoinsuranceBusiness suretyChangeAgentBusinessCompany = new JudicialSuretyChangeCoinsuranceBusiness();
                return suretyChangeAgentBusinessCompany.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeCoinsuranceSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeCoinsurance(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {
            try
            {
                JudicialSuretyChangeCoinsuranceBusiness suretyChangeAgentBusinessCompany = new JudicialSuretyChangeCoinsuranceBusiness();
                return suretyChangeAgentBusinessCompany.CreateEndorsementChangeCoinsurance(companyPolicy, clearPolicies);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeCoinsuranceSurety);
            }
        }
    }
}
