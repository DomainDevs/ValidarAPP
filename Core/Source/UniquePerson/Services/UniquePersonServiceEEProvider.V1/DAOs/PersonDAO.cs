using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
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
using INTEN = Sistran.Core.Application.Integration.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Resources;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
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
        public Models.Person GetPersonByIndividualId(int individualId)
        {

            Models.Person personsModel = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Person.Properties.IndividualId, typeof(Person).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Person), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                personsModel = ModelAssembler.CreatePersons(businessCollection).FirstOrDefault();
                //todo ricardo
                //AddressDAO addressDAO = new AddressDAO();
                //personsModel.Addresses = addressDAO.GetAddresses(personsModel.IndividualId);

                //EmailDAO emailDAO = new EmailDAO();
                //personsModel.Emails = emailDAO.GetEmailsByIndividualId(personsModel.IndividualId);

                //PhoneDAO phoneDAO = new PhoneDAO();
                //personsModel.Phones = phoneDAO.GetPhonesByIndividualId(personsModel.IndividualId);

                MaritalStatusDAO maritalStatusDAO = new MaritalStatusDAO();
                personsModel.MaritalStatus = maritalStatusDAO.GetMaritalStateById(personsModel.MaritalStatus.Id);

                //todo ricardo
                //PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
                //personsModel.PaymentMthodAccount = paymentMethodAccountDAO.GetPaymentMethodAccountByIndividualId(personsModel.IndividualId);

                //PersonIndividualTypeDAO personIndividualTypeDAO = new PersonIndividualTypeDAO();
                //Models.PersonIndividualType personTypeModel = new Models.PersonIndividualType();
                //personTypeModel = personIndividualTypeDAO.GetPersonIndividualTypeIndividualId(personsModel.IndividualId);
                //personsModel.PersonType = personTypeModel;

                //todo ricardo
                //PersonJobDAO personJobDAO = new PersonJobDAO();
                //personsModel = personJobDAO.GetPersonJobByIndividualId(personsModel.IndividualId);
                //IndividualDAO individualDAO = new IndividualDAO();
                //Models.Individual individualModel = new Models.Individual();
                //individualModel = individualDAO.GetIndividualByIndividualId(personsModel.IndividualId);

                //EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                //personsModel.EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                //personsModel.IndividualType = individualModel.IndividualType;
                //personsModel.CustomerType = individualModel.CustomerType;
            }
            return personsModel;
        }

        ///// <summary>
        ///// Obtener persona por numero de documento
        ///// </summary>
        ///// <param name="individualCode">numero de documento</param>
        ///// <returns></returns>
        //public virtual Models.Person GetPersonByDocumentNumber(string documentNumber)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();

        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(UniquePersonV1.Entities.Person.Properties.IdCardNo, typeof(UniquePersonV1.Entities.Person).Name);
        //    filter.Equal();
        //    filter.Constant(documentNumber);
        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Person), filter.GetPredicate()));
        //    Models.Person person = ModelAssembler.CreatePersons(businessCollection).FirstOrDefault();

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonByDocumentNumber");
        //    return person;
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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<Models.Person> persons = new List<Models.Person>();
            List<Models.Person> personsAll = new List<Models.Person>();
            List<Models.ProspectNatural> prospectNaturals = new List<Models.ProspectNatural>();
            ObjectCriteriaBuilder filter;
            List<Models.DocumentType> documentTypes = new List<Models.DocumentType>();
            BusinessCollection businessCollectionDoc = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(DocumentType)));
            documentTypes = ModelAssembler.CreateDocumentTypes(businessCollectionDoc);

            if (searchType == (int)IndividualSearchType.Person || searchType == (int)IndividualSearchType.All)
            {

                filter = new ObjectCriteriaBuilder();
                BusinessCollection<UniquePersonV1.Entities.Person> businessCollection;
                if (documentNumber != "" && documentNumber != null)
                {
                    filter.Property(Person.Properties.IdCardNo, typeof(Person).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePersonV1.Entities.Person>(filter.GetPredicate());
                }
                else
                {
                    bool useAdd = false;

                    if (!string.IsNullOrEmpty(surname))
                    {
                        filter.Property(UniquePersonV1.Entities.Person.Properties.Surname, typeof(UniquePersonV1.Entities.Person).Name);
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
                        filter.Property(UniquePersonV1.Entities.Person.Properties.MotherLastName, typeof(UniquePersonV1.Entities.Person).Name);
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
                        filter.Property(UniquePersonV1.Entities.Person.Properties.Name, typeof(UniquePersonV1.Entities.Person).Name);
                        filter.Like();
                        filter.Constant(name + "%");
                    }

                    using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                    {
                        string[] sort = { };
                        int totalCount;

                        businessCollection = dataFacade.List<UniquePersonV1.Entities.Person>(filter.GetPredicate(), sort, 200, 0, out totalCount);

                        if (totalCount == 1)
                        {
                            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePersonV1.Entities.Person>(filter.GetPredicate());
                        }

                    }
                }

                persons = ModelAssembler.CreatePersons(businessCollection);
                businessCollection = null;

                if (persons.Count == 1)
                {
                    //todo ricardo
                    //AddressDAO addressDAO = new AddressDAO();
                    //persons[0].Addresses = addressDAO.GetAddresses(persons[0].IndividualId);

                    //PhoneDAO phoneDAO = new PhoneDAO();
                    //persons[0].Phones = phoneDAO.GetPhonesByIndividualId(persons[0].IndividualId);

                    //EmailDAO emailDAO = new EmailDAO();
                    //persons[0].Emails = emailDAO.GetEmailsByIndividualId(persons[0].IndividualId);

                    MaritalStatusDAO maritalStatusDAO = new MaritalStatusDAO();
                    persons[0].MaritalStatus = maritalStatusDAO.GetMaritalStateById(persons[0].MaritalStatus.Id);
                    //todo ricardo
                    //PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
                    //persons[0].PaymentMthodAccount = paymentMethodAccountDAO.GetPaymentMethodAccountByIndividualId(persons[0].IndividualId);

                    //PersonJobDAO personJobDAO = new PersonJobDAO();
                    //persons[0].LaborPerson = personJobDAO.GetPersonJobByIndividualId(persons[0].IndividualId);

                    //PersonIndividualTypeDAO personIndividualTypeDAO = new PersonIndividualTypeDAO();
                    //persons[0].PersonType = personIndividualTypeDAO.GetPersonIndividualTypeIndividualId(persons[0].IndividualId);

                    IndividualDAO individualDAO = new IndividualDAO();
                    Models.Individual individualModel = new Models.Individual();
                    individualModel = individualDAO.GetIndividualByIndividualId(persons[0].IndividualId);

                    EconomicActivityDAO economicActivityDAO = new EconomicActivityDAO();
                    persons[0].EconomicActivity = economicActivityDAO.GetEconomicActivitiesByEconomicActiviti(individualModel.EconomicActivity.Id);
                    persons[0].IndividualType = individualModel.IndividualType;
                    persons[0].CustomerType = individualModel.CustomerType;

                }
                personsAll.AddRange(persons);

            }
            if (searchType == (int)IndividualSearchType.ProspectusPerson || searchType == (int)IndividualSearchType.All)
            {
                filter = new ObjectCriteriaBuilder();
                BusinessCollection<Prospect> businessCollection1;
                if (documentNumber != "" && documentNumber != null)
                {
                    filter.Property(Prospect.Properties.IdCardNo, typeof(Prospect).Name);
                    filter.Equal();
                    filter.Constant(documentNumber);
                    businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<Prospect>(filter.GetPredicate());
                }
                else
                {
                    bool useAdd = false;

                    if (!string.IsNullOrEmpty(surname))
                    {
                        filter.Property(Prospect.Properties.Surname, typeof(Prospect).Name);
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
                        filter.Property(Prospect.Properties.MotherLastName, typeof(Prospect).Name);
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
                        filter.Property(Prospect.Properties.Name, typeof(Prospect).Name);
                        filter.Like();
                        filter.Constant(name + "%");
                    }
                    using (IDataFacade dataFacade = DataFacadeManager.Instance.GetDataFacade())
                    {
                        string[] sort = { };
                        int totalCount = 0;

                        businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<Prospect>(filter.GetPredicate());

                        if (totalCount == 1)
                        {
                            businessCollection1 = DataFacadeManager.Instance.GetDataFacade().List<Prospect>(filter.GetPredicate());
                        }

                    }

                }

                persons = ModelAssembler.CreatePersonProspects(businessCollection1);
                if (persons.Count == 1)
                {
                    //todo ricardo
                    //foreach (Models.Address item in persons[0].Addresses)
                    //{
                    //    if (item.City != null)
                    //    {
                    //        item.City = DelegateService.commonServiceCore.GetCityByCity(item.City);
                    //    }
                    //}
                }
                personsAll.AddRange(persons);
            }

            for (int i = 0; i < personsAll.Count(); i++)
            {
                personsAll[i].IdentificationDocument.DocumentType.SmallDescription = documentTypes.Where(b => b.Id == persons[i].IdentificationDocument.DocumentType.Id).FirstOrDefault().SmallDescription;

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonByDocumentNumberSurnameMotherLastName");
            return personsAll;
        }


        /// <summary>
        /// edita la informacion personal
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public Models.Person UpdatePersonalInformation(Models.Person person)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                PrimaryKey key = Person.CreatePrimaryKey(person.IndividualId);
                Person personEntity = (Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                if (personEntity != null)
                {
                    personEntity.Children = person.Children;
                    personEntity.EducativeLevelCode = person.EducativeLevel.Id != 0 ? (int?)person.EducativeLevel.Id : null;
                    personEntity.HouseTypeCode = person.HouseType.Id != 0 ? (int?)person.HouseType.Id : null;
                    personEntity.SocialLayerCode = person.SocialLayer.Id != 0 ? (int?)person.SocialLayer.Id : null;
                    personEntity.SpouseName = person.SpouseName;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(personEntity);
                }

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePersonalInformation");
                return person;
            }
            catch (Exception)
            {

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePersonalInformation");
                throw;
            }
        }


        /// <summary>
        /// crear nueva persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public virtual Models.Person CreatePerson(Models.Person person)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
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
                        person.IndividualType = IndividualType.Person;
                        UniquePersonV1.Entities.Person personEntity = EntityAssembler.CreatePerson(person);

                        if (person.EducativeLevel.Id == 0)
                        {
                            personEntity.EducativeLevelCode = null;
                        }
                        if (person.HouseType.Id == 0)
                        {
                            personEntity.HouseTypeCode = null;
                        }
                        if (person.SocialLayer.Id == 0)
                        {
                            personEntity.SocialLayerCode = null;
                        }

                        DataFacadeManager.Instance.GetDataFacade().InsertObject(personEntity);

                        if (person.PersonType != null)
                        {
                            PersonIndividualTypeDAO personType = new PersonIndividualTypeDAO();
                            //personType.CreatePersonIndividualType(person.PersonType, personEntity.IndividualId);
                        }
                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePerson");
                        transaction.Complete();
                        return GetPersonByIndividualId(personEntity.IndividualId);
                    }
                    catch (DuplicatedObjectException)
                    {
                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePerson");
                        transaction.Dispose();
                        throw;
                    }
                    catch (Exception ex)
                    {
                        stopWatch.Stop();
                        Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePerson");
                        transaction.Dispose();
                        throw new BusinessException("Error in CreatePerson", ex);
                    }

                }
            }

        }

        /// <summary>
        /// Actualizar persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public virtual Models.Person UpdatePerson(Models.Person person, Models.PersonIndividualType personIndividualType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PhoneDAO phoneDAO = new PhoneDAO();
            EmailDAO emailDAO = new EmailDAO();
            AddressDAO addressDAO = new AddressDAO();
            //PaymentMethodAccountDAO paymentMethodAccountDAO = new PaymentMethodAccountDAO();
            PersonJobDAO personJobDAO = new PersonJobDAO();
            PrimaryKey key = UniquePersonV1.Entities.Person.CreatePrimaryKey(person.IndividualId);
            UniquePersonV1.Entities.Person personEntity = (UniquePersonV1.Entities.Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (personEntity != null)
            {
                personEntity.EconomicActivityCode = person.EconomicActivity != null ? (int?)person.EconomicActivity.Id : null;
                personEntity.Gender = person.Gender;
                personEntity.BirthPlace = person.BirthPlace != null ? person.BirthPlace : null;
                personEntity.Children = person.Children;
                personEntity.Name = person.FullName;
                personEntity.Surname = person.SurName;
                personEntity.MotherLastName = person.SecondSurName;
                personEntity.SpouseName = person.SpouseName;
                personEntity.BirthDate = person.BirthDate != null ? person.BirthDate : DateTime.MinValue;

                if (person.MaritalStatus != null)
                    personEntity.MaritalStatusCode = person.MaritalStatus.Id;

                if (person.EducativeLevel.Id == 0)
                {
                    personEntity.EducativeLevelCode = null;
                }
                else
                {
                    personEntity.EducativeLevelCode = person.EducativeLevel.Id;
                }
                if (person.HouseType.Id == 0)
                {
                    personEntity.HouseTypeCode = null;
                }
                else
                {
                    personEntity.HouseTypeCode = person.HouseType.Id;
                }
                if (person.SocialLayer.Id == 0)
                {
                    personEntity.SocialLayerCode = null;
                }
                else
                {
                    personEntity.SocialLayerCode = person.SocialLayer.Id;
                }


                if (personEntity != null)
                {

                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.UpdateObject(personEntity);
                    }

                    if (personIndividualType != null)
                    {
                        PersonIndividualTypeDAO personType = new PersonIndividualTypeDAO();
                        personType.UpdatePersonIndividualType(personIndividualType, person.IndividualId);
                    }
                    //todo ricardo
                    //if (person.Addresses != null)
                    //{
                    //    foreach (Models.Address Ad in person.Addresses)
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
                    //    foreach (Models.Phone phon in person.Phones)
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
                    //    foreach (Models.Email em in person.Emails)
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

                    //if (person.PaymentMthodAccount != null)
                    //{
                    //    foreach (Models.PaymentMethodAccount pmac in person.PaymentMthodAccount)
                    //    {
                    //        if (pmac.PaymentMethod.Id != (int)PaymentMethodType.Cash)
                    //        {
                    //            if (pmac.Id == 0)
                    //            {
                    //                paymentMethodAccountDAO.CreatePaymentMethodAccount(pmac, person.IndividualId);
                    //            }
                    //            else
                    //            {
                    //                paymentMethodAccountDAO.UpdatePaymentMethodAccount(pmac, personEntity.IndividualId);
                    //            }
                    //        }

                    //    }
                    //}

                    //if (person.LaborPerson != null && person.LaborPerson.Occupation.Id != 0 && personEntity.IndividualId != 0)
                    //{
                    //    person.LaborPerson.IndividualId = personEntity.IndividualId;
                    //    personJobDAO.UpdatePersonJob(person.LaborPerson);
                    //}
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePerson");
            return GetPersonByIndividualId(personEntity.IndividualId);
        }

        /// <summary>
        /// Busca el Person segun el IndividualId
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public static UniquePersonV1.Entities.Person Find(int individualId)
        {
            PrimaryKey key = UniquePersonV1.Entities.Person.CreatePrimaryKey(individualId);
            UniquePersonV1.Entities.Person person = (UniquePersonV1.Entities.Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return person;
        }


        public Models.Person GetPersonByDocumentByDocumentType(string document, int documentType)
        {
            Models.Person person = new Models.Person();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Person.Properties.IdCardNo, typeof(Person).Name, document);
            filter.And();
            filter.PropertyEquals(Person.Properties.IdCardTypeCode, typeof(Person).Name, documentType);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(Person), filter.GetPredicate());
            List<Models.Person> persons = ModelAssembler.CreatePersons(businessObjects);
            if (persons.Count > 0)
            {
                person = persons.Last();
            }
            return person;
        }

        public Models.Company GetCompanyByDocumentByDocumentType(string document, int documentType)
        {
            Models.Company company = new Models.Company();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Company.Properties.TributaryIdNo, typeof(Company).Name, document);
            filter.And();
            filter.PropertyEquals(Company.Properties.TributaryIdTypeCode, typeof(Company).Name, documentType);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(Company), filter.GetPredicate());
            List<Models.Company> companies = ModelAssembler.CreateCompanies(businessObjects);
            if (companies.Count > 0)
            {
                company = companies.Last();
            }
            return company;
        }

        /// <summary>
        /// Obtener el Asegurado por individuo
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public int GetInsuredByIndividualId(int individualId)
        {
            int insuredCode = 0;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Insured.Properties.IndividualId, typeof(UniquePersonV1.Entities.Insured).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.Insured), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                insuredCode = ModelAssembler.CreateInsureds(businessCollection).FirstOrDefault().InsuredCode;
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetInsuredByIndividualId");
            return insuredCode;
        }

        #region Punto de Control (Integracion)
        public Models.PersonAccountBankControl CreatePersonAccountBankControl(Models.PersonAccountBankControl personAccountBankControl)
        {
            try
            {
                return ModelAssembler.CreatePersonAccountBankControl((INTEN.UpPersonAccountBankControl)DataFacadeManager.Insert(EntityAssembler.CreatePersonAccountBankControl(personAccountBankControl)));
            }
            catch (Exception ex)
            {
                throw new Exception(Errors.ErrorControlPoint, ex);
            }
        }
        public Models.IndividualControl CreateIndividualControl(Models.IndividualControl individualControl)
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


        public Models.InsuredControl CreateInsuredControl(Models.InsuredControl insuredControl)
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
        #endregion


    }
}
