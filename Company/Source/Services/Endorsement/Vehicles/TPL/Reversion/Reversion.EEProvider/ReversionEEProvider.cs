using Sistran.Company.Application.ReversionEndorsement.EEProvider.DAOs;
using Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.Assemblers;
using Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.ThirdPartyLiabilityReversionService.EEProvider
{
    public class ThirdPartyLiabilityReversionServiceEEProvider : IThirdPartyLiabilityReversionServiceCia
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
                ThirdPartyLiabilityReversionBusinessCia ThirdPartyLiabilityReversionBusinessCia = new ThirdPartyLiabilityReversionBusinessCia();
                CompanyPolicy companyPolicy = ThirdPartyLiabilityReversionBusinessCia.CreateTemporal(policy, clearPolicies);

                if (companyPolicy.InfringementPolicies == null || companyPolicy.InfringementPolicies.Count == 0)
                {
                    CreateEndorsementWorkFlow(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.TicketNumber.ToString(), Convert.ToDateTime(companyPolicy.TicketDate));
                    DelegateService.underwritingService.SaveTextLarge(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id);
                }
                return companyPolicy;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<string> GetEndorsementWorkFlow(int PolyciId)
        {
            try
            {
                ReversionDAO EndorsementWorkFlow = new ReversionDAO();
                return EndorsementWorkFlow.GetEndorsementWorkFlow(PolyciId);
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool CreateEndorsementWorkFlow(int? PolyciId, int? EndorsementId, string filingNumber, DateTime fiingDate)
        {
            try
            {
                ReversionDAO EndorsementWorkFlow = new ReversionDAO();
                bool result = EndorsementWorkFlow.CreateEndorsementWorkFlow(PolyciId, EndorsementId, filingNumber, fiingDate);
                return result;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
    }
}