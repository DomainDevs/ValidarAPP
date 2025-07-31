using Sistran.Company.Application.LiabilityChangeCoinsuranceService.EEProvider.Business;
using Sistran.Company.Application.LiabilityChangeCoinsuranceService.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingServices;

namespace Sistran.Company.Application.LiabilityChangeCoinsuranceService.EEProvider
{
    public class LiabilityChangeCoinsuranceServiceEEProvider : ILiabilityChangeCoinsuranceServiceCia
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                LiabilityChangeCoinsuranceBusinessCia liabilityChangeCoinsuranceBusinessCia = new LiabilityChangeCoinsuranceBusinessCia();
                return liabilityChangeCoinsuranceBusinessCia.CreateTemporal(companyPolicy, isMassive);
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
                LiabilityChangeCoinsuranceBusinessCia liabilityChangeCoinsuranceBusinessCia = new LiabilityChangeCoinsuranceBusinessCia();
                return liabilityChangeCoinsuranceBusinessCia.CreateEndorsementChangeCoinsurance(companyPolicy, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeCoinsuranceSurety);
            }
        }
    }
}
