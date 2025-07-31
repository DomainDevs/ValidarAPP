using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices
{
    [ServiceContract]
    public interface IConceptBIntegrationService
    {
        [OperationContract]
        void Execute();

        [OperationContract]
        List<int> GetData();
    }
}