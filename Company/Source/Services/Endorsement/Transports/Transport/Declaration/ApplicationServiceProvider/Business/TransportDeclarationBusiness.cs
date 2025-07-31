using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Transports.TransportApplicationService;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Business
{
    public class TransportDeclarationBusiness
    {

        //public DeclarationDTO GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int PolicyId)
        //{
        //    //List<EndorsementDTO> endorsements =new List<EndorsementDTO>();
        //    List<EndorsementDTO> endorsements = DelegateService.transportApplicationService.GetEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, PolicyId);
        //    //if (endorsements == null)
        //    //{
        //    //    endorsementTypeId = 2;
        //    //    List<EndorsementDTO> endorsementsM = DelegateService.transportApplicationService.GetCompanyEndorsementByEndorsementTypeId(endorsementTypeId, PolicyId);
        //    //    if (endorsementsM == null)
        //    //    {
        //    //        endorsementTypeId = 1;
        //    //        List<EndorsementDTO> endorsementsA = DelegateService.transportApplicationService.GetCompanyEndorsementByEndorsementTypeId(endorsementTypeId, PolicyId);
        //    //    }
        //    //    else
        //    //    {
        //    //        return DTOAssembler.CreateEndorsment(endorsementsM);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    return DTOAssembler.CreateEndorsment(endorsements);
        //    //}
        //    return DTOAssembler.CreateEndorsment(endorsements);
        //}

        //public CompanyPolicy CreateDeclaration(DeclarationDTO declarationDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public DeclarationDTO QuotateDeclaration(DeclarationDTO declarationDTO)
        //{
        //    throw new NotImplementedException();
        //}

        //public DeclarationDTO GetPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        //{
        //    CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
        //    return DTOAssembler.CreatePolicy(companyPolicy);
        //}
         
        //public DeclarationDTO CalculateDays(string CurrentFrom, string CurrentTo)
        //{
        //    DeclarationDTO declaration = new DeclarationDTO();
        //    DateTime currentFrom = Convert.ToDateTime(CurrentFrom);
        //    DateTime currentTo = Convert.ToDateTime(CurrentTo);
        //    TimeSpan timeSpan = currentTo - currentFrom;
        //    declaration.Days = timeSpan.Days;
        //    return declaration;
        //}
    }
}
