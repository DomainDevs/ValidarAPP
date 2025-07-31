using System.ServiceModel;

namespace Sistran.Company.Integration.OperationQuotaCompanyServices
{
    [ServiceContract]
    public interface IOperationQuotaCompanyIntegrationService
    {
        [OperationContract]
        void prueba(int Id);
    }
}