using Sistran.Company.ExternalConsultPrinterServices;
using Sistran.Company.ExternalConsultPrinterServices.Models;
using Sistran.Company.ExternalPrinterServicesEEProvider.Assemblers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.ExternalConsultPrinterServicesEEProvider
{
    public class ExternalConsultPrinterServicesEEProvider: IExternalConsultPrinterService
    {
        private WSPolicyPrinter.WSPolicyPrinterSoapClient _client = new WSPolicyPrinter.WSPolicyPrinterSoapClient();
        public ConsulBenefPolicyResponse ConsulBenefPolicy(PolicyPrinterClass param)
        {
            WSPolicyPrinter.BenefPolicy benefPolicy = _client.ConsulBenefPolicy(param.BranchId, param.PrefixNum, param.DocumentNumber, param.Endorsement_nro);

            return ModelAssembler.GenerateBenefPolicy(benefPolicy);
        }

        public ConsultCoveragePolicyResponse ConsultCoveragePolicy(PolicyPrinterClass param)
        {
            WSPolicyPrinter.CoveragePolicy coveragePolicy = _client.ConsultCoveragePolicy(param.BranchId, param.PrefixNum, param.DocumentNumber);

            return ModelAssembler.ConsultCoveragesPolicy(coveragePolicy);
        }

        public ConsultDataBasicPolicyResponse ConsultDataBasicPolicy(PolicyPrinterClass param)
        {
            WSPolicyPrinter.BasicsPolicy datesBasicsPolicyClass = _client.ConsultDataBasicPolicy(param.BranchId, param.PrefixNum, param.DocumentNumber, param.Endorsement_nro);

            return ModelAssembler.ConsultDataBasicPolicies(datesBasicsPolicyClass.ResulBasics);

        }

        public ConsultDataClientResponse ConsultDataClient(ConsultDataClientRequest consultData)
        {
            WSPolicyPrinter.BasicsClient datesBasicsClient = _client.ConsultDataClient(consultData.Tipodoc, consultData.Nrodoc, consultData.Codcli);
            return ModelAssembler.ConsultDataClients(datesBasicsClient.ResulClient);
        }

        public ConsultMultiriskHomePolicyResponse ConsultMultiriskHomePolicy(PolicyPrinterClass policyPrinter)
        {
            WSPolicyPrinter.Multirisk multiriskHomeClass = _client.ConsultMultiriskHomePolicy(policyPrinter.BranchId, policyPrinter.PrefixNum, policyPrinter.DocumentNumber);
            return ModelAssembler.ConsultMultiriskHomePolicies(multiriskHomeClass.MultiriskHome, multiriskHomeClass.ProcessMessage);
        }
        
        public GetConsulEndorsementPolicyResponse GetConsulEndorsementPolicy(GetConsulEndorsementPolicyRequest endorsement)
        {
            WSPolicyPrinter.EndorsementPolicy endorsementPolicy = _client.GetConsulEndorsementPolicy(endorsement.BranchId, endorsement.PrefixNum, endorsement.DocumentNumber, endorsement.Typequery);
            return ModelAssembler.GetConsulEndorsementPolicies(endorsementPolicy.ListEndorsementPolicyClass, endorsementPolicy.ProcessMessage);
        }

        public GetConsulRecoveriesPolicyResponse GetConsulRecoveriesPolicy(GetConsulRecoveriesPolicyRequest getConsulRecoveriesPolicyRequest)
        {
            WSPolicyPrinter.RecoveriesPolicy recoveriesPolicy = _client.GetConsulRecoveriesPolicy
            (
                getConsulRecoveriesPolicyRequest.Codsuc,
                getConsulRecoveriesPolicyRequest.Codramo,
                getConsulRecoveriesPolicyRequest.Numpoliza,
                getConsulRecoveriesPolicyRequest.Numendoso,
                getConsulRecoveriesPolicyRequest.Fecinimora,
                getConsulRecoveriesPolicyRequest.Fecfinmora,
                getConsulRecoveriesPolicyRequest.Fecdesdeultpago,
                getConsulRecoveriesPolicyRequest.Fechastaultpago,
                getConsulRecoveriesPolicyRequest.Fecdesdeproxpago,
                getConsulRecoveriesPolicyRequest.Fechastaproxpago
            );

            return ModelAssembler.GetConsulRecoveriesPolicies(recoveriesPolicy.ListRecoveriesPolicyClass, recoveriesPolicy.ProcessMessage);

        }

        public GetConsulSinisterPolicyResponse GetConsulSinisterPolicy(GetConsulSinisterPolicyRequest getConsulSinisterPolicyrequest)
        {
            WSPolicyPrinter.SinisterPolicy sinisterPolicy = _client.GetConsulSinisterPolicy
            (
                getConsulSinisterPolicyrequest.Codsuc,
                getConsulSinisterPolicyrequest.Codramo,
                getConsulSinisterPolicyrequest.Numpoliza,
                getConsulSinisterPolicyrequest.Numendoso,
                getConsulSinisterPolicyrequest.Fecdesdesin,
                getConsulSinisterPolicyrequest.Fechastasin,
                getConsulSinisterPolicyrequest.Fecdesdeavisin,
                getConsulSinisterPolicyrequest.Fechastaavisin,
                getConsulSinisterPolicyrequest.Fecdesdepagsin,
                getConsulSinisterPolicyrequest.Fechastapagsin
            );

            return ModelAssembler.GetConsulSinisterPolicies(sinisterPolicy.ListSinisterPolicyClass, sinisterPolicy.ProcessMessage);
        }
    }
}
