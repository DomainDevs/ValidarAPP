using AutoMapper;
using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Business
{
    public class PersonBusiness
    {
        private Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider coreProvider;

        public PersonBusiness()
        {
            coreProvider = new Core.Application.UniquePersonService.V1.UniquePersonServiceEEProvider();
        }


        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyPerson CreateCompanyPerson(CompanyPerson person)
        {
            try
            {

                var imapper = ModelAssembler.CreateMapperPerson();
                var personCore = imapper.Map<CompanyPerson, Person>(person);
                var resultCore = coreProvider.CreatePerson(personCore);
                var resultCompany = imapper.Map<Person, CompanyPerson>(resultCore);

                if (person.Exoneration != null)
                {
                    person.Exoneration.Id = resultCore.IndividualId;
                    person.Exoneration.EnteredDate = DateTime.Now;
                    var sarlaftExoneration = EntityAssembler.CreateSarlaftExoneration(person.Exoneration);
                    sarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.Insert(sarlaftExoneration);

                    resultCompany.Exoneration = sarlaftExoneration.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
                    {
                        Id = sarlaftExoneration.IndividualId,
                        ExonerationType = new CompanyExonerationType
                        {
                            Id = (int)sarlaftExoneration.ExonerationTypeCode,
                            IndividualTypeCode = (int)person.IndividualType
                        },
                    };
                }

                if (person.IndividualId > 0)
                {
                    CompanyInsured insuredModel = GetCompanyInsuredByIndividualId(person.IndividualId);
                    if (insuredModel != null)
                    {
                        insuredModel.ElectronicBiller = person.Insured.ElectronicBiller;
                        CompanyInsured result = UpdateCompanyInsured(insuredModel);
                    }
                }

                return resultCompany;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Crear una nueva Persona
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public CompanyPerson UpdateCompanyPerson(CompanyPerson person)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var personCore = imapper.Map<CompanyPerson, Person>(person);
                var resultCore = coreProvider.UpdatePerson(personCore);
                var resultCompany = imapper.Map<Person, CompanyPerson>(resultCore);

                if (person.Exoneration != null)
                {
                    person.Exoneration = new CompanySarlaftExoneration()
                    {
                        Id = resultCore.IndividualId,
                        EnteredDate = DateTime.Now
                    };
                    //person.Exoneration.Id = ;
                    //person.Exoneration.EnteredDate = DateTime.Now;
                    var primaryKeyExoneration = IndividualSarlaftExoneration.CreatePrimaryKey(resultCore.IndividualId);
                    var sarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.GetObject(primaryKeyExoneration);
                    sarlaftExoneration.IsExonerated = person.Exoneration.IsExonerated;
                    sarlaftExoneration.RegistrationDate = person.Exoneration.EnteredDate;
                    sarlaftExoneration.RoleCode = person.Exoneration.RolId;
                    sarlaftExoneration.UserId = person.Exoneration.UserId;
                    sarlaftExoneration.ExonerationTypeCode = sarlaftExoneration.ExonerationTypeCode;
                    DataFacadeManager.Update(sarlaftExoneration);
                    
                }
                if (person.IndividualId > 0)
                {
                    CompanyInsured insuredModel = GetCompanyInsuredByIndividualId(person.IndividualId);
                    if (insuredModel != null)
                    {
                        insuredModel.ElectronicBiller = person.Insured.ElectronicBiller;
                        CompanyInsured result = UpdateCompanyInsured(insuredModel);
                    }
                }



                return resultCompany;
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
                var imapper = ModelAssembler.CreateMapperPerson();
                var result = coreProvider.GetAddresses(individualId);
                return imapper.Map<List<Address>, List<CompanyAddress>>(result);
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
                var imapper = ModelAssembler.CreateMapperPerson();
                var coreAddresses = imapper.Map<List<CompanyAddress>, List<Address>>(addresses);
                var result = coreProvider.CreateAddresses(individualId, coreAddresses);
                return imapper.Map<List<Address>, List<CompanyAddress>>(result);
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
                var imapper = ModelAssembler.CreateMapperPerson();
                var coreAddresses = imapper.Map<List<CompanyAddress>, List<Address>>(addresses);
                var result = coreProvider.UpdateAddresses(individualId, coreAddresses);
                return imapper.Map<List<Address>, List<CompanyAddress>>(result);
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
                var imapper = ModelAssembler.CreateMapperPerson();
                var result = coreProvider.GetPhones(individualId);
                return imapper.Map<List<Phone>, List<CompanyPhone>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        public List<CompanyPhone> CreateCompanyPhones(int individualId, List<CompanyPhone> Phones)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var corePhones = imapper.Map<List<CompanyPhone>, List<Phone>>(Phones);
                var result = coreProvider.CreatePhones(individualId, corePhones);
                return imapper.Map<List<Phone>, List<CompanyPhone>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyPhone> UpdateCompanyPhones(int individualId, List<CompanyPhone> Phones)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var corePhones = imapper.Map<List<CompanyPhone>, List<Phone>>(Phones);
                var result = coreProvider.UpdatePhones(individualId, corePhones);
                return imapper.Map<List<Phone>, List<CompanyPhone>>(result);
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
                var imapper = ModelAssembler.CreateMapperPerson();
                var result = coreProvider.GetEmails(individualId);
                return imapper.Map<List<Email>, List<CompanyEmail>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEmail> CreateCompanyEmails(int individualId, List<CompanyEmail> Emails)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var coreEmails = imapper.Map<List<CompanyEmail>, List<Email>>(Emails);
                var result = coreProvider.CreateEmails(individualId, coreEmails);
                return imapper.Map<List<Email>, List<CompanyEmail>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompanyEmail> UpdateCompanyEmails(int individualId, List<CompanyEmail> Emails)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var coreEmails = imapper.Map<List<CompanyEmail>, List<Email>>(Emails);
                var result = coreProvider.UpdateEmails(individualId, coreEmails);
                return imapper.Map<List<Email>, List<CompanyEmail>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public CompanyPerson GetCompanyPersonByIndividualId(int id)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                PersonBusiness personBusiness = new PersonBusiness();
                var corePerson = coreProvider.GetPersonByIndividualId(id);
                return imapper.Map<Person, CompanyPerson>(corePerson);
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
                var imapper = ModelAssembler.CreateMapperPerson();
                PersonBusiness personBusiness = new PersonBusiness();
                var corePerson = coreProvider.GetPersonByDocument(customerType, documentNumber);
                var result = imapper.Map<List<Person>, List<CompanyPerson>>(corePerson);

                foreach (var person in result)
                {
                    var primaryKeyIndividualSarlaftExoneration = IndividualSarlaftExoneration.CreatePrimaryKey(person.IndividualId);
                    var individualSarlaftExoneration = (IndividualSarlaftExoneration)DataFacadeManager.GetObject(primaryKeyIndividualSarlaftExoneration);

                    person.Exoneration = individualSarlaftExoneration?.ExonerationTypeCode == null ? null : new CompanySarlaftExoneration
                    {
                        Id = individualSarlaftExoneration.IndividualId,
                        ExonerationType = new CompanyExonerationType
                        {
                            Id = (int)individualSarlaftExoneration.ExonerationTypeCode,
                            IndividualTypeCode = (int)person.IndividualType
                        },

                    };
                   
                }

                return result;
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
                var imapper = ModelAssembler.CreateMapperPerson();
                PersonBusiness personBusiness = new PersonBusiness();
                var corePerson = imapper.Map<CompanyPerson, Person>(person);
                var result = coreProvider.GetPersonAdv(customerType, corePerson);
                return imapper.Map<List<Person>, List<CompanyPerson>>(result);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public List<CompanyName> CompanyGetNotificationAddressesByIndividualId(int individualId, CustomerType customerType)
        {
            try
            {
                CompanyNameDAO companyNameDao = new CompanyNameDAO();
                return companyNameDao.CompanyGetNotificationAddressesByIndividualId(individualId, customerType);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public Models.CompanyInsured CreateCompanyInsured(Models.CompanyInsured companyInsured)
        {
            Insured coreInsured = ModelAssembler.CreateCoreInsured(companyInsured);
            coreInsured = coreProvider.CreateInsured(coreInsured);
            return ModelAssembler.CreateCompanyInsured(coreInsured);
        }

        public Models.CompanyInsured UpdateCompanyInsured(Models.CompanyInsured companyInsured)
        {
            Insured coreInsured = ModelAssembler.CreateCoreInsured(companyInsured);
            coreInsured = coreProvider.UpdateInsured(coreInsured);
            return ModelAssembler.CreateCompanyInsured(coreInsured);
        }

        public Models.CompanyInsured GetCompanyInsuredByIndividualId(int individualId)
        {
            Insured coreInsured = coreProvider.GetInsuredByIndividualId(individualId);
            if (coreInsured != null)
                return ModelAssembler.CreateCompanyInsured(coreInsured);
            else
                return null;
        }

        public Models.CompanyInsured GetCompanyInsuredByIndividualCode(int individualCode)
        {
            //Insured coreInsured = coreProvider.GetInsuredByIndividualCode(individualCode);
            //return ModelAssembler.CreateCompanyInsured(coreInsured);
            return null;
        }

        //public List<Models.CompanyInsured> GetCompanyInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        //{
        //    List<Insured> coreInsureds = coreProvider.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
        //    return coreInsureds.Select(ModelAssembler.CreateCompanyInsured).ToList();
        //}

        public Models.CompanyInsured GetCompanyInsuredByInsuredCode(int insuredCode)
        {
            Insured coreInsured = coreProvider.GetInsuredByInsuredCode(insuredCode);
            return ModelAssembler.CreateCompanyInsured(coreInsured);
        }

        public List<Models.CompanyInsuredMain> GetCompanyInsuredsByName(string filterString)
        {
            List<Models.CompanyInsuredMain> listResult = new List<Models.CompanyInsuredMain>();
            List<BaseInsuredMain> coreInsuredMainList = coreProvider.GetInsuredsByName(filterString);
            foreach (BaseInsuredMain item in coreInsuredMainList)
            {
                listResult.Add(ModelAssembler.CreateCompanyInsuredMain(item));
            }

            return listResult;
        }

        public CompanyPerson UpdateCompanyPersonBasicInfo(CompanyPerson person)
        {
            try
            {
                var imapper = ModelAssembler.CreateMapperPerson();
                var personCore = imapper.Map<CompanyPerson, Person>(person);
                var resultCore = coreProvider.UpdatePersonBasicInfo(personCore);
                var resultCompany = imapper.Map<Person, CompanyPerson>(resultCore);

                return resultCompany;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
