using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class PersonBusiness
    {
        /// <summary>
        /// crear nueva persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.Person CreatePerson(Models.Person person)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    person.IndividualType = IndividualType.Person;
                    UniquePersonV1.Entities.Person personEntity = EntityAssembler.CreatePerson(person);

                    personEntity = (Person)DataFacadeManager.Insert(personEntity);
                    person.IndividualId = personEntity.IndividualId;

                   
                    if (person.PersonType != null)
                    {
                        CreatePersonIndividualType(person.PersonType, personEntity.IndividualId);
                    }
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Complete();
                    return person;
                }
                catch (DuplicatedObjectException)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw;
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw new BusinessException("Error in CreatePerson", ex);
                }

            }

        }

        /// <summary>
        /// actualiza una persona
        /// </summary>
        /// <param name="person">datos persona</param>
        /// <returns></returns>
        public Models.Person UpdatePerson(Models.Person person)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    person.IndividualType = IndividualType.Person;
                    PrimaryKey primaryKey = Person.CreatePrimaryKey(person.IndividualId);
                    var personEntity = (Person)DataFacadeManager.GetObject(primaryKey);
                    personEntity.IndividualTypeCode = (int)person.IndividualType;
                    personEntity.EconomicActivityCode = person.EconomicActivity.Id;
                    personEntity.Gender = person.Gender;
                    personEntity.BirthDate = person.BirthDate == null ? default(DateTime) : (DateTime)person.BirthDate;
                    personEntity.BirthPlace = person.BirthPlace;
                    personEntity.Name = person.FullName;
                    personEntity.IdCardTypeCode = person.IdentificationDocument.DocumentType.Id;
                    personEntity.IdCardNo = person.IdentificationDocument.Number;
                    personEntity.Surname = person.SurName;
                    personEntity.MotherLastName = person.SecondSurName;
                    personEntity.MaritalStatusCode = person.MaritalStatus.Id;
                    personEntity.CheckPayable = person.CheckPayable;
                    personEntity.DataProtection = person?.DataProtection ?? false;

                    DataFacadeManager.Update(personEntity);
                    person.IndividualId = personEntity.IndividualId;
                    if (person.PersonType != null)
                    {
                        CreatePersonIndividualType(person.PersonType, personEntity.IndividualId);
                    }
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdatePerson");
                    transaction.Complete();
                    return person;
                }
                catch (DuplicatedObjectException)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw;
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw new BusinessException("Error in CreatePerson", ex);
                }

            }

        }

        /// <summary>
        /// Creates the type of the person individual.
        /// </summary>
        /// <param name="personIndividualType">Type of the person individual.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.PersonIndividualType CreatePersonIndividualType(Models.PersonType personIndividualType, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                PersonIndividualType personIndividualTypeEntity = EntityAssembler.CreatePersonIndividualType(personIndividualType, individualId);
                DataFacadeManager.Insert(personIndividualTypeEntity);
                return ModelAssembler.CreatePersonIndividualType(personIndividualTypeEntity);
            }

            catch (DuplicatedObjectException)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                throw;
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                throw new BusinessException("Error in CreatePerson", ex);
            }

        }
        public List<MOUP.Person> GetPersonByDocument(string documentNumber)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.Property(Person.Properties.IdCardNo, typeof(Person).Name);
            filter.Equal();
            filter.Constant(documentNumber);
            var personCollection = DataFacadeManager.GetObjects(typeof(Person), filter.GetPredicate());
            return ModelAssembler.CreatePersons(personCollection);
        }

        public MOUP.Person GetPersonByIndividualId(int id)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.Person.CreatePrimaryKey(id);
            var personCollection = (Person)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreatePerson(personCollection);
        }

        public List<MOUP.Person> GetPersonAdv(MOUP.Person person)
        {
            bool useAdd = false;
            var filter = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(person.IdentificationDocument.Number))
            {
                filter.Property(Person.Properties.IdCardNo, typeof(Person).Name);
                filter.Equal();
                filter.Constant(person.IdentificationDocument.Number);
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(person.SurName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(UniquePersonV1.Entities.Person.Properties.Surname, typeof(UniquePersonV1.Entities.Person).Name);
                filter.Like();
                filter.Constant(person.SurName + "%");
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(person.SecondSurName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(UniquePersonV1.Entities.Person.Properties.MotherLastName, typeof(UniquePersonV1.Entities.Person).Name);
                filter.Like();
                filter.Constant(person.SecondSurName + "%");
                useAdd = true;
            }
            if (!string.IsNullOrEmpty(person.FullName))
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(UniquePersonV1.Entities.Person.Properties.Name, typeof(UniquePersonV1.Entities.Person).Name);
                filter.Like();
                filter.Constant(person.FullName + "%");
                useAdd = true;
            }
            if (person.IndividualId > 0)
            {
                if (useAdd)
                {
                    filter.And();
                }
                filter.Property(UniquePersonV1.Entities.Person.Properties.IndividualId, typeof(UniquePersonV1.Entities.Person).Name);
                filter.Like();
                filter.Constant(person.IndividualId);
            }

            var personCollection = DataFacadeManager.GetObjects(typeof(Person), filter.GetPredicate());
            return ModelAssembler.CreatePersons(personCollection);
        }

        public Models.Person UpdatePersonBasicInfo(Models.Person person)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    PrimaryKey primaryKey = Person.CreatePrimaryKey(person.IndividualId);
                    var personEntity = (Person)DataFacadeManager.GetObject(primaryKey);
                    personEntity.Name = person.Name;
                    personEntity.Surname = person.SurName;
                    personEntity.MotherLastName = person.SecondSurName;

                    personEntity.CheckPayable = $"{person.Name??string.Empty} {person.SurName ?? string.Empty} {person.SecondSurName ?? string.Empty}";

                    DataFacadeManager.Update(personEntity);
                    person.IndividualId = personEntity.IndividualId;
                    if (person.PersonType != null)
                    {
                        CreatePersonIndividualType(person.PersonType, personEntity.IndividualId);
                    }

                    PrimaryKey primaryKeyAgent = Agent.CreatePrimaryKey(person.IndividualId);
                    Agent entityAgent = (Agent)DataFacadeManager.GetObject(primaryKeyAgent);

                    if (entityAgent != null)
                    {
                        entityAgent.CheckPayableTo = personEntity.CheckPayable;
                        DataFacadeManager.Update(entityAgent);
                    }

                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdatePersonBasicInfo");
                    transaction.Complete();
                    return person;
                }
                catch (DuplicatedObjectException)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw;
                }
                catch (Exception ex)
                {
                    stopWatch.Stop();
                    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreatePerson");
                    transaction.Dispose();
                    throw new BusinessException("Error in CreatePerson", ex);
                }

            }

        }

    }
}
