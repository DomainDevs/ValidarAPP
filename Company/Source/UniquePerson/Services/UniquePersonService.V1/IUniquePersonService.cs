using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using company = Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Company.Application.UniquePersonServices.V1.DTOs;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;

namespace Sistran.Company.Application.UniquePersonServices.V1
{
    [ServiceContract]
    public interface IUniquePersonService : Sistran.Core.Application.UniquePersonService.V1.IUniquePersonServiceCore
    {
        #region person
        /// <summary>
        /// Crear Una nueva persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyPerson CreateCompanyPerson(CompanyPerson person);

        /// <summary>
        /// Actualizar los los datos de un persona
        /// </summary>
        /// <param name="person"> Modelo person</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyPerson UpdateCompanyPerson(Models.CompanyPerson person);


        //***************************************************
        //Address
        //***************************************************
        [OperationContract]
        List<CompanyAddress> GetCompanyAddresses(int individualId);

        [OperationContract]
        List<CompanyAddress> CreateCompanyAddresses(int individualId, List<CompanyAddress> addresses);

        [OperationContract]
        List<CompanyAddress> UpdateCompanyAddresses(int individualId, List<CompanyAddress> addresses);

        //***************************************************
        //Phones
        //***************************************************
        [OperationContract]
        List<CompanyPhone> GetCompanyPhones(int individualId);

        [OperationContract]
        List<CompanyPhone> CreateCompanyPhones(int individualId, List<CompanyPhone> addresses);

        [OperationContract]
        List<CompanyPhone> UpdateCompanyPhones(int individualId, List<CompanyPhone> addresses);

        //***************************************************
        //Emails
        //***************************************************
        [OperationContract]
        List<CompanyEmail> GetCompanyEmails(int individualId);

        [OperationContract]
        List<CompanyEmail> CreateCompanyEmails(int individualId, List<CompanyEmail> addresses);

        [OperationContract]
        List<CompanyEmail> UpdateCompanyEmails(int individualId, List<CompanyEmail> addresses);

        //***************************************************
        //person
        //***************************************************



        [OperationContract]
        List<CompanyPerson> GetCompanyPersonByDocument(CustomerType customerType, string documentNumber);

        [OperationContract]
        List<CompanyPerson> GetCompanyPersonAdv(CustomerType customerType, CompanyPerson person);

        [OperationContract]
        List<CompanyCompany> GetCompanyCompanyAdv(CustomerType customerType, CompanyCompany company);

        [OperationContract]
        CompanyEconomicActivity GetCompanyEconomicActivitiesById(int id);

        [OperationContract]
        CompanyPerson UpdateApplicationPersonBasicInfo(CompanyPerson companyPerson, bool validatePolicies = true);

        [OperationContract]
        List<CompanyPerson> GetPerson2gByDocumentNumber(string documentNumber, bool company);

        [OperationContract]
        PersonDTO GetPerson2gByPersonId(int personId, bool company);

        [OperationContract]
        CompanyDTO GetCompany2gByPersonId(int personId, bool company);
        #endregion person

        #region Reinsurer V1
        /// <summary>
        /// Obtiene reasegurador por el id del individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyReInsurer GetCompanyReInsurerByIndividualId(int individualId);

        /// <summary>
        /// Crear reasegurador
        /// </summary>
        /// <param name="reinsurer">The reinsurer.</param>
        /// <returns></returns>
        [OperationContract]
        CompanyReInsurer CreateCompanyReinsurer(CompanyReInsurer reinsurer);

        /// <summary>
        /// Actualizar reasegurador
        /// </summary>
        /// <param name="reinsurer"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyReInsurer UpdateCompanyReinsurer(CompanyReInsurer reinsurer);

        #endregion V1

        #region Agent V1
        /// <summary>
        /// Obtener lista de ramos comerciales por agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        CompanyAgent GetCompanyAgentByIndividualId(int individualId);

        /// <summary>
        /// Crea el Agente.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>
        [OperationContract]
        CompanyAgent CreateCompanyAgent(CompanyAgent companyAgent);

        /// <summary>
        /// Crea el Agente Rol.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>
        [OperationContract]
        CompanyAgent CreateCompanyAgentRol(CompanyAgent companyAgent);

        /// <summary>
        /// Actualiza el Agente.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>
        [OperationContract]
        CompanyAgent UpdateCompanyAgent(CompanyAgent companyAgent);

        /// <summary>
        /// Obtiene la la agencia por inviduoId
        /// </summary>
        /// <param name="InvidualId">Codigo del InvidualId</param>
        /// <returns></returns>
        /// 
        [OperationContract]
        List<CompanyAgency> GetCompanyAgencyByInvidualId(int InvidualId);
        [OperationContract]
        List<CompanyAgency> GetActiveCompanyAgencyByInvidualId(int InvidualId);

        /// <summary>
        /// Crea las Agencias por inviduaId
        /// </summary>
        /// <param name="companyAgencies">Modelo de Agencias.</param>
        /// <param name="IndividualId">Codigo Invidual id</param>
        /// <returns></returns>
        /// 
        [OperationContract]
        CompanyAgency CreateCompanyAgencyByInvidualId(CompanyAgency companyAgencies, int IndividualId);

        /// <summary>
        /// Actualiza Agencia por InvidualId
        /// </summary>
        /// <param name="companyAgencies"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        /// 
        [OperationContract]
        CompanyAgency UpdateCompanyAgencyByInvidualId(CompanyAgency companyAgencies, int IndividualId);
        /// <summary>
        /// Obtiene los Ramos Por IndividualID
        /// </summary>
        /// <param name="IndividualId">Codigo IndividualId</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyPrefixs> GetPrefixesByAgentIds(int IndividualId);
        /// <summary>
        /// Crea ramos por el codigo de la persona.
        /// </summary>
        /// <param name="companyPrefix">model del ramo</param>
        /// <param name="IndividualId">codigo de la persoa </param>
        /// <returns></returns>
        [OperationContract]
        CompanyPrefixs CreatePrefixesByAgentIds(CompanyPrefixs companyPrefix, int IndividualId);
        /// <summary>
        /// Actualiza ramos por el codigo de la persona.
        /// </summary>
        /// <param name="companyPrefix">model del ramo</param>
        /// <param name="IndividualId">codigo de la persoa </param>
        /// <returns></returns>
        [OperationContract]
        CompanyPrefixs UpdatePrefixesByAgentIds(CompanyPrefixs companyPrefix, int IndividualId);
        /// <summary>
        /// Elimina ramos por el codigo de la persona.
        /// </summary>
        /// <param name="companyPrefix">model del ramo</param>
        /// <param name="IndividualId">codigo de la persoa </param>
        /// <returns></returns>
        [OperationContract]
        CompanyPrefixs DeletePrefixesByAgentIds(CompanyPrefixs companyPrefix, int IndividualId);
        /// <summary>
        /// Obtiene los Comisiones por individual id 
        /// </summary>
        /// <param name="IndividualId">codigo persona</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyComissionAgent> GetCompanycommissionAgent(int IndividualId);
        /// <summary>
        /// Crea las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        [OperationContract]
        CompanyComissionAgent CreateCompanycommissionAgent(CompanyComissionAgent companyComissionAgent, int IndividualId, int AgencyId);
        /// <summary>
        /// Actuliza las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        [OperationContract]
        CompanyComissionAgent UpdateCompanycommissionAgent(CompanyComissionAgent companyComissionAgent, int IndividualId, int AgencyId);
        /// <summary>
        /// Elimina las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        [OperationContract]
        CompanyComissionAgent DeleteCompanycommissionAgent(CompanyComissionAgent companyComissionAgent);

        [OperationContract]
        List<CompanyAgency> GetCompanyAgenciesByAgentIdDescription(int agentId, string description);

        [OperationContract]
        List<CompanyAgency> GetCompanyAgenciesByAgentId(int agentId);

        #endregion Agent V1

        #region Sarlaftv1
        [OperationContract]
        List<IndividualSarlaft> GetSarlaftByNumberSarlaft(int sarlaftId);
        [OperationContract]
        IndividualSarlaft CreateSarlaftByNumberSarlaft(IndividualSarlaft financialSarlaf);
        [OperationContract]
        IndividualSarlaft UpdateSarlaftByNumberSarlaft(IndividualSarlaft financialSarlaf);
        #endregion

        #region Individual Tax V1
        /// <summary>
        /// Obtiene el impuesto individual por lista
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyIndividualTax> GetCompanyIndividualTaxExeptionByIndividualId(int individualId);

        /// <summary>
        /// Crear imuesto individual
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyIndividualTaxExeption CreateCompanyIndividualTaxExeption(CompanyIndividualTaxExeption individualTaxExeption);
        /// <summary>
        /// Crear imuesto
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyIndividualTax CreateCompanyIndividualTax(CompanyIndividualTax individualTaxExeption);

        /// <summary>
        /// Modificar imuesto
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyIndividualTax UpdateCompanyIndividualTax(CompanyIndividualTax individualTaxExeption);

        /// <summary>
        /// Modificar imuesto individual
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyIndividualTaxExeption UpdateCompanyIndividualTaxExeption(CompanyIndividualTaxExeption individualTaxExeption);

        /// <summary>
        /// Eliminar de un impuesto
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTaxExeption</param>
        /// <returns></returns>
        [OperationContract]
        void DeleteCompanyIndividualTax(CompanyIndividualTaxExeption individualTaxExeption);

        /// <summary>
        /// Eliminar de un impuesto
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTaxExeption</param>
        /// <returns></returns>
        [OperationContract]
        void DeleteCompanyIndividualTaxExeption(CompanyIndividualTax individualTax);


        #endregion Individual Tax V1

        #region OperatingQuota V1

        /// <summary>
        /// Obtiene el cupo operativo del tomador 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyOperatingQuota> GetCompanyOperatingQuotaByIndividualId(int individualId);

        /// <summary>
        /// Crea el cupo operativo del tomador 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyOperatingQuota> CreateCompanyOperatingQuota(List<CompanyOperatingQuota> listOperatingQuote);

        /// <summary>
        /// Actualiza datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CompanyOperatingQuota UpdateCompanyOperatingQuota(CompanyOperatingQuota OperatingQuota);

        /// <summary>
        /// Elimina datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool DeleteCompanyOperatingQuota(CompanyOperatingQuota OperatingQuota);

        #endregion OperatingQuota V1

        #region Supplier v1

        /// <summary>
        /// Obtener todos los SupplierAccountingConcept por Supplier
        /// </summary> 
        List<CompanySupplierAccountingConcept> GetCompanySupplierAccountingConceptsBySupplierId(int SupplierId);

        /// <summary>
        /// Obtener todos los AccountingConcept
        /// </summary> 
        List<CompanyAccountingConcept> GetCompanyAccountingConcepts();

        /// <summary>
        /// Obtener lista de GetCompanySupplierProfiles
        /// </summary> 
        [OperationContract]
        List<CompanySupplierProfile> GetCompanySupplierProfiles(int suppilierTypeId);

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary> 
        [OperationContract]
        CompanySupplier GetCompanySupplierById(int SupplierId);

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary> 
        [OperationContract]
        List<CompanySupplier> GetCompanySuppliers();

        /// <summary>
        /// Obtener lista de tipos proveedores
        /// </summary> 
        [OperationContract]
        List<CompanySupplierType> GetCompanySupplierTypes();

        /// <summary>
        /// Obtener lista de proveedores declinados
        /// </summary>   
        [OperationContract]
        List<CompanySupplierDeclinedType> GetCompanySupplierDeclinedTypes();

        /// <summary>
        /// Obtener grupo de proveedores 
        /// </summary>   
        [OperationContract]
        List<Models.CompanyGroupSupplier> GetCompanyGroupSupplier();

        /// <summary>
        /// Crear proveedor
        /// </summary>   
        [OperationContract]
        CompanySupplier CreateCompanySupplier(CompanySupplier companySupplier);

        #region Partner V1
        [OperationContract]
        CompanyPartner GetCompanyPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualId);

        [OperationContract]
        List<CompanyPartner> GetCompanyPartnerByIndividualId(int individualId);

        [OperationContract]
        CompanyPartner CreateCompanyPartner(CompanyPartner partner);

        [OperationContract]
        CompanyPartner UpdateCompanyPartner(CompanyPartner partner);
        #endregion Partner V1

        /// <summary>
        /// Actualizar Proveedor
        /// </summary> 
        [OperationContract]
        CompanySupplier UpdateCompanySupplier(CompanySupplier companySupplier);

        /// <summary>
        /// Get proveedor
        /// </summary> 
        [OperationContract]
        CompanySupplier GetCompanySupplierByIndividualId(int individualId);

        #endregion Supplier v1

        #region IndividualRole V1

        /// <summary>
        /// GetIndividualRoleByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyIndividualRole> GetCompanyIndividualRoleByIndividualId(int individualId);

        #endregion IndividualRole V1

        #region CoInsured
        [OperationContract]
        company.CompanyCoInsured GetCoInsuredIndividuald(int individual);
        [OperationContract]
        company.CompanyCoInsured CreateCoInsuredIndividuald(company.CompanyCoInsured companyCoInsured);
        [OperationContract]
        company.CompanyCoInsured UpdateCoInsuredIndividuald(company.CompanyCoInsured companyCoInsured);
        #endregion

        #region Company
        /// <summary>
        /// Obtener compañia por numero de documento
        /// </summary>
        /// <param name="documentNumber">numero de documento</param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyCompany> GetCompanyCompanyByDocument(CustomerType customerType, string documentNumber);

        [OperationContract]
        CompanyCompany UpdateApplicationCompanyBasicInfo(CompanyCompany company, bool validatePolicies = true);
        #endregion


        /// <summary>
        /// Buscar compañias y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        [OperationContract]
        new List<Models.CompanyCompany> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType);

        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns></returns>
        [OperationContract]
        new Models.CompanyCompany GetCompanyByIndividualId(int individualId);


        /// <summary>
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyCompany CreateCompanyCompany(Models.CompanyCompany company);

        /// <summary>
        ///  Actualizar Compañia
        /// </summary>
        /// <param name="company">Modelo Compañia</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyCompany UpdateCompanyCompany(Models.CompanyCompany company);



        /// <summary>
        /// Obtener Consorcios
        /// </summary>
        /// <param name="insuredCode">codigo de asegurado</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.CompanyConsortium> GetCoConsortiumsByInsuredCode(int insuredCode);

        /// <summary>
        /// Buscar Personas y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        [OperationContract]
        new List<Models.CompanyPerson> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType);


        /// <summary>
        /// Buscar persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyPerson GetComPanyPersonByIndividualId(int individualId);



        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo Company
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Direcciones de Notificación</returns>
        [OperationContract]
        List<CompanyName> CompanyGetNotificationAddressesByIndividualId(int individualId, CustomerType customerType);

        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo Company
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Direcciones de Notificación</returns>
        [OperationContract]
        List<CompanyName> CompanyGetNotificationByIndividualId(int individualId, CustomerType customerType);

        /// <summary>
        /// Obtener compañia por numero de documento
        /// </summary>
        /// <param name="documentNumber">numero de documento</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyCompany GetCompanyByDocumentNumber(string documentNumber);

        /// <summary>
        /// Obtener persona por numero de documento
        /// </summary>
        /// <param name="documentNumber">numero de documento</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyPerson GetPersonByDocumentNumber(string documentNumber);

        [OperationContract]
        Models.CoInsurerCompany CreateCoInsurer(Models.CoInsurerCompany CoInsurer);

        [OperationContract]
        Models.CoInsurerCompany GetCoInsurerByIndividualId(int individualId);


        #region Extensión CompanyInsured

        [OperationContract]
        Models.CompanyInsured CreateCompanyInsured(Models.CompanyInsured companyInsured);

        [OperationContract]
        Models.CompanyInsured GetCompanyInsuredByIndividualId(int individualId);

        [OperationContract]
        Models.CompanyInsured GetCompanyInsuredElectronicBillingByIndividualId(int individualId);

        [OperationContract]
        Models.CompanyInsured GetCompanyInsuredByIndividualCode(int individualCode);

        //        [OperationContract]
        // List<Models.CompanyInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        [OperationContract]
        Models.CompanyInsured GetCompanyInsuredByInsuredCode(int insuredCode);

        #endregion

        #region Company.ScoreTypeDoc

        /// <summary>
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito</returns>
        [OperationContract]
        List<Models.ScoreTypeDoc> GetAllScoreTypeDoc();

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de documento datacrédito
        /// </summary>
        /// <param name="listAdded">tipos de documento datacrédito para agregar</param>
        /// <param name="listEdited">tipos de documento datacrédito para editar</param>
        /// <param name="listDeleted">tipos de documento datacrédito para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        [OperationContract]
        ParametrizationResponse<Models.ScoreTypeDoc> CreateScoreTypeDocs(List<Models.ScoreTypeDoc> listAdded, List<Models.ScoreTypeDoc> listEdited, List<Models.ScoreTypeDoc> listDeleted);

        /// <summary>
        /// Generar archivo excel de tipo documento datacrédito
        /// </summary>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        string GenerateFileToScoreTypeDoc();

        #endregion
        #region asegurados
        //[OperationContract(Name = "GetCompanyInsuredsByDescriptionInsuredSearchType")]
        //List<CompanyInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType);
        #endregion asegurados

        [OperationContract]
        CompanyInsured UpdateCompanyInsured(CompanyInsured insured);

        [OperationContract]
        CompanyInsured UpdateCompanyInsuredElectronicBilling(CompanyInsured companyInsured);

        #region sarlaft

        /// <summary>
        /// Actualiza la informacion sarlaft en la tabla UP.FINANCIAL_SARLAFT
        /// </summary>
        /// <param name="sarlaft">modelo sarlaft</param>
        /// <returns></returns>
        [OperationContract]
        Models.FinancialSarlaf UpdateFinancialSarlaft(Models.FinancialSarlaf sarlaft);

        /// <summary>
        /// Gets the financial sarlaft by sarlaft identifier.
        /// </summary>
        /// <param name="sarlaftId">The sarlaft identifier.</param>
        /// <returns></returns>
        [OperationContract]
        Models.FinancialSarlaf GetCompanyFinancialSarlaftBySarlaftId(int sarlaftId);
        /// <summary>
        /// Guardar la inofrmacion sarlaft en la tabla UP.FINANCIAL_SARLAFT
        /// </summary>
        /// <param name="sarlaft">modelo sarlaft</param>
        /// <returns></returns>
        [OperationContract]
        Models.FinancialSarlaf CreateCompanyFinancialSarlaft(Models.FinancialSarlaf sarlaft);

        /// <summary>
        /// Guardar la inofrmacion sarlaft en la tabla UP.INDIVIDUAL_SARLAFT
        /// </summary>
        /// <param name="individualSarlaft"></param>
        /// <returns></returns>
        [OperationContract]
        Models.IndividualSarlaft CreateCompanyIndividualSarlaft(Models.IndividualSarlaft individualSarlaft, int IndividualId, int economiActivity);

        /// <summary>
        /// Gets the individual sarlaft by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.IndividualSarlaft> GetIndividualSarlaftByIndividualId(int individualId);


        Models.IndividualSarlaft CreateIndividualSarlaftByIndividualId(Models.IndividualSarlaft individualSarlaft, int individualId, int IdActivityEconomic);

        Models.FinancialSarlaf UpdateIndividualSarlaftByIndividualId(Models.IndividualSarlaft individualSarlaft);

        Models.IndividualSarlaft UpdateIndividualSarlaftByIndividualIds(Models.IndividualSarlaft individualSarlaft);
        #endregion

        #region PaymentMethod

        /// <summary>
        /// obtiene los metodos de pagos
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyIndividualPaymentMethod> GetIndividualpaymentMethodByIndividualId(int individualId);

        /// <summary>
        /// crea los metodos de pagos
        /// </summary>
        /// <param name="individualpaymentMethodDTO"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<CompanyIndividualPaymentMethod> CreateIndividualpaymentMethods(List<CompanyIndividualPaymentMethod> individualPaymentMethods, int individualId);
        [OperationContract]
        List<CompanyPaymentAccountType> getCompanyPaymentAccountTypes();

        #endregion

        #region Leal Representative

        /// <summary>
        /// Guardar la informacion de una representante legal
        /// </summary>
        /// <param name="CompanyLegalRepresentative">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        CompanyLegalRepresentative CreateLegalRepresentative(CompanyLegalRepresentative legalRepresent, int individualId);

        /// <summary>
        /// Buscar la infromacion de un representante legal
        /// </summary>
        /// <param name="CompanyLegalRepresentative">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        CompanyLegalRepresentative GetLegalRepresentativeByIndividualId(int individualId);

        #endregion

        #region LabourPerson
        [OperationContract]
        CompanyLabourPerson CreateCompanyLabourPerson(CompanyLabourPerson personJob, int individualId);
        /// <summary>
        /// Agregar los datos laborales de un persona
        /// </summary>
        /// <param name="person"> Modelo person</param>
        /// <returns></returns>
        [OperationContract]
        CompanyLabourPerson UpdateCompanyLabourPerson(CompanyLabourPerson personJob);

        /// <summary>
        /// Actualizar la informacion laborar de la persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        [OperationContract]
        CompanyLabourPerson GetCompanyLabourPersonByIndividualId(int individualId);
        /// <summary>
        /// Buscar información laboral
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        #endregion

        #region ProspectNatural

        [OperationContract]
        CompanyProspectNatural GetProspectNaturalByDocumentNumber(string documentNum);


        [OperationContract]
        CompanyProspectNatural GetProspectPersonNatural(int individualId);

        [OperationContract]
        CompanyProspectNatural CreateProspectPersonNatural(CompanyProspectNatural prospectNatural);

        [OperationContract]
        CompanyProspectNatural UpdateProspectPersonNatural(CompanyProspectNatural prospectNatural);

        [OperationContract]
        CompanyProspectNatural GetProspectByDocumentNumber(string documentNum, int searchType);

        #endregion

        #region ProspectLegal

        [OperationContract]
        CompanyProspectNatural GetProspectLegalByDocumentNumber(string documentNum);

        [OperationContract]
        CompanyProspectNatural GetProspectPersonLegal(int individualId);

        [OperationContract]
        CompanyProspectNatural CreateProspectPersonLegal(CompanyProspectNatural prospectNatural);

        [OperationContract]
        CompanyProspectNatural UpdateProspectPersonLegal(CompanyProspectNatural prospectNatural);

        #endregion

        #region Insured Guarantee

        /// <summary>
        /// listado de Contragarantias
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        List<CompanyGuaranteeInsuredGuarantee> GetCompanyInsuredGuaranteesByIndividualId(int individualId);

        /// <summary>
        ///  Obtener contragarantias asegurado tipo Pagaré
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyInsuredGuaranteePromissoryNote GetCompanyInsuredGuaranteePromissoryNoteByIndividualIdById(int individualId, int id);

        CompanyInsuredGuaranteePromissoryNote CreateCompanyInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote);

        CompanyInsuredGuaranteePromissoryNote UpdateCompanyInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote);


        /// <summary>
        ///  Obtener contragarantias asegurado tipo Prenda
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyInsuredGuaranteePledge GetCompanyInsuredGuaranteePledgeByIndividualIdById(int individualId, int id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyInsuredGuaranteePledge"></param>
        /// <returns></returns>
        CompanyInsuredGuaranteePledge CreateCompanyInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyInsuredGuaranteePledge"></param>
        /// <returns></returns>
        CompanyInsuredGuaranteePledge UpdateCompanyInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge);


        /// <summary>
        ///  Obtener contragarantias asegurado tipo Hipoteca
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyInsuredGuaranteeMortgage GetCompanyInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id);

        CompanyInsuredGuaranteeMortgage CreateCompanyCompanyInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage);

        CompanyInsuredGuaranteeMortgage UpdateCompanyCompanyInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage);


        /// <summary>
        ///  Obtener contragarantias asegurado tipo CDTIS
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        CompanyInsuredGuaranteeFixedTermDeposit GetCompanyInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id);

        CompanyInsuredGuaranteeFixedTermDeposit CreateCompanyInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit companyInsuredGuaranteeMortgage);

        CompanyInsuredGuaranteeFixedTermDeposit UpdateCompanyInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit companyInsuredGuaranteeMortgage);


        /// <summary>
        /// Obtener contragarantias asegurado tipo Others
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyInsuredGuaranteeOthers GetCompanyInsuredGuaranteeOthersByIndividualIdById(int individualId, int id);

        [OperationContract]
        CompanyInsuredGuaranteeOthers CreateCompanyInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers insuredGuaranteeOthers);

        [OperationContract]
        CompanyInsuredGuaranteeOthers UpdateCompanyInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers insuredGuaranteeOthers);


        #endregion

        #region InsuredGuaranteeLog

        /// <summary>
        /// Consulta ramos asociados a una bitacora
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        [OperationContract]
        List<CompanyInsuredGuaranteeLog> GetCompanyInsuredGuaranteeLogsByindividualIdByguaranteeId(int individualId, int guaranteeId);

        [OperationContract]
        CompanyInsuredGuaranteeLog CreateCompanyInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog);

        [OperationContract]
        CompanyInsuredGuaranteeLog UpdateCompanyInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog);

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        [OperationContract]
        List<CompanyGuarantor> GetCompanyGuarantorByindividualIdByguaranteeId(int individualId, int guaranteeId);

        [OperationContract]
        CompanyGuarantor CreateCompanyGuarantor(CompanyGuarantor insuredGuaranteeLog);

        [OperationContract]
        CompanyGuarantor UpdateCompanyGuarantor(CompanyGuarantor insuredGuaranteeLog);

        [OperationContract]
        void DeleteCompanyGuarantor(CompanyGuarantor insuredGuaranteeLog);

        #region MaritalStatus

        [OperationContract]
        List<CompanyMaritalStatus> GetCompanyMaritalStatus();

        #endregion

        #region DocumentType

        [OperationContract]
        List<CompanyDocumentType> GetCompanyDocumentType(int typeDocument);

        #endregion

        #region AddressesType

        [OperationContract]
        List<CompanyAddressType> GetCompanyAddressesTypes();

        #endregion

        #region PhoneType

        [OperationContract]
        List<CompanyPhoneType> GetCompanyPhoneTypes();

        #endregion

        #region EmailType

        [OperationContract]
        List<CompanyEmailType> GetCompanyEmailTypes();

        #endregion

        #region EconomicActivity

        [OperationContract]
        List<CompanyEconomicActivity> GetCompanyEconomicActivities();

        #endregion

        #region AssociationType

        [OperationContract]
        List<CompanyAssociationType> GetCompanyAssociationTypes();

        #endregion

        #region CompanyType

        [OperationContract]
        List<CompanyCompanyType> GetCompanyCompanyType();

        #endregion

        #region FiscalResponsibility

        [OperationContract]
        List<CompanyFiscalResponsibility> GetCompanyFiscalResponsibility();

        [OperationContract]
        bool DeleteCompanyFiscalResponsibility(CompanyInsuredFiscalResponsibility companyFiscal);

        #endregion



        #region Consortium
        [OperationContract]
        List<Models.CompanyConsortium> GetCoConsortiumsByInsuredCod(int insuredCode);
        [OperationContract]
        Models.CompanyConsortium CreateCompanyConsortium(Models.CompanyConsortium companyConsortium);
        [OperationContract]
        Models.CompanyConsortium UpdateCompanyConsortium(Models.CompanyConsortium companyConsortium);
        [OperationContract]
        bool DeleteCompanyConsortium(Models.CompanyConsortium companyConsortium);
        #endregion

        #region Insured

        [OperationContract]
        List<CompanyInsuredDeclinedType> GetCompanyInsuredDeclinedTypes();

        [OperationContract]
        List<Models.CompanyInsuredSegment> GetCompanyInsuredSegment();

        [OperationContract]
        List<Models.CompanyInsuredProfile> GetCompanyInsuredProfile();
        #endregion

        #region AgentType

        [OperationContract]
        List<CompanyAgentType> GetCompanyAgentTypes();

        #endregion

        #region AgentDeclinedType

        [OperationContract]
        List<CompanyAgentDeclinedType> GetCompanyAgentDeclinedTypes();

        #endregion

        #region GroupAgent

        [OperationContract]
        List<CompanyGroupAgent> GetCompanyGroupAgent();

        #endregion

        #region SalesChannel

        [OperationContract]
        List<CompanySalesChannel> GetCompanySalesChannel();

        #endregion

        #region EmployeePerson

        [OperationContract]
        List<CompanyEmployeePerson> GetCompanyEmployeePersons();

        #endregion

        #region AllOthersDeclinedType

        [OperationContract]
        List<Models.CompanyAllOthersDeclinedType> GetCompanyAllOthersDeclinedTypes();

        #endregion

        #region InsuredGuaranteeDocumentation

        [OperationContract]
        CompanyInsuredGuaranteePrefix CreateCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix insuredGuaranteePrefix);

        [OperationContract]
        CompanyInsuredGuaranteeDocumentation CreateCompanyInsuredGuaranteeDocumentation(CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation);

        [OperationContract]
        CompanyInsuredGuaranteeDocumentation UpdateCompanyInsuredGuaranteeDocumentation(CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation);

        [OperationContract]
        void DeleteCompanyInsuredGuaranteeDocumentation(int individualId, int insuredguaranteeId, int guaranteeId, int documentId);

        [OperationContract]
        CompanyInsuredGuaranteeDocumentation GetCompanyInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId);

        [OperationContract]
        List<CompanyInsuredGuaranteeDocumentation> GetCompanyInsuredGuaranteeDocumentation();

        [OperationContract]
        List<CompanyInsuredGuaranteeDocumentation> GetCompanyInsuredGuaranteeDocument(int individualId, int guaranteeId);

        #endregion

        #region GuaranteeRequiredDocument

        [OperationContract]
        List<CompanyGuaranteeRequiredDocument> GetCompanyInsuredGuaranteeRequiredDocumentation(int guaranteeId);

        #endregion

        [OperationContract]
        CompanyInsuredGuaranteePrefix UpdateCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix companyInsuredGuaranteePrefix);

        [OperationContract]
        void DeleteCompanyInsuredGuaranteePrefix(int individualId, int guaranteeId, int documentId);

        [OperationContract]
        List<CompanyInsuredGuaranteePrefix> GetCompanyInsuredGuaranteePrefix(int individualId, int guaranteeId);

        #region Cumplimiento
        [OperationContract]
        bool IsConsortiumindividualId(int individualId);
        [OperationContract]
        bool IsConsortiumindividualIdR1(int individualId);
        [OperationContract]
        decimal GetAvailableCumulus(int individualId, int currencyCode, int prefixCode, System.DateTime issueDate);
        #endregion

        /// <summary>
        /// Obtiene todos los tipos de teléfono
        /// </summary>
        /// <returns>Tipos de teléfono</returns>
        [OperationContract]
        List<Models.CompanyPhoneType> GetAllCompanyPhoneType();

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de teléfono
        /// </summary>
        /// <param name="listAdded">tipos de teléfono para agregar</param>
        /// <param name="listEdited">tipos de teléfono para editar</param>
        /// <param name="listDeleted">tipos de teléfono para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        [OperationContract]
        ParametrizationResponse<Models.CompanyPhoneType> CreateCompanyPhoneTypes(List<Models.CompanyPhoneType> listAdded, List<Models.CompanyPhoneType> listEdited, List<Models.CompanyPhoneType> listDeleted);


        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de dirección
        /// </summary>
        /// <param name="listAdded">tipos de dirección para agregar</param>
        /// <param name="listEdited">tipos de dirección para editar</param>
        /// <param name="listDeleted">tipos de direción para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        [OperationContract]
        ParametrizationResponse<Models.CompanyAddressType> CreateCompanyAddressTypes(List<Models.CompanyAddressType> listAdded, List<Models.CompanyAddressType> listEdited, List<Models.CompanyAddressType> listDeleted);

        /// <summary>
        /// Obtiene todos los tipos de dirección
        /// </summary>
        /// <returns>Tipos de dirección</returns>
        [OperationContract]
        List<Models.CompanyAddressType> GetAllCompanyAddressType();

        [OperationContract]
        List<Models.CompanyInsuredMain> GetCompanyInsuredsByName(string filterString);

        /// <summary>
        /// GetCiaDocumentTypeRangeId
        /// </summary>
        /// <param name="documentTypeRangeId"></param>
        /// <returns></returns>
        [OperationContract]
        CiaDocumentTypeRange GetCiaDocumentTypeRangeId(int documentTypeRangeId);

        /// <summary>
        /// Generar archivo excel de tipo de dirección
        /// </summary>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        string GenerateFileToAddressType();

        /// <summary>
        /// Generar archivo excel de tipo documento datacrédito
        /// </summary>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        string GenerateFileToPhoneType();

        #region ThirdPerson
        /// <summary>
        /// Consulta motivo dado de baja para tercero
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<CompanyThirdDeclinedType> GetAllThirdDeclinedTypes();

        /// <summary>
        /// Crear tercero
        /// </summary>
        /// <param name="companyThird"></param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyThird CreateCompanyThird(Models.CompanyThird companyThird);

        /// <summary>
        /// Actualizar tercero.
        /// </summary>
        /// <param name="companyThird"></param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyThird CreateUpdateThird(Models.CompanyThird companyThird);

        [OperationContract]
        Models.CompanyThird GetCompanyThirdByIndividualId(int individualId);
        #endregion

        /// <summary>
        /// CreateCiaDocumentTypeRange
        /// </summary>
        /// <param name="CiaDocumentTypeRange"></param>
        /// <returns></returns>
        [OperationContract]
        CiaDocumentTypeRange CreateCiaDocumentTypeRange(CiaDocumentTypeRange CiaDocumentTypeRange);

        /// <summary>
        /// Guardar la inofrmacion sarlaft en la tabla UP.INDIVIDUAL_SARLAFT
        /// </summary>
        /// <param name="individualSarlaft"></param>
        /// <returns></returns>
        [OperationContract]
        Models.IndividualSarlaft CreateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft, int IndividualId, int economiActivity);
        /// <summary>
        /// UpdateCiaDocumentTypeRange
        /// </summary>
        /// <param name="CiaDocumentTypeRange"></param>
        /// <returns></returns>
        [OperationContract]
        CiaDocumentTypeRange UpdateCiaDocumentTypeRange(CiaDocumentTypeRange ciaDocumentTypeRange);

        /// <summary>
        /// DocumentTypeRange
        /// </summary>
        /// <param name="DocumentTypeRange"></param>
        /// <returns></returns>
        [OperationContract]
        Models.DocumentTypeRange UpdateDocumentTypeRange(Models.DocumentTypeRange DocumentTypeRange);

        /// <summary>
        /// CompanyEmployee
        /// </summary>
        /// <param name="companyEmploye"></param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyEmployee CreateCompanyEmployee(Models.CompanyEmployee companyEmploye);

        /// <summary>
        /// enumRoles
        /// </summary>
        /// <param name="enumRoles"></param>
        /// <returns></returns>
        [OperationContract]
        EnumRoles GetEnumsRoles();

        /// <summary>
        /// individualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyEmployee GetCompanyEmployee(int individualId);

        [OperationContract]
        List<Models.CompanyCoClintonList> GetListClintonByDocumentNumberFullName(string documentNumber, string fullName);

        [OperationContract]
        List<Models.CompanyCoOnuList> GetListOnuByDocumentNumberFullName(string documentNumber, string fullName);

        [OperationContract]
        List<Models.CompanyCoOwnList> GetListOwnByDocumentNumberFullName(string documentNumber, string fullName);

        #region Politicas   
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesPerson(CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesInsured(CompanyInsured insured, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesProvider(CompanySupplier companySupplier, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesThirdParty(CompanyThird companyThird, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesPersonalInformation(CompanyLabourPerson labourPerson, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesTaxes(CompanyIndividualTax individualTax, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        #endregion

        /// <summary>
        /// individualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesAgent(CompanyAgent agent, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        /// <summary>
        /// individualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="companyEmployee"></param>
        /// <param name="companyPerson"></param>
        /// <param name="companyCompany"></param>
        /// <param name="companyAddress"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesEmployee(CompanyEmployee companyEmployee, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        /// <summary>
        /// individualId
        /// </summary>
        /// <param name="companyPaymentMethod"></param>
        /// <param name="companyPerson"></param>
        /// <param name="companyCompany"></param>
        /// <param name="companyAddress"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesPaymentMethods(CompanyIndividualPaymentMethod companyPaymentMethod, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        /// <summary>
        /// individualId
        /// </summary>
        /// <param name="companyPaymentMethod"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesOperatingQuota(CompanyOperatingQuota companyOperationQuota, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        /// <summary>
        /// individualId
        /// </summary>
        /// <param name="companyPaymentMethod"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesBankTransfers(models.BankTransfers bankTransfers, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        /// <summary>
        /// ValidateAuthorizationPoliciesReInsurer
        /// </summary>
        /// <param name="companyReInsurer"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesReInsurer(CompanyReInsurer companyReInsurer, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        /// <summary>
        /// ValidateAuthorizationPoliciesCoInsured
        /// </summary>
        /// <param name="companyReInsurer"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesCoInsured(UniquePersonServices.V1.Models.CompanyCoInsured companyCoInsured, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        Models.CompanyPersonOperation CreateCompanyPersonOperation(CompanyPersonOperation companyPersonOperation);

        [OperationContract]
        CompanyPersonOperation GetCompanyPersonOperation(int operationId);

        /// <summary>
        /// ValidateAuthorizationPoliciesBusinessName
        /// </summary>
        /// <param name="companyCoInsured"></param>
        /// <returns></returns>
        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesBusinessName(models.CompanyName companyCoInsured, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesConsortium(CompanyConsortium companyConsortium, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        List<AuthorizationRequest> GetAuthorizationRequestByIndividualId(int individualId);


        [OperationContract]
        List<PoliciesAut> ValidateAuthorizationPoliciesGuarantee(models.Guarantee guarantee, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress);

        [OperationContract]
        Models.CompanySarlaftExoneration UpdateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration, int individualId);

        [OperationContract]
        CompanyProspectNatural GetCompanyProspectLegalAdv(CustomerType customerType, CompanyProspectNatural company);

        [OperationContract]
        CompanyProspectNatural GetCompanyProspectNaturalAdv(CustomerType customerType, CompanyProspectNatural company);

        [OperationContract]
        CompanyOrPerson GetIdentificationPersonOrCompanyByIndividualId(int individualId);
        [OperationContract]
        List<AccountType> getCompanyAccountType();

        /// <summary>
        /// Recupera una agencia por el indice único código de agencia - tipo de agencia 
        /// </summary>
        /// <param name="agentCode"></param>
        /// <param name="agentTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        CompanyAgency GetCompanyAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode);

        [OperationContract]
        void CreateIntegrationNotification(int IndividualId, int PerifericoId);
    }
}
