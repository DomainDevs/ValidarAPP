using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Resources;
using Sistran.Company.Application.UniquePersonServices.EEProvider;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using CONS = Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using MOCOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using MOCOV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
using UTILHELPER = Sistran.Core.Application.Utilities.Helper;

namespace Sistran.Company.Application.UniquePersonAplicationServices.EEProvider
{
    using System.Threading.Tasks;
    using Core.Application.AuthorizationPoliciesServices.Models;
    using Newtonsoft.Json;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.OperationQuotaServices.DTOs.OperationQuota;
    using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;
    using System.Threading.Tasks;
    using Sistran.Core.Application.CommonService.Models;
    using ENUMCOAP = Core.Application.AuthorizationPoliciesServices.Enums;
    using Sistran.Company.Application.UniquePersonAplicationServices.EEProvider.Enums;

    /// <summary>
    /// Personas 
    /// </summary>

    /// <seealso cref="Sistran.Company.Application.UniquePersonService.IUniquePersonService" />
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class UniquePersonAplicationServiceEEProvider : IUniquePersonAplicationService
    {
        object look = new object();
        #region Person
        public PersonDTO CreateAplicationPerson(PersonDTO personDTO, bool validatePolicies = true)
        {

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            CompanyPerson person = ModelAssembler.CreatePerson(personDTO);

            if (validatePolicies)
            {
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, null, ModelAssembler.CreateAdddresses(personDTO.Addresses)));
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    personDTO.OperationId = this.CreateAplicationPersonOperation(
                            new PersonOperationDTO
                            {
                                IndividualId = personDTO.Id,
                                Operation = JsonConvert.SerializeObject(personDTO),
                                ProcessType = "Create Person",
                                FunctionId = (int)ENUMCOAP.TypeFunction.PersonGeneral,
                                Proccess = ENUMCOAP.TypeFunction.PersonGeneral.ToString()
                            }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                person = DelegateService.uniquePersonService.CreateCompanyPerson(person);

                var addresses = DelegateService.uniquePersonService.CreateCompanyAddresses(person.IndividualId, ModelAssembler.CreateAdddresses(personDTO.Addresses));
                var emails = DelegateService.uniquePersonService.CreateCompanyEmails(person.IndividualId, ModelAssembler.CreateEmails(personDTO.Emails));
                var phones = DelegateService.uniquePersonService.CreateCompanyPhones(person.IndividualId, ModelAssembler.CreatePhones(personDTO.Phones));


                var modelPerson = AplicationAssembler.CreatePerson(person);
                modelPerson.Addresses = AplicationAssembler.CreateAdddresses(addresses);
                modelPerson.Emails = AplicationAssembler.CreateEmails(emails);
                modelPerson.Phones = AplicationAssembler.CreatePhones(phones);

                if (personDTO.Sarlaft != null)
                {
                    foreach (var itemSarlaft in personDTO.Sarlaft)
                    {
                        itemSarlaft.IndividualId = modelPerson.Id;
                    }
                    modelPerson.Sarlaft = CreateIndividualSarlaft(personDTO.Sarlaft);
                }

                List<CompanyIndividualPaymentMethod> individualPaymentMethods = new List<CompanyIndividualPaymentMethod>();
                individualPaymentMethods.Add(
                    new CompanyIndividualPaymentMethod
                    {
                        Account = new CompanyPaymentAccount
                        {
                            BankBranch = new CompanyBankBranch
                            {
                                Bank = new CompanyBank()
                            },
                            Type = new CompanyPaymentAccountType()
                        },
                        Method = new CompanyPaymentMethod { Id = 1 }
                    }
                    );
                DelegateService.uniquePersonService.CreateIndividualpaymentMethods(individualPaymentMethods, modelPerson.Id);
                personDTO = modelPerson;
            }
            personDTO.InfringementPolicies = infringementPolicies;
            return personDTO;
        }

        public PersonDTO UpdateAplicationPerson(PersonDTO personDTO, bool validatePolicies = true)
        {
            CompanyPerson person = ModelAssembler.CreatePerson(personDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, null, ModelAssembler.CreateAdddresses(personDTO.Addresses)));
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    personDTO.OperationId = this.CreateAplicationPersonOperation(
                            new PersonOperationDTO
                            {
                                IndividualId = personDTO.Id,
                                Operation = JsonConvert.SerializeObject(personDTO),
                                ProcessType = "Update Person",
                                FunctionId = (int)ENUMCOAP.TypeFunction.PersonGeneral,
                                Proccess = ENUMCOAP.TypeFunction.PersonGeneral.ToString()
                            }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                person = DelegateService.uniquePersonService.UpdateCompanyPerson(person);

                var addressess = new List<CompanyAddress>();
                addressess.AddRange(ModelAssembler.CreateAdddresses(personDTO.Addresses.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Original).ToList()));
                addressess.AddRange(DelegateService.uniquePersonService.CreateCompanyAddresses(person.IndividualId, ModelAssembler.CreateAdddresses(personDTO.Addresses.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Insert).ToList())));
                addressess.AddRange(DelegateService.uniquePersonService.UpdateCompanyAddresses(person.IndividualId, ModelAssembler.CreateAdddresses(personDTO.Addresses.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Update).ToList())));

                var emails = new List<CompanyEmail>();
                emails.AddRange(ModelAssembler.CreateEmails(personDTO.Emails.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Original).ToList()));
                emails.AddRange(DelegateService.uniquePersonService.CreateCompanyEmails(person.IndividualId, ModelAssembler.CreateEmails(personDTO.Emails.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Insert).ToList())));
                emails.AddRange(DelegateService.uniquePersonService.UpdateCompanyEmails(person.IndividualId, ModelAssembler.CreateEmails(personDTO.Emails.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Update).ToList())));

                var phones = new List<CompanyPhone>();
                phones.AddRange(DelegateService.uniquePersonService.CreateCompanyPhones(person.IndividualId, ModelAssembler.CreatePhones(personDTO.Phones.Where(x => x.Id == 0).ToList())));
                phones.AddRange(DelegateService.uniquePersonService.UpdateCompanyPhones(person.IndividualId, ModelAssembler.CreatePhones(personDTO.Phones.Where(x => x.Id > 0).ToList())));

                var modelPerson = AplicationAssembler.CreatePerson(person);
                modelPerson.Addresses = AplicationAssembler.CreateAdddresses(addressess);
                modelPerson.Emails = AplicationAssembler.CreateEmails(emails);
                modelPerson.Phones = AplicationAssembler.CreatePhones(phones);


                if (modelPerson.Sarlaft != null)
                {
                    foreach (var itemSarlaft in personDTO.Sarlaft)
                    {
                        itemSarlaft.IndividualId = personDTO.Id;
                    }
                    modelPerson.Sarlaft = UpdateIndividualSarlaft(personDTO.Sarlaft);
                }

                personDTO = modelPerson;
            }

            personDTO.InfringementPolicies = infringementPolicies;
            return personDTO;
        }

        public List<PersonDTO> GetAplicationPersonByDocument(string documentNumber)
        {

            var personsModel = DelegateService.uniquePersonService.GetCompanyPersonByDocument(CustomerType.Individual, documentNumber).Cast<CompanyPerson>().ToList();
            var persons = AplicationAssembler.CreatePersons(personsModel);
            if (persons.Count == 1)
            {
                persons[0].Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(persons[0].Id));
                persons[0].Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(persons[0].Id));
                persons[0].Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(persons[0].Id));
                persons[0].EconomicActivityDescription = DelegateService.uniquePersonService.GetEconomicActivitiesById(persons[0].EconomicActivityId).Description;
                persons[0].Sarlaft = DelegateService.uniquePersonAplicationService.GetIndividualSarlaft(persons[0].Id);
            }

            return persons;
        }


        public List<PersonDTO> GetAplicationPersonAdv(PersonDTO personDTO)
        {
            CompanyPerson person = ModelAssembler.CreatePerson(personDTO);
            var personsModel = DelegateService.uniquePersonService.GetCompanyPersonAdv(CustomerType.Individual, person);
            var persons = AplicationAssembler.CreatePersons(personsModel);
            if (persons.Count == 1)
            {
                persons[0].Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(persons[0].Id));
                persons[0].Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(persons[0].Id));
                persons[0].Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(persons[0].Id));
            }
            return persons;
        }

        public PersonDTO GetAplicationPersonById(int id)
        {
            var personsModel = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(id);
            var person = AplicationAssembler.CreatePerson(personsModel);
            if (string.IsNullOrEmpty(person.EconomicActivityDescription))
            {
                person.EconomicActivityDescription = DelegateService.uniquePersonAplicationService.GetAplicationEconomicActivities().Where(x => x.Id == person.EconomicActivityId).Select(x => x.Description).FirstOrDefault();
            }
            if (person.ExonerationTypeCode == null)
            {
                person.ExonerationTypeCode = GetAplicationPersonByDocument(person.Document).Where(x => x.Id == person.Id).Select(x => x.ExonerationTypeCode).FirstOrDefault();
            }
            person.Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(person.Id));
            person.Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(person.Id));
            person.Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(person.Id));
            return person;
        }

        public PersonDTO UpdateApplicationPersonBasicInfo(PersonDTO personDTO)
        {
            CompanyPerson person = ModelAssembler.CreatePerson(personDTO);
            person = DelegateService.uniquePersonService.UpdateCompanyPerson(person);
            var modelPerson = AplicationAssembler.CreatePerson(person);
            return modelPerson;
        }

        #endregion
        public IndividualTypePersonDTO GetPersonByIndividualId(int id)
        {

            IndividualTypePersonDTO individualTypePerson = new IndividualTypePersonDTO();
            var personsModel = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(id);
            if (personsModel.IndividualId > 0)
            {
                var person = AplicationAssembler.CreatePerson(personsModel);
                if (string.IsNullOrEmpty(person.EconomicActivityDescription))
                {
                    person.EconomicActivityDescription = DelegateService.uniquePersonAplicationService.GetAplicationEconomicActivities().Where(x => x.Id == person.EconomicActivityId).Select(x => x.Description).FirstOrDefault();
                }
                if (person.ExonerationTypeCode == null)
                {
                    person.ExonerationTypeCode = GetAplicationPersonByDocument(person.Document).Where(x => x.Id == person.Id).Select(x => x.ExonerationTypeCode).FirstOrDefault();
                }
                person.Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(person.Id));
                person.Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(person.Id));
                person.Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(person.Id));
                individualTypePerson.Person = person;
                individualTypePerson.PersonType = UniquePersonService.Enums.PersonType.Natural;
            }

            var companyModel = DelegateService.uniquePersonService.GetCompanyByIndividualId(id);

            if (companyModel.IndividualId > 0)
            {
                var company = AplicationAssembler.CreateCompany(companyModel);

                company.Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(company.Id));
                company.Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(company.Id));
                company.Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(company.Id));
                company.EconomicActivityDescription = DelegateService.uniquePersonService.GetEconomicActivitiesById(company.EconomicActivityId).Description;

                var insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(company.Id);
                company.Insured = insured != null ? AplicationAssembler.CreateInsured(insured) : null;

                if (company.Insured != null && company.Insured?.Id > 0)
                {
                    var consortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(company.Insured.Id);
                    company.ConsortiumMembers = consortiums != null ? AplicationAssembler.CreateConsortiums(consortiums) : null;
                }

                individualTypePerson.Company = company;
                individualTypePerson.PersonType = UniquePersonService.Enums.PersonType.Legal;
            }

            return individualTypePerson;
        }

        public Parameter GetParameterFutureSociety(int parameterFutureSociety)
        {
            List<Task> agent = new List<Task>();
            Parameter parameter = new Parameter();
            Parameter updateParameter = new Parameter();

            agent.Add(Task.Run(() =>
            {
                lock (look)
                {

                    parameter = DelegateService.commonService.GetParameterByParameterId(parameterFutureSociety);
                    updateParameter = parameter;
                    updateParameter.NumberParameter = updateParameter.NumberParameter + 1;
                    updateParameter.Id = parameterFutureSociety;
                    DelegateService.commonService.UpdateParameter(updateParameter);
                }
            }));
            Task.WaitAll(agent.ToArray());
            return parameter;
        }

        #region Company
        public CompanyDTO CreateAplicationCompany(CompanyDTO companyDTO, bool validatePolicies = true)
        {
            UniquePersonServices.V1.Models.CompanyCompany company = ModelAssembler.CreateCompany(companyDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            List<CONS.ConsortiumEventDTO> consortiumEventDTOs = companyDTO.ConsortiumEventDTOs;
            CONS.ConsortiumEventDTO consortiumEventDTO = companyDTO.ConsortiumeventDTO;
            if (validatePolicies)
            {
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(null, company, ModelAssembler.CreateAdddresses(companyDTO.Addresses)));
                if (companyDTO.ConsortiumMembers != null)
                {
                    if (company.Insured != null)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesInsured(company.Insured, null, company, ModelAssembler.CreateAdddresses(companyDTO.Addresses)));
                    }
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, ModelAssembler.CreateAdddresses(companyDTO.Addresses)));
                    }
                }
            }
            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    companyDTO.OperationId = CreateAplicationPersonOperation(
                            new PersonOperationDTO
                            {
                                IndividualId = companyDTO.Id,
                                Operation = JsonConvert.SerializeObject(companyDTO),
                                ProcessType = "Create Company",
                                FunctionId = (int)ENUMCOAP.TypeFunction.PersonGeneral,
                                Proccess = ENUMCOAP.TypeFunction.PersonGeneral.ToString()
                            }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                company = DelegateService.uniquePersonService.CreateCompanyCompany(company);
                var addresses = DelegateService.uniquePersonService.CreateCompanyAddresses(company.IndividualId, ModelAssembler.CreateAdddresses(companyDTO.Addresses));
                var emails = DelegateService.uniquePersonService.CreateCompanyEmails(company.IndividualId, ModelAssembler.CreateEmails(companyDTO.Emails));
                var phones = DelegateService.uniquePersonService.CreateCompanyPhones(company.IndividualId, ModelAssembler.CreatePhones(companyDTO.Phones));

                var businessName = DelegateService.uniquePersonService.CreateBusinessName(ModelAssembler.CreateCompanyName(company.IndividualId, addresses[0].Id, emails[0].Id, phones[0].Id, companyDTO));

                var modelCompany = AplicationAssembler.CreateCompany(company);
                modelCompany.Addresses = AplicationAssembler.CreateAdddresses(addresses);
                modelCompany.Emails = AplicationAssembler.CreateEmails(emails);
                modelCompany.Phones = AplicationAssembler.CreatePhones(phones);
                if (companyDTO.ConsortiumMembers != null)
                {
                    DelegateService.uniquePersonService.DeleteUserAssignedConsortium(1009, companyDTO.UserId);
                    modelCompany.ConsortiumMembers = AplicationAssembler.CreateConsortiums(company.Consortiums);
                    //Consortium event
                    consortiumEventDTO.IndividualConsortiumID = company.IndividualId;
                    DelegateService.consortiumIntegrationService.CreateConsortiumEvent(ModelAssembler.CreateConsortiumEvent(consortiumEventDTO));
                    if (consortiumEventDTOs.Count > 0)
                    {
                        consortiumEventDTOs.ForEach(x => x.IndividualConsortiumID = company.IndividualId);
                        List<ConsortiumEventDTO> eventDTOs = new List<ConsortiumEventDTO>();
                        eventDTOs = DelegateService.consortiumIntegrationService.AssigendIndividualToConsotium(ModelAssembler.CreateConsortiumEvents(consortiumEventDTOs));
                    }
                }
                if (companyDTO.Sarlaft != null)
                {
                    foreach (var itemSarlaft in companyDTO.Sarlaft)
                    {
                        itemSarlaft.IndividualId = modelCompany.Id;
                        itemSarlaft.ActivityEconomic = modelCompany.EconomicActivityId;
                    }
                    modelCompany.Sarlaft = CreateIndividualSarlaft(companyDTO.Sarlaft);
                }

                List<CompanyIndividualPaymentMethod> individualPaymentMethods = new List<CompanyIndividualPaymentMethod>();
                individualPaymentMethods.Add(
                    new CompanyIndividualPaymentMethod
                    {
                        Account = new CompanyPaymentAccount
                        {
                            BankBranch = new CompanyBankBranch
                            {
                                Bank = new CompanyBank()
                            },
                            Type = new CompanyPaymentAccountType()
                        },
                        Method = new CompanyPaymentMethod { Id = 1 }
                    }
                    );
                DelegateService.uniquePersonService.CreateIndividualpaymentMethods(individualPaymentMethods, modelCompany.Id);

                companyDTO = modelCompany;
            }

            companyDTO.InfringementPolicies = infringementPolicies;
            return companyDTO;
        }

        public CompanyDTO UpdateAplicationCompany(CompanyDTO personDTO, bool validatePolicies = true)
        {
            UniquePersonServices.V1.Models.CompanyCompany company = ModelAssembler.CreateCompany(personDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies)
            {
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(null, company, ModelAssembler.CreateAdddresses(personDTO.Addresses)));

                if (personDTO.ConsortiumMembers != null)
                {
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, ModelAssembler.CreateAdddresses(personDTO.Addresses)));
                    }
                }
            }
            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    personDTO.OperationId = CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = personDTO.Id,
                               Operation = JsonConvert.SerializeObject(personDTO),
                               ProcessType = "Update Company",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonGeneral,
                               Proccess = ENUMCOAP.TypeFunction.PersonGeneral.ToString()
                           }
                   ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {

                var addressess = new List<CompanyAddress>();
                addressess.AddRange(ModelAssembler.CreateAdddresses(personDTO.Addresses.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Original).ToList()));
                addressess.AddRange(DelegateService.uniquePersonService.CreateCompanyAddresses(company.IndividualId, ModelAssembler.CreateAdddresses(personDTO.Addresses.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Insert).ToList())));
                addressess.AddRange(DelegateService.uniquePersonService.UpdateCompanyAddresses(company.IndividualId, ModelAssembler.CreateAdddresses(personDTO.Addresses.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Update).ToList())));

                var emails = new List<CompanyEmail>();
                emails.AddRange(DelegateService.uniquePersonService.CreateCompanyEmails(company.IndividualId, ModelAssembler.CreateEmails(personDTO.Emails.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Insert).ToList())));
                emails.AddRange(DelegateService.uniquePersonService.UpdateCompanyEmails(company.IndividualId, ModelAssembler.CreateEmails(personDTO.Emails.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Update).ToList())));

                var phones = new List<CompanyPhone>();
                phones.AddRange(DelegateService.uniquePersonService.CreateCompanyPhones(company.IndividualId, ModelAssembler.CreatePhones(personDTO.Phones.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Insert && x.Id == 0).ToList())));
                phones.AddRange(DelegateService.uniquePersonService.UpdateCompanyPhones(company.IndividualId, ModelAssembler.CreatePhones(personDTO.Phones.Where(x => x.AplicationStaus == CommonAplicationServices.Enums.AplicationStaus.Update).ToList())));
                company = DelegateService.uniquePersonService.UpdateCompanyCompany(company);
                var modelCompany = AplicationAssembler.CreateCompany(company);

                modelCompany.Addresses = AplicationAssembler.CreateAdddresses(addressess);
                if (emails.Count > 0)
                {
                    personDTO.Emails.ForEach(x =>
                    {
                        var listEmails = emails.Where(y => y.Description == x.Description).FirstOrDefault();
                        if (listEmails != null)
                        {
                            x.Id = listEmails.Id;
                        }

                    });
                    modelCompany.Emails = AplicationAssembler.CreateEmails(ModelAssembler.CreateEmails(personDTO.Emails));
                }
                else
                {
                    modelCompany.Emails = AplicationAssembler.CreateEmails(ModelAssembler.CreateEmails(personDTO.Emails));
                }

                if (phones.Count > 0)
                {
                    personDTO.Phones.ForEach(x =>
                    {
                        var listPhones = phones.Where(y => y.Description == x.Description).FirstOrDefault();
                        if (listPhones != null)
                        {
                            x.Id = listPhones.Id;
                        }

                    });
                    modelCompany.Phones = AplicationAssembler.CreatePhones(ModelAssembler.CreatePhones(personDTO.Phones));
                }
                else
                {
                    modelCompany.Phones = AplicationAssembler.CreatePhones(ModelAssembler.CreatePhones(personDTO.Phones));
                }

                //if (modelCompany.ConsortiumMembers.Count > 0)
                //{
                //    var updConsorciatedDTO = personDTO.ConsortiumMembers.Where(m => m.ConsortiumId > 0).ToList();
                //    modelCompany.ConsortiumMembers.AddRange(DelegateService.uniquePersonAplicationService.UpdateConsortium(updConsorciatedDTO, false));
                //    var insConsorciatedDTO = personDTO.ConsortiumMembers.Where(m => m.ConsortiumId == 0).ToList();
                //    if (personDTO.ConsortiumEventDTOs.Count > 0)
                //    {
                //        personDTO.ConsortiumEventDTOs.ForEach(x => x.IndividualConsortiumID = company.IndividualId);
                //        List<ConsortiumEventDTO> eventDTOs = new List<ConsortiumEventDTO>();
                //        eventDTOs = DelegateService.consortiumIntegrationService.AssigendIndividualToConsotium(ModelAssembler.CreateConsortiumEvents(personDTO.ConsortiumEventDTOs));
                //    }
                //    //  modelCompany.ConsortiumMembers = DelegateService.uniquePersonAplicationService.CreateConsortium(insConsorciatedDTO, company.IndividualId, false);
                //}
                //else
                //{
                //    modelCompany.ConsortiumMembers = null;
                //}

                personDTO = modelCompany;
            }

            personDTO.InfringementPolicies = infringementPolicies;
            return personDTO;
        }

        public List<CompanyDTO> GetAplicationCompanyByDocument(string documentNumber)
        {
            var comapaniesModel = DelegateService.uniquePersonService.GetCompanyCompanyByDocument(CustomerType.Individual, documentNumber).Cast<CompanyCompany>().ToList();
            var comapanies = AplicationAssembler.CreateCompanies(comapaniesModel);
            if (comapanies.Count == 1)
            {
                comapanies[0].Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(comapanies[0].Id));
                comapanies[0].Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(comapanies[0].Id));
                comapanies[0].Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(comapanies[0].Id));
                comapanies[0].EconomicActivityDescription = DelegateService.uniquePersonService.GetEconomicActivitiesById(comapanies[0].EconomicActivityId).Description;

                var insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(comapanies[0].Id);
                comapanies[0].Insured = insured != null ? AplicationAssembler.CreateInsured(insured) : null;

                var consortiums = insured != null ? DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(comapanies[0].Insured.Id) : null;
                comapanies[0].ConsortiumMembers = consortiums != null ? AplicationAssembler.CreateConsortiums(consortiums) : null;

            }

            return comapanies;
        }

        public List<CompanyDTO> GetAplicationCompanyAdv(CompanyDTO companyDTO)
        {
            CompanyCompany company = ModelAssembler.CreateCompany(companyDTO);
            var companiesModel = DelegateService.uniquePersonService.GetCompanyCompanyAdv(CustomerType.Individual, company);
            var companies = AplicationAssembler.CreateCompanies(companiesModel);
            if (companies.Count == 1)
            {
                companies[0].Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(companies[0].Id));
                companies[0].Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(companies[0].Id));
                companies[0].Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(companies[0].Id));
                companies[0].EconomicActivityDescription = DelegateService.uniquePersonService.GetEconomicActivitiesById(companies[0].EconomicActivityId).Description;

                var insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(companies[0].Id);
                companies[0].Insured = insured != null ? AplicationAssembler.CreateInsured(insured) : null;
                var consortiums = companies[0].Insured != null ? DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(companies[0].Insured.Id) : null;
                companies[0].ConsortiumMembers = consortiums != null ? AplicationAssembler.CreateConsortiums(consortiums) : null;
            }
            return companies;
        }

        public CompanyDTO GetAplicationCompanyById(int id)
        {

            var companyModel = DelegateService.uniquePersonService.GetCompanyByIndividualId(id);
            var company = AplicationAssembler.CreateCompany(companyModel);

            company.Addresses = AplicationAssembler.CreateAdddresses(DelegateService.uniquePersonService.GetCompanyAddresses(company.Id));
            company.Emails = AplicationAssembler.CreateEmails(DelegateService.uniquePersonService.GetCompanyEmails(company.Id));
            company.Phones = AplicationAssembler.CreatePhones(DelegateService.uniquePersonService.GetCompanyPhones(company.Id));
            company.EconomicActivityDescription = DelegateService.uniquePersonService.GetEconomicActivitiesById(company.EconomicActivityId).Description;

            var insured = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(company.Id);
            company.Insured = insured != null ? AplicationAssembler.CreateInsured(insured) : null;

            if (company != null && company.Insured != null)
            {
                var consortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(company.Insured.Id);

                company.ConsortiumMembers = consortiums != null ? AplicationAssembler.CreateConsortiums(consortiums) : null;
            }
            return company;
        }

        #endregion

        #region insured
        public InsuredDTO GetAplicationInsuredByIndividualId(int individualId)
        {
            var insuredModel = DelegateService.uniquePersonService.GetCompanyInsuredByIndividualId(individualId);
            InsuredDTO insured = null;
            if (insuredModel != null)
            {
                insured = AplicationAssembler.CreateInsured(insuredModel);
            }

            return insured;
        }

        public InsuredDTO GetAplicationInsuredElectronicBillingByIndividualId(int individualId)
        {
            var insuredModel = DelegateService.uniquePersonService.GetCompanyInsuredElectronicBillingByIndividualId(individualId);
            InsuredDTO insured = null;
            if (insuredModel != null)
            {
                insured = AplicationAssembler.CreateInsuredElectronicBilling(insuredModel);
            }

            return insured;
        }

        public InsuredDTO CreateAplicationInsured(InsuredDTO insuredDTO, bool validatePolicies = true)
        {
            insuredDTO.ModifyDate = DateTime.Now;
            insuredDTO.EnteredDate = DateTime.Now;
            insuredDTO.DeclinedDate = null;

            CompanyInsured insured = ModelAssembler.CreateInsured(insuredDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies)
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(insured.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(insured.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(insured.IndividualId);

                person.UserId = insuredDTO.UserId;
                company.UserId = insuredDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesInsured(insured, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(insuredDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    insuredDTO.OperationId = this.CreateAplicationPersonOperation(
                            new PersonOperationDTO
                            {
                                IndividualId = insuredDTO.IndividualId,
                                Operation = JsonConvert.SerializeObject(insuredDTO),
                                ProcessType = "Create",
                                FunctionId = (int)ENUMCOAP.TypeFunction.PersonInsured,
                                Proccess = ENUMCOAP.TypeFunction.PersonInsured.ToString()
                            }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                CompanyInsured insuredModel = DelegateService.uniquePersonService.CreateCompanyInsured(insured);
                insuredDTO = AplicationAssembler.CreateInsured(insuredModel);
            }

            insuredDTO.InfringementPolicies = infringementPolicies;
            return insuredDTO;
        }

        public InsuredDTO UpdateAplicationInsured(InsuredDTO insuredDTO, bool validatePolicies = true)
        {
            insuredDTO.ModifyDate = DateTime.Now;
            CompanyInsured insured = ModelAssembler.CreateInsured(insuredDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies)
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(insured.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(insured.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(insured.IndividualId);

                person.UserId = insuredDTO.UserId;
                company.UserId = insuredDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesInsured(insured, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(insuredDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    insuredDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = insuredDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(insuredDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonInsured,
                               Proccess = ENUMCOAP.TypeFunction.PersonInsured.ToString()
                           }
                   ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                insured = DelegateService.uniquePersonService.UpdateCompanyInsured(insured);
                insuredDTO = AplicationAssembler.CreateInsured(insured);
            }

            insuredDTO.InfringementPolicies = infringementPolicies;
            return insuredDTO;
        }
        #endregion

        #region ElectronicBilling
        public InsuredDTO UpdateAplicationInsuredElectronicBilling(InsuredDTO insuredDTO)
        {
            insuredDTO.ModifyDate = DateTime.Now;
            CompanyInsured insured = ModelAssembler.CreateInsured(insuredDTO);

            insured = DelegateService.uniquePersonService.UpdateCompanyInsuredElectronicBilling(insured);
            insuredDTO = AplicationAssembler.CreateInsuredElectronicBilling(insured);
            return insuredDTO;
        }
        #endregion ElectronicBilling


        #region Supplier v1

        public List<SupplierDeclinedTypeDTO> GetAplicationSupplierDeclinedTypes()
        {
            var SupplierDeclinedTypesModel = DelegateService.uniquePersonService.GetCompanySupplierDeclinedTypes();
            var result = AplicationAssembler.CreateSupplierDeclinedTypes(SupplierDeclinedTypesModel);
            return result;
        }

        public List<GroupSupplierDTO> GetAplicationGroupSupplierDTO()
        {
            var GroupSupplier = DelegateService.uniquePersonService.GetCompanyGroupSupplier();
            var result = AplicationAssembler.CreateGroupsSupplier(GroupSupplier);
            return result;
        }

        public List<SupplierAccountingConceptDTO> GetAplicationSupplierAccountingConceptsBySupplierId(int SupplierId)
        {
            var SupplierAccountingConceptModel = DelegateService.uniquePersonService.GetCompanySupplierAccountingConceptsBySupplierId(SupplierId);
            var result = AplicationAssembler.CreateSupplierAccountingConcepts(SupplierAccountingConceptModel);
            return result;
        }

        public List<AccountingConceptDTO> GetAplicationAccountingConcepts()
        {
            var AccountingConceptModel = DelegateService.uniquePersonService.GetCompanyAccountingConcepts();
            var result = AplicationAssembler.CreateAccountingConcepts(AccountingConceptModel);
            return result;
        }

        public List<SupplierProfileDTO> GetAplicationSupplierProfiles(int suppilierTypeId)
        {
            var SupplierProfileModel = DelegateService.uniquePersonService.GetCompanySupplierProfiles(suppilierTypeId);
            var result = AplicationAssembler.CreateSupplierProfiles(SupplierProfileModel);
            return result;
        }

        public List<SupplierTypeDTO> GetAplicationSupplierTypes()
        {
            var supplierModel = DelegateService.uniquePersonService.GetCompanySupplierTypes();
            var result = AplicationAssembler.CreateSupplierTypes(supplierModel);
            return result;

        }

        public ProviderDTO CreateAplicationSupplier(ProviderDTO providerDTO, bool validatePolicies = true)
        {
            MOCOV1.CompanySupplier supplier = ModelAssembler.CreateSupplier(providerDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(providerDTO.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(providerDTO.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(providerDTO.IndividualId);

                person.UserId = providerDTO.UserId;
                company.UserId = providerDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesProvider(supplier, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(providerDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    providerDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = providerDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(providerDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonProvider,
                               Proccess = ENUMCOAP.TypeFunction.PersonProvider.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {

                supplier = DelegateService.uniquePersonService.CreateCompanySupplier(supplier);
                providerDTO = AplicationAssembler.CreateSupplier(supplier);
            }

            providerDTO.InfringementPolicies = infringementPolicies;
            return providerDTO;
        }

        public ProviderDTO UpdateAplicationSupplier(ProviderDTO providerDTO, bool validatePolicies = true)
        {
            MOCOV1.CompanySupplier provider = ModelAssembler.CreateSupplier(providerDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(providerDTO.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(providerDTO.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(providerDTO.IndividualId);

                person.UserId = providerDTO.UserId;
                company.UserId = providerDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesProvider(provider, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(providerDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    providerDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = providerDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(providerDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonProvider,
                               Proccess = ENUMCOAP.TypeFunction.PersonProvider.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                provider = DelegateService.uniquePersonService.UpdateCompanySupplier(provider);
                providerDTO = AplicationAssembler.CreateSupplier(provider);
            }

            providerDTO.InfringementPolicies = infringementPolicies;
            return providerDTO;
        }

        public ProviderDTO GetAplicationSupplierByIndividualId(int id)
        {
            var supplierModel = DelegateService.uniquePersonService.GetCompanySupplierByIndividualId(id);
            var result = AplicationAssembler.CreateSupplier(supplierModel);
            return result;
        }

        #endregion Supplier

        #region Agent 
        public AgentDTO ProcessAplicationAgent(AgentDTO agentDTOs, bool validatePolicies = true)
        {
            try
            {
                AgentDTO agentDTO = new AgentDTO();
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                CompanyAgent companyAgent = ModelAssembler.CreateAgent(agentDTOs);
                if (validatePolicies)
                {
                    CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(agentDTOs.IndividualId);
                    CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(agentDTOs.IndividualId);
                    List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(agentDTOs.IndividualId);
                    person.UserId = agentDTOs.UserId;
                    company.UserId = agentDTOs.UserId;
                    infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                    infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesAgent(companyAgent, person, company, addresses));

                    IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(agentDTOs.IndividualId);
                    if ((consortiumDTO?.Any()).GetValueOrDefault())
                    {
                        company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                        foreach (CompanyConsortium consorciated in company.Consortiums)
                        {
                            infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                        }
                    }
                }

                if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                    {
                        var AgenDB = DelegateService.uniquePersonService.GetCompanyAgentByIndividualId(agentDTOs.IndividualId);
                        agentDTOs.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = agentDTOs.IndividualId,
                               Operation = JsonConvert.SerializeObject(agentDTOs),
                               ProcessType = AgenDB != null ? "Create" : "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonIntermediary,
                               Proccess = ENUMCOAP.TypeFunction.PersonIntermediary.ToString()
                           }
                        ).OperationId;
                    }
                }
                else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
                {
                    if (agentDTOs.IndividualId > 0)
                    {
                        var AgenDB = DelegateService.uniquePersonService.GetCompanyAgentByIndividualId(agentDTOs.IndividualId);
                        if (AgenDB != null)
                        {
                            var agentModel = DelegateService.uniquePersonService.UpdateCompanyAgent(companyAgent);
                            agentDTO = AplicationAssembler.CreateAgent(agentModel);
                        }
                        else
                        {
                            var agentModel = DelegateService.uniquePersonService.CreateCompanyAgent(companyAgent);
                            agentDTO = AplicationAssembler.CreateAgent(agentModel);
                        }
                    }

                    if (agentDTOs.Agencies != null)
                    {
                        var individualRoles = DelegateService.uniquePersonService.GetIndividualRoleByIndividualId(agentDTOs.IndividualId);
                        if (individualRoles.Count() == 0)
                        {
                            var agentModel = DelegateService.uniquePersonService.CreateCompanyAgentRol(companyAgent);
                        }
                        else
                        {
                            foreach (var item in individualRoles)
                            {
                                if (item.RoleId != Convert.ToInt32(UTILHELPER.EnumHelper.GetEnumParameterValue(PersonKeys.PER_ROL_AGENT)))
                                {
                                    var agentModel = DelegateService.uniquePersonService.CreateCompanyAgentRol(companyAgent);
                                }
                            }
                        }

                        foreach (var item in agentDTOs.Agencies)
                        {
                            CompanyAgency companyAgency = ModelAssembler.CreateAgency(item);
                            if (companyAgency.Id > 0)
                            {                                
                                DelegateService.uniquePersonService.UpdateCompanyAgencyByInvidualId(companyAgency, agentDTOs.IndividualId);
                            }
                            else
                            {
                                item.Id = DelegateService.uniquePersonService.CreateCompanyAgencyByInvidualId(companyAgency, agentDTOs.IndividualId).Id;
                            }

                        }
                    }
                    if (agentDTOs.Prefixes != null)
                    {
                        var companyPrefixModel = DelegateService.uniquePersonService.GetPrefixesByAgentIds(agentDTOs.IndividualId);
                        var prefixDb = AplicationAssembler.CreatePrefixes(companyPrefixModel);

                        var prefixDelete = prefixDb.Where(l2 =>
                                                !agentDTOs.Prefixes.Any(l1 => l1.Id == l2.Id));
                        var prefixInsert = agentDTOs.Prefixes.Where(l2 =>
                                                !prefixDb.Any(l1 => l1.Id == l2.Id));
                        foreach (var item in prefixDelete)
                        {
                            CompanyPrefixs companyPrefix = ModelAssembler.CreatePrefix(item);
                            DelegateService.uniquePersonService.DeletePrefixesByAgentIds(companyPrefix, agentDTOs.IndividualId);
                        }
                        foreach (var item in prefixInsert)
                        {
                            CompanyPrefixs companyPrefix = ModelAssembler.CreatePrefix(item);
                            var PrefixDb = DelegateService.uniquePersonService.CreatePrefixesByAgentIds(companyPrefix, agentDTOs.IndividualId);
                            agentDTO.Prefixes = AplicationAssembler.CreatePrefixes(PrefixDb);
                        }
                    }

                    if (agentDTOs.ComissionAgents != null)
                    {
                        //cambiar estos id por un switch 
                        foreach (var item in agentDTOs.ComissionAgents)
                        {
                            var companyComissionAgent = ModelAssembler.CreateCommissionAgent(item);
                            switch (item.StatusTypeService)
                            {

                                case ModelServices.Enums.StatusTypeService.Create:
                                    item.Id = DelegateService.uniquePersonService.CreateCompanycommissionAgent(companyComissionAgent, agentDTOs.IndividualId, companyComissionAgent.AgentAgencyId).Id;
                                    break;
                                case ModelServices.Enums.StatusTypeService.Update:
                                    DelegateService.uniquePersonService.UpdateCompanycommissionAgent(companyComissionAgent, agentDTOs.IndividualId, companyComissionAgent.AgentAgencyId);
                                    break;
                                case ModelServices.Enums.StatusTypeService.Delete:
                                    DelegateService.uniquePersonService.DeleteCompanycommissionAgent(companyComissionAgent);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                agentDTOs.InfringementPolicies = infringementPolicies;

                return agentDTOs;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
        #region AgentC
        public AgentDTO GetAplicationAgentByIndividualId(int id)
        {

            var agentModel = DelegateService.uniquePersonService.GetCompanyAgentByIndividualId(id);


            AgentDTO result = new AgentDTO();
            if (agentModel != null)
            {

                if (agentModel.EmployeePerson != null)
                {
                    string employe = Convert.ToString(agentModel.EmployeePerson.Id);
                    var personsModel = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(Convert.ToInt32(employe));
                    agentModel.EmployeePerson = new CompanyEmployeePerson
                    {
                        Name = personsModel.Name,
                        MotherLastName = personsModel.SecondSurName,
                        IdCardNo = personsModel.IdentificationDocument.Number,
                    };
                }
            }
            result.DateCurrent = DateTime.Now;
            if (agentModel != null)
            {
                result = AplicationAssembler.CreateAgent(agentModel);
            }

            return result;
        }

        public AgentDTO CreateAplicationAgent(AgentDTO agentDTO)
        {
            CompanyAgent companyAgent = ModelAssembler.CreateAgent(agentDTO);
            var agentModel = DelegateService.uniquePersonService.CreateCompanyAgent(companyAgent);
            var result = AplicationAssembler.CreateAgent(agentModel);
            return result;
        }

        public AgentDTO UpdateAplicationAgent(AgentDTO agentDTO)
        {
            CompanyAgent companyAgent = ModelAssembler.CreateAgent(agentDTO);
            var agentModel = DelegateService.uniquePersonService.UpdateCompanyAgent(companyAgent);
            var result = AplicationAssembler.CreateAgent(agentModel);
            return result;
        }

        public List<AgencyDTO> GetAplicationAgenciesByAgentIdDescription(int agentId, string description)
        {
            var agency = DelegateService.uniquePersonService.GetCompanyAgenciesByAgentIdDescription(agentId, description).Cast<CompanyAgency>().ToList();
            return AplicationAssembler.CreateAgencies(agency);
        }

        public List<AgencyDTO> GetAplicationAgenciesByAgentId(int agentId)
        {
            var agency = DelegateService.uniquePersonService.GetCompanyAgenciesByAgentId(agentId).Cast<CompanyAgency>().ToList();
            return AplicationAssembler.CreateAgencies(agency);
        }

        #endregion

        #region AgencyByIndividualID
        public List<AgencyDTO> GetAplicationAgencyByInvidualID(int IndividualId)
        {
            var agencyCompanyModel = DelegateService.uniquePersonService.GetCompanyAgencyByInvidualId(IndividualId);
            var branch = DelegateService.commonService.GetBranches();
            foreach (var item in agencyCompanyModel)
            {
                foreach (var item2 in branch)
                {
                    if (item.Branch.Id == item2.Id)
                    {
                        item.Branch.Description = item2.Description;
                    }
                }
            }
            var result = AplicationAssembler.CreateAgencies(agencyCompanyModel);
            return result;
        }

        public List<AgencyDTO> GetActiveAplicationAgencyByInvidualID(int IndividualId)
        {
            var agencyCompanyModel = DelegateService.uniquePersonService.GetActiveCompanyAgencyByInvidualId(IndividualId);
            var result = AplicationAssembler.CreateAgencies(agencyCompanyModel);
            return result;
        }

        public List<AgencyDTO> CreateAplicationAgencyByInvidualID(List<AgencyDTO> agencyDTOs, int IndividualId)
        {
            List<CompanyAgency> companyAgency = ModelAssembler.CreateAgencies(agencyDTOs);
            CompanyAgency companyAgencies = new CompanyAgency();
            foreach (var item in companyAgency)
            {
                companyAgencies = DelegateService.uniquePersonService.CreateCompanyAgencyByInvidualId(item, IndividualId);
            }

            var result = AplicationAssembler.CreateAgencies(companyAgencies);
            return result;
        }

        public List<AgencyDTO> UpdateAplicationAgencyByInvidualID(List<AgencyDTO> agencyDTOs, int IndividualId)
        {
            List<CompanyAgency> companyAgency = ModelAssembler.CreateAgencies(agencyDTOs);
            CompanyAgency companyAgencies = new CompanyAgency();
            foreach (var item in companyAgency)
            {
                companyAgencies = DelegateService.uniquePersonService.UpdateCompanyAgencyByInvidualId(item, IndividualId);
            }

            var result = AplicationAssembler.CreateAgencies(companyAgencies);
            return result;
        }
        #endregion AgencyByIndividualID

        #region PrefixAgent
        public List<PrefixDTO> GetPrefixAgentByInvidualId(int IndividualId)
        {
            var agencyCompanyModel = DelegateService.uniquePersonService.GetPrefixesByAgentIds(IndividualId);
            var result = AplicationAssembler.CreatePrefixes(agencyCompanyModel);
            return result;
        }
        public List<PrefixDTO> CreatePrefixAgentByInvidualId(List<PrefixDTO> prefixDTOs, int IndividualId)
        {
            List<CompanyPrefixs> companyPrefix = ModelAssembler.CreatePrefixes(prefixDTOs);
            CompanyPrefixs agencyCompanyModel = new CompanyPrefixs();
            foreach (var item in companyPrefix)
            {
                agencyCompanyModel = DelegateService.uniquePersonService.CreatePrefixesByAgentIds(item, IndividualId);
            }

            var result = AplicationAssembler.CreatePrefixes(agencyCompanyModel);
            return result;
        }
        public List<PrefixDTO> UpdatePrefixAgentByInvidualId(List<PrefixDTO> prefixDTOs, int IndividualId)
        {
            List<CompanyPrefixs> companyPrefix = ModelAssembler.CreatePrefixes(prefixDTOs);
            CompanyPrefixs agencyCompanyModel = new CompanyPrefixs();
            foreach (var item in companyPrefix)
            {
                agencyCompanyModel = DelegateService.uniquePersonService.UpdatePrefixesByAgentIds(item, IndividualId);
            }

            var result = AplicationAssembler.CreatePrefixes(agencyCompanyModel);
            return result;
        }
        public List<PrefixDTO> DeletePrefixAgentByInvidualId(List<PrefixDTO> prefixDTOs, int IndividualId)
        {
            List<CompanyPrefixs> companyPrefix = ModelAssembler.CreatePrefixes(prefixDTOs);
            CompanyPrefixs agencyCompanyModel = new CompanyPrefixs();
            foreach (var item in companyPrefix)
            {
                agencyCompanyModel = DelegateService.uniquePersonService.DeletePrefixesByAgentIds(item, IndividualId);
            }

            var result = AplicationAssembler.CreatePrefixes(agencyCompanyModel);
            return result;
        }
        #endregion

        #region CommissionAgentIndividualId
        public List<ComissionAgentDTO> GetcommissionPorIndividualId(int IndividualId)
        {
            var agencyCompanyModel = DelegateService.uniquePersonService.GetCompanycommissionAgent(IndividualId);
            var prefixAgent = GetPrefixAgentByInvidualId(IndividualId);
            var agenciesAgent = GetAplicationAgencyByInvidualID(IndividualId);
            List<ComissionAgentDTO> listCommision = new List<ComissionAgentDTO>();
            foreach (var item in agencyCompanyModel)
            {
                foreach (var item2 in prefixAgent)
                {
                    if (item.Prefix.Id == item2.Id)
                    {
                        item.Prefix.Description = item2.Description;
                    }
                }
                foreach (var item3 in agenciesAgent)
                {
                    if (item.AgentAgencyId == item3.Id)
                    {
                        item.agency = item3.FullName;
                    }

                }
                var suBLine = DelegateService.commonService.GetSubLinesBusinessByLineBusinessId(item.SubLineBusiness.Id);
                var Line = DelegateService.commonService.GetLineBusinessByPrefixId(item.Prefix.Id);
                foreach (var item4 in suBLine)
                {
                    if (item.SubLineBusiness.Id == item4.Id)
                    {
                        item.SubLineBusiness.Description = item4.Description;
                    }
                }
                foreach (var item5 in Line)
                {
                    if (item.LineBusiness.Id == item5.Id)
                    {
                        item.LineBusiness.Description = item5.Description;
                    }
                }
            }
            listCommision = AplicationAssembler.CreateComissionAgentsa(agencyCompanyModel);
            return listCommision;
        }
        public List<ComissionAgentDTO> CreatecommissionPorIndividualId(List<ComissionAgentDTO> comissionAgentDTOs, int IndividualId, int AgencyId)
        {
            List<CompanyComissionAgent> companyPrefix = ModelAssembler.CreateComissionAgents(comissionAgentDTOs);
            CompanyComissionAgent companyComissionAgent = new CompanyComissionAgent();
            foreach (var item in companyPrefix)
            {
                companyComissionAgent = DelegateService.uniquePersonService.CreateCompanycommissionAgent(item, IndividualId, AgencyId);
            }

            var result = AplicationAssembler.CreateComissionAgents(companyComissionAgent);
            return result;
        }
        public List<ComissionAgentDTO> UpdatecommissionPorIndividualId(List<ComissionAgentDTO> comissionAgentDTOs, int IndividualId, int AgencyId)
        {
            List<CompanyComissionAgent> companyPrefix = ModelAssembler.CreateComissionAgents(comissionAgentDTOs);
            CompanyComissionAgent companyComissionAgent = new CompanyComissionAgent();
            foreach (var item in companyPrefix)
            {
                companyComissionAgent = DelegateService.uniquePersonService.UpdateCompanycommissionAgent(item, IndividualId, AgencyId);
            }

            var result = AplicationAssembler.CreateComissionAgents(companyComissionAgent);
            return result;
        }
        //public List<ComissionAgentDTO> DeletecommissionPorIndividualId(List<ComissionAgentDTO> comissionAgentDTOs, int IndividualId, int AgencyId)
        //{
        //    List<CompanyComissionAgent> companyPrefix = ModelAssembler.CreateComissionAgents(comissionAgentDTOs);
        //    CompanyComissionAgent companyComissionAgent = new CompanyComissionAgent();
        //    foreach (var item in companyPrefix)
        //    {
        //        companyComissionAgent = DelegateService.uniquePersonService.DeleteAgentCommission(companyComissionAgent);
        //    }

        //    var result = AplicationAssembler.CreateComissionAgents(companyComissionAgent);
        //    return result;
        //}
        #endregion

        #endregion

        #region Sarlaft V1 
        public List<IndividualSarlaftDTO> GetIndividualSarlaft(int individualSarlaft)
        {
            List<IndividualSarlaft> companyIndividualSarlaft = DelegateService.uniquePersonService.GetSarlaftByNumberSarlaft(individualSarlaft);
            var result = AplicationAssembler.CreateIndividualSarlafts(companyIndividualSarlaft);
            return result;
        }
        public List<IndividualSarlaftDTO> CreateIndividualSarlaft(List<IndividualSarlaftDTO> individualSarlaftDTOs)
        {
            List<IndividualSarlaft> listIndividualSarlaft = ModelAssembler.CreateIndividualSarlafts(individualSarlaftDTOs);
            IndividualSarlaft companyIndividualSarlaft = new IndividualSarlaft();

            foreach (var item in listIndividualSarlaft)
            {
                companyIndividualSarlaft = DelegateService.uniquePersonService.CreateSarlaftByNumberSarlaft(item);
            }

            var result = AplicationAssembler.CreateIndividualSarlafts(listIndividualSarlaft);
            return result;
        }
        public List<IndividualSarlaftDTO> UpdateIndividualSarlaft(List<IndividualSarlaftDTO> individualSarlaftDTOs)
        {
            List<IndividualSarlaft> listIndividualSarlaft = ModelAssembler.CreateIndividualSarlafts(individualSarlaftDTOs);
            IndividualSarlaft companyIndividualSarlaft = new IndividualSarlaft();
            foreach (var item in listIndividualSarlaft)
            {
                companyIndividualSarlaft = DelegateService.uniquePersonService.UpdateSarlaftByNumberSarlaft(item);
            }

            var result = AplicationAssembler.CreateIndividualSarlafts(listIndividualSarlaft);
            return result;
        }
        #endregion

        #region ReInsurer
        public ReInsurerDTO GetAplicationReInsurerByIndividualId(int individualId)
        {
            var reInsurerModel = DelegateService.uniquePersonService.GetCompanyReInsurerByIndividualId(individualId);
            var result = AplicationAssembler.CreateReInsurer(reInsurerModel);
            return result;
        }

        public ReInsurerDTO CreateAplicationReInsurer(ReInsurerDTO reInsurerDTO, bool validatePolicies = true)
        {
            CompanyReInsurer reInsurerModel = ModelAssembler.CreateReInsurer(reInsurerDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(reInsurerDTO.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(reInsurerDTO.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(reInsurerDTO.IndividualId);

                person.UserId = reInsurerDTO.UserId;
                company.UserId = reInsurerDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesReInsurer(reInsurerModel, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(reInsurerDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    reInsurerDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = reInsurerDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(reInsurerDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonReinsurer,
                               Proccess = ENUMCOAP.TypeFunction.PersonReinsurer.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {

                reInsurerModel = DelegateService.uniquePersonService.CreateCompanyReinsurer(reInsurerModel);
                reInsurerDTO = AplicationAssembler.CreateReInsurer(reInsurerModel);
            }

            reInsurerDTO.InfringementPolicies = infringementPolicies;
            return reInsurerDTO;
        }

        public ReInsurerDTO UpdateAplicationReInsurer(ReInsurerDTO reInsurerDTO, bool validatePolicies = true)
        {
            CompanyReInsurer reInsurerModel = ModelAssembler.CreateReInsurer(reInsurerDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(reInsurerDTO.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(reInsurerDTO.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(reInsurerDTO.IndividualId);

                person.UserId = reInsurerDTO.UserId;
                company.UserId = reInsurerDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesReInsurer(reInsurerModel, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(reInsurerDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    reInsurerDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = reInsurerDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(reInsurerDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonReinsurer,
                               Proccess = ENUMCOAP.TypeFunction.PersonReinsurer.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                reInsurerModel = DelegateService.uniquePersonService.UpdateCompanyReinsurer(reInsurerModel);
                reInsurerDTO = AplicationAssembler.CreateReInsurer(reInsurerModel);
            }
            reInsurerDTO.InfringementPolicies = infringementPolicies;

            return reInsurerDTO;
        }

        #endregion

        #region Partner

        public PartnerDTO GetAplicationPartnerByDocumentIdDocumentTypeIndividualId(String documentId, int documentType, int IndividualId)
        {

            var partnerModel = DelegateService.uniquePersonService.GetCompanyPartnerByDocumentIdDocumentTypeIndividualId(documentId, documentType, IndividualId);
            if (partnerModel != null)
            {
                var result = AplicationAssembler.CreatePartner(partnerModel);
                return result;
            }

            return null;
        }
        /// <summary>
        /// Obtiene un asociado por su id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<PartnerDTO> GetAplicationPartnerByIndividualId(int individualId)
        {
            var partnerModel = DelegateService.uniquePersonService.GetCompanyPartnerByIndividualId(individualId);

            foreach (var item in partnerModel)
            {
                if (item.IdentificationDocument.DocumentType.Id > 3)
                {
                    var typeDocument = DelegateService.uniquePersonAplicationService.GetAplicationDocumentTypes(3);
                    foreach (var item2 in typeDocument)
                    {
                        if (item.IdentificationDocument.DocumentType.Id == item2.Id)
                        {
                            item.IdentificationDocument.DocumentType.Description = item2.SmallDescription;
                        }
                    }
                }

            }



            var result = AplicationAssembler.CreatePartners(partnerModel);
            return result;
        }
        /// <summary>
        /// crea un asociado 
        /// </summary>
        /// <param name="insuredDTO"></param>
        /// <returns></returns>
        public PartnerDTO CreateAplicationPartner(PartnerDTO partnerDTO)
        {
            CompanyPartner provider = ModelAssembler.CreatePartner(partnerDTO);
            CompanyPartner partnerModel = new CompanyPartner();
            //foreach (var item in provider)
            //{
            partnerModel = DelegateService.uniquePersonService.CreateCompanyPartner(provider);


            //}
            var result = AplicationAssembler.CreatePartner(provider);

            return result;


        }
        /// <summary>
        /// Actualizacion de asociado
        /// </summary>
        /// <param name="insuredDTO"></param>
        /// <returns></returns>
        public PartnerDTO UpdateAplicationPartner(PartnerDTO partnerDTO)
        {
            CompanyPartner provider = ModelAssembler.CreatePartner(partnerDTO);
            CompanyPartner partnerModel = new CompanyPartner();

            //foreach (var item in provider)
            //{
            partnerModel = DelegateService.uniquePersonService.UpdateCompanyPartner(provider);
            //}
            var result = AplicationAssembler.CreatePartner(provider);
            return result;
        }

        #endregion

        #region SarlaftPerson
        /// <summary>
        /// Obtiene sarlaft 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SarlaftDTO GetAplicationFinancialSarlaftByIndividualId(int id)
        {
            var sarlaftModel = DelegateService.uniquePersonService.GetCompanyFinancialSarlaftBySarlaftId(id);
            var result = AplicationAssembler.CreateFinancialSarlaft(sarlaftModel);
            return result;
        }
        /// <summary>
        /// creacion de Sarlaft
        /// </summary>
        /// <param name="sarlaftDTO"></param>
        /// <returns></returns>
        public SarlaftDTO CreateAplicationFinancialSarlaft(SarlaftDTO sarlaftDTO)
        {
            FinancialSarlaf fSarlaft = ModelAssembler.CreateFinancialSarlaft(sarlaftDTO);
            var fSarlaftModel = DelegateService.uniquePersonService.CreateCompanyFinancialSarlaft(fSarlaft);
            var result = AplicationAssembler.CreateFinancialSarlaft(fSarlaftModel);
            return result;
        }
        /// <summary>
        /// Actualiza Sarlaft
        /// </summary>
        /// <param name="sarlaftDTO"></param>
        /// <returns></returns>
        public SarlaftDTO UpdateAplicationFinancialSarlaft(SarlaftDTO sarlaftDTO)
        {
            FinancialSarlaf fSarlaft = ModelAssembler.CreateFinancialSarlaft(sarlaftDTO);
            var fSarlaftModel = DelegateService.uniquePersonService.UpdateFinancialSarlaft(fSarlaft);
            var result = AplicationAssembler.CreateFinancialSarlaft(fSarlaftModel);
            return result;
        }
        #endregion

        #region PersonInformationLabor
        /// <summary>
        /// Obtiene la informacion persona ó laboral de una persona.
        /// </summary>
        /// <param name="individualId">Parametro para consultar Informacion de la persona.</param>
        /// <returns>Retorna el Resultado de una persona.</returns>
        //public PersonInformationAndLaborDTO GetPersonJobByIndividualId(int individualId)
        //{
        //    var personModel = DelegateService.uniquePersonService.GetPersonJobByIndividualId(individualId);
        //    var result = AplicationAssembler.CreateInformationPersonLabor(personModel);
        //    return result;
        //}
        ///// <summary>
        ///// Crea la información / laboral de una persona.  
        ///// </summary>
        ///// <param name="personInformationAndLabor">Modelo el encargado de guardar la información por medio de parametros.</param>
        ///// <param name="individualId">Pasar el numero de persona para realizar la asociacion de la información.</param>
        ///// <returns>Retorna la Informacion Creada.</returns>
        //public PersonInformationAndLaborDTO CreatePersonJob(PersonInformationAndLaborDTO personInformationAndLabor, int individualId)
        //{
        //    var rePersonInformationPersonAndLabor = ModelAssembler.CreateInformationPersonLabor(personInformationAndLabor);
        //    rePersonInformationPersonAndLabor = DelegateService.uniquePersonService.CreatePersonJob(rePersonInformationPersonAndLabor,individualId);
        //    var result = AplicationAssembler.CreateInformationPersonLabor(rePersonInformationPersonAndLabor);
        //    return result;
        //}
        ///// <summary>
        ///// Actualiza la información / laboral de una persona
        ///// </summary>
        ///// <param name="personInformationAndLabor">Modelo que actualiza los datos e informacion laboral </param>
        ///// <returns>Retorna la Informacion actualizada.</returns>
        //public PersonInformationAndLaborDTO UpdatePersonJob(PersonInformationAndLaborDTO personInformationAndLabor)
        //{
        //    var rePersonInformationPersonAndLabor = ModelAssembler.CreateInformationPersonLabor(personInformationAndLabor);
        //    rePersonInformationPersonAndLabor = DelegateService.uniquePersonService.UpdatePersonJob(rePersonInformationPersonAndLabor);
        //    var result = AplicationAssembler.CreateInformationPersonLabor(rePersonInformationPersonAndLabor);
        //    return result;
        //}
        #endregion

        #region OperatingQuota
        /// <summary>
        /// Agrega un nuevo OperatingQuotaDTO a la  lista 
        /// </summary>
        /// <param name="ListOperatingQuotaDTO"></param>
        /// <returns>Lista de OperatingQuotaDTO</returns>
        public List<OperatingQuotaDTO> CreateOperatingQuota(List<OperatingQuotaDTO> ListOperatingQuotaDTO, List<OperatingQuotaEventDTO> operatingQuotaEventDTOs, bool validatePolicies = true)
        {
            List<CompanyOperatingQuota> listOperatingQuotaToSave = ModelAssembler.CreateOperatingQuotas(ListOperatingQuotaDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(ListOperatingQuotaDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(ListOperatingQuotaDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(ListOperatingQuotaDTO.First().IndividualId);

                person.UserId = ListOperatingQuotaDTO.First().UserId;
                company.UserId = ListOperatingQuotaDTO.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));

                foreach (CompanyOperatingQuota operatingQuotaToSave in listOperatingQuotaToSave)
                {
                    infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesOperatingQuota(operatingQuotaToSave, person, company, addresses));
                }

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(ListOperatingQuotaDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    ListOperatingQuotaDTO[0].OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = ListOperatingQuotaDTO[0].IndividualId,
                               Operation = JsonConvert.SerializeObject(new { ListOperatingQuotaDTO, operatingQuotaEventDTOs }),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonOperatingQuota,
                               Proccess = ENUMCOAP.TypeFunction.PersonOperatingQuota.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                var ListOperatingQuotaModel = DelegateService.uniquePersonService.CreateCompanyOperatingQuota(listOperatingQuotaToSave);
                DelegateService.operationQuotaService.InsertOperatingQuotaEvent(operatingQuotaEventDTOs);
                ListOperatingQuotaDTO = AplicationAssembler.CreateOperatingQuotaDTOs(ListOperatingQuotaModel);
            }

            ListOperatingQuotaDTO.First().InfringementPolicies = infringementPolicies;

            return ListOperatingQuotaDTO;
        }

        /// <summary>
        /// Consulta OperatingQuotaDTO por individualId
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<OperatingQuotaDTO> GetOperatingQuotaByIndividualId(int individualId)
        {
            var ListOperatingQuotaModel = DelegateService.uniquePersonService.GetCompanyOperatingQuotaByIndividualId(individualId);
            var result = AplicationAssembler.CreateOperatingQuotaDTOs(ListOperatingQuotaModel);
            return result;
        }

        /// <summary>
        /// Borra el operatingQuotaDTO que se pasa como parametro
        /// </summary>
        /// <param name="operatingQuotaDTO"></param>
        /// <returns></returns>
        public bool DeleteOperatingQuota(OperatingQuotaDTO operatingQuotaDTO)
        {
            CompanyOperatingQuota OperatingQuotaToDelete = ModelAssembler.CreateOperatingQuota(operatingQuotaDTO);
            return DelegateService.uniquePersonService.DeleteCompanyOperatingQuota(OperatingQuotaToDelete);
        }

        /// <summary>
        /// Actualiza el OperatingQuotaDTO que se pasa como parametro
        /// </summary>
        /// <param name="operatingQuotaDTO"></param>
        /// <returns></returns>
        public OperatingQuotaDTO UpdateOperatingQuota(OperatingQuotaDTO operatingQuotaDTO)
        {
            CompanyOperatingQuota OperatingQuotaToUpdate = ModelAssembler.CreateOperatingQuota(operatingQuotaDTO);
            var operatingQuotaUpdateModel = DelegateService.uniquePersonService.UpdateCompanyOperatingQuota(OperatingQuotaToUpdate);
            var result = AplicationAssembler.CreateOperatingQuotaDTO(operatingQuotaUpdateModel);
            return result;
        }
        #endregion

        #region IndividualTaxExeption

        /// <summary>
        /// Obtiene el impuesto individual por lista
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<IndividualTaxExeptionDTO> GetIndividualTaxExeptionByIndividualId(int individualId)
        {
            var ListIndividualTaxExeptionModel = DelegateService.uniquePersonService.GetCompanyIndividualTaxExeptionByIndividualId(individualId);

            var result = AplicationAssembler.CreateIndividualTaxExeptionDTOs(ListIndividualTaxExeptionModel);
            return result;

        }

        /// <summary>
        ///  Crea el impuesto
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTO"></param>
        /// <returns></returns>
        public List<IndividualTaxExeptionDTO> CreateIndividualTax(List<IndividualTaxExeptionDTO> listIndividualTaxDTO, bool validatePolicies = true)
        {
            List<CompanyIndividualTax> individualTaxModel = ModelAssembler.CreateIndividualTaxes(listIndividualTaxDTO);
            List<CompanyIndividualTaxExeption> individualTaxExpetionModel = ModelAssembler.CreateIndividualTaxesExeptions(listIndividualTaxDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(listIndividualTaxDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(listIndividualTaxDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(listIndividualTaxDTO.First().IndividualId);

                person.UserId = listIndividualTaxDTO.First().UserId;
                company.UserId = listIndividualTaxDTO.First().UserId;
                var policiesPerson = DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses);

                for (int j = 0; j < individualTaxModel.Count; j++)
                {
                    individualTaxModel[j].IndividualTaxExeption = individualTaxExpetionModel[j];
                    if (listIndividualTaxDTO[j].InfringementPolicies == null)
                    {
                        listIndividualTaxDTO[j].InfringementPolicies = new List<PoliciesAut>();
                    }

                    if (j == 0 && policiesPerson.Count() > 0)
                    {
                        listIndividualTaxDTO[j].InfringementPolicies.AddRange(policiesPerson);
                    }
                    listIndividualTaxDTO[j].InfringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesTaxes(individualTaxModel[j], person, company, addresses));
                    infringementPolicies.AddRange(listIndividualTaxDTO[j].InfringementPolicies);
                }

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(listIndividualTaxDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    listIndividualTaxDTO.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = listIndividualTaxDTO.First().IndividualId,
                               Operation = JsonConvert.SerializeObject(listIndividualTaxDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonTaxes,
                               Proccess = ENUMCOAP.TypeFunction.PersonTaxes.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                for (int j = 0; j < individualTaxModel.Count; j++)
                {
                    CompanyIndividualTax item = individualTaxModel[j];
                    CompanyIndividualTax companyIndividualTax = DelegateService.uniquePersonService.CreateCompanyIndividualTax(item);//CREA TABLA 1
                    CompanyIndividualTaxExeption companyIndividualTaxExeption = DelegateService.uniquePersonService.CreateCompanyIndividualTaxExeption(individualTaxExpetionModel[j]);//crea tabla 2
                    listIndividualTaxDTO[j] = AplicationAssembler.CreateIndividualTaxExeptionDTO(companyIndividualTax, companyIndividualTaxExeption);
                }
            }

            listIndividualTaxDTO.First().InfringementPolicies = infringementPolicies;

            return listIndividualTaxDTO;
        }

        /// <summary>
        /// Actualiza los datos del impuesto individual
        /// </summary>
        /// <param name="individualTaxExeptionDTO"></param>
        /// <returns></returns>
        public List<IndividualTaxExeptionDTO> UpdateIndividualTaxExeption(List<IndividualTaxExeptionDTO> individualTaxExeptionDTO, bool validatePolicies = true)
        {
            List<CompanyIndividualTax> individualTaxModel = ModelAssembler.CreateIndividualTaxes(individualTaxExeptionDTO);
            List<CompanyIndividualTaxExeption> individualTaxExpetionModel = ModelAssembler.CreateIndividualTaxesExeptions(individualTaxExeptionDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(individualTaxExeptionDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(individualTaxExeptionDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(individualTaxExeptionDTO.First().IndividualId);

                person.UserId = individualTaxExeptionDTO.First().UserId;
                company.UserId = individualTaxExeptionDTO.First().UserId;

                var policiesPerson = DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses);
                for (int j = 0; j < individualTaxModel.Count; j++)
                {
                    individualTaxModel[j].IndividualTaxExeption = individualTaxExpetionModel[j];
                    if (individualTaxExeptionDTO[j].InfringementPolicies == null)
                    {
                        individualTaxExeptionDTO[j].InfringementPolicies = new List<PoliciesAut>();
                    }

                    if (j == 0 && policiesPerson.Count() > 0)
                    {
                        individualTaxExeptionDTO[j].InfringementPolicies.AddRange(policiesPerson);
                    }
                    individualTaxExeptionDTO[j].InfringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesTaxes(individualTaxModel[j], person, company, addresses));
                    infringementPolicies.AddRange(individualTaxExeptionDTO[j].InfringementPolicies);
                }

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(individualTaxExeptionDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    individualTaxExeptionDTO.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = individualTaxExeptionDTO.First().IndividualId,
                               Operation = JsonConvert.SerializeObject(individualTaxExeptionDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonTaxes,
                               Proccess = ENUMCOAP.TypeFunction.PersonTaxes.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                for (int j = 0; j < individualTaxModel.Count; j++)
                {
                    CompanyIndividualTax item = individualTaxModel[j];
                    CompanyIndividualTax companyIndividualTax = DelegateService.uniquePersonService.UpdateCompanyIndividualTax(item);//Actualiza tabla 1
                    CompanyIndividualTaxExeption companyIndividualTaxExeption = DelegateService.uniquePersonService.UpdateCompanyIndividualTaxExeption(individualTaxExpetionModel[j]);//Actuaiza tabla 2
                    individualTaxExeptionDTO[j] = AplicationAssembler.CreateIndividualTaxExeptionDTO(companyIndividualTax, companyIndividualTaxExeption);
                }
            }

            individualTaxExeptionDTO.First().InfringementPolicies = infringementPolicies;
            return individualTaxExeptionDTO;
        }

        /// <summary>
        /// Elimina los datos del impuesto individual
        /// </summary>
        /// <param name="individualTaxExeptionDTO"></param>
        public void DeleteIndividualTaxExeption(IndividualTaxExeptionDTO individualTaxExeptionDTO)
        {

            CompanyIndividualTax individualTaxModel = ModelAssembler.CreateIndividualTax(individualTaxExeptionDTO);
            CompanyIndividualTaxExeption individualTaxExeptionModel = ModelAssembler.CreateIndividualTaxExeption(individualTaxExeptionDTO);

            DelegateService.uniquePersonService.DeleteCompanyIndividualTaxExeption(individualTaxModel);
            DelegateService.uniquePersonService.DeleteCompanyIndividualTax(individualTaxExeptionModel);

            var result = AplicationAssembler.CreateIndividualTaxExeptionDTO(individualTaxModel, individualTaxExeptionModel);


        }

        #endregion

        #region ProspectPersonNatural
        public ProspectPersonNaturalDTO GetProspectNaturalByDocumentNumber(string documentNum)
        {
            var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectNaturalByDocumentNumber(documentNum);
            if (ProspectPersonNatural.IdCardNo != null)
            {
                return AplicationAssembler.CreateProspectPersonNatural(ProspectPersonNatural);
            }
            else
            {
                return null;
            }

        }

        public ProspectPersonNaturalDTO GetProspectPersonNatural(int individualId)
        {
            var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectPersonNatural(individualId);
            return AplicationAssembler.CreateProspectPersonNatural(ProspectPersonNatural);
        }

        public ProspectPersonNaturalDTO CreateProspectPersonNatural(ProspectPersonNaturalDTO prospectPersonNaturalDTO)
        {
            CompanyProspectNatural prospectNatural = ModelAssembler.CreateProspectNatural(prospectPersonNaturalDTO);
            if (prospectPersonNaturalDTO.ProspectCode == 0)
            {
                prospectNatural = DelegateService.uniquePersonService.CreateProspectPersonNatural(prospectNatural);
                return AplicationAssembler.CreateProspectPersonNatural(prospectNatural);
            }
            else
            {
                return AplicationAssembler.CreateProspectPersonNatural(DelegateService.uniquePersonService.UpdateProspectPersonNatural(prospectNatural));
            }

        }

        public ProspectPersonNaturalDTO GetProspectByDocumentNumber(string documentNum, int searchType)
        {
            var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectByDocumentNumber(documentNum, searchType);
            return AplicationAssembler.CreateProspectPersonNatural(ProspectPersonNatural);
        }

        #endregion

        #region ProspectLegal

        public ProspectLegalDTO GetProspectLegalByDocumentNumber(string documentNum)
        {
            var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectLegalByDocumentNumber(documentNum);
            return AplicationAssembler.CreateProspectLegalDTO(ProspectPersonNatural);
        }

        public ProspectLegalDTO GetProspectPersonLegal(int individualId)
        {
            var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectPersonLegal(individualId);
            return AplicationAssembler.CreateProspectLegalDTO(ProspectPersonNatural);
        }

        /// <summary>
        /// Crea el prospect legal
        /// </summary>
        /// <param name="prospectLegalDTO"></param>
        /// <returns></returns>
        public ProspectLegalDTO CreateProspectLegal(ProspectLegalDTO prospectLegalDTO)
        {
            CompanyProspectNatural prospectNatural = ModelAssembler.CreateProspectLegalModel(prospectLegalDTO);
            if (prospectLegalDTO.ProspectCode == 0)
            {
                return AplicationAssembler.CreateProspectLegalDTO(DelegateService.uniquePersonService.CreateProspectPersonLegal(prospectNatural));
            }
            else
            {
                return AplicationAssembler.CreateProspectLegalDTO(DelegateService.uniquePersonService.UpdateProspectPersonLegal(prospectNatural));
            }
        }

        /// <summary>
        /// Obtiene los datos de la compania por numero de documento, nombre o tipo. 
        /// </summary>
        /// <param name="documentNumber"></param>
        /// <param name="name"></param>
        /// <param name="searchType"></param>
        /// <returns></returns>
        public List<CompanyDTO> GetCompaniesByDocumentNumberNameSearchType(string documentNumber, string name, int searchType)
        {
            List<UniquePersonServices.V1.Models.CompanyCompany> ListCompanyModel = null;// DelegateService.uniquePersonService.GetCompaniesByDocumentNumberNameSearchType(documentNumber, name, searchType);
            var result = AplicationAssembler.CreateCompanies(ListCompanyModel);
            return result;

        }

        public List<ProspectLegalDTO> GetAplicationProspectLegalAdv(ProspectLegalDTO companyDTO)
        {
            List<CompanyProspectNatural> companiesModels = new List<CompanyProspectNatural>();
            CompanyProspectNatural company = ModelAssembler.CreateProspectLegal(companyDTO);
            companiesModels.Add(DelegateService.uniquePersonService.GetCompanyProspectLegalAdv(CustomerType.Prospect, company));
            var companies = AplicationAssembler.CreateProspectLegalDTOs(companiesModels);

            return companies;
        }

        public List<ProspectPersonNaturalDTO> GetAplicationProspectNaturalAdv(ProspectPersonNaturalDTO companyDTO)
        {
            List<CompanyProspectNatural> companiesModels = new List<CompanyProspectNatural>();
            CompanyProspectNatural company = ModelAssembler.CreateProspectNaturalModel(companyDTO);
            companiesModels.Add(DelegateService.uniquePersonService.GetCompanyProspectNaturalAdv(CustomerType.Prospect, company));
            var companies = AplicationAssembler.CreateProspectPersonNaturals(companiesModels);

            return companies;
        }

        #endregion


        //
        ///// <summary>
        ///// Obtiene Consorciado por Asegurado y por Individuo
        ///// </summary>
        ///// <param name="InsureCode">Id asegurado </param>
        ///// <param name="IndividualId">Id Individuo</param>
        ///// <returns>Se Retorna Consorciados</returns>
        //public ConsorciatedDTO GetConsortiumInsuredCodeAndIndividualID(int InsureCode, int IndividualId)
        //{
        //    BusinessPerson businessPerson = new BusinessPerson();
        //    return businessPerson.GetConsortiumInsuredCodeAndIndividualID(InsureCode, IndividualId);
        //}
        /// <summary>
        /// Obtiene los Consorciados por Id asegurado
        /// </summary>
        /// <param name="InsureCode">Id Asegurado</param>
        /// <returns>Retorna Consorciados Por Asegurado.</returns>
        #region Consortiums
        public List<ConsorciatedDTO> GetConsortiumByIndividualId(int individualId)
        {
            var insured = DelegateService.uniquePersonService.GetInsuredByIndividualId(individualId);
            if (insured != null)
            {
                var consortiums = DelegateService.uniquePersonService.GetCoConsortiumsByInsuredCod(insured.InsuredCode);
                var consorciatedDTOs = AplicationAssembler.CreateConsortiums(consortiums);
                return consorciatedDTOs;
            }
            else
            {
                return new List<ConsorciatedDTO>();
            }
        }
        /// <summary>
        /// Crea Consorciados para Un Individuo
        /// </summary>
        /// <param name="model">Informacion Consorciado</param>
        /// <returns>Retorna Informacion Consorciado Creado</returns>

        public List<ConsorciatedDTO> CreateConsortium(List<ConsorciatedDTO> consorciatedDTOs, int individualId, bool validatePolicies = true)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();

            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(individualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(individualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(individualId);

                person.UserId = consorciatedDTOs.First().UserId;
                company.UserId = consorciatedDTOs.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));

                foreach (ConsorciatedDTO consorciated in consorciatedDTOs)
                {
                    infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(ModelAssembler.CreateConsortium(consorciated), person, company, addresses));
                }

            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    consorciatedDTOs.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = individualId,
                               Operation = JsonConvert.SerializeObject(consorciatedDTOs),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonConsortiates,
                               Proccess = ENUMCOAP.TypeFunction.PersonConsortiates.ToString()
                           }
                   ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                var updConsorciatedDTO = consorciatedDTOs.Where(m => m.ConsortiumId > 0).ToList();
                var insConsorciatedDTO = consorciatedDTOs.Where(m => m.ConsortiumId == 0).ToList();
                List<ConsorciatedDTO> consurtimDto = new List<ConsorciatedDTO>();
                //Consortium event
                if (consorciatedDTOs.First().ConsortiumEventDTO != null)
                {
                    DelegateService.consortiumIntegrationService.CreateConsortiumEvent(ModelAssembler.CreateConsortiumEvent(consorciatedDTOs.First().ConsortiumEventDTO));
                }
                if (consorciatedDTOs.First().ConsortiumEventDTOs.Count > 0)
                {
                    List<ConsortiumEventDTO> eventDTOs = new List<ConsortiumEventDTO>();
                    eventDTOs = DelegateService.consortiumIntegrationService.AssigendIndividualToConsotium(ModelAssembler.CreateConsortiumEvents(consorciatedDTOs.First().ConsortiumEventDTOs));
                }

                foreach (ConsorciatedDTO item in insConsorciatedDTO)
                {
                    var insured = DelegateService.uniquePersonService.GetInsuredByIndividualId(individualId);
                    if (insured != null)
                    {

                        var companyConsortium = ModelAssembler.CreateConsortium(item);
                        companyConsortium.InsuredCode = insured.InsuredCode;
                        var ResultCompanyConsortium = DelegateService.uniquePersonService.CreateCompanyConsortium(companyConsortium);
                        consurtimDto.Add(AplicationAssembler.CreateConsortium(ResultCompanyConsortium));
                    }
                    else
                    {
                        throw (new BusinessException("Debe existir un asegurado"));
                    }
                }
                consorciatedDTOs = consurtimDto;
                consorciatedDTOs.AddRange(UpdateConsortium(updConsorciatedDTO, false));

            }
            consorciatedDTOs.First().InfringementPolicies = infringementPolicies;
            return consorciatedDTOs;
        }
        /// <summary>
        /// Actualiza Informacion Del Consorciado.
        /// </summary>
        /// <param name="model">Model Para Actualizar consorciados</param>
        /// <returns>Retorna la actualizacion DE Consorciados.</returns>

        public List<ConsorciatedDTO> UpdateConsortium(List<ConsorciatedDTO> consorciatedDTO, bool validatePolicies = true)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(consorciatedDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(consorciatedDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(consorciatedDTO.First().IndividualId);

                person.UserId = consorciatedDTO.First().UserId;
                company.UserId = consorciatedDTO.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                foreach (ConsorciatedDTO consorciated in consorciatedDTO)
                {
                    MOCOUP.Insured insured = DelegateService.uniquePersonService.GetInsuredByIndividualId(consorciated.IndividualId);
                    if (insured != null)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(ModelAssembler.CreateConsortium(consorciated), person, company, addresses));
                    }
                }

                consorciatedDTO.First().InfringementPolicies = infringementPolicies;
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    consorciatedDTO.First().OperationId = this.CreateAplicationPersonOperation(
                          new PersonOperationDTO
                          {
                              IndividualId = consorciatedDTO.First().IndividualId,
                              Operation = JsonConvert.SerializeObject(consorciatedDTO),
                              ProcessType = "Update",
                              FunctionId = (int)ENUMCOAP.TypeFunction.PersonConsortiates,
                              Proccess = ENUMCOAP.TypeFunction.PersonConsortiates.ToString()
                          }
                  ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                List<ConsorciatedDTO> consurtimDto = new List<ConsorciatedDTO>();
                foreach (var item in consorciatedDTO)
                {
                    var companyConsortium = ModelAssembler.CreateConsortium(item);
                    var ResultCompanyConsortium = DelegateService.uniquePersonService.UpdateCompanyConsortium(companyConsortium);
                    consurtimDto.Add(AplicationAssembler.CreateConsortium(ResultCompanyConsortium));
                }

                consorciatedDTO = consurtimDto;
            }

            consorciatedDTO.First().InfringementPolicies = infringementPolicies;
            return consorciatedDTO;
        }
        /// <summary>
        /// Elimina Consorciados Por Asegurado 
        /// </summary>
        /// <param name="InsuredID">Id asegurado</param>
        /// <returns>Retorna la Eliminación del Consorciado.</returns>

        public bool DeleteConsortium(ConsorciatedDTO consorciatedDTO)
        {
            CompanyConsortium consortiumToModel = ModelAssembler.CreateConsortium(consorciatedDTO);
            CompanyConsortium companyConsortium = new CompanyConsortium();
            bool asigna = DelegateService.uniquePersonService.DeleteCompanyConsortium(consortiumToModel);
            return asigna;
        }
        #endregion

        #region CompanyCoInsured

        public CompanyCoInsuredDTO CreateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO, bool validatePolicies = true)
        {
            CompanyCoInsured companyCoInsured = ModelAssembler.CreateCompanyCoInsureds(companyCoInsuredDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(companyCoInsuredDTO.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(companyCoInsuredDTO.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(companyCoInsuredDTO.IndividualId);

                person.UserId = companyCoInsuredDTO.UserId;
                company.UserId = companyCoInsuredDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesCoInsured(companyCoInsured, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(companyCoInsuredDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    companyCoInsuredDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = companyCoInsuredDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(companyCoInsuredDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonCoinsurer,
                               Proccess = ENUMCOAP.TypeFunction.PersonCoinsurer.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                var CoinSured = DelegateService.uniquePersonService.CreateCoInsuredIndividuald(companyCoInsured);
                companyCoInsuredDTO = AplicationAssembler.CreateCompanyCoInsureds(CoinSured);
            }
            companyCoInsuredDTO.InfringementPolicies = infringementPolicies;
            return companyCoInsuredDTO;
        }

        public CompanyCoInsuredDTO UpdateCompanyCoInsured(CompanyCoInsuredDTO companyCoInsuredDTO, bool validatePolicies = true)
        {
            CompanyCoInsured companyCoInsured = ModelAssembler.CreateCompanyCoInsureds(companyCoInsuredDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(companyCoInsuredDTO.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(companyCoInsuredDTO.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(companyCoInsuredDTO.IndividualId);
                person.UserId = companyCoInsuredDTO.UserId;
                company.UserId = companyCoInsuredDTO.UserId;
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesCoInsured(companyCoInsured, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(companyCoInsuredDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    companyCoInsuredDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = companyCoInsuredDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(companyCoInsuredDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonCoinsurer,
                               Proccess = ENUMCOAP.TypeFunction.PersonCoinsurer.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                var CoinSured = DelegateService.uniquePersonService.UpdateCoInsuredIndividuald(companyCoInsured);
                companyCoInsuredDTO = AplicationAssembler.CreateCompanyCoInsureds(CoinSured);
            }
            companyCoInsuredDTO.InfringementPolicies = infringementPolicies;
            return companyCoInsuredDTO;
        }

        public CompanyCoInsuredDTO GetCompanyCoInsuredIndivualID(int IndividualId)
        {
            var CoinSured = DelegateService.uniquePersonService.GetCoInsuredIndividuald(IndividualId);
            if (CoinSured != null)
            {
                var result = AplicationAssembler.CreateCompanyCoInsureds(CoinSured);
                return result;
            }
            else
            {
                return null;
            }
        }

        public CompanyCoInsuredDTO GetCompanyCoInsured(string IdTributary)
        {

            var CoinSured = DelegateService.uniquePersonService.GetCompanyCoInsuredTributary(IdTributary);
            var result = AplicationAssembler.CreateCompanyCoInsured(CoinSured);
            return result;
        }



        public CompanyCoInsuredDTO CreateInsurerByExistingCompany(CompanyCoInsuredDTO companyCoInsuredDTO)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CompanyDTO companyDTO = GetAplicationCompanyById(companyCoInsuredDTO.IndividualId);

            companyCoInsuredDTO.TributaryIdNo = companyCoInsuredDTO.TributaryIdNo;
            companyCoInsuredDTO.Description = companyDTO.BusinessName; //.TradeName;
            companyCoInsuredDTO.CountryCode = companyDTO.Addresses.Where(x => x.IsPrincipal == true).Select(x => x.CountryId).First();
            companyCoInsuredDTO.PhoneNumber = companyDTO.Phones.Where(x => x.IsPrincipal == true).Select(x => x.Description).First();
            companyCoInsuredDTO.PhoneTypeCode = companyDTO.Phones.Where(x => x.IsPrincipal == true).Select(x => x.PhoneTypeId).First();
            companyCoInsuredDTO.AddressTypeCode = companyDTO.Addresses.Select(x => x.AddressTypeId).First();
            companyCoInsuredDTO.CountryCode = companyDTO.Addresses.Select(x => x.CountryId).First();
            companyCoInsuredDTO.StateCode = companyDTO.Addresses.Select(x => x.StateId).First();
            companyCoInsuredDTO.CityCode = companyDTO.Addresses.Select(x => x.CityId).First();
            companyCoInsuredDTO.Street = companyDTO.Addresses.Select(x => x.Description).First();

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), Resources.Errors.ErrorCreateInsurerByExistingCompany);

            return companyCoInsuredDTO;
        }

        #endregion CompanyCoInsured

        #region Guarantees

        public GuaranteeDTO GetInsuredGuaranteeByIdGuarantee(int guaranteId)
        {
            //var guaranteeModel = DelegateService.uniquePersonService.GetInsuredGuaranteeByIdGuarantee(guaranteId);
            //var guarantee = AplicationAssembler.CreateGuarantee(guaranteeModel);

            //var listGuarantors = DelegateService.uniquePersonService.GetGuarantorsByGuaranteeId(guarantee.InsuredGuaranteeId);
            //guarantee.Guarantors = AplicationAssembler.CreateGuarantors(listGuarantors);

            //var listDocumentation = DelegateService.uniquePersonService.GetInsuredGuaranteeDocumentation(guarantee.IndividualId, guarantee.InsuredGuaranteeId);
            //guarantee.listDocumentation = AplicationAssembler.CreateInsuredGuaranteeDocumentation(listDocumentation);


            //var listPrefix = DelegateService.uniquePersonService.GetInsuredGuaranteePrefix(guarantee.IndividualId, guarantee.InsuredGuaranteeId);
            //guarantee.listPrefix = AplicationAssembler.CreateInsuredGuaranteePrefixies(listPrefix);

            return null;
        }

        public List<GuaranteeDTO> GetInsuredGuaranteesByIndividualId(int id)
        {
            //var guarantiesModel = DelegateService.uniquePersonService.GetInsuredGuaranteesByIndividualId(id);
            //var guaranties = AplicationAssembler.CreateGuaranties(guarantiesModel);

            //foreach (var guarantee in guaranties)
            //{
            //    var listGuarantors = DelegateService.uniquePersonService.GetGuarantorsByGuaranteeId(guarantee.InsuredGuaranteeId);
            //    guarantee.Guarantors = AplicationAssembler.CreateGuarantors(listGuarantors);

            //    var listDocumentation = DelegateService.uniquePersonService.GetInsuredGuaranteeDocumentation(guarantee.IndividualId, guarantee.InsuredGuaranteeId);
            //    guarantee.listDocumentation = AplicationAssembler.CreateInsuredGuaranteeDocumentation(listDocumentation);

            //    var listPrefix = DelegateService.uniquePersonService.GetInsuredGuaranteePrefix(guarantee.IndividualId, guarantee.InsuredGuaranteeId);
            //    guarantee.listPrefix = AplicationAssembler.CreateInsuredGuaranteePrefixies(listPrefix);
            //}
            return null;
        }

        public List<GuaranteeDTO> SaveInsuredGuarantees(List<GuaranteeDTO> listGuarantee)
        {
            var guaranties = ModelAssembler.CreateGuaranties(listGuarantee);
            var guarantiesModel = DelegateService.uniquePersonService.SaveInsuredGuarantees(guaranties);
            return AplicationAssembler.CreateGuaranties(guarantiesModel);
        }

        public GuaranteeDTO SaveInsuredGuarantee(GuaranteeDTO guarantee)
        {
            var guaranteeModel = ModelAssembler.CreateGuarantee(guarantee);
            guaranteeModel = DelegateService.uniquePersonService.SaveInsuredGuarantee(guaranteeModel);
            return AplicationAssembler.CreateGuarantee(guaranteeModel);
        }

        public GuaranteeDto SaveApplicationInsuredGuarantee(GuaranteeDto guaranteeDto, bool validatePolicies = true)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(guaranteeDto.Guarantee.InsuredGuarantee.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(guaranteeDto.Guarantee.InsuredGuarantee.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(guaranteeDto.Guarantee.InsuredGuarantee.IndividualId);

                person.UserId = guaranteeDto.UserId;
                company.UserId = guaranteeDto.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesGuarantee(guaranteeDto.Guarantee, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(guaranteeDto.Guarantee.InsuredGuarantee.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    guaranteeDto.OperationId = this.CreateAplicationPersonOperation(
                        new PersonOperationDTO
                        {
                            IndividualId = guaranteeDto.Guarantee.InsuredGuarantee.IndividualId,
                            Operation = JsonConvert.SerializeObject(guaranteeDto),
                            ProcessType = "Create",
                            FunctionId = (int)ENUMCOAP.TypeFunction.PersonGuarantees,
                            Proccess = ENUMCOAP.TypeFunction.PersonGuarantees.ToString()
                        }
                    ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                MOCOUP.Guarantee guaranteeModel = DelegateService.uniquePersonService.SaveInsuredGuarantee(guaranteeDto.Guarantee);
                guaranteeDto.Guarantee = guaranteeModel;
                if (guaranteeDto != null && guaranteeDto.Guarantee != null && guaranteeDto.Guarantee.InsuredGuarantee != null && guaranteeDto.Guarantee.InsuredGuarantee.IndividualId != null)
                    DelegateService.uniquePersonService.CreateIntegrationNotification(guaranteeDto.Guarantee.InsuredGuarantee.IndividualId, (int)UniquePersonService.Enums.Peripheraltype.COUNT_GUAR);
            }

            guaranteeDto.InfringementPolicies = infringementPolicies;
            return guaranteeDto;
        }

        public List<InsuredGuaranteePrefixDTO> GetInsuredGuaranteePrefix(int individualId, int guaranteeId)
        {
            var prefix = DelegateService.uniquePersonService.GetInsuredGuaranteePrefix(individualId, guaranteeId);
            return AplicationAssembler.CreateInsuredGuaranteePrefixies(prefix);
        }


        #endregion

        #region IndividualSarlaft
        public List<IndividualSarlaftDTO> GetSarlaftByIndividualId(int IndividualId)
        {
            var sarlaftModel = DelegateService.uniquePersonService.GetIndividualSarlaftByIndividualId(IndividualId);
            var result = AplicationAssembler.CreateSarlaftPErsonNaturals(sarlaftModel);
            return result;
        }
        public IndividualSarlaftDTO CreateSarlaftByIndividualId(IndividualSarlaftDTO individualSarlaftDTOs, int IndividualId, int ActivityEconomic)
        {
            IndividualSarlaft individualSarlaft = ModelAssembler.CreateSarlaftPErsonNatural(individualSarlaftDTOs);
            individualSarlaft.AuthorizedBy = individualSarlaftDTOs.AuthorizedBy;
            individualSarlaft.UserId = individualSarlaftDTOs.UserId;
            var sarlaftModel = DelegateService.uniquePersonService.CreateIndividualSarlaftByIndividualId(individualSarlaft, IndividualId, ActivityEconomic);
            var result = AplicationAssembler.CreateSarlaftPErsonNatural(sarlaftModel);
            return result;
        }
        public IndividualSarlaftDTO UpdateSarlaftByIndividualId(IndividualSarlaftDTO individualSarlafts)
        {
            IndividualSarlaft individualSarlaft = ModelAssembler.CreateSarlaftPErsonNatural(individualSarlafts);
            var sarlaftModel = DelegateService.uniquePersonService.UpdateIndividualSarlaftByIndividualIds(individualSarlaft);
            var result = AplicationAssembler.CreateSarlaftPErsonNatural(sarlaftModel);
            return result;
        }

        #endregion

        #region LabourPerson
        // <summary>
        // Obtiene la informacion laboral de una persona.
        // </summary>
        // <param name = "individualId" > Parametro para consultar Informacion de la persona.</param>
        // <returns>Retorna el Resultado de una persona.</returns>
        public PersonInformationAndLabourDTO GetApplicationLabourPersonByIndividualId(int individualId)
        {
            PersonInformationAndLabourDTO PersonLabourDto = new PersonInformationAndLabourDTO();
            var labourpersonModel = DelegateService.uniquePersonService.GetCompanyLabourPersonByIndividualId(individualId);
            if (labourpersonModel != null)
            {
                PersonLabourDto = AplicationAssembler.CreateLabourPerson(labourpersonModel);
            }

            return PersonLabourDto;
        }
        /// <summary>
        /// Crea la informacion  laboral de una persona.  
        /// </summary>
        /// <param name="personInformationAndLabor">Modelo el encargado de guardar la información por medio de parametros.</param>
        /// <param name="individualId">Pasar el numero de persona para realizar la asociacion de la información.</param>
        /// <returns>Retorna la Informacion Creada.</returns>
        public PersonInformationAndLabourDTO CreateApplicationLabourPerson(PersonInformationAndLabourDTO personInformationAndLabour, int individualId, bool validatePolicies = true)
        {
            CompanyLabourPerson labourPerson = ModelAssembler.CreateInformationLabourPerson(personInformationAndLabour);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(individualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(individualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(individualId);

                person.UserId = personInformationAndLabour.UserId;
                company.UserId = personInformationAndLabour.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPersonalInformation(labourPerson, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(individualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    personInformationAndLabour.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = personInformationAndLabour.IndividualId,
                               Operation = JsonConvert.SerializeObject(personInformationAndLabour),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonPersonalInf,
                               Proccess = ENUMCOAP.TypeFunction.PersonPersonalInf.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                labourPerson = DelegateService.uniquePersonService.CreateCompanyLabourPerson(labourPerson, individualId);
                personInformationAndLabour = AplicationAssembler.CreateLabourPerson(labourPerson);
            }

            personInformationAndLabour.InfringementPolicies = infringementPolicies;
            return personInformationAndLabour;
        }

        /// <summary>
        /// Actualiza la informacion laboral de una persona
        /// </summary>
        /// <param name="personInformationAndLabor">Modelo que actualiza los datos e informacion laboral </param>
        /// <returns>Retorna la Informacion actualizada.</returns>
        public PersonInformationAndLabourDTO UpdateApplicationLabourPerson(PersonInformationAndLabourDTO personInformationAndLabour, bool validatePolicies = true)
        {
            CompanyLabourPerson labourPerson = ModelAssembler.CreateInformationLabourPerson(personInformationAndLabour);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(personInformationAndLabour.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(personInformationAndLabour.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(personInformationAndLabour.IndividualId);

                person.UserId = personInformationAndLabour.UserId;
                company.UserId = personInformationAndLabour.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPersonalInformation(labourPerson, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(personInformationAndLabour.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    personInformationAndLabour.OperationId = this.CreateAplicationPersonOperation(
                          new PersonOperationDTO
                          {
                              IndividualId = personInformationAndLabour.IndividualId,
                              Operation = JsonConvert.SerializeObject(personInformationAndLabour),
                              ProcessType = "Update",
                              FunctionId = (int)ENUMCOAP.TypeFunction.PersonPersonalInf,
                              Proccess = ENUMCOAP.TypeFunction.PersonPersonalInf.ToString()
                          }
                       ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                labourPerson = DelegateService.uniquePersonService.UpdateCompanyLabourPerson(labourPerson);
                personInformationAndLabour = AplicationAssembler.CreateLabourPerson(labourPerson);
            }

            personInformationAndLabour.InfringementPolicies = infringementPolicies;
            return personInformationAndLabour;
        }
        #endregion LabourPerson

        #region PaymentMethodAccount

        /// <summary>
        /// Obtiene los datos de un objeto IndividualPaymentMethodDTO 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<IndividualPaymentMethodDTO> GetIndividualpaymentMethodByIndividualId(int individualId)
        {
            return AplicationAssembler.CreateIndividualpaymentMethods(DelegateService.uniquePersonService.GetIndividualpaymentMethodByIndividualId(individualId));
        }

        /// <summary>
        /// Actualiza o crea los datos de un objeto IndividualPaymentMethodDTO 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="individualpaymentMethodDTO"></param>
        /// <returns></returns>
        public List<IndividualPaymentMethodDTO> CreateIndividualpaymentMethods(List<IndividualPaymentMethodDTO> individualpaymentMethodDTO, int individualId, bool validatePolicies = true)
        {
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            List<CompanyIndividualPaymentMethod> listPaymentMethodAccount = ModelAssembler.CreateIndividualpaymentMethods(individualpaymentMethodDTO);
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(individualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(individualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(individualId);

                person.UserId = individualpaymentMethodDTO.FirstOrDefault().UserId;
                company.UserId = individualpaymentMethodDTO.FirstOrDefault().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPaymentMethods(listPaymentMethodAccount.FirstOrDefault(), person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(individualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    //individualpaymentMethodDTO.First().Id = individualId;
                    individualpaymentMethodDTO.First().OperationId = this.CreateAplicationPersonOperation(
                          new PersonOperationDTO
                          {
                              IndividualId = individualId,
                              Operation = JsonConvert.SerializeObject(individualpaymentMethodDTO),
                              ProcessType = "Create",
                              FunctionId = (int)ENUMCOAP.TypeFunction.PersonPaymentMethods,
                              Proccess = ENUMCOAP.TypeFunction.PersonPaymentMethods.ToString()
                          }
                       ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                individualpaymentMethodDTO = AplicationAssembler.CreateIndividualpaymentMethods(DelegateService.uniquePersonService.CreateIndividualpaymentMethods(listPaymentMethodAccount, individualId));
            }

            individualpaymentMethodDTO.FirstOrDefault().InfringementPolicies = infringementPolicies;

            return individualpaymentMethodDTO;
        }

        #endregion

        #region Legal Representative

        /// <summary>
        /// crean los datos de un objeto LegalRepresentativeDTO 
        /// </summary>
        /// <param name="individualId"></param>
        /// <param name="legalRepresentative"></param>
        /// <returns></returns>
        public LegalRepresentativeDTO CreateLegalRepresentative(LegalRepresentativeDTO legalRepresentative)
        {
            CompanyLegalRepresentative companylegalRepresentative = ModelAssembler.CreateLegalRepresentative(legalRepresentative);
            companylegalRepresentative = DelegateService.uniquePersonService.CreateLegalRepresentative(companylegalRepresentative, legalRepresentative.individualId);
            return AplicationAssembler.CreateLegalRepresentativeDTO(companylegalRepresentative);
        }

        /// <summary>
        /// Obtiene los datos de un objeto LegalRepresentativeDTO 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public LegalRepresentativeDTO GetLegalRepresentativeByIndividualId(int individualId)
        {
            return AplicationAssembler.CreateLegalRepresentativeDTO(DelegateService.uniquePersonService.GetLegalRepresentativeByIndividualId(individualId));
        }

        #endregion

        #region IndividualRole

        /// <summary>
        /// Obtiene los datos de un objeto LegalRepresentativeDTO 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<IndividualRoleDTO> GetAplicationIndividualRoleByIndividualId(int individualId)
        {
            var individualRoleModel = DelegateService.uniquePersonService.GetCompanyIndividualRoleByIndividualId(individualId);
            return AplicationAssembler.CreateIndividualRoles(individualRoleModel);
        }

        #endregion

        #region Insured Guarantee

        public List<GuaranteeInsuredGuaranteeDTO> GetAplicationInsuredGuaranteesByIndividualId(int individualId)
        {
            return AplicationAssembler.CreateGuaranteeInsuredGuarantees(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteesByIndividualId(individualId));
        }


        public InsuredGuaranteeMortgageDTO GetAplicationInsuredGuaranteeMortgageByIndividualIdById(int individualId, int id)
        {
            return AplicationAssembler.CreateInsuredGuaranteeMortgage(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeMortgageByIndividualIdById(individualId, id));
        }

        public InsuredGuaranteeMortgageDTO CreateAplicationInsuredGuaranteeMortgage(InsuredGuaranteeMortgageDTO insuredGuaranteeMortgage)
        {
            CompanyInsuredGuaranteeMortgage companyInsuredGuaranteeMortgage = ModelAssembler.CreateInsuredGuaranteeMortgage(insuredGuaranteeMortgage);

            if (insuredGuaranteeMortgage.Id == 0)
            {
                companyInsuredGuaranteeMortgage.RegistrationDate = DateTime.Now;
                return AplicationAssembler.CreateInsuredGuaranteeMortgage(DelegateService.uniquePersonService.CreateCompanyCompanyInsuredGuaranteeMortgage(companyInsuredGuaranteeMortgage));
            }
            else
            {
                return AplicationAssembler.CreateInsuredGuaranteeMortgage(DelegateService.uniquePersonService.UpdateCompanyCompanyInsuredGuaranteeMortgage(companyInsuredGuaranteeMortgage));
            }
        }


        public InsuredGuaranteePledgeDTO GetAplicationInsuredGuaranteePledgeByIndividualIdById(int individualId, int id)
        {
            return AplicationAssembler.CreateInsuredGuaranteePledge(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteePledgeByIndividualIdById(individualId, id));
        }

        public InsuredGuaranteePledgeDTO CreateAplicationInsuredGuaranteePledge(InsuredGuaranteePledgeDTO insuredGuaranteePledge)
        {
            CompanyInsuredGuaranteePledge companyInsuredGuaranteePledge = ModelAssembler.CreateInsuredGuaranteePledge(insuredGuaranteePledge);

            if (insuredGuaranteePledge.Id == 0)
            {
                companyInsuredGuaranteePledge.RegistrationDate = DateTime.Now;
                return AplicationAssembler.CreateInsuredGuaranteePledge(DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteePledge(companyInsuredGuaranteePledge));
            }
            else
            {
                return AplicationAssembler.CreateInsuredGuaranteePledge(DelegateService.uniquePersonService.UpdateCompanyInsuredGuaranteePledge(companyInsuredGuaranteePledge));

            }
        }


        public InsuredGuaranteePromissoryNoteDTO GetAplicationInsuredGuaranteePromissoryNoteeByIndividualIdById(int individualId, int id)
        {
            return AplicationAssembler.CreateInsuredGuaranteePromissoryNote(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteePromissoryNoteByIndividualIdById(individualId, id));
        }

        public InsuredGuaranteePromissoryNoteDTO CreateAplicationInsuredGuaranteePromissoryNote(InsuredGuaranteePromissoryNoteDTO insuredGuaranteePromissoryNote)
        {

            CompanyInsuredGuaranteePromissoryNote companyInsuredGuaranteePromissoryNote = ModelAssembler.CreateInsuredGuaranteePromissoryNote(insuredGuaranteePromissoryNote);

            if (insuredGuaranteePromissoryNote.Id == 0)
            {
                companyInsuredGuaranteePromissoryNote.RegistrationDate = DateTime.Now;
                return AplicationAssembler.CreateInsuredGuaranteePromissoryNote(DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteePromissoryNote(companyInsuredGuaranteePromissoryNote));
            }
            else
            {
                return AplicationAssembler.CreateInsuredGuaranteePromissoryNote(DelegateService.uniquePersonService.UpdateCompanyInsuredGuaranteePromissoryNote(companyInsuredGuaranteePromissoryNote));
            }
        }


        public InsuredGuaranteeFixedTermDepositDTO GetAplicationInsuredGuaranteeFixedTermDepositByIndividualIdById(int individualId, int id)
        {
            return AplicationAssembler.CreateInsuredGuaranteeFixedTermDeposit(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeFixedTermDepositByIndividualIdById(individualId, id));
        }

        public InsuredGuaranteeFixedTermDepositDTO CreateAplicationInsuredGuaranteeFixedTermDeposit(InsuredGuaranteeFixedTermDepositDTO guaranteeFixedTermDeposit)
        {
            CompanyInsuredGuaranteeFixedTermDeposit companyGuaranteeFixedTermDeposit = ModelAssembler.CreateInsuredGuaranteeFixedTermDeposit(guaranteeFixedTermDeposit);

            if (guaranteeFixedTermDeposit.Id == 0)
            {
                companyGuaranteeFixedTermDeposit.RegistrationDate = DateTime.Now;
                return AplicationAssembler.CreateInsuredGuaranteeFixedTermDeposit(DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteeFixedTermDeposit(companyGuaranteeFixedTermDeposit));
            }
            else
            {
                return AplicationAssembler.CreateInsuredGuaranteeFixedTermDeposit(DelegateService.uniquePersonService.UpdateCompanyInsuredGuaranteeFixedTermDeposit(companyGuaranteeFixedTermDeposit));
            }
        }

        public InsuredGuaranteeOthersDTO GetAplicationInsuredGuaranteeOthersByIndividualIdById(int individualId, int id)
        {
            return AplicationAssembler.CreateInsuredGuaranteeOthers(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeOthersByIndividualIdById(individualId, id));
        }

        public InsuredGuaranteeOthersDTO CreateAplicationInsuredGuaranteeOthers(InsuredGuaranteeOthersDTO guaranteePromissoryOthers)
        {
            CompanyInsuredGuaranteeOthers companyInsuredGuaranteeOthers = ModelAssembler.CreateInsuredGuaranteeOthers(guaranteePromissoryOthers);

            if (guaranteePromissoryOthers.Id == 0)
            {
                companyInsuredGuaranteeOthers.RegistrationDate = DateTime.Now;
                return AplicationAssembler.CreateInsuredGuaranteeOthers(DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteeOthers(companyInsuredGuaranteeOthers));
            }
            else
            {
                return AplicationAssembler.CreateInsuredGuaranteeOthers(DelegateService.uniquePersonService.UpdateCompanyInsuredGuaranteeOthers(companyInsuredGuaranteeOthers));
            }
        }


        #endregion

        #region InsuredGuaranteelog

        public List<InsuredGuaranteeLogDTO> GetAplicationInsuredGuaranteeLogByIndividualIdById(int individualId, int id)
        {
            return AplicationAssembler.CreateInsuredGuaranteeLogs(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeLogsByindividualIdByguaranteeId(individualId, id));
        }

        public InsuredGuaranteeLogDTO CreateAplicationInsuredGuaranteeLog(InsuredGuaranteeLogDTO insuredGuaranteeLogDTO)
        {
            CompanyInsuredGuaranteeLog companyInsuredGuaranteeLog = ModelAssembler.CreateInsuredGuaranteeLog(insuredGuaranteeLogDTO);
            return AplicationAssembler.CreateInsuredGuaranteeLog(DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteeLog(companyInsuredGuaranteeLog));
        }

        #endregion

        #region MaritalStatus
        public List<MaritalStatusDTO> GetAplicationMaritalStatus()
        {
            var maritalStatus = DelegateService.uniquePersonService.GetCompanyMaritalStatus();
            return AplicationAssembler.CreateMaritalStatusDTOs(maritalStatus);
        }
        #endregion

        #region DocumentType
        public List<DocumentTypeDTO> GetAplicationDocumentTypes(int typeDocument)
        {
            var documentType = DelegateService.uniquePersonService.GetCompanyDocumentType(typeDocument);
            return AplicationAssembler.CreateDocumentTypes(documentType);
        }
        #endregion

        #region AddressesType
        public List<AddressTypeDTO> GetAplicationAddressesTypes()
        {
            var addressesType = DelegateService.uniquePersonService.GetCompanyAddressesTypes();
            return AplicationAssembler.CreateAddressTypes(addressesType);
        }
        #endregion

        #region PhoneType
        public List<PhoneTypeDTO> GetAplicationPhoneTypes()
        {
            var phoneTypes = DelegateService.uniquePersonService.GetCompanyPhoneTypes();
            return AplicationAssembler.CreatePhoneTypes(phoneTypes);
        }
        #endregion

        #region EmailType
        public List<EmailTypeDTO> GetAplicationEmailTypes()
        {
            var emailType = DelegateService.uniquePersonService.GetCompanyEmailTypes();
            return AplicationAssembler.CreateEmailTypes(emailType);
        }
        #endregion

        #region EconomicActivity
        public List<EconomicActivityDTO> GetAplicationEconomicActivities()
        {
            var emailType = DelegateService.uniquePersonService.GetCompanyEconomicActivities();
            return AplicationAssembler.CreateEconomicActivities(emailType);
        }
        #endregion

        #region AssociationType
        public List<AssociationTypeDTO> GetAplicationAssociationTypes()
        {
            var associationType = DelegateService.uniquePersonService.GetCompanyAssociationTypes();
            return AplicationAssembler.CreateAssociationTypes(associationType);
        }
        #endregion

        #region CompanyType
        public List<CompanyTypeDTO> GetAplicationCompanyTypes()
        {
            var companyTypes = DelegateService.uniquePersonService.GetCompanyCompanyType();
            return AplicationAssembler.CreateCompanyTypes(companyTypes);
        }
        #endregion

        #region Insured
        public List<InsuredDeclinedTypeDTO> GetAplicationInsuredDeclinedTypes()
        {
            var insuredDeclinedType = DelegateService.uniquePersonService.GetCompanyInsuredDeclinedTypes();
            return AplicationAssembler.CreateInsuredDeclinedTypes(insuredDeclinedType);
        }

        public List<InsuredSegmentDTO> GetAplicationInsuredSegment()
        {
            var insuredSegment = DelegateService.uniquePersonService.GetCompanyInsuredSegment();
            return AplicationAssembler.CreateInsuredSegments(insuredSegment);
        }

        public List<InsuredProfileDTO> GetAplicationInsuredProfile()
        {
            var insuredProfile = DelegateService.uniquePersonService.GetCompanyInsuredProfile();
            return AplicationAssembler.CreateInsuredProfiles(insuredProfile);
        }
        #endregion

        #region AgentType
        public List<AgentTypeDTO> GetAplicationAgentTypes()
        {
            var agentType = DelegateService.uniquePersonService.GetCompanyAgentTypes();
            return AplicationAssembler.CreateAgentTypes(agentType);
        }
        #endregion

        #region AgentDeclinedType
        public List<AgentDeclinedTypeDTO> GetAplicationAgentDeclinedTypes()
        {
            var agentDeclinedType = DelegateService.uniquePersonService.GetCompanyAgentDeclinedTypes();
            return AplicationAssembler.CreateAgentDeclinedTypes(agentDeclinedType);
        }
        #endregion

        #region GroupAgent
        public List<GroupAgentDTO> GetAplicationGroupAgent()
        {
            var groupAgent = DelegateService.uniquePersonService.GetCompanyGroupAgent();
            return AplicationAssembler.CreateGroupAgents(groupAgent);
        }
        #endregion

        #region SalesChannel
        public List<SalesChannelDTO> GetAplicationSalesChannel()
        {
            var salesChannel = DelegateService.uniquePersonService.GetCompanySalesChannel();
            return AplicationAssembler.CreateSalesChannels(salesChannel);
        }
        #endregion

        #region EmployeePerson
        public List<EmployeePersonDTO> GetAplicationEmployeePersons()
        {
            var employeePerson = DelegateService.uniquePersonService.GetCompanyEmployeePersons();
            return AplicationAssembler.CreateEmployeePersons(employeePerson);
        }
        #endregion

        #region AllOthersDeclinedType
        public List<AllOthersDeclinedTypeDTO> GetAplicationAllOthersDeclinedTypes()
        {
            var allOthersDeclinedType = DelegateService.uniquePersonService.GetCompanyAllOthersDeclinedTypes();
            return AplicationAssembler.CreateAllOthersDeclinedTypes(allOthersDeclinedType);
        }
        #endregion

        #region InsuredGuaranteeDocumentation

        public List<InsuredGuaranteeDocumentationDTO> CreateAplicationInsuredGuaranteeDocumentation(List<InsuredGuaranteeDocumentationDTO> insuredGuaranteeDocumentationDTO)
        {
            List<InsuredGuaranteeDocumentationDTO> result = new List<InsuredGuaranteeDocumentationDTO>();
            MOCOV1.CompanyInsuredGuaranteeDocumentation guaranteeDocument = new MOCOV1.CompanyInsuredGuaranteeDocumentation();

            foreach (InsuredGuaranteeDocumentationDTO item in insuredGuaranteeDocumentationDTO)
            {
                switch (item.ParametrizationStatus)
                {

                    case ParametrizationStatus.Create:

                        guaranteeDocument = DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteeDocumentation(ModelAssembler.CreateInsuredGuaranteeDocument(item));
                        result.Add(AplicationAssembler.CreateInsuredGuaranteeDocument(guaranteeDocument));

                        break;
                    case ParametrizationStatus.Update:

                        guaranteeDocument = DelegateService.uniquePersonService.UpdateCompanyInsuredGuaranteeDocumentation(ModelAssembler.CreateInsuredGuaranteeDocument(item));
                        result.Add(AplicationAssembler.CreateInsuredGuaranteeDocument(guaranteeDocument));

                        break;
                    case ParametrizationStatus.Delete:

                        DelegateService.uniquePersonService.DeleteCompanyInsuredGuaranteeDocumentation(item.IndividualId, item.GuaranteeId, item.GuaranteeCode, item.DocumentCode);

                        break;

                }

            }
            return result;
        }

        public List<InsuredGuaranteeDocumentationDTO> GetAplicationInsuredGuaranteeDocumentation()
        {
            List<InsuredGuaranteeDocumentationDTO> result = new List<InsuredGuaranteeDocumentationDTO>();

            List<CompanyInsuredGuaranteeDocumentation> insuredGuaranteeDocumentation = DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeDocumentation();

            foreach (CompanyInsuredGuaranteeDocumentation item in insuredGuaranteeDocumentation)
            {
                result.Add(AplicationAssembler.CreateInsuredGuaranteeDocument(item));
            }

            return result;
        }

        public InsuredGuaranteeDocumentationDTO GetAplicationInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(int individualId, int insuredguaranteeId, int guaranteeId, int documentId)
        {
            InsuredGuaranteeDocumentationDTO result = new InsuredGuaranteeDocumentationDTO();

            CompanyInsuredGuaranteeDocumentation companyInsuredGuaranteeDocumentation = DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeDocumentationByInsuredGuaranteeDocumentationId(individualId, insuredguaranteeId, guaranteeId, documentId);
            result = AplicationAssembler.CreateInsuredGuaranteeDocument(companyInsuredGuaranteeDocumentation);

            return result;
        }

        public List<InsuredGuaranteeDocumentationDTO> GetAplicationInsuredGuaranteeDocument(int individualId, int guaranteeId)
        {
            List<CompanyInsuredGuaranteeDocumentation> companyInsuredGuaranteeDocumentation = DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeDocument(individualId, guaranteeId);
            return AplicationAssembler.CreateInsuredGuaranteeDocuments(companyInsuredGuaranteeDocumentation);
        }


        #endregion

        #region GuaranteeRequiredDocument
        public List<GuaranteeRequiredDocumentDTO> GetAplicationInsuredGuaranteeRequiredDocumentation(int guaranteeId)
        {
            List<GuaranteeRequiredDocumentDTO> result = new List<GuaranteeRequiredDocumentDTO>();
            List<CompanyGuaranteeRequiredDocument> insuredGuaranteeDocumentation = DelegateService.uniquePersonService.GetCompanyInsuredGuaranteeRequiredDocumentation(guaranteeId);

            foreach (CompanyGuaranteeRequiredDocument item in insuredGuaranteeDocumentation)
            {
                result.Add(AplicationAssembler.CreateGuaranteeRequiredDocument(item));
            }

            return result;
        }
        #endregion

        #region Guaraantor
        public List<GuarantorDTO> GetAplicationGuarantorByindividualIdByguaranteeId(int individualId, int guaranteeId)
        {
            return AplicationAssembler.CreateGuarantors(DelegateService.uniquePersonService.GetCompanyGuarantorByindividualIdByguaranteeId(individualId, guaranteeId));
        }

        public List<GuarantorDTO> CreateAplicationGuarantor(List<GuarantorDTO> guarantor)
        {
            List<GuarantorDTO> GuarantorDTOs = new List<GuarantorDTO>();

            foreach (GuarantorDTO item in guarantor)
            {
                CompanyGuarantor companyGuarantor = (ModelAssembler.CreateGuarantor(item));
                GuarantorDTO guarantorDTO = new GuarantorDTO();

                switch (item.ParametrizationStatus)
                {

                    case ParametrizationStatus.Create:
                        guarantorDTO = AplicationAssembler.CreateGuarantor(DelegateService.uniquePersonService.CreateCompanyGuarantor(companyGuarantor));
                        break;

                    case ParametrizationStatus.Update:
                        guarantorDTO = AplicationAssembler.CreateGuarantor(DelegateService.uniquePersonService.UpdateCompanyGuarantor(companyGuarantor));
                        break;

                    case ParametrizationStatus.Delete:
                        DelegateService.uniquePersonService.DeleteCompanyGuarantor(companyGuarantor);
                        break;

                    case ParametrizationStatus.Original:
                        guarantorDTO = AplicationAssembler.CreateGuarantor(companyGuarantor);
                        break;

                    default:
                        break;
                }

                GuarantorDTOs.Add(guarantorDTO);

            }
            if (GuarantorDTOs.Count > 0)
            {
                return GuarantorDTOs;
            }
            else
            {
                return null;
            }

        }



        #region GuaranteePrefixAssocieted               
        /// <summary>
        /// Creates the application prefix associeted.
        /// </summary>
        /// <param name="PrefixAssocieteds">The prefix associeteds.</param>
        /// <returns></returns>
        public List<InsuredGuaranteePrefixDTO> CreateApplicationPrefixAssocieted(List<InsuredGuaranteePrefixDTO> PrefixAssocieteds)
        {
            List<InsuredGuaranteePrefixDTO> result = new List<InsuredGuaranteePrefixDTO>();

            foreach (InsuredGuaranteePrefixDTO prefixAssociated in PrefixAssocieteds)
            {
                CompanyInsuredGuaranteePrefix GuaranteePrefixAssocieted = new CompanyInsuredGuaranteePrefix();
                GuaranteePrefixAssocieted = ModelAssembler.CreateInsuredGuaranteePrefix(prefixAssociated);

                switch (prefixAssociated.Parameter)
                {

                    case ParametrizationStatus.Create:
                        GuaranteePrefixAssocieted = DelegateService.uniquePersonService.CreateCompanyInsuredGuaranteePrefix(GuaranteePrefixAssocieted);
                        result.Add(AplicationAssembler.CreateInsuredGuaranteePrefix(GuaranteePrefixAssocieted));

                        break;
                    case ParametrizationStatus.Update:

                        GuaranteePrefixAssocieted = DelegateService.uniquePersonService.UpdateCompanyInsuredGuaranteePrefix(ModelAssembler.CreateInsuredGuaranteePrefix(prefixAssociated));
                        result.Add(AplicationAssembler.CreateInsuredGuaranteePrefix(GuaranteePrefixAssocieted));

                        break;
                    case ParametrizationStatus.Delete:
                        DelegateService.uniquePersonService.DeleteCompanyInsuredGuaranteePrefix(prefixAssociated.IndividualId, prefixAssociated.GuaranteeId, prefixAssociated.PrefixCode);

                        break;
                }

            }


            return result;

        }

        public List<InsuredGuaranteePrefixDTO> GetAplicationInsuredGuaranteePrefixByindividualIdByguaranteeId(int individualId, int guaranteeId)
        {
            return AplicationAssembler.CreateInsuredGuaranteePrefixes(DelegateService.uniquePersonService.GetCompanyInsuredGuaranteePrefix(individualId, guaranteeId));
        }
        #endregion
        #endregion

        #region Prospect
        public ProspectLigthQuotationDTO GetProspectByPersonTypeDocumentTypeDocumentNumber(IndividualType individualType, int documentTypeId, string documentNumber)
        {
            try
            {
                if (individualType == IndividualType.Person)
                {
                    var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectNaturalByDocumentNumber(documentNumber);
                    return AplicationAssembler.CreateProspectLigthQuotation(ProspectPersonNatural);
                }
                else
                {
                    var ProspectPersonNatural = DelegateService.uniquePersonService.GetProspectLegalByDocumentNumber(documentNumber);
                    return AplicationAssembler.CreateProspectLigthQuotation(ProspectPersonNatural);
                }
            }
            catch (Exception ex)
            {
                throw (new BusinessException(Errors.ErrorGetProspectByPersonTypeDocumentTypeDocumentNumber, ex));
            }
        }
        #endregion

        #region Better Performance
        public LoadDataDTO LoadInitialData(bool isEmail)
        {
            try
            {
                LoadDataDTO result = new LoadDataDTO();

                result.GenderTypes = AplicationAssembler.CreateGenders();
                result.AddressTypes = GetAplicationAddressesTypes();
                result.AgentDeclinedTypes = GetAplicationAgentDeclinedTypes();
                result.Currencies = AplicationAssembler.CreateCurrencies(DelegateService.commonService.GetCurrencies());
                result.EmailTypes = GetAplicationEmailTypes();
                result.ExonerationTypes = AplicationAssembler.CreateExonerationTypes(DelegateService.uniquePersonService.GetExonerationTypes());
                result.MaritalStatus = GetAplicationMaritalStatus();
                result.PhoneTypes = GetAplicationPhoneTypes();

                GenericModelsServicesQueryModel model = DelegateService.uniquePersonParamServiceWeb.GetAddress(isEmail);
                if (model != null)
                {
                    result.AddressTypesbyEmail = AplicationAssembler.CreateAddressTypesbyEmail(model.GenericModelServicesQueryModel);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw (new BusinessException(Errors.ErrorLoadInitialData, ex));
            }
        }

        public LoadLegalDataDTO LoadInitialLegalData(int typeDocument)
        {
            try
            {
                LoadLegalDataDTO result = new LoadLegalDataDTO();

                result.AssociationTypes = GetAplicationAssociationTypes();
                result.CompanyTypes = GetAplicationCompanyTypes();
                result.DocumentTypes = GetAplicationDocumentTypes(typeDocument);

                if (result.AssociationTypes != null && result.AssociationTypes.Count > 0)
                {
                    result.AssociationTypes = result.AssociationTypes.OrderBy(x => x.Description).ToList();
                }

                if (result.DocumentTypes != null && result.DocumentTypes.Count > 0)
                {
                    result.DocumentTypes = result.DocumentTypes.OrderBy(x => x.Description).ToList();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw (new BusinessException(Errors.ErrorLoadInitialLegalData, ex));
            }
        }
        #endregion

        #region ThirdPerson

        public ThirdPartyDTO CreateAplicationThird(ThirdPartyDTO ThirdPartyDTO, bool validatePolicies = true)
        {
            MOCOV1.CompanyThird thirdParty = ModelAssembler.CreateThird(ThirdPartyDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(thirdParty.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(thirdParty.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(thirdParty.IndividualId);

                person.UserId = ThirdPartyDTO.UserId;
                company.UserId = ThirdPartyDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesThirdParty(thirdParty, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(ThirdPartyDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    ThirdPartyDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = ThirdPartyDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(ThirdPartyDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonThird,
                               Proccess = ENUMCOAP.TypeFunction.PersonIntermediary.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                thirdParty = DelegateService.uniquePersonService.CreateCompanyThird(thirdParty);
                ThirdPartyDTO = AplicationAssembler.CreateThird(thirdParty);
            }

            ThirdPartyDTO.InfringementPolicies = infringementPolicies;
            return ThirdPartyDTO;
        }

        public ThirdPartyDTO UpdateAplicationThird(ThirdPartyDTO thirdDTO, bool validatePolicies = true)
        {
            MOCOV1.CompanyThird third = ModelAssembler.CreateThird(thirdDTO);

            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(third.IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(third.IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(third.IndividualId);

                person.UserId = thirdDTO.UserId;
                company.UserId = thirdDTO.UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesThirdParty(third, person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(thirdDTO.IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    thirdDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = thirdDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(thirdDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonThird,
                               Proccess = ENUMCOAP.TypeFunction.PersonThird.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                third = DelegateService.uniquePersonService.CreateUpdateThird(third);
                thirdDTO = AplicationAssembler.CreateThird(third);
            }

            thirdDTO.InfringementPolicies = infringementPolicies;
            return thirdDTO;
        }

        public ThirdPartyDTO GetAplicationThirdByIndividualId(int id)
        {
            var thirdModel = DelegateService.uniquePersonService.GetCompanyThirdByIndividualId(id);
            var result = AplicationAssembler.CreateThird(thirdModel);
            return result;
        }
        #endregion


        /// <summary>
        /// Crear Empleado
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public EmployeeDTO CreateEmployee(EmployeeDTO employeeDTO, bool validatePolicies = true)
        {
            try
            {
                MOCOV1.CompanyEmployee companyEmployee = ModelAssembler.CreateEmployee(employeeDTO);
                List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
                if (validatePolicies) //VALIDACION DE POLITICAS
                {
                    CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(employeeDTO.IndividualId);
                    CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(employeeDTO.IndividualId);
                    List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(employeeDTO.IndividualId);

                    person.UserId = employeeDTO.UserId;
                    company.UserId = employeeDTO.UserId;

                    infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                    infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesEmployee(companyEmployee, person, company, addresses));

                    IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(employeeDTO.IndividualId);
                    if ((consortiumDTO?.Any()).GetValueOrDefault())
                    {
                        company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                        foreach (CompanyConsortium consorciated in company.Consortiums)
                        {
                            infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                        }
                    }
                }

                if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                    {
                        employeeDTO.OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = employeeDTO.IndividualId,
                               Operation = JsonConvert.SerializeObject(employeeDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonEmployed,
                               Proccess = ENUMCOAP.TypeFunction.PersonEmployed.ToString()
                           }
                        ).OperationId;
                    }
                }
                else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
                {
                    CompanyEmployee restul = DelegateService.uniquePersonService.CreateCompanyEmployee(ModelAssembler.CreateEmployee(employeeDTO));

                    employeeDTO = AplicationAssembler.CreateEmployee(restul);
                }
                employeeDTO.InfringementPolicies = infringementPolicies;

                return employeeDTO;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Traer Empleado
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        public EmployeeDTO GetEmployeeIndividualId(int individualId)
        {
            try
            {
                CompanyEmployee restul = DelegateService.uniquePersonService.GetCompanyEmployee(individualId);
                return AplicationAssembler.CreateEmployee(restul);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #region BusinessName
        /// <summary>
        ///  Crea razon social
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTO"></param>
        /// <returns></returns>
        public List<CompanyNameDTO> CreateBusinessName(List<CompanyNameDTO> listBusinessNameDTO, bool validatePolicies = true)
        {
            var result = new List<CompanyNameDTO>();
            List<MOCOUP.CompanyName> businessNameModel = ModelAssembler.CreateBusinessNames(listBusinessNameDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(listBusinessNameDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(listBusinessNameDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(listBusinessNameDTO.First().IndividualId);

                person.UserId = listBusinessNameDTO.First().UserId;
                company.UserId = listBusinessNameDTO.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesBusinessName(businessNameModel.First(), person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(listBusinessNameDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    listBusinessNameDTO.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = listBusinessNameDTO.First().IndividualId,
                               Operation = JsonConvert.SerializeObject(listBusinessNameDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonBusinessName,
                               Proccess = ENUMCOAP.TypeFunction.PersonBusinessName.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                UniquePersonAplicationServiceEEProvider uniquePersonAplicationServiceEEProvider = new UniquePersonAplicationServiceEEProvider();
                List<CompanyNameDTO> companyNameDTOs = uniquePersonAplicationServiceEEProvider.GetCompanyBusinessByIndividualId(listBusinessNameDTO.First().IndividualId);
                List<CompanyNameDTO> updateCompanyName = new List<CompanyNameDTO>();
                List<MOCOUP.CompanyName> createCompanyName = new List<MOCOUP.CompanyName>();

                if (companyNameDTOs.Count == 0)
                {
                    createCompanyName.AddRange(ModelAssembler.CreateBusinessNames(listBusinessNameDTO));
                }
                foreach (CompanyNameDTO item in listBusinessNameDTO)
                {
                    if (companyNameDTOs.Any(x => x.NameNum == item.NameNum))
                    {
                        updateCompanyName.Add(item);
                    }
                    else if (businessNameModel.Any(x => x.NameNum == item.NameNum))
                    {
                        createCompanyName.Add(ModelAssembler.CreateBusinessName(item));
                    }
                }

                //var updateCompanyName = listBusinessNameDTO.Where(m => m.NameNum > 0).ToList();
                //var createCompanyName = businessNameModel.Where(m => m.NameNum == 0).ToList();
                listBusinessNameDTO.Clear();

                foreach (MOCOUP.CompanyName item in createCompanyName)
                {
                    item.NameNum = CountAplicationBusinessName(item.IndividualId);
                    item.Enabled = true;
                    var companyBusinessName = DelegateService.uniquePersonService.CreateBusinessName(item);//CREA TABLA 1
                    var businessName = AplicationAssembler.CreateBusinessNameDTO(companyBusinessName);
                    listBusinessNameDTO.Add(businessName);
                }
                if (updateCompanyName.Count > 0)
                {
                    listBusinessNameDTO = UpdateAplicationBusinessName(updateCompanyName, false);
                }
                if (listBusinessNameDTO != null && listBusinessNameDTO.First().IndividualId != null)
                    DelegateService.uniquePersonService.CreateIntegrationNotification(listBusinessNameDTO.First().IndividualId, (int)UniquePersonService.Enums.Peripheraltype.BUSINSS_NAME);
            }

            listBusinessNameDTO.First().InfringementPolicies = infringementPolicies;
            return listBusinessNameDTO;
        }

        public List<CompanyNameDTO> UpdateAplicationBusinessName(List<CompanyNameDTO> listBusinessNameDTO, bool validatePolicies = true)
        {
            List<MOCOUP.CompanyName> businessNameModel = ModelAssembler.CreateBusinessNames(listBusinessNameDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(listBusinessNameDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(listBusinessNameDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(listBusinessNameDTO.First().IndividualId);

                person.UserId = listBusinessNameDTO.First().UserId;
                company.UserId = listBusinessNameDTO.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesBusinessName(businessNameModel.First(), person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(listBusinessNameDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    listBusinessNameDTO.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = listBusinessNameDTO.First().IndividualId,
                               Operation = JsonConvert.SerializeObject(listBusinessNameDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonBusinessName,
                               Proccess = ENUMCOAP.TypeFunction.PersonBusinessName.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                int i = 0;
                foreach (MOCOUP.CompanyName item in businessNameModel)
                {
                    item.Enabled = true;
                    var companyBusinessName = DelegateService.uniquePersonService.UpdateBusinessName(item);//CREA TABLA 1
                    var businessName = AplicationAssembler.CreateBusinessNameDTO(companyBusinessName);
                    listBusinessNameDTO.Add(businessName);
                    i++;
                }
            }
            listBusinessNameDTO.First().InfringementPolicies = infringementPolicies;

            return listBusinessNameDTO;
        }

        public int CountAplicationBusinessName(int individualId)
        {
            int countBusinessName = 0;
            return countBusinessName = DelegateService.uniquePersonService.CountBusinessNameByIndividualId(individualId);//CREA TABLA 1
        }

        /// <summary>
        /// Obtiene razon social individual por lista
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<CompanyNameDTO> GetCompanyBusinessByIndividualId(int individualId)
        {
            var ListIndividualCompanyBusinessModel = DelegateService.uniquePersonService.GetBusinessNameByIndividualId(individualId);

            var result = AplicationAssembler.CreateIndividualCompanyBusinessDTOs(ListIndividualCompanyBusinessModel);
            return result;

        }
        #endregion
        #region BankTransfers
        /// <summary>
        ///  Crea la cuenta para su transferencia
        /// </summary>
        /// <param name="listIndividualTaxExeptionDTO"></param>
        /// <returns></returns>
        public List<BankTransfersDTO> CreateBankTransfers(List<BankTransfersDTO> listBankTransfersDTO, bool validatePolicies = true)
        {
            List<MOCOUP.BankTransfers> bankTransfersModel = ModelAssembler.CreateBankTransfers(listBankTransfersDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(listBankTransfersDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(listBankTransfersDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(listBankTransfersDTO.First().IndividualId);

                person.UserId = listBankTransfersDTO.First().UserId;
                company.UserId = listBankTransfersDTO.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesBankTransfers(bankTransfersModel.First(), person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(listBankTransfersDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    listBankTransfersDTO.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = listBankTransfersDTO.First().IndividualId,
                               Operation = JsonConvert.SerializeObject(listBankTransfersDTO),
                               ProcessType = "Create",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonBankTransfers,
                               Proccess = ENUMCOAP.TypeFunction.PersonBusinessName.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {
                listBankTransfersDTO = AplicationAssembler.CreateBankTransfersDTO(DelegateService.uniquePersonService.CreateBankTransfers(bankTransfersModel));
                if (listBankTransfersDTO != null && listBankTransfersDTO.First().IndividualId != null)
                    DelegateService.uniquePersonService.CreateIntegrationNotification(listBankTransfersDTO.First().IndividualId, (int)UniquePersonService.Enums.Peripheraltype.BANK_TRANSF);
            }

            listBankTransfersDTO.First().InfringementPolicies = infringementPolicies;

            return listBankTransfersDTO;
        }

        public List<BankTransfersDTO> UpdateAplicationBankTransfers(List<BankTransfersDTO> listBankTransfersDTO, bool validatePolicies = true)
        {

            List<MOCOUP.BankTransfers> bankTransfersModel = ModelAssembler.CreateBankTransfers(listBankTransfersDTO);
            List<PoliciesAut> infringementPolicies = new List<PoliciesAut>();
            if (validatePolicies) //VALIDACION DE POLITICAS
            {
                CompanyPerson person = DelegateService.uniquePersonService.GetComPanyPersonByIndividualId(listBankTransfersDTO.First().IndividualId);
                CompanyCompany company = DelegateService.uniquePersonService.GetCompanyByIndividualId(listBankTransfersDTO.First().IndividualId);
                List<CompanyAddress> addresses = DelegateService.uniquePersonService.GetCompanyAddresses(listBankTransfersDTO.First().IndividualId);

                person.UserId = listBankTransfersDTO.First().UserId;
                company.UserId = listBankTransfersDTO.First().UserId;

                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesPerson(person, company, addresses));
                infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesBankTransfers(bankTransfersModel.First(), person, company, addresses));

                IEnumerable<ConsorciatedDTO> consortiumDTO = this.GetConsortiumByIndividualId(listBankTransfersDTO.First().IndividualId);
                if ((consortiumDTO?.Any()).GetValueOrDefault())
                {
                    company.Consortiums = consortiumDTO.Select(ModelAssembler.CreateConsortium).ToList();
                    foreach (CompanyConsortium consorciated in company.Consortiums)
                    {
                        infringementPolicies.AddRange(DelegateService.uniquePersonService.ValidateAuthorizationPoliciesConsortium(consorciated, null, company, addresses));
                    }
                }
            }

            if (infringementPolicies.Any(x => x.Type == ENUMCOAP.TypePolicies.Restrictive || x.Type == ENUMCOAP.TypePolicies.Authorization))
            {
                if (infringementPolicies.Where(x => x.Type != ENUMCOAP.TypePolicies.Notification).All(x => x.Type == ENUMCOAP.TypePolicies.Authorization))
                {
                    listBankTransfersDTO.First().OperationId = this.CreateAplicationPersonOperation(
                           new PersonOperationDTO
                           {
                               IndividualId = listBankTransfersDTO.First().IndividualId,
                               Operation = JsonConvert.SerializeObject(listBankTransfersDTO),
                               ProcessType = "Update",
                               FunctionId = (int)ENUMCOAP.TypeFunction.PersonBankTransfers,
                               Proccess = ENUMCOAP.TypeFunction.PersonBankTransfers.ToString()
                           }
                        ).OperationId;
                }
            }
            else if (infringementPolicies.All(x => x.Type == ENUMCOAP.TypePolicies.Notification))
            {

                bankTransfersModel = ModelAssembler.CreateBankTransfers(listBankTransfersDTO);
                int i = 0;
                foreach (MOCOUP.BankTransfers item in bankTransfersModel)
                {
                    var companybankTransfers = DelegateService.uniquePersonService.UpdateBankTransfers(item);//CREA TABLA 1
                    var bankTransfers = AplicationAssembler.CreateBankTransfersDTO(companybankTransfers);
                    listBankTransfersDTO.Add(bankTransfers);
                    i++;
                }
                if (listBankTransfersDTO != null && listBankTransfersDTO.First().IndividualId != null)
                    DelegateService.uniquePersonService.CreateIntegrationNotification(listBankTransfersDTO.First().IndividualId, (int)UniquePersonService.Enums.Peripheraltype.BANK_TRANSF);
            }

            listBankTransfersDTO.First().InfringementPolicies = infringementPolicies;

            return listBankTransfersDTO;
        }

        /// <summary>
        /// Obtiene cuentas asociadas por 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<BankTransfersDTO> GetCompanyBankTransfersByIndividualId(int individualId)
        {
            var ListIndividualCompanyBankModel = DelegateService.uniquePersonService.GetBankTransfersByIndividualId(individualId);

            var result = AplicationAssembler.CreateBankTransfersDTOs(ListIndividualCompanyBankModel);
            return result;

        }
        #endregion BankTransfers

        #region FiscalREsponsibility
        public List<FiscalResponsibilityDTO> GetAplicationCompanyFiscalResponsibility()
        {
            var fiscalResponsibility = DelegateService.uniquePersonService.GetCompanyFiscalResponsibility();
            return AplicationAssembler.CreateCompanyFiscalResponsibilities(fiscalResponsibility);
        }
        #endregion

        #region IndividualFiscalResponsibility

        /// <summary>
        ///  Crea lista de responsabilidades fiscales
        /// </summary>
        /// <param name="listInsuredFiscalResponsibilityDTO"></param>
        /// <returns></returns>
        public List<InsuredFiscalResponsibilityDTO> CreateIndividualFiscalResponsibility(List<InsuredFiscalResponsibilityDTO> listInsuredFiscalResponsibilityDTO)
        {
            List<MOCOUP.InsuredFiscalResponsibility> fiscalResponsibilityModel = ModelAssembler.CreateListInsuredFiscalResponsibility(listInsuredFiscalResponsibilityDTO);

            listInsuredFiscalResponsibilityDTO = AplicationAssembler.CreateListInsuredFiscalResponsibilityDTO(DelegateService.uniquePersonService.CreateInsuredFiscalResponsibility(fiscalResponsibilityModel));

            return listInsuredFiscalResponsibilityDTO;
        }

        public List<InsuredFiscalResponsibilityDTO> UpdateAplicationFiscalRespondibility(List<InsuredFiscalResponsibilityDTO> listFiscalDTO)
        {

            List<MOCOUP.InsuredFiscalResponsibility> fiscalModel = ModelAssembler.CreateListInsuredFiscalResponsibility(listFiscalDTO);
            int i = 0;
            foreach (MOCOUP.InsuredFiscalResponsibility fiscal in fiscalModel)
            {
                var companyFiscalResponsibility = DelegateService.uniquePersonService.UpdateInsuredFiscalResponsibility(fiscal);//CREA TABLA 1
                var fiscalResponsibility = AplicationAssembler.CreateInsuredFiscalResponsibilityDTO(companyFiscalResponsibility);
                listFiscalDTO.Add(fiscalResponsibility);
                i++;
            }

            return listFiscalDTO;
        }
        /// <summary>
        /// Obtiene responsabilidades fiscales por individualID 
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public List<InsuredFiscalResponsibilityDTO> GetCompanyFiscalResponsibilityByIndividualId(int individualId)
        {
            var ListIndividualCompanyFiscalModel = DelegateService.uniquePersonService.GetFiscalResponsibilityByIndividualId(individualId);
            var result = AplicationAssembler.CreateListInsuredFiscalResponsibilityDTO(ListIndividualCompanyFiscalModel);
            return result;

        }
        
        public bool DeleteFiscalResponsibility(InsuredFiscalResponsibilityDTO fiscalDTO)
        {
            CompanyInsuredFiscalResponsibility fiscalModel = ModelAssembler.CreateCompanyInsuredFiscalResponsibility(fiscalDTO);
            bool asigna = DelegateService.uniquePersonService.DeleteCompanyFiscalResponsibility(fiscalModel);
            return asigna;
        }
        #endregion IndividualFiscalResponsibility
        #region Politicas
        /// <summary>
        /// Obtener politicas generadas
        /// </summary>
        /// <returns></returns>
        public PersonOperationDTO GetPersonOperation(int operationId)
        {
            try
            {
                CompanyPersonOperation result = DelegateService.uniquePersonService.GetCompanyPersonOperation(operationId);
                return AplicationAssembler.CreatePersonOperation(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtener politicas generadas
        /// </summary>
        /// <returns></returns>
        public PersonOperationDTO CreateAplicationPersonOperation(PersonOperationDTO personOperation)
        {
            try
            {
                CompanyPersonOperation person = ModelAssembler.CreatePersonOperation(personOperation);
                CompanyPersonOperation result = DelegateService.uniquePersonService.CreateCompanyPersonOperation(person);
                return AplicationAssembler.CreatePersonOperation(result);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion Politicas
    }
}
