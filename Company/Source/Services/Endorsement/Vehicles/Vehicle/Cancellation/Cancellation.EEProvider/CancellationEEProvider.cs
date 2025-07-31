using Sistran.Company.Application.CancellationEndorsement.EEProvider;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleCancellationService.EEProvider.Business;
using Sistran.Company.Application.VehicleCancellationService.EEProvider.DAOs;
using Sistran.Company.Application.VehicleCancellationService.EEProvider.Resources;
using Sistran.Core.Framework.BAF;
using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;
using System;
using System.Collections.Generic;

namespace Sistran.Company.Application.VehicleCancellationService.EEProvider
{
    public class VehicleCancellationServiceEEProvider : CiaCancellationEndorsementEEProvider, ICiaVehicleCancellationService
    {
        /// <summary>
        /// Crear temporal de cancelacion
        /// </summary>
        /// <param name="policy">Modelo policy</param>
        /// <param name="cancellationFactor">factor de cancelacion</param>
        /// <returns>Id temporal</returns>
        public CompanyPolicy CreateTemporalEndorsementCancellation(CompanyEndorsement companyEndorsement)
        {
            try
            {
                VehicleCancellationBusinessCia vehicleCancellationBusinessCia = new VehicleCancellationBusinessCia();
                return vehicleCancellationBusinessCia.CreateTemporal(companyEndorsement);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPolicy ExecuteThread(List<VEM.CompanyVehicle> risksThread, CompanyPolicy policy, List<CompanyRisk> risks)
        {
            try
            {
                VehicleCancellationBusinessCia vehicleCancellationBusinessCia = new VehicleCancellationBusinessCia();
                return vehicleCancellationBusinessCia.ExecuteThread(risksThread, policy, risks);

            }
            catch (Exception)
            {

                throw new BusinessException(Errors.ErrorCreateTemporalCancellationVehicle);
            }
        }

        public List<CompanyRisk> CreateVehicleCancelation(CompanyPolicy companyPolicy, List<VEM.CompanyVehicle> companyVehicles, int cancellationFactor)
        {
            VehicleCancellationBusinessCia vehicleCancellationBusinessCia = new VehicleCancellationBusinessCia(); return vehicleCancellationBusinessCia.CreateVehicleCancelation(companyPolicy, companyVehicles, cancellationFactor);
        }
    }
}
