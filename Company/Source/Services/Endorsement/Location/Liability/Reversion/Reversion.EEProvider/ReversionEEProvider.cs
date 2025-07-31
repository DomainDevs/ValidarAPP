using Sistran.Company.Application.LiabilityReversionService.EEProvider.Business;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System.Collections.Generic;
using Sistran.Company.Application.ReversionEndorsement.EEProvider.DAOs;
using System;
using Sistran.Company.Application.LiabilityReversionService.EEProvider.Assemblers;

namespace Sistran.Company.Application.LiabilityReversionService.EEProvider
{
    public class LiabilityReversionServiceEEProvider : ILiabilityReversionServiceCia
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
                LiabilityReversionBusinessCia LiabilityReversionBusinessCia = new LiabilityReversionBusinessCia();
                CompanyPolicy companyPolicy = LiabilityReversionBusinessCia.CreateTemporal(policy, clearPolicies);

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