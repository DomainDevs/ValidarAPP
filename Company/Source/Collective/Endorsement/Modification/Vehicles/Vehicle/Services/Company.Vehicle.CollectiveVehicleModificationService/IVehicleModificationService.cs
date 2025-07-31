using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using System.ServiceModel;

namespace Sistran.Company.Application.Vehicle.ModificationService
{
    [ServiceContract]
    public interface IVehicleModificationService
    {
        [OperationContract]
        CollectiveEmission CreateCollectiveModification(CollectiveEmission collectiveEmission);
        
        [OperationContract]
        void QuotateCollectiveIncluition(MassiveLoad massiveLoad);

        [OperationContract]
        void QuotateCollectiveExclusion(MassiveLoad massiveLoad);

        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(MassiveLoad collectiveIssue);
    }
}