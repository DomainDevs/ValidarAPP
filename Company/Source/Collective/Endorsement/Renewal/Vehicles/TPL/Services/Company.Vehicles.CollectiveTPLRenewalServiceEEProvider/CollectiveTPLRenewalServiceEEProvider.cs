using Sistran.Company.Application.Vehicles.CollectiveTPLRenewalService;
using System;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Company.Application.Vehicles.CollectiveTPLRenewalServiceEEProvider.DAOs;
using Sistran.Company.Application.Vehicles.CollectiveTPLRenewalServiceEEProvider.Resources;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.Vehicles.CollectiveTPLRenewalServiceEEProvider
{
    public class CollectiveTPLRenewalServiceEEProvider : ICollectiveThirdPartyLiabilityRenewalService
    {
        public CollectiveTPLRenewalServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveLoad)
        {
            ThirdPartyLiabilityRenewalDAO vehicleRenwalDAO = new ThirdPartyLiabilityRenewalDAO();
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