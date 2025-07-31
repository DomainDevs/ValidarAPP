using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using Newtonsoft.Json;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Enums;
using Sistran.Company.Application.UniquePersonServices.V1.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Resources;
using Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using ACM = Sistran.Core.Application.CommonService.Models;
using ENUMCOAP = Sistran.Core.Application.AuthorizationPoliciesServices.Enums;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider
{
    /// <summary>
    /// Personas
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider" />
    /// <seealso cref="Sistran.Company.Application.UniquePersonService.IUniquePersonService" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonServiceV1EEProvider : Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider, IUniquePersonService
    {
        #region person
        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyPerson CreateCompanyPerson(Models.CompanyPerson person)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                CompanyPerson companyPerson = new CompanyPerson();
                PersonDAO personDAO = new PersonDAO();
                companyPerson = personBusiness.CreateCompanyPerson(person);
                #region Control 
                IndividualControl individualControl = new IndividualControl();
                individualControl.IndividualId = companyPerson.IndividualId;
                individualControl.Action = "I";
                personDAO.CreateIndividualControl(individualControl);
                #endregion
                return companyPerson;
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
        public Models.CompanyPerson UpdateCompanyPerson(Models.CompanyPerson person)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                CompanyPerson companyPerson = new CompanyPerson();
                PersonDAO personDAO = new PersonDAO();
                companyPerson = personBusiness.UpdateCompanyPerson(person);
                #region Control 
                IndividualControl individualControl = new IndividualControl();
                individualControl.IndividualId = companyPerson.IndividualId;
                individualControl.Action = "U";
                personDAO.CreateIndividualControl(individualControl);
                #endregion
                return companyPerson;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<CompanyAddress> GetCompanyAddresses(int individualId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.GetCompanyAddresses(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<CompanyAddress> CreateCompanyAddresses(int individualId, List<CompanyAddress> addresses)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.CreateCompanyAddresses(individualId, addresses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyAddress> UpdateCompanyAddresses(int individualId, List<CompanyAddress> addresses)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.UpdateCompanyAddresses(individualId, addresses);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPhone> GetCompanyPhones(int individualId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.GetCompanyPhones(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<CompanyPhone> CreateCompanyPhones(int individualId, List<CompanyPhone> phones)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.CreateCompanyPhones(individualId, phones);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPhone> UpdateCompanyPhones(int individualId, List<CompanyPhone> phones)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.UpdateCompanyPhones(individualId, phones);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEmail> GetCompanyEmails(int individualId)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.GetCompanyEmails(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEmail> CreateCompanyEmails(int individualId, List<CompanyEmail> emails)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.CreateCompanyEmails(individualId, emails);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEmail> UpdateCompanyEmails(int individualId, List<CompanyEmail> emails)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.UpdateCompanyEmails(individualId, emails);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPerson> GetCompanyPersonByDocument(CustomerType customerType, string documentNumber)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.GetCompanyPersonByDocument(customerType, documentNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPerson> GetCompanyPersonAdv(CustomerType customerType, CompanyPerson person)
        {
            try
            {
                PersonBusiness personBusiness = new PersonBusiness();
                return personBusiness.GetCompanyPersonAdv(customerType, person);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyCompany> GetCompanyCompanyAdv(CustomerType customerType, CompanyCompany company)
        {
            try
            {
                CompanyBusiness companyBusiness = new CompanyBusiness();
                return companyBusiness.GetCompanyCompanyAdv(customerType, company);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyEconomicActivity GetCompanyEconomicActivitiesById(int id)
        {
            try
            {
                EconomicActivityBusiness economicActivityBusiness = new EconomicActivityBusiness();
                return economicActivityBusiness.GetEconomicActivitiesById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public Models.CompanyPerson GetComPanyPersonByIndividualId(int individualId)
        {
            try
            {
                PersonBusiness personModel = new PersonBusiness();
                return personModel.GetCompanyPersonByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public Models.CompanyPerson UpdateApplicationPersonBasicInfo(CompanyPerson companyPerson, bool validatePolicies = true)
        {
            try
            {
                PersonBusiness personModel = new PersonBusiness();
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies = ValidateAuthorizationPoliciesPersonBasicInfo(companyPerson, null);
                }
                if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                    {
                        companyPerson.OperationId = this.CreateCompanyPersonOperation(
                                new CompanyPersonOperation
                                {
                                    IndividualId = companyPerson.IndividualId,
                                    Operation = JsonConvert.SerializeObject(companyPerson),
                                    ProcessType = "Update Person Basic Info",
                                    FunctionId = (int)ENUMCOAP.TypeFunction.PersonBasicInfo,
                                    Process = ENUMCOAP.TypeFunction.PersonBasicInfo.ToString()
                                }
                        ).OperationId;
                    }
                }
                else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
                {
                    companyPerson = personModel.UpdateCompanyPersonBasicInfo(companyPerson);
                    CreateIntegrationNotification(companyPerson.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.EDI_INF_BASIC);
                }
                companyPerson.InfringementPolicies = infringementPolicies == null ? new List<PoliciesAut>() : infringementPolicies;
                return companyPerson;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        public List<CompanyPerson> GetPerson2gByDocumentNumber(string documentNumber, bool company)
        {
            return ModelAssembler.CreatePeople(DelegateService.externalProxyService.GetPerson2gByDocumentNumber(documentNumber, company).ToList());
        }

        public PersonDTO GetPerson2gByPersonId(int personId, bool company)
        {
            CompanyPersonOperationBusiness companyPersonOperationBusiness = new CompanyPersonOperationBusiness();
            return companyPersonOperationBusiness.CreatePerson2gOperation(personId, company);
        }

        public CompanyDTO GetCompany2gByPersonId(int personId, bool company)
        {
            CompanyPersonOperationBusiness companyPersonOperationBusiness = new CompanyPersonOperationBusiness();
            return companyPersonOperationBusiness.CreateCompany2gOperation(personId, company);
        }
        #endregion person

        #region Reinsurer V1
        public Models.CompanyReInsurer GetCompanyReInsurerByIndividualId(int individualId)
        {
            try
            {
                CompanyReinsurerBusiness companyReinsurerBusiness = new CompanyReinsurerBusiness();
                return companyReinsurerBusiness.GetCompanyReInsurerByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyReInsurer CreateCompanyReinsurer(CompanyReInsurer reinsurer)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                bool validateRolReinsure = GetCompanyIndividualRoleByIndividualId(reinsurer.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_REINSURER)));
                if (!validateRolReinsure)
                {
                    CreateCompanyIndividualRoleByIndividualId(reinsurer.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_REINSURER)));
                }
                CompanyReInsurer companyReInsurer = GetCompanyReInsurerByIndividualId(reinsurer.IndividualId);
                if (companyReInsurer.IndividualId > 0)
                {
                    CompanyReInsurer updateCompanyReinsurer = UpdateCompanyReinsurer(reinsurer);
                    return updateCompanyReinsurer;
                }
                else
                {
                    CompanyReinsurerBusiness companyReinsurerBusiness = new CompanyReinsurerBusiness();
                    CompanyReInsurer createCompanyReInsurer = new CompanyReInsurer();
                    createCompanyReInsurer = companyReinsurerBusiness.CreateCompanyReinsurer(reinsurer);
                    #region Control
                    CompanyReinsuranceControl companyReinsuranceControl = new CompanyReinsuranceControl();
                    companyReinsuranceControl.IndividualId = createCompanyReInsurer.IndividualId;
                    companyReinsuranceControl.Action = "I";
                    personDAO.CreateCompanyReinsuranceControl(companyReinsuranceControl);
                    #endregion
                    return createCompanyReInsurer;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyReInsurer UpdateCompanyReinsurer(CompanyReInsurer reinsurer)
        {
            try
            {
                CompanyReinsurerBusiness companyReInsurerBusiness = new CompanyReinsurerBusiness();
                CompanyReInsurer companyReInsurer = new CompanyReInsurer();
                PersonDAO personDAO = new PersonDAO();
                companyReInsurer = companyReInsurerBusiness.UpdateCompanyReInsurerBusiness(reinsurer);
                #region Control
                CompanyReinsuranceControl companyReinsuranceControl = new CompanyReinsuranceControl();
                companyReinsuranceControl.IndividualId = companyReInsurer.IndividualId;
                companyReinsuranceControl.Action = "U";
                personDAO.CreateCompanyReinsuranceControl(companyReinsuranceControl);
                #endregion
                return companyReInsurer;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Reinsurer V1

        #region Agent V1
        /// <summary>
        /// Obtener lista de ramos comerciales por agente
        /// </summary>
        /// <param name="agentId">Id agente</param>
        /// <returns></returns>
        public CompanyAgent GetCompanyAgentByIndividualId(int individualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.GetCompanyAgentByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea el Agente.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>

        public CompanyAgent CreateCompanyAgent(CompanyAgent companyAgent)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                bool validateRolAgent = GetCompanyIndividualRoleByIndividualId(companyAgent.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_AGENT)));
                if (!validateRolAgent)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyAgent.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_AGENT)));
                }
                return companyAgentBusiness.CreateCompanyAgent(companyAgent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea el Agente rol.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>
        public CompanyAgent CreateCompanyAgentRol(CompanyAgent companyAgent)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                bool validateRolAgent = GetCompanyIndividualRoleByIndividualId(companyAgent.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_AGENT)));
                if (!validateRolAgent)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyAgent.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_AGENT)));
                }
                return companyAgent;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el Agente.
        /// </summary>
        /// <param name="companyAgent">Modelo Agente</param>
        /// <returns></returns>

        public CompanyAgent UpdateCompanyAgent(CompanyAgent companyAgent)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.UpdateCompanyAgent(companyAgent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyAgency> GetCompanyAgencyByInvidualId(int InvidualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                var result = companyAgentBusiness.GetCompanyAgencyByInvidualId(InvidualId);
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<CompanyAgency> GetActiveCompanyAgencyByInvidualId(int InvidualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                var result = companyAgentBusiness.GetActiveCompanyAgency(InvidualId);
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea las Agencias por inviduaId
        /// </summary>
        /// <param name="companyAgencies">Modelo de Agencias.</param>
        /// <param name="IndividualId">Codigo Invidual id</param>
        /// <returns></returns>
        /// 
        public CompanyAgency CreateCompanyAgencyByInvidualId(CompanyAgency companyAgencies, int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                CompanyAgency resultCompanyAgency = new CompanyAgency();
                PersonDAO personDAO = new PersonDAO();
                resultCompanyAgency = companyAgentBusiness.CreateCompanyAgencyByInvidualId(companyAgencies, IndividualId);
                #region Control                                 
                AgentControl agentControl = new AgentControl();
                agentControl.IndividualId = IndividualId;
                agentControl.Action = "I";
                personDAO.CreateAgentControl(agentControl);
                #endregion
                return resultCompanyAgency;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza Agencia por InvidualId
        /// </summary>
        /// <param name="companyAgencies"></param>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        /// 
        public CompanyAgency UpdateCompanyAgencyByInvidualId(CompanyAgency companyAgencies, int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                CompanyAgency resultCompanyAgency = new CompanyAgency();
                PersonDAO personDAO = new PersonDAO();
                resultCompanyAgency = companyAgentBusiness.UpdateCompanyAgencyByInvidualId(companyAgencies, IndividualId);
                #region Control                                 
                AgentControl agentControl = new AgentControl();
                agentControl.IndividualId = IndividualId;
                agentControl.Action = "U";
                personDAO.CreateAgentControl(agentControl);
                #endregion
                return resultCompanyAgency;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtiene los Ramos Por IndividualID
        /// </summary>
        /// <param name="IndividualId">Codigo IndividualId</param>
        /// <returns></returns>
        public List<CompanyPrefixs> GetPrefixesByAgentIds(int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.GetCompanyPrefixesAgentIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea ramos por el codigo de la persona.
        /// </summary>
        /// <param name="companyPrefix">model del ramo</param>
        /// <param name="IndividualId">codigo de la persoa </param>
        /// <returns></returns>
        public CompanyPrefixs CreatePrefixesByAgentIds(CompanyPrefixs companyPrefix, int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.CretaeCompanyPrefixesAgentIndividualId(companyPrefix, IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza ramos por el codigo de la persona.
        /// </summary>
        /// <param name="companyPrefix">model del ramo</param>
        /// <param name="IndividualId">codigo de la persoa </param>
        /// <returns></returns>
        public CompanyPrefixs UpdatePrefixesByAgentIds(CompanyPrefixs companyPrefix, int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.UpdateCompanyPrefixesAgentIndividualId(companyPrefix, IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Elimina ramos por el codigo de la persona.
        /// </summary>
        /// <param name="companyPrefix">model del ramo</param>
        /// <param name="IndividualId">codigo de la persoa </param>
        /// <returns></returns>
        public CompanyPrefixs DeletePrefixesByAgentIds(CompanyPrefixs companyPrefix, int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.DeleteCompanyPrefixesAgentIndividualId(companyPrefix, IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        public List<CompanyComissionAgent> GetCompanycommissionAgent(int IndividualId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.GetCompanyComissionIndividualId(IndividualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Crea las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        public CompanyComissionAgent CreateCompanycommissionAgent(CompanyComissionAgent companyComissionAgent, int IndividualId, int AgencyId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.CreateCompanyComissionIndividualId(companyComissionAgent, IndividualId, AgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actuliza las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        public CompanyComissionAgent UpdateCompanycommissionAgent(CompanyComissionAgent companyComissionAgent, int IndividualId, int AgencyId)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.UpdateCompanyComissionIndividualId(companyComissionAgent, IndividualId, AgencyId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Elimina las comisiones a los agentes y personas
        /// </summary>
        /// <param name="companyComissionAgent">model de comisiones</param>
        /// <param name="IndividualId">Codigo de la persona</param>
        /// <param name="AgencyId">Codigo de la agencia</param>
        /// <returns></returns>
        public CompanyComissionAgent DeleteCompanycommissionAgent(CompanyComissionAgent companyComissionAgent)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.DeleteCompanyComissionIndividualId(companyComissionAgent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyAgency> GetCompanyAgenciesByAgentIdDescription(int agentId, string description)
        {
            try
            {
                CompanyAgentBusiness agentBusiness = new CompanyAgentBusiness();
                return agentBusiness.GetCompanyAgenciesByAgentIdDescription(agentId, description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyAgency> GetCompanyAgenciesByAgentId(int agentId)
        {
            try
            {
                CompanyAgentBusiness agentBusiness = new CompanyAgentBusiness();
                return agentBusiness.GetCompanyAgenciesByAgentId(agentId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Agent V1

        #region Insured V1
        /// <summary>
        /// crea un Asegurado
        /// </summary>
        /// <param name="companyInsured"></param>
        /// <returns></returns>
        public Models.CompanyInsured CreateCompanyInsured(CompanyInsured companyInsured)
        {
            try
            {
                CompanyInsured resultCompanyInsured = new CompanyInsured();
                PersonDAO personDAO = new PersonDAO();

                bool validateRolInsured = GetCompanyIndividualRoleByIndividualId(companyInsured.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_INSURED)));
                if (!validateRolInsured)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyInsured.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_INSURED)));
                }
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                resultCompanyInsured = companyInsuredBusiness.CreateCompanyInsured(companyInsured);

                #region InsuredControl
                InsuredControl insuredControl = ModelAssembler.CreateInsuredControl(resultCompanyInsured);
                insuredControl.Action = "I";
                personDAO.CreateInsuredControl(insuredControl);
                #endregion

                return resultCompanyInsured;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Trae Asegurado por IndividualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.CompanyInsured GetCompanyInsuredByIndividualId(int individualId)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                return companyInsuredBusiness.GetCompanyInsuredByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Trae Asegurado por IndividualId para Facturacion electronica
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public Models.CompanyInsured GetCompanyInsuredElectronicBillingByIndividualId(int individualId)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                return companyInsuredBusiness.GetCompanyInsuredElectronicBillingByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualiza el asegurado
        /// </summary>
        /// <param name="companyInsured"></param>
        /// <returns></returns>
        public CompanyInsured UpdateCompanyInsured(CompanyInsured companyInsured)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                CompanyInsured resultCompanyInsured = new CompanyInsured();
                PersonDAO personDAO = new PersonDAO();
                bool validateRolInsured = GetCompanyIndividualRoleByIndividualId(companyInsured.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_INSURED)));
                if (!validateRolInsured)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyInsured.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_INSURED)));
                }
                resultCompanyInsured = companyInsuredBusiness.UpdateCompanyInsured(companyInsured);

                #region InsuredControl
                InsuredControl insuredControl = ModelAssembler.CreateInsuredControl(resultCompanyInsured);
                insuredControl.Action = "U";
                personDAO.CreateInsuredControl(insuredControl);
                #endregion

                return resultCompanyInsured;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualiza el asegurado
        /// </summary>
        /// <param name="companyInsured"></param>
        /// <returns></returns>
        public CompanyInsured UpdateCompanyInsuredElectronicBilling(CompanyInsured companyInsured)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                CompanyInsured resultCompanyInsured = new CompanyInsured();
                PersonDAO personDAO = new PersonDAO();
                
                resultCompanyInsured = companyInsuredBusiness.UpdateCompanyInsuredElectronicBilling(companyInsured);
                return resultCompanyInsured;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyInsuredConcept CreateCompanyInsuredConcept(CompanyInsuredConcept insuredConcept)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                return companyInsuredBusiness.CreateCompanyInsuredConcept(insuredConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyInsuredConcept UpdateCompanyInsuredConcept(CompanyInsuredConcept insuredConcept)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredBusiness = new CompanyInsuredBusiness();
                return companyInsuredBusiness.UpdateCompanyInsuredConcept(insuredConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyInsuredAgent CreateCompanyInsuredAgent(CompanyAgency insuredAgent, int individualId)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredAgentBusiness = new CompanyInsuredBusiness();
                return companyInsuredAgentBusiness.CreateCompanyInsuredAgent(insuredAgent, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyInsuredAgent UpdateCompanyInsuredAgent(CompanyAgency insuredAgent, int individualId)
        {
            try
            {
                CompanyInsuredBusiness companyInsuredAgentBusiness = new CompanyInsuredBusiness();
                return companyInsuredAgentBusiness.UpdateCompanyInsuredAgent(insuredAgent, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Insured V1

        #region Sarlaft V1
        /// <summary>
        /// Obtiene el sarlaft por idSarlaft
        /// </summary>
        /// <param name="sarlaftId">Codigó del sarlaft</param>
        /// <returns></returns>
        public List<IndividualSarlaft> GetSarlaftByNumberSarlaft(int sarlaftId)
        {
            CompanySarlaftIndividualBusiness companySarlaftBusiness = new CompanySarlaftIndividualBusiness();
            return companySarlaftBusiness.GetIndividualSarlaftByIndividualId(sarlaftId);
        }
        /// <summary>
        /// Crea el sarlaft 
        /// </summary>
        /// <param name="financialSarlaf">Obtiene el objeto sarlaft</param>
        /// <returns></returns>
        public IndividualSarlaft CreateSarlaftByNumberSarlaft(IndividualSarlaft financialSarlaf)
        {
            CompanySarlaftIndividualBusiness companySarlaftBusiness = new CompanySarlaftIndividualBusiness();
            return companySarlaftBusiness.CreateIndividualSarlaft(financialSarlaf, financialSarlaf.IndividualId, financialSarlaf.EconomicActivity.Id);
        }
        public IndividualSarlaft UpdateSarlaftByNumberSarlaft(IndividualSarlaft financialSarlaf)
        {
            CompanySarlaftIndividualBusiness companySarlaftBusiness = new CompanySarlaftIndividualBusiness();
            return companySarlaftBusiness.UpdateIndividualSarlaft(financialSarlaf);
        }
        #endregion

        #region individualTax
        /// <summary>
        /// Obtiene el impuesto individual por lista
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyIndividualTax> GetCompanyIndividualTaxExeptionByIndividualId(int individualId)
        {
            try
            {
                TaxCompanyBusiness taxCompanyBusiness = new TaxCompanyBusiness();
                return taxCompanyBusiness.GetCompanyIndividualTaxExeptionByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// CreateCompanyIndividualTax
        /// </summary>
        /// <param name="reinsurer"></param>
        /// <returns></returns>
        public CompanyIndividualTax CreateCompanyIndividualTax(CompanyIndividualTax individualTax)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTax = new TaxCompanyBusiness();
                CompanyIndividualTax companyIndividualTa = companyIndividualTax.CreateCompanyIndividualTax(individualTax);
                if (individualTax != null && individualTax.IndividualId != null)
                    CreateIntegrationNotification(individualTax.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.TAXES);
                return companyIndividualTa;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyIndividualTaxExeption CreateCompanyIndividualTaxEx(CompanyIndividualTax individualTax)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTax = new TaxCompanyBusiness();
                CompanyIndividualTaxExeption companyIndividualTaxExeption = companyIndividualTax.CreateCompanyIndividualTaxEx(individualTax);
                if (individualTax != null && individualTax.IndividualId != null)
                    CreateIntegrationNotification(individualTax.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.TAXES);
                return companyIndividualTaxExeption;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// CreatecompanyIndividualTaxExeption
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public CompanyIndividualTaxExeption CreateCompanyIndividualTaxExeption(CompanyIndividualTaxExeption individualTaxExeption)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTaxExeption = new TaxCompanyBusiness();
                return companyIndividualTaxExeption.CreateCompanyIndividualTaxExeption(individualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// UpdateCompanyIndividualTaxExeption
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public CompanyIndividualTax UpdateCompanyIndividualTax(CompanyIndividualTax individualTaxExeption)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTaxExeption = new TaxCompanyBusiness();
                return companyIndividualTaxExeption.UpdateCompanyIndividualTax(individualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="individualTaxExeption"></param>
        /// <returns></returns>
        public CompanyIndividualTaxExeption UpdateCompanyIndividualTaxExeption(CompanyIndividualTaxExeption individualTaxExeption)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTaxExeption = new TaxCompanyBusiness();
                return companyIndividualTaxExeption.UpdateCompanyIndividualTaxExeption(individualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="OperatingQuota"></param>
        /// <returns></returns>
        public void DeleteCompanyIndividualTax(CompanyIndividualTaxExeption individualTaxExeption)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTaxExeption = new TaxCompanyBusiness();
                companyIndividualTaxExeption.DeleteCompanyIndividualTax(individualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void DeleteCompanyIndividualTaxExeption(CompanyIndividualTax individualTaxExeption)
        {
            try
            {
                TaxCompanyBusiness companyIndividualTax = new TaxCompanyBusiness();
                companyIndividualTax.DeleteCompanyIndividualTaxExeption(individualTaxExeption);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion IndividualTax

        #region OperatingQuota V1

        /// <summary>
        /// Obtiene el cupo operativo del tomador
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List OperatingQuota</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.CompanyOperatingQuota> GetCompanyOperatingQuotaByIndividualId(int individualId)
        {
            try
            {
                CompanyOperatingQuotaBusiness companyOperatingQuotaBusiness = new CompanyOperatingQuotaBusiness();
                return companyOperatingQuotaBusiness.GetCompanyOperatingQuotaByIndividualId(individualId);
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
        public List<Models.CompanyOperatingQuota> CreateCompanyOperatingQuota(List<Models.CompanyOperatingQuota> listOperatingQuota)
        {
            try
            {
                CompanyOperatingQuotaBusiness companyOperatingQuotaBusiness = new CompanyOperatingQuotaBusiness();
                List<Models.CompanyOperatingQuota> CompanyOperatingQuotas = companyOperatingQuotaBusiness.CreateCompanyOperatingQuota(listOperatingQuota);
                if (listOperatingQuota != null && listOperatingQuota[0] != null && listOperatingQuota[0].IndividualId != null)
                    CreateIntegrationNotification(listOperatingQuota[0].IndividualId, (int)UniquePersonService.Enums.Peripheraltype.OPER_QUOT);
                return CompanyOperatingQuotas;
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
        public Models.CompanyOperatingQuota UpdateCompanyOperatingQuota(Models.CompanyOperatingQuota OperatingQuota)
        {
            try
            {
                CompanyOperatingQuotaBusiness companyOperatingQuotaBusiness = new CompanyOperatingQuotaBusiness();
                return companyOperatingQuotaBusiness.UpdateCompanyOperatingQuota(OperatingQuota);
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
        public bool DeleteCompanyOperatingQuota(Models.CompanyOperatingQuota OperatingQuota)
        {
            try
            {
                CompanyOperatingQuotaBusiness companyOperatingQuotaBusiness = new CompanyOperatingQuotaBusiness();
                return companyOperatingQuotaBusiness.DeleteCompanyOperatingQuota(OperatingQuota);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion OperatingQuota V1

        #region Company V1
        public List<CompanyCompany> GetCompanyCompanyByDocument(CustomerType customerType, string documentNumber)
        {
            try
            {
                CompanyBusiness companyBusiness = new CompanyBusiness();
                return companyBusiness.GetCompanyCompanyByDocument(customerType, documentNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear una nueva compañia
        /// </summary>
        /// <param name="company">Datos compañia</param>
        /// <returns></returns>
        public Models.CompanyCompany CreateCompanyCompany(Models.CompanyCompany company)
        {
            try
            {
                CompanyBusiness companyBusiness = new CompanyBusiness();
                CompanyCompany companyCompany = new CompanyCompany();
                PersonDAO personDAO = new PersonDAO();
                companyCompany = companyBusiness.CreateCompanyCompany(company);
                #region Control                                 
                IndividualControl individualControl = new IndividualControl();
                individualControl.IndividualId = companyCompany.IndividualId;
                individualControl.Action = "I";
                personDAO.CreateIndividualControl(individualControl);

                if (companyCompany.Consortiums.Count > 0)
                {
                    InsuredControl insuredControl = new InsuredControl();
                    insuredControl.IndividualId = companyCompany.Insured.IndividualId;
                    insuredControl.InsuredCode = companyCompany.Insured.InsuredCode;
                    insuredControl.Action = "I";
                    personDAO.CreateInsuredControl(insuredControl);

                }
                #endregion
                return companyCompany;
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
        /// <returns></returns>
        public Models.CompanyCompany UpdateCompanyCompany(Models.CompanyCompany company)
        {
            try
            {
                CompanyBusiness companyBusiness = new CompanyBusiness();
                CompanyCompany companyCompany = new CompanyCompany();
                PersonDAO personDAO = new PersonDAO();
                companyCompany = companyBusiness.UpdateCompanyCompany(company);
                #region Control                 
                IndividualControl individualControl = new IndividualControl();
                individualControl.IndividualId = companyCompany.IndividualId;
                individualControl.Action = "U";
                personDAO.CreateIndividualControl(individualControl);

                if (companyCompany.Consortiums.Count > 0)
                {
                    InsuredControl insuredControl = new InsuredControl();
                    insuredControl.IndividualId = companyCompany.Insured.IndividualId;
                    insuredControl.InsuredCode = companyCompany.Insured.InsuredCode;
                    insuredControl.Action = "U";
                    personDAO.CreateInsuredControl(insuredControl);
                }
                #endregion
                return companyCompany;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.CompanyCompany UpdateApplicationCompanyBasicInfo(Models.CompanyCompany company, bool validatePolicies = true)
        {
            try
            {
                CompanyBusiness companyBusiness = new CompanyBusiness();
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies)
                {
                    infringementPolicies = ValidateAuthorizationPoliciesPersonBasicInfo(null, company);
                }

                if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                    {
                        company.OperationId = this.CreateCompanyPersonOperation(
                                new CompanyPersonOperation
                                {
                                    IndividualId = company.IndividualId,
                                    Operation = JsonConvert.SerializeObject(company),
                                    ProcessType = "Update Company Basic Info",
                                    FunctionId = (int)ENUMCOAP.TypeFunction.PersonBasicInfo,
                                    Process = ENUMCOAP.TypeFunction.PersonBasicInfo.ToString()
                                }
                        ).OperationId;
                    }
                }
                else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
                {
                    List<CompanyAgency> companyAgencies = GetCompanyAgencyByInvidualId(company.IndividualId);
                    CompanyAgency newCompanyAgency = companyAgencies.Where(x => x.Id == 1).FirstOrDefault();
                    if (newCompanyAgency != null)
                    {
                        newCompanyAgency.FullName = newCompanyAgency.Code + " - " + company.FullName;
                        UpdateCompanyAgencyByInvidualId(newCompanyAgency, company.IndividualId);
                    }
                    company = companyBusiness.UpdateCompanyCompanyBasicInfo(company);
                    CreateIntegrationNotification(company.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.EDI_INF_BASIC);
                }
                company.InfringementPolicies = infringementPolicies == null ? new List<PoliciesAut>() : infringementPolicies; ;
                return company;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Company

        #region Supplier v1



        /// <summary>
        /// Obtener todos los SupplierAccountingConcept por Supplier
        /// </summary> 
        public List<Models.CompanySupplierAccountingConcept> GetCompanySupplierAccountingConceptsBySupplierId(int SupplierId)
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanySupplierAccountingConceptsBySupplierId(SupplierId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener todos los AccountingConcept
        /// </summary> 
        public List<Models.CompanyAccountingConcept> GetCompanyAccountingConcepts()
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanyAccountingConcepts();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// GetCompanySupplierProfiles
        /// </summary>        
        public List<Models.CompanySupplierProfile> GetCompanySupplierProfiles(int suppilierTypeId)
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanySupplierProfiles(suppilierTypeId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener  proveedor por id
        /// </summary>        
        public Models.CompanySupplier GetCompanySupplierById(int SupplierId)
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanySupplierById(SupplierId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>        
        public List<Models.CompanySupplier> GetCompanySuppliers()
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanySuppliers();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de tipos proveedores
        /// </summary>        
        public List<Models.CompanySupplierType> GetCompanySupplierTypes()
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanySupplierTypes();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener lista de proveedores declinados
        /// </summary>        
        public List<Models.CompanySupplierDeclinedType> GetCompanySupplierDeclinedTypes()
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanySupplierDeclinedType();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener grupo de proveedores
        /// </summary> 
        public List<Models.CompanyGroupSupplier> GetCompanyGroupSupplier()
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                return companySupplierBusiness.GetCompanyGroupSupplier();

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Partner V1
        public CompanyPartner GetCompanyPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int individualId)
        {
            try
            {
                CompanyPartnerBusiness partnerBusiness = new CompanyPartnerBusiness();
                return partnerBusiness.GetCompanyPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPartner> GetCompanyPartnerByIndividualId(int individualId)
        {
            try
            {
                CompanyPartnerBusiness partnerBusiness = new CompanyPartnerBusiness();
                return partnerBusiness.GetCompanyPartnerByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPartner CreateCompanyPartner(CompanyPartner partner)
        {
            try
            {
                CompanyPartnerBusiness partnerBusiness = new CompanyPartnerBusiness();
                return partnerBusiness.CreateCompanyPartner(partner);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyPartner UpdateCompanyPartner(CompanyPartner partner)
        {
            try
            {
                CompanyPartnerBusiness partnerBusiness = new CompanyPartnerBusiness();
                return partnerBusiness.UpdateCompanyPartner(partner);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        #endregion Partner V1

        /// <summary>
        /// Crear proveedor
        /// </summary>        
        public Models.CompanySupplier CreateCompanySupplier(Models.CompanySupplier companySupplier)
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                SupplierControl supplierControl = new SupplierControl();
                CompanySupplier result = new CompanySupplier();
                PersonDAO personDAO = new PersonDAO();
                bool validateRolSupplier = GetCompanyIndividualRoleByIndividualId(companySupplier.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_SUPPLIER)));
                if (!validateRolSupplier)
                {
                    CreateCompanyIndividualRoleByIndividualId(companySupplier.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_SUPPLIER)));
                }
                result = companySupplierBusiness.CreateCompanySupplier(companySupplier);
                #region Control                
                supplierControl.SupplierCode = result.Id;
                supplierControl.Action = "I";
                personDAO.CreateSupplierControl(supplierControl);
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Actualizar Proveedor
        /// </summary>        
        public Models.CompanySupplier UpdateCompanySupplier(Models.CompanySupplier companySupplier)
        {
            try
            {
                CompanySupplierBusiness companySupplierBusiness = new CompanySupplierBusiness();
                SupplierControl supplierControl = new SupplierControl();
                CompanySupplier result = new CompanySupplier();
                PersonDAO personDAO = new PersonDAO();
                result = companySupplierBusiness.UpdateCompanySupplier(companySupplier);
                #region Control
                supplierControl.SupplierCode = result.Id;
                supplierControl.Action = "U";
                personDAO.CreateSupplierControl(supplierControl);
                #endregion
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Get proveedor
        /// </summary>        
        public Models.CompanySupplier GetCompanySupplierByIndividualId(int individualId)
        {
            try
            {
                CompanySupplierBusiness supplierBusiness = new CompanySupplierBusiness();
                return supplierBusiness.GetCompanySupplierByIndividualId(individualId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #endregion Supplier v1

        #region IndividualRole V1

        /// <summary>
        /// Obtiene los roles de un individuo
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns>List IndividualRole</returns>
        /// <exception cref="BusinessException"></exception>
        public List<Models.CompanyIndividualRole> GetCompanyIndividualRoleByIndividualId(int individualId)
        {
            try
            {
                CompanyIndividualRoleBusiness individualRoleBusiness = new CompanyIndividualRoleBusiness();
                return individualRoleBusiness.GetCompanyIndividualRoleByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public void CreateCompanyIndividualRoleByIndividualId(int individualId, int roleId)
        {
            try
            {

                CompanyIndividualRoleBusiness individualRoleBusiness = new CompanyIndividualRoleBusiness();
                individualRoleBusiness.CretateCompanyIndividualRoleByIndividualId(new CompanyIndividualRole { IndividualId = individualId, RoleId = roleId });
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion IndividualRole V1

        #region Coinsured
        public CompanyCoInsured GetCoInsuredIndividuald(int individual)
        {
            try
            {
                CompanyCoInsuredBusiness companyCoInsuredBusiness = new CompanyCoInsuredBusiness();
                return companyCoInsuredBusiness.GetCompanyCoInsured(individual);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyCoInsured CreateCoInsuredIndividuald(CompanyCoInsured companyCoInsured)
        {
            try
            {
                CompanyCoInsured resultCompanyCoInsured = new CompanyCoInsured();
                PersonDAO personDAO = new PersonDAO();

                bool validateRoleCoInsured = GetCompanyIndividualRoleByIndividualId(companyCoInsured.IndividualId).Exists(x => x.RoleId ==
               Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_CONINSURED)));
                if (!validateRoleCoInsured)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyCoInsured.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_CONINSURED)));
                }
                CompanyCoInsuredBusiness companyCoInsuredBusiness = new CompanyCoInsuredBusiness();
                resultCompanyCoInsured = companyCoInsuredBusiness.CreateCompanyCoInsured(companyCoInsured);

                #region Control 
                InsuranceCompanyControl insuranceCompanyControl = new InsuranceCompanyControl();
                insuranceCompanyControl.InsuranceCompanyId = Convert.ToInt32(resultCompanyCoInsured.InsuraceCompanyId);
                insuranceCompanyControl.Action = "I";
                personDAO.CreateCoInsuranceCompanyControl(insuranceCompanyControl);
                #endregion
                return resultCompanyCoInsured;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CompanyCoInsured UpdateCoInsuredIndividuald(CompanyCoInsured companyCoInsured)
        {
            try
            {
                CompanyCoInsuredBusiness companyCoInsuredBusiness = new CompanyCoInsuredBusiness();
                CompanyCoInsured resultCompanyCoInsured = new CompanyCoInsured();
                PersonDAO personDAO = new PersonDAO();
                resultCompanyCoInsured = companyCoInsuredBusiness.UpdateCompanyCoInsured(companyCoInsured);
                #region Control 
                InsuranceCompanyControl insuranceCompanyControl = new InsuranceCompanyControl();
                insuranceCompanyControl.InsuranceCompanyId = Convert.ToInt32(resultCompanyCoInsured.InsuraceCompanyId);
                insuranceCompanyControl.Action = "U";
                personDAO.CreateCoInsuranceCompanyControl(insuranceCompanyControl);
                #endregion
                return resultCompanyCoInsured;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Company
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
        public new List<Models.CompanyCompany> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType)
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

        /// <summary>
        /// Obtener compañia por identificador
        /// </summary>
        /// <param name="individualId">identificador</param>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public new Models.CompanyCompany GetCompanyByIndividualId(int individualId)
        {
            try
            {
                CompanyBusiness companyBss = new CompanyBusiness();
                return companyBss.GetCompanyByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }



        #endregion

        #region Sarlaft
        /// <summary>
        /// Guardar la informacion sarlaft en la talbla UP.FINANCIAL_SARLAFT
        /// </summary>
        /// <param name="sarlaft">modelo sarlaft</param>
        /// <returns>Models.FinancialSarlaf</returns>
        /// <exception cref="BusinessException"></exception>
        public virtual Models.FinancialSarlaf CreateCompanyFinancialSarlaft(Models.FinancialSarlaf sarlaft)
        {
            try
            {
                SarlaftDAO sarlaftProvider = new SarlaftDAO();
                return sarlaftProvider.CreateFinancialSarlaft(sarlaft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Guardar la informacion sarlaft de la persona en la tabla UP.INDIVIDUAL_SARLAFT
        /// </summary>
        /// <param name="individualSarlaft">Models.IndividualSarlaft.</param>
        /// <param name="individualId">Id Individuo</param>
        /// <param name="economiActivity">Actividad Economica</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public Models.IndividualSarlaft CreateCompanyIndividualSarlaft(Models.IndividualSarlaft individualSarlaft, int individualId, int economicActivity)
        {
            try
            {
                IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
                return individualSarlaftDAO.CreateIndividualSarlaft(individualSarlaft, individualId, economicActivity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Obtener Sarlaft
        /// </summary>
        /// <param name="sarlaftId">Id Sarlaft</param>
        /// <returns>Models.FinancialSarlaf</returns>
        /// <exception cref="BusinessException">Error Obteniendo GetFinancialSarlaftBySarlaftId</exception>
        public Models.FinancialSarlaf GetCompanyFinancialSarlaftBySarlaftId(int sarlaftId)
        {
            try
            {
                SarlaftDAO sarlaftProvider = new SarlaftDAO();
                return sarlaftProvider.GetFinancialSarlaftBySarlaftId(sarlaftId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        /// <summary>
        /// Actualizar Sarlaft
        /// </summary>
        /// <param name="sarlaftId">Id Sarlaft</param>
        /// <returns>Models.FinancialSarlaf</returns>
        /// <exception cref="BusinessException">Error Obteniendo GetFinancialSarlaftBySarlaftId</exception>
        public Models.FinancialSarlaf UpdateFinancialSarlaft(Models.FinancialSarlaf sarlaft)
        {
            try
            {
                SarlaftDAO sarlaftProvider = new SarlaftDAO();
                return sarlaftProvider.UpdateFinancialSarlaf(sarlaft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #endregion

        #region consorcion
        public List<Models.CompanyConsortium> GetCoConsortiumsByInsuredCode(int insuredCode)
        {
            try
            {
                ConsortiumDAO coConsortium = new ConsortiumDAO();
                return coConsortium.GetCoConsortiumsByInsuredCode(insuredCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        /// <summary>
        /// Buscar persona por número de documento o apellidos y nombres
        /// </summary>
        /// <param name="documentNumber">número de documento</param>
        /// <param name="surname">primer apellido</param>
        /// <param name="motherLastName">segundo apellido</param>
        /// <param name="name">nombres</param>
        /// <param name="searchType">tipo de busqueda</param>
        /// <returns></returns>
        public new List<Models.CompanyPerson> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType)
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
        /// Gets the individual sarlaft by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public List<Models.IndividualSarlaft> GetIndividualSarlaftByIndividualId(int individualId)
        {
            try
            {
                IndividualSarlaftDAO dao = new IndividualSarlaftDAO();
                return dao.GetIndividualSarlaftByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.IndividualSarlaft CreateIndividualSarlaftByIndividualId(Models.IndividualSarlaft individualSarlaft, int individualId, int IdActivityEconomic)
        {
            try
            {
                IndividualSarlaftDAO dao = new IndividualSarlaftDAO();
                return dao.CreateIndividualSarlaft(individualSarlaft, individualId, IdActivityEconomic);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.FinancialSarlaf UpdateIndividualSarlaftByIndividualId(Models.IndividualSarlaft individualSarlaft)
        {
            try
            {
                IndividualSarlaftDAO dao = new IndividualSarlaftDAO();
                return dao.UpdateIndividualSarlaft(individualSarlaft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.IndividualSarlaft UpdateIndividualSarlaftByIndividualIds(Models.IndividualSarlaft individualSarlaft)
        {
            try
            {
                IndividualSarlaftDAO dao = new IndividualSarlaftDAO();
                return dao.UpdateIndividualSarlafts(individualSarlaft);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<models.CompanyName> CompanyGetNotificationAddressesByIndividualId(int individualId, CustomerType customerType)
        {
            try
            {
                PersonBusiness businessPerson = new PersonBusiness();
                return businessPerson.CompanyGetNotificationAddressesByIndividualId(individualId, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<models.CompanyName> CompanyGetNotificationByIndividualId(int individualId, CustomerType customerType)
        {
            try
            {
                PersonBusiness businessPerson = new PersonBusiness();
                return businessPerson.CompanyGetNotificationAddressesByIndividualId(individualId, customerType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Obtener compañia por numero de documento
        /// </summary>
        /// <param name="individualId">numero de documento</param>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.CompanyCompany GetCompanyByDocumentNumber(string documentNumber)
        {
            try
            {
                CompanyDAO companyDAO = new CompanyDAO();
                return companyDAO.GetCompanyByDocumentNumber(documentNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener persona por numero de documento
        /// </summary>
        /// <param name="documentNumber">numero de documento</param>
        /// <returns>Models.Company</returns>
        /// <exception cref="BusinessException"></exception>
        public Models.CompanyPerson GetPersonByDocumentNumber(string documentNumber)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.GetPersonByDocumentNumber(documentNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public CoInsurerCompany GetCoInsurerByIndividualId(int individualId)
        {
            try
            {
                CoInsurerDAO personModel = new CoInsurerDAO();
                return personModel.GetCoInsurerByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CoInsurerCompany CreateCoInsurer(CoInsurerCompany CoInsurer)
        {
            try
            {
                CoInsurerDAO personModel = new CoInsurerDAO();
                return personModel.CreateInsurerByExistingCompany(CoInsurer);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Extensión CompanyInsured

        public Models.CompanyInsured GetCompanyInsuredByIndividualCode(int individualCode)
        {
            PersonBusiness bis = new PersonBusiness();
            return bis.GetCompanyInsuredByIndividualCode(individualCode);
        }

        //public List<Models.CompanyInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        //{
        //    PersonBusiness bis = new PersonBusiness();
        //    return bis.GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
        //}

        //public List<Models.CompanyInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType, TemporalType temporalType)
        //{
        //    List<CompanyInsured> insureds = GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
        //    insureds.ForEach(x => x.Name = (x.Surname + " " + (string.IsNullOrEmpty(x.SecondSurname) ? "" : x.SecondSurname + " ") + x.Name));

        //    if (insureds.Count == 1 && customerType == CustomerType.Individual && temporalType != TemporalType.Quotation)
        //    {
        //        if (insureds[0].InsuredId == 0)
        //        {
        //            throw new Exception(Errors.ErrorInsuredWithoutRol);
        //        }
        //        else if (insureds[0].DeclinedDate > DateTime.MinValue)
        //        {
        //            throw new Exception(Errors.ErrorInsuredDisabled);
        //        }
        //        else if (insureds[0].CompanyName == null)
        //        {
        //            throw new Exception(Errors.ErrorInsuredWithoutAddress);
        //        }
        //        else
        //        {
        //            return insureds;
        //        }
        //    }
        //    else
        //    {
        //        insureds.Where(x => x.CustomerType == CustomerType.Individual).ToList().ForEach(x => x.CustomerTypeDescription = Errors.Individual);
        //        insureds.Where(x => x.CustomerType == CustomerType.Prospect).ToList().ForEach(x => x.CustomerTypeDescription = Errors.Prospect);

        //        return insureds;
        //    }

        //}

        public Models.CompanyInsured GetCompanyInsuredByInsuredCode(int insuredCode)
        {
            PersonBusiness bis = new PersonBusiness();
            return bis.GetCompanyInsuredByInsuredCode(insuredCode);
        }

        #endregion

        #region Company.ScoreTypeDoc

        /// <summary>
        /// Realiza las operaciones CRUD para el tipo de documento datacrédito
        /// </summary>
        /// <param name="listAdded">tipos de documento datacrédito para agregar</param>
        /// <param name="listEdited">tipos de documento datacrédito para editar</param>
        /// <param name="listDeleted">tipos de documento datacrédito para eliminar</param>
        /// <returns>Resumen de las operaciones</returns>
        public ParametrizationResponse<ScoreTypeDoc> CreateScoreTypeDocs(List<ScoreTypeDoc> listAdded, List<ScoreTypeDoc> listEdited, List<ScoreTypeDoc> listDeleted)
        {
            try
            {
                ScoreTypeDocDAO scoreTypeDoc = new ScoreTypeDocDAO();
                return scoreTypeDoc.SaveScoreTypeDocs(listAdded, listEdited, listDeleted);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Resources.Errors.ErrorCRUDProcessesForScoreTypeDocs), ex);
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de documentos datacrédito
        /// </summary>
        /// <returns>Tipos de documento datacrédito</returns>
        public List<Models.ScoreTypeDoc> GetAllScoreTypeDoc()
        {
            try
            {
                ScoreTypeDocDAO scoreTypeDocDAO = new ScoreTypeDocDAO();
                return scoreTypeDocDAO.GetAllScoreTypeDoc();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        //pendiente
        /// <summary>
        /// Genera archivo excel para tipo de documento datacrédito
        /// </summary>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToScoreTypeDoc()
        {
            try
            {

                CompanyFileDAO fileDAO = new CompanyFileDAO();
                return fileDAO.GenerateFileToScoreTypeDoc(Errors.FileScoreTypeDocName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFileToScoreTypeDoc), ex);
            }
        }


        #endregion

        #region LabourPerson

        /// <summary>
        /// Guardar la informacion laboral de la persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        ///// <returns></returns
        public CompanyLabourPerson CreateCompanyLabourPerson(CompanyLabourPerson personJob, int individualId)
        {
            try
            {
                CompanyLabourPersonBusiness LabourPersonBusiness = new CompanyLabourPersonBusiness();
                CompanyLabourPerson companyLabourPerson = LabourPersonBusiness.CreateLabourPerson(personJob, individualId);
                if (personJob != null && personJob.IndividualId != null)
                    CreateIntegrationNotification(personJob.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.INF_LAB);
                return companyLabourPerson;
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
        public CompanyLabourPerson UpdateCompanyLabourPerson(CompanyLabourPerson personJob)
        {
            try
            {
                CompanyLabourPersonBusiness LabourPersonBusiness = new CompanyLabourPersonBusiness();
                CompanyLabourPerson companyLabourPerson = LabourPersonBusiness.UpdateLabourPerson(personJob);
                if (personJob != null && personJob.IndividualId != null)
                    CreateIntegrationNotification(personJob.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.INF_LAB);
                return companyLabourPerson;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crea la notificacion para el proceso de integracion por periferico.
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <param name="PerifericoId"></param>
        public void CreateIntegrationNotification(int IndividualId, int PerifericoId)
        {
            #region Control 
            try
            {
                CoIndividualControl CoindividualControl = new CoIndividualControl();
                PersonDAO personDAO = new PersonDAO();
                CoindividualControl.IndividualId = IndividualId;
                CoindividualControl.PerifericoId = PerifericoId;
                personDAO.CreateCoIndividualControl(CoindividualControl);
            }
            catch (Exception ex) { }
            #endregion
        }

        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns
        public CompanyLabourPerson GetCompanyLabourPersonByIndividualId(int individualId)
        {
            try
            {
                CompanyLabourPersonBusiness LabourPersonBusiness = new CompanyLabourPersonBusiness();
                return LabourPersonBusiness.GetLabourPersonByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region Paymentmethod
        public List<CompanyIndividualPaymentMethod> GetIndividualpaymentMethodByIndividualId(int individualId)
        {
            CompanyIndividualPaymentMethodBusiness bis = new CompanyIndividualPaymentMethodBusiness();
            return bis.GetIndividualPaymentMethods(individualId);
        }

        public List<CompanyIndividualPaymentMethod> CreateIndividualpaymentMethods(List<CompanyIndividualPaymentMethod> companyIndividualPaymentMethods, int individualId)
        {
            CompanyIndividualPaymentMethodBusiness bis = new CompanyIndividualPaymentMethodBusiness();
            List<CompanyIndividualPaymentMethod> CompanyIndividualPaymentMethods = bis.CreateIndividualPaymentMethods(companyIndividualPaymentMethods, individualId);
            CreateIntegrationNotification(individualId, (int)UniquePersonService.Enums.Peripheraltype.PAY_MET);
            return CompanyIndividualPaymentMethods;
        }

        #endregion

        #region Leal Representative

        public CompanyLegalRepresentative CreateLegalRepresentative(CompanyLegalRepresentative legalRepresent, int individualId)
        {
            CompanyLealRepresentativeBusiness bis = new CompanyLealRepresentativeBusiness();
            return bis.CreateLegalRepresentative(legalRepresent, individualId);
        }

        public CompanyLegalRepresentative GetLegalRepresentativeByIndividualId(int individualId)
        {
            CompanyLealRepresentativeBusiness bis = new CompanyLealRepresentativeBusiness();
            return bis.GetLegalRepresentativeByIndividualId(individualId);
        }
        #endregion

        #region ProspectPersonNatural

        public CompanyProspectNatural GetProspectPersonNatural(int individualId)
        {
            CompanyProspectNaturalBusiness bis = new CompanyProspectNaturalBusiness();
            return bis.GetProspectPersonNatural(individualId);
        }

        public CompanyProspectNatural CreateProspectPersonNatural(CompanyProspectNatural prospectPersonNatural)
        {
            CompanyProspectNaturalBusiness bis = new CompanyProspectNaturalBusiness();
            return bis.CreateProspectPersonNatural(prospectPersonNatural);
        }

        public CompanyProspectNatural UpdateProspectPersonNatural(CompanyProspectNatural prospectPersonNatural)
        {
            CompanyProspectNaturalBusiness bis = new CompanyProspectNaturalBusiness();
            return bis.UpdateProspectPersonNatural(prospectPersonNatural);
        }

        public CompanyProspectNatural GetProspectByDocumentNumber(string documentNum, int searchType)
        {
            CompanyProspectNaturalBusiness bis = new CompanyProspectNaturalBusiness();
            return bis.GetProspectByDocumentNumber(documentNum, searchType);
        }

        public CompanyProspectNatural GetProspectNaturalByDocumentNumber(string documentNum)
        {
            CompanyProspectNaturalBusiness bis = new CompanyProspectNaturalBusiness();
            return bis.GetProspectNaturalByDocumentNumber(documentNum);
        }
        #endregion

        #region prospectLegal

        public CompanyProspectNatural GetProspectLegalByDocumentNumber(string documentNum)
        {
            CompanyProspectLegalBusiness bis = new CompanyProspectLegalBusiness();
            return bis.GetProspectLegalByDocumentNumber(documentNum);
        }

        public CompanyProspectNatural GetProspectPersonLegal(int individualId)
        {
            CompanyProspectLegalBusiness bis = new CompanyProspectLegalBusiness();
            return bis.GetProspectPersonLegal(individualId);
        }

        public CompanyProspectNatural CreateProspectPersonLegal(CompanyProspectNatural prospectNatural)
        {
            CompanyProspectLegalBusiness bis = new CompanyProspectLegalBusiness();
            return bis.CreateProspectPersonLegal(prospectNatural);
        }

        public CompanyProspectNatural UpdateProspectPersonLegal(CompanyProspectNatural prospectNatural)
        {
            CompanyProspectLegalBusiness bis = new CompanyProspectLegalBusiness();
            return bis.UpdateProspectPersonLegal(prospectNatural);
        }

        public CompanyProspectNatural GetCompanyProspectLegalAdv(CustomerType customerType, CompanyProspectNatural company)
        {
            try
            {
                CompanyProspectLegalBusiness companyBusiness = new CompanyProspectLegalBusiness();
                if (company.TributaryIdNumber != null)
                    return companyBusiness.GetProspectLegalByDocumentNumber(company.TributaryIdNumber);
                else
                    if (company.ProspectCode != null)
                    return companyBusiness.GetProspectPersonLegal(company.ProspectCode);

                return null;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public CompanyProspectNatural GetCompanyProspectNaturalAdv(CustomerType customerType, CompanyProspectNatural company)
        {
            try
            {
                CompanyProspectNaturalBusiness companyBusiness = new CompanyProspectNaturalBusiness();
                if (company.IdCardNo != null)
                    return companyBusiness.GetProspectNaturalByDocumentNumber(company.IdCardNo);
                else
                    if (company.ProspectCode != null)
                    return companyBusiness.GetProspectPersonNatural(company.ProspectCode);

                return null;

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion


        #region InsuredGuarantee

        public List<CompanyGuaranteeInsuredGuarantee> GetCompanyInsuredGuaranteesByIndividualId(int individualId)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.GetCompanyInsuredGuaranteesByIndividualId(individualId);
        }

        public CompanyInsuredGuaranteePromissoryNote GetCompanyInsuredGuaranteePromissoryNoteByIndividualIdById(int individualId, int id)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.GetCompanyInsuredGuaranteePromissoryNoteByIdByIndividualId(individualId, id);
        }

        public CompanyInsuredGuaranteePledge GetCompanyInsuredGuaranteePledgeByIndividualIdById(int individualId, int id)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.GetCompanyInsuredGuaranteePledgeByIdByIndividualId(individualId, id);
        }

        public CompanyInsuredGuaranteeMortgage GetCompanyInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.GetCompanyInsuredGuaranteeMortgageByIdByIndividualId(individualId, id);
        }

        public CompanyInsuredGuaranteePromissoryNote CreateCompanyInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.CreateCompanyInsuredGuaranteePromissoryNote(companyInsuredGuaranteePromissoryNote);
        }

        public CompanyInsuredGuaranteePledge CreateCompanyInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.CreateCompanyInsuredGuaranteePledge(companyInsuredGuaranteePledge);
        }

        public CompanyInsuredGuaranteeMortgage CreateCompanyCompanyInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.CreateCompanyCompanyInsuredGuaranteeMortgage(companyInsuredGuaranteeMortgage);
        }

        public CompanyInsuredGuaranteePromissoryNote UpdateCompanyInsuredGuaranteePromissoryNote(CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.UpdateCompanyInsuredGuaranteePromissoryNote(companyInsuredGuaranteePromissoryNote);
        }

        public CompanyInsuredGuaranteePledge UpdateCompanyInsuredGuaranteePledge(CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.UpdateCompanyInsuredGuaranteePledge(companyInsuredGuaranteePledge);
        }

        public CompanyInsuredGuaranteeMortgage UpdateCompanyCompanyInsuredGuaranteeMortgage(CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.UpdateCompanyCompanyInsuredGuaranteeMortgage(companyInsuredGuaranteeMortgage);
        }


        public CompanyInsuredGuaranteeFixedTermDeposit GetCompanyInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.GetCompanyInsuredGuaranteeFixedTermDepositByIdByIndividualId(individualId, id);
        }

        public CompanyInsuredGuaranteeFixedTermDeposit CreateCompanyInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit guaranteeFixedTermDeposit)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.CreateCompanyInsuredGuaranteeFixedTermDeposit(guaranteeFixedTermDeposit);
        }

        public CompanyInsuredGuaranteeFixedTermDeposit UpdateCompanyInsuredGuaranteeFixedTermDeposit(CompanyInsuredGuaranteeFixedTermDeposit guaranteeFixedTermDeposit)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.UpdateCompanyInsuredGuaranteeFixedTermDeposit(guaranteeFixedTermDeposit);
        }

        public CompanyInsuredGuaranteeOthers GetCompanyInsuredGuaranteeOthersByIndividualIdById(int individualId, int id)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.GetCompanyInsuredGuaranteeOthersByIndividualIdById(individualId, id);
        }

        public CompanyInsuredGuaranteeOthers CreateCompanyInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.CreateCompanyInsuredGuaranteeOthers(insuredGuaranteeOthers);
        }

        public CompanyInsuredGuaranteeOthers UpdateCompanyInsuredGuaranteeOthers(CompanyInsuredGuaranteeOthers insuredGuaranteeOthers)
        {
            CompanyInsuredGuaranteeBusiness bis = new CompanyInsuredGuaranteeBusiness();
            return bis.UpdateCompanyInsuredGuaranteeOthers(insuredGuaranteeOthers);
        }



        #endregion

        #region MaritalStatus
        public List<CompanyMaritalStatus> GetCompanyMaritalStatus()
        {
            try
            {
                CompanyMaritalStatusBusiness maritalStatusBusiness = new CompanyMaritalStatusBusiness();
                return maritalStatusBusiness.GetCompanyMaritalStatus();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region DocumentType
        public List<CompanyDocumentType> GetCompanyDocumentType(int typeDocument)
        {
            try
            {
                CompanyDocumentTypeBusiness documentTypeBusiness = new CompanyDocumentTypeBusiness();
                return documentTypeBusiness.GetCompanyDocumentTypes(typeDocument);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region AddressesType
        public List<CompanyAddressType> GetCompanyAddressesTypes()
        {
            try
            {
                CompanyAddressTypeBusiness addressTypeBusiness = new CompanyAddressTypeBusiness();
                return addressTypeBusiness.GetCompanyAddressesTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

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
        #endregion

        #region PhoneType
        public List<CompanyPhoneType> GetCompanyPhoneTypes()
        {
            try
            {
                CompanyPhoneTypeBusiness phoneTypeBusiness = new CompanyPhoneTypeBusiness();
                return phoneTypeBusiness.GetCompanyPhoneType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
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
        #endregion

        #region EmailType
        public List<CompanyEmailType> GetCompanyEmailTypes()
        {
            try
            {
                CompanyEmailTypesBusiness emailTypesBusiness = new CompanyEmailTypesBusiness();
                return emailTypesBusiness.GetCompanyEmailTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EconomicActivity
        public List<CompanyEconomicActivity> GetCompanyEconomicActivities()
        {
            try
            {
                CompanyEconomicActivityBusiness economicActivityBusiness = new CompanyEconomicActivityBusiness();
                return economicActivityBusiness.GetCompanyEconomicActivity();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region AssociationType
        public List<CompanyAssociationType> GetCompanyAssociationTypes()
        {
            try
            {
                CompanyAssociationTypeBusiness associationTypeBusiness = new CompanyAssociationTypeBusiness();
                return associationTypeBusiness.GetCompanyAssociationType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region CompanyType
        public List<CompanyCompanyType> GetCompanyCompanyType()
        {
            try
            {
                CompanyCompanyTypeBusiness companyTypeBusiness = new CompanyCompanyTypeBusiness();
                return companyTypeBusiness.GetCompanyCompanyType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region FiscalResponsibility
        public List<CompanyFiscalResponsibility> GetCompanyFiscalResponsibility()
        {
            try
            {
                CompanyFiscalResponsibilityBusiness companyFiscalResponsibility = new CompanyFiscalResponsibilityBusiness();
                return companyFiscalResponsibility.GetCompanyCompanyFiscalResponsibility();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteCompanyFiscalResponsibility(CompanyInsuredFiscalResponsibility companyFiscal)
        {
            try
            {
                CompanyFiscalResponsibilityBusiness coFiscal = new CompanyFiscalResponsibilityBusiness();
                return coFiscal.DeleteCompanyInsuredFiscalResponsibility(companyFiscal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        #endregion

        #region Consortium
        public List<Models.CompanyConsortium> GetCoConsortiumsByInsuredCod(int insuredCode)
        {
            try
            {
                CompanyConsotuimBusiness coConsortium = new CompanyConsotuimBusiness();
                return coConsortium.GetCompanyConsurtiumCode(insuredCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.CompanyConsortium CreateCompanyConsortium(Models.CompanyConsortium companyConsortium)
        {
            try
            {
                CompanyConsotuimBusiness coConsortium = new CompanyConsotuimBusiness();
                return coConsortium.CreateCompanyConsortium(companyConsortium);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.CompanyConsortium UpdateCompanyConsortium(Models.CompanyConsortium companyConsortium)
        {
            try
            {
                CompanyConsotuimBusiness coConsortium = new CompanyConsotuimBusiness();
                return coConsortium.UpdateCompanyConsortium(companyConsortium);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool DeleteCompanyConsortium(Models.CompanyConsortium companyConsortium)
        {
            try
            {
                CompanyConsotuimBusiness coConsortium = new CompanyConsotuimBusiness();
                return coConsortium.DeleteCompanyConsortuim(companyConsortium);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }

        }
        #endregion

        #region Insured
        public List<CompanyInsuredDeclinedType> GetCompanyInsuredDeclinedTypes()
        {
            try
            {
                CompanyInsuredDeclinedTypeBusiness companyInsuredDeclinedType = new CompanyInsuredDeclinedTypeBusiness();
                return companyInsuredDeclinedType.GetCompanyInsuredDeclinedType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.CompanyInsuredSegment> GetCompanyInsuredSegment()
        {
            try
            {
                CompanyInsuredBusiness insuredBusiness = new CompanyInsuredBusiness();
                return insuredBusiness.GetInsuredSegment();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.CompanyInsuredProfile> GetCompanyInsuredProfile()
        {
            try
            {
                CompanyInsuredBusiness insuredBusiness = new CompanyInsuredBusiness();
                return insuredBusiness.GetInsuredProfile();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region AgentType
        public List<CompanyAgentType> GetCompanyAgentTypes()
        {
            try
            {
                CompanyAgentTypeBusiness agentTypeBusiness = new CompanyAgentTypeBusiness();
                return agentTypeBusiness.GetCompanyAgentType();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region AgentDeclinedType
        public List<CompanyAgentDeclinedType> GetCompanyAgentDeclinedTypes()
        {
            try
            {
                CompanyAgentDeclinedTypeBusiness agentDeclinedType = new CompanyAgentDeclinedTypeBusiness();
                return agentDeclinedType.GetCompanyAgentDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region GroupAgent
        public List<CompanyGroupAgent> GetCompanyGroupAgent()
        {
            try
            {
                CompanyGroupAgentBusiness groupAgent = new CompanyGroupAgentBusiness();
                return groupAgent.GetCompanyGroupAgent();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region SalesChannel
        public List<CompanySalesChannel> GetCompanySalesChannel()
        {
            try
            {
                CompanySalesChannelBusiness salesChannel = new CompanySalesChannelBusiness();
                return salesChannel.GetCompanySalesChannel();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region EmployeePerson
        public List<CompanyEmployeePerson> GetCompanyEmployeePersons()
        {
            try
            {
                CompanyEmployeePersonBusiness employeePerson = new CompanyEmployeePersonBusiness();
                return employeePerson.GetCompanyEmployeePerson();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region AllOthersDeclinedType
        public List<CompanyAllOthersDeclinedType> GetCompanyAllOthersDeclinedTypes()
        {
            try
            {
                CompanyAllOthersDeclinedTypeBusiness allOthersDeclinedType = new CompanyAllOthersDeclinedTypeBusiness();
                return allOthersDeclinedType.GetCompanyAllOthersDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region InsuredGuaranteeLog
        public List<CompanyInsuredGuaranteeLog> GetCompanyInsuredGuaranteeLogsByindividualIdByguaranteeId(int individualId, int guaranteeId)
        {
            CompanyInsuredGuaranteeLogBusiness bis = new CompanyInsuredGuaranteeLogBusiness();
            return bis.GetCompanyInsuredGuaranteeLogsByindividualIdByguaranteeId(individualId, guaranteeId);
        }

        public CompanyInsuredGuaranteeLog CreateCompanyInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog)
        {
            CompanyInsuredGuaranteeLogBusiness bis = new CompanyInsuredGuaranteeLogBusiness();
            return bis.CreateCompanyInsuredGuaranteeLog(insuredGuaranteeLog);
        }

        public CompanyInsuredGuaranteeLog UpdateCompanyInsuredGuaranteeLog(CompanyInsuredGuaranteeLog insuredGuaranteeLog)
        {
            CompanyInsuredGuaranteeLogBusiness bis = new CompanyInsuredGuaranteeLogBusiness();
            return bis.UpdateCompanyInsuredGuaranteeLog(insuredGuaranteeLog);
        }

        #endregion

        #region InsuredGuaranteeDocumentation
        public CompanyInsuredGuaranteeDocumentation CreateCompanyInsuredGuaranteeDocumentation(CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation)
        {
            try
            {
                CompanyInsuredGuaranteeDocumentationBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteeDocumentationBusiness();
                return companyInsuredGuaranteeDocumentationBusiness.CreateCompanyInsuredGuaranteeDocumentation(companyInsuredGuaranteeDocumentation);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public CompanyInsuredGuaranteeDocumentation UpdateCompanyInsuredGuaranteeDocumentation(CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation)
        {
            try
            {
                CompanyInsuredGuaranteeDocumentationBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteeDocumentationBusiness();
                return companyInsuredGuaranteeDocumentationBusiness.UpdateCompanyInsuredGuaranteeDocumentation(companyInsuredGuaranteeDocumentation);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }


        public void DeleteCompanyInsuredGuaranteeDocumentation(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            try
            {
                CompanyInsuredGuaranteeDocumentationBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteeDocumentationBusiness();
                companyInsuredGuaranteeDocumentationBusiness.DeleteCompanyInsuredGuaranteeDocumentation(individualId, insuredguaranteeId, guaranteeId, documentId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }


        public CompanyInsuredGuaranteeDocumentation GetCompanyInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            try
            {
                CompanyInsuredGuaranteeDocumentationBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteeDocumentationBusiness();
                return companyInsuredGuaranteeDocumentationBusiness.GetCompanyInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(individualId, insuredguaranteeId, guaranteeId, documentId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredGuaranteeDocumentation> GetCompanyInsuredGuaranteeDocumentation()
        {
            try
            {
                CompanyInsuredGuaranteeDocumentationBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteeDocumentationBusiness();
                return companyInsuredGuaranteeDocumentationBusiness.GetCompanyInsuredGuaranteeDocumentation();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyInsuredGuaranteeDocumentation> GetCompanyInsuredGuaranteeDocument(int individualId, int guaranteeId)
        {
            try
            {
                CompanyInsuredGuaranteeDocumentationBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteeDocumentationBusiness();
                return companyInsuredGuaranteeDocumentationBusiness.GetCompanyInsuredGuaranteeDocument(individualId, guaranteeId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region GuaranteeRequiredDocument

        public List<CompanyGuaranteeRequiredDocument> GetCompanyInsuredGuaranteeRequiredDocumentation(int guaranteeId)
        {
            try
            {
                CompanyGuaranteeRequiredDocumentBusiness companyGuaranteeRequiredDocumentBusiness = new CompanyGuaranteeRequiredDocumentBusiness();
                return companyGuaranteeRequiredDocumentBusiness.GetCompanyInsuredGuaranteeRequiredDocumentation(guaranteeId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion

        #region Guarantor
        public List<CompanyGuarantor> GetCompanyGuarantorByindividualIdByguaranteeId(int individualId, int guaranteeId)
        {
            CompanyGuarantorBusiness bis = new CompanyGuarantorBusiness();
            return bis.GetCompanyGuarantorByindividualIdByguaranteeId(individualId, guaranteeId);
        }

        public CompanyGuarantor CreateCompanyGuarantor(CompanyGuarantor companyGuarantor)
        {
            CompanyGuarantorBusiness bis = new CompanyGuarantorBusiness();
            return bis.CreateCompanyGuarator(companyGuarantor);
        }

        public CompanyGuarantor UpdateCompanyGuarantor(CompanyGuarantor companyGuarantor)
        {
            CompanyGuarantorBusiness bis = new CompanyGuarantorBusiness();
            return bis.UpdateCompanyGuarantor(companyGuarantor);
        }

        public void DeleteCompanyGuarantor(CompanyGuarantor companyGuarantor)
        {
            CompanyGuarantorBusiness bis = new CompanyGuarantorBusiness();
            bis.DeleteCompanyGuarantor(companyGuarantor);
        }

        #endregion
        #region
        public CompanyInsuredGuaranteePrefix CreateCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix companyInsuredGuaranteePrefix)
        {
            CompanyInsuredGuaranteesPrefixBusiness bis = new CompanyInsuredGuaranteesPrefixBusiness();
            return bis.CreateCompanyCompanyInsuredGuaranteePrefix(companyInsuredGuaranteePrefix);
        }

        public CompanyInsuredGuaranteePrefix UpdateCompanyInsuredGuaranteePrefix(CompanyInsuredGuaranteePrefix companyInsuredGuaranteePrefix)
        {
            try
            {
                CompanyInsuredGuaranteesPrefixBusiness companyInsuredGuaranteePrefixBusiness = new CompanyInsuredGuaranteesPrefixBusiness();
                return companyInsuredGuaranteePrefixBusiness.UpdateCompanyInsuredGuaranteePrefix(companyInsuredGuaranteePrefix);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }


        public void DeleteCompanyInsuredGuaranteePrefix(int individualId, int guaranteeId, int documentId)
        {
            try
            {
                CompanyInsuredGuaranteesPrefixBusiness companyInsuredGuaranteeDocumentationBusiness = new CompanyInsuredGuaranteesPrefixBusiness();
                companyInsuredGuaranteeDocumentationBusiness.DeleteCompanyInsuredGuaranteePrefix(individualId, guaranteeId, documentId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<CompanyInsuredGuaranteePrefix> GetCompanyInsuredGuaranteePrefix(int individualId, int guaranteeId)
        {
            try
            {
                CompanyInsuredGuaranteesPrefixBusiness companyInsuredGuaranteePrefixBusiness = new CompanyInsuredGuaranteesPrefixBusiness();
                return companyInsuredGuaranteePrefixBusiness.GetCompanyInsuredGuaranteePrefix(individualId, guaranteeId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        #region Cumplimiento
        public bool IsConsortiumindividualId(int individualId)
        {
            ConsortiumDAO consortium = new ConsortiumDAO();
            return consortium.IsConsortiumindividualId(individualId);
        }

        public bool IsConsortiumindividualIdR1(int individualId)
        {
            ConsortiumDAO consortium = new ConsortiumDAO();
            return consortium.IsConsortiumindividualIdR1(individualId);
        }

        public decimal GetAvailableCumulus(int individualId, int currencyCode, int prefixCode, System.DateTime issueDate)
        {
            ConsortiumDAO consortium = new ConsortiumDAO();
            return consortium.GetAvailableCumulus(individualId, currencyCode, prefixCode, issueDate);

        }

        #endregion
        public List<Models.CompanyInsuredMain> GetCompanyInsuredsByName(string filterString)
        {
            PersonBusiness personBusiness = new PersonBusiness();
            return personBusiness.GetCompanyInsuredsByName(filterString);
        }
        public CiaDocumentTypeRange GetCiaDocumentTypeRangeId(int documentTypeRangeId)
        {
            try
            {
                CiaDocumentTypeRangeDAO CiaDocumentTypeRangeDAO = new CiaDocumentTypeRangeDAO();
                return CiaDocumentTypeRangeDAO.GetCiaDocumentTypeRangeId(documentTypeRangeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public CiaDocumentTypeRange CreateCiaDocumentTypeRange(CiaDocumentTypeRange CiaDocumentTypeRange)
        {
            try
            {
                CiaDocumentTypeRangeDAO CiaDocumentTypeRangeDAO = new CiaDocumentTypeRangeDAO();
                return CiaDocumentTypeRangeDAO.CreateCiaDocumentTypeRange(CiaDocumentTypeRange);
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

        #region ThirdPerson
        public List<CompanyThirdDeclinedType> GetAllThirdDeclinedTypes()
        {
            try
            {
                CompanyThirdDeclinedTypeBusiness businesss = new CompanyThirdDeclinedTypeBusiness();
                return businesss.GetAllThirdDeclinedTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear proveedor
        /// </summary>        
        public Models.CompanyThird CreateCompanyThird(Models.CompanyThird companyThird)
        {
            try
            {
                bool validateRolThird = GetCompanyIndividualRoleByIndividualId(companyThird.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_THIRD)));
                if (!validateRolThird)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyThird.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_THIRD)));
                }
                CompanyThirdBusiness companyThirdBusiness = new CompanyThirdBusiness();
                return companyThirdBusiness.CreateThird(companyThird);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public Models.CompanyThird CreateUpdateThird(Models.CompanyThird companyThird)
        {
            try
            {
                CompanyThirdBusiness companyThirdBusiness = new CompanyThirdBusiness();
                return companyThirdBusiness.UpdateThird(companyThird);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.CompanyThird GetCompanyThirdByIndividualId(int individualId)
        {
            try
            {
                CompanyThirdBusiness thirdBusiness = new CompanyThirdBusiness();
                return thirdBusiness.GetThirdByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion
        /// <summary>
        /// Guardar la informacion sarlaft de la persona en la tabla UP.INDIVIDUAL_SARLAFT
        /// </summary>
        /// <param name="individualSarlaft">Models.IndividualSarlaft.</param>
        /// <param name="individualId">Id Individuo</param>
        /// <param name="economiActivity">Actividad Economica</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public Models.IndividualSarlaft CreateIndividualSarlaft(Models.IndividualSarlaft individualSarlaft, int individualId, int economicActivity)
        {
            try
            {
                IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
                return individualSarlaftDAO.CreateIndividualSarlaft(individualSarlaft, individualId, economicActivity);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public CiaDocumentTypeRange UpdateCiaDocumentTypeRange(CiaDocumentTypeRange ciaDocumentTypeRange)
        {
            try
            {
                CiaDocumentTypeRangeDAO CiaDocumentTypeRangeDAO = new CiaDocumentTypeRangeDAO();
                return CiaDocumentTypeRangeDAO.UpdateCiaDocumentTypeRange(ciaDocumentTypeRange);
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

        public EnumRoles GetEnumsRoles()
        {
            try
            {
                EnumRoles enumRoles = new EnumRoles();

                enumRoles.Agent = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_AGENT));
                enumRoles.Coinsured = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_CONINSURED));
                enumRoles.Employee = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_EMPLOYEE));
                enumRoles.Insured = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_INSURED));
                enumRoles.Supplier = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_SUPPLIER));
                enumRoles.Third = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_THIRD));
                try
                {
                    enumRoles.Reinsurer = Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_REINSURER));
                }
                catch (Exception es) { enumRoles.Reinsurer = 0; }


                return enumRoles;
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message, ex);
            }

        }

        /// <summary>
        /// Crear empleado
        /// </summary>        
        public Models.CompanyEmployee CreateCompanyEmployee(Models.CompanyEmployee companyEmploye)
        {
            try
            {
                bool validateRolEmployee = GetCompanyIndividualRoleByIndividualId(companyEmploye.IndividualId).Exists(x => x.RoleId ==
                Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_EMPLOYEE)));
                if (!validateRolEmployee)
                {
                    CreateCompanyIndividualRoleByIndividualId(companyEmploye.IndividualId,
                        Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue<PersonKeys>(PersonKeys.PER_ROL_EMPLOYEE)));
                }

                if (string.IsNullOrEmpty(companyEmploye.FileNumber))
                {
                    ACM.Parameter updateParameter = new ACM.Parameter();
                    ACM.Parameter parameter = DelegateService.commonService.GetParameterByDescription("Employee");
                    updateParameter = parameter;
                    companyEmploye.FileNumber = parameter.NumberParameter.ToString();
                    updateParameter = parameter;
                    updateParameter.NumberParameter = updateParameter.NumberParameter + 1;
                    updateParameter.Id = parameter.Id;
                    DelegateService.commonService.UpdateParameter(updateParameter);
                }

                CompanyEmployeeBusiness companyEmployeeBusiness = new CompanyEmployeeBusiness();
                companyEmploye = companyEmployeeBusiness.CreateEmployee(companyEmploye);

                //#region Control 
                PersonDAO personDAO = new PersonDAO();
                EmployeeControl employeeControl = new EmployeeControl();
                employeeControl.IndividualId = companyEmploye.IndividualId;
                employeeControl.Action = "I";
                personDAO.CreateEmployeeControl(employeeControl);
                //#endregion
                return companyEmploye;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear empleado
        /// </summary>        
        public Models.CompanyEmployee GetCompanyEmployee(int individualId)
        {
            try
            {

                CompanyEmployeeBusiness companyEmployeeBusiness = new CompanyEmployeeBusiness();
                return companyEmployeeBusiness.GetEmployeeByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.CompanyCoClintonList> GetListClintonByDocumentNumberFullName(string documentNumber, string fullName)
        {
            try
            {
                CoClintonListDAO coClintonListDao = new CoClintonListDAO();
                return coClintonListDao.GetListClintonByDocumentNumberFullName(documentNumber, fullName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.CompanyCoOnuList> GetListOnuByDocumentNumberFullName(string documentNumber, string fullName)
        {
            try
            {
                CoOnuListDAO coOnuListDao = new CoOnuListDAO();
                return coOnuListDao.GetListOnuByDocumentNumberFullName(documentNumber, fullName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Models.CompanyCoOwnList> GetListOwnByDocumentNumberFullName(string documentNumber, string fullName)
        {
            try
            {
                CoOwnListDAO coOwnListDao = new CoOwnListDAO();
                return coOwnListDao.GetListOwnByDocumentNumberFullName(documentNumber, fullName);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #region Politicas
        public List<PoliciesAut> ValidateAuthorizationPoliciesPerson(CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson, companyCompany, companyAddress);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_GENERAL_PERSON);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesInsured(CompanyInsured insured, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();

            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeInsured(facade, insured);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_INSURED);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesProvider(CompanySupplier companySupplier, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();

            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeProvider(facade, companySupplier);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_PROVIDER);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesThirdParty(CompanyThird companyThird, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();

            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeThird(facade, companyThird);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_THIRD);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesAgent(CompanyAgent agent, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();

            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeAgent(facade, agent);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_INTERMEDIARY);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesEmployee(CompanyEmployee companyEmployee, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();

            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeEmployee(facade, companyEmployee);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_EMPLOYED);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesPaymentMethods(CompanyIndividualPaymentMethod companyPaymentMethod, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadePaymentMethod(facade, companyPaymentMethod);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_PAYMENT_METHODS);
        }
        public List<PoliciesAut> ValidateAuthorizationPoliciesPersonalInformation(CompanyLabourPerson labourPerson, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadePersonalInformation(facade, labourPerson);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_PERSONAL_INF);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesOperatingQuota(CompanyOperatingQuota companyOperationQuota, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeOperatingQuota(facade, companyOperationQuota);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_OPERATING_QUOTA);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesBankTransfers(models.BankTransfers bankTransfers, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeBankTransfers(facade, bankTransfers);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_BANK_TRANSFERS);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesReInsurer(CompanyReInsurer companyReInsurer, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeReInsurer(facade, companyReInsurer);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_REINSURER);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesTaxes(CompanyIndividualTax individualTax, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;

            Facade facade = new Facade();

            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeTaxes(facade, individualTax);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_TAXES);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesCoInsured(CompanyCoInsured companyCoInsured, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeCoInsured(facade, companyCoInsured);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_COINSURER);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesBusinessName(models.CompanyName companyCoInsured, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeBusinessName(facade, companyCoInsured);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_BUSINESS_NAME);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesConsortium(CompanyConsortium companyConsortium, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeConsortium(facade, companyConsortium);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_CONSORTIATES);
        }

        public List<PoliciesAut> ValidateAuthorizationPoliciesGuarantee(models.Guarantee guarantee, CompanyPerson companyPerson, CompanyCompany companyCompany, List<CompanyAddress> companyAddress)
        {
            int package = 14;
            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPerson(facade, companyPerson?.IndividualId == 0 ? null : companyPerson, companyCompany?.IndividualId == 0 ? null : companyCompany, companyAddress);
            EntityAssembler.CreateFacadeGuarantee(facade, guarantee);
            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "14,0", facade, FacadeType.RULE_FACADE_GUARANTEES);
        }
        #endregion

        #region Politicas Personas

        /// <summary>
        /// Obtener politicas ejecutadas
        /// </summary>        
        public CompanyPersonOperation GetCompanyPersonOperation(int operationId)
        {
            try
            {
                CompanyPersonOperationBusiness companyPersonOperation = new CompanyPersonOperationBusiness();
                return companyPersonOperation.GetCompanyPersonOperation(operationId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear politicas ejecutadas
        /// </summary>        
        public Models.CompanyPersonOperation CreateCompanyPersonOperation(CompanyPersonOperation companyPersonOperation)
        {
            try
            {
                CompanyPersonOperationBusiness companyPersonOperationBusiness = new CompanyPersonOperationBusiness();
                return companyPersonOperationBusiness.CreateCompanyPersonOperation(companyPersonOperation);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        /// <summary>
        /// Politicas ejecutadas por persona
        /// </summary>        
        public List<AuthorizationRequest> GetAuthorizationRequestByIndividualId(int individualId)
        {
            try
            {
                CompanyPersonOperationBusiness companyPersonOperationBusiness = new CompanyPersonOperationBusiness();
                return companyPersonOperationBusiness.GetAuthorizationRequestByIndividualId(individualId);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion Politicas Personas

        #region Politicas Informacion Basica


        public List<PoliciesAut> ValidateAuthorizationPoliciesPersonBasicInfo(CompanyPerson companyPerson, CompanyCompany companyCompany)
        {
            int package = 20;

            Facade facade = new Facade();
            EntityAssembler.CreateFacadeGeneralPersonBasicInfo(facade, companyPerson, companyCompany);

            return DelegateService.AuthorizationPoliciesServiceCore.ValidateAuthorizationPolicies(package, "1", facade, FacadeType.RULE_FACADE_GENERAL_BASIC_INFO);
        }


        #endregion

        #region SarlaftExoneration
        public Models.CompanySarlaftExoneration UpdateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration, int individualId)
        {
            SarlaftDAO sarlaftDAO = new SarlaftDAO();

            return sarlaftDAO.UpdateSarlaftExoneration(sarlaftExoneration, individualId);
        }

        #endregion SarlaftExoneration

        public List<CompanyPaymentAccountType> getCompanyPaymentAccountTypes()
        {
            try
            {
                PaymentAccountTypeDAO paymentAccountTypeDAO = new PaymentAccountTypeDAO();
                return paymentAccountTypeDAO.getCompanyPaymentAccountTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<models.AccountType> getCompanyAccountType()
        {
            try
            {
                return DelegateService.UniquePersonServiceCore.GetAccountTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        #region Cupo Operativo
        public CompanyOrPerson GetIdentificationPersonOrCompanyByIndividualId(int individualId)
        {
            try
            {
                PersonDAO personDAO = new PersonDAO();
                return personDAO.GetIdentificationPersonOrCompanyByIndividualId(individualId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion

        public CompanyAgency GetCompanyAgencyByAgentCodeAgentTypeCode(int agentCode, int agentTypeCode)
        {
            try
            {
                CompanyAgentBusiness companyAgentBusiness = new CompanyAgentBusiness();
                return companyAgentBusiness.GetCompanyAgencyByAgentCodeAgentTypeCode(agentCode, agentTypeCode);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
