
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.SarlaftBusinessServices
{
    /// <summary>
    /// Interfaz ISarlaftBusinessServices
    /// </summary>
    [ServiceContract]
    public interface ISarlaftBusinessServices
    {

        #region Sarlaft

        /// <summary>
        /// Retorna usuario
        [OperationContract]
        CompanyUser GetUserByUserId(int userId, string userName);
        /// <summary>
        /// Obtiene una persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPerson GetPersonByDocumentNumberAndSearchType(string documentNumber, int searchType);

        /// <summary>
        /// Obtiene lista de personas
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyPerson> GetPersonByDocumentNumberAndSearchTypeList(string documentNumber, int searchType);

        /// <summary>
        /// Obtiene una compañia
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCompany GetCompanyByDocumentNumberAndSearchType(string documentNumber, int searchType);

        /// <summary>
        /// Obtiene una compañia
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCompany> GetCompanyByDocumentNumberAndSearchTypeList(string documentNumber, int searchType);

        /// <summary>
        /// Obtiene el historial Sarlaft 
        /// </summary>
        /// <param name="sarlaft"></param>
        /// <returns></returns>
        /// 
        [OperationContract]
        List<CompanyIndividualSarlaft> GetSarlaft(int individualId);
        /// <summary>
        /// Crea el Sarlaft 
        /// </summary>
        /// <param name="sarlaft"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCustomerKnowledge CreateCompanySarlaft(CompanyIndividualSarlaft sarlaft, CompanyFinancialSarlaft financialSarlaft);
        /// <summary>
        /// Obtiene el historial Sarlaft 
        /// </summary>
        /// <param name="sarlaft"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanySarlaftExoneration> GetSarlaftExoneration(int individualId);
        /// <summary>
        /// Actualiza Sarlaft 
        /// </summary>
        /// <param name="sarlaft"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCustomerKnowledge UpdateCompanySarlaft(CompanyIndividualSarlaft sarlaft, CompanyFinancialSarlaft financialSarlaft);
        /// <summary>
        /// Obtiene un srlaft específico
        /// </summary>
        /// <param name="sarlaft"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyCustomerKnowledge GetSarlaftBySarlaftId(int sarlaftId);
        [OperationContract]
        List<CompanyEconomicActivity> GetEconomicActivities(string description);

        [OperationContract]
        CompanyIndividualSarlaft GetLastSarlaftId(int individualId);

        [OperationContract]
        bool ValidationAccessAndHierarchyByUser(int UserId);

        [OperationContract]
        CompanyTmpSarlaftOperation GetCompanyTmpSarlaftOperation(int operationId);

        [OperationContract]
        CompanyTmpSarlaftOperation CreateCompanySarlaftOperation(CompanyTmpSarlaftOperation companyTmpSarlaftOperation);

        #endregion

        #region Links

        [OperationContract]
        List<CompanyRelationShip> GetRelationship();
        [OperationContract]
        List<CompanyIndvidualLink> GetIndividualLinksByIndividualId(int individualLink, int sarlaftId);
        [OperationContract]
        CompanyIndvidualLink CreateIndividualLink(CompanyIndvidualLink companyIndvidualLink);
        [OperationContract]
        CompanyIndvidualLink UpdateIndividualLink(CompanyIndvidualLink companyIndvidualLink);

        #endregion

        #region LegalRepresentative

        [OperationContract]
        CompanyLegalRepresentative CreateLegalRepresentative(CompanyLegalRepresentative legalRepresentative);
        [OperationContract]
        CompanyLegalRepresentative UpdateLegalRepresentative(CompanyLegalRepresentative legalRepresentative);
        [OperationContract]
        List<CompanyLegalRepresentative> GetLegalRepresentativeByIndividualId(int individualId, int sarlaftId);
        [OperationContract]
        CompanyLegalRepresentative CreateSubstituteLegalRepresentative(CompanyLegalRepresentative legalRepresentative);
        [OperationContract]
        CompanyLegalRepresentative UpdateSubstituteLegalRepresentative(CompanyLegalRepresentative legalRepresentative);

        #endregion

        #region Partners

        [OperationContract]
        CompanyIndividualPartner CreatePartner(CompanyIndividualPartner companyPartner);
        [OperationContract]
        CompanyIndividualPartner UpdatePartner(CompanyIndividualPartner companyPartner);
        [OperationContract]
        List<CompanyIndividualPartner> GetCompanyPartnerByIndividualId(int individualId, int sarlaftId);
        [OperationContract]
        void DeletePartner(CompanyIndividualPartner companyPartner);

        #endregion

        #region InternationalOperations

        [OperationContract]
        List<CompanySarlaftOperation> GetInternationalOperationsBySarlaftId(int sarlafId);
        [OperationContract]
        CompanySarlaftOperation CreateInternationalOperation(CompanySarlaftOperation companySarlaftOperation);
        [OperationContract]
        CompanySarlaftOperation UpdateInternationalOperation(CompanySarlaftOperation companySarlaftOperation);
        [OperationContract]
        void DeleteInternationalOperation(CompanySarlaftOperation companySarlaftOperation);

        #endregion

        #region politicas

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesSarlaft(List<CompanySarlaftOperation> companySarlaftOperations, List<CompanyLegalRepresentative> companyLegalRepresentative, List<CompanyIndividualPartner> companyIndividualPartners, List<CompanyIndvidualLink> companyIndvidualLinks, CompanyIndividualSarlaft companyIndividualSarlaft, CompanyCoSarlaft coSarlaft, CompanyFinancialSarlaft companyFinancialSarlaft);

        [OperationContract]
        List<CompanyTmpSarlaftOperation> GetSarlaftOperationTmp(int IndividualId);

        [OperationContract]
        List<AuthorizationRequest> GetSarlaftAuthorizationRequestByIndividualId(int individualId);
        #endregion politicas

        #region peps
        [OperationContract]
        CompanyIndvidualPeps CreatePeps(CompanyIndvidualPeps companyPeps);
        #endregion

        #region peps
        [OperationContract]
        CompanyCoSarlaft CreateCoSarlaft(CompanyCoSarlaft companyCoSarlaft);
        [OperationContract]
        CompanyIndvidualPeps GetIndividualPepsByIndividualId(int individualId, int sarlaftId);
        [OperationContract]
        CompanyCoSarlaft GetIndividualCoSarlft(int individualId, int sarlaftId);
        #endregion
        [OperationContract]
        List<CompanyRole> GetRoles();

        [OperationContract]
        List<CompanyEntity> GetCategory();

        [OperationContract]
        List<CompanyEntity> GetAffinity();

        [OperationContract]
        List<CompanyEntity> GetRelation();

        [OperationContract]
        List<CompanyEntity> GetOppositor();

        [OperationContract]
        List<CompanyEntity> GetSociety();

        [OperationContract]
        List<CompanyEntity> GetNationality();

        [OperationContract]
        List<string> GetInterviewManagerByDescription(string InterviewManager);

        [OperationContract]
        List<string> GetInterviewManagerByDescriptionSarlaft(string InterviewManager);

    }
}
