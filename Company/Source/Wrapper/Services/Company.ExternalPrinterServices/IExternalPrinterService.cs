using Sistran.Company.ExternalPrinterServices.Models;
using System.ServiceModel;

namespace Sistran.Company.ExternalPrinterServices
{
    [ServiceContract]
    [XmlSerializerFormat]
    public interface IExternalPrinterService
    {
        [OperationContract]
        ConsulBenefPolicyResponse ConsulBenefPolicy(PolicyPrinterClass  consulBenefPolicyRequest);

        [OperationContract]
        ConsultCoveragePolicyResponse ConsultCoveragePolicy(PolicyPrinterClass consultCoveragePolicyRequest);

        [OperationContract]
        ConsultDataBasicPolicyResponse ConsultDataBasicPolicy(PolicyPrinterClass consultDataBasicPolicyRequest);
        
        [OperationContract]
        ConsultDataClientResponse ConsultDataClient(ConsultDataClientRequest consultDataClientRequest);

        [OperationContract]
        ConsultMultiriskHomePolicyResponse ConsultMultiriskHomePolicy(PolicyPrinterClass policyPrinterClass);
        
        [OperationContract]
        PrinterClass GenerateWSPolicyFormatCollect(GenerateWSPolicyFormatCollectRequest generateWSPolicyFormatCollectRequest);

        [OperationContract]
        GenerateWSPolicyPrinterResponse GenerateWSPolicyPrinter(GenerateWSPolicyPrinterRequest generateWSPolicyPrinterRequest);

        [OperationContract]
        PrinterClass GenerateWSQuotePrinter(GenerateWSQuotePrinterRequest generateWSQuotePrinterRequest);

        [OperationContract]
        GetConsulEndorsementPolicyResponse GetConsulEndorsementPolicy(GetConsulEndorsementPolicyRequest getConsulEndorsementPolicyRequest);

        [OperationContract]
        GetConsulRecoveriesPolicyResponse GetConsulRecoveriesPolicy(GetConsulRecoveriesPolicyRequest getConsulRecoveriesPolicyRequest);

        [OperationContract]
        GetConsulSinisterPolicyResponse GetConsulSinisterPolicy(GetConsulSinisterPolicyRequest getConsulSinisterPolicyrequest);
    }
}
