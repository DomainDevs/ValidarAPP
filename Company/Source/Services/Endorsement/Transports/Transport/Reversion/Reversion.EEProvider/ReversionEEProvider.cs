using Sistran.Company.Application.ReversionEndorsement.EEProvider;
using Sistran.Company.Application.TranportReversionService.EEProvider.Assemblers;
using Sistran.Company.Application.TranportReversionService.EEProvider.Business;
using Sistran.Company.Application.TransportReversionService;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.TranportReversionService.EEProvider
{
    public class TransportReversionServiceEEProvider : CiaReversionEndorsementEEProvider, ITransportReversionService
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy , bool clearPolicies)
        {
            try
            {
                TranportReversionBusinessCia transportReversionBusinessCia = new TranportReversionBusinessCia();
                CompanyPolicy companyPolicy = transportReversionBusinessCia.CreateTemporal(policy, clearPolicies);

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
    }
}