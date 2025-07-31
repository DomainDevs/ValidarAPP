using Sistran.Company.Application.UniquePersonServices.V1.DAOs;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;
using entityAssembler = Sistran.Core.Application.UniquePersonService.V1.Assemblers.EntityAssembler;
using enums = Sistran.Core.Application.UniquePersonService.V1.Enums;
using modelAssembler = Sistran.Core.Application.UniquePersonService.V1.Assemblers.ModelAssembler;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Resources;

namespace Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs
{
    /// <summary>
    /// Personas
    /// </summary>
    public class PersonDAO
    {
        /// <summary>
        /// Busca datos de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        public Models.CompanyPerson GetPersonByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Models.CompanyPerson personsModel = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entities.Person.Properties.IndividualId, typeof(entities.Person).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.Person), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                personsModel = ModelAssembler.CreatePersons(businessCollection).FirstOrDefault();



                MaritalStatusDAO maritalStatusDAO = new MaritalStatusDAO();
                //personsModel.MaritalStatus = maritalStatusDAO.GetMaritalStateById(personsModel.MaritalStatus.Id);



                PersonIndividualTypeDAO personIndividualTypeDAO = new PersonIndividualTypeDAO();
                models.PersonIndividualType personTypeModel = new models.PersonIndividualType();
                personTypeModel = personIndividualTypeDAO.GetPersonIndividualTypeIndividualId(personsModel.IndividualId);
                // personsModel.PersonType = personTypeModel;

                IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
                personsModel.Sarlafts = individualSarlaftDAO.GetIndividualSarlaftByIndividualId(personsModel.IndividualId);
                Models.CompanySarlaftExoneration exonerationModel = new Models.CompanySarlaftExoneration();
                IndividualSarlaftExonerationDAO individualSarlaftExonerationDAO = new IndividualSarlaftExonerationDAO();
                exonerationModel = individualSarlaftExonerationDAO.GetSarlaftExonerationByIndividualId(personsModel.IndividualId);
                personsModel.Exoneration = exonerationModel;
                PersonJobDAO personJobDAO = new PersonJobDAO();
                //personsModel.LaborPerson = personJobDAO.GetPersonJobByIndividualId(personsModel.IndividualId);
                IndividualDAO individualDAO = new IndividualDAO();
                models.Individual individualModel = new models.Individual();
                individualModel = individualDAO.GetIndividualByIndividualId(personsModel.IndividualId);

                EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                // personsModel.EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                personsModel.IndividualType = individualModel.IndividualType;
                personsModel.CustomerType = individualModel.CustomerType;
                //ProviderDAO providerDAO = new ProviderDAO();
                //personsModel.Provider = providerDAO.GetProviderByIndividualId(personsModel.IndividualId);
                TaxDAO taxDAO = new TaxDAO();
                //personsModel.IndividualTaxs = taxDAO.GetIndivualTaxsByIndividualId(personsModel.IndividualId);

                //PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
                //personsModel.PaymentMethod = paymentMethodDAO.GetPaymentMethodByIndividualId(individualId);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.GetPersonByIndividualId");

            return personsModel;
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
        public List<Models.CompanyPerson> GetPersonByDocumentNumberSurnameMotherLastName(string documentNumber, string surname, string motherLastName, string name, int searchType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.CompanyPerson> persons = new List<Models.CompanyPerson>();
            List<Models.CompanyPerson> personsAll = new List<Models.CompanyPerson>();
            List<models.ProspectNatural> prospectNaturals = new List<models.ProspectNatural>();
            ObjectCriteriaBuilder filter;
            List<models.DocumentType> documentTypes = new List<models.DocumentType>();
            BusinessCollection businessCollectionDoc = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.DocumentType)));
            documentTypes = modelAssembler.CreateDocumentTypes(businessCollectionDoc);

            if (searchType == (int)IndividualSearchType.Person || searchType == (int)IndividualSearchType.All)
            {

                filter = new ObjectCriteriaBuilder();
                BusinessCollection<entities.Person> businessCollection = new BusinessCollection<entities.Person>();
                if (documentNumber != "" && documentNumber != null)
                {
                    filter.Property(entities.Person.Properties.IdCardNo, typeof(entities.Person).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    businessCollection = DataFacadeManager.Instance.GetDataFacade().List<entities.Person>(filter.GetPredicate());
                }
                else
                {
                    bool useAdd = false;

                    if (!string.IsNullOrEmpty(surname))
                    {
                        filter.Property(entities.Person.Properties.Surname, typeof(entities.Person).Name);
                        filter.Like();
                        filter.Constant(surname + "%");
                        useAdd = true;
                    }
                    if (!string.IsNullOrEmpty(motherLastName))
                    {
                        if (useAdd)
                        {
                            filter.And();
                        }
                        filter.Property(entities.Person.Properties.MotherLastName, typeof(entities.Person).Name);
                        filter.Like();
                        filter.Constant(motherLastName + "%");
                        useAdd = true;
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (useAdd)
                        {
                            filter.And();
                        }
                        filter.Property(entities.Person.Properties.Name, typeof(entities.Person).Name);
                        filter.Like();
                        filter.Constant(name + "%");
                    }

                    BusinessCollection<entities.Person> businessCollectionAll = DataFacadeManager.Instance.GetDataFacade().List<entities.Person>(filter.GetPredicate());
                    int cont = 0;
                    foreach (var item in businessCollectionAll)
                    {
                        cont++;
                        if (cont <= 20)
                        {
                            businessCollection.Add(item);
                        }
                    }
                }

                persons = ModelAssembler.CreatePersons(businessCollection);
                businessCollection = null;

                if (persons.Count == 1)
                {
                    //AddressDAO addressDAO = new AddressDAO();
                    //persons[0].Addresses = addressDAO.GetAddresses(persons[0].IndividualId);

                    //PhoneDAO phoneDAO = new PhoneDAO();
                    //persons[0].Phones = phoneDAO.GetPhonesByIndividualId(persons[0].IndividualId);

                    //EmailDAO emailDAO = new EmailDAO();
                    //persons[0].Emails = emailDAO.GetEmailsByIndividualId(persons[0].IndividualId);

                    MaritalStatusDAO maritalStatusDAO = new MaritalStatusDAO();
                    //persons[0].MaritalStatus = maritalStatusDAO.GetMaritalStateById(persons[0].MaritalStatus.Id);

                    //PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
                    //persons[0].PaymentMthodAccount = paymentMethodAccountDAO.GetPaymentMethodAccountByIndividualId(persons[0].IndividualId);

                    //PersonJobDAO personJobDAO = new PersonJobDAO();
                    //persons[0].LaborPerson = personJobDAO.GetPersonJobByIndividualId(persons[0].IndividualId);

                    IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
                    persons[0].Sarlafts = individualSarlaftDAO.GetIndividualSarlaftByIndividualId(persons[0].IndividualId);

                    // PersonIndividualTypeDAO personIndividualTypeDAO = new PersonIndividualTypeDAO();
                    //persons[0].PersonType = personIndividualTypeDAO.GetPersonIndividualTypeIndividualId(persons[0].IndividualId);
                    Models.CompanySarlaftExoneration exonerationModel = new Models.CompanySarlaftExoneration();
                    IndividualSarlaftExonerationDAO individualSarlaftExonerationDAO = new IndividualSarlaftExonerationDAO();
                    exonerationModel = individualSarlaftExonerationDAO.GetSarlaftExonerationByIndividualId(persons[0].IndividualId);
                    persons[0].Exoneration = exonerationModel;
                    IndividualDAO individualDAO = new IndividualDAO();
                    models.Individual individualModel = new models.Individual();
                    individualModel = individualDAO.GetIndividualByIndividualId(persons[0].IndividualId);

                    EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                    //persons[0].EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                    persons[0].IndividualType = individualModel.IndividualType;
                    persons[0].CustomerType = CustomerType.Individual;
                    //ProviderDAO providerDAO = new ProviderDAO();
                    //persons[0].Provider = providerDAO.GetProviderByIndividualId(persons[0].IndividualId);
                    TaxDAO taxDAO = new TaxDAO();
                    //persons[0].IndividualTaxs = taxDAO.GetIndivualTaxsByIndividualId(persons[0].IndividualId);
                }
                personsAll.AddRange(persons);

            }
            if (searchType == (int)IndividualSearchType.ProspectusPerson || searchType == (int)IndividualSearchType.All)
            {
                filter = new ObjectCriteriaBuilder();
                BusinessCollection<entities.Prospect> businessCollection1;
                if (documentNumber != "" && documentNumber != null)
                {
                    filter.Property(entities.Prospect.Properties.IdCardNo, typeof(entities.Prospect).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<entities.Prospect>(filter.GetPredicate());
                }
                else
                {
                    bool useAdd = false;

                    if (!string.IsNullOrEmpty(surname))
                    {
                        filter.Property(entities.Prospect.Properties.Surname, typeof(entities.Prospect).Name);
                        filter.Like();
                        filter.Constant(surname + "%");
                        useAdd = true;
                    }
                    if (!string.IsNullOrEmpty(motherLastName))
                    {
                        if (useAdd)
                        {
                            filter.And();
                        }
                        filter.Property(entities.Prospect.Properties.MotherLastName, typeof(entities.Prospect).Name);
                        filter.Like();
                        filter.Constant(motherLastName + "%");
                        useAdd = true;
                    }
                    if (!string.IsNullOrEmpty(name))
                    {
                        if (useAdd)
                        {
                            filter.And();
                        }
                        filter.Property(entities.Prospect.Properties.Name, typeof(entities.Prospect).Name);
                        filter.Like();
                        filter.Constant(name + "%");
                    }
                    using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                    {
                        string[] sort = { };
                        int totalCount = 0;

                        businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<entities.Prospect>(filter.GetPredicate());

                        if (totalCount == 1)
                        {
                            businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<entities.Prospect>(filter.GetPredicate());
                        }

                    }

                }

                persons = ModelAssembler.CreatePersonProspects(businessCollection1);
                //if (persons.Count == 1)
                //{
                //    foreach (models.Address item in persons[0].Addresses)
                //    {
                //        if (item.City != null)
                //        {
                //            item.City = DelegateService.commonService.GetCityByCity(item.City);
                //        }
                //    }
                //}
                personsAll.AddRange(persons);
            }

            for (int i = 0; i < personsAll.Count(); i++)
            {
                personsAll[i].IdentificationDocument.DocumentType.SmallDescription = documentTypes.Where(b => b.Id == persons[i].IdentificationDocument.DocumentType.Id).FirstOrDefault().SmallDescription;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.GetPersonByDocumentNumberSurnameMotherLastName");

            return personsAll;

        }

        /// <summary>
        /// crear nueva persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.CompanyPerson CreatePerson(Models.CompanyPerson person, models.PersonIndividualType personIndividualType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PhoneDAO phoneDAO = new PhoneDAO();
            EmailDAO emailDAO = new EmailDAO();
            AddressDAO addressDAO = new AddressDAO();
            PersonJobDAO personJobDAO = new PersonJobDAO();
            IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
            IndividualSarlaftExonerationDAO individualSarlaftExonerationDAO = new IndividualSarlaftExonerationDAO();
            PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
            //PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            IDataFacadeManager dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
            IDataFacade df = dataFacadeManager.GetDataFacade();

            Transaction.Created += delegate (object sender, TransactionEventArgs e)
            {

            };

            using (Context.Current)
            {
                using (Transaction transaction = new Transaction())
                {
                    transaction.Completed += delegate (object sender, TransactionEventArgs e)
                    {

                    };
                    transaction.Disposed += delegate (object sender, TransactionEventArgs e)
                    {

                    };
                    try
                    {
                        //person.IndividualType = IndividualType.Person;
                        //entities.Person personEntity = entityAssembler.CreatePerson(person);

                        //DataFacadeManager.Instance.GetDataFacade().InsertObject(personEntity);

                        //if (personIndividualType != null)
                        //{
                        //    PersonIndividualTypeDAO personType = new PersonIndividualTypeDAO();
                        //    personType.CreatePersonIndividualType(personIndividualType, personEntity.IndividualId);
                        //}


                        //if (person.Addresses != null)
                        //{
                        //    foreach (models.Address Ad in person.Addresses)
                        //    {
                        //        if (person.Addresses.Count == 1)
                        //        {
                        //            Ad.IsPrincipal = true;
                        //        }
                        //        addressDAO.CreateAddress(Ad, personEntity.IndividualId);
                        //    }
                        //}
                        //if (person.Phones != null)
                        //{
                        //    foreach (models.Phone phon in person.Phones)
                        //    {
                        //        phoneDAO.CreatePhone(phon, personEntity.IndividualId);
                        //    }
                        //}
                        //if (person.Emails != null)
                        //{
                        //    foreach (models.Email em in person.Emails)
                        //    {
                        //        emailDAO.CreateEmail(em, personEntity.IndividualId);
                        //    }
                        //}
                        //if (person.PaymentMthodAccount != null)
                        //{
                        //    foreach (models.PaymentMethodAccount pmac in person.PaymentMthodAccount)
                        //    {
                        //        paymentMethodDAO.CreatePaymentMethod(pmac, personEntity.IndividualId);
                        //        if (pmac.PaymentMethod.Id != (int)PaymentMethodType.Cash)
                        //        {
                        //            paymentMethodAccountDAO.CreatePaymentMethodAccount(pmac, personEntity.IndividualId);
                        //        }

                        //    }
                        //}
                        //if (person.LaborPerson.Occupation.Id != 0 && personEntity.IndividualId != 0)
                        //{
                        //    personJobDAO.CreatePersonJob(person.LaborPerson, personEntity.IndividualId);
                        //}

                        if (person.Sarlafts != null)
                        {

                            foreach (Models.IndividualSarlaft isf in person.Sarlafts)
                            {
                                if (isf.FinancialSarlaft != null)
                                {
                                    //individualSarlaftDAO.CreateIndividualSarlaft(isf, personEntity.IndividualId, person.EconomicActivity.Id);
                                }
                            }

                        }
                        if (person.Exoneration != null)
                        {
                            //individualSarlaftExonerationDAO.CreateSarlaftExoneration(person.Exoneration, personEntity.IndividualId);
                        }

                        transaction.Complete();
                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.CreatePerson");

                        return GetPersonByIndividualId(1);
                    }
                    catch (DuplicatedObjectException)
                    {
                        transaction.Dispose();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        transaction.Dispose();
                        throw;
                    }

                }
            }

        }


        /// <summary>
        /// Buscar persona por identificación
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.CompanyPerson GetPersonByIdentification(models.IdentificationDocument identificationDocument)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entities.Person.Properties.IdCardTypeCode);
            filter.Equal();
            filter.Constant(identificationDocument.DocumentType.Id);
            filter.And();
            filter.Property(entities.Person.Properties.IdCardNo);
            filter.Equal();
            filter.Constant(identificationDocument.Number);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                                                                                                        .SelectObjects(typeof(entities.Person), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Count > 0)
            {
                entities.Person personEntity = (entities.Person)(businessCollection[0]);
                return ModelAssembler.CreatePerson(personEntity);
            }

            return null;
        }

        /// <summary>
        /// Actualizar persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.CompanyPerson UpdatePerson(Models.CompanyPerson person, models.PersonIndividualType personIndividualType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PhoneDAO phoneDAO = new PhoneDAO();
            EmailDAO emailDAO = new EmailDAO();
            IndividualSarlaftDAO individualSarlaftDAO = new IndividualSarlaftDAO();
            IndividualSarlaftExonerationDAO individualSarlaftExonerationDAO = new IndividualSarlaftExonerationDAO();
            AddressDAO addressDAO = new AddressDAO();
            PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
            PersonJobDAO personJobDAO = new PersonJobDAO();
            //PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
            PrimaryKey key = entities.Person.CreatePrimaryKey(person.IndividualId);
            entities.Person personEntity = (entities.Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (personEntity != null)
            {
                personEntity.EconomicActivityCode = person.EconomicActivity.Id;
                personEntity.Gender = person.Gender;
                personEntity.BirthPlace = person.BirthPlace != null ? person.BirthPlace : null;
                personEntity.Children = person.Children;
                personEntity.Name = person.FullName;
                personEntity.Surname = person.SurName;
                personEntity.MotherLastName = person.SecondSurName;
                personEntity.SpouseName = person.SpouseName;
                personEntity.MaritalStatusCode = person.MaritalStatus.Id;
                personEntity.BirthDate = person.BirthDate;
                //personEntity.PersonTypeCode = person.PersonStateType;
                personEntity.DataProtection = person.DataProtection;
                if (person.EducativeLevel != null)
                {
                    if (person.EducativeLevel.Id == 0)
                    {
                        personEntity.EducativeLevelCode = null;
                    }
                    else
                    {
                        personEntity.EducativeLevelCode = person.EducativeLevel.Id;
                    }
                }

                if (person.HouseType != null)
                {
                    if (person.HouseType.Id == 0)
                    {
                        personEntity.HouseTypeCode = null;
                    }
                    else
                    {
                        personEntity.HouseTypeCode = person.HouseType.Id;
                    }
                }

                if (person.SocialLayer != null)
                {

                    if (person.SocialLayer.Id == 0)
                    {
                        personEntity.SocialLayerCode = null;
                    }
                    else
                    {
                        personEntity.SocialLayerCode = person.SocialLayer.Id;
                    }
                }

                if (personEntity != null)
                {
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(personEntity);

                    if (personIndividualType != null)
                    {
                        PersonIndividualTypeDAO personType = new PersonIndividualTypeDAO();
                        personType.UpdatePersonIndividualType(personIndividualType, person.IndividualId);
                    }

                    //if (person.Addresses != null)
                    //{
                    //    foreach (models.Address Ad in person.Addresses)
                    //    {
                    //        if (Ad.Id == 0)
                    //        {
                    //            addressDAO.CreateAddress(Ad, person.IndividualId);
                    //        }
                    //        else
                    //        {
                    //            addressDAO.UpdateAddress(Ad, person.IndividualId);
                    //        }
                    //    }
                    //}

                    //if (person.Phones != null)
                    //{
                    //    foreach (models.Phone phon in person.Phones)
                    //    {
                    //        if (phon.Id == 0)
                    //        {
                    //            phoneDAO.CreatePhone(phon, person.IndividualId);
                    //        }
                    //        else
                    //        {
                    //            phoneDAO.UpdatePhone(phon, person.IndividualId);
                    //        }
                    //    }
                    //}

                    //if (person.Emails != null)
                    //{
                    //    foreach (models.Email em in person.Emails)
                    //    {
                    //        if (em.Id == 0)
                    //        {
                    //            emailDAO.CreateEmail(em, person.IndividualId);
                    //        }
                    //        else
                    //        {
                    //            emailDAO.UpdateEmail(em, person.IndividualId);
                    //        }
                    //    }
                    //}


                    if (person.Sarlafts != null)
                    {
                        foreach (Models.IndividualSarlaft sf in person.Sarlafts)
                        {
                            if (sf.FinancialSarlaft != null)
                            {
                                if (sf.FinancialSarlaft.SarlaftId != 0)
                                {
                                    individualSarlaftDAO.UpdateIndividualSarlaft(sf);
                                }
                                else
                                {
                                    individualSarlaftDAO.CreateIndividualSarlaft(sf, person.IndividualId, person.EconomicActivity.Id);
                                }

                            }

                        }
                    }

                    if (person.Exoneration != null)
                    {
                        individualSarlaftExonerationDAO.UpdateSarlaftExoneration(person.Exoneration, person.IndividualId);

                    }

                    //if (person.PaymentMthodAccount != null)
                    //{
                    //    foreach (models.PaymentMethodAccount pmac in person.PaymentMthodAccount)
                    //    {
                    //        paymentMethodDAO.UpdatePaymentMethod(pmac, personEntity.IndividualId);
                    //        if (pmac.PaymentMethod.Id != (int)PaymentMethodType.Cash)
                    //        {
                    //            paymentMethodAccountDAO.CreatePaymentMethodAccount(pmac, person.IndividualId);
                    //        }
                    //    }
                    //}

                    //if (person.LaborPerson.Occupation.Id != 0 && personEntity.IndividualId != 0)
                    //{
                    //    person.LaborPerson.IndividualId = personEntity.IndividualId;
                    //    personJobDAO.UpdatePersonJob(person.LaborPerson);
                    //}
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.Entities.DAOs.UpdatePerson");

            return GetPersonByIndividualId(personEntity.IndividualId);
        }

        /// <summary>
        /// Obtener persona por numero de documento
        /// </summary>
        /// <param name="individualCode">numero de documento</param>
        /// <returns></returns>
        public Models.CompanyPerson GetPersonByDocumentNumber(string documentNumber)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Models.CompanyPerson person = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(entities.Person.Properties.IdCardNo, typeof(entities.Person).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.Person), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                person = ModelAssembler.CreatePersons(businessCollection).FirstOrDefault();

                IndividualDAO individualDAO = new IndividualDAO();
                models.Individual individualModel = new models.Individual();
                individualModel = individualDAO.GetIndividualByIndividualId(person.IndividualId);
                person.CustomerType = individualModel.CustomerType;

                //PaymentMethodDAO paymentMethodDao = new PaymentMethodDAO();
                //person.PaymentMethod = paymentMethodDao.GetPaymentMethodByIndividualId(person.IndividualId);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonByDocumentNumber");
            return person;
        }

        /// <summary>
        /// Obtiene los motivos de baja
        /// </summary>
        /// <returns></returns>
        public List<OthersDeclinedTypes> GetAllOtrhesDeclinedTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(OthersDeclinedType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAllOthersDeclinedTypes");
            return ModelAssembler.GetAllOthersDeclinedTypes(businessCollection);
        }

        public IndividualControl CreateIndividualControl(IndividualControl individualControl)
        {
            try
            {
                return ModelAssembler.CreateIndividualControl((INTEN.UpIndividualControl)DataFacadeManager.Insert(EntityAssembler.CreateIndividualControl(individualControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        public EmployeeControl CreateEmployeeControl(EmployeeControl employeeControl)
        {
            try
            {
                return ModelAssembler.CreateEmployeeControl((INTEN.UpEmployeeControl)DataFacadeManager.Insert(EntityAssembler.CreateEmployeeControl(employeeControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        public InsuredControl CreateInsuredControl(InsuredControl insuredControl)
        {
            try
            {
                return ModelAssembler.CreateInsuredControl((INTEN.UpInsuredControl)DataFacadeManager.Insert(EntityAssembler.CreateInsuredControl(insuredControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        public InsuranceCompanyControl CreateCoInsuranceCompanyControl(InsuranceCompanyControl coInsuranceCompanyControl)
        {
            try
            {
                return ModelAssembler.CreateCoInsuranceCompanyControl((INTEN.UpInsuranceCompanyControl)DataFacadeManager.Insert(EntityAssembler.CreateCoInsuranceCompanyControl(coInsuranceCompanyControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }


        public AgentControl CreateAgentControl(AgentControl agentControl)
        {
            try
            {
                return ModelAssembler.CreateAgentControl((INTEN.UpAgentControl)DataFacadeManager.Insert(EntityAssembler.CreateAgentControl(agentControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        public CompanyReinsuranceControl CreateCompanyReinsuranceControl(CompanyReinsuranceControl companyReinsuranceControl)
        {
            try
            {
                return ModelAssembler.CreateCompanyReinsuranceControl((INTEN.UpReinsuranceCompanyControl)DataFacadeManager.Insert(EntityAssembler.CreateCompanyReinsuranceControl(companyReinsuranceControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        public SupplierControl CreateSupplierControl(SupplierControl supplierControl)
        {
            try
            {
                return ModelAssembler.CreateSupplierControl((INTEN.UpSupplierControl)DataFacadeManager.Insert(EntityAssembler.CreateSupplierControl(supplierControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        public CoIndividualControl CreateCoIndividualControl(CoIndividualControl CoindividualControl)
        {
            try
            {
                return ModelAssembler.CreateCoIndividualControl((INTEN.UpCoIndividualControl)DataFacadeManager.Insert(EntityAssembler.CreateCoIndividualControl(CoindividualControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }

        internal CompanyOrPerson GetIdentificationPersonOrCompanyByIndividualId(int individualId)
        {
            List<CompanyOrPerson> companyOrPersons = new List<CompanyOrPerson>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(entities.Company.Properties.IndividualId, typeof(entities.Company).Name, individualId);
            BusinessCollection businessObjectsCompany = DataFacadeManager.GetObjects(typeof(entities.Company), filter.GetPredicate());
            if (businessObjectsCompany.Count > 0)
            {
                companyOrPersons = ModelAssembler.CreateCompanies1(businessObjectsCompany);
            }
            else
            {
                filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(entities.Person.Properties.IndividualId, typeof(entities.Person).Name, individualId);
                BusinessCollection businessObjectsPerson = DataFacadeManager.GetObjects(typeof(entities.Person), filter.GetPredicate());
                companyOrPersons = ModelAssembler.CreatePersons1(businessObjectsPerson);
            }

            return companyOrPersons.Last();
        }
    }
}
