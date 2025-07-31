using Sistran.Company.Application.ReversionEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.SuretyReversionService.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Company.Application.SuretyReversionService.EEProvider.Assemblers;

namespace Sistran.Company.Application.SuretyReversionService.EEProvider
{
    public class SuretyReversionServiceEEProvider : CiaReversionEndorsementEEProvider, ISuretyReversionServiceCia
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy, bool clearPolicies)
        {
            try
            {
                SuretyReversionBusinessCia suretyReversionBusinessCia = new SuretyReversionBusinessCia();
                CompanyPolicy companyPolicy = suretyReversionBusinessCia.CreateTemporal(policy, clearPolicies);

                if (companyPolicy.InfringementPolicies == null || companyPolicy.InfringementPolicies.Count == 0)
                {
                    CreateEndorsementWorkFlow(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.TicketNumber.ToString(), Convert.ToDateTime(companyPolicy.TicketDate));
                    DelegateService.underwritingService.SaveTextLarge(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id);
                }

                return companyPolicy;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }  
    }
}