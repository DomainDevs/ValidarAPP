using System.ServiceModel;

namespace Sistran.Core.Integration.UnderwritingReinsuranceWorkerServices
{
    [ServiceContract]
    public interface IUnderwritingReinsuranceWorkerIntegrationServices
    {
        [OperationContract]
        int ReinsuranceIssue(int policyId, int endorsementId, int userId);
    }
}
