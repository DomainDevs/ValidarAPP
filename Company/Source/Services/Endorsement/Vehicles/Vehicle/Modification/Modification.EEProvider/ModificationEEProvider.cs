using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleModificationService.EEProvider.Business;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.VehicleEndorsementModificationService.EEProvider;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.VehicleModificationService.EEProvider
{
    public class VehicleModificationServiceEEProviderCia : VehicleModificationServiceEEProvider, IVehicleModificationServiceCia
    {

        /// <summary>
        /// Creacion Temporal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <param name="isMassive">if set to <c>true</c> [is massive].</param>
        /// <returns></returns> 
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive)
        {
            try
            {
                return CreateTemporal(companyEndorsement, isMassive, false);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Creacion Temporal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        public CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive, bool isSaveTemp)
        {
            try
            {
                VehicleEndorsementBusinessCia endorsementBusinessCia = new VehicleEndorsementBusinessCia();
                return endorsementBusinessCia.CreatePolicyTemporal(companyEndorsement, isMassive, isSaveTemp);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }

        }

        /// <summary>
        /// Gets the data modification.
        /// </summary>
        /// <param name="risk">The risk.</param>
        /// <param name="coverageStatusType">Type of the coverage status.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyVehicle GetDataModification(CompanyVehicle risk, CoverageStatusType coverageStatusType)
        {
            try
            {
                VehicleEndorsementBusinessCia endorsementBusinessCia = new VehicleEndorsementBusinessCia();
                return endorsementBusinessCia.GetDataModification(risk, coverageStatusType);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }
    }
}
