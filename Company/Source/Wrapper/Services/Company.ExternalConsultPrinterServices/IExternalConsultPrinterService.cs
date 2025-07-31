using Sistran.Company.ExternalConsultPrinterServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalConsultPrinterServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IExternalConsultPrinterService
    {
        [OperationContract]
        ConsulBenefPolicyResponse ConsulBenefPolicy(PolicyPrinterClass consulBenefPolicyRequest);

        [OperationContract]
        ConsultCoveragePolicyResponse ConsultCoveragePolicy(PolicyPrinterClass consultCoveragePolicyRequest);

        [OperationContract]
        ConsultDataBasicPolicyResponse ConsultDataBasicPolicy(PolicyPrinterClass consultDataBasicPolicyRequest);

        [OperationContract]
        ConsultDataClientResponse ConsultDataClient(ConsultDataClientRequest consultDataClientRequest);

        [OperationContract]
        ConsultMultiriskHomePolicyResponse ConsultMultiriskHomePolicy(PolicyPrinterClass policyPrinterClass);       

        [OperationContract]
        GetConsulEndorsementPolicyResponse GetConsulEndorsementPolicy(GetConsulEndorsementPolicyRequest getConsulEndorsementPolicyRequest);

        [OperationContract]
        GetConsulRecoveriesPolicyResponse GetConsulRecoveriesPolicy(GetConsulRecoveriesPolicyRequest getConsulRecoveriesPolicyRequest);

        [OperationContract]
        GetConsulSinisterPolicyResponse GetConsulSinisterPolicy(GetConsulSinisterPolicyRequest getConsulSinisterPolicyrequest);
    }
}
