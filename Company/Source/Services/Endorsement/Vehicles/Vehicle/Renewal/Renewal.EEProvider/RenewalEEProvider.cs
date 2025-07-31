using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.VehicleRenewalService.EEProvider.Business;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;

namespace Sistran.Company.Application.VehicleEndorsementRenewalService.EEProvider
{
    public class VehicleRenewalServiceEEProvider :  IVehicleRenewalService
    {
        /// <summary>
        /// Creacion Renewal
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public CompanyPolicy CreateRenewal(CompanyPolicy companyPolicy)
        {
            try
            {
                RenewalBusinessCia RenewalBusinessCia = new RenewalBusinessCia();
                return RenewalBusinessCia.CreateTemporal(companyPolicy);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }
    }
}
