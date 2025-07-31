
using System.ServiceModel;

namespace Sistran.Company.Application.QueueBrokerBusinessService
{
    [ServiceContract]
    public interface ICompanyQueueBrokerBusinessService
    {
        [OperationContract]
        void Queue();
    }
}