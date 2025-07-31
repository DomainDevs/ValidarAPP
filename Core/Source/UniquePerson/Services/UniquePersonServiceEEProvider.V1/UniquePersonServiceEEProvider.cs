using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Providers;
using Sistran.Core.Application.UniquePersonService.V1.Resources;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using ModelIndividual = Sistran.Core.Application.UniquePersonService.V1Individual.Models;
using PersonBase = Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.EEProvider.DAOs;
using Sistran.Core.Application.CommonService.Models.Base;
using Sistran.Core.Application.UniquePersonService.V1.Business;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Application.UniquePersonService.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.DAOsGetCompanyInsuredsByName;
using Sistran.Core.Services.UtilitiesServices.Models;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonServiceEEProvider : IUniquePersonServiceCore
    {
        #region Person
        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Models.Person CreatePerson(Models.Person person)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.CreatePerson(person);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.Address> CreateAddresses(int individualId, List<Models.Address> addresses)
        {
            var addressBusiness = new AddressBusiness();
            return addressBusiness.CreateAddresses(individualId, addresses);
        }

        public List<Models.Address> UpdateAddresses(int individualId, List<Models.Address> addresses)
        {
            var addressBusiness = new AddressBusiness();
            return addressBusiness.UpdateAddresses(individualId, addresses);
        }

        public List<Models.Phone> CreatePhones(int individualId, List<Models.Phone> addresses)
        {
            var phoneBusiness = new PhoneBusiness();
            return phoneBusiness.CreatePhones(individualId, addresses);
        }

        public List<Models.Phone> UpdatePhones(int individualId, List<Models.Phone> addresses)
        {
            var phoneBusiness = new PhoneBusiness();
            return phoneBusiness.UpdatePhones(individualId, addresses);
        }

        public List<Models.Email> CreateEmails(int individualId, List<Models.Email> addresses)
        {
            var emailBusiness = new EmailBusiness();
            return emailBusiness.CreateEmails(individualId, addresses);
        }

        public List<Models.Email> UpdateEmails(int individualId, List<Models.Email> addresses)
        {
            var emailBusiness = new EmailBusiness();
            return emailBusiness.UpdateEmails(individualId, addresses);
        }

        public Models.Person GetPersonByIndividualId(int individualId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.GetPersonByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.Person> GetPersonByDocument(CustomerType customerType, string documentNumber)
        {
            if (customerType == CustomerType.Individual)
            {
                var personBusiness = new PersonBusiness();
                return personBusiness.GetPersonByDocument(documentNumber);
            }
            else
            {
                var prospectBusiness = new ProspectBusiness();
                return prospectBusiness.GetPersonByDocument(documentNumber);
            }
        }

        public List<Models.Person> GetPersonAdv(CustomerType customerType, Models.Person person)
        {
            if (customerType == CustomerType.Individual)
            {
                var personBusiness = new PersonBusiness();
                return personBusiness.GetPersonAdv(person);
            }
            else
            {
                var prospectBusiness = new ProspectBusiness();
                return prospectBusiness.GetPersonAdv(person);
            }
        }

        public List<Models.Address> GetAddresses(int individualId)
        {
            var addressBusiness = new AddressBusiness();
            return addressBusiness.GetAddresses(individualId);
        }

        public List<Models.Phone> GetPhones(int individualId)
        {
            var PhoneBusiness = new PhoneBusiness();
            return PhoneBusiness.GetPhones(individualId);
        }

        public List<Models.Email> GetEmails(int individualId)
        {
            var emailBusiness = new EmailBusiness();
            return emailBusiness.GetEmails(individualId);
        }

        public Models.EconomicActivity GetEconomicActivitiesById(int id)
        {
            var economyActivityBusiness = new EconomyActivityBusiness();
            return economyActivityBusiness.GetEconomicActivitiesById(id);
        }

        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns
        public Models.LabourPerson GetPersonJobByIndividualId(int individualId)
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

        public virtual Models.Person UpdatePersonBasicInfo(Models.Person person)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.UpdatePersonBasicInfo(person);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Person

        #region reinsurer
        public ReInsurer GetReInsurerByIndividualId(int individualId)
        {
            try
            {
                ReInsurerBusiness business = new ReInsurerBusiness();
                return business.GetReInsurerByIndividualId(individualId);
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
                ReInsurerBusiness business = new ReInsurerBusiness();
                return business.CreateReinsurer(reinsurer);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreatingReinsurer), e);
            }
        }

        public ReInsurer UpdateReinsurer(ReInsurer reinsurer)
        {
            try
            {
                ReInsurerBusiness reInsurerBusiness = new ReInsurerBusiness();
                return reInsurerBusiness.UpdateReInsurer(reinsurer);
            }
            catch (Exception e)
            {
                throw new BusinessException(ExceptionManager.GetMessage(e, Errors.ErrorCreatingReinsurer), e);
            }
        }
        #endregion reinsurer

        #region Partner
        public Partner GetPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int individualId)
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                return partnerBusiness.GetPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Partner> GetPartnerByIndividualId(int individualId)
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                return partnerBusiness.GetPartnerByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Partner CreatePartner(Partner partner)
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                IndividualPartner IndParNer = EntityAssembler.IndividualPartnerFields(partner);
                return partnerBusiness.CreatePartner(partner);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Partner UpdatePartner(Partner partner)
        {
            try
            {
                PartnerBusiness partnerBusiness = new PartnerBusiness();
                IndividualPartner IndParNer = EntityAssembler.IndividualPartnerFields(partner);
                return partnerBusiness.UpdatePartner(partner);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Partner

        #region Agent V1

        #region Agent
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
                AgentBusiness agentBussines = new AgentBusiness();
                return agentBussines.GetAgentByIndividualId(individualId);
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
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.CreateAgent(Agent);
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
        public Models.Agent UpdateAgent(Models.Agent Agent)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.UpdateAgent(Agent);
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
                AgencyBusiness agency = new AgencyBusiness();
                return agency.GetAgenciesByAgentIdDescription(agentId, description);
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
                AgencyBusiness agency = new AgencyBusiness();
                return agency.GetAgenciesByAgentId(agentId);
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


        #endregion Agent

        #region AgencyByInvidualId
        /// <summary>
        /// Obtiene Agencias Por InvidialID
        /// </summary>
        /// <param name="individualId">Cod Del la persona</param>
        /// <returns></returns>
        public List<Models.Agency> GetAgencyByInvidualId(int individualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.GetAgencyByInvidualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        public List<Models.Agency> GetActiveAgencyByInvidualId(int individualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.GetActiveAgenciesByInvidualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        /// <summary>
        /// Crea Agencias Por InvidualId
        /// </summary>
        /// <param name="agencies">model de agencias</param>
        /// <param name="individualId">Codigo de InvidualId</param>
        /// <returns></returns>
        public Models.Agency CreateAgencyByInvidualId(Models.Agency agencies, int individualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.CreateCompanyAgency(agencies, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        /// <summary>
        /// Actualiza Agencias Por InvidualId
        /// </summary>
        /// <param name="agencies">model de agencias</param>
        /// <param name="individualId">Codigo de InvidualId</param>
        /// <returns></returns
        public Models.Agency UpdateAgencyByInvidualId(Models.Agency agencies, int individualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.UpdateCompanyAgency(agencies, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        #endregion

        #region AgentPrefix
        public List<BasePrefix> GetPrefixesByAgentId(int agentId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.GetPrefixesByAgentId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public BasePrefix CreatePrefixesByAgentId(BasePrefix basePrefix, int IndivualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.CreateAgentPrefix(basePrefix, IndivualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public BasePrefix UpdatePrefixesByAgentId(BasePrefix basePrefix, int IndivualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.UpdateteAgentPrefix(basePrefix, IndivualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public BasePrefix DeletePrefixesByAgentId(BasePrefix basePrefix, int IndivualId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.deleteAgentPrefix(basePrefix, IndivualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Commisionagent
        public List<Models.Commission> GetCommissionInvidualId(int agentId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.GetAgentCommissionByIndividualId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.Commission CreateCommissionInvidualId(Models.Commission commission, int InvidualId, int AgencyId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.CreateAgentCommission(commission, InvidualId, AgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.Commission UpdateCommissionInvidualId(Models.Commission commission, int InvidualId, int AgencyId)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.UpdateAgentCommission(commission, InvidualId, AgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public bool DeleteCommissionInvidualId(Models.Commission commission)
        {
            try
            {
                AgentBusiness agentBusiness = new AgentBusiness();
                return agentBusiness.DeleteAgentCommission(commission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion
        #endregion Agent V1

        #region IndividualTax
        /// <summary>
        /// Obtiene el impuesto individual por lista
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<Models.IndividualTax> GetIndividualTaxByIndividualId(int individualId)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.GetIndivualTaxsByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear un impuesto
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public Models.IndividualTax CreateIndividualTax(Models.IndividualTax individualTax)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.CreateIndividualTax(individualTax);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        /// <summary>
        /// crea un mpuesto individual
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public Models.IndividualTaxExeption CreateIndividualTaxExeption(Models.IndividualTaxExeption individualTaxExeption)
        {
            try
            {
                TaxBusiness taxBusiness = new TaxBusiness();
                return taxBusiness.CreateIndividualTaxExeption(individualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Actualiza los datos del impuesto individual
        /// </summary>
        /// <param name="IndividualTaxExeption"></param>
        /// <returns></returns>
        public Models.IndividualTaxExeption UpdateIndividualTaxExeption(Models.IndividualTaxExeption IndividualTaxExeption)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.UpdateIndividualTaxExemption(IndividualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza los datos del impuesto individual
        /// </summary>
        /// <param name="IndividualTaxExeption"></param>
        /// <returns></returns>
        public Models.IndividualTax UpdateIndividualTax(Models.IndividualTax IndividualTaxExeption)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.UpdateIndividualTax(IndividualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Elimina los datos del impuesto individual
        /// </summary>
        /// <param name="IndividualTaxExeption"></param>
        public void DeleteIndividualTaxExeption(Models.IndividualTaxExeption IndividualTaxExeption)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                taxDAO.DeleteIndividualTaxExemption(IndividualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteIndividualTax(Models.IndividualTax IndividualTaxExeption)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                taxDAO.DeleteIndividualTax(IndividualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion IndividulTax

        #region LabourPerson
        /// <summary>
        /// Guardar la informacion laboral de la persona
        /// </summary>
        /// <param name="personJob">Moledo PersonJob</param>
        /// <returns></returns
        public LabourPerson CreateLabourPerson(LabourPerson personJob, int individualId)
        {
            try
            {
                LabourPersonBusiness LabourPersonBusiness = new LabourPersonBusiness();
                return LabourPersonBusiness.CreateLabourPerson(personJob, individualId);
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
        public LabourPerson UpdateLabourPerson(LabourPerson personJob)
        {
            try
            {
                LabourPersonBusiness LabourPersonBusiness = new LabourPersonBusiness();
                return LabourPersonBusiness.UpdateLabourPerson(personJob);

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
        public LabourPerson GetLabourPersonByIndividualId(int individualId)
        {
            try
            {
                LabourPersonBusiness LabourPersonBusiness = new LabourPersonBusiness();
                return LabourPersonBusiness.GetLabourPersonByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion LabourPerson

        #region OperatingQuota V1

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
                OperatingQuotaBusiness operatingQuotaBusiness = new OperatingQuotaBusiness();
                return operatingQuotaBusiness.GetOperatingQuotaByIndividualId(individualId);
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
                OperatingQuotaBusiness operatingQuotaBusiness = new OperatingQuotaBusiness();
                return operatingQuotaBusiness.CreateOperatingQuota(listOperatingQuota);
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
                OperatingQuotaBusiness operatingQuotaBusiness = new OperatingQuotaBusiness();
                return operatingQuotaBusiness.UpdateOperatingQuota(OperatingQuota);
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
                OperatingQuotaBusiness operatingQuotaBusiness = new OperatingQuotaBusiness();
                if (operatingQuotaBusiness.DeleteOperatingQuota(OperatingQuota))
                {
                    return operatingQuotaBusiness.DeleteOperatingQuotaEvent(OperatingQuota.IndividualId, OperatingQuota.LineBusinessId);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion OperatingQuota V1

        #region IndividualRole V1

        public List<Models.SupplierAccountingConcept> GetSupplierAccountingConceptsBySupplierId(int SupplierId)
        {

            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierAccountingConceptsBySupplierId(SupplierId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.AccountingConcept> GetAccountingConcepts()
        {

            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetAccountingConcepts();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.SupplierProfile> GetSupplierProfiles()
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierProfiles();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.SupplierProfile> GetSupplierTypeProfileById(int suppilierTypeId)
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierTypeProfileById(suppilierTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene los roles de un individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List IndividualRole</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.IndividualRole> GetIndividualRoleByIndividualId(int individualId)
        {
            try
            {
                IndividualRoleBusiness individualRoleBusiness = new IndividualRoleBusiness();
                return individualRoleBusiness.GetIndividualRoleByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion IndividualRole V1

        #region Supplier v1

        public Models.Supplier GetSupplierById(int SupplierId)
        {

            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierById(SupplierId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }


        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>        
        public List<Models.Supplier> GetSuppliers()
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSuppliers();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipo proveedores
        /// </summary>        
        public List<Models.SupplierType> GetSupplierTypes()
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de proveedores declinados
        /// </summary>        
        public List<Models.SupplierDeclinedType> GetSupplierDeclinedTypes()
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener grupo de proveedores 
        /// </summary>        
        public List<Models.GroupSupplier> GetGroupSupplier()
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetGroupSupplier();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear proveedor
        /// </summary>        
        public Models.Supplier CreateSupplier(Models.Supplier provider)
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.CreateSupplier(provider);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar Proveedor
        /// </summary>        
        public Models.Supplier UpdateSupplier(Models.Supplier provider)
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.UpdateSupplier(provider);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Get proveedor
        /// </summary>        
        public Models.Supplier GetSupplierByIndividualId(int individualId)
        {
            try
            {
                SupplierBusiness supplierBusiness = new SupplierBusiness();
                return supplierBusiness.GetSupplierByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Supplier v1

        #region MaritalStatus 
        /// <summary>
        /// Obtiene Estado Civil
        /// </summary>
        /// <returns>Models.MaritalStatus</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.MaritalStatus> GetMaritalStatus()
        {
            try
            {
                MaritalStatusBusiness maritalStatusBusiness = new MaritalStatusBusiness();
                return maritalStatusBusiness.GetMaritalStatus();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Marital Status

        #region DocumentType
        /// <summary>
        /// Obtener los tipos de documentos
        /// </summary>
        /// <param name="typeDocument">tipo de documento
        /// 1. persona natural
        /// 2. persona juridica
        /// 3. todos</param>
        /// <returns></returns>
        public List<Models.DocumentType> GetDocumentTypes(int typeDocument)
        {
            try
            {
                DocumentTypeBusiness documentTypeBusiness = new DocumentTypeBusiness();
                return documentTypeBusiness.GetDocumentTypes(typeDocument);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region AddressType
        /// <summary>
        /// Obtener tipo de direcciones
        /// </summary>
        /// <returns>Lista tipo de direcciones </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AddressType> GetAddressesTypes()
        {
            try
            {
                AddressTypeBusiness addressTypeBusiness = new AddressTypeBusiness();
                return addressTypeBusiness.GetAddressesTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Document Types

        #region PhoneTypes
        /// <summary>
        /// Obtener lista de tipos de teléfono
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException">Error Obteniendo GetPhoneTypes</exception>
        public List<Models.PhoneType> GetPhoneTypes()
        {
            try
            {
                PhoneBusiness phoneBusiness = new PhoneBusiness();
                return phoneBusiness.GetPhoneTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EmailTypes
        /// <summary>
        /// Obtener lista de tipos de email
        /// </summary>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.EmailType> GetEmailTypes()
        {
            try
            {
                EmailBusiness emailBusiness = new EmailBusiness();
                return emailBusiness.GetEmailTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EconomicActivities
        /// <summary>
        /// Obtener lista de actividades economicas
        /// </summary>
        /// <returns></returns>
        public List<Models.EconomicActivity> GetEconomicActivities()
        {
            try
            {
                EconomyActivityBusiness economyActivity = new EconomyActivityBusiness();
                return economyActivity.GetEconomicActivities();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorGetListOfEconomicActivities), ex);
            }
        }
        #endregion

        #region AssociationType

        /// <summary>
        /// Obtener lista de tipos de asociación
        /// </summary>
        /// <returns>Models.CoAssociationType</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AssociationType> GetAssociationTypes()
        {
            try
            {
                AssociationTypeBusiness associationType = new AssociationTypeBusiness();
                return associationType.GetAssociationTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion AssociationType

        #region CompanyType
        /// <summary>
        /// Obtener Tipos de Empresa
        /// </summary>
        /// <returns>Tipos de Empresa</returns>
        public List<Models.CompanyType> GetCompanyTypes()
        {
            try
            {
                CompanyTypeBusiness companyType = new CompanyTypeBusiness();
                return companyType.GetCompanyTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region FiscalResponsibility
        /// <summary>
        /// Obtener Los Tipos de responsabilidad fiscal
        /// </summary>
        /// <returns>Tipos de Empresa</returns>
        public List<Models.FiscalResponsibility> GetFiscalResponsibility()
        {
            try
            {
                FiscalResponsibilityBusiness responsabilities = new FiscalResponsibilityBusiness();
                return responsabilities.GetFiscalResponsibility();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion



        #region ThirdPerson

        public List<ThirdPerson> GetThirdByDescriptionInsuredSearchType(string description, InsuredSearchType insuredSearchType)
        {
            try
            {
                ThirdPartyBusiness thirdBusiness = new ThirdPartyBusiness();
                return thirdBusiness.GetThirdByDescriptionInsuredSearchType(description, insuredSearchType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }                

        #endregion

        #region InsuredDeclinedType
        /// <summary>
        /// Obtener el listado de los motivos de baja del asegurado
        /// </summary>
        /// <returns>Models.InsuredDeclinedType</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.InsuredDeclinedType> GetInsuredDeclinedTypes()
        {
            try
            {
                InsuredDeclinedTypeBusiness insuredDeclinedType = new InsuredDeclinedTypeBusiness();
                return insuredDeclinedType.GetInsuredDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region AgentType
        /// <summary>
        /// Obtener Los Tipos de Agente
        /// </summary>
        /// <returns>List<Models.AgentType></returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AgentType> GetAgentTypes()
        {
            try
            {
                AgentTypeBusiness agentType = new AgentTypeBusiness();
                return agentType.GetAgentTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region AgentDeclinedType
        /// <summary>
        /// Lista de los Motivos de baja para los agentes
        /// </summary>
        /// <returns>Lista AgentDeclinedType</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AgentDeclinedType> GetAgentDeclinedTypes()
        {
            try
            {
                AgentDeclinedTypeBusiness agentDeclinedType = new AgentDeclinedTypeBusiness();
                return agentDeclinedType.GetAgentDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region GroupAgent
        /// <summary>
        /// Lista de los grupos al cual pertenece el intermediario.
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.GroupAgent> GetGroupAgent()
        {
            try
            {
                AgentGroupBusiness agentGroup = new AgentGroupBusiness();
                return agentGroup.GetGroupAgent();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region SalesChannel
        /// <summary>
        /// Lista de los canales al cual pertenece el intermediario.
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.SalesChannel> GetSalesChannel()
        {
            try
            {
                SalesChannelBusiness agentSalesChannel = new SalesChannelBusiness();
                return agentSalesChannel.GetSalesChannel();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EmployeePerson
        /// <summary>
        /// Lista de Empleados
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.EmployeePerson> GetEmployeePersons()
        {
            try
            {
                EmployeePersonBusiness employeePerson = new EmployeePersonBusiness();
                return employeePerson.GetEmployeePersons();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region OthersDeclinedType
        /// <summary>
        /// Lista de Empleados
        /// </summary>
        /// <exception cref="BusinessException"></exception>
        public List<Models.AllOthersDeclinedType> GetAllOthersDeclinedTypes()
        {
            try
            {
                OthersDeclinedTypeBusiness othersDeclinedType = new OthersDeclinedTypeBusiness();
                return othersDeclinedType.GetAllOthersDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

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
        /// Crear asociado
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Actualizar asociado
        /// </summary>
        /// <param name="partner"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.Partner UpdatePartner(Partner partner, int individualId)
        {
            try
            {
                PartnerDAO partnerDAO = new PartnerDAO();
                return partnerDAO.UpdatePartner(partner, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar Razon Social
        /// </summary>
        /// <param name="copyName"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.CompanyName UpdateCompanyName(CompanyName copyName, int individualId)
        {
            try
            {
                CompanyNameDAO CompanyNameDao = new CompanyNameDAO();
                return CompanyNameDao.UpdateCompaniesName(copyName, individualId);
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
        /// Actualizar los los datos de un persona
        /// </summary>
        /// <param name="person">Modelo person</param>
        /// <param name="personIndividualType"></param>
        /// <returns></returns>
        public virtual Models.Person UpdatePerson(Models.Person person)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.UpdatePerson(person);
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
        //public Models.LaborPerson CreatePersonJob(Models.LaborPerson personJob, int individualId)
        //{
        //    try
        //    {
        //        PersonJobDAO personJobDAO = new PersonJobDAO();
        //        return personJobDAO.CreatePersonJob(personJob, individualId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Actualizar los datos laborales de la persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns
        //public Models.LaborPerson UpdatePersonJob(Models.LaborPerson personJob)
        //{
        //    try
        //    {
        //        PersonJobDAO personjobModel = new PersonJobDAO();
        //        return personjobModel.UpdatePersonJob(personJob);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns
        //public Models.LaborPerson GetPersonJobByIndividualId(int individualId)
        //{
        //    try
        //    {
        //        PersonJobDAO personJobDAO = new PersonJobDAO();
        //        return personJobDAO.GetPersonJobByIndividualId(individualId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new BusinessException(ex.Message, ex);
        //    }
        //}

        /// <summary>
        /// Buscar persona por número de documento o apellidos y nombres
        /// </summary>
        /// <param name="documentNumber">número de documento</param>
        /// <param name="surname">primer apellido</param>
        /// <param name="motherLastName">segundo apellido</param>
        /// <param name="name">nombres</param>
        /// <param name="searchType">tipo de busqueda</param>
        /// <returns></returns>
        public virtual List<Models.Person> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.GetPersonByDocumentNumberSurnameMotherLastName(documentNumber, surname, motherLastName, name, searchType);
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
        public List<Models.PaymentAccount> GetPaymentMethodAccountByIndividualId(int individualId)
        {
            try
            {
                //PaymentMethodAccountDAO paymentMethodaccountDAO = new PaymentMethodAccountDAO();
                //return paymentMethodaccountDAO.GetPaymentMethodAccountByIndividualId(individualId);
                return null;
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
        virtual public List<Models.Company> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType)
        {
            try
            {
                CompanyDAO company = new CompanyDAO();
                return company.GetCompaniesByDocumentNumberNameSearchType(documentNumber, name, searchType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #region Insured V1
        /// <summary>
        /// Crea Asegurado
        /// </summary>
        /// <param name="insured"></param>
        /// <returns></returns>
        public Models.Insured CreateInsured(Models.Insured insured)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.CreateInsured(insured);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Busca el asegurado por IndividualId
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns></returns>
        public Models.Insured GetInsuredByIndividualId(int individualId)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.GetInsuredByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        

        /// <summary>
        /// Busca el asegurado por IndividualId para facturacion electronica
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <returns></returns>
        public Models.Insured GetInsuredElectronicBillingByIndividualId(int individualId)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.GetInsuredElectronicBillingByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el asegurado
        /// </summary>
        /// <param name="insured"></param>
        /// <returns></returns>
        public Models.Insured UpdateInsured(Models.Insured insured)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.UpdateInsured(insured);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        

        /// <summary>
        /// Actualiza el asegurado para facturacion electronica
        /// </summary>
        /// <param name="insured"></param>
        /// <returns></returns>
        public Models.Insured UpdateInsuredElectronicBilling(Models.Insured insured)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.UpdateInsuredElectronicBilling(insured);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea conceptos de asegurado
        /// </summary>
        /// <param name="insuredConcept"></param>
        /// <returns></returns>
        public Models.InsuredConcept CreateInsuredConcept(Models.InsuredConcept insuredConcept)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.CreateInsuredConcept(insuredConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.InsuredConcept UpdateInsuredConcept(Models.InsuredConcept insuredConcept)
        {
            try
            {
                InsuredBusiness insuredBusiness = new InsuredBusiness();
                return insuredBusiness.UpdateInsuredConcept(insuredConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.InsuredAgent CreateInsuredAgent(Models.Agency insuredAgency, int individualId)
        {
            try
            {
                AgentBusiness insuredAgentBusiness = new AgentBusiness();
                return insuredAgentBusiness.CreateInsuredAgent(insuredAgency, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.InsuredAgent UpdateInsuredAgent(Models.Agency insuredAgency, int individualId)
        {
            try
            {
                AgentBusiness insuredAgentBusiness = new AgentBusiness();
                return insuredAgentBusiness.UpdateInsuredAgent(insuredAgency, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        #endregion


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



        #region PaymentMethodAccount

        /// <summary>
        /// Crear las formas de pago de la persona o de la compañia
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="IndividualId"></param>
        /// <returns>Models.PaymentMethodAccount</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.PaymentAccount CreatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int IndividualId)
        {
            try
            {
                //PaymentMethodAccountDAO paymentAccountTypeDAO = new PaymentMethodAccountDAO();
                //return paymentAccountTypeDAO.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId);
                return null;
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
        public List<Models.PaymentAccount> CreatePaymentMethodAccounts(List<Models.PaymentAccount> paymentMethodAccounts, int IndividualId)
        {

            //List<Models.PaymentAccount> ModelpaymentMethodAccounts = new List<Models.PaymentAccount>();
            //PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            //PaymentMethodAccountDAO paymentAccountTypeDAO = new PaymentMethodAccountDAO();

            //foreach (Models.PaymentAccount paymentMethodAccount in paymentMethodAccounts)
            //{
            //    paymentMethodDAO.CreatePaymentMethod(paymentMethodAccount, IndividualId);
            //    //if (paymentMethodAccount.PaymentMethod.Id != (int)PaymentMethodType.Cash)
            //    //{
            //    //    ModelpaymentMethodAccounts.Add(paymentAccountTypeDAO.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId));
            //    //}
            //}

            return null;
        }

        /// <summary>
        /// Actualizar las formas de pago de la persona o de la compañia
        /// </summary>
        /// <param name="paymentMethodAccount"></param>
        /// <param name="IndividualId"></param>
        /// <returns>Models.PaymentMethodAccount</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.PaymentAccount UpdatePaymentMethodAccount(Models.PaymentAccount paymentMethodAccount, int IndividualId)
        {
            try
            {
                //PaymentMethodAccountDAO paymentAccountTypeDAO = new PaymentMethodAccountDAO();
                //return paymentAccountTypeDAO.CreatePaymentMethodAccount(paymentMethodAccount, IndividualId);
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion


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
        public List<Models.ProspectNatural> GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(string documentNumber, string surname, string motherLastName, string name, string tradeName, int searchType)
        {
            try
            {
                ProspectNaturalDAO prospectNaturalDAO = new ProspectNaturalDAO();
                return prospectNaturalDAO.GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(documentNumber, surname, motherLastName, name, tradeName, searchType);
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
        /// Buscar prospectos
        /// </summary>
        /// <param name="individualType">Tipo de individuo</param>
        /// <param name="documentTypeId">Id tipo de documento</param>
        /// <param name="document">Documento</param>
        /// <returns>
        /// Prospecto
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public Models.Prospect GetProspectByIndividualTypeDocumentTypeIdDocument(IndividualType individualType, int documentTypeId, string document)
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

        #region IndividualTaxExeption
        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualTax"></param>
        /// <returns></returns>
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

        #endregion

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
                return null;
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
                return null;

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
                //PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
                //return paymentMethodDAO.GetPaymentMethodByIndividualId(individualId);
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<HouseType> GetHouseTypes()
        {
            HouseTypeDAO houseTypeDAO = new HouseTypeDAO();
            return houseTypeDAO.GetHouseTypes();
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
        /// Obtener Comisiones por Id Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Commission> GetAgentCommissionByIndividualId(int individualId)
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

        /// <summary>
        /// Obtener lista de Comisiones por Id agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns>Models.Agency</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.Commission> GetAgentCommissionByAgencyId(int agentId)
        {
            try
            {
                CommissionAgentDao ocmmissionAgent = new CommissionAgentDao();
                return ocmmissionAgent.GetAgentCommissionByAgencyId(agentId);
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
        public bool DeleteAgentCommission(Commission commissionAgent)
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

        #region "Insured"

        public List<Models.InsuredSegment> GetInsuredSegment()
        {
            try
            {
                InsuredBusiness insured = new InsuredBusiness();
                return insured.GetInsuredSegment();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.InsuredProfile> GetInsuredProfile()
        {
            try
            {
                InsuredBusiness insured = new InsuredBusiness();
                return insured.GetInsuredProfile();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //public List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter)
        //{
        //    InsuredDAO insuredDAO = new InsuredDAO();
        //    return insuredDAO.GetInsuredsByName(stringFilter);
        //}

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




        public List<Models.Company> GetCompanyByDocument(CustomerType customerType, string documentNumber)
        {
            if (customerType == CustomerType.Individual)
            {
                var companyBusiness = new CompanyBusiness();
                return companyBusiness.GetCompanyByDocument(documentNumber);
            }
            else
            {
                var prospectBusiness = new ProspectBusiness();
                return prospectBusiness.GetCompanyByDocument(documentNumber);
            }

        }

        public List<Models.Company> GetCompanyAdv(CustomerType customerType, Models.Company Company)
        {
            var companyBusiness = new CompanyBusiness();
            return companyBusiness.GetCompanyAdv(Company);
        }

        public Models.Company CreateCompany(Models.Company company)
        {
            var companyBusiness = new CompanyBusiness();
            return companyBusiness.CreateCompany(company);
        }

        /// <summary>
        /// Actualizar una nueva compañia
        /// </summary>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public virtual Models.Company UpdateCompany(Models.Company company)
        {
            var companyBusiness = new CompanyBusiness();
            return companyBusiness.UpdateCompany(company);
        }

        public List<Agency> CreateAgencies(List<Agency> agencies, int IndividualId)
        {
            AgencyBusiness agencyBusiness = new AgencyBusiness();
            return agencyBusiness.CreateAgencies(agencies, IndividualId);
        }

        public Agency CreateAgency(Agency agency, int IndividualId)
        {
            throw new NotImplementedException();
        }

        #region Consortium

        /// <summary>
        /// Obtiene consorciados por Asegurado y Por Individuo
        /// </summary>
        /// <param name="InsuredId">Id Asegurado</param>
        /// <param name="IndividualId">Id Individiduo</param>
        /// <returns></returns>
        public Consortium GetConsortiumByInsurendIdOnInvidualId(int InsuredId, int IndividualId)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.GetConsortiumByInsurendIdOnInvidualId(InsuredId, IndividualId);

            //var result = new ConsortiatedDAO();
            //return result.GetConsortiumByInsurendIdOnInvidualId(InsuredId, IndividualId);
        }

        /// <summary>
        /// Obtiene los Consorciados Por asegurado 
        /// </summary>
        /// <param name="InsuredIdd">Id Asegurado</param>
        /// <returns>Retorna el Resultado del consorciado.</returns>
        public List<Consortium> GetConsortiumByInsurendId(int InsuredId)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.GetCoConsortiumsByInsuredCode(InsuredId);

            //var result = new ConsortiatedDAO();
            //return result.GetCoConsortiumsByInsuredCode(InsuredId);
        }

        /// <summary>
        /// Crea  el consorciado 
        /// </summary>
        /// <param name="consortia"></param>
        /// <returns>Retorna Los datos de Creacion</returns>
        public Consortium CreateConsortium(Consortium consortia)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.CreateConsortium(consortia);
        }

        /// <summary>
        /// Actualiza Los Datos Del consorciado.
        /// </summary>
        /// <param name="consortium">Datos Actualizar Del Consorciado</param>
        /// <returns>Retorna los Datos Actualizados Del consorciado.</returns>
        public Consortium UpdateConsortium(Consortium consortium)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.UpdateConsortium(consortium);

            //var result = new ConsortiatedDAO();
            //return result.UpdateConsortium(consortium);
        }

        /// <summary>
        /// Elimina los Datos del consoriciado
        /// </summary>
        /// <param name="InsuredIdd">Id del Asegurado</param>
        /// <returns>Retorna la Eliminacon del Consorciado</returns>
        public bool DeleteConsortium(Consortium consortium)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.DeleteConsortium(consortium);

            //bool registro = false;
            //ConsortiatedDAO result = new ConsortiatedDAO();
            //registro = result.DeleteConsortium(InsuredIdd);
            //if (registro == true)
            //{
            //    return registro;
            //}
            //return registro;
        }
        /// <summary>
        ///  Elimina y actualiza el parametro del documento de tipos de consorcio
        /// </summary>
        /// <param name="parameterFutureSociety"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool DeleteUserAssignedConsortium(int parameterFutureSociety, int userId)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.DeleteUserAssignedConsortium(parameterFutureSociety, userId);
        }
        /// <summary>
        /// Consulta de documento de cualquier tipo consorcio
        /// </summary>
        /// <param name="parameterFutureSociety"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Models.UserAssignedConsortium GetUserAssignedConsortiumByparameterFutureSocietyByuserId(int parameterFutureSociety, int userId)
        {
            ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
            return consortiumBusiness.GetUserAssignedConsortiumByparameterFutureSocietyByuserId(parameterFutureSociety, userId);
        }
        #endregion Consortium

        /// <summary>
        /// Metodo para Crear el Coasegurado de una persona juridica.
        /// </summary>
        /// <param name="companyCoInsured">Datos Para Crear Coasegurado Una Compañia.</param>
        /// <returns>Retorna los datos de Coasegurado.</returns>
        public CompanyCoInsured CreateCompanyCoInsured(CompanyCoInsured companyCoInsured)
        {
            CompanyCoInsuredBusiness companyCoInsuredBusiness = new CompanyCoInsuredBusiness();
            return companyCoInsuredBusiness.CreateCompanyCoInsured(companyCoInsured);
        }

        /// <summary>
        /// Actualiza el coasegurado de una persona juridica.
        /// </summary>
        /// <param name="companyCoInsured">Datos Actualizar de una persona Juridica.</param>
        /// <returns>retorna la actualizacion de la persona juridica</returns>
        public CompanyCoInsured UpdateCompanyCoInsured(CompanyCoInsured companyCoInsured)
        {
            CompanyCoInsuredBusiness companyCoInsuredBusiness = new CompanyCoInsuredBusiness();
            return companyCoInsuredBusiness.UpdateCompanyCoInsured(companyCoInsured);
        }

        /// <summary>
        /// Obtiene los CoAsegurados por Individuo
        /// </summary>
        /// <param name="IndividualId">Id del Indivual Id</param>
        /// <returns>Retorna los Datos de CoAsegurado de Indivuo ID.</returns>
        public CompanyCoInsured GetCompanyCoInsuredIndividualId(int IndividualId)
        {
            CompanyCoInsuredBusiness companyCoInsuredBusiness = new CompanyCoInsuredBusiness();
            return companyCoInsuredBusiness.GetCompanyCoInsuredIndividualId(IndividualId);
        }
        /// <summary>
        /// Obtiene los Datos de CoaSegurado numero Tributario
        /// </summary>
        /// <param name="tributaryNo">Id Tributario.</param>
        /// <returns>Retorna informacion Tributaria de un coasegurado.</returns>
        public CompanyCoInsured GetCompanyCoInsuredTributary(string tributaryNo)
        {
            CompanyCoInsuredDAO companyCoInsuredDAO = new CompanyCoInsuredDAO();
            var result = companyCoInsuredDAO.GetCompanyCoInsuredTributaryID(tributaryNo);
            return result;
        }

        public List<Models.Guarantor> GetGuarantorsByGuaranteeId(int id)
        {
            GuarantorDAO guarantorDao = new GuarantorDAO();
            var result = guarantorDao.GetGuarantorsByGuaranteeId(id);
            return result;
        }


        #region IndividualPaymentMethod

        public List<Models.IndividualPaymentMethod> GetIndividualPaymentMethods(int individualId)
        {
            InvididualPaymentMethodBusiness invididualPaymentMethodBusiness = new InvididualPaymentMethodBusiness();
            return invididualPaymentMethodBusiness.GetIndividualPaymentMethodByindividualId(individualId);
        }

        public Models.PaymentAccount GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(Models.PaymentAccount paymentAccount, int paymentId, int individualId)
        {
            PaymentAccountBusiness paymentAccountBusiness = new PaymentAccountBusiness();
            return paymentAccountBusiness.GetPaymentMethodAccountByIndividualIdByPaymentMetodByPaymentAccount(paymentAccount, paymentId, individualId);
        }

        public Models.IndividualPaymentMethod CreateIndividualPaymentMethod(Models.IndividualPaymentMethod individualPaymentMethod, int individualId)
        {

            InvididualPaymentMethodBusiness invididualPaymentMethodBusiness = new InvididualPaymentMethodBusiness();
            PaymentAccountBusiness paymentAccountBusiness = new PaymentAccountBusiness();

            Models.IndividualPaymentMethod individualPayment = invididualPaymentMethodBusiness.CreateIndividualPaymentMethod(individualPaymentMethod, individualId);

            if (individualPaymentMethod.Method.Id != (int)PaymentMethodType.Cash)
            {
                individualPayment.Account = paymentAccountBusiness.CreatePaymentAccount(individualPaymentMethod.Account, Convert.ToInt32(individualPayment.Id), individualId);
            }
            return individualPayment;
        }

        public Models.IndividualPaymentMethod UpdateIndividualPaymentMethod(Models.IndividualPaymentMethod individualPaymentMethod, int individualId)
        {
            InvididualPaymentMethodBusiness invididualPaymentMethodBusiness = new InvididualPaymentMethodBusiness();
            PaymentAccountBusiness paymentAccountBusiness = new PaymentAccountBusiness();

            Models.IndividualPaymentMethod individualPayment = invididualPaymentMethodBusiness.UpdateIndividualPaymentMethod(individualPaymentMethod, individualId);
            if (individualPaymentMethod.Method.Id != (int)PaymentMethodType.Cash)
            {
                individualPayment.Account = paymentAccountBusiness.UpdatePaymentMethodAccount(individualPaymentMethod.Account, Convert.ToInt32(individualPayment.Id), individualId);
            }
            return individualPayment;
        }
        #endregion

        #region LegalRepresentative

        /// <summary>
        /// Guardar la informacion de un representante legal
        /// </summary>
        /// <param name="legalRepresent">Modelo LegalRepresent</param>
        /// <returns></returns
        public Models.LegalRepresentative CreateLegalRepresent(Models.LegalRepresentative legalRepresent, int individualId)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentativeBusiness = new LegalRepresentativeBusiness();
                return legalRepresentativeBusiness.CrateRepresentLegal(legalRepresent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// actualiza la informacion de un representante legal
        /// </summary>
        /// <param name="legalRepresent"></param>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.LegalRepresentative UpdateLegalRepresent(Models.LegalRepresentative legalRepresent, int individualId)
        {
            try
            {
                LegalRepresentativeBusiness legalRepresentativeBusiness = new LegalRepresentativeBusiness();
                return legalRepresentativeBusiness.UpdateRepresentLegal(legalRepresent);
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
                LegalRepresentativeBusiness legalRepresentativeBusiness = new LegalRepresentativeBusiness();
                return legalRepresentativeBusiness.GetLegalRepresentByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region Prospect


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
        /// Obtener Prospecto Natural
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        public List<Models.ProspectNatural> GetProspectNaturalByDocument(string documentNumber)
        {
            try
            {
                ProspectNaturalBusiness bis = new ProspectNaturalBusiness();
                return bis.GetProspectNaturalByDocument(documentNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener Prospecto legal
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <returns></returns>
        public List<ProspectNatural> GetProspectLegalByDocument(string documentNumber)
        {
            try
            {
                ProspectLegalBusiness bis = new ProspectLegalBusiness();
                return bis.GetProspectLegalByDocument(documentNumber);
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
                ProspectNaturalBusiness bis = new ProspectNaturalBusiness();
                return bis.CreateProspectNatural(prospectNatural);
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
                ProspectLegalBusiness bis = new ProspectLegalBusiness();
                return bis.CreateProspectLegal(prospectLegal);
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
                ProspectLegalBusiness bis = new ProspectLegalBusiness();
                return bis.UpdateProspectLegal(prospectLegal);
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
                ProspectNaturalBusiness bis = new ProspectNaturalBusiness();
                return bis.UpdateProspectNatural(prospectNatural);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        #endregion

        #region InsuredGuarantee

        /// <summary>
        /// listado de Contragarantias
        /// </summary>
        /// <param name="IndividualId">IndividualId asegurado</param>
        /// <returns>
        /// List Models.Guarantee
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.GuaranteeInsuredGuarantee> GetInsuredGuaranteeByIndividualId(int IndividualId)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.GetInsuredGuaranteesByIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        ///  Obtener contragarantias asegurado tipo Hipoteca
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public InsuredGuaranteeMortgage GetInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.GetInsuredGuaranteeMortgageByIndividualIdById(individualId, id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener contragarantias asegurado tipo Prenda
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public InsuredGuaranteePledge GetInsuredGuaranteePledgeByIndividualIdById(int individualId, int id)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.GetInsuredGuaranteePledgeByIndividualIdById(individualId, id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        ///  Obtener contragarantias asegurado tipo Pagaré
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public InsuredGuaranteePromissoryNote GetInsuredGuaranteePromissoryNoteByIndividualIdById(int individualId, int id)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.GetInsuredGuaranteePromissoryNoteByIndividualIdById(individualId, id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeMortgage CreateInsuredGuaranteeMortgage(InsuredGuaranteeMortgage insuredGuaranteeMortgage)
        {

            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.CreateInsuredGuaranteeMortgage(insuredGuaranteeMortgage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteePledge CreateInsuredGuaranteePledge(InsuredGuaranteePledge insuredGuaranteePledge)
        {

            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.CreateInsuredGuaranteePledge(insuredGuaranteePledge);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteePromissoryNote CreateInsuredGuaranteePromissoryNote(InsuredGuaranteePromissoryNote insuredGuaranteePromissoryNote)
        {

            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.CreateInsuredGuaranteePromissoryNote(insuredGuaranteePromissoryNote);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeMortgage UpdateInsuredGuaranteeMortgage(InsuredGuaranteeMortgage insuredGuaranteeMortgage)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.UpdateInsuredGuaranteeMortgage(insuredGuaranteeMortgage);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteePledge UpdateInsuredGuaranteePledge(InsuredGuaranteePledge insuredGuaranteePledge)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.UpdateInsuredGuaranteePledge(insuredGuaranteePledge);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteePromissoryNote UpdateInsuredGuaranteePromissoryNote(InsuredGuaranteePromissoryNote insuredGuaranteePromissoryNote)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.UpdateInsuredGuaranteePromissoryNote(insuredGuaranteePromissoryNote);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeFixedTermDeposit GetInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.GetInsuredGuaranteeFixedTermDepositByIndividualIdById(individualId, id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeFixedTermDeposit CreateInsuredGuaranteeFixedTermDeposit(InsuredGuaranteeFixedTermDeposit insuredGuaranteeFixedTermDeposit)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.CreateInsuredGuaranteeFixedTermDeposit(insuredGuaranteeFixedTermDeposit);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeFixedTermDeposit UpdateInsuredGuaranteeFixedTermDeposit(InsuredGuaranteeFixedTermDeposit insuredGuaranteeFixedTermDeposit)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.UpdateInsuredGuaranteeFixedTermDeposit(insuredGuaranteeFixedTermDeposit);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeOthers GetInsuredGuaranteeOthersByIndividualIdById(int individualId, int id)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.GetInsuredGuaranteeOthersByIndividualIdById(individualId, id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeOthers CreateInsuredGuaranteeOthers(InsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.CreateInsuredGuaranteeOthers(insuredGuaranteeOthers);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public InsuredGuaranteeOthers UpdateInsuredGuaranteeOthers(InsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            try
            {
                InsuredGuaranteeBusiness bis = new InsuredGuaranteeBusiness();
                return bis.UpdateInsuredGuaranteeOthers(insuredGuaranteeOthers);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region InsuredGuaranteeDocumentation

        public Models.InsuredGuaranteeDocumentation CreateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation guarantee)
        {
            InsuredGuaranteeDocumentationBusiness insuredGuaranteeDocumentationBusiness = new InsuredGuaranteeDocumentationBusiness();
            return insuredGuaranteeDocumentationBusiness.CreateInsuredGuaranteeDocumentation(guarantee);
        }

        public Models.InsuredGuaranteeDocumentation UpdateInsuredGuaranteeDocumentation(Models.InsuredGuaranteeDocumentation guarantee)
        {
            InsuredGuaranteeDocumentationBusiness insuredGuaranteeDocumentationBusiness = new InsuredGuaranteeDocumentationBusiness();
            return insuredGuaranteeDocumentationBusiness.UpdateInsuredGuaranteeDocumentation(guarantee);
        }

        public void DeleteInsuredGuaranteeDocumentation(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            InsuredGuaranteeDocumentationBusiness insuredGuaranteeDocumentationBusiness = new InsuredGuaranteeDocumentationBusiness();
            insuredGuaranteeDocumentationBusiness.DeleteInsuredGuaranteeDocumentation(individualId, insuredguaranteeId, guaranteeId, documentId);
        }

        public Models.InsuredGuaranteeDocumentation GetInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            InsuredGuaranteeDocumentationBusiness insuredGuaranteeDocumentationBusiness = new InsuredGuaranteeDocumentationBusiness();
            return insuredGuaranteeDocumentationBusiness.GetInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(individualId, insuredguaranteeId, guaranteeId, documentId);
        }

        public List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocumentation()
        {
            InsuredGuaranteeDocumentationBusiness insuredGuaranteeDocumentationBusiness = new InsuredGuaranteeDocumentationBusiness();
            return insuredGuaranteeDocumentationBusiness.GetInsuredGuaranteeDocumentation();
        }

        /// <summary>
        /// Consulta documentación asociada a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Documentación asociada a una contragarantía </returns>
        public List<Models.InsuredGuaranteeDocumentation> GetInsuredGuaranteeDocument(int individualId, int guaranteeId)
        {
            try
            {
                InsuredGuaranteeDocumentationBusiness insuredGuaranteeDocumentationBusiness = new InsuredGuaranteeDocumentationBusiness();
                return insuredGuaranteeDocumentationBusiness.GetInsuredGuaranteeDocumentation(individualId, guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        # region GuaranteeRequiredDocument
        /// <summary>
        /// Obtiene Documentación Recibida
        /// </summary>
        /// <param name="guaranteeId"></param>
        /// <returns>
        /// List Models.GuaranteeRequiredDocument
        /// </returns>
        /// <exception cref="BusinessException"></exception>
        public List<GuaranteeRequiredDocument> GetDocumentationReceivedByGuaranteeId(int guaranteeId)
        {
            try
            {
                GuaranteeRequiredDocumentBusiness guaranteeRequiredDocumentBusiness = new GuaranteeRequiredDocumentBusiness();
                return guaranteeRequiredDocumentBusiness.GetDocumentationReceivedByGuaranteeId(guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region InsuredGuaranteeprefix

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
                InsuredGuaranteePrefixBusiness bis = new InsuredGuaranteePrefixBusiness();
                return bis.CreateInsuredGuaranteePrefixByIndividualIdByGuaranteeId(individualId, guaranteeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.InsuredGuaranteePrefix CreateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix insuredGuaranteePrefix)
        {
            try
            {
                InsuredGuaranteePrefixBusiness bis = new InsuredGuaranteePrefixBusiness();
                return bis.CreateInsuredGuaranteePrefix(insuredGuaranteePrefix);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.InsuredGuaranteePrefix UpdateInsuredGuaranteePrefix(Models.InsuredGuaranteePrefix insuredGuaranteePrefix)
        {
            try
            {
                InsuredGuaranteePrefixBusiness bis = new InsuredGuaranteePrefixBusiness();
                return bis.UpdateInsuredGuaranteePrefix(insuredGuaranteePrefix);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteInsuredGuaranteePrefix(int individualId, int guaranteeId, int insureedguaranteePrefixId)
        {
            try
            {
                InsuredGuaranteePrefixBusiness bis = new InsuredGuaranteePrefixBusiness();
                bis.DeleteInsuredGuaranteePrefix(individualId, guaranteeId, insureedguaranteePrefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region insuredGuaranteLog

        public List<Models.InsuredGuaranteeLog> GetInsuredGuaranteeLogByIndividualIdByGuaranteeId(int individualId, int guaranteeId)
        {
            InsuredGuaranteeLogBusiness insuredGuaranteeLog = new InsuredGuaranteeLogBusiness();
            return insuredGuaranteeLog.GetInsuredGuaranteeLogByIndividualIdByGuaranteeId(individualId, guaranteeId);
        }

        public Models.InsuredGuaranteeLog CreateInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog)
        {
            InsuredGuaranteeLogBusiness bis = new InsuredGuaranteeLogBusiness();
            return bis.CreateInsuredGuaranteeLog(insuredGuaranteeLog);
        }

        public Models.InsuredGuaranteeLog UpdateInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog)
        {
            InsuredGuaranteeLogBusiness bis = new InsuredGuaranteeLogBusiness();
            return bis.UpdateInsuredGuaranteeLog(insuredGuaranteeLog);
        }

        public void DeleteInsuredGuaranteeLog(Models.InsuredGuaranteeLog insuredGuaranteeLog)
        {
            InsuredGuaranteeLogBusiness bis = new InsuredGuaranteeLogBusiness();
            bis.DeleteInsuredGuaranteeLog(insuredGuaranteeLog);
        }

        #endregion

        #region Guarantor

        public List<Models.Guarantor> GetGuarantorByIndividualIdByGuaranteeId(int individualId, int guaranteeId)
        {
            GuarantorBusiness business = new GuarantorBusiness();
            return business.GetGuarantorsByindividualIdByid(individualId, guaranteeId);
        }

        public Models.Guarantor CreateGuatantor(Models.Guarantor guarantor)
        {
            GuarantorBusiness business = new GuarantorBusiness();
            return business.CreateGuarantor(guarantor);
        }

        public Models.Guarantor UpdateGuarantor(Models.Guarantor guarantor)
        {
            GuarantorBusiness business = new GuarantorBusiness();
            return business.UpdateGuarantor(guarantor);
        }

        public void DeleteGuarantor(Models.Guarantor guarantor)
        {
            GuarantorBusiness business = new GuarantorBusiness();
            business.DeleteGuarantor(guarantor);
        }


        #endregion
        #region CompanyInsurance
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
        #endregion

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

        public List<Models.Base.BaseInsuredMain> GetInsuredsByName(string stringFilter)
        {
            InsuredDAO insuredDAO = new InsuredDAO();
            return insuredDAO.GetInsuredsByName(stringFilter);
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
        /// <summary>
        /// Obtiene la lista de Segmentos de Asegurado.
        /// </summary>
        /// <returns>Lista de Segmentos de Asegurado consultados</returns>
        public List<Models.InsuredSegmentV1> GetInsuredSegments()
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
        public ParametrizationResponse<Models.InsuredSegmentV1> CreateInsuredSegments(List<Models.InsuredSegmentV1> ListAdded, List<Models.InsuredSegmentV1> ListEdited, List<Models.InsuredSegmentV1> ListDeleted)
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
        public string GenerateFileToInsuredSegment(List<Models.InsuredSegmentV1> insuredSegment, string fileName)
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

        #region BusinessName
        /// <summary>
        /// Crear un impuesto
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public Models.CompanyName CreateBusinessName(Models.CompanyName companyName)
        {
            try
            {
                Models.CompanyName modelCompanyName = new CompanyName();
                CoCompanyNameBusiness companyNameBusiness = new CoCompanyNameBusiness();
                modelCompanyName = companyNameBusiness.CreateCoCompanyNames(companyName);
                CreateInsuredControl(modelCompanyName.IndividualId);
                return modelCompanyName;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public Models.CompanyName UpdateBusinessName(Models.CompanyName companyName)
        {
            try
            {
                Models.CompanyName modelCompanyName = new CompanyName();
                CoCompanyNameBusiness companyNameBusiness = new CoCompanyNameBusiness();
                modelCompanyName = companyNameBusiness.UpdateCoCompanyName(companyName);
                CreateInsuredControl(modelCompanyName.IndividualId);
                return modelCompanyName;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.EconomicGroup CreateEconomicGroup(Models.EconomicGroup economicGroup, List<Models.EconomicGroupDetail> listGroupDetail)
        {

            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.CreateEconomicGroup(economicGroup, listGroupDetail);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.EconomicGroup> GetGroupEconomicById(int Id)
        {
            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.GetGroupEconomicById(Id);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public int CountBusinessNameByIndividualId(int individualId)
        {
            try
            {
                CoCompanyNameBusiness companyNameBusiness = new CoCompanyNameBusiness();
                return companyNameBusiness.CountBusinessNameByIndividualId(individualId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.TributaryIdentityType> GetTributaryType()
        {
            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.GetTributaryType();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.EconomicGroupDetail CreateEconomicGroupDetail(Models.EconomicGroupDetail economicGroupDetail)
        {

            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.CreateEconomicGroupDetail(economicGroupDetail);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.CompanyName> GetBusinessNameByIndividualId(int individualId)
        {
            try
            {
                CoCompanyNameBusiness companyNameBusiness = new CoCompanyNameBusiness();
                return companyNameBusiness.GetCoCompanyNamesByIndividualId(individualId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.EconomicGroup> GetEconomicGroupByDocument(string groupName, string documentNo)
        {
            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.GetEconomicGroup(groupName, documentNo, null);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion
        #region BankTransfers
        /// <summary>
        /// Create Bank Transfers
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public List<Models.BankTransfers> CreateBankTransfers(List<Models.BankTransfers> companyBankTransfers)
        {
            try
            {
                Models.PersonAccountBankControl personAccountBankControl = new Models.PersonAccountBankControl();
                BankTransfersBusiness companyBankTransfersBusiness = new BankTransfersBusiness();
                List<Models.BankTransfers> bankTransfers = new List<BankTransfers>();

                PersonDAO personDAO = new PersonDAO();
                bankTransfers = companyBankTransfersBusiness.CreateBankTransfers(companyBankTransfers);
                #region Punto de control (Integracion)
                personAccountBankControl.IndividualId = bankTransfers.First().Individual;
                personAccountBankControl.Action = "I";
                personDAO.CreatePersonAccountBankControl(personAccountBankControl);
                #endregion
                return bankTransfers;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public Models.BankTransfers UpdateBankTransfers(Models.BankTransfers bankTransfers)
        {
            try
            {
                Models.PersonAccountBankControl personAccountBankControl = new Models.PersonAccountBankControl();
                BankTransfersBusiness companybankTransfers = new BankTransfersBusiness();
                Models.BankTransfers bankTransfer = new BankTransfers();
                PersonDAO personDAO = new PersonDAO();
                bankTransfer = companybankTransfers.UpdateBankTransfers(bankTransfers);
                #region Punto de control (Integracion)
                personAccountBankControl.IndividualId = bankTransfer.Individual;
                personAccountBankControl.Action = "U";
                personDAO.CreatePersonAccountBankControl(personAccountBankControl);
                #endregion
                return bankTransfer;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<Models.BankTransfers> GetBankTransfersByIndividualId(int individualId)
        {
            try
            {
                BankTransfersBusiness companyBankTransfers = new BankTransfersBusiness();
                return companyBankTransfers.GetPersonAccountBankByIndividualId(individualId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.Company UpdateCompanyBasicInfo(Models.Company company)
        {
            var companyBusiness = new CompanyBusiness();
            return companyBusiness.UpdateCompanyBasicInfo(company);
        }

        public List<Models.EconomicGroup> GetEconomicGroupByEconomicGroup(Models.EconomicGroup economicGroup)
        {
            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.GetEconomicGroup(economicGroup.EconomicGroupName, economicGroup.TributaryIdNo, economicGroup.Enabled);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.EconomicGroupDetail> GetEconomicGroupDetailById(int economicGroupId)
        {
            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.GetEconomicGroupDetail(economicGroupId);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.Insured> GetEconomicGroupInsureds(int economicGroupId)
        {
            try
            {
                EconomicGroupDAO daoEconomiGroup = new EconomicGroupDAO();
                return daoEconomiGroup.GetEconomicGroupInsureds(economicGroupId);

            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion BankTransfers

        #region Consortium
        public Consortium GetConsortiumByIndividualId(int IndividualId)
        {
            try
            {
                ConsortiumBusiness consortiumBusiness = new ConsortiumBusiness();
                return consortiumBusiness.GetConsortiumByIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<GuaranteeStatus> GetGuaranteeStatusRoutesByGuaranteeStatusId(int guaranteeStatusId)
        {
            try
            {
                GuaranteeStatusDAO guaranteeStatusDAO = new GuaranteeStatusDAO();
                return guaranteeStatusDAO.GetGuaranteeStatusRoutesByGuaranteeStatusId(guaranteeStatusId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        public List<GuaranteeStatus> GetUnassignedGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId)
        {

            try
            {
                GuaranteeStatusDAO guaranteeStatusDAO = new GuaranteeStatusDAO();
                return guaranteeStatusDAO.GetUnassignedGuaranteeStatusByGuaranteeStatusId(guaranteeStatusId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<GuaranteeStatus> CreateGuaranteeStatusRoutes(List<GuaranteeStatus> allGuaranteeEstatusAssign, int guaranteeStatusId)
        {
            try
            {
                GuaranteeStatusDAO guaranteeStatusDAO = new GuaranteeStatusDAO();
                return guaranteeStatusDAO.CreateGuaranteeStatusRoutes(allGuaranteeEstatusAssign, guaranteeStatusId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public Models.EconomicGroupDetail GetExistIndividdualByIndividualId(int IndividualId)
        {
            try
            {
                EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                return economicGroupDAO.GetExistIndividdualByIndividualId(IndividualId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Models.EconomicGroupDetail> GetEconomicGroupDetailByIndividual(int IndividualId)
        {
            try
            {
                EconomicGroupDAO economicGroupDAO = new EconomicGroupDAO();
                return economicGroupDAO.GetEconomicGroupDetailByIndividual(IndividualId);
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Create Person Operation
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public Models.PersonOperation CreatePersonOperation(Models.PersonOperation personOperation)
        {
            try
            {
                PersonOperationDAO personOperationDAO = new PersonOperationDAO();
                return personOperationDAO.CreatePersonOperation(personOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public Models.PersonOperation GetPersonOperation(int personOperation)
        {
            try
            {
                PersonOperationDAO personOperationDAO = new PersonOperationDAO();
                return personOperationDAO.GetPersonOperation(personOperation);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<GuaranteeStatus> GetGuaranteeStatusByGuaranteeStatusId(int guaranteeStatusId)
        {
            try
            {
                GuaranteeStatusDAO guaranteeStatusDAO = new GuaranteeStatusDAO();
                return guaranteeStatusDAO.GetGuaranteeStatusByGuaranteeStatusId(guaranteeStatusId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }


        public List<Models.PersonOperation> GetOperationTmp(int IndividualId)
        {
            try
            {
                PersonOperationDAO personOperationDAO = new PersonOperationDAO();
                return personOperationDAO.GetOperationTmp(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }


        public List<Models.Consortium> GetConsortiumsByIndividualId(int individualId)
        {
            try
            {
                ConsortiatedDAO consortiatedDAO = new ConsortiatedDAO();
                return consortiatedDAO.GetConsortiumsByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public Models.Person GetPersonByDocumentByDocumentType(string document, int documentType)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.GetPersonByDocumentByDocumentType(document, documentType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.Company GetCompanyByDocumentByDocumentType(string document, int documentType)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.GetCompanyByDocumentByDocumentType(document, documentType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.CompanyExtended GetCoCompanyByIndividualId(int individualId)
        {
            try
            {
                CompanyExtendedDAO companyExtended = new CompanyExtendedDAO();
                return companyExtended.GetCoCompanyByIndividualId(individualId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.AccountType> GetAccountTypes()
        {
            try
            {
                CompanyExtendedDAO companyExtended = new CompanyExtendedDAO();
                return companyExtended.GetAccountTypes();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        #region ElectronicBilling
        /// <summary>
        /// Create electronic billing
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public List<Models.InsuredFiscalResponsibility> CreateInsuredFiscalResponsibility(List<Models.InsuredFiscalResponsibility> companyInsuredFiscalResponsibility)
        {
            try
            {
                InsuredFiscalResponsibilityBusiness insuredFiscalResponsibilityBusiness = new InsuredFiscalResponsibilityBusiness();
                List<Models.InsuredFiscalResponsibility> insuredFiscal = new List<Models.InsuredFiscalResponsibility>();
                insuredFiscal = insuredFiscalResponsibilityBusiness.CreateInsuredFiscalResponsibility(companyInsuredFiscalResponsibility);
                return insuredFiscal;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Elimina las responsabilidades fiscales seleccionadas
        /// </summary>
        /// <param name="InsuredIdd">Id del Asegurado</param>
        /// <returns>Retorna la Eliminacon del Consorciado</returns>
        public bool DeleteFiscalResponsibility(Models.InsuredFiscalResponsibility fiscal)
        {
            InsuredFiscalResponsibilityBusiness fiscalBusiness = new InsuredFiscalResponsibilityBusiness();
            return fiscalBusiness.DeleteFiscalResponsibility(fiscal);
            
        }



        public Models.InsuredFiscalResponsibility UpdateInsuredFiscalResponsibility(Models.InsuredFiscalResponsibility companyInsuredFiscalResponsibility)
        {
            try
            {
                InsuredFiscalResponsibilityBusiness insuredFiscalResponsibilityBusiness = new InsuredFiscalResponsibilityBusiness();
                Models.InsuredFiscalResponsibility insuredFiscal = new Models.InsuredFiscalResponsibility();
                insuredFiscal = insuredFiscalResponsibilityBusiness.UpdateInsuredFiscalResponsibility(companyInsuredFiscalResponsibility);
                return insuredFiscal;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }

        public List<Models.InsuredFiscalResponsibility> GetFiscalResponsibilityByIndividualId(int individualId)
        {
            try
            {
                InsuredFiscalResponsibilityBusiness companyInsuredFiscal = new InsuredFiscalResponsibilityBusiness();
                return companyInsuredFiscal.GetFiscalResponsibilityByIndividualId(individualId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }


        public Models.FiscalResponsibility GetFiscalResponsibilityById(int Id)
        {
            try
            {
                FiscalResponsibilityBusiness fiscalProvider = new FiscalResponsibilityBusiness();
                return fiscalProvider.GetFiscaResponsibilityById(Id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion ElectronicBilling

        #region Punto de control(Integracion)

        private void CreateInsuredControl(int individualId)
        {
            try
            {
                InsuredControl insuredControl = new InsuredControl();
                PersonDAO personDAO = new PersonDAO();
                int insuredCode = 0;

                CreateIndividualControl(individualId);
                insuredCode = personDAO.GetInsuredByIndividualId(individualId);
                if (insuredCode > 0)
                {
                    insuredControl.IndividualId = individualId;
                    insuredControl.InsuredCode = insuredCode;
                    insuredControl.Action = "I";
                    personDAO.CreateInsuredControl(insuredControl);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        private void CreateIndividualControl(int individualId)
        {
            try
            {
                IndividualControl individualControl = new IndividualControl();
                PersonDAO personDAO = new PersonDAO();
                individualControl.IndividualId = individualId;
                individualControl.Action = "U";
                personDAO.CreateIndividualControl(individualControl);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

    }
}


