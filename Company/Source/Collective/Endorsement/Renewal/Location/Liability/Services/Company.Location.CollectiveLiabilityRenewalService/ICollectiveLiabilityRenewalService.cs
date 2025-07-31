using System.ServiceModel;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;

namespace Sistran.Company.Application.Location.CollectiveLiabilityRenewalService
{
    [ServiceContract]
    public interface ICollectiveLiabilityRenewalService
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveRenewal(CollectiveEmission collectiveEmission);

        [OperationContract]
        void QuotateCollectiveRenewal(int collectiveEmissionId);

        [OperationContract]
        MassiveLoad IssuanceRenewalMassiveEmission(MassiveLoad collectiveEmission);
    }
}