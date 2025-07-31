using Sistran.Core.Application.Utilities.Error;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using System;
using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Company.Application.UniquePersonAplicationServices
{
    [ServiceContract]
    public interface IUniquePersonAplicationService
    {
        #region Person
        [OperationContract]
        PersonDTO CreateAplicationPerson(PersonDTO personDTO, bool validatePolicies = true);

        [OperationContract]
        PersonDTO UpdateAplicationPerson(PersonDTO personDTO, bool validatePolicies = true);

        [OperationContract]
        List<PersonDTO> GetAplicationPersonByDocument(string documentNumber);


        [OperationContract]
        List<PersonDTO> GetAplicationPersonAdv(PersonDTO person);

        [OperationContract]
        PersonDTO GetAplicationPersonById(int id);

        [OperationContract]
        PersonDTO UpdateApplicationPersonBasicInfo(PersonDTO person);
        #endregion

        #region Company
        [OperationContract]
        CompanyDTO CreateAplicationCompany(CompanyDTO companyDTO, bool validatePolicies = true);

        [OperationContract]
        CompanyDTO UpdateAplicationCompany(CompanyDTO personDTO, bool validatePolicies = true);

        [OperationContract]
        List<CompanyDTO> GetAplicationCompanyByDocument(string documentNumber);

        [OperationContract]
        List<CompanyDTO> GetAplicationCompanyAdv(CompanyDTO person);

        [OperationContract]
        CompanyDTO GetAplicationCompanyById(int id);
        #endregion

        #region Insured
        [OperationContract]
        InsuredDTO GetAplicationInsuredByIndividualId(int id);

        [OperationContract]
        InsuredDTO GetAplicationInsuredElectronicBillingByIndividualId(int individualId);

        [OperationContract]
        InsuredDTO CreateAplicationInsured(InsuredDTO insuredDTO, bool validatePolicies = true);

        [OperationContract]
        InsuredDTO UpdateAplicationInsured(InsuredDTO insuredDTO, bool validatePolicies = true);
        #endregion

        [OperationContract] 
        InsuredDTO UpdateAplicationInsuredElectronicBilling(InsuredDTO insuredDTO);

        #region Supplier v1

        [OperationContract]
        List<SupplierDeclinedTypeDTO> GetAplicationSupplierDeclinedTypes();

        [OperationContract]
        List<GroupSupplierDTO> GetAplicationGroupSupplierDTO();

        [OperationContract]
        List<SupplierAccountingConceptDTO> GetAplicationSupplierAccountingConceptsBySupplierId(int SupplierId);

        [OperationContract]
        List<AccountingConceptDTO> GetAplicationAccountingConcepts();

        [OperationContract]
        List<SupplierProfileDTO> GetAplicationSupplierProfiles(int suppilierTypeId);

        [OperationContract]
        List<SupplierTypeDTO> GetAplicationSupplierTypes();

        [OperationContract]
        ProviderDTO CreateAplicationSupplier(ProviderDTO insuredDTO, bool validatePolicies = true);

        [OperationContract]
        ProviderDTO UpdateAplicationSupplier(ProviderDTO insuredDTO, bool validatePolicies = true);

        [OperationContract]
        ProviderDTO GetAplicationSupplierByIndividualId(int id);

        #endregion Supplier v1

        #region Agent
        [OperationContract]
        AgentDTO ProcessAplicationAgent(AgentDTO agentDTOs, bool validatePolicies = true);

        [OperationContract]
        AgentDTO GetAplicationAgentByIndividualId(int id);
        [OperationContract]
        AgentDTO CreateAplicationAgent(AgentDTO agentDTO);
        [OperationContract]
        AgentDTO UpdateAplicationAgent(AgentDTO agentDTO);
        [OperationContract]
        List<AgencyDTO> GetAplicationAgencyByInvidualID(int IndividualId);
        [OperationContract]
        List<AgencyDTO> GetActiveAplicationAgencyByInvidualID(int IndividualId);
        [OperationContract]
        List<AgencyDTO> CreateAplicationAgencyByInvidualID(List<AgencyDTO> agencyDTOs, int IndividualId);
        [OperationContract]
        List<AgencyDTO> UpdateAplicationAgencyByInvidualID(List<AgencyDTO> agencyDTOs, int IndividualId);
        [OperationContract]
        List<PrefixDTO> GetPrefixAgentByInvidualId(int IndividualId);
        [OperationContract]
        List<PrefixDTO> CreatePrefixAgentByInvidualId(List<PrefixDTO> prefixDTOs, int IndividualId);
        [OperationContract]
        List<PrefixDTO> UpdatePrefixAgentByInvidualId(List<PrefixDTO> prefixDTOs, int IndividualId);
        [OperationContract]
        List<PrefixDTO> DeletePrefixAgentByInvidualId(List<PrefixDTO> prefixDTOs, int IndividualId);

        [OperationContract]
        List<ComissionAgentDTO> GetcommissionPorIndividualId(int IndividualId);
        [OperationContract]
        List<ComissionAgentDTO> CreatecommissionPorIndividualId(List<ComissionAgentDTO> comissionAgentDTOs, int IndividualId, int AgencyId);
        [OperationContract]
        List<ComissionAgentDTO> UpdatecommissionPorIndividualId(List<ComissionAgentDTO> comissionAgentDTOs, int IndividualId, int AgencyId);
        //[OperationContract]
        //List<ComissionAgentDTO> DeletecommissionPorIndividualId(List<ComissionAgentDTO> comissionAgentDTOs, int IndividualId, int AgencyId);

        [OperationContract]
        List<AgencyDTO> GetAplicationAgenciesByAgentIdDescription(int agentId, string description);

        [OperationContract]
        List<AgencyDTO> GetAplicationAgenciesByAgentId(int agentId);

        #endregion

        #region Sarlaft V1
        [OperationContract]
        List<IndividualSarlaftDTO> GetIndividualSarlaft(int individualSarlaft);
        [OperationContract]
        List<IndividualSarlaftDTO> CreateIndividualSarlaft(List<IndividualSarlaftDTO> individualSarlaftDTOs);
        [OperationContract]
        List<IndividualSarlaftDTO> UpdateIndividualSarlaft(List<IndividualSarlaftDTO> individualSarlaftDTOs);
        #endregion

        #region Coinsured
        [OperationContract]
        CompanyCoInsuredDTO CreateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO, bool validatePolicies = true);
        [OperationContract]
        CompanyCoInsuredDTO UpdateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO, bool validatePolicies = true);
        [OperationContract]
        CompanyCoInsuredDTO GetCompanyCoInsuredIndivualID(int IndividualId);
        #endregion

        #region ReInsurer

        [OperationContract]
        ReInsurerDTO GetAplicationReInsurerByIndividualId(int individualId);

        [OperationContract]
        ReInsurerDTO CreateAplicationReInsurer(ReInsurerDTO reInsurerDTO, bool validatePolicies = true);

        [OperationContract]
        ReInsurerDTO UpdateAplicationReInsurer(ReInsurerDTO reInsurerDTO, bool validatePolicies = true);
        #endregion

        #region Partners        
        /// <summary>
        /// Gets the aplication partner by document identifier document type individual identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <returns></returns>

        [OperationContract]
        PartnerDTO GetAplicationPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualId);
        /// <summary>
        /// Obtiene un asociado por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        List<PartnerDTO> GetAplicationPartnerByIndividualId(int individualId);
        /// <summary>
        /// crea un asociado 
        /// </summary>
        /// <param name="partnerDTO"></param>
        /// <returns></returns>
        [OperationContract]
        PartnerDTO CreateAplicationPartner(PartnerDTO partnerDTO);
        /// <summary>
        /// Ctualizacion de asociado
        /// </summary>
        /// <param name="partnerDTO"></param>
        /// <returns></returns>
        [OperationContract]
        PartnerDTO UpdateAplicationPartner(PartnerDTO partnerDTO);
        #endregion

        #region SarlaftPerson
        /// <summary>
        /// Obtiene sarlaft 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        SarlaftDTO GetAplicationFinancialSarlaftByIndividualId(int id);
        /// <summary>
        /// creacion de Sarlaft
        /// </summary>
        /// <param name="sarlaftDTO"></param>
        /// <returns></returns>
        [OperationContract]
        SarlaftDTO CreateAplicationFinancialSarlaft(SarlaftDTO sarlaftDTO);
        /// <summary>
        /// Actualiza Sarlaft
        /// </summary>
        /// <param name="sarlaftDTO"></param>
        /// <returns></returns>
        [OperationContract]
        SarlaftDTO UpdateAplicationFinancialSarlaft(SarlaftDTO sarlaftDTO);
        #endregion

        #region InformacionPersonaLaboral
        //PersonInformationAndLaborDTO GetPersonJobByIndividualId(int individualId);
        //PersonInformationAndLaborDTO CreatePersonJob(PersonInformationAndLaborDTO personInformationAndLabor, int individualId);
        //PersonInformationAndLaborDTO UpdatePersonJob(PersonInformationAndLaborDTO personInformationAndLabor);
        #endregion

        #region OperatingQuota       
        [OperationContract]
        List<OperatingQuotaDTO> CreateOperatingQuota(List<OperatingQuotaDTO> operatingQuotaDTO, List<OperatingQuotaEventDTO> operatingQuotaEventDTOs, bool validatePolicies = true);

        [OperationContract]
        List<OperatingQuotaDTO> GetOperatingQuotaByIndividualId(int individualId);

        [OperationContract]
        bool DeleteOperatingQuota(OperatingQuotaDTO operatingQuotaDTO);

        [OperationContract]
        OperatingQuotaDTO UpdateOperatingQuota(OperatingQuotaDTO OperatingQuota);

        #endregion

        #region IndividualpaymentMethod
        [OperationContract]
        List<IndividualPaymentMethodDTO> GetIndividualpaymentMethodByIndividualId(int individualId);

        [OperationContract]
        List<IndividualPaymentMethodDTO> CreateIndividualpaymentMethods(List<IndividualPaymentMethodDTO> individualpaymentMethodDTO, int individualId, bool validatePolicies = true);
        #endregion

        #region IndividualTaxExeption

        /// <summary>
        ///  Crea el impuesto individual 
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualTaxExeptionDTO> CreateIndividualTax(List<IndividualTaxExeptionDTO> listIndividualTaxExeptionDTO, bool validatePolicies = true);

        /// <summary>
        /// Obtiene el impuesto individual por lista
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualTaxExeptionDTO> GetIndividualTaxExeptionByIndividualId(int individualId);

        /// <summary>
        /// Elimina los datos del impuesto individual
        /// </summary>
        /// <param name="individualTaxExeptionDTO"></param>
        [OperationContract]
        void DeleteIndividualTaxExeption(IndividualTaxExeptionDTO individualTaxExeptionDTO);


        /// <summary>
        /// Actualiza los datos del impuesto individual
        /// </summary>
        /// <param name="individualTaxExeptionDTO"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualTaxExeptionDTO> UpdateIndividualTaxExeption(List<IndividualTaxExeptionDTO> individualTaxExeptionDTO, bool validatePolicies = true);

        #endregion

        #region ProspectPersonNatural

        [OperationContract]
        ProspectPersonNaturalDTO GetProspectPersonNatural(int individualId);

        [OperationContract]
        ProspectPersonNaturalDTO CreateProspectPersonNatural(ProspectPersonNaturalDTO prospectPersonNaturalDTO);

        [OperationContract]
        ProspectPersonNaturalDTO GetProspectNaturalByDocumentNumber(string documentNum);

        [OperationContract]
        ProspectPersonNaturalDTO GetProspectByDocumentNumber(string documentNum, int searchType);

        #endregion

        #region Consortiums
        //[OperationContract]
        //ConsorciatedDTO GetConsortiumInsuredCodeAndIndividualID(int InsureCode, int IndividualId);
        [OperationContract]
        List<ConsorciatedDTO> GetConsortiumByIndividualId(int InsureCode);
        [OperationContract]
        List<ConsorciatedDTO> CreateConsortium(List<ConsorciatedDTO> consorciatedDTOs, int individualId, bool validatePolicies = true);
        [OperationContract]
        List<ConsorciatedDTO> UpdateConsortium(List<ConsorciatedDTO> consorciatedDTO, bool validatePolicies = true);
        [OperationContract]
        bool DeleteConsortium(ConsorciatedDTO consorciatedDTO);
        #endregion

        #region ProspectLegal

        [OperationContract]
        ProspectLegalDTO GetProspectLegalByDocumentNumber(string documentNum);

        [OperationContract]
        ProspectLegalDTO GetProspectPersonLegal(int individualId);

        [OperationContract]
        ProspectLegalDTO CreateProspectLegal(ProspectLegalDTO prospectLegalDTO);

        [OperationContract]
        List<CompanyDTO> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType);

        #endregion

        #region CompanyCoInsured
        //[OperationContract]
        //CompanyCoInsuredDTO CreateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO);
        //[OperationContract]
        //CompanyCoInsuredDTO UpdateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO);
        //[OperationContract]
        //CompanyCoInsuredDTO GetCompanyCoInsuredIndivualID(int IndividualId);
        //[OperationContract]
        //CompanyCoInsuredDTO GetCompanyCoInsured(string IdTributary);
        #endregion

        #region Guarantees
        /// <summary>
        /// Consulta una contragarantía dado su id
        /// </summary>
        /// <param name="guaranteId"> Id de la contragarantía</param>
        /// <returns> Contragarantía según id </returns>
        [OperationContract]
        GuaranteeDTO GetInsuredGuaranteeByIdGuarantee(int guaranteId);

        /// <summary>
        /// Consulta contragarantias de un afianzado
        /// </summary>
        /// <param name="id"> Id del afianzado</param>
        /// <returns> Contragarantías </returns>
        [OperationContract]
        List<GuaranteeDTO> GetInsuredGuaranteesByIndividualId(int id);

        /// <summary>
        /// Guarda contragarantias de un afianzado
        /// </summary>
        /// <param name="listGuarantee"> Lista Contragarantías</param>
        /// <returns> Contragarantías </returns>
        [OperationContract]
        List<GuaranteeDTO> SaveInsuredGuarantees(List<GuaranteeDTO> listGuarantee);

        [OperationContract]
        List<InsuredGuaranteeDocumentationDTO> CreateAplicationInsuredGuaranteeDocumentation(List<InsuredGuaranteeDocumentationDTO> insuredGuaranteeDocumentationDTO);

        [OperationContract]
        InsuredGuaranteeDocumentationDTO GetAplicationInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId);

        /// <summary>
        /// Consulta documentación asociada a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Documentación asociada a una contragarantía </returns>
        [OperationContract]
        List<InsuredGuaranteeDocumentationDTO> GetAplicationInsuredGuaranteeDocument(int individualId, int guaranteeId);

        [OperationContract]
        List<InsuredGuaranteeDocumentationDTO> GetAplicationInsuredGuaranteeDocumentation();

        [OperationContract]
        List<GuaranteeRequiredDocumentDTO> GetAplicationInsuredGuaranteeRequiredDocumentation(int guaranteeId);


        /// <summary>
        /// Consulta ramos asociados a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        [OperationContract]
        List<InsuredGuaranteePrefixDTO> GetInsuredGuaranteePrefix(int individualId, int guaranteeId);

        /// <summary>
        /// Guarda contragarantia de un afianzado
        /// </summary>
        /// <param name="guarantee">Contragarantías</param>
        /// <returns> Contragarantía </returns>
        [OperationContract]
        GuaranteeDTO SaveInsuredGuarantee(GuaranteeDTO guarantee);

        [OperationContract]
        GuaranteeDto SaveApplicationInsuredGuarantee(GuaranteeDto guaranteeDto, bool validatePolicies = true);
        #endregion

        #region Sarlaf
        [OperationContract]
        List<IndividualSarlaftDTO> GetSarlaftByIndividualId(int IndividualId);
        [OperationContract]
        IndividualSarlaftDTO CreateSarlaftByIndividualId(IndividualSarlaftDTO individualSarlaftDTOs, int IndividualId, int ActivityEconomic);
        [OperationContract]
        IndividualSarlaftDTO UpdateSarlaftByIndividualId(IndividualSarlaftDTO individualSarlafts);
        #endregion

        #region LabourPerson
        [OperationContract]
        PersonInformationAndLabourDTO GetApplicationLabourPersonByIndividualId(int individualId);

        [OperationContract]
        PersonInformationAndLabourDTO CreateApplicationLabourPerson(PersonInformationAndLabourDTO personInformationAndLabor, int individualId, bool validatePolicies = true);
        [OperationContract]
        PersonInformationAndLabourDTO UpdateApplicationLabourPerson(PersonInformationAndLabourDTO personInformationAndLabor, bool validatePolicies = true);
        #endregion LabourPerson

        #region Legal Representative

        /// <summary>
        /// Guardar la informacion de una representante legal
        /// </summary>
        /// <param name="LegalRepresentativeDTO">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentativeDTO CreateLegalRepresentative(LegalRepresentativeDTO legalRepresentative);

        /// <summary>
        /// Buscar la infromacion de un representante legal
        /// </summary>
        /// <param name="LegalRepresentativeDTO">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentativeDTO GetLegalRepresentativeByIndividualId(int individualId);

        #endregion

        #region Supplier v1

        [OperationContract]
        List<IndividualRoleDTO> GetAplicationIndividualRoleByIndividualId(int individualId);

        #endregion Supplier v1

        #region Insured Guarantee

        [OperationContract]
        List<GuaranteeInsuredGuaranteeDTO> GetAplicationInsuredGuaranteesByIndividualId(int individualId);


        [OperationContract]
        InsuredGuaranteeMortgageDTO GetAplicationInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id);

        [OperationContract]
        InsuredGuaranteeMortgageDTO CreateAplicationInsuredGuaranteeMortgage(InsuredGuaranteeMortgageDTO insuredGuaranteeMortgage);


        [OperationContract]
        InsuredGuaranteePledgeDTO GetAplicationInsuredGuaranteePledgeByIndividualIdById(int individualId, int id);

        [OperationContract]
        InsuredGuaranteePledgeDTO CreateAplicationInsuredGuaranteePledge(InsuredGuaranteePledgeDTO insuredGuaranteePledge);


        [OperationContract]
        InsuredGuaranteePromissoryNoteDTO GetAplicationInsuredGuaranteePromissoryNoteeByIndividualIdById(int individualId, int id);

        [OperationContract]
        InsuredGuaranteePromissoryNoteDTO CreateAplicationInsuredGuaranteePromissoryNote(InsuredGuaranteePromissoryNoteDTO insuredGuaranteePromissoryNote);

        [OperationContract]
        InsuredGuaranteeFixedTermDepositDTO GetAplicationInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id);

        [OperationContract]
        InsuredGuaranteeFixedTermDepositDTO CreateAplicationInsuredGuaranteeFixedTermDeposit(InsuredGuaranteeFixedTermDepositDTO guaranteeFixedTermDeposit);


        [OperationContract]
        InsuredGuaranteeOthersDTO GetAplicationInsuredGuaranteeOthersByIndividualIdById(int individualId, int id);

        [OperationContract]
        InsuredGuaranteeOthersDTO CreateAplicationInsuredGuaranteeOthers(InsuredGuaranteeOthersDTO guaranteePromissoryOthers);

        #endregion

        #region InsuredGuaranteelog

        [OperationContract]
        List<InsuredGuaranteeLogDTO> GetAplicationInsuredGuaranteeLogByIndividualIdById(int individualId, int id);

        [OperationContract]
        InsuredGuaranteeLogDTO CreateAplicationInsuredGuaranteeLog(InsuredGuaranteeLogDTO insuredGuaranteeLogDTO);

        #endregion

        #region MaritalStatus

        [OperationContract]
        List<MaritalStatusDTO> GetAplicationMaritalStatus();

        #endregion

        #region DocumentType

        [OperationContract]
        List<DocumentTypeDTO> GetAplicationDocumentTypes(int typeDocument);

        #endregion

        #region AddressesType

        [OperationContract]
        List<AddressTypeDTO> GetAplicationAddressesTypes();

        #endregion

        #region PhoneType

        [OperationContract]
        List<PhoneTypeDTO> GetAplicationPhoneTypes();

        #endregion

        #region EmailType

        [OperationContract]
        List<EmailTypeDTO> GetAplicationEmailTypes();

        #endregion

        #region EconomicActivity

        [OperationContract]
        List<EconomicActivityDTO> GetAplicationEconomicActivities();

        #endregion

        #region AssociationType

        [OperationContract]
        List<AssociationTypeDTO> GetAplicationAssociationTypes();

        #endregion

        #region CompanyType

        [OperationContract]
        List<CompanyTypeDTO> GetAplicationCompanyTypes();

        #endregion

        #region FiscalResponsibility

        [OperationContract]
        List<FiscalResponsibilityDTO> GetAplicationCompanyFiscalResponsibility();

        [OperationContract]
        List<InsuredFiscalResponsibilityDTO> CreateIndividualFiscalResponsibility(List<InsuredFiscalResponsibilityDTO> listInsuredFiscalResponsibilityDTO);

        [OperationContract]
        List<InsuredFiscalResponsibilityDTO> GetCompanyFiscalResponsibilityByIndividualId(int individualId);
       
        [OperationContract]
        List<InsuredFiscalResponsibilityDTO> UpdateAplicationFiscalRespondibility(List<InsuredFiscalResponsibilityDTO> listFiscalDTO);
        #endregion FiscalResponsibility

        [OperationContract]
        bool DeleteFiscalResponsibility(InsuredFiscalResponsibilityDTO fiscalDTO);

        #region Insured

        [OperationContract]
        List<InsuredDeclinedTypeDTO> GetAplicationInsuredDeclinedTypes();

        [OperationContract]
        List<InsuredSegmentDTO> GetAplicationInsuredSegment();

        [OperationContract]
        List<InsuredProfileDTO> GetAplicationInsuredProfile();

        #endregion

        #region AgentType

        [OperationContract]
        List<AgentTypeDTO> GetAplicationAgentTypes();

        #endregion

        #region AgentDeclinedType

        [OperationContract]
        List<AgentDeclinedTypeDTO> GetAplicationAgentDeclinedTypes();

        #endregion

        #region GroupAgent

        [OperationContract]
        List<GroupAgentDTO> GetAplicationGroupAgent();

        #endregion

        #region SalesChannel

        [OperationContract]
        List<SalesChannelDTO> GetAplicationSalesChannel();

        #endregion

        #region EmployeePerson

        [OperationContract]
        List<EmployeePersonDTO> GetAplicationEmployeePersons();

        #endregion

        #region AllOthersDeclinedType

        [OperationContract]
        List<AllOthersDeclinedTypeDTO> GetAplicationAllOthersDeclinedTypes();

        #endregion

        #region GuarantorDTO

        [OperationContract]
        List<GuarantorDTO> GetAplicationGuarantorByindividualIdByguaranteeId(int individualId, int guaranteeId);

        #endregion
        [OperationContract]

        List<InsuredGuaranteePrefixDTO> CreateApplicationPrefixAssocieted(List<InsuredGuaranteePrefixDTO> PrefixAssocieteds);
        [OperationContract]
        List<GuarantorDTO> CreateAplicationGuarantor(List<GuarantorDTO> guarantor);

        #region Prospect
        /// <summary>
        /// Retorna un objeto tipo persona que coincide con los criterios de búsqueda
        /// </summary>
        /// <param name="individualType">Tipo de persona</param>
        /// <param name="DocumentTypeId">Identificador del tipo de documento</param>
        /// <param name="DocumentNumber">Número de documento</param>
        /// <returns>Un objeto de tipo persona</returns>
        [OperationContract]
        ProspectLigthQuotationDTO GetProspectByPersonTypeDocumentTypeDocumentNumber(Core.Services.UtilitiesServices.Enums.IndividualType individualType, int DocumentTypeId, string DocumentNumber);
        #endregion

        #region Better Performance
        /// <summary>
        /// Get the initial information for person view
        /// </summary>
        /// <param name="isEmail">Indicates if is email flag</param>
        /// <returns>List of objects</returns>
        [OperationContract]
        LoadDataDTO LoadInitialData(bool isEmail);

        /// <summary>
        /// Get the initial information for person legal view
        /// </summary>
        /// <param name="typeDocument">Type of document to filter</param>
        /// <returns>List of objects</returns>
        [OperationContract]
        LoadLegalDataDTO LoadInitialLegalData(int typeDocument);
        #endregion

        #region ThirdPerson
        [OperationContract]
        ThirdPartyDTO CreateAplicationThird(ThirdPartyDTO thirdDTO, bool validatePolicies = true);

        [OperationContract]
        ThirdPartyDTO UpdateAplicationThird(ThirdPartyDTO thirdDTO, bool validatePolicies = true);
        /// <summary>
        /// Permite consulta por individualId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        ThirdPartyDTO GetAplicationThirdByIndividualId(int id);
        #endregion

        /// <summary>
        /// Permite consulta por individualId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        EmployeeDTO CreateEmployee(EmployeeDTO employee, bool validatePolicies = true);

        /// <summary>
        /// Permite consulta por individualId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        EmployeeDTO GetEmployeeIndividualId(int individualId);


        #region BusinessName
        [OperationContract]
        List<CompanyNameDTO> CreateBusinessName(List<CompanyNameDTO> listBusinessNameDTO, bool validatePolicies = true);

        [OperationContract]
        List<BankTransfersDTO> CreateBankTransfers(List<BankTransfersDTO> listBankTransfersDTO, bool validatePolicies = true);

        [OperationContract]
        List<BankTransfersDTO> UpdateAplicationBankTransfers(List<BankTransfersDTO> listBankTransfersDTO, bool validatePolicies = true);

        [OperationContract]
        List<BankTransfersDTO> GetCompanyBankTransfersByIndividualId(int individualId);

        [OperationContract]
        List<CompanyNameDTO> UpdateAplicationBusinessName(List<CompanyNameDTO> listBusinessNameDTO, bool validatePolicies = true);

        [OperationContract]
        int CountAplicationBusinessName(int individualId);

        /// <summary>
        /// Obtiene razon social por individual 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyNameDTO> GetCompanyBusinessByIndividualId(int individualId);

        #endregion

        #region
        [OperationContract]
        PersonOperationDTO CreateAplicationPersonOperation(PersonOperationDTO personOperation);

        [OperationContract]
        PersonOperationDTO GetPersonOperation(int operationId);

        #endregion
        /// <summary>
        /// Permite consulta por individualId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        IndividualTypePersonDTO GetPersonByIndividualId(int id);

        [OperationContract]
        List<ProspectLegalDTO> GetAplicationProspectLegalAdv(ProspectLegalDTO companyDTO);


        [OperationContract]
        List<ProspectPersonNaturalDTO> GetAplicationProspectNaturalAdv(ProspectPersonNaturalDTO companyDTO);

        [OperationContract]
        Parameter GetParameterFutureSociety(int parameterFutureSociety);


    }
}
