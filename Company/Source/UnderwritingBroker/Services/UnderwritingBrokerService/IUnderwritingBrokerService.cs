using System.ServiceModel;

namespace Sistran.Company.Application.UnderwritingBrokerService
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IUnderwritingBrokerService
    {
        [OperationContract]
        int CreatePendingOperation(string businessCollection);

        [OperationContract]
        int CreatePendingOperationWithParent(string businessCollection);

        [OperationContract]
        int UpdatePendingOperation(string businessCollection);

        [OperationContract]
        void ProcessResponseFromExperienceServiceHistoricPolicies(string businessCollection);

        [OperationContract]
        void ProcessResponseFromExperienceServiceHistoricSinister(string businessCollection);

        [OperationContract]
        void ProcessResponseFromScoreService(string businessCollection);

        [OperationContract]
        void ProcessResponseFromSimitService(string businessCollection);

        [OperationContract]
        string ExecuteCreatePolicy(string businessCollection);

        [OperationContract]
        void UpdatePOAndRecordEndorsementOperation(string businessCollection);

        [OperationContract]
        void UpdateMassiveEmissionRows(string businessCollection, string error);
    }
}