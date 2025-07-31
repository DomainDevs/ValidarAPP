using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.LiabilityChangeTermService.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.LiabilityChangeTermService.EEProvider.Resources;
using System.Linq;
using System.Collections.Generic;

namespace Sistran.Company.Application.LiabilityChangeTermService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.LiabilityChangeTermService.ILiabilityChangeTermService" />
    public class LiabilityChangeTermServiceEEProvider : ILiabilityChangeTermServiceCompany
    {
        /// <summary>
        /// Creates the texts.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="clearPolicies">If validate Policies</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateChangeTerms(CompanyEndorsement companyEndorsement, bool clearPolicies = false)
        {
            try
            {
                LiabilityChangeTermBusinessCompany LiabilityChangeTermBusinessCompany = new LiabilityChangeTermBusinessCompany();
                return LiabilityChangeTermBusinessCompany.CreateEndorsementChangeTerm(companyEndorsement, clearPolicies).FirstOrDefault();
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                LiabilityChangeTermBusinessCompany LiabilityChangeTermBusinessCompany = new LiabilityChangeTermBusinessCompany();
                return LiabilityChangeTermBusinessCompany.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentLiability);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeTerm(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {
            try
            {
                LiabilityChangeTermBusinessCompany LiabilityChangeTermBusinessCompany = new LiabilityChangeTermBusinessCompany();
                return LiabilityChangeTermBusinessCompany.CreateEndorsementChangeTerm(companyPolicy, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentLiability);
            }
        }

    }
}
