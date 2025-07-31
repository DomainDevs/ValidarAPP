using Sistran.Company.ExternalRenewalServices.Models;
using System.ServiceModel;

namespace Sistran.Company.ExternalRenewalServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IExternalRenewalServices
    {
        [OperationContract]
        ResponsePolicyRenewal PolicyRenewal(PolicyRenewalRequest policyRenewalRequest);

        [OperationContract]
        ResponsePolicyRenewal PolicyRenewalByAlliance(PolicyRenewalByAllianceRequest policyRenewalByAllianceRequest);

        [OperationContract]
        ResponseFirmRenewalPolicy FirmRenewalPolicy(int tempId);
        
    }
}
