using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using ModelIndividual = Sistran.Core.Application.UniquePersonService.V1Individual.Models;
using PersonBase = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
namespace Sistran.Core.Application.UniquePersonService.V1
{
    /// <summary>
    /// Personas
    /// </summary>
    [ServiceContract]
    public interface IUniquePersonServiceCore
    {
        #region person
        /// <summary>
        /// Crear Una nueva persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        [OperationContract()]
        Models.Person CreatePerson(Models.Person person);

        [OperationContract]
        List<Address> CreateAddresses(int individualId, List<Address> addresses);

        [OperationContract]
        List<Address> UpdateAddresses(int individualId, List<Address> addresses);

        [OperationContract]
        List<Phone> CreatePhones(int individualId, List<Phone> addresses);

        [OperationContract]
        List<Phone> UpdatePhones(int individualId, List<Phone> addresses);

        [OperationContract]
        List<Email> CreateEmails(int individualId, List<Email> addresses);

        [OperationContract]
        List<Email> UpdateEmails(int individualId, List<Email> addresses);

        [OperationContract]
        Person GetPersonByIndividualId(int individualId);

        [OperationContract]
        List<Person> GetPersonByDocument(CustomerType customerType, string documentNumber);

        [OperationContract]
        List<Person> GetPersonAdv(CustomerType customerType, Person person);

        [OperationContract]
        Models.EconomicActivity GetEconomicActivitiesById(int id);

        /// <summary>
        /// Buscar la informacion la boral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        [OperationContract]
        LabourPerson GetPersonJobByIndividualId(int individualId);

        [OperationContract]
        Person UpdatePersonBasicInfo(Person individualId);
        #endregion person

        #region reinsurer
        /// <summary>
        /// Obtiene reasegurador por el id del individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        ReInsurer GetReInsurerByIndividualId(int individualId);

        /// <summary>
        /// Crear reasegurador
        /// </summary>
        /// <param name="reinsurer"></param>
        /// <returns></returns>
        [OperationContract]
        ReInsurer CreateReinsurer(ReInsurer reinsurer);

        /// <summary>
        /// Actualizar reasegurador
        /// </summary>
        /// <param name="reinsurer"></param>
        /// <returns></returns>
        [OperationContract]
        ReInsurer UpdateReinsurer(ReInsurer reinsurer);


        #endregion reinsurer

        #region Partner
        /// <summary>
        /// Gets the partner by document identifier document type individual identifier.
        /// </summary>
        /// <param name="documentId">The document identifier.</param>
        /// <param name="documentType">Type of the document.</param>
        /// <param name="IndividualId">The individual isd.</param>
        /// <returns></returns>
        [OperationContract]
        Partner GetPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualId);


        /// <summary>
        /// Gets the part ner by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Partner> GetPartnerByIndividualId(int individualId);

        /// <summary>
        ///Crear Accionista
        /// </summary>
        /// <param name="partner">Accionista</param>
        /// <returns></returns>
        [OperationContract]
        Partner CreatePartner(Partner partner);

        /// <summary>
        ///Actualizar Accionista
        /// </summary>
        /// <param name="partner">Accionista</param>
        /// <returns></returns>
        [OperationContract]
        Partner UpdatePartner(Partner partner);

        #endregion Partner
        #region Agent V1
        /// <summary>
        /// Obtener el agente por invidualId
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        Agent GetAgentByIndividualId(int individualId);
        /// <summary>
        /// Obtiene las agencias dabas de baja.
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="AgentDeclinedTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Agency> GetActiveAgencyByInvidualId(int individualId);
        /// <summary>
        /// Crea el Agente 
        /// </summary>
        /// <param name="agent">model del agente</param>
        /// <returns></returns>
        [OperationContract]
        Agent CreateAgent(Agent agent);
        /// <summary>
        /// Actualiza el Agente.
        /// </summary>
        /// <param name="agent">model del agente</param>
        /// <returns></returns>
        [OperationContract]
        Agent UpdateAgent(Agent agent);

        /// <summary>
        /// Obtener Agencias
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<Agency> GetAgenciesByAgentIdDescription(int agentId, string description);

        /// <summary>
        /// Obtener lista de agencias por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        List<Agency> GetAgenciesByAgentId(int agentId);


        /// <summary>
        /// Obtiene los agentes por ramo
        /// </summary>
        /// <param name="prefixId">id ramo</param>
        /// <returns>Lista de agentes por ramo</returns>
        [OperationContract]
        List<Models.Agent> GetAgentsByPrefix(int prefixId);
        #endregion Agent V1

        #region OperatingQuota V1

        /// <summary>
        /// Obtiene el cupo operativo del tomador 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuota> GetOperatingQuotaByIndividualId(int individualId);

        /// <summary>
        /// Crea el cupo operativo del tomador 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<OperatingQuota> CreateOperatingQuota(List<Models.OperatingQuota> listOperatingQuote);

        /// <summary>
        /// Actualiza datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        OperatingQuota UpdateOperatingQuota(Models.OperatingQuota OperatingQuota);

        /// <summary>
        /// Elimina datos del cupo operativo 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool DeleteOperatingQuota(Models.OperatingQuota OperatingQuota);

        #endregion OperatingQuota V1

        #region AgenciesInvidualId
        /// <summary>
        /// Obtiene Agencias Por InvidialID
        /// </summary>
        /// <param name="individualId">Cod Del la persona</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Agency> GetAgencyByInvidualId(int individualId);
        /// <summary>
        /// Crea la agencia por InvidualId.
        /// </summary>
        /// <param name="agencies"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Agency CreateAgencyByInvidualId(Models.Agency agencies, int individualId);
        /// <summary>
        /// Actualiza la agencia por InvidualId.
        /// </summary>
        /// <param name="agencies"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Agency UpdateAgencyByInvidualId(Models.Agency agencies, int individualId);


        [OperationContract]
        List<BasePrefix> GetPrefixesByAgentId(int agentId);
        [OperationContract]
        BasePrefix CreatePrefixesByAgentId(BasePrefix basePrefix, int IndivualId);
        [OperationContract]
        BasePrefix UpdatePrefixesByAgentId(BasePrefix basePrefix, int IndivualId);
        [OperationContract]
        BasePrefix DeletePrefixesByAgentId(BasePrefix basePrefix, int IndivualId);

        /// <summary>
        /// Obtener Agencias habilitadas por ramo
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <param name="prefixId">Código o Nombre</param>
        /// <returns>Agencias</returns>
        [OperationContract]
        List<Agency> GetAgenciesByAgentIdDescriptionIdPrefix(int agentId, string description, int prefixId);
        #endregion

        #region IndividualTax




        /// <summary>
        /// Creación de un impuesto individual
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTaxExeption</param>
        /// <returns></returns>
        [OperationContract]
        IndividualTax CreateIndividualTax(IndividualTax individualTax);

        /// <summary>
        /// Creación de un impuesto Exeption
        /// </summary>
        /// <param name="taxcode"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        IndividualTaxExeption CreateIndividualTaxExeption(IndividualTaxExeption individualTaxExeption);

        /// <summary>
        /// Modificar de un impuesto
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTaxExeption</param>
        /// <returns></returns>
        [OperationContract]
        IndividualTax UpdateIndividualTax(IndividualTax individualTaxExeption);

        /// <summary>
        /// Modificar de un impuesto individual
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTaxExeption</param>
        /// <returns></returns>
        [OperationContract]
        IndividualTaxExeption UpdateIndividualTaxExeption(IndividualTaxExeption individualTaxExeption);

        /// <summary>
        /// Eliminar de un impuesto
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTaxExeption</param>
        /// <returns></returns>
        [OperationContract]
        void DeleteIndividualTaxExeption(IndividualTaxExeption individualTaxExeption);

        /// <summary>
        /// Eliminar de un impuesto
        /// </summary>
        /// <param name="TaxExeption">Modelo = individualTax</param>
        /// <returns></returns>
        [OperationContract]
        void DeleteIndividualTax(IndividualTax individualTax);

        #endregion IndividualTax

        #region Comissionagent
        [OperationContract]
        List<Models.Commission> GetCommissionInvidualId(int agentId);
        [OperationContract]
        Models.Commission CreateCommissionInvidualId(Models.Commission commission, int InvidualId, int AgencyId);
        [OperationContract]
        Models.Commission UpdateCommissionInvidualId(Models.Commission commission, int InvidualId, int AgencyId);
        [OperationContract]
        bool DeleteCommissionInvidualId(Models.Commission commission);

        #endregion

        #region Supplier v1

        /// <summary>
        /// Obtener todos los SupplierAccountingConcept por Supplier
        /// </summary> 
        List<SupplierAccountingConcept> GetSupplierAccountingConceptsBySupplierId(int SupplierId);

        /// <summary>
        /// Obtener todos los AccountingConcept
        /// </summary> 
        List<AccountingConcept> GetAccountingConcepts();


        /// <summary>
        /// Obtener SupplierProfile
        /// </summary> 
        List<Models.SupplierProfile> GetSupplierProfiles();

        /// <summary>
        /// Obtener SupplierProfile
        /// </summary> 
        List<Models.SupplierProfile> GetSupplierTypeProfileById(int suppilierTypeId);

        /// <summary>
        /// Obtener  proveedor por id
        /// </summary> 
        Supplier GetSupplierById(int SupplierId);

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>        
        [OperationContract]
        List<Models.Supplier> GetSuppliers();

        /// <summary>
        /// Obtener lista de tipos proveedores
        /// </summary>        
        [OperationContract]
        List<Models.SupplierType> GetSupplierTypes();

        /// <summary>
        /// Obtener lista de proveedores declinados (motivo de baja de un proveedor)
        /// </summary>        
        [OperationContract]
        List<Models.SupplierDeclinedType> GetSupplierDeclinedTypes();

        /// <summary>
        /// Obtener grupo de proveedores 
        /// </summary>        
        [OperationContract]
        List<Models.GroupSupplier> GetGroupSupplier();

        /// <summary>
        /// Crear proveedor
        /// </summary>        
        [OperationContract]
        Models.Supplier CreateSupplier(Models.Supplier provider);

        /// <summary>
        /// Actualizar Proveedor
        /// </summary>        
        [OperationContract]
        Models.Supplier UpdateSupplier(Models.Supplier provider);

        [OperationContract]
        Supplier GetSupplierByIndividualId(int individualId);

        #endregion Supplier v1


        #region IndividualRole V1

        /// <summary>
        /// GetIndividualRoleByIndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        List<IndividualRole> GetIndividualRoleByIndividualId(int individualId);

        #endregion IndividualRole V1

        #region MaritalStatus V1
        /// <summary>
        /// Obtiene los Estados Civil
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.MaritalStatus> GetMaritalStatus();
        #endregion

        #region DocumentType V1
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
        /// GetDocumentTypeRange
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<DocumentTypeRange> GetDocumentTypeRange();
        #endregion

        #region AddressType V1

        /// <summary>
        /// Obtener tipo de direcciones
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<AddressType> GetAddressesTypes();

        #endregion AddressType V1

        #region PhoneType
        /// <summary>
        /// Obtener lista de tipos de teléfono
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<PhoneType> GetPhoneTypes();
        #endregion

        #region EconomicActivities
        /// <summary>
        /// Obtener lista de actividades economicas
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.EconomicActivity> GetEconomicActivities();
        #endregion

        #region AssociationType
        /// <summary>
        /// Obtener lista de tipos de asociación
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.AssociationType> GetAssociationTypes();
        #endregion AssociationType

        #region CompanyType
        /// <summary>
        /// Obtener Tipos de Empresa
        /// </summary>
        /// <returns>Tipos de Empresa</returns>
        [OperationContract]
        List<CompanyType> GetCompanyTypes();
        #endregion

        #region FiscalResponsibility
        /// <summary>
        /// Obtener Tipos de Responsabilidad Fiscal
        /// </summary>
        /// <returns>Tipos de Responsabilidad Fiscal</returns>
        [OperationContract]
        List<FiscalResponsibility> GetFiscalResponsibility();

        #endregion

        #region ThirdPerson

        [OperationContract]
        List<ThirdPerson> GetThirdByDescriptionInsuredSearchType(string description, InsuredSearchType insuredSearchType);

        #endregion

        #region InsuredDeclinedType
        /// <summary>
        /// Obtener el listado de los motivos de baja del asegurado
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.InsuredDeclinedType> GetInsuredDeclinedTypes();
        #endregion

        #region AgentType
        /// <summary>
        /// Obtener Tipos de Agente
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<AgentType> GetAgentTypes();
        #endregion

        #region AgentDeclinedType
        /// <summary>
        /// Listaod de los Motivos de baja para los agentes
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.AgentDeclinedType> GetAgentDeclinedTypes();
        #endregion

        #region GroupAgent
        /// <summary>
        /// Listado para seleccionar el grupo al cual pertenece el intermediario.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<GroupAgent> GetGroupAgent();

        #endregion

        #region SalesChannel
        /// <summary>
        /// Listado para seleccionar el canal al cual pertenece el intermediario.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<SalesChannel> GetSalesChannel();
        #endregion

        #region EmployeePerson
        /// <summary>
        /// Lista de Empleados
        /// </summary>
        [OperationContract]
        List<Models.EmployeePerson> GetEmployeePersons();
        #endregion

        #region OthersDeclinedType
        /// <summary>
        /// Lista de Empleados
        /// </summary>
        [OperationContract]
        List<Models.AllOthersDeclinedType> GetAllOthersDeclinedTypes();
        #endregion

        #region Company
        List<Company> GetCompanyByDocument(CustomerType customerType, string documentNumber);

        Company UpdateCompanyBasicInfo(Models.Company company);

        #endregion Company


        /// <summary>
        /// Obtener Direcciones por tipo de individuo
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        [OperationContract]
        List<Address> GetAddressesByIndividualId(int individualId);


        /// <summary>
        /// Obtener lista de teléfonos asociados a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        [OperationContract]
        List<Phone> GetPhonesByIndividualId(int individualId);

        /// <summary>
        /// Actualizar Razon social
        /// </summary>
        /// <param name="copyName"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        Models.CompanyName UpdateCompanyName(CompanyName copyName, int individualId);

        /// <summary>
        /// Obtener lista de tipos de email
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<EmailType> GetEmailTypes();

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
        /// Guardar la informaicon laboral de una persona
        /// </summary>
        /// <param name="personJob">Modelo personJob</param>
        /// <returns></returns>
        [OperationContract]
        LabourPerson CreateLabourPerson(LabourPerson personJob, int individualId);
        /// <summary>
        /// Agregar los datos laborales de un persona
        /// </summary>
        /// <param name="person"> Modelo person</param>
        /// <returns></returns>
        [OperationContract(Name = "UpdatePersonCore")]
        Person UpdatePerson(Person person);
        /// <summary>
        /// actualizar la informacion laborar de la persona
        /// </summary>
        /// <param name="personJob">Modelo personJob</param>
        /// <returns></returns>
        [OperationContract]
        LabourPerson UpdateLabourPerson(LabourPerson personJob);

        /// <summary>
        /// Actualizar la informacion laborar de la persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        [OperationContract]
        LabourPerson GetLabourPersonByIndividualId(int individualId);
        /// <summary>
        /// Buscar información laboral
        /// </summary>
        /// <param name="filter">Filtro de la busqueda</param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns></returns>
        [OperationContract(Name = "GetPersonByDocumentNumberSurnameMotherLastNameCore")]
        List<Models.Person> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType);
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
        List<Models.PaymentAccount> GetPaymentMethodAccountByIndividualId(int individualId);

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
        List<Models.Company> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType);

        #region InsuredV1
        [OperationContract]
        Models.Insured CreateInsured(Models.Insured insured);

        [OperationContract]
        Models.Insured GetInsuredByIndividualId(int individualId);
        /// <summary>
        /// 

        [OperationContract]
        Models.Insured GetInsuredElectronicBillingByIndividualId(int individualId);

        #endregion

        /// <summary>
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <returns></returns>
        [OperationContract(Name = "CreateCompanyCore")]
        Models.Company CreateCompany(Models.Company company);

        [OperationContract]
        List<Models.AgentPrefix> GetAgentPrefixByIndividualId(int IndividualId);

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
        /// Crear los mediod de  pago de la persona o de compañia
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.PaymentAccount CreatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int IndividualId);


        [OperationContract]
        List<Models.PaymentAccount> CreatePaymentMethodAccounts(List<Models.PaymentAccount> paymentMethodAccounts, int IndividualId);

        /// <summary>
        /// Crear los mediod de  pago de la persona o de compañia
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.PaymentAccount UpdatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int IndividualId);

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
        List<Models.ProspectNatural> GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(string documentNumber, string surname, string motherLastName, string name, string tradeName, int searchType);

        /// <summary>
        /// Obtener agencia por Identificadores
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>Agencia</returns>
        [OperationContract]
        Agency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId);

        /// <summary>
        /// Buscar prospectos
        /// </summary>
        /// <param name="individualType">Tipo de individuo</param>
        /// <param name="documentTypeId">Id tipo de documento</param>
        /// <param name="document">Documento</param>
        /// <returns>Prospecto</returns>
        [OperationContract]
        Prospect GetProspectByIndividualTypeDocumentTypeIdDocument(IndividualType individualType, int documentTypeId, string document);

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
        /// Obtener lista de tipo de origen
        /// </summary>        
        [OperationContract]
        List<Models.OriginType> GetOriginTypes();



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
        /// Get Individual por individualId de persona
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        Models.Individual GetIndividualByIndividualId(int individualId);

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
        //[OperationContract]
        //Models.Insured GetInsuredByInsuredCode(int insuredCode);

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

        [OperationContract]
        List<Models.ExonerationType> GetExonerationTypes();


        /// <summary>
        /// Recupera Actividad Economica por el Codigo de la Acividad
        /// </summary>
        /// <param name="EconomicActiviti">The economic activiti.</param>
        [OperationContract]
        Models.EconomicActivity GetEconomicActivitiesByEconomicActiviti(int EconomicActiviti);

        /// <summary>
        /// Consulta listados de tipo de bien
        /// </summary>
        /// <returns> Listado de tipos de bien </returns>
        [OperationContract]
        List<AssetType> GetAssetType();


        /// <summary>
        /// Obtener Comisiones por agente
        /// </summary>
        /// <param name="individualId">identificador del individuo</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.Commission> GetAgentCommissionByIndividualId(int individualId);

        /// <summary>
        /// Obtener lista de COmisiones por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        List<Commission> GetAgentCommissionByAgencyId(int agentId);

        /// <summary>
        /// Elimina datos de la Comision del agente
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool DeleteAgentCommission(Commission commissionAgent);

        [OperationContract]
        List<HouseType> GetHouseTypes();

        #region "Insureds"

        [OperationContract]
        List<Models.InsuredSegment> GetInsuredSegment();

        [OperationContract]
        List<Models.InsuredProfile> GetInsuredProfile();

        //[OperationContract]
        //List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter);

        #endregion

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
        /// Crea el impuesto individual 
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



        List<Company> GetCompanyAdv(CustomerType customerType, Company Company);

        [OperationContract]
        List<Models.Address> GetAddresses(int individualId);

        [OperationContract]
        List<Models.Phone> GetPhones(int individualId);

        [OperationContract]
        List<Models.Email> GetEmails(int individualId);

        [OperationContract]
        Insured UpdateInsured(Insured insured);

        [OperationContract]
        Models.Insured UpdateInsuredElectronicBilling(Models.Insured insured);


        [OperationContract]
        Agency CreateAgency(Agency agency, int IndividualId);

        #region Consortium

        [OperationContract]
        Consortium GetConsortiumByInsurendIdOnInvidualId(int InsuredId, int IndividualId);
        [OperationContract]
        List<Consortium> GetConsortiumByInsurendId(int InsuredIdd);
        [OperationContract]
        Consortium CreateConsortium(Consortium consortia);
        [OperationContract]
        Consortium UpdateConsortium(Consortium consortium);
        [OperationContract]
        bool DeleteConsortium(Consortium consortium);

        #endregion

        #region CompanyCoInsured
        [OperationContract]
        CompanyCoInsured CreateCompanyCoInsured(CompanyCoInsured companyCoInsured);
        [OperationContract]
        CompanyCoInsured UpdateCompanyCoInsured(CompanyCoInsured companyCoInsured);
        [OperationContract]
        CompanyCoInsured GetCompanyCoInsuredIndividualId(int IndividualId);
        [OperationContract]
        CompanyCoInsured GetCompanyCoInsuredTributary(string tributaryNo);
        #endregion

        #region Guarantee
        /// <summary>
        /// Obtiene los contragarantes de una contragarantía
        /// </summary>
        /// <returns> Listado de contragarantías </returns>
        List<Models.Guarantor> GetGuarantorsByGuaranteeId(int id);


        #endregion

        #region InsuredGuarantee

        /// <summary>
        /// Obtener la documentacion recivida
        /// </summary>
        /// <param name="IndividualId">IndividualId asegurado</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.GuaranteeRequiredDocument> GetDocumentationReceivedByGuaranteeId(int guaranteeId);

        /// <summary>
        /// Obtener contragarantias asegurado 
        /// </summary>
        /// <param name="IndividualId">IndividualId asegurado</param>
        /// <returns></returns>
        [OperationContract]
        List<Models.GuaranteeInsuredGuarantee> GetInsuredGuaranteeByIndividualId(int IndividualId);

        /// <summary>
        /// Obtener contragarantias asegurado tipo Hipoteca
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.InsuredGuaranteeMortgage GetInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id);

        [OperationContract]
        Models.InsuredGuaranteeMortgage CreateInsuredGuaranteeMortgage(Models.InsuredGuaranteeMortgage insuredGuaranteeMortgage);

        [OperationContract]
        Models.InsuredGuaranteeMortgage UpdateInsuredGuaranteeMortgage(Models.InsuredGuaranteeMortgage insuredGuaranteeMortgage);


        /// <summary>
        /// Obtener contragarantias asegurado tipo prenda
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.InsuredGuaranteePledge GetInsuredGuaranteePledgeByIndividualIdById(int individualId, int id);

        [OperationContract]
        Models.InsuredGuaranteePledge CreateInsuredGuaranteePledge(Models.InsuredGuaranteePledge insuredGuaranteePledge);

        [OperationContract]
        Models.InsuredGuaranteePledge UpdateInsuredGuaranteePledge(Models.InsuredGuaranteePledge insuredGuaranteePledge);


        /// <summary>
        /// Obtener contragarantias asegurado tipo pagaré
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.InsuredGuaranteePromissoryNote GetInsuredGuaranteePromissoryNoteByIndividualIdById(int individualId, int id);

        [OperationContract]
        Models.InsuredGuaranteePromissoryNote CreateInsuredGuaranteePromissoryNote(Models.InsuredGuaranteePromissoryNote insuredGuaranteePromissoryNote);

        [OperationContract]
        Models.InsuredGuaranteePromissoryNote UpdateInsuredGuaranteePromissoryNote(Models.InsuredGuaranteePromissoryNote insuredGuaranteePromissoryNote);

        /// <summary>
        /// Obtener contragarantias asegurado tipo CDT
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.InsuredGuaranteeFixedTermDeposit GetInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id);

        [OperationContract]
        Models.InsuredGuaranteeFixedTermDeposit CreateInsuredGuaranteeFixedTermDeposit(Models.InsuredGuaranteeFixedTermDeposit insuredGuaranteeFixedTermDeposit);

        [OperationContract]
        Models.InsuredGuaranteeFixedTermDeposit UpdateInsuredGuaranteeFixedTermDeposit(Models.InsuredGuaranteeFixedTermDeposit insuredGuaranteeFixedTermDeposit);


        /// <summary>
        /// Obtener contragarantias asegurado tipo Others
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        Models.InsuredGuaranteeOthers GetInsuredGuaranteeOthersByIndividualIdById(int individualId, int id);

        [OperationContract]
        Models.InsuredGuaranteeOthers CreateInsuredGuaranteeOthers(Models.InsuredGuaranteeOthers insuredGuaranteeOthers);

        [OperationContract]
        Models.InsuredGuaranteeOthers UpdateInsuredGuaranteeOthers(Models.InsuredGuaranteeOthers insuredGuaranteeOthers);




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
        /// Guarda contragarantia de un afianzado
        /// </summary>
        /// <param name="guarantee">Contragarantías</param>
        /// <returns> Contragarantía </returns>
        [OperationContract]
        Models.Guarantee SaveInsuredGuarantee(Models.Guarantee guarantee);


        #endregion

        #region insuredGuaranteePrefix

        /// <summary>
        /// Consulta ramos asociados a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        [OperationContract]
        List<Models.InsuredGuaranteePrefix> GetInsuredGuaranteePrefix(int individualId, int guaranteeId);

        [OperationContract]
        Models.InsuredGuaranteePrefix CreateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix insuredGuaranteePrefix);

        [OperationContract]
        Models.InsuredGuaranteePrefix UpdateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix insuredGuaranteePrefix);

        [OperationContract]
        void DeleteInsuredGuaranteePrefix(int individualId, int guaranteeId, int insureedguaranteePrefixId);


        #endregion

        #region InsuredGuaranteeLog

        /// <summary>
        /// Consulta ramos asociados a una bitacora
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        [OperationContract]
        List<Models.InsuredGuaranteeLog> GetInsuredGuaranteeLogByIndividualIdByGuaranteeId(int individualId, int guaranteeId);

        [OperationContract]
        Models.InsuredGuaranteeLog CreateInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog);

        [OperationContract]
        Models.InsuredGuaranteeLog UpdateInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog);

        [OperationContract]
        void DeleteInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog);

        #endregion

        #region InsuredGuaranteeDocumentation

        [OperationContract]

        InsuredGuaranteeDocumentation CreateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation insuredGuaranteeDocumentation);


        [OperationContract]

        InsuredGuaranteeDocumentation UpdateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation insuredGuaranteeDocumentation);

        [OperationContract]

        void DeleteInsuredGuaranteeDocumentation(int individualId, int insuredguaranteeId, int guaranteeId, int documentId);

        [OperationContract]

        InsuredGuaranteeDocumentation GetInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId);

        [OperationContract]

        List<InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation();

        /// <summary>
        /// Consulta documentación asociada a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Documentación asociada a una contragarantía </returns>
        [OperationContract]
        List<InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocument(int individualId, int guaranteeId);


        #endregion

        #region IndividualPaymentmethod
        /// <summary>
        /// Consultar medios de pagos
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List<Models.IndividualPaymentMethod></returns>
        List<Models.IndividualPaymentMethod> GetIndividualPaymentMethods(int individualId);

        /// <summary>
        /// CreateindividualPaymentMethods
        /// </summary>
        /// <param name="individualPaymentMethods"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        Models.IndividualPaymentMethod CreateIndividualPaymentMethod(Models.IndividualPaymentMethod individualPaymentMethods, int individualId);

        /// <summary>
        /// UpdateIndividualPaymentMethod
        /// </summary>
        /// <param name="individualPaymentMethods"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        Models.IndividualPaymentMethod UpdateIndividualPaymentMethod(Models.IndividualPaymentMethod individualPaymentMethods, int individualId);

        Models.PaymentAccount GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(Models.PaymentAccount paymentAccount, int paymentId, int individualId);

        #endregion

        #region Legal Representative

        /// <summary>
        /// Guardar la informacion de una representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentative CreateLegalRepresent(LegalRepresentative legalRepresent, int individualId);

        /// <summary>
        /// Buscar la infromacion de un representante legal
        /// </summary>
        /// <param name="idCardNo"></param>
        /// <param name="idCardTypeCode"></param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentative GetLegalRepresentByIndividualId(int individualId);

        /// <summary>
        /// actualiza la informacion de una representante legal
        /// </summary>
        /// <param name="legalRepresent"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        [OperationContract]
        LegalRepresentative UpdateLegalRepresent(LegalRepresentative legalRepresent, int individualId);

        #endregion

        #region Prospect

        [OperationContract]
        List<Models.ProspectNatural> GetProspectNaturalByDocument(string documentNumber);

        /// <summary>
        /// Crear un prospect natural
        /// </summary>
        /// <param name="prospectNatural">Modelo prospecto natural</param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural CreateProspectNatural(Models.ProspectNatural prospectNatural);

        /// <summary>
        /// Actualizar los datos de un prospecto natural
        /// </summary>
        /// <param name="prospectNatural"></param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural UpdateProspectNatural(Models.ProspectNatural prospectNatural);

        [OperationContract]
        List<ProspectNatural> GetProspectLegalByDocument(string documentNumber);

        /// <summary>
        /// Crear un prospect legal
        /// </summary>
        /// <param name="prospectLegal">Modelo prospecto legal</param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural CreateProspectLegal(Models.ProspectNatural prospectLegal);

        /// <summary>
        /// Actualizar los datos de un prospecto juridico
        /// </summary>
        /// <param name="prospectLegal">Modelo de prospecto juridico</param>
        /// <returns></returns>
        [OperationContract]
        Models.ProspectNatural UpdateProspectLegal(Models.ProspectNatural prospectLegal);

        #endregion

        #region Guarantor

        [OperationContract]
        List<Models.Guarantor> GetGuarantorByIndividualIdByGuaranteeId(int individualId, int guaranteeId);

        [OperationContract]
        Models.Guarantor CreateGuatantor(Models.Guarantor guarantor);

        [OperationContract]
        Models.Guarantor UpdateGuarantor(Models.Guarantor guarantor);

        [OperationContract]
        void DeleteGuarantor(Models.Guarantor guarantor);

        #endregion

        #region CompanyInsurance
        /// <summary>
        /// Obtener la lista de compañias Coaseguradoras
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        List<Models.CoInsuranceCompany> GetCoInsuranceCompanies();

        #endregion

        /// <summary>
        /// GetDocumentsTypeRangeId
        /// </summary>
        /// <param name="DocumentTypeRangeId"></param>
        /// <returns></returns>
        [OperationContract]
        List<Models.DocumentTypeRange> GetDocumentsTypeRangeId(int DocumentTypeRangeId);

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        [OperationContract]
        List<InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId);

        /// <summary>
        /// DocumentTypeRange
        /// </summary>
        /// <param name="DocumentTypeRange"></param>
        /// <returns></returns>
        [OperationContract]
        Models.DocumentTypeRange CreateDocumentTypeRange(Models.DocumentTypeRange DocumentTypeRange);

        /// <summary>
        /// Obtener lista de COmisiones por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        [OperationContract]
        List<CommissionAgent> GetAgentCommissionByAgentId(int agentId);

        [OperationContract]
        List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter);

        /// <summary>
        /// GetIndividualTypeById
        /// </summary>
        /// <param name="individualTypeId"></param>
        /// <returns></returns>
        [OperationContract]
        ModelIndividual.IndividualType GetIndividualTypeById(int individualTypeId);

        /// <summary>
        /// Obtiene la lista de Segmentos de Asegurado.
        /// </summary>
        /// <returns>Lista de Segmentos de Asegurado consultados</returns>
        [OperationContract]
        List<InsuredSegmentV1> GetInsuredSegments();
        /// <summary>
        /// Adiciona y Guarda los cambios del Segmento de Asegurado.
        /// </summary>
        /// <returns>Lista de Segmentos de Asegurado consultados</returns>
        [OperationContract]
        ParametrizationResponse<InsuredSegmentV1> CreateInsuredSegments(List<InsuredSegmentV1> ListAdded, List<InsuredSegmentV1> ListEdited, List<InsuredSegmentV1> ListDeleted);

        #region InsuredProfileV1
        /// <summary>
        /// 
        /// </summary>
        /// <param name="insuredProfile"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        string GenerateFileToInsuredProfile(List<Models.InsuredProfile> insuredProfile, string fileName);

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
        #endregion InsuredProfileV1

        /// Genera archivo excel de Segmentos de Asegurado.
        /// </summary>
        /// <param name="insuredSegment"></param>
        /// <param name="fileName"></param>
        /// <returns>Path archivo de excel</returns>
        [OperationContract]
        string GenerateFileToInsuredSegment(List<InsuredSegmentV1> insuredSegment, string fileName);

        #region BusinessName
        [OperationContract]
        Models.CompanyName CreateBusinessName(Models.CompanyName companyName);

        [OperationContract]
        Models.CompanyName UpdateBusinessName(Models.CompanyName companyName);

        [OperationContract]
        int CountBusinessNameByIndividualId(int individualId);

        [OperationContract]

        List<Models.CompanyName> GetBusinessNameByIndividualId(int individualId);

        [OperationContract]
        Models.CompanyExtended GetCoCompanyByIndividualId(int individualId);
        #endregion

        #region BankTransfers
        [OperationContract]
        List<Models.BankTransfers> CreateBankTransfers(List<Models.BankTransfers> companyBankTransfers);

        [OperationContract]
        BankTransfers UpdateBankTransfers(BankTransfers companyBankTransfers);

        [OperationContract]
        List<BankTransfers> GetBankTransfersByIndividualId(int individualId);
        #endregion BankTransfers

        #region ElectronicBilling
        [OperationContract]
        List<Models.InsuredFiscalResponsibility> CreateInsuredFiscalResponsibility(List<Models.InsuredFiscalResponsibility> companyInsuredFiscalResponsibility);

        [OperationContract]
        List<Models.InsuredFiscalResponsibility> GetFiscalResponsibilityByIndividualId(int individualId);

        [OperationContract]
        Models.FiscalResponsibility GetFiscalResponsibilityById(int Id);

        [OperationContract]
        Models.InsuredFiscalResponsibility UpdateInsuredFiscalResponsibility(Models.InsuredFiscalResponsibility companyInsuredFiscalResponsibility);

        [OperationContract]
        bool DeleteFiscalResponsibility(InsuredFiscalResponsibility fiscal);

        #endregion ElectronicBilling


        #region Consortium
        [OperationContract]
        Consortium GetConsortiumByIndividualId(int IndividualId);
        #endregion
        #region EconomicGroup
        [OperationContract]
        Models.EconomicGroup CreateEconomicGroup(Models.EconomicGroup economicGroup, List<Models.EconomicGroupDetail> listGroupDetail);

        [OperationContract]
        List<Models.EconomicGroup> GetGroupEconomicById(int Id);

        [OperationContract]
        List<Models.TributaryIdentityType> GetTributaryType();

        [OperationContract]
        List<Models.EconomicGroup> GetEconomicGroupByDocument(string groupName, string documentNo);

        [OperationContract]
        List<Models.EconomicGroup> GetEconomicGroupByEconomicGroup(Models.EconomicGroup economicGroup);

        [OperationContract]
        List<Models.Insured> GetEconomicGroupInsureds(int economicGroupId);

        #endregion
        [OperationContract]
        List<GuaranteeStatus> GetGuaranteeStatusRoutesByGuaranteeStatusId(int guaranteeStatusId);

        [OperationContract]
        List<GuaranteeStatus> GetUnassignedGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId);

        [OperationContract]
        List<GuaranteeStatus> CreateGuaranteeStatusRoutes(List<GuaranteeStatus> allGuaranteeEstatusAssign, int guaranteeStatusId);

        [OperationContract]
        List<GuaranteeStatus> GetGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId);

        [OperationContract]
        Models.EconomicGroupDetail GetExistIndividdualByIndividualId(int IndividualId);

        [OperationContract]
        List<Models.Consortium> GetConsortiumsByIndividualId(int individualId);


        [OperationContract]
        List<Models.EconomicGroupDetail> GetEconomicGroupDetailByIndividual(int IndividualId);

        #region Politicas
        [OperationContract]
        Models.PersonOperation CreatePersonOperation(Models.PersonOperation personOperation);

        [OperationContract]
        Models.PersonOperation GetPersonOperation(int personOperation);

        [OperationContract]
        List<Models.PersonOperation> GetOperationTmp(int IndividualId);
        #endregion Politicas

        [OperationContract]
        Models.Person GetPersonByDocumentByDocumentType(string document, int documentType);

        [OperationContract]
        Models.Company GetCompanyByDocumentByDocumentType(string document, int documentType);
        [OperationContract]
        List<Models.AccountType> GetAccountTypes();

        [OperationContract]
        bool DeleteUserAssignedConsortium(int parameterFutureSociety, int userId);

        [OperationContract]
        Models.UserAssignedConsortium GetUserAssignedConsortiumByparameterFutureSocietyByuserId(int parameterFutureSociety, int userId);

        [OperationContract]
        List<Models.EconomicGroupDetail> GetEconomicGroupDetailById(int economicGroupId);

    }
}
