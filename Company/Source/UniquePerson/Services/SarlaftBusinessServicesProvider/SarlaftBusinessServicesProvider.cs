using Sistran.Company.Application.SarlaftBusinessServices;
using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Assemblers;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.Business;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.DAO;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using UCCEN = Sistran.Core.Application.UniquePerson.Entities;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider
{
    /// <summary>
    /// clase que implementa la interfaz "ISarlaftApplicationServices"
    /// </summary>  
    public class SarlaftBusinessServicesProvider : ISarlaftBusinessServices
    {

        #region Sarlaft
        public CompanyUser GetUserByUserId(int userId, string userName)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetUserByUserId(userId, userName);
        }

        public CompanyPerson GetPersonByDocumentNumberAndSearchType(string documentNum, int searchType)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetPersonByDocumentNumberAndSearchType(documentNum, searchType);
        }

        public List<CompanyPerson> GetPersonByDocumentNumberAndSearchTypeList(string documentNum, int searchType)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetPersonByDocumentNumberAndSearchTypeList(documentNum, searchType);
        }

        public CompanyCompany GetCompanyByDocumentNumberAndSearchType(string documentNum, int searchType)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetCompanyByDocumentNumberAndSearchType(documentNum, searchType);
        }

        public List<CompanyCompany> GetCompanyByDocumentNumberAndSearchTypeList(string documentNum, int searchType)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetCompanyByDocumentNumberAndSearchTypeList(documentNum, searchType);
        }

        public List<CompanyIndividualSarlaft> GetSarlaft(int individualId)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetSarlaft(individualId);
        }

        public CompanyIndividualSarlaft GetLastSarlaftId(int individualId)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.GetLastSarlaftId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public CompanyCustomerKnowledge CreateCompanySarlaft(CompanyIndividualSarlaft sarlaft, CompanyFinancialSarlaft financialSarlaft)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.CreateSarlaft(sarlaft, financialSarlaft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCustomerKnowledge UpdateCompanySarlaft(CompanyIndividualSarlaft sarlaft, CompanyFinancialSarlaft financialSarlaft)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.UpdateSarlaft(sarlaft, financialSarlaft);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCustomerKnowledge GetSarlaftBySarlaftId(int sarlaftId)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.GetSarlaftBySarlaftId(sarlaftId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<CompanySarlaftExoneration> GetSarlaftExoneration(int individualId)
        {

            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetSarlaftExonerationByIndividualId(individualId);

        }

        public List<CompanyEconomicActivity> GetEconomicActivities(string description)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return sarlaftDAO.GetEconomicActivities(description);
        }

        public bool ValidationAccessAndHierarchyByUser(int UserId)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.ValidationAccessAndHierarchyByUser(UserId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyTmpSarlaftOperation GetCompanyTmpSarlaftOperation(int operationId)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.GetCompanyTmpSarlaftOperation(operationId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyTmpSarlaftOperation CreateCompanySarlaftOperation(CompanyTmpSarlaftOperation companyTmpSarlaftOperation)
        {
            try
            {
                SarlaftDAO sarlaftDAO = new SarlaftDAO();
                return sarlaftDAO.CreateCompanySarlaftOperation(companyTmpSarlaftOperation);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region Links
        public List<CompanyRelationShip> GetRelationship()
        {
            {
                try
                {
                    IndividualLinkBusiness individualLinkBusiness = new IndividualLinkBusiness();
                    return individualLinkBusiness.GetRelationship();
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public List<CompanyIndvidualLink> GetIndividualLinksByIndividualId(int individualId, int sarlaftId)
        {
            {
                try
                {
                    IndividualLinkBusiness individualLinkBusiness = new IndividualLinkBusiness();
                    return individualLinkBusiness.GetIndividualLinksByIndividualId(individualId, sarlaftId);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public CompanyIndvidualLink CreateIndividualLink(CompanyIndvidualLink companyIndvidualLink)
        {
            {
                try
                {
                    IndividualLinkBusiness individualLinkBusiness = new IndividualLinkBusiness();
                    return individualLinkBusiness.CreateIndividualLink(companyIndvidualLink);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public CompanyIndvidualLink UpdateIndividualLink(CompanyIndvidualLink companyIndvidualLink)
        {
            {
                try
                {
                    IndividualLinkBusiness individualLinkBusiness = new IndividualLinkBusiness();
                    return individualLinkBusiness.UpdateIndividualLink(companyIndvidualLink);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        #endregion

        #region LegalRepresentative
        public CompanyLegalRepresentative CreateLegalRepresentative(CompanyLegalRepresentative legalRepresent)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentBusiness = new LegalRepresentativeBusiness();
                return legalRepresentBusiness.CreateRepresentLegal(legalRepresent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public CompanyLegalRepresentative UpdateLegalRepresentative(CompanyLegalRepresentative legalRepresent)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentBusiness = new LegalRepresentativeBusiness();
                return legalRepresentBusiness.UpdateRepresentLegal(legalRepresent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyLegalRepresentative> GetLegalRepresentativeByIndividualId(int individualId, int sarlaftId)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentBusiness = new LegalRepresentativeBusiness();
                return legalRepresentBusiness.GetLegalRepresentByIndividualId(individualId, sarlaftId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyLegalRepresentative CreateSubstituteLegalRepresentative(CompanyLegalRepresentative legalRepresent)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentBusiness = new LegalRepresentativeBusiness();
                return legalRepresentBusiness.CreateSubstituteRepresentLegal(legalRepresent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public CompanyLegalRepresentative UpdateSubstituteLegalRepresentative(CompanyLegalRepresentative legalRepresent)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentBusiness = new LegalRepresentativeBusiness();
                return legalRepresentBusiness.UpdateSubstituteRepresentLegal(legalRepresent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region Partners
        public CompanyIndividualPartner CreatePartner(CompanyIndividualPartner companyPartner)
        {
            try
            {
                CompanyIndividualPartner partner;
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                partner = partnerBusiness.CreatePartner(companyPartner);
                partner.FinalBeneficiary = companyPartner.FinalBeneficiary;

                if (partner.FinalBeneficiary != null && partner.FinalBeneficiary.Count > 0)
                    partner.FinalBeneficiary = partnerBusiness.CreateBeneficiaryPartner(partner.FinalBeneficiary, partner.Id, partner.IndividualId);
                

                return partner;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIndividualPartner UpdatePartner(CompanyIndividualPartner companyPartner)
        {
            try
            {
                CompanyIndividualPartner partner;
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                partner = partnerBusiness.UpdatePartner(companyPartner);
                partner.FinalBeneficiary = companyPartner.FinalBeneficiary;
                if (partner.FinalBeneficiary != null && partner.FinalBeneficiary.Count > 0)
                {
                    partnerBusiness.DeleteCoBeneficiaryPartners(companyPartner);
                    partner.FinalBeneficiary = partnerBusiness.CreateBeneficiaryPartner(partner.FinalBeneficiary, partner.Id, partner.IndividualId);
                }
                return partner;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<CompanyIndividualPartner> GetCompanyPartnerByIndividualId(int individualId, int sarlaftId)
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                return partnerBusiness.GetPartnersByIndividualId(individualId, sarlaftId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeletePartner(CompanyIndividualPartner companyPartner)
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                partnerBusiness.DeletePartner(companyPartner);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region InternationalOperations
        public List<CompanySarlaftOperation> GetInternationalOperationsBySarlaftId(int sarlafId)
        {
            {
                try
                {
                    SarlaftOperationsBusiness sarlaftOperationsBusiness = new SarlaftOperationsBusiness();
                    return sarlaftOperationsBusiness.GetInternationalOperationsBySarlaftId(sarlafId);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public CompanySarlaftOperation CreateInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            {
                try
                {
                    SarlaftOperationsBusiness sarlaftOperationsBusiness = new SarlaftOperationsBusiness();
                    return sarlaftOperationsBusiness.CreateInternationalOperation(companySarlaftOperation);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public CompanySarlaftOperation UpdateInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            {
                try
                {
                    SarlaftOperationsBusiness sarlaftOperationsBusiness = new SarlaftOperationsBusiness();
                    return sarlaftOperationsBusiness.UpdateInternationalOperation(companySarlaftOperation);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public void DeleteInternationalOperation(CompanySarlaftOperation companySarlaftOperation)
        {
            {
                try
                {
                    SarlaftOperationsBusiness sarlaftOperationsBusiness = new SarlaftOperationsBusiness();
                    sarlaftOperationsBusiness.DeleteInternationalOperation(companySarlaftOperation);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        #endregion


        #region peps
        public CompanyIndvidualPeps CreatePeps(CompanyIndvidualPeps companyPeps)
        {
            try
            {
                PepsBusiness pepsBusiness = new PepsBusiness();

                return pepsBusiness.CreatePeps(companyPeps);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyIndvidualPeps GetIndividualPepsByIndividualId(int individualId, int sarlaftId)
        {
            {
                try
                {
                    PepsBusiness pepsBusiness = new PepsBusiness();
                    return pepsBusiness.GetPepsByIndividualId(individualId, sarlaftId);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }


        #endregion

        #region CoSarlaft
        public CompanyCoSarlaft CreateCoSarlaft(CompanyCoSarlaft companyCoSarlaft)
        {
            try
            {

                CoSarlaftBusiness CoSarlaftBusiness = new CoSarlaftBusiness();

                return CoSarlaftBusiness.CreateCoSarlaft(companyCoSarlaft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyCoSarlaft GetIndividualCoSarlft(int individualId, int sarlaftId)
        {
            {
                try
                {
                    CoSarlaftBusiness CoSarlaftBusiness = new CoSarlaftBusiness();
                    return CoSarlaftBusiness.GetCoSarlaftByIndividualId(individualId, sarlaftId);
                }
                catch (Exception ex)
                {
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
        #endregion

        #region Politicas

        public List<PoliciesAut> ValidateAuthorizationPoliciesSarlaft(List<CompanySarlaftOperation> companySarlaftOperations, List<CompanyLegalRepresentative> companyLegalRepresentative, List<CompanyIndividualPartner> companyIndividualPartners, List<CompanyIndvidualLink> companyIndvidualLinks, CompanyIndividualSarlaft companyIndividualSarlaft, CompanyCoSarlaft coSarlaft, CompanyFinancialSarlaft companyFinancialSarlaft)
        {
            int package = 15;
            List<PoliciesAut> policiesAuts = new List<PoliciesAut>();

            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralSarlaft(facade, companyFinancialSarlaft, coSarlaft, companyIndividualSarlaft);
            policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "15,0", facade, FacadeType.RULE_FACADE_GENERAL_SARLAFT));

            if (companySarlaftOperations != null)
            {
                facade = new Facade();
                EntityAssembler.CreateFacadeGeneralSarlaft(facade, companyFinancialSarlaft, coSarlaft, companyIndividualSarlaft);
                foreach (CompanySarlaftOperation companySarlaftOperation in companySarlaftOperations)
                {
                    EntityAssembler.CreateFacadeInternationalOperations(facade, companySarlaftOperation);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "15,0", facade, FacadeType.RULE_FACADE_INTERNATIONAL_OPERATIONS));
                }
            }

            if (companyLegalRepresentative != null)
            {
                facade = new Facade();
                EntityAssembler.CreateFacadeGeneralSarlaft(facade, companyFinancialSarlaft, coSarlaft, companyIndividualSarlaft);
                foreach (var item in companyLegalRepresentative)
                {
                    EntityAssembler.CreateFacadeLegalRepresentative(facade, item);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "15,0", facade, FacadeType.RULE_FACADE_LEGAL_REPRESENTATIVE));
                }
            }

            if (companyIndvidualLinks != null)
            {
                facade = new Facade();
                EntityAssembler.CreateFacadeGeneralSarlaft(facade, companyFinancialSarlaft, coSarlaft, companyIndividualSarlaft);
                foreach (CompanyIndvidualLink companyIndvidualLink in companyIndvidualLinks)
                {
                    EntityAssembler.CreateFacadeLinks(facade, companyIndvidualLink);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "15,0", facade, FacadeType.RULE_FACADE_LINKS));
                }
            }

            if (companyIndividualPartners != null)
            {
                facade = new Facade();
                EntityAssembler.CreateFacadeGeneralSarlaft(facade, companyFinancialSarlaft, coSarlaft, companyIndividualSarlaft);
                foreach (CompanyIndividualPartner companyIndividualPartner in companyIndividualPartners)
                {
                    EntityAssembler.CreateFacadePartners(facade, companyIndividualPartner);
                    policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "15,0", facade, FacadeType.RULE_FACADE_PARTNERS));

                    if (companyIndividualPartner.FinalBeneficiary != null && companyIndividualPartner.FinalBeneficiary.Any())
                    {
                        foreach (CompanyFinalBeneficiary beneficiary in companyIndividualPartner.FinalBeneficiary)
                        {
                            EntityAssembler.CreateFacadeBeneficiaries(facade, beneficiary);
                            policiesAuts.AddRange(DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "15,0", facade, FacadeType.RULE_FACADE_BENEFICIARIES));
                        }
                    }
                }
            }

            return policiesAuts;
        }

        public List<CompanyTmpSarlaftOperation> GetSarlaftOperationTmp(int IndividualId)
        {
            try
            {
                SarlaftDAO sarlaftOperationDAO = new SarlaftDAO();
                return sarlaftOperationDAO.GetSarlaftOperationTmp(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }


        public List<AuthorizationRequest> GetSarlaftAuthorizationRequestByIndividualId(int individualId)
        {
            try
            {
                CompanySarlaftOperationBusiness companySarlaftOperationBusiness = new CompanySarlaftOperationBusiness();
                return companySarlaftOperationBusiness.GetSarlaftAuthorizationRequestByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        #endregion Politicas

        #region Combos
        public List<CompanyRole> GetRoles()
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                return partnerBusiness.GetRoles();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEntity> GetCategory()
        {
            try
            {
                EntityBusiness entityBusiness = new EntityBusiness();
                return entityBusiness.GetCategory();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        public List<CompanyEntity> GetAffinity()
        {
            try
            {
                EntityBusiness entityBusiness = new EntityBusiness();
                return entityBusiness.GetAffinity();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        public List<CompanyEntity> GetRelation()
        {
            try
            {
                EntityBusiness entityBusiness = new EntityBusiness();
                return entityBusiness.GetRelation();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEntity> GetOppositor()
        {
            try
            {
                EntityBusiness entityBusiness = new EntityBusiness();
                return entityBusiness.GetOppositor();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEntity> GetSociety()
        {
            try
            {
                EntityBusiness entityBusiness = new EntityBusiness();
                return entityBusiness.GetSociety();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEntity> GetNationality()
        {
            try
            {
                EntityBusiness entityBusiness = new EntityBusiness();
                return entityBusiness.GetNationality();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #endregion 

        #region InterviewManager
        public List<string> GetInterviewManagerByDescription(string InterviewManager)
        {
            
            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return EntityAssembler.CreateInterviewManager(sarlaftDAO.GetUserByAccountName(InterviewManager));
        }

        public List<string> GetInterviewManagerByDescriptionSarlaft(string InterviewManager)
        {

            SarlaftDAO sarlaftDAO = new SarlaftDAO();
            return EntityAssembler.CreateInterviewManagerSarlaft(sarlaftDAO.GetUserByAccountNameSarlaft(InterviewManager));
        }
        
        #endregion

    }
}
