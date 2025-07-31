using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.DAOs;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.UniquePersonService.Providers;
using Sistran.Core.Application.UniquePersonService.Resources;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using ModelIndividual = Sistran.Core.Application.UniquePersonServiceIndividual.Models;
using PersonBase = Sistran.Core.Application.UniquePersonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.EEProvider.DAOs;
using Sistran.Core.Application.CommonService.Models.Base;
using System.Linq;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Application.UniquePersonService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonServiceEEProvider : IUniquePersonServiceCore
    {

        /// <summary>
        /// Obtener tipo de direcciones
        /// </summary>
        /// <returns>Lista tipo de direcciones </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AddressType> GetAddressesTypes()
        {
            try
            {
                AddressDAO addressDAO = new AddressDAO();
                return addressDAO.GetAddressTypes(false);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Direcciones por tipo de individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error Obteniendo GetAddressesByIndividualId</exception>
        public List<Models.Address> GetAddressesByIndividualId(int individualId)
        {
            try
            {
                AddressDAO addressDAO = new AddressDAO();
                return addressDAO.GetAddresses(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de teléfono
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error Obteniendo GetPhoneTypes</exception>
        public List<Models.PhoneType> GetPhoneTypes()
        {
            try
            {
                PhoneDAO phoneDAO = new PhoneDAO();
                return phoneDAO.GetPhoneTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de teléfonos asociados a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Phone> GetPhonesByIndividualId(int individualId)
        {
            try
            {
                PhoneDAO phoneDAO = new PhoneDAO();
                return phoneDAO.GetPhonesByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.DocumentType> GetDocumentTypes(int typeDocument)
        {
            try
            {
                DocumentTypeDAO documentypeDAO = new DocumentTypeDAO();
                return documentypeDAO.GetDocumentTypes(typeDocument);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Asociado
        /// </summary>
        /// <param name="documentId">Numero Documento o Nombre</param>
        /// <param name="documentType">Tipo de Documento</param>
        /// <param name="IndividualId">Id de individuo</param>
        /// <returns></returns>
        public Models.Partner GetPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int individualId)
        {
            try
            {
                PartnerDAO parnertDAO = new PartnerDAO();
                return parnertDAO.GetPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.Partner CreatePartner(Models.Partner partner, int individualId)
        {
            try
            {
                PartnerDAO parnertDAO = new PartnerDAO();
                IndividualPartner IndParNer = EntityAssembler.IndividualPartnerFields(partner);
                return parnertDAO.CreatePartner(partner, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.Partner> GetPartnerByIndividualId(int individualId)
        {
            try
            {
                PartnerDAO parnertDAO = new PartnerDAO();
                return parnertDAO.GetPartnerByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de email
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.EmailType> GetEmailTypes()
        {
            try
            {
                EmailDAO emailDAO = new EmailDAO();
                return emailDAO.GetEmailTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar la informacion de un representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns
        public Models.LegalRepresentative CreateLegalRepresent(Models.LegalRepresentative legalRepresent, int individualId)
        {
            try
            {
                LegalRepresentativeDAO legalRepresentDAO = new LegalRepresentativeDAO();
                return legalRepresentDAO.CrateRepresentLegal(legalRepresent, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de emails asociados a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        /// 
        public List<Models.Email> GetEmailsByIndividualId(int individualId)
        {
            try
            {
                EmailDAO emailDAO = new EmailDAO();
                return emailDAO.GetEmailsByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de Niveles Educativos
        /// </summary>
        /// <returns></returns>
        public System.Collections.Generic.List<Models.EducativeLevel> GetEducativeLevels()
        {
            try
            {
                EducativeLevelDAO educativaLevel = new EducativeLevelDAO();
                return educativaLevel.GetEducativeLevels();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de estratos
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.SocialLayer> GetSocialLayers()
        {
            try
            {
                SocialLayerDAO socialLayerDAO = new SocialLayerDAO();
                return socialLayerDAO.GetSocialLayers();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Cargos
        /// </summary>
        /// <returns></returns>
        public List<Models.Occupation> GetOccupations()
        {
            try
            {
                OccupationDAO occupationDAO = new OccupationDAO();
                return occupationDAO.GetOccupations();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de Especialidades
        /// </summary>
        /// <returns></returns>
        public List<Models.Speciality> GetSpecialties()
        {
            try
            {
                SpecialityDAO specialityDAO = new SpecialityDAO();
                return specialityDAO.GetSpecialties();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de salarios
        /// </summary>
        /// <returns></returns
        public List<Models.IncomeLevel> GetIncomeLevels()
        {
            try
            {
                IncomeLevelDAO incomeLevelDAO = new IncomeLevelDAO();
                return incomeLevelDAO.GetIncomeLevels();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Buscar Un Representate legal
        /// </summary>
        /// <param name="idCardNo">Numero del Documento</param>
        /// <param name="idCardTypeCode">Tipo de docuemnto</param>
        /// <returns></returns
        public Models.LegalRepresentative GetLegalRepresentByIndividualId(int individualId)
        {
            try
            {
                LegalRepresentativeDAO legalRepresent = new LegalRepresentativeDAO();
                return legalRepresent.GetLegalRepresentByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar la informacion de un representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public Models.LegalRepresentative UpdateLegalRepresent(Models.LegalRepresentative legalRepresent, int individualId)
        {
            try
            {
                LegalRepresentativeDAO legalRepresentModel = new LegalRepresentativeDAO();
                return legalRepresentModel.UpdateLegalRepresent(legalRepresent, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar los los datos de un persona
        /// </summary>
        /// <param name="person">Modelo person</param>
        /// <param name="personIndividualType"></param>
        /// <returns></returns>
        public virtual Models.Person UpdatePerson(Models.Person person, Models.PersonIndividualType personIndividualType)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.UpdatePerson(person, personIndividualType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar la informacion laboral de la persona
        /// </summary>
        /// <param name="personJob">Moledo PersonJob</param>
        /// <returns></returns
        public Models.LaborPerson CreatePersonJob(Models.LaborPerson personJob, int individualId)
        {
            try
            {
                PersonJobDAO personJobDAO = new PersonJobDAO();
                return personJobDAO.CreatePersonJob(personJob, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar los datos laborales de la persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns
        public Models.LaborPerson UpdatePersonJob(Models.LaborPerson personJob)
        {
            try
            {
                PersonJobDAO personjobModel = new PersonJobDAO();
                return personjobModel.UpdatePersonJob(personJob);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns
        public Models.LaborPerson GetPersonJobByIndividualId(int individualId)
        {
            try
            {
                PersonJobDAO personJobDAO = new PersonJobDAO();
                return personJobDAO.GetPersonJobByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Buscar persona por número de documento o apellidos y nombres
        /// </summary>
        /// <param name="documentNumber">número de documento</param>
        /// <param name="surname">primer apellido</param>
        /// <param name="motherLastName">segundo apellido</param>
        /// <param name="name">nombres</param>
        /// <param name="searchType">tipo de busqueda</param>
        /// <returns></returns>
        public virtual List<Models.Person> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType, int? documentType, int? individualId)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.GetPersonByDocumentNumberSurnameMotherLastName(documentNumber, surname, motherLastName, name, searchType, documentType, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Tipos de pagos
        /// </summary>
        /// <returns></returns>
        public List<Models.PaymentAccountType> GetPaymentTypes()
        {
            try
            {
                PaymentAccountTypeDAO paymentAccountTypeDAO = new PaymentAccountTypeDAO();
                return paymentAccountTypeDAO.GetPaymentTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Metodos de Pago
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Lista Metodos de Pago</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.PaymentMethodAccount> GetPaymentMethodAccountByIndividualId(int individualId)
        {
            try
            {
                PaymentMethodAccountDAO paymentMethodaccountDAO = new PaymentMethodAccountDAO();
                return paymentMethodaccountDAO.GetPaymentMethodAccountByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista de los Motivos de baja para los agentes
        /// </summary>
        /// <returns>Lista AgentDeclinedType</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AgentDeclinedType> GetAgentDeclinedTypes()
        {

            try
            {
                AgentDeclinedTypeDAO agentDeclinedTypeDAO = new AgentDeclinedTypeDAO();
                return agentDeclinedTypeDAO.GetAgentDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creacion del Agente
        /// </summary>
        /// <param name="Agent">Models.Agent</param>
        /// <returns>
        /// Models.Agent
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Agent CreateAgent(Models.Agent Agent)
        {
            try
            {
                AgentDAO agentDAO = new AgentDAO();
                return agentDAO.CreateAgent(Agent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Buscar compañias y prospectos por número de documento o por razón social
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <param name="name"></param>
        /// <param name="searchType">Tipo de busqueda
        /// 2. Compañia
        /// 3. Prospecto
        /// 4. Todos</param>
        /// <returns>
        /// Models.Company
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        virtual public List<Models.Company> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType, int? documentType, int? individualId)
        {
            try
            {
                CompanyDAO company = new CompanyDAO();
                return company.GetCompaniesByDocumentNumberNameSearchType(documentNumber, name, searchType, documentType, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de agencias por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Agency> GetAgenciesByAgentId(int agentId)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgenciesByAgentId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// crear un asegurado
        /// </summary>
        /// <param name="insured">modelo Insured</param>
        /// <returns></returns>
        public Models.Insured CreateInsured(Models.Insured insured)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                return insuredDAO.CreateInsured(insured);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// buscar el asegurado por el individualId
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns></returns>
        public Models.Insured GetInsuredByIndividualId(int individualId)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                return insuredDAO.GetInsuredByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Buscar el asegurado por el individualCode
        /// </summary>
        /// <param name="individualId">individualCode</param>
        /// <returns></returns>
        public Models.Insured GetInsuredByIndividualCode(int individualCode)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                return insuredDAO.GetInsuredByIndividualCode(individualCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <param name="coCompany"></param>
        /// <param name="coConsortium"></param>
        /// <param name="coCompanyName"></param>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public virtual Models.Company CreateCompany(Models.Company company)
        {
            try
            {
                CompanyDAO companyDAO = new CompanyDAO();
                return companyDAO.CreateCompany(company);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener agente por Id
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>Models.Agent</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Agent GetAgentByAgentId(int agentId)
        {
            try
            {
                AgentDAO agentDAO = new AgentDAO();
                return agentDAO.GetAgentByAgentId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene Estado Civil
        /// </summary>
        /// <returns>Models.MaritalStatus</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.MaritalStatus> GetMaritalStatus()
        {
            try
            {
                MaritalStatusDAO maritalStatusDAO = new MaritalStatusDAO();
                return maritalStatusDAO.GetMaritalStatus();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos de asociación
        /// </summary>
        /// <returns>Models.CoAssociationType</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AssociationType> GetAssociationTypes()
        {
            try
            {
                CoAssociationTypeDAO coAssociationTypeDAO = new CoAssociationTypeDAO();
                return coAssociationTypeDAO.GetAssociationTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene Ramos por Agente
        /// </summary>
        /// <param name="IndividualId">Id Individuo</param>
        /// <returns>
        /// Models.AgentPrefix
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AgentPrefix> GetAgentPrefixByIndividualId(int IndividualId)
        {
            try
            {
                AgentPrefixDAO agentPrefix = new AgentPrefixDAO();
                return agentPrefix.GetAgentPrefixByIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el listado de los motivos de baja del asegurado
        /// </summary>
        /// <returns>Models.InsuredDeclinedType</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.InsuredDeclinedType> GetInsuredDeclinedTypes()
        {
            try
            {
                InsuredDeclinedTypeDAO insuredDeclinedTypeDAO = new InsuredDeclinedTypeDAO();
                return insuredDeclinedTypeDAO.GetInsuredDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public virtual Models.Company GetCompanyByIndividualId(int individualId)
        {
            try
            {
                CompanyDAO companyDAO = new CompanyDAO();
                return companyDAO.GetCompanyByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <param name="coCompany"></param>
        /// <param name="coConsortium"></param>
        /// <param name="coCompanyName"></param>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public virtual Models.Company UpdateCompany(Models.Company company)
        {
            try
            {
                CompanyDAO companyDAO = new CompanyDAO();
                return companyDAO.UpdateCompany(company);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear las formas de pago de la persona o de la compañia
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="IndividualId"></param>
        /// <returns>Models.PaymentMethodAccount</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.PaymentMethodAccount CreatePaymentMethodAccount(Models.PaymentMethodAccount paymentMethodAccount, int IndividualId)
        {
            try
            {
                PaymentMethodAccountDAO paymentAccountTypeDAO = new PaymentMethodAccountDAO();
                return paymentAccountTypeDAO.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear las formas de pago de la persona o de la compañia
        /// </summary>
        /// <param name="paymentMethodAccounts"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public List<Models.PaymentMethodAccount> CreatePaymentMethodAccounts(List<Models.PaymentMethodAccount> paymentMethodAccounts, int IndividualId)
        {

            List<Models.PaymentMethodAccount> ModelpaymentMethodAccounts = new List<Models.PaymentMethodAccount>();
            PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            PaymentMethodAccountDAO paymentAccountTypeDAO = new PaymentMethodAccountDAO();

            foreach (Models.PaymentMethodAccount paymentMethodAccount in paymentMethodAccounts)
            {
                paymentMethodDAO.CreatePaymentMethod(paymentMethodAccount, IndividualId);
                if (paymentMethodAccount.PaymentMethod.Id != (int)PaymentMethodType.Cash)
                {
                    ModelpaymentMethodAccounts.Add(paymentAccountTypeDAO.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId));
                }
            }

            return ModelpaymentMethodAccounts;
        }

        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public virtual Models.Person CreatePerson(Models.Person person, Models.PersonIndividualType personIndividualType)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.CreatePerson(person, personIndividualType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear un prospect natural
        /// </summary>
        /// <param name="prospectNatural">Modelo prospecto legal</param>
        /// <returns>
        /// Models.ProspectNatural
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.ProspectNatural CreateProspectNatural(Models.ProspectNatural prospectNatural)
        {
            try
            {
                ProspectNaturalDAO prospectoNaturalDAO = new ProspectNaturalDAO();
                return prospectoNaturalDAO.CreateProspectNatural(prospectNatural);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear un prospect legal
        /// </summary>
        /// <param name="prospectLegal">Modelo prospecto legal</param>
        /// <returns>Models.ProspectNatural</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.ProspectNatural CreateProspectLegal(Models.ProspectNatural prospectLegal)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.CreateProspectLegal(prospectLegal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar los datos de un prospesto legal
        /// </summary>
        /// <param name="prospectLegal">Modelo prospecto legal</param>
        /// <returns>
        /// Models.ProspectNatural
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.ProspectNatural UpdateProspectLegal(Models.ProspectNatural prospectLegal)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.UpdateProspectLegal(prospectLegal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualizar un prospecto natural
        /// </summary>
        /// <param name="prospectNatural">Modelo de prospecto natural</param>
        /// <returns>Models.ProspectNatural</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.ProspectNatural UpdateProspectNatural(Models.ProspectNatural prospectNatural)
        {
            try
            {
                ProspectNaturalDAO prospectoNaturalDAO = new ProspectNaturalDAO();
                return prospectoNaturalDAO.UpdateProspectNatural(prospectNatural);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de ramos comerciales por agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>List<Prefix></returns>
        /// <exception cref="BusinessException"></exception>
        public List<BasePrefix> GetPrefixesByAgentId(int agentId)
        {
            try
            {
                AgentPrefixDAO agentPrefixDAO = new AgentPrefixDAO();
                return agentPrefixDAO.GetPrefixesByAgentId(agentId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Agentes por Individual Id
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>
        /// Models.Agent
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Agent GetAgentByIndividualId(int individualId)
        {
            try
            {
                AgentDAO agentDAO = new AgentDAO();
                return agentDAO.GetAgentByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Los Tipos de Agente
        /// </summary>
        /// <returns>List<Models.AgentType></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AgentType> GetAgentTypes()
        {
            try
            {
                AgentTypeDAO agentTypeDAO = new AgentTypeDAO();
                return agentTypeDAO.GetAgentTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// listado del tipos de personas
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.PersonType> GetPersonTypes()
        {
            try
            {
                PersonTypeDAO personTypeDAO = new PersonTypeDAO();
                return personTypeDAO.GetPersonTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the agents.
        /// </summary>
        /// <returns>List<Models.Agent></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Agent> GetAgents()
        {
            try
            {
                AgentDAO agents = new AgentDAO();
                return agents.GetAgents();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene Agentes por Filtro
        /// </summary>
        /// <param name="query">Filtro</param>
        /// <returns>List<Models.Agent> </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Agent> GetAgentByQuery(string query)
        {
            try
            {
                AgentDAO agents = new AgentDAO();
                return agents.GetAgentByQuery(query);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Agencias por Id Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Agency> GetAgencyByIndividualId(int individualId)
        {
            try
            {
                AgencyDAO agencies = new AgencyDAO();
                return agencies.GetAgencyByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Gets the name of the agent by.
        /// </summary>
        /// <param name="nameAgent">The name agent.</param>
        /// <returns>Models.Agent</returns>
        /// <exception cref="BusinessException">Error Obteniendo GetAgentByName</exception>
        public List<Models.Agent> GetAgentByName(string nameAgent)
        {
            try
            {
                AgentDAO agentDAO = new AgentDAO();
                return agentDAO.GetAgentByName(nameAgent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Prospecto por Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Models.ProspectNatural</returns>
        /// <exception cref="BusinessException">Error Obteniendo GetProspectNaturalByIndividualId</exception>
        public Models.ProspectNatural GetProspectNaturalByIndividualId(int individualId)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.GetProspectNaturalByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener prospecto legal por individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Models.ProspectNatural</returns>
        /// <exception cref="BusinessException">Error Obteniendo GetProspectLegalByIndividualId</exception>
        public Models.ProspectNatural GetProspectLegalByIndividualId(int individualId)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.GetProspectLegalByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        ///Obtiene Compañias Por Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Models.CoCompanyName</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.CompanyName> GetCompanyNamesByIndividualId(int individualId)
        {
            try
            {
                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
                return companyNameDAO.GetCompanyNamesByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener el Agente por codigo o nombre
        /// </summary>
        /// <param name="agentCode">Codigo Agente</param>
        /// <param name="fullName">Nombre Agente</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error Obteniendo GetInsuredsByDescription</exception>
        public List<Models.Agent> GetAgentByAgentCodeFullName(int agentCode, string fullName)
        {
            try
            {
                AgentDAO ag = new AgentDAO();
                return ag.GetAgentByAgentCodeFullName(agentCode, fullName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Asegurados por Nombre
        /// </summary>
        /// <param name="description"></param>
        /// <param name="insuredSearchType"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Insured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                return insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el Cupo Operativo y el Cumulo
        /// <para> </para><para>
        /// Valores Salida
        /// </para><para> </para><para>Decimal  Cupo Operativo</para><para>Decimal Cumulo</para>
        /// </summary>
        /// <param name="individualId">Individual Id Asegurado</param>
        /// <param name="lineBusinessCode">Linea del Negocio</param>
        /// <param name="issueDate">Fecha Hasta</param>
        /// <returns>
        /// Lista de sumas
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        /// para
        /// <code>
        /// AggregateDAO aggregateDAO = new AggregateDAO();
        /// </code>
        public List<Amount> GetAvailableAmountByIndividualId(int individualId, int lineBusinessCode, DateTime issueDate)
        {
            try
            {
                AggregateDAO aggregate = new AggregateDAO();
                return aggregate.GetAvailableAmountByIndividualId(individualId, lineBusinessCode, issueDate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener agencia por Identificadores
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <param name="agentAgencyId">Id agencia</param>
        /// <returns>
        /// Agencia
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Agency GetAgencyByAgentIdAgentAgencyId(int agentId, int agentAgencyId)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgencyByAgentIdAgentAgencyId(agentId, agentAgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// listado de todas las personas
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <param name="surname"></param>
        /// <param name="motherLastName"></param>
        /// <param name="name"></param>
        /// <param name="tradeName"></param>
        /// <param name="searchType"></param>
        /// <returns>
        /// Models.ProspectNatural
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.ProspectNatural> GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(string documentNumber, string surname, string motherLastName, string name, string tradeName, int searchType, int? documentType, int? individualId)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(documentNumber, surname, motherLastName, name, tradeName, searchType, documentType, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// listado de Contragarantias
        /// </summary>
        /// <param name="IndividualId">IndividualId asegurado</param>
        /// <returns>
        /// List Models.Guarantee
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Guarantee> GetInsuredGuaranteeByIndividualId(int IndividualId)
        {
            try
            {
                GuaranteeDAO guarantee = new GuaranteeDAO();
                return guarantee.GetInsuredGuaranteeByIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// listado de Contragarantias       
        /// <param name="id">Id del afianzado</param>
        /// <returns>
        /// Contragarantías
        /// </returns>
        public List<Models.Guarantee> GetInsuredGuaranteesByIndividualId(int id)
        {
            try
            {
                GuaranteeDAO guarantee = new GuaranteeDAO();
                return guarantee.GetInsuredGuaranteesByIndividualId(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el listado de estados de contragarantías
        /// </summary>
        /// <returns>
        /// Listado de estados de contragarantías
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.GuaranteeStatus> GetGuaranteeStatus()
        {
            try
            {
                GuaranteeStatusDAO guaranteeStatusDAO = new GuaranteeStatusDAO();
                return guaranteeStatusDAO.GetGuaranteeStatus();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene los tipos de pagaré
        /// </summary>
        /// <returns>
        /// Listado de tipos de pagaré
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.PromissoryNoteType> GetPromissoryNoteType()
        {
            try
            {
                PromissoryNoteTypeDAO promissoryNoteTypeDAO = new PromissoryNoteTypeDAO();
                return promissoryNoteTypeDAO.GetPromissoryNoteType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene las unidades de medida
        /// </summary>
        /// <returns>
        /// Listado de unidad de medida
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.MeasurementType> GetMeasurementType()
        {
            try
            {
                MeasurementTypeDAO measurementTypeDAO = new MeasurementTypeDAO();
                return measurementTypeDAO.GetMeasurementType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta una contragarantía dado su id
        /// </summary>
        /// <param name="guaranteeId">Id de la contragarantía</param>
        /// <returns>
        /// Models.Guarantee
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Guarantee GetInsuredGuaranteeByIdGuarantee(int guaranteeId)
        {
            try
            {
                GuaranteeDAO guaranteeDAO = new GuaranteeDAO();
                return guaranteeDAO.GetInsuredGuaranteeByIdGuarantee(guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda contragarantias de un afianzado
        /// </summary>
        /// <param name="listGuarantee">Lista Contragarantías</param>
        /// <returns>
        /// List Models.Guarantee
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Guarantee> SaveInsuredGuarantees(List<Models.Guarantee> listGuarantee)
        {
            try
            {
                GuaranteeDAO guarantee = new GuaranteeDAO();
                return guarantee.SaveInsuredGuarantees(listGuarantee);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta documentación asociada a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Documentación asociada a una contragarantía </returns>
        public List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation(int individualId, int guaranteeId)
        {
            try
            {
                InsuredGuaranteeDocumentationDAO insuredGuaranteeDocumentationDAO = new InsuredGuaranteeDocumentationDAO();
                return insuredGuaranteeDocumentationDAO.GetInsuredGuaranteeDocumentation(individualId, guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Consulta ramos asociados a una contragarantía
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <param name="guaranteeId">Id de la contragarantía</param>
        /// <returns>
        /// Ramos asociados a una contragarantía
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.InsuredGuaranteePrefix> GetInsuredGuaranteePrefix(int individualId, int guaranteeId)
        {
            try
            {
                InsuredGuaranteePrefixDAO insuredGuaranteePrefixDAO = new InsuredGuaranteePrefixDAO();
                return insuredGuaranteePrefixDAO.GetInsuredGuaranteePrefix(individualId, guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guarda contragarantia de un afianzado
        /// </summary>
        /// <param name="guarantee">Contragarantías</param>
        /// <returns>
        /// Models.Guarantee 
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Guarantee SaveInsuredGuarantee(Models.Guarantee guarantee)
        {
            try
            {
                GuaranteeDAO guaranteeDAO = new GuaranteeDAO();
                return guaranteeDAO.SaveInsuredGuarantee(guarantee);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>
        /// Listado de Bitacora de asegurado y garantia
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId)
        {
            try
            {
                InsuredGuaranteeLogDAO insGuaranteeDAO = new InsuredGuaranteeLogDAO();
                return insGuaranteeDAO.GetInsuredGuaranteeLogs(individualId, guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene Documentación Recibida
        /// </summary>
        /// <param name="guaranteeId"></param>
        /// <returns>
        /// List Models.GuaranteeRequiredDocument
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.GuaranteeRequiredDocument> GetDocumentationReceivedByGuaranteeId(int guaranteeId)
        {
            try
            {
                InsuredGuaranteeDocumentationDAO insuredGuaranteeDocumentationDAO = new InsuredGuaranteeDocumentationDAO();
                return insuredGuaranteeDocumentationDAO.GetDocumentationReceivedByGuaranteeId(guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Buscar prospectos
        /// </summary>
        /// <param name="individualType">Tipo de individuo</param>
        /// <param name="documentTypeId">Id tipo de documento</param>
        /// <param name="document">Documento</param>
        /// <returns>
        /// Prospecto
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Prospect GetProspectByIndividualTypeDocumentTypeIdDocument(Enums.IndividualType individualType, int documentTypeId, string document)
        {
            try
            {
                ProspectDAO prospectDAO = new ProspectDAO();
                return prospectDAO.GetProspectByIndividualTypeDocumentTypeIdDocument(individualType, documentTypeId, document);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear prospecto
        /// </summary>
        /// <param name="prospect">Datos prospecto</param>
        /// <returns>
        /// Prospecto
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Prospect CreateProspect(Models.Prospect prospect)
        {
            try
            {
                ProspectDAO prospectDAO = new ProspectDAO();
                return prospectDAO.CreateProspect(prospect);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar prospecto
        /// </summary>
        /// <param name="prospect">Datos prospecto</param>
        /// <returns>
        /// Prospecto
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Prospect UpdateProspect(Models.Prospect prospect)
        {
            try
            {
                ProspectDAO prospectDAO = new ProspectDAO();
                return prospectDAO.UpdateProspect(prospect);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear Actualizar Cupo Operativo
        /// </summary>
        /// <param name="listOperatingQuota">List OperatingQuota</param>
        /// <returns>
        /// List OperatingQuota
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.OperatingQuota> CreateOperatingQuota(List<Models.OperatingQuota> listOperatingQuota)
        {
            try
            {
                OperatingQuotaDAO operatingQuotaDAO = new OperatingQuotaDAO();
                return operatingQuotaDAO.CreateOperatingQuota(listOperatingQuota);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene el cupo operativo del tomador
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List OperatingQuota</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.OperatingQuota> GetOperatingQuotaByIndividualId(int individualId)
        {
            try
            {
                OperatingQuotaDAO operatingQuotaDAO = new OperatingQuotaDAO();
                return operatingQuotaDAO.GetOperatingQuotaByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el cupo operativo del tomador
        /// </summary>
        /// <param name="OperatingQuota">Cupo Operativo</param>
        /// <returns>
        /// List OperatingQuota
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.OperatingQuota UpdateOperatingQuota(Models.OperatingQuota OperatingQuota)
        {
            try
            {
                OperatingQuotaDAO operatingQuotaDAO = new OperatingQuotaDAO();
                return operatingQuotaDAO.UpdateOperatingQuota(OperatingQuota);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina el cupo operativo del tomador
        /// </summary>
        /// <param name="OperatingQuota"></param>
        /// <returns>Verdadero o Falso</returns>
        /// <exception cref="BusinessException"></exception>
        public bool DeleteOperatingQuota(Models.OperatingQuota OperatingQuota)
        {
            try
            {
                OperatingQuotaDAO operatingQuotaDAO = new OperatingQuotaDAO();
                return operatingQuotaDAO.DeleteOperatingQuota(OperatingQuota);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Prospecto Natural por Persona
        /// </summary>
        /// <param name="documentNum">document num</param>
        /// <param name="searchType"></param>
        /// <returns>List Models.ProspectNatural</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.ProspectNatural> GetProspectByDocumentNum(string documentNum, int searchType)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.GetProspectByDocumentNum(documentNum, searchType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar la informacion personal
        /// </summary>
        /// <param name="person"></param>
        /// <returns>Person</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Person UpdatePersonalInformation(Models.Person person)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.UpdatePersonalInformation(person);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// obtener el agente por individual id o Nombre
        /// </summary>
        /// <param name="IndividualId">The individual identifier.</param>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Agent> GetAgentByIndividualIdFullName(int IndividualId, string fullName)
        {
            try
            {
                AgentDAO agent = new AgentDAO();
                return agent.GetAgentByIndividualIdFullName(IndividualId, fullName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Prospectos Por Id, Número De Documento O Nombre
        /// </summary>
        /// <param name="description">Id, Número De Documento O Nombre</param>
        /// <param name="insuredSearchType">Tipo De Busqueda</param>
        /// <returns>Prospectos</returns>
        public List<Models.Prospect> GetProspectByDescription(string description, InsuredSearchType insuredSearchType)
        {
            try
            {
                ProspectDAO prospectDAO = new ProspectDAO();
                return prospectDAO.GetProspectByDescription(description, insuredSearchType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.Prospect> GetAdvancedProspectByDescription(string description)
        {
            try
            {
                ProspectDAO prospectDAO = new ProspectDAO();
                return prospectDAO.GetAdvancedProspectByDescription(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Direcciones de Notificación</returns>
        public List<Models.CompanyName> GetNotificationAddressesByIndividualId(int individualId, CustomerType customerType)
        {
            try
            {
                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
                return companyNameDAO.GetNotificationAddressesByIndividualId(individualId, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Agencias
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<Models.Agency> GetAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgenciesByAgentIdDescription(agentId, description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener Agencias habilitadas por ramo
        /// </summary>
        /// <param name="agentId">Id Agente</param>
        /// <param name="description">Código o Nombre</param>
        /// <param name="prefixId">Código o Nombre</param>
        /// <returns>Agencias</returns>
        public List<Models.Agency> GetAgenciesByAgentIdDescriptionIdPrefix(int agentId, string description, int prefixId)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgenciesByAgentIdDescriptionIdPrefix(agentId, description, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Tipo de contragarantias
        /// </summary>        
        public List<Models.GuaranteeType> GetGuaranteesTypes()
        {
            try
            {
                GuaranteeDAO guaranteeDAO = new GuaranteeDAO();
                return guaranteeDAO.GetGuaranteesTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de contragarantias
        /// </summary>        
        public List<Models.Guarantee> GetGuarantees()
        {
            try
            {
                GuaranteeDAO guaranteeDAO = new GuaranteeDAO();
                return guaranteeDAO.GetGuarantees();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>        
        public List<Models.ProviderType> GetProviderTypes()
        {
            try
            {
                ProviderDAO providerDAO = new ProviderDAO();
                return providerDAO.GetProviderTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de proveedores declinados
        /// </summary>        
        public List<Models.ProviderDeclinedType> GetProviderDeclinedType()
        {
            try
            {
                ProviderDAO providerDAO = new ProviderDAO();
                return providerDAO.GetProviderDeclinedType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipo de origen
        /// </summary>        
        public List<Models.OriginType> GetOriginTypes()
        {
            try
            {
                OriginDAO originDAO = new OriginDAO();
                return originDAO.GetOriginTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear proveedor
        /// </summary>        
        public Models.Provider CreateProvider(Models.Provider provider)
        {
            try
            {
                ProviderDAO providerDAO = new ProviderDAO();
                return providerDAO.CreateProvider(provider);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get proveedor
        /// </summary>        
        public Models.Provider GetProviderByIndividualId(int individualId)
        {
            try
            {
                ProviderDAO providerDAO = new ProviderDAO();
                return providerDAO.GetProviderByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar Proveedor
        /// </summary>        
        public Models.Provider UpdateProvider(Models.Provider provider)
        {
            try
            {
                ProviderDAO providerDAO = new ProviderDAO();
                return providerDAO.UpdateProvider(provider);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear impuestos asociados al individuo
        /// </summary>        
        public List<Models.IndividualTax> CreateIndivualTaxs(List<Models.IndividualTax> individualTax)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.CreateIndivualTaxs(individualTax);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<Models.IndividualTaxExeption> CreateIndivualExemptionTaxs(List<Models.IndividualTaxExeption> individualTax)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.CreateIndividualTaxExemption(individualTax);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Individual por individualId de persona
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.Individual GetIndividualByIndividualId(int individualId)
        {
            try
            {
                IndividualDAO DAO = new IndividualDAO();
                return DAO.GetIndividualByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Get Impuestos por individualId de persona
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<Models.IndividualTax> GetIndivualTaxsByIndividualId(int individualId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetIndivualTaxsByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Tipos de Empresa
        /// </summary>
        /// <returns>Tipos de Empresa</returns>
        public List<Models.CompanyType> GetCompanyTypes()
        {
            try
            {
                CompanyTypeDAO companyTypeDAO = new CompanyTypeDAO();
                return companyTypeDAO.GetCompanyTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener prospecto por Identificador
        /// </summary>
        /// <param name="id">Identificador</param>
        /// <returns>Prospecto</returns>
        public Models.Prospect GetProspectByProspectId(int prospectId)
        {
            try
            {
                ProspectDAO prospectDAO = new ProspectDAO();
                return prospectDAO.GetProspectByProspectId(prospectId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Creates the name of the companies.
        /// </summary>
        /// <param name="coCompanyName">Name of the co company.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyName CreateCompaniesName(Models.CompanyName coCompanyName, int individualId)
        {
            try
            {
                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
                return companyNameDAO.CreateCompaniesName(coCompanyName, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// guarda relación de individuos
        /// </summary>
        /// <param name="List<Models.IndividualRelationApp>">list of IndividualRelationApp</param>
        /// <returns></returns>
        public void SaveIndividualRelationApp(List<Models.IndividualRelationApp> individualsRelationApp)
        {
            try
            {
                IndividualRelationAppDAO dao = new IndividualRelationAppDAO();
                dao.SaveIndividualRelationApp(individualsRelationApp);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// elimina relación de individuos por userId
        /// </summary>
        /// <param name="parentIndividualId">parentIndividualId</param>
        /// <returns></returns>
        public void DeleteIndividualRelationAppByUserId(int parentIndividualId)
        {
            try
            {
                IndividualRelationAppDAO dao = new IndividualRelationAppDAO();
                dao.DeleteIndividualRelationAppByUserId(parentIndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener Prospecto Natural por Persona
        /// </summary>
        /// <param name="documentNum">document num</param>
        /// <param name="searchType"></param>
        /// <returns>List Models.ProspectNatural</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.ProspectNatural> GetProspectByIndividualId(string individualId)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.GetProspectByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Busqueda de person por codigo
        /// </summary>
        /// <param name="insuredCode"></param>
        /// <returns></returns>
        public Models.Insured GetInsuredByInsuredCode(int insuredCode)
        {
            try
            {
                InsuredDAO insuredDao = new InsuredDAO();
                return insuredDao.GetInsuredByInsuredCode(insuredCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        ///Obtener typo de agente por id
        /// </summary>
        /// <param name="id">Identificador de agente</param>
        /// <returns>AgentType</returns>
        public Models.AgentType GetAgentTypeById(int id)
        {
            try
            {
                AgentTypeDAO agentTypeDAO = new AgentTypeDAO();
                return agentTypeDAO.GetAgentTypeById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.IndividualPaymentMethod GetPaymentMethodByIndividualId(int individualId)
        {
            try
            {
                PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
                return paymentMethodDAO.GetPaymentMethodByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Recupera una agencia por el indice único código de agencia - tipo de agencia 
        /// </summary>
        /// <param name="agentCode"></param>
        /// <param name="agentTypeCode"></param>
        /// <returns></returns>
        public Models.Agency GetAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode)
        {
            try
            {
                AgencyDAO agencyDAO = new AgencyDAO();
                return agencyDAO.GetAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeCode);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGettingAgency));
            }
        }

        public ReInsurer GetReInsurerByIndividualId(int individualId)
        {
            try
            {
                ReinsurerDAO dao = new ReinsurerDAO();
                return dao.GetReInsurerByIndividualId(individualId);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorGettingReinsurer));
            }
        }

        public ReInsurer CreateReinsurer(ReInsurer reinsurer)
        {
            try
            {
                ReinsurerDAO dao = new ReinsurerDAO();
                return dao.CreateReinsurer(reinsurer);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreatingReinsurer), e);
            }
        }

        public IdentificationDocument GetIdentificationDocumentByInsuredId(int insuredId)
        {
            try
            {
                InsuredDAO dao = new InsuredDAO();
                return dao.GetIdentificationDocumentByInsuredId(insuredId);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.ToString());
            }
        }

        /// <summary>
        /// Normalizar Dirección
        /// </summary>
        /// <param name="address">Dirección</param>
        /// <returns>Dirección</returns>
        public string NormalizeAddress(string address)
        {
            try
            {
                NomenclatureDAO nomenclatureDAO = new NomenclatureDAO();
                return nomenclatureDAO.NormalizeAddress(address);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.ToString());
            }
        }

        public ModelIndividual.IndividualType GetIndividualTypeById(int individualTypeId)
        {
            try
            {
                IndividualTypeProvider individualTypeProvider = new IndividualTypeProvider();
                return individualTypeProvider.GetIndividualTypeById(individualTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetIndividualTypes), ex);
            }
        }

        public List<ModelIndividual.IndividualType> GetIndividualTypes()
        {
            try
            {
                IndividualTypeProvider individualTypeProvider = new IndividualTypeProvider();
                return individualTypeProvider.GetIndividualTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetIndividualTypes), ex);
            }
        }



        /// <summary>
        /// Obtener la lista de compañias Coaseguradoras
        /// </summary>
        /// <returns></returns>
        public List<Models.CoInsuranceCompany> GetCoInsuranceCompanies()
        {
            try
            {
                CoInsuranceCompanyProvider coInsuranceCompanyProvider = new CoInsuranceCompanyProvider();
                return coInsuranceCompanyProvider.GetCoInsuranceCompanies();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetListOfCompaniesCoinsurers), ex);
            }
        }




        public List<Models.ExonerationType> GetExonerationTypes()
        {
            try
            {
                Providers.ExonerationType exonerationTypeProvider = new Providers.ExonerationType();
                return exonerationTypeProvider.GetExonerationTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetExonerationTypes), ex);
            }
        }

        /// <summary>
        /// Obtener Coaseguradora
        /// </summary>
        /// <param name="userId">identificador de Coaseguradora</param>
        /// <returns></returns>
        public Models.CoInsuranceCompany GetCoInsuranceCompanyByCoinsuranceId(int coInsuranceId)
        {
            try
            {
                CoInsuranceCompanyProvider coInsuranceCompanyProvider = new CoInsuranceCompanyProvider();
                return coInsuranceCompanyProvider.GetCoInsuranceCompanyByCoinsuranceId(coInsuranceId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetCoInsurance), ex);
            }
        }


        /// Obtiene la lista de Perfiles de Asegurado.
        /// </summary>
        /// <returns>Lista de sucursales consultadas</returns>
        public List<Models.InsuredProfile> GetInsuredProfiles()
        {
            try
            {
                InsuredProfileDAO insuredProfileprovider = new InsuredProfileDAO();
                return insuredProfileprovider.GetInsuredProfiles();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Realiza los procesos del CRUD para los Perfiles de Asegurado.
        /// </summary>
        /// <param name="ListAdded"> Lista de insuredProfiles(perfiles de asergurado) para ser agregados</param>
        /// <param name="ListEdited">Lista de insuredProfiles(perfiles de asergurado) para ser modificados</param>
        /// <param name="ListDeleted">Lista de insuredProfiles(perfiles de asergurado) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public ParametrizationResponse<Models.InsuredProfile> CreateInsuredProfiles(List<Models.InsuredProfile> ListAdded, List<Models.InsuredProfile> ListEdited, List<Models.InsuredProfile> ListDeleted)
        {
            try
            {
                InsuredProfileDAO insuredProfile = new InsuredProfileDAO();
                return insuredProfile.SaveInsuredSegments(ListAdded, ListEdited, ListDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Genera archivo excel de Perfiles de Asegurado.
        /// </summary>
        /// <param name="insuredProfile"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToInsuredProfile(List<Models.InsuredProfile> insuredProfile, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToInsuredProfile(insuredProfile, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToInsuredProfile), ex);
            }
        }

        /// <summary>
        /// Obtiene la lista de Segmentos de Asegurado.
        /// </summary>
        /// <returns>Lista de Segmentos de Asegurado consultados</returns>
        public List<Models.InsuredSegment> GetInsuredSegments()
        {
            try
            {
                InsuredSegmentDAO insuredSegmentProvider = new InsuredSegmentDAO();
                return insuredSegmentProvider.GetInsuredSegments();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        /// <summary>
        /// Realiza los procesos del CRUD para los Segmentos de Asegurado.
        /// </summary>
        /// <param name="ListAdded"> Lista de insuredSegments(Segmentos de Asegurado) para ser agregados</param>
        /// <param name="ListEdited">Lista de insuredSegments(Segmentos de Asegurado) para ser modificados</param>
        /// <param name="ListDeleted">Lista de insuredSegments(Segmentos de Asegurado) para ser eliminados</param>
        /// <returns>El modelo de respuesta con el total de procesos realizados </returns>
        public ParametrizationResponse<Models.InsuredSegment> CreateInsuredSegments(List<Models.InsuredSegment> ListAdded, List<Models.InsuredSegment> ListEdited, List<Models.InsuredSegment> ListDeleted)
        {
            try
            {
                InsuredSegmentDAO insuredSegment = new InsuredSegmentDAO();
                return insuredSegment.SaveInsuredSegments(ListAdded, ListEdited, ListDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Genera archivo excel de Segmentos de Asegurado.
        /// </summary>
        /// <param name="insuredSegment"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string GenerateFileToInsuredSegment(List<Models.InsuredSegment> insuredSegment, string fileName)
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToInsuredSegment(insuredSegment, fileName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToInsuredSegment), ex);
            }
        }

        /// <summary>
        /// Obtener Comisiones por Id Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.CommissionAgent> GetAgentCommissionByIndividualId(int individualId)
        {
            try
            {
                CommissionAgentDao commissionAgents = new CommissionAgentDao();
                return commissionAgents.GetAgentCommissionByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Company.AddressType

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de dirección
        /// </summary>
        /// <param name="listAdded">tipos de dirección para agregar</param>
        /// <param name="listEdited">tipos de dirección para editar</param>
        /// <param name="listDeleted">tipos de direción para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<Models.CompanyAddressType> CreateCompanyAddressTypes(List<Models.CompanyAddressType> listAdded, List<Models.CompanyAddressType> listEdited, List<Models.CompanyAddressType> listDeleted)
        {
            try
            {
                CompanyAddressTypeDAO companyAddressType = new CompanyAddressTypeDAO();
                return companyAddressType.SaveCompanyAddressTypes(listAdded, listEdited, listDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForAddressTypes), ex);
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de dirección
        /// </summary>
        /// <returns>Tipos de dirección</returns>
        public List<Models.CompanyAddressType> GetAllCompanyAddressType()
        {
            try
            {
                CompanyAddressTypeDAO companyAddressTypeDAO = new CompanyAddressTypeDAO();
                return companyAddressTypeDAO.GetAllCompanyAddressType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Verifica si el tipo de dirección esta relacionado en otras tablas
        /// </summary>
        /// <param name="addressTypeCode">key del tipo de dirección</param>
        /// <returns>Resultado de la validación</returns>
        public bool ValidateForeingCompanyAddressType(int addressTypeCode)
        {
            try
            {
                CompanyAddressTypeDAO companyAddressTypeDAO = new CompanyAddressTypeDAO();
                return companyAddressTypeDAO.ValidateForeingCompanyAddressType(addressTypeCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Genera archivo excel para tipo de dirección
        /// </summary>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToAddressType()
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToAddressType(Errors.FileAddressTypeName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToAddressType), ex);
            }
        }
        #endregion

        #region Company.PhoneType

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de teléfono
        /// </summary>
        /// <param name="listAdded">tipos de teléfono para agregar</param>
        /// <param name="listEdited">tipos de teléfono para editar</param>
        /// <param name="listDeleted">tipos de teléfono para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<Models.CompanyPhoneType> CreateCompanyPhoneTypes(List<Models.CompanyPhoneType> listAdded, List<Models.CompanyPhoneType> listEdited, List<Models.CompanyPhoneType> listDeleted)
        {
            try
            {
                CompanyPhoneTypeDAO companyPhoneType = new CompanyPhoneTypeDAO();
                return companyPhoneType.SaveCompanyPhoneTypes(listAdded, listEdited, listDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCRUDProcessesForPhoneTypes), ex);
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de teléfono
        /// </summary>
        /// <returns>Tipos de teléfono</returns>
        public List<Models.CompanyPhoneType> GetAllCompanyPhoneType()
        {
            try
            {
                CompanyPhoneTypeDAO companyPhoneTypeDAO = new CompanyPhoneTypeDAO();
                return companyPhoneTypeDAO.GetAllCompanyPhoneType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Verifica si el tipo de teléfono esta relacionado en otras tablas
        /// </summary>
        /// <param name="phoneTypeCode">key del tipo de teléfono</param>
        /// <returns>Resultado de la validación</returns>
        public bool ValidateForeingCompanyPhoneType(int phoneTypeCode)
        {
            try
            {
                CompanyPhoneTypeDAO companyPhoneTypeDAO = new CompanyPhoneTypeDAO();
                return companyPhoneTypeDAO.ValidateForeingCompanyPhoneType(phoneTypeCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Genera archivo excel para tipo de teléfono
        /// </summary>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToPhoneType()
        {
            try
            {
                FileDAO fileDAO = new FileDAO();
                return fileDAO.GenerateFileToPhoneType(Errors.FilePhoneTypeName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToPhoneType), ex);
            }
        }
        #endregion


        /// <summary>
        /// Obtener lista de Comisiones por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.CommissionAgent> GetAgentCommissionByAgentId(int agentId)
        {
            try
            {
                CommissionAgentDao ocmmissionAgent = new CommissionAgentDao();
                return ocmmissionAgent.GetAgentCommissionByAgentId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina datos de la Comision del agente
        /// </summary>
        /// <param name="commissionAgent"></param>
        /// <returns>Verdadero o Falso</returns>
        /// <exception cref="BusinessException"></exception>
        public bool DeleteAgentCommission(CommissionAgent commissionAgent)
        {
            try
            {
                CommissionAgentDao agentCommissionDAO = new CommissionAgentDao();
                return agentCommissionDAO.DeleteAgentCommission(commissionAgent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de actividades economicas
        /// </summary>
        /// <returns></returns>
        public List<Models.EconomicActivity> GetEconomicActivities()
        {
            try
            {
                DAOs.EconomicActivityDAO economicActivityProvider = new DAOs.EconomicActivityDAO();
                return economicActivityProvider.GetEconomicActivities();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetListOfEconomicActivities), ex);
            }
        }

        public Models.EconomicActivity GetEconomicActivitiesByEconomicActiviti(int EconomicActiviti)
        {
            try
            {
                EconomicActivityDAO EconomicActivityProvider = new EconomicActivityDAO();
                return EconomicActivityProvider.GetEconomicActivitiesByEconomicActiviti(EconomicActiviti);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetEconomicActivitiesByEconomicActivity), ex);
            }
        }

        /// <summary>
        /// Consulta listados de tipo de bien
        /// </summary>
        /// <returns> Listado de tipos de bien </returns>
        public List<AssetType> GetAssetType()
        {
            try
            {
                AssetTypeDAO assetTypeDao = new AssetTypeDAO();

                return assetTypeDao.GetAssetType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAssetType), ex);
            }
        }

        /// <summary>
        /// Lista de los grupos al cual pertenece el intermediario.
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Base.BaseGroupAgent> GetGroupAgent()
        {

            try
            {
                AgentGroupDAO agentGroupDAO = new AgentGroupDAO();
                return agentGroupDAO.GetGroupAgent();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista de los canales al cual pertenece el intermediario.
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Base.BaseSalesChannel> GetSalesChannel()
        {

            try
            {
                SalesChannelDAO agentSalesChannelDAO = new SalesChannelDAO();
                return agentSalesChannelDAO.GetSalesChannel();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Lista de Empleados
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Base.BaseEmployeePerson> GetEmployeePersons()
        {
            try
            {
                EmployeeDAO EmployeeProvider = new EmployeeDAO();
                return EmployeeProvider.GetEmployeePersons();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetEconomicActivitiesByEconomicActivity), ex);
            }
        }

        #region "Insured"

        public List<Models.Base.BaseInsuredSegment> GetInsuredSegment()
        {
            InsuredDAO insuredDAO = new InsuredDAO();
            return insuredDAO.GetInsuredSegment();
        }

        public List<Models.Base.BaseInsuredProfile> GetInsuredProfile()
        {
            InsuredDAO insuredDAO = new InsuredDAO();
            return insuredDAO.GetInsuredProfile();
        }

        public List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter)
        {
            InsuredDAO insuredDAO = new InsuredDAO();
            return insuredDAO.GetInsuredsByName(stringFilter);
        }

        #endregion

        #region "PersonInterestGroup"

        public List<Models.InterestGroupsType> GetInterestGroupTypes()
        {
            PersonInterestGroupDAO personInterestGroupDAO = new PersonInterestGroupDAO();
            return personInterestGroupDAO.GetInterestGroupTypes();
        }

        public List<Models.PersonInterestGroup> GetPersonInterestGroups(int individualId)
        {
            PersonInterestGroupDAO personInterestGroupDAO = new PersonInterestGroupDAO();
            return personInterestGroupDAO.GetPersonInterestGroups(individualId);
        }

        public Models.PersonInterestGroup CreatePersonInterestGroup(Models.PersonInterestGroup personInterestGroup)
        {
            PersonInterestGroupDAO personInterestGroupDAO = new PersonInterestGroupDAO();
            return personInterestGroupDAO.CreatePersonInterestGroup(personInterestGroup);
        }

        public Models.PersonInterestGroup DeletePersonInterestGroup(Models.PersonInterestGroup personInterestGroup)
        {
            PersonInterestGroupDAO personInterestGroupDAO = new PersonInterestGroupDAO();
            return personInterestGroupDAO.DeletePersonInterestGroup(personInterestGroup);
        }

        #endregion
        #region datos Poliza
        /// <summary>
        /// Gets the agents by individual ids.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Datos Vacios</exception>
        public async Task<List<Models.Agent>> GetAgentsByIndividualIds(List<int> individualId)
        {
            if (individualId == null)
            {
                throw new ArgumentException(Errors.ErrorParameterEmpty);
            }
            AgentDAO agents = new AgentDAO();
            return await agents.GetAgentsByIndividualIds(individualId);
        }

        /// <summary>
        /// Gets the agents by individual ids by agency identifier.
        /// </summary>
        /// <param name="IndividualIds">The individual ids.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Datos Vacios</exception>
        public async Task<List<Models.Agent>> GetAgentsByIndividualIdsByAgencyId(List<PersonBase.BaseAgentAgency> baseAgentAgency, Int16 prefixId = -1)
        {
            if (baseAgentAgency == null)
            {
                throw new ArgumentException(Errors.ErrorParameterEmpty);
            }
            AgentDAO agents = new AgentDAO();
            return await agents.GetAgentsByIndividualIdsByAgencyId(baseAgentAgency, prefixId);
        }
        #endregion

        /// <summary>
        /// Obtiene los agentes por ramo
        /// </summary>
        /// <param name="prefixId">id ramo</param>
        /// <returns>Lista de agentes por ramo</returns>
        public List<Models.Agent> GetAgentsByPrefix(int prefixId)
        {
            try
            {
                AgentDAO agents = new AgentDAO();
                return agents.GetAgentsByPrefix(prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region "UpdateAddressEmail"

        public bool UpdateAddressEmail(Models.Email email, int individualId, string user)
        {
            bool result;
            try
            {
                AddressDAO addressDAO = new AddressDAO();
                Models.Address address = addressDAO.GetAddresses(individualId).First(x => x.Description == email.Description);
                address.UpdateUser = user;
                address.UpdateDate = DateTime.Now.ToShortDateString();
                addressDAO.UpdateAddress(address, individualId);

                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        #endregion
        #region holder
        /// <summary>
        /// Gets the type of the i holders by description insured search type customer.
        /// </summary>
        /// <param name="description">The description.</param>
        /// <param name="insuredSearchType">Type of the insured search.</param>
        /// <param name="customerType">Type of the customer.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Insured> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            try
            {
                InsuredDAO insuredDAO = new InsuredDAO();
                return insuredDAO.GetHoldersByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.GetBaseException().Message, ex);
            }
        }
        #endregion


        #region DocumentTypeRange
        public DocumentTypeRange CreateDocumentTypeRange(DocumentTypeRange DocumentTypeRange)
        {
            try
            {
                DocumentTypeRangeDAO DocumentTypeRangeDAO = new DocumentTypeRangeDAO();
                return DocumentTypeRangeDAO.CreateDocumentTypeRange(DocumentTypeRange);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public DocumentTypeRange UpdateDocumentTypeRange(DocumentTypeRange DocumentTypeRange)
        {
            try
            {
                DocumentTypeRangeDAO DocumentTypeRangeDAO = new DocumentTypeRangeDAO();
                return DocumentTypeRangeDAO.UpdateDocumentTypeRange(DocumentTypeRange);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteDocumentTypeRange(int DocumentTypeRangeId)
        {
            try
            {
                DocumentTypeRangeDAO DocumentTypeRangeDAO = new DocumentTypeRangeDAO();
                DocumentTypeRangeDAO.DeleteDocumentTypeRange(DocumentTypeRangeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<DocumentTypeRange> GetDocumentsTypeRangeId(int DocumentTypeRangeId)
        {
            try
            {
                DocumentTypeRangeDAO DocumentTypeRangeDAO = new DocumentTypeRangeDAO();
                return DocumentTypeRangeDAO.GetDocumentsTypeRangeId(DocumentTypeRangeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<DocumentTypeRange> GetDocumentTypeRange()
        {
            try
            {
                DocumentTypeRangeDAO DocumentTypeRangeDAO = new DocumentTypeRangeDAO();
                return DocumentTypeRangeDAO.GetDocumentTypeRange();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion


    }
}