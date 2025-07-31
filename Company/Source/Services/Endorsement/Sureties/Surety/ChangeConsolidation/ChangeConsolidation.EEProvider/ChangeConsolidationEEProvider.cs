using System;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider.Business;
using Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider.Resources;
using Sistran.Company.Application.Sureties.SuretyServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.SuretyChangeConsolidationService.EEProvider
{
    public class SuretyChangeConsolidationServiceEEProvider : ICiaSuretyChangeConsolidationService
    {
        public CompanyChangeConsolidation CreateTemporal(CompanyChangeConsolidation companyChangeConsolidation, bool isMassive = false)
        {
            try
            {
                SuretyChangeConsolidationBusinessCia suretyConsolidationChangeBusinessCia = new SuretyChangeConsolidationBusinessCia();
                return suretyConsolidationChangeBusinessCia.CreateTemporal(companyChangeConsolidation, isMassive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalConsolidationChangeSurety);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeConsolidation(CompanyChangeConsolidation companyChangeConsolidation, bool clearPolicies = false)
        {
            try
            {
                SuretyChangeConsolidationBusinessCia suretyConsolidationChangeBusinessCia = new SuretyChangeConsolidationBusinessCia();
                return suretyConsolidationChangeBusinessCia.CreateEndorsementChangeConsolidation(companyChangeConsolidation, clearPolicies);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalConsolidationChangeSurety);
            }
        }

        public CompanyContract GetCompanyContractByTemporalId(int temporalId, bool isMasive)
        {
            try
            {
                SuretyChangeConsolidationBusinessCia suretyConsolidationChangeBusinessCia = new SuretyChangeConsolidationBusinessCia();
                return suretyConsolidationChangeBusinessCia.GetCompanyContractByTemporalId(temporalId,isMasive);
            }
            catch (Exception)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalConsolidationChangeSurety);
            }
        }

    }
}
