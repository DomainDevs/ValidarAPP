using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sistran.Company.Application.SarlaftApplicationServices;
using Sistran.Company.Application.SarlaftApplicationServices.DTO;
using Sistran.Company.Application.SarlaftApplicationServicesProvider.Assemblers;
using Sistran.Company.Application.SarlaftBusinessServices.Enum;
using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using ENUMCOAP = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;

namespace Sistran.Company.Application.SarlaftApplicationServicesProvider
{
    public class SarlaftApplicationServicesProvider : ISarlaftApplicationServices
    {

        #region Sarlaft

        // <summary>
        // Obtiene los datos del usuario que ingresa.
        // </summary>
        // <param name = "individualId" > Parámetro para consultar Información de la persona.</param>
        // <returns>Retorna el Resultado de una persona.</returns>
        public UserDTO GetUserByUserId(int userId, string userName)
        {
            UserDTO userDTO = new UserDTO();

            CompanyUser user = DelegateService.sarlaftBusinessService.GetUserByUserId(userId, userName);

            if (user != null)
            {
                userDTO = AplicationAssembler.CreateUser(user);
            }

            return userDTO;

        }
        // <summary>
        // Obtiene los datos básicos de una persona.
        // </summary>
        // <param name = "individualId" > Parámetro para consultar Información de la persona.</param>
        // <returns>Retorna el Resultado de una persona.</returns>
        public PersonDTO GetPersonByDocumentNumberAndSearchType(string documentNum, int searchType)
        {
            PersonDTO personDTO = new PersonDTO();

            if (searchType == 1)
            {
                CompanyPerson person = DelegateService.sarlaftBusinessService.GetPersonByDocumentNumberAndSearchType(documentNum, searchType);
                if (person != null)
                {
                    personDTO = AplicationAssembler.CreatePerson(person);
                }
            }
            else
            {
                CompanyCompany company = DelegateService.sarlaftBusinessService.GetCompanyByDocumentNumberAndSearchType(documentNum, searchType);
                if (company != null)
                {
                    personDTO = AplicationAssembler.CreatePerson(company);
                }
            }
            return personDTO;
        }

        // <summary>
        // Obtiene los datos básicos de una persona.
        // </summary>
        // <param name = "individualId" > Parámetro para consultar Información de la persona.</param>
        // <returns>Retorna el Resultado de una persona.</returns>
        public List<PersonDTO> GetPersonByDocumentNumberAndSearchTypeList(string documentNum, int searchType)
        {
            List<PersonDTO> personDTO = new List<PersonDTO>();

            if (searchType == 1)
            {
                List<CompanyPerson> persons = DelegateService.sarlaftBusinessService.GetPersonByDocumentNumberAndSearchTypeList(documentNum, searchType);

                if (persons != null)
                {//foreach
                    foreach (var item in persons)
                    {
                        personDTO.Add(AplicationAssembler.CreatePerson(item));
                    }
                }
            }
            else
            {
                List<CompanyCompany> company = DelegateService.sarlaftBusinessService.GetCompanyByDocumentNumberAndSearchTypeList(documentNum, searchType);
                if (company != null)
                {
                    foreach (var item in company)
                    {
                        personDTO.Add(AplicationAssembler.CreatePerson(item));
                    }
                }
            }
            return personDTO;
        }

        public List<SarlaftDTO> GetSarlaft(int individualId)
        {
            List<SarlaftDTO> sarlaftsDTO = new List<SarlaftDTO>();

            List<CompanyIndividualSarlaft> individualsSarlaft = DelegateService.sarlaftBusinessService.GetSarlaft(individualId);

            if (individualsSarlaft.Count > 0)
            {
                sarlaftsDTO = AplicationAssembler.CreateIndividualsSarlaft(individualsSarlaft);
            }

            return sarlaftsDTO;
        }

        public SarlaftDTO GetLastSarlaftId(PersonDTO person, int UserLogged)
        {
            SarlaftDTO sarlaftDTO = new SarlaftDTO();

            CompanyIndividualSarlaft individualsSarlaft = DelegateService.sarlaftBusinessService.GetLastSarlaftId(person.IndividualId);

            if (individualsSarlaft != null)
            {
                sarlaftDTO = AplicationAssembler.CreateIndividualSarlaft(individualsSarlaft);
            }

            return sarlaftDTO;
        }

        public CustomerKnowledgeDTO CreateSarlaft(CustomerKnowledgeDTO customerKnowledgeDTO, bool validatePolicies = true)
        {
            CompanyIndividualSarlaft sarlaft = ModelAssembler.CreateSarlaft(customerKnowledgeDTO.SarlaftDTO);
            CompanyCoSarlaft coSarlaft = ModelAssembler.CreateCoSarlaft(customerKnowledgeDTO.CoSarlaftDTO);
            CompanyFinancialSarlaft financialsarlaft = ModelAssembler.CreateFinancialSarlaft(customerKnowledgeDTO.FinancialSarlaftDTO);

            List<CompanySarlaftOperation> sarlaftOperations = new List<CompanySarlaftOperation>();
            if (customerKnowledgeDTO.InternationalOperationDTO != null)
            {
                sarlaftOperations = ModelAssembler.CreateInternationalOperations(customerKnowledgeDTO.InternationalOperationDTO);
            }

            List<CompanyIndividualPartner> individualPartners = new List<CompanyIndividualPartner>();
            if (customerKnowledgeDTO.PartnerDTO != null)
            {
                individualPartners = ModelAssembler.CreatePartners(customerKnowledgeDTO.PartnerDTO);
            }

            List<CompanyIndvidualLink> individualLinks = new List<CompanyIndvidualLink>();
            if (customerKnowledgeDTO.LinksDTO != null)
            {
                individualLinks = ModelAssembler.CreateIndividualLinks(customerKnowledgeDTO.LinksDTO);
            }

            List<CompanyLegalRepresentative> legalRepresentative = new List<CompanyLegalRepresentative>();
            if (customerKnowledgeDTO.LegalRepresentDTO != null)
            {
                legalRepresentative = ModelAssembler.CreateLegalRepresentatives(customerKnowledgeDTO.LegalRepresentDTO);
            }

            //Implementacion politicas

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies)
            {
                infringementPolicies.AddRange(DelegateService.sarlaftBusinessService.ValidateAuthorizationPoliciesSarlaft(sarlaftOperations, legalRepresentative, individualPartners, individualLinks, sarlaft, coSarlaft, financialsarlaft));
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    customerKnowledgeDTO.OperationId = this.CreateCompanySarlaftOperation(
                            new TmpSarlaftOperationDTO
                            {
                                IndividualId = customerKnowledgeDTO.SarlaftDTO.IndividualId,
                                SarlaftId = customerKnowledgeDTO.SarlaftDTO.Id,
                                Operation = JsonConvert.SerializeObject(customerKnowledgeDTO),
                                TypeProccess = "Create Sarlaft",
                                FunctionId = (int)ENUMCOAP.TypeFunction.SarlaftGeneral,
                                Proccess = ENUMCOAP.TypeFunction.SarlaftGeneral.ToString()
                            }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                var companyCustomerKnowledge = DelegateService.sarlaftBusinessService.CreateCompanySarlaft(sarlaft, financialsarlaft);
                var sarlaftDTO = new SarlaftDTO();
                sarlaftDTO = AplicationAssembler.CreateIndividualSarlaft(companyCustomerKnowledge.Sarlaft);
                var financialSarlaftDTO = new FinancialSarlaftDTO();
                financialSarlaftDTO = AplicationAssembler.CreatefinancialSarlaft(companyCustomerKnowledge.FinancialSarlaft);

                List<LinkDTO> linksDTO = new List<LinkDTO>();

                List<LegalRepresentativeDTO> legalRepresentantDTO = new List<LegalRepresentativeDTO>();
                List<PartnersDTO> listPartnerDTO = new List<PartnersDTO>();
                List<InternationalOperationDTO> listInternationalOperationDTO = new List<InternationalOperationDTO>();
                PepsDTO pepsDTO = new PepsDTO();
                CoSarlaftDTO coSarlaftDTO = new CoSarlaftDTO();
                SarlaftExonerationtDTO exonerationtDTO = new SarlaftExonerationtDTO();

                List<InternationalOperationDTO> listToSaveIntOperations = new List<InternationalOperationDTO>();

                if (customerKnowledgeDTO.InternationalOperationDTO?.Count() > 0)
                {
                    foreach (InternationalOperationDTO internationalOperation in customerKnowledgeDTO.InternationalOperationDTO)
                    {
                        internationalOperation.SarlaftId = sarlaftDTO.Id;
                        listInternationalOperationDTO.Add(internationalOperation);
                    }
                }

                if (customerKnowledgeDTO.LinksDTO != null && customerKnowledgeDTO.LinksDTO.Count > 0)
                {
                    customerKnowledgeDTO.LinksDTO.ForEach(x => x.SarlaftId = sarlaftDTO.Id);
                    linksDTO = ExecuteOperationLink(customerKnowledgeDTO.LinksDTO);
                }

                if (customerKnowledgeDTO.CoSarlaftDTO != null)
                {
                    customerKnowledgeDTO.CoSarlaftDTO.individualid = customerKnowledgeDTO.SarlaftDTO.IndividualId;
                    customerKnowledgeDTO.CoSarlaftDTO.sarlaftid = companyCustomerKnowledge.Sarlaft.Id;
                    coSarlaftDTO = SaveCoSarlaft(customerKnowledgeDTO.CoSarlaftDTO);
                }

                if (customerKnowledgeDTO.PartnerDTO != null && customerKnowledgeDTO.PartnerDTO.Count > 0)
                {
                    foreach (var item in customerKnowledgeDTO.PartnerDTO)
                    {
                        item.SarlaftId = sarlaftDTO.Id;
                        if (item.FinalBeneficiary != null && item.FinalBeneficiary.Count > 0)
                        {
                            item.FinalBeneficiary.ForEach(x => x.SarlaftId = sarlaftDTO.Id);
                        }
                        listPartnerDTO.Add(SavePartner(item));
                    }
                }

                var partnerDTO = listPartnerDTO;

                if (listInternationalOperationDTO != null && listInternationalOperationDTO.Count > 0)
                {
                    foreach (var item in listInternationalOperationDTO)
                    {
                        listToSaveIntOperations.Add(ExecuteOperation(item));
                    }
                }

                if (customerKnowledgeDTO.PepsDTO != null)
                {
                    customerKnowledgeDTO.PepsDTO.SarlaftId = sarlaftDTO.Id;
                    pepsDTO = SavePeps(customerKnowledgeDTO.PepsDTO);
                }

                if (customerKnowledgeDTO.SarlaftExonerationtDTO != null)
                {
                    customerKnowledgeDTO.SarlaftExonerationtDTO.IndividualId = customerKnowledgeDTO.SarlaftDTO.IndividualId;
                    exonerationtDTO = SaveExonerationt(customerKnowledgeDTO.SarlaftExonerationtDTO);

                }

                if (customerKnowledgeDTO.LegalRepresentDTO != null && customerKnowledgeDTO.LegalRepresentDTO.Count > 0)
                {
                    foreach (LegalRepresentativeDTO item in customerKnowledgeDTO.LegalRepresentDTO)
                    {
                        item.SarlaftId = sarlaftDTO.Id;
                        legalRepresentantDTO.Add(SaveLegalRepresentative(item));
                    }
                }


                var internationalDTO = listToSaveIntOperations;

                customerKnowledgeDTO = new CustomerKnowledgeDTO
                {
                    SarlaftDTO = sarlaftDTO,
                    FinancialSarlaftDTO = financialSarlaftDTO,
                    LinksDTO = linksDTO,
                    LegalRepresentDTO = legalRepresentantDTO,
                    PartnerDTO = partnerDTO,
                    InternationalOperationDTO = internationalDTO,
                    PepsDTO = pepsDTO,
                    CoSarlaftDTO = coSarlaftDTO,
                    SarlaftExonerationtDTO = exonerationtDTO

                };
            }

            customerKnowledgeDTO.InfringementPolicies = infringementPolicies;
            return customerKnowledgeDTO;
        }

        public CustomerKnowledgeDTO UpdateSarlaft(CustomerKnowledgeDTO customerKnowledgeDTO, bool validatePolicies = true)
        {
            CompanyIndividualSarlaft sarlaft = ModelAssembler.CreateSarlaft(customerKnowledgeDTO.SarlaftDTO);
            CompanyCoSarlaft coSarlaft = ModelAssembler.CreateCoSarlaft(customerKnowledgeDTO.CoSarlaftDTO);
            CompanyFinancialSarlaft financialsarlaft = ModelAssembler.CreateFinancialSarlaft(customerKnowledgeDTO.FinancialSarlaftDTO);

            List<CompanySarlaftOperation> sarlaftOperations = new List<CompanySarlaftOperation>();
            if (customerKnowledgeDTO.InternationalOperationDTO != null)
            {
                sarlaftOperations = ModelAssembler.CreateInternationalOperations(customerKnowledgeDTO.InternationalOperationDTO);
            }

            List<CompanyLegalRepresentative> legalRepresentative = new List<CompanyLegalRepresentative>();
            if (customerKnowledgeDTO.LegalRepresentDTO != null)
            {
                legalRepresentative = ModelAssembler.CreateLegalRepresentatives(customerKnowledgeDTO.LegalRepresentDTO);
            }

            List<CompanyIndividualPartner> individualPartners = new List<CompanyIndividualPartner>();
            if (customerKnowledgeDTO.PartnerDTO != null)
            {
                individualPartners = ModelAssembler.CreatePartners(customerKnowledgeDTO.PartnerDTO);
            }

            List<CompanyIndvidualLink> individualLinks = new List<CompanyIndvidualLink>();
            if (customerKnowledgeDTO.LinksDTO != null)
            {
                individualLinks = ModelAssembler.CreateIndividualLinks(customerKnowledgeDTO.LinksDTO);
            }

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                infringementPolicies.AddRange(DelegateService.sarlaftBusinessService.ValidateAuthorizationPoliciesSarlaft(sarlaftOperations, legalRepresentative, individualPartners, individualLinks, sarlaft, coSarlaft, financialsarlaft));
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    customerKnowledgeDTO.OperationId = this.CreateCompanySarlaftOperation(
                            new TmpSarlaftOperationDTO
                            {
                                IndividualId = customerKnowledgeDTO.SarlaftDTO.IndividualId,
                                SarlaftId = customerKnowledgeDTO.SarlaftDTO.Id,
                                Operation = JsonConvert.SerializeObject(customerKnowledgeDTO),
                                TypeProccess = "Update Sarlaft",
                                FunctionId = (int)ENUMCOAP.TypeFunction.SarlaftGeneral,
                                Proccess = ENUMCOAP.TypeFunction.SarlaftGeneral.ToString()
                            }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                List<LinkDTO> linksDTO = new List<LinkDTO>();

                List<LegalRepresentativeDTO> legalRepresentantDTO = new List<LegalRepresentativeDTO>();
                List<PartnersDTO> listPartnerDTO = new List<PartnersDTO>();
                List<InternationalOperationDTO> listInternationalOperationDTO = new List<InternationalOperationDTO>();

                PepsDTO pepsDTO = new PepsDTO();
                CoSarlaftDTO coSarlaftDTO = new CoSarlaftDTO();
                SarlaftExonerationtDTO exonerationtDTO = new SarlaftExonerationtDTO();


                if (customerKnowledgeDTO.LinksDTO != null && customerKnowledgeDTO.LinksDTO.Count > 0)
                {
                    foreach (var item in customerKnowledgeDTO.LinksDTO)
                    {
                        item.Status = (int)SarlaftStatus.Update;
                    }
                    customerKnowledgeDTO.LinksDTO.ForEach(x => x.SarlaftId = sarlaft.Id);
                    linksDTO = ExecuteOperationLink(customerKnowledgeDTO.LinksDTO);
                }

                if (customerKnowledgeDTO.LegalRepresentDTO != null && customerKnowledgeDTO.LegalRepresentDTO.Count > 0)
                {
                    foreach (LegalRepresentativeDTO item in customerKnowledgeDTO.LegalRepresentDTO)
                    {
                        if (item.IndividualId > 0)
                        {
                            item.SarlaftId = sarlaft.Id;
                            legalRepresentantDTO.Add(SaveLegalRepresentative(item));
                        }
                    }
                }

                if (customerKnowledgeDTO.PartnerDTO != null && customerKnowledgeDTO.PartnerDTO.Count > 0)
                {
                    foreach (var item in customerKnowledgeDTO.PartnerDTO)
                    {
                        item.SarlaftId = sarlaft.Id;
                        if (item.FinalBeneficiary != null && item.FinalBeneficiary.Count > 0)
                        {
                            item.FinalBeneficiary.ForEach(x => x.SarlaftId = sarlaft.Id);
                        }
                        listPartnerDTO.Add(SavePartner(item));
                    }
                }

                var partnerDTO = listPartnerDTO;

                if (customerKnowledgeDTO.InternationalOperationDTO != null && customerKnowledgeDTO.InternationalOperationDTO.Count > 0)
                {
                    foreach (var item in customerKnowledgeDTO.InternationalOperationDTO)
                    {
                        listInternationalOperationDTO.Add(ExecuteOperation(item));
                    }
                }

                if (customerKnowledgeDTO.CoSarlaftDTO != null)
                {
                    customerKnowledgeDTO.CoSarlaftDTO.individualid = customerKnowledgeDTO.SarlaftDTO.IndividualId;
                    customerKnowledgeDTO.CoSarlaftDTO.sarlaftid = customerKnowledgeDTO.SarlaftDTO.Id;
                    coSarlaftDTO = SaveCoSarlaft(customerKnowledgeDTO.CoSarlaftDTO);
                }

                if (customerKnowledgeDTO.PepsDTO != null && customerKnowledgeDTO.PepsDTO.Individual_Id != 0)
                {
                    customerKnowledgeDTO.PepsDTO.SarlaftId = sarlaft.Id;
                    pepsDTO = SavePeps(customerKnowledgeDTO.PepsDTO);
                }

                if (customerKnowledgeDTO.SarlaftExonerationtDTO != null)
                {
                    customerKnowledgeDTO.SarlaftExonerationtDTO.IndividualId = customerKnowledgeDTO.SarlaftDTO.IndividualId;
                    exonerationtDTO = SaveExonerationt(customerKnowledgeDTO.SarlaftExonerationtDTO);

                }


                var internationalDTO = listInternationalOperationDTO;

                var companyCustomerKnowledge = DelegateService.sarlaftBusinessService.UpdateCompanySarlaft(sarlaft, financialsarlaft);
                var sarlaftDTO = new SarlaftDTO();
                sarlaftDTO = AplicationAssembler.CreateIndividualSarlaft(companyCustomerKnowledge.Sarlaft);
                var financialSarlaftDTO = new FinancialSarlaftDTO();
                financialSarlaftDTO = AplicationAssembler.CreatefinancialSarlaft(companyCustomerKnowledge.FinancialSarlaft);

                customerKnowledgeDTO = new CustomerKnowledgeDTO
                {
                    SarlaftDTO = sarlaftDTO,
                    FinancialSarlaftDTO = financialSarlaftDTO,
                    LinksDTO = linksDTO,
                    LegalRepresentDTO = legalRepresentantDTO,
                    PartnerDTO = partnerDTO,
                    InternationalOperationDTO = internationalDTO,
                    PepsDTO = pepsDTO,
                    CoSarlaftDTO = coSarlaftDTO,
                    SarlaftExonerationtDTO = exonerationtDTO
                };

            }

            customerKnowledgeDTO.InfringementPolicies = infringementPolicies;
            return customerKnowledgeDTO;
        }

        // <summary>
        // Obtiene los datos sarlaft por sarlaftId
        // </summary>
        // <param name = "sarlaftId" > Parámetro para consultar Información sarlaft.</param>
        // <returns>Retorna el Resultado del sarlaft.</returns>
        public CustomerKnowledgeDTO GetSarlaftBySarlaftId(int sarlaftId)
        {
            CompanyCustomerKnowledge companyCustomerKnowledge = DelegateService.sarlaftBusinessService.GetSarlaftBySarlaftId(sarlaftId);
            var sarlaftDTO = new SarlaftDTO();
            sarlaftDTO = AplicationAssembler.CreateIndividualSarlaft(companyCustomerKnowledge.Sarlaft);

            var financialSarlaftDTO = new FinancialSarlaftDTO();
            financialSarlaftDTO = AplicationAssembler.CreatefinancialSarlaft(companyCustomerKnowledge.FinancialSarlaft);

            var linksDTO = AplicationAssembler.CreateLinks(DelegateService.sarlaftBusinessService.GetIndividualLinksByIndividualId(sarlaftDTO.IndividualId, sarlaftId));
            var legalRepresentDTO = AplicationAssembler.CreateLegalRepresentatives(DelegateService.sarlaftBusinessService.GetLegalRepresentativeByIndividualId(sarlaftDTO.IndividualId, sarlaftId));
            var partnerDTO = AplicationAssembler.CreatePartners(DelegateService.sarlaftBusinessService.GetCompanyPartnerByIndividualId(sarlaftDTO.IndividualId, sarlaftId));
            var internationalOperationDTO = AplicationAssembler.CreateInternationalOperations(DelegateService.sarlaftBusinessService.GetInternationalOperationsBySarlaftId(sarlaftId));

            var pepsDTO = new PepsDTO();
            pepsDTO = AplicationAssembler.CreatePeps(DelegateService.sarlaftBusinessService.GetIndividualPepsByIndividualId(sarlaftDTO.IndividualId, sarlaftId));

            var coSarlaftDTO = new CoSarlaftDTO();
            coSarlaftDTO = AplicationAssembler.CreateCoSarlaft(DelegateService.sarlaftBusinessService.GetIndividualCoSarlft(sarlaftDTO.IndividualId, sarlaftId));

            var ExonerationDTO = new SarlaftExonerationtDTO();

            CustomerKnowledgeDTO customerKnowledgeDTO = new CustomerKnowledgeDTO()
            {
                SarlaftDTO = sarlaftDTO,
                FinancialSarlaftDTO = financialSarlaftDTO,
                LinksDTO = linksDTO,
                LegalRepresentDTO = legalRepresentDTO,
                PartnerDTO = partnerDTO,
                InternationalOperationDTO = internationalOperationDTO,
                PepsDTO = pepsDTO,
                CoSarlaftDTO = coSarlaftDTO,
                SarlaftExonerationtDTO = ExonerationDTO
            };

            return customerKnowledgeDTO;
        }

        public List<SarlaftExonerationtDTO> GetSarlaftExoneration(int individualId)
        {
            List<SarlaftExonerationtDTO> sarlaftExonerationDTO = new List<SarlaftExonerationtDTO>();

            List<CompanySarlaftExoneration> individualsSarlaft = DelegateService.sarlaftBusinessService.GetSarlaftExoneration(individualId);

            if (individualsSarlaft.Count > 0)
            {
                sarlaftExonerationDTO = AplicationAssembler.CreateSarlaftExoneration(individualsSarlaft);
            }

            return sarlaftExonerationDTO;
        }

        public List<EconomicActivityDTO> GetEconomicActivities(string description)
        {
            return AplicationAssembler.CreateEconomicActivities(DelegateService.sarlaftBusinessService.GetEconomicActivities(description));
        }

        public TmpSarlaftOperationDTO GetCompanyTmpSarlaftOperation(int operationId)
        {
            TmpSarlaftOperationDTO tmpSarlaftOperationDTO = new TmpSarlaftOperationDTO();

            CompanyTmpSarlaftOperation companyTmpSarlaftOperation = DelegateService.sarlaftBusinessService.GetCompanyTmpSarlaftOperation(operationId);

            if (companyTmpSarlaftOperation != null)
            {
                tmpSarlaftOperationDTO = AplicationAssembler.CreateTmpSarlaftOperation(companyTmpSarlaftOperation);
            }

            return tmpSarlaftOperationDTO;

        }

        public TmpSarlaftOperationDTO CreateCompanySarlaftOperation(TmpSarlaftOperationDTO tmpSarlaftOperationDTO)
        {
            CompanyTmpSarlaftOperation companyTmpSarlaftOperation = DelegateService.sarlaftBusinessService.CreateCompanySarlaftOperation(ModelAssembler.CreateTmpSarlaftOperation(tmpSarlaftOperationDTO));

            if (companyTmpSarlaftOperation != null)
            {
                tmpSarlaftOperationDTO = AplicationAssembler.CreateTmpSarlaftOperation(companyTmpSarlaftOperation);
            }

            return tmpSarlaftOperationDTO;

        }
        #endregion

        #region Links

        public List<RelationShipDTO> GetRelationship()
        {
            return AplicationAssembler.CreateRelationShips(DelegateService.sarlaftBusinessService.GetRelationship());
        }

        public List<LinkDTO> GetIndividualLinksByIndividualId(int individualId, int sarlaftId)
        {
            return AplicationAssembler.CreateLinks(DelegateService.sarlaftBusinessService.GetIndividualLinksByIndividualId(individualId, sarlaftId));
        }

        public List<LinkDTO> ExecuteOperationLink(List<LinkDTO> linkDTOs)
        {
            List<LinkDTO> links = new List<LinkDTO>();
            foreach (LinkDTO linkDTO in linkDTOs)
            {
                switch (linkDTO.Status)
                {
                    case (int)SarlaftStatus.Create:
                        links.Add(AplicationAssembler.CreateLink(DelegateService.sarlaftBusinessService.CreateIndividualLink(ModelAssembler.CreateIndividualLink(linkDTO))));
                        break;
                    case (int)SarlaftStatus.Update:
                        links.Add(AplicationAssembler.CreateLink(DelegateService.sarlaftBusinessService.UpdateIndividualLink(ModelAssembler.CreateIndividualLink(linkDTO))));
                        break;
                }
            }

            return links;
        }

        #endregion

        #region LegalRepresentative
        public LegalRepresentativeDTO SaveLegalRepresentative(LegalRepresentativeDTO legalRepresentative)
        {
            switch (legalRepresentative.Status)
            {
                case (int)SarlaftStatus.Create:
                    // if (legalRepresentative.IsMain)
                    // {
                    legalRepresentative = AplicationAssembler.CreateLegalRepresentative(DelegateService.sarlaftBusinessService.CreateLegalRepresentative(ModelAssembler.CreateLegalRepresentative(legalRepresentative)));
                    //}
                    //else
                    //{
                    //    legalRepresentative = AplicationAssembler.CreateLegalRepresentative(DelegateService.sarlaftBusinessService.CreateSubstituteLegalRepresentative(ModelAssembler.CreateLegalRepresentative(legalRepresentative)));
                    //}
                    break;
                case (int)SarlaftStatus.Update:
                    //if (legalRepresentative.IsMain)
                    //{
                    legalRepresentative = AplicationAssembler.CreateLegalRepresentative(DelegateService.sarlaftBusinessService.UpdateLegalRepresentative(ModelAssembler.CreateLegalRepresentative(legalRepresentative)));
                    //}
                    //else
                    //{
                    //    legalRepresentative = AplicationAssembler.CreateLegalRepresentative(DelegateService.sarlaftBusinessService.UpdateSubstituteLegalRepresentative(ModelAssembler.CreateLegalRepresentative(legalRepresentative)));
                    //}
                    break;
            }
            return legalRepresentative;
        }

        public List<LegalRepresentativeDTO> GetLegalRepresentativeByIndividualId(int individualId, int sarlaftId)
        {
            return AplicationAssembler.CreateLegalRepresentatives(DelegateService.sarlaftBusinessService.GetLegalRepresentativeByIndividualId(individualId, sarlaftId));
        }
        #endregion

        #region Partners
        public PartnersDTO SavePartner(PartnersDTO partnerReturn)
        {
            switch (partnerReturn.Status)
            {
                case (int)SarlaftStatus.Create:
                    partnerReturn = AplicationAssembler.CreatePartner(DelegateService.sarlaftBusinessService.CreatePartner(ModelAssembler.CreatePartner(partnerReturn)));
                    break;
                case (int)SarlaftStatus.Update:
                    partnerReturn = AplicationAssembler.CreatePartner(DelegateService.sarlaftBusinessService.UpdatePartner(ModelAssembler.CreatePartner(partnerReturn)));
                    break;
                case (int)SarlaftStatus.Delete:
                    if (partnerReturn.Id != 0)
                    {
                        DelegateService.sarlaftBusinessService.DeletePartner(ModelAssembler.CreatePartner(partnerReturn));
                    }

                    break;
            }
            return partnerReturn;
        }



        public List<PartnersDTO> GetPartnersByIndividualId(int individualId, int sarlaftId)
        {
            return AplicationAssembler.CreatePartners(DelegateService.sarlaftBusinessService.GetCompanyPartnerByIndividualId(individualId, sarlaftId));
        }

        #endregion

        #region InternationalOperations

        public List<InternationalOperationDTO> GetInternationalOperationsBySarlaftId(int sarlaftId)
        {
            return AplicationAssembler.CreateInternationalOperations(DelegateService.sarlaftBusinessService.GetInternationalOperationsBySarlaftId(sarlaftId));
        }

        public InternationalOperationDTO ExecuteOperation(InternationalOperationDTO internationalOperationDTO)
        {
            switch (internationalOperationDTO.Status)
            {
                case (int)SarlaftStatus.Create:
                    internationalOperationDTO = AplicationAssembler.CreateInternationalOperation(DelegateService.sarlaftBusinessService.CreateInternationalOperation(ModelAssembler.CreateInternationalOperation(internationalOperationDTO)));
                    break;
                case (int)SarlaftStatus.Update:
                    internationalOperationDTO = AplicationAssembler.CreateInternationalOperation(DelegateService.sarlaftBusinessService.UpdateInternationalOperation(ModelAssembler.CreateInternationalOperation(internationalOperationDTO)));
                    break;
                case (int)SarlaftStatus.Delete:
                    if (internationalOperationDTO.Id != 0)
                    {
                        DelegateService.sarlaftBusinessService.DeleteInternationalOperation(ModelAssembler.CreateInternationalOperation(internationalOperationDTO));
                    }

                    break;
            }
            return internationalOperationDTO;
        }

        #endregion

        #region Peps

        public PepsDTO SavePeps(PepsDTO PepsReturn)
        {
            PepsReturn = AplicationAssembler.CreatePeps(DelegateService.sarlaftBusinessService.CreatePeps(ModelAssembler.CreatePeps(PepsReturn)));
            return PepsReturn;
        }

        public PepsDTO GetPepsByIndividualId(int individualId, int sarlaftId)
        {
            return AplicationAssembler.CreatePeps(DelegateService.sarlaftBusinessService.GetIndividualPepsByIndividualId(individualId, sarlaftId));
        }

        #endregion

        #region Cosarlaft
        public CoSarlaftDTO SaveCoSarlaft(CoSarlaftDTO CoSarlaftReturn)
        {
            CoSarlaftReturn = AplicationAssembler.CreateCoSarlaft(DelegateService.sarlaftBusinessService.CreateCoSarlaft(ModelAssembler.CreateCoSarlaft(CoSarlaftReturn)));

            return CoSarlaftReturn;
        }

        public CoSarlaftDTO GetCosarlftByIndividualId(int individualId)
        {
            return new CoSarlaftDTO();
        }
        #endregion


        #region Combos
        public List<RolDTO> GetRoles()
        {
            return AplicationAssembler.CreateeRoles(DelegateService.sarlaftBusinessService.GetRoles());
        }

        public List<EntityDTO> GetCategory()
        {
            return AplicationAssembler.CreateEntity(DelegateService.sarlaftBusinessService.GetCategory());
        }

        public List<EntityDTO> GetAffinity()
        {
            return AplicationAssembler.CreateEntity(DelegateService.sarlaftBusinessService.GetAffinity());
        }


        public List<EntityDTO> GetRelation()
        {
            return AplicationAssembler.CreateEntity(DelegateService.sarlaftBusinessService.GetRelation());
        }

        public List<EntityDTO> GetOppositor()
        {
            return AplicationAssembler.CreateEntity(DelegateService.sarlaftBusinessService.GetOppositor());
        }

        public List<EntityDTO> GetSociety()
        {
            return AplicationAssembler.CreateEntity(DelegateService.sarlaftBusinessService.GetSociety());
        }

        public List<EntityDTO> GetNationality()
        {
            return AplicationAssembler.CreateEntity(DelegateService.sarlaftBusinessService.GetNationality());
        }
        #endregion

        #region Exonerationt
        public SarlaftExonerationtDTO SaveExonerationt(SarlaftExonerationtDTO ExonerationtReturn)
        {
            ExonerationtReturn = ModelAssembler.CreateExoneration(DelegateService.uniquePersonService.UpdateSarlaftExoneration(ModelAssembler.CreateExoneration(ExonerationtReturn), ExonerationtReturn.IndividualId));

            return ExonerationtReturn;
        }
        #endregion Exonerationt


        #region InterviewManager
        public List<string> GetInterviewManagerByDescription(string InterviewManager)
        {
            //return new List<string>() { "Carlos","Roberto"};
            return DelegateService.sarlaftBusinessService.GetInterviewManagerByDescription(InterviewManager);
        }

        public List<string> GetInterviewManagerByDescriptionSarlaft(string InterviewManager)
        {
            //return new List<string>() { "Carlos","Roberto"};
            return DelegateService.sarlaftBusinessService.GetInterviewManagerByDescriptionSarlaft(InterviewManager);
        }
        #endregion
    }
}
