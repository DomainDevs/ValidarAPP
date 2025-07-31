using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.Location.LiabilityCollectiveService
{
    [ServiceContract]
    public interface ILiabilityCollectiveService
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveLoad(CollectiveEmission collectiveLoad);

        [OperationContract]
        void QuotateCollectiveLoad(List<int> collectiveLoadIds);

        [OperationContract]
        void QuotateMassiveCollectiveEmission(int collectiveEmissionId);

        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveEmission);

        [OperationContract]
        string GenerateReportToCollectiveLoad(CollectiveEmission collectiveEmission, int endorsementType);
    }
}