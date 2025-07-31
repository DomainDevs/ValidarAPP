using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.UniquePersonService.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using ModelIndividual = Sistran.Core.Application.UniquePersonServiceIndividual.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using PersonBase = Sistran.Core.Application.UniquePersonService.Models.Base;

namespace Sistran.Core.Application.UniquePersonService
{
    /// <summary>
    /// Personas
    /// </summary>
    [ServiceContract]
    public interface IUniquePersonServiceCore
    {
        /// <summary>
        /// Obtener tipo de direcciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<AddressType> GetAddressesTypes();
        /// <summary>
        /// Obtener Direcciones por tipo de individuo
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Address> GetAddressesByIndividualId(int individualId);

        /// <summary>
        /// Obtener lista de tipos de teléfono
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PhoneType> GetPhoneTypes();

        /// <summary>
        /// Obtener lista de teléfonos asociados a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        [OperationContract]
        List<Phone> GetPhonesByIndividualId(int individualId);

        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        [OperationContract]
        List<DocumentType> GetDocumentTypes(int typeDocument);

        /// <summary>
        /// Gets the partner by document identifier document type individual identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="IndividualISD">The individual isd.</param>
        /// <returns></returns>
        [OperationContract]
        Partner GetPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualISD);
        /// <summary>
        ///Crear Accionista
        /// </summary>
        /// <param name="partner">Accionista</param>
        /// <returns></returns>
        [OperationContract]
        Partner CreatePartner(Partner partner, int individualId);
        /// <summary>
        /// Gets the part ner by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Partner> GetPartnerByIndividualId(int individualId);
        /// <summary>
        /// Obtener lista de tipos de email
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EmailType> GetEmailTypes();
        /// <summary>
        /// Guardar la informacion de una representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentative CreateLegalRepresent(LegalRepresentative legalRepresent, int individualId);
        /// <summary>
        /// Gets the emails by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Email> GetEmailsByIndividualId(int individualId);
        /// <summary>
        /// Lista de niveles educativos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EducativeLevel> GetEducativeLevels();
        /// <summary>
        /// lista de estratos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SocialLayer> GetSocialLayers();
        /// <summary>
        /// Obetener lista de los cargos que pueden tener una persona
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Occupation> GetOccupations();
        /// <summary>
        /// Obtener lista de especialidades
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Speciality> GetSpecialties();
        /// <summary>
        /// Obtener lista de nivel de ingresos
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<IncomeLevel> GetIncomeLevels();
        /// <summary>
        /// Buscar la infromacion de un representante legal
        /// </summary>
        /// <param name="idCardNo"></param>
        /// <param name="idCardTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentative GetLegalRepresentByIndividualId(int individualId);
        /// <summary>
        /// Actualizar la informacion de un representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentative UpdateLegalRepresent(LegalRepresentative legalRepresent, int individualId);
        /// <summary>
        /// Guardar la informaicon laboral de una persona
        /// </summary>
        /// <param name="personJob">Modelo personJob</param>
        /// <returns></returns>
        [OperationContract]
        LaborPerson CreatePersonJob(LaborPerson personJob, int individualId);
        /// <summary>
        /// Actualizar los los datos de un persona
        /// </summary>
        /// <param name="person"> Modelo person</param>
        /// <returns></returns>
        [OperationContract(Name = "UpdatePersonCore")]
        Person UpdatePerson(Person person, Models.PersonIndividualType personIndividualType);
        /// <summary>
        /// actualizar la informacion laborar de la persona
        /// </summary>
        /// <param name="personJob">Modelo personJob</param>
        /// <returns></returns>
        [OperationContract]
        LaborPerson UpdatePersonJob(LaborPerson personJob);

        /// <summary>
        /// Buscar la informacion la boral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        [OperationContract]
        LaborPerson GetPersonJobByIndividualId(int individualId);
        /// <summary>
        /// Buscar compañias y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPersonByDocumentNumberSurnameMotherLastNameCore")]
        List<Models.Person> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType, int? documentType, int? individualId);
        /// <summary>
        /// lista d etipos de cuenta
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.PaymentAccountType> GetPaymentTypes();
        /// <summary>
        /// Gets the payment method account by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.PaymentMethodAccount> GetPaymentMethodAccountByIndividualId(int individualId);
        /// <summary>
        /// Listaod de los Motivos de baja para los agentes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.AgentDeclinedType> GetAgentDeclinedTypes();

        /// <summary>
        /// Obtener Agencias
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<Agency> GetAgenciesByAgentIdDescription(int agentId, string description);

        /// <summary>
        /// Obtener Agencias habilitadas por ramo
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <param name="prefixId">Código o Nombre</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<Agency> GetAgenciesByAgentIdDescriptionIdPrefix(int agentId, string description, int prefixId);


        /// <summary>
        /// Obtener lista de agencias por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        List<Agency> GetAgenciesByAgentId(int agentId);
        /// <summary>
        /// Buscar compañias y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        [OperationContract(Name = "GetCompaniesByDocumentNumberNameSearchTypeCore")]
        List<Models.Company> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType, int? documentType, int? individualId);
        /// <summary>
        /// Creacion del Agente
        /// </summary>
        /// <param name="Agent">The agent.</param>
        /// <returns></returns>
        [OperationContract]
        Models.Agent CreateAgent(Agent Agent);

        [OperationContract]
        Models.Insured CreateInsured(Models.Insured insured);

        [OperationContract]
        Models.Insured GetInsuredByIndividualId(int individualId);
        /// <summary>
        [OperationContract]
        Models.Insured GetInsuredByIndividualCode(int individualCode);
        /// <summary>
        /// Obtiene los Estados Civil
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.MaritalStatus> GetMaritalStatus();
        /// <summary>
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <returns></returns>
        [OperationContract(Name = "CreateCompanyCore")]
        Models.Company CreateCompany(Models.Company company);

        /// <summary>
        /// Obtener lista de tipos de asociación
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.AssociationType> GetAssociationTypes();

        [OperationContract]
        List<Models.AgentPrefix> GetAgentPrefixByIndividualId(int IndividualId);
        /// <summary>
        /// Obtener el listado de los motivos de baja del asegurado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.InsuredDeclinedType> GetInsuredDeclinedTypes();
        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns></returns>
        [OperationContract(Name = "GetCompanyByIndividualIdCore")]
        Company GetCompanyByIndividualId(int individualId);

        ///// <summary>
        ///// Obtener compañia por numero de documento
        ///// </summary>
        ///// <param name="documentNumber">numero de documento</param>
        ///// <returns></returns>
        //[OperationContract]
        //Company GetCompanyByDocumentNumber(string documentNumber);

        ///// <summary>
        ///// Obtener persona por numero de documento
        ///// </summary>
        ///// <param name="documentNumber">numero de documento</param>
        ///// <returns></returns>
        //[OperationContract]
        //Person GetPersonByDocumentNumber(string documentNumber);

        /// <summary>
        ///  Actualizar Compañia
        /// </summary>
        /// <param name="company">Modelo Compañia</param>
        /// <returns></returns>
        [OperationContract(Name = "UpdateCompanyCore")]
        Models.Company UpdateCompany(Models.Company company);

        /// <summary>
        /// Crear los medios de  pago de la persona o de compañia
        /// </summary>
        /// <param name="paymentMethodAccounts"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>

        [OperationContract]
        List<Models.PaymentMethodAccount> CreatePaymentMethodAccounts(List<Models.PaymentMethodAccount> paymentMethodAccounts, int IndividualId);

        /// <summary>
        /// Crear los mediod de  pago de la persona o de compañia
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.PaymentMethodAccount CreatePaymentMethodAccount(Models.PaymentMethodAccount paymentMethodAccount, int IndividualId);
        /// <summary>
        /// Crear Una nueva persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract(Name = "CreatePersonCore")]
        Models.Person CreatePerson(Models.Person person, Models.PersonIndividualType personIndividualType);
        /// <summary>
        /// Crear un prospect natural
        /// </summary>
        /// <param name="prospectNatural">Modelo prospecto natural</param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural CreateProspectNatural(Models.ProspectNatural prospectNatural);
        /// <summary>
        /// Crear un prospect legal
        /// </summary>
        /// <param name="prospectLegal">Modelo prospecto legal</param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural CreateProspectLegal(Models.ProspectNatural prospectLegal);
        /// <summary>
        /// Actualizar los datos de un prospecto natural
        /// </summary>
        /// <param name="prospectNatural"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural UpdateProspectNatural(Models.ProspectNatural prospectNatural);
        /// <summary>
        /// Actualizar los datos de un prospecto juridico
        /// </summary>
        /// <param name="prospectLegal">Modelo de prospecto juridico</param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural UpdateProspectLegal(Models.ProspectNatural prospectLegal);
        /// <summary>
        /// Obtener lista de ramos comerciales por agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        List<BasePrefix> GetPrefixesByAgentId(int agentId);

        [OperationContract]
        Agent GetAgentByIndividualId(int individualId);

        /// <summary>
        /// Obtener Tipos de Agente
        /// </summary>
        /// <param name="prefixId"></param>
        /// <returns></returns>
        [OperationContract]
        List<AgentType> GetAgentTypes();

        /// <summary>
        /// Ontener listado tipos de personas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.PersonType> GetPersonTypes();

        [OperationContract]
        List<Models.Agent> GetAgents();

        [OperationContract]
        List<Models.Agent> GetAgentByQuery(string query);

        [OperationContract]
        List<Models.Agency> GetAgencyByIndividualId(int individualId);

        /// <summary>
        /// Gets the name of the agent by.
        /// </summary>
        /// <param name="nameAgent">The name agent.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Agent> GetAgentByName(string nameAgent);

        /// <summary>
        /// Gets the prospect natural by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        ProspectNatural GetProspectNaturalByIndividualId(int individualId);

        /// <summary>
        /// Gets the prospect legal by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        ProspectNatural GetProspectLegalByIndividualId(int individualId);

        /// <summary>
        /// Gets the co company name by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.CompanyName> GetCompanyNamesByIndividualId(int individualId);

        /// <summary>
        /// Obtener el Agente por codigo o nombre
        /// </summary>
        /// <param name="agentCode">Codigo Agente</param>
        /// <param name="fullName">Nombre Agente</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Agent> GetAgentByAgentCodeFullName(int agentCode, string fullName);

        /// <summary>
        /// Obtener Asegurados por Nombre
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Insured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);

        /// <summary>
        /// Obtiene el Cupo Operativo y el Cumulo
        ///     <para>&#160;</para>
        ///     <para>
        ///         Valores Salida
        ///     </para>
        ///     <para>&#160;</para>
        ///     <para>Decimal  Cupo Operativo</para>       
        ///     <para>Decimal Cumulo</para>
        /// </summary>
        /// para
        ///<code>
        ///AggregateDAO aggregateDAO = new AggregateDAO();
        ///</code>      
        /// <param name="individualId">Individual Id Asegurado</param>
        /// <param name="currencyCode">Moneda</param>
        /// <param name="lineBusinessCode">Linea del Negocio</param>
        /// <param name="issueDate">Fecha Hasta</param>       
        /// <returns>Lista de sumas</returns>       
        [OperationContract]
        List<Amount> GetAvailableAmountByIndividualId(int individualId, int lineBusinessCode, DateTime issueDate);

        /// <summary>
        /// Obtener Todas las personas
        /// </summary>
        /// <param name="documentNumber"></param>
        ///<param name="surname"></param>
        /// <param name="motherLastName"></param>
        /// <param name="name"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.ProspectNatural> GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(string documentNumber, string surname, string motherLastName, string name, string tradeName, int searchType, int? documentType, int? individualId);

        /// <summary>
        /// Obtener agencia por Identificadores
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>Agencia</returns>
        [OperationContract]
        Agency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId);

        /// <summary>
        /// Obtener la documentacion recivida
        /// </summary>
        /// <param name="IndividualId">IndividualId asegurado</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.GuaranteeRequiredDocument> GetDocumentationReceivedByGuaranteeId(int guaranteeId);
        /// <summary>


        /// <summary>
        /// Obtener contragarantias asegurado
        /// </summary>
        /// <param name="IndividualId">IndividualId asegurado</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Guarantee> GetInsuredGuaranteeByIndividualId(int IndividualId);

        /// <summary>
        /// Obtiene listado de estados de contragarantias
        /// </summary>
        /// <returns> Listado de estados de contragarantías </returns>
        [OperationContract]
        List<Models.GuaranteeStatus> GetGuaranteeStatus();

        /// <summary>
        /// Obtiene los tipos de pagaré
        /// </summary>
        /// <returns> Listado de tipos de pagaré </returns>
        [OperationContract]
        List<Models.PromissoryNoteType> GetPromissoryNoteType();

        /// <summary>
        /// Obtiene las unidades de medida
        /// </summary>
        /// <returns> Listado de unidad de medida </returns>
        [OperationContract]
        List<Models.MeasurementType> GetMeasurementType();


        /// <summary>
        /// Consulta una contragarantía dado su id
        /// </summary>
        /// <param name="guaranteId"> Id de la contragarantía</param>
        /// <returns> Contragarantía según id </returns>
        [OperationContract]
        Models.Guarantee GetInsuredGuaranteeByIdGuarantee(int guaranteId);

        /// <summary>
        /// Consulta contragarantias de un afianzado
        /// </summary>
        /// <param name="id"> Id del afianzado</param>
        /// <returns> Contragarantías </returns>
        [OperationContract]
        List<Models.Guarantee> GetInsuredGuaranteesByIndividualId(int id);

        /// <summary>
        /// Guarda contragarantias de un afianzado
        /// </summary>
        /// <param name="listGuarantee"> Lista Contragarantías</param>
        /// <returns> Contragarantías </returns>
        [OperationContract]
        List<Models.Guarantee> SaveInsuredGuarantees(List<Models.Guarantee> listGuarantee);

        /// <summary>
        /// Consulta documentación asociada a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Documentación asociada a una contragarantía </returns>
        [OperationContract]
        List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation(int individualId, int guaranteeId);

        /// <summary>
        /// Consulta ramos asociados a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        [OperationContract]
        List<Models.InsuredGuaranteePrefix> GetInsuredGuaranteePrefix(int individualId, int guaranteeId);

        /// <summary>
        /// Guarda contragarantia de un afianzado
        /// </summary>
        /// <param name="guarantee">Contragarantías</param>
        /// <returns> Contragarantía </returns>
        [OperationContract]
        Models.Guarantee SaveInsuredGuarantee(Models.Guarantee guarantee);

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        [OperationContract]
        List<InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId);

        /// <summary>
        /// Buscar prospectos
        /// </summary>
        /// <param name="individualType">Tipo de individuo</param>
        /// <param name="documentTypeId">Id tipo de documento</param>
        /// <param name="document">Documento</param>
        /// <returns>Prospecto</returns>
        [OperationContract]
        Prospect GetProspectByIndividualTypeDocumentTypeIdDocument(Enums.IndividualType individualType, int documentTypeId, string document);

        /// <summary>
        /// Crear prospecto
        /// </summary>
        /// <param name="prospect">Datos prospecto</param>
        /// <returns>Prospecto</returns>
        [OperationContract]
        Prospect CreateProspect(Prospect prospect);

        /// <summary>
        /// Actualizar prospecto
        /// </summary>
        /// <param name="prospect">Datos prospecto</param>
        /// <returns>Prospecto</returns>
        [OperationContract]
        Prospect UpdateProspect(Prospect prospect);



        /// <summary>
        /// Crea el cupo operativo del tomador 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.OperatingQuota> CreateOperatingQuota(List<Models.OperatingQuota> listOperatingQuote);

        /// <summary>
        /// Obtiene el cupo operativo del tomador 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.OperatingQuota> GetOperatingQuotaByIndividualId(int individualId);

        /// <summary>
        /// Actualiza datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        OperatingQuota UpdateOperatingQuota(OperatingQuota OperatingQuota);

        /// <summary>
        /// Elimina datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool DeleteOperatingQuota(Models.OperatingQuota OperatingQuota);
        /// <summary>
        /// Gets the prospect natural by document num.
        /// </summary>
        /// <param name="documentNum">document num</param>
        /// <returns></returns>
        [OperationContract]
        List<ProspectNatural> GetProspectByDocumentNum(string documentNum, int searchType);

        /// <summary>
        /// edita la informacion personal
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Person UpdatePersonalInformation(Models.Person person);

        /// <summary>
        /// obtener el agente por individual id o Nombre
        /// </summary>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Agent> GetAgentByIndividualIdFullName(int IndividualId, string fullName);

        /// <summary>
        /// Obtener Prospectos Por Id, Número De Documento O Nombre
        /// </summary>
        /// <param name="description">Id, Número De Documento O Nombre</param>
        /// <param name="insuredSearchType">Tipo De Busqueda</param>
        /// <returns>Prospectos</returns>
        [OperationContract]
        List<Models.Prospect> GetProspectByDescription(string description, InsuredSearchType insuredSearchType);

        [OperationContract]
        List<Models.Prospect> GetAdvancedProspectByDescription(string description);
        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Direcciones de Notificación</returns>
        [OperationContract]
        List<Models.CompanyName> GetNotificationAddressesByIndividualId(int individualId, CustomerType customerType);

        /// <summary>
        /// Obtener tipo de contragarantias
        /// </summary>
        [OperationContract]
        List<Models.GuaranteeType> GetGuaranteesTypes();

        /// <summary>
        /// Obtener lista de contragarantias
        /// </summary>
        [OperationContract]
        List<Models.Guarantee> GetGuarantees();

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>        
        [OperationContract]
        List<Models.ProviderType> GetProviderTypes();

        /// <summary>
        /// Obtener lista de proveedores declinados (motivo de baja de un proveedor)
        /// </summary>        
        [OperationContract]
        List<Models.ProviderDeclinedType> GetProviderDeclinedType();

        /// <summary>
        /// Obtener lista de tipo de origen
        /// </summary>        
        [OperationContract]
        List<Models.OriginType> GetOriginTypes();

        /// <summary>
        /// Crear proveedor
        /// </summary>        
        [OperationContract]
        Models.Provider CreateProvider(Models.Provider provider);

        /// <summary>
        /// Actualizar Proveedor
        /// </summary>        
        [OperationContract]
        Models.Provider UpdateProvider(Models.Provider provider);

        /// <summary>
        /// Crear impuestos asociados al individuo
        /// </summary>
        /// <param name="individualTax"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.IndividualTax> CreateIndivualTaxs(List<Models.IndividualTax> individualTax);

        /// <summary>
        /// Get Impuestos por individualId de persona
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualTax> GetIndivualTaxsByIndividualId(int individualId);

        /// <summary>
        /// Obtener Tipos de Empresa
        /// </summary>
        /// <returns>Tipos de Empresa</returns>
        [OperationContract]
        List<CompanyType> GetCompanyTypes();

        /// <summary>
        /// Obtener prospecto por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Prospecto</returns>
        [OperationContract]
        Models.Prospect GetProspectByProspectId(int prospectId);

        /// <summary>
        /// Creates the name of the companies.
        /// </summary>
        /// <param name="coCompanyName">Name of the co company.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        Models.CompanyName CreateCompaniesName(Models.CompanyName coCompanyName, int individualId);

        /// <summary>
        /// guarda relación de individuos
        /// </summary>
        /// <param name="List<Models.IndividualRelationApp>">list of IndividualRelationApp</param>
        /// <returns></returns>
        [OperationContract]
        void SaveIndividualRelationApp(List<Models.IndividualRelationApp> individualsRelationApp);

        /// <summary>
        /// elimina relación de individuos por userId
        /// </summary>
        /// <param name="parentIndividualId">parentIndividualId</param>
        /// <returns></returns>
        [OperationContract]
        void DeleteIndividualRelationAppByUserId(int parentIndividualId);

        /// <summary>
        /// Listado de Prospecto por IndividualId
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.ProspectNatural> GetProspectByIndividualId(string individualId);

        /// <summary>
        /// Busqueda de person por codigo
        /// </summary>
        /// <param name="insuredCode"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Insured GetInsuredByInsuredCode(int insuredCode);

        /// <summary>
        ///Obtener typo de agente por id
        /// </summary>
        /// <param name="id">Identificador de agente</param>
        /// <returns>AgentType</returns>
        [OperationContract]
        Models.AgentType GetAgentTypeById(int id);

        [OperationContract]
        Models.IndividualPaymentMethod GetPaymentMethodByIndividualId(int individualId);

        /// <summary>
        /// Recupera una agencia por el indice único código de agencia - tipo de agencia 
        /// </summary>
        /// <param name="agentCode"></param>
        /// <param name="agentTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Agency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode);

        [OperationContract]
        ReInsurer GetReInsurerByIndividualId(int individualId);

        [OperationContract]
        ReInsurer CreateReinsurer(ReInsurer reinsurer);

        [OperationContract]
        IdentificationDocument GetIdentificationDocumentByInsuredId(int insuredId);

        /// <summary>
        /// Normalizar Dirección
        /// </summary>
        /// <param name="address">Dirección</param>
        /// <returns>Dirección</returns>
        [OperationContract]
        string NormalizeAddress(string address);

        /// <summary>
        /// /// Devuelve listado de tipos de Personas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<ModelIndividual.IndividualType> GetIndividualTypes();

        /// <summary>
        /// GetIndividualTypeById
        /// </summary>
        /// <param name="individualTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        ModelIndividual.IndividualType GetIndividualTypeById(int individualTypeId);

        [OperationContract]
        List<Models.ExonerationType> GetExonerationTypes();

        /// <summary>
        /// Obtener la lista de compañias Coaseguradoras
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.CoInsuranceCompany> GetCoInsuranceCompanies();

        /// <summary>
        /// Obtener Coaseguradora
        /// </summary>
        /// <param name="userId">identificador de Coaseguradora</param>
        /// <returns></returns>
        [OperationContract]
        Models.CoInsuranceCompany GetCoInsuranceCompanyByCoinsuranceId(int coInsuranceId);




        /// Obtiene la lista de Perfiles de Asegurado
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        [OperationContract]
        List<InsuredProfile> GetInsuredProfiles();

        /// <summary>
        /// Adiciona y Guarda los cambios del Perfil de Asegurado.
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        [OperationContract]
        ParametrizationResponse<InsuredProfile> CreateInsuredProfiles(List<InsuredProfile> ListAdded, List<InsuredProfile> ListEdited, List<InsuredProfile> ListDeleted);

        /// <summary>
        /// Obtiene la lista de Segmentos de Asegurado.
        /// </summary>
        /// <returns>Lista de Segmentos de Asegurado consultados</returns>
        [OperationContract]
        List<InsuredSegment> GetInsuredSegments();

        /// <summary>
        /// Adiciona y Guarda los cambios del Segmento de Asegurado.
        /// </summary>
        /// <returns>Lista de Segmentos de Asegurado consultados</returns>
        [OperationContract]
        ParametrizationResponse<InsuredSegment> CreateInsuredSegments(List<InsuredSegment> ListAdded, List<InsuredSegment> ListEdited, List<InsuredSegment> ListDeleted);

        /// <summary>
        /// Genera archivo excel de Perfiles de Asegurado.
        /// </summary>
        /// <param name="insuredProfile"></param>
        /// <param name="fileName"></param>
        /// <returns>Path archivo de excel</returns>
        [OperationContract]
        string GenerateFileToInsuredProfile(List<InsuredProfile> insuredProfile, string fileName);

        /// <summary>
        /// Genera archivo excel de Segmentos de Asegurado.
        /// </summary>
        /// <param name="insuredSegment"></param>
        /// <param name="fileName"></param>
        /// <returns>Path archivo de excel</returns>
        [OperationContract]
        string GenerateFileToInsuredSegment(List<InsuredSegment> insuredSegment, string fileName);

        /// <summary>
        #region Company.AddressType

        /// <summary>
        /// Obtiene todos los tipos de dirección
        /// </summary>
        /// <returns>Tipos de dirección</returns>
        [OperationContract]
        List<Models.CompanyAddressType> GetAllCompanyAddressType();

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
        /// Verifica si el tipo de dirección esta relacionado en otras tablas
        /// </summary>
        /// <param name="addressTypeCode">key del tipo de dirección</param>
        /// <returns>Resultado de la validación</returns>
        [OperationContract]
        bool ValidateForeingCompanyAddressType(int addressTypeCode);

        /// <summary>
        /// Generar archivo excel de tipo de dirección
        /// </summary>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        string GenerateFileToAddressType();
        #endregion

        #region Company.PhoneType

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
        /// Verifica si el tipo de teléfono esta relacionado en otras tablas
        /// </summary>
        /// <param name="phoneTypeCode">key del tipo de teléfono</param>
        /// <returns>Resultado de la validación</returns>
        [OperationContract]
        bool ValidateForeingCompanyPhoneType(int phoneTypeCode);

        /// <summary>
        /// Generar archivo excel de tipo documento datacrédito
        /// </summary>
        /// <returns>Archivo de excel</returns>
        [OperationContract]
        string GenerateFileToPhoneType();
        #endregion
        /// Obtener Comisiones por agente
        /// </summary>
        /// <param name="individualId">identificador del individuo</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.CommissionAgent> GetAgentCommissionByIndividualId(int individualId);

        /// <summary>
        /// Obtener lista de COmisiones por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        List<CommissionAgent> GetAgentCommissionByAgentId(int agentId);

        /// <summary>
        /// Elimina datos de la Comision del agente
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool DeleteAgentCommission(CommissionAgent commissionAgent);

        /// <summary>
        /// Listado para seleccionar el grupo al cual pertenece el intermediario.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Base.BaseGroupAgent> GetGroupAgent();


        /// <summary>
        /// Listado para seleccionar el canal al cual pertenece el intermediario.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.Base.BaseSalesChannel> GetSalesChannel();

        #region "Insureds"

        [OperationContract]
        List<Models.Base.BaseInsuredSegment> GetInsuredSegment();

        [OperationContract]
        List<Models.Base.BaseInsuredProfile> GetInsuredProfile();

        [OperationContract]
        List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter);

        #endregion
        [OperationContract]
        List<Models.Base.BaseEmployeePerson> GetEmployeePersons();

        #region "PersonInterestGroup"

        [OperationContract]
        List<Models.InterestGroupsType> GetInterestGroupTypes();

        [OperationContract]
        List<Models.PersonInterestGroup> GetPersonInterestGroups(int individualId);

        [OperationContract]
        Models.PersonInterestGroup CreatePersonInterestGroup(Models.PersonInterestGroup personInterestGroup);

        [OperationContract]
        Models.PersonInterestGroup DeletePersonInterestGroup(Models.PersonInterestGroup personInterestGroup);

        #endregion

        #region "TaxExcemption"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualTax"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.IndividualTaxExeption> CreateIndivualExemptionTaxs(List<Models.IndividualTaxExeption> individualTax);

        #endregion
        #region datos Poliza
        /// <summary>
        /// Gets the agents by individual ids.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        Task<List<Models.Agent>> GetAgentsByIndividualIds(List<int> individualId);

        /// <summary>
        /// Gets the agents by individual ids by agency identifier.
        /// </summary>
        /// <param name="IndividualIds">The individual ids.</param>
        /// <returns></returns>
        Task<List<Models.Agent>> GetAgentsByIndividualIdsByAgencyId(List<PersonBase.BaseAgentAgency> IndividualIds, Int16 prefixId);
        #endregion datos Poliza


        /// <summary>
        /// Obtiene los agentes por ramo
        /// </summary>
        /// <param name="prefixId">id ramo</param>
        /// <returns>Lista de agentes por ramo</returns>
        [OperationContract]
        List<Models.Agent> GetAgentsByPrefix(int prefixId);

        /// <summary>
        /// Consulta listados de tipo de bien
        /// </summary>
        /// <returns> Listado de tipos de bien </returns>
        [OperationContract]
        List<AssetType> GetAssetType();

        /// <summary>
        /// Obtener lista de actividades economicas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EconomicActivity> GetEconomicActivities();

        #region "UpdateAddressEmail"

        [OperationContract]
        bool UpdateAddressEmail(Models.Email email, int individualId, string user);

        #endregion
        #region Holder
        /// <summary>
        /// Gets the type of the i holders by description insured search type customer.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="insuredSearchType">Type of the insured search.</param>
        /// <param name="customerType">Type of the customer.</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Insured> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType);
        #endregion


        #region DocumentTypeRange
        /// <summary>
        /// DocumentTypeRange
        /// </summary>
        /// <param name="DocumentTypeRange"></param>
        /// <returns></returns>
        [OperationContract]
        Models.DocumentTypeRange CreateDocumentTypeRange(Models.DocumentTypeRange DocumentTypeRange);

        /// <summary>
        /// DocumentTypeRange
        /// </summary>
        /// <param name="DocumentTypeRange"></param>
        /// <returns></returns>
        [OperationContract]
        Models.DocumentTypeRange UpdateDocumentTypeRange(Models.DocumentTypeRange DocumentTypeRange);

        /// <summary>
        /// DeleteDocumentTypeRange
        /// </summary>
        /// <param name="DocumentTypeRangeId"></param>
        [OperationContract]
        void DeleteDocumentTypeRange(int DocumentTypeRangeId);

        /// <summary>
        /// GetDocumentsTypeRangeId
        /// </summary>
        /// <param name="DocumentTypeRangeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.DocumentTypeRange> GetDocumentsTypeRangeId(int DocumentTypeRangeId);

        /// <summary>
        /// GetDocumentTypeRange
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.DocumentTypeRange> GetDocumentTypeRange();



        #endregion

    }
}