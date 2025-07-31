using Sistran.Company.Application.SarlaftApplicationServices.DTO;
using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.SarlaftApplicationServices
{
    /// <summary>
    /// Interfaz ISarlaftApplicationServices
    /// </summary>
    [ServiceContract]
    public interface ISarlaftApplicationServices
    {

        #region Sarlaft        
        [OperationContract]
        UserDTO GetUserByUserId(int userId, string userName);
        [OperationContract]
        PersonDTO GetPersonByDocumentNumberAndSearchType(string documentNum, int searchType);

        /// <summary>
        /// Obtiene lista de personas
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        List<PersonDTO> GetPersonByDocumentNumberAndSearchTypeList(string documentNumber, int searchType);

        [OperationContract]
        List<SarlaftDTO> GetSarlaft(int individuald);
        [OperationContract]
        CustomerKnowledgeDTO CreateSarlaft(CustomerKnowledgeDTO customerKnowledgeDTO, bool validatePolicies = true);
        [OperationContract]
        CustomerKnowledgeDTO UpdateSarlaft(CustomerKnowledgeDTO customerKnowledgeDTO, bool validatePolicies = true);
        [OperationContract]
        CustomerKnowledgeDTO GetSarlaftBySarlaftId(int sarlaftId);
        [OperationContract]
        List<SarlaftExonerationtDTO> GetSarlaftExoneration(int individuald);
        
        [OperationContract]
        List<EconomicActivityDTO> GetEconomicActivities(string description);

        [OperationContract]
        SarlaftDTO GetLastSarlaftId(PersonDTO person, int UserLogged);

        [OperationContract]
        TmpSarlaftOperationDTO GetCompanyTmpSarlaftOperation(int operationId);

        [OperationContract]
        TmpSarlaftOperationDTO CreateCompanySarlaftOperation(TmpSarlaftOperationDTO tmpSarlaftOperationDTO);

        #endregion

        #region Links
        [OperationContract]
        List<RelationShipDTO> GetRelationship();
        [OperationContract]
        List<LinkDTO> ExecuteOperationLink(List<LinkDTO> linkDTOs);
        [OperationContract]
        List<LinkDTO> GetIndividualLinksByIndividualId(int individualId, int sarlaftId);
        #endregion

        #region LegalRepresentative
        [OperationContract]
        LegalRepresentativeDTO SaveLegalRepresentative(LegalRepresentativeDTO legalRepresentative);

        [OperationContract]
        List<LegalRepresentativeDTO> GetLegalRepresentativeByIndividualId(int individualId, int sarlaftId);

        #endregion

        #region Partners
        [OperationContract]
        PartnersDTO SavePartner(PartnersDTO partner);
        [OperationContract]
        List<PartnersDTO> GetPartnersByIndividualId(int individualId, int sarlaftId);
        #endregion

        #region InternationalOperations
        [OperationContract]
        List<InternationalOperationDTO> GetInternationalOperationsBySarlaftId(int sarlaftId);
        [OperationContract]
        InternationalOperationDTO ExecuteOperation(InternationalOperationDTO internationalOperationDTO);
        #endregion

        #region peps
        [OperationContract]
        PepsDTO SavePeps(PepsDTO peps);

        [OperationContract]
        PepsDTO GetPepsByIndividualId(int individualId, int sarlaftId);

        #endregion
        [OperationContract]
        List<RolDTO> GetRoles();

        [OperationContract]
        List<EntityDTO> GetCategory();

        [OperationContract]
        List<EntityDTO> GetAffinity();

        [OperationContract]
        List<EntityDTO> GetRelation();

        [OperationContract]
        List<EntityDTO> GetOppositor();

        [OperationContract]
        List<EntityDTO> GetSociety();

        [OperationContract]
        List<EntityDTO> GetNationality();

        [OperationContract]
        List<string> GetInterviewManagerByDescription(string InterviewManager);

        [OperationContract]
        List<string> GetInterviewManagerByDescriptionSarlaft(string InterviewManager);

    }
}
