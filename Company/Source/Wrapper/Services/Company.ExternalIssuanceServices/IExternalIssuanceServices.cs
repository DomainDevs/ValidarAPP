using Sistran.Company.ExternalIssuanceServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalIssuanceServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IExternalIssuanceServices
    {
        [OperationContract]
        PolicyIssuanceVehicleResult PolicyIssuanceVehicle(PolicyIssuanceVehicleRequest policyIssuanceVehicleRequest);

        [OperationContract]
        ResponsePolicyRenewal PolicyRenewal(PolicyRenewalRequest policyRenewalRequest);

        [OperationContract]
        ResponsePolicyRenewal PolicyRenewalByAlliance(PolicyRenewalByAllianceRequest policyRenewalByAllianceRequest);

        [OperationContract]
        ResponseFirmRenewalPolicy FirmRenewalPolicy(int tempId);
    }
}
