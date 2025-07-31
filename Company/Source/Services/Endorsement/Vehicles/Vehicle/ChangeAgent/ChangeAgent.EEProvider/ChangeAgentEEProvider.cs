using Sistran.Company.Application.ChangeAgentEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleChangeAgentService.EEProvider.Business;
using Sistran.Company.Application.VehicleChangeAgentService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.VehicleChangeAgentService.EEProvider
{
    public class VehicleChangeAgentServiceEEProvider : CiaChangeAgentEndorsementEEProvider, ICiaVehicleChangeAgentService
    {
        public CompanyPolicy CreateTemporal(CompanyPolicy companyPolicy, bool isMassive = false)
        {
            try
            {
                VehicleChangeAgentBusinessCia vehicleChangeAgentBusinessCia = new VehicleChangeAgentBusinessCia();
                return vehicleChangeAgentBusinessCia.CreateTemporal(companyPolicy, isMassive);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentVehicle);
            }
        }

        public List<CompanyPolicy> CreateEndorsementChangeAgent(CompanyEndorsement companyPolicy)
        {
            try
            {
                VehicleChangeAgentBusinessCia vehicleChangeAgentBusinessCia = new VehicleChangeAgentBusinessCia();
                return vehicleChangeAgentBusinessCia.CreateEndorsementChangeAgent(companyPolicy);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorCreateTemporalChangeAgentVehicle);
            }
        }

    }
}
