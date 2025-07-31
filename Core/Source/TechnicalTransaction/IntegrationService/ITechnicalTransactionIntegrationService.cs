using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using System.ServiceModel;

namespace Sistran.Core.Integration.TechnicalTransactionGeneratorServices
{
    [ServiceContract]
    public interface ITechnicalTransactionIntegrationService
    {
        [OperationContract]
        TechnicalTransactionDTO GetTechnicalTransaction(TechnicalTransactionParameterDTO parameters);
    }
}
