using Sistran.Company.Application.ReversionEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleReversionService.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.VehicleReversionService.EEProvider
{
    using Assemblers;

    public class VehicleReversionServiceEEProvider : CiaReversionEndorsementEEProvider, IVehicleReversionService
    {
        /// <summary>
        /// Crear temporal de anulacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="clearPolicies"></param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateEndorsementReversion(CompanyEndorsement policy, bool clearPolicies)
        {
            try
            {
                VehicleReversionBusinessCia vehicleReversionBusinessCia = new VehicleReversionBusinessCia();
                CompanyPolicy companyPolicy = vehicleReversionBusinessCia.CreateTemporal(policy, clearPolicies);
                companyPolicy.TicketDate = policy.TicketDate;
                companyPolicy.TicketNumber = policy.TicketNumber;

                if (companyPolicy.InfringementPolicies == null || companyPolicy.InfringementPolicies.Count == 0)
                {
                    this.CreateEndorsementWorkFlow(companyPolicy.Endorsement.PolicyId, companyPolicy.Endorsement.Id, companyPolicy.TicketNumber.ToString(), Convert.ToDateTime(companyPolicy.TicketDate));
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