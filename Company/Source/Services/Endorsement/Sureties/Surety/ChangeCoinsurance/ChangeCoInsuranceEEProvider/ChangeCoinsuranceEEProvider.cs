using Sistran.Company.Application.SuretyChangeCoinsuranceService.EEProvider.Business;
using Sistran.Company.Application.SuretyChangeCoinsuranceService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.SuretyChangeCoinsuranceService;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyChangeCoinsuranceService.EEProvider
{
    public class SuretyChangeCoinsuranceServiceEEProvider : ICiaSuretyChangeCoinsuranceService
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                SuretyChangeCoinsuranceBusinessCia suretyChangeCoinsuranceBusiness = new SuretyChangeCoinsuranceBusinessCia();
                return suretyChangeCoinsuranceBusiness.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeCoinsuranceSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeCoinsurance(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {

            try
            {
                SuretyChangeCoinsuranceBusinessCia suretyChangeCoinsuranceBusiness = new SuretyChangeCoinsuranceBusinessCia();
                return suretyChangeCoinsuranceBusiness.CreateEndorsementChangeCoinsurance(companyPolicy, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeCoinsuranceSurety);
            }

        }
    }
}
