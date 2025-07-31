using Company.Location.CollectiveLiabilityRenewalService.EEProvider.DAOs;
using Company.Location.CollectiveLiabilityRenewalService.EEProvider.Resources;
using Sistran.Company.Application.Location.CollectiveLiabilityRenewalService.EEProvider.DAOs;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;

namespace Sistran.Company.Application.Location.CollectiveLiabilityRenewalService.EEProvider
{
    public class CollectiveLiabilityRenewalServiceEEProvider : ICollectiveLiabilityRenewalService
    {
        public CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveEmission)
        {
            CollectiveLiabilityRenewalDAO collectiveLiabilityRenewalDAO = new CollectiveLiabilityRenewalDAO();
            return collectiveLiabilityRenewalDAO.CreateMassiveLoad(collectiveEmission);
        }

        public void QuotateCollectiveRenewal(int collectiveEmissionId)
        {
            CollectiveLiabilityRenewalProcessDAO collectiveLiabilityRenewalProcessDAO = new CollectiveLiabilityRenewalProcessDAO();
            collectiveLiabilityRenewalProcessDAO.QuotateMassiveCollectiveEmission(collectiveEmissionId);
        }

        public MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad collectiveEmission)
        {
            try
            {
                return new CollectiveLiabilityRenewalProcessDAO().IssuanceCollectiveEmission(collectiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuingLiabilityPolicies), ex);
            }
        }
    }
}