using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalService;
using System;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.DAOs;
using Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.Vehicles.CollectiveVehicleRenewalServiceEEProvider
{
    public class CollectiveVehicleRenewalServiceEEProvider : ICollectiveVehicleRenewalService
    {
        public CollectiveVehicleRenewalServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveLoad)
        {
            VehicleRenwalDAO vehicleRenwalDAO = new VehicleRenwalDAO();
            return vehicleRenwalDAO.CreateCollectiveRenewal(collectiveLoad);
        }

        public void QuotateCollectiveRenewal(MassiveLoad massiveLoad)
        {
            RenewalQuotateDAO dao = new RenewalQuotateDAO();
            dao.QuotateCollectiveRenewal(massiveLoad);
        }

        public MassiveLoad IssuanceCollectiveRenewal(MassiveLoad collectiveRenewal)
        {
            try
            {
                return new RenewalQuotateDAO().IssuanceCollectiveRenewal(collectiveRenewal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, ""), ex);
            }
        }
    }
}