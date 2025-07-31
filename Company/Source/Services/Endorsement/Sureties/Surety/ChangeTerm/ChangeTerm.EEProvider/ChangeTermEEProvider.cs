using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyChangeTermService.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.SuretyChangeTermService.EEProvider.Resources;
using System.Linq;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyChangeTermService.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Sistran.Company.Application.SuretyChangeTermService.ISuretyChangeTermService" />
    public class SuretyChangeTermServiceEEProvider : ISuretyChangeTermServiceCompany
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
                SuretyChangeTermBusinessCompany suretyChangeTermBusinessCompany = new SuretyChangeTermBusinessCompany();
                return suretyChangeTermBusinessCompany.CreateEndorsementChangeTerm(companyEndorsement, clearPolicies).FirstOrDefault();
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
                SuretyChangeTermBusinessCompany suretyChangeTermBusinessCompany = new SuretyChangeTermBusinessCompany();
                return suretyChangeTermBusinessCompany.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeTerm(CompanyEndorsement companyPolicy, bool clearPolicies = false)
        {
            try
            {
                SuretyChangeTermBusinessCompany suretyChangeTermBusinessCompany = new SuretyChangeTermBusinessCompany();
                return suretyChangeTermBusinessCompany.CreateEndorsementChangeTerm(companyPolicy, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentSurety);
            }
        }

    }
}
