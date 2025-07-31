using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.WrapperAuthorizationPoliciesService
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IWrapperAuthorizationPoliciesService
    {
        [OperationContract]
        void UpdateCollectiveLoadAuthorization(int loadId, int temporalId, List<int> risks);


        [OperationContract]
        void CompanyUpdateMassiveLoadAuthorization(string loadId, List<string> temporalId);

        [OperationContract]
        void CreatePolicyAuthorization(int TemporalId);

        [OperationContract]
        void CreateClaimAuthorization(int claimTemporalId);

        [OperationContract]
        void CreatePaymentRequestAuthorization(int paymentRequestTemporalId);

        [OperationContract]
        void CreateChargeRequestAuthorization(int chargeRequestTemporalId);

        [OperationContract]
        void CreateClaimNoticeAuthorization(int claimNoticeTemporalId);

        [OperationContract]
        void CreatePersonAuthorization(int operationId);
        
        [OperationContract]
        void CreateInsuredAuthorization(int operationId);

        [OperationContract]
        void CreateProviderAuthorization(int operationId);

        [OperationContract]
        void CreateAgentAuthorization(int operationId);

        [OperationContract]
        void CreateReInsurerAuthorization(int operationId);

        [OperationContract]
        void CreateQuotaAuthorization(int operationId);

        [OperationContract]
        void CreateTaxAuthorization(int operationId);

        [OperationContract]
        void CreateCoInsuredAuthorization(int operationId);

        [OperationContract]
        void CreateThirdAuthorization(int operationId);

        [OperationContract]
        void CreateEmployedAuthorization(int operationId);

        [OperationContract]
        void CreatePersonalInformationAuthorization(int operationId);

        [OperationContract]
        void CreatePaymentMethodsAuthorization(int operationId);

        [OperationContract]
        void CreateBankTransfersAuthorization(int operationId);

        [OperationContract]
        void CreateConsortiatesAuthorization(int operationId);

        [OperationContract]
        void CreateBusinessNameAuthorization(int operationId);

        [OperationContract]
        void CreateGuaranteeAuthorization(int operationId);

        [OperationContract]
        void CreateSarlaftAuthorization(int operationId);

        [OperationContract]
        void CreateBasicInfoAuthorization(int operationId);
        [OperationContract]
        void CreateAutomaticQuotaAuthorization(int id);
    }
}