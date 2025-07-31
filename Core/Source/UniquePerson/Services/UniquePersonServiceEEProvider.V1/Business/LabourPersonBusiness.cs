using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    class LabourPersonBusiness
    {

        /// <summary>
        /// Guardar la informacion laboral de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.LabourPerson CreateLabourPerson(Models.LabourPerson personJob, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int? def = null;

            PersonJob personJobEntity = EntityAssembler.CreatePersonJob(personJob, individualId);
            //personJobEntity = (PersonJob)
            DataFacadeManager.Instance.GetDataFacade().InsertObject(personJobEntity);

            //Actualiza la información de la persona
            PrimaryKey key = Person.CreatePrimaryKey(personJob.IndividualId);
            Person personEntity = null;
            personEntity = (Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (personEntity != null)
            {
                personEntity.EducativeLevelCode = personJob.EducativeLevel != null ? personJob.EducativeLevel.Id : def;
                personEntity.HouseTypeCode = personJob.HouseType != null ? personJob.HouseType.Id : def;
                personEntity.SocialLayerCode = personJob.SocialLayer != null ? personJob.SocialLayer.Id : def;
                personEntity.Children = personJob.Children;
                personEntity.SpouseName = personJob.SpouseName;
                personEntity.BirthCountryCode = personJob.BirthCountryId;
                personEntity.PersonTypeCode = personJob.PersonType != null ? personJob.PersonType.Id : def;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personEntity);
            }

            List<Models.PersonInterestGroup> personInterestGroups = new PersonInterestGroupBusiness().GetPersonInterestGroups(personJob.IndividualId);
            foreach (Models.PersonInterestGroup item in personJob.PersonInterestGroup)
            {
                PersonInterestGroupBusiness personInterestGroupbusiness = new PersonInterestGroupBusiness();
                if (!personInterestGroups.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                {
                    personInterestGroupbusiness.CreatePersonInterestGroup(item);
                }
            }
            foreach (Models.PersonInterestGroup item in personInterestGroups)
            {
                PersonInterestGroupBusiness personInterestGroupDAO = new PersonInterestGroupBusiness();
                if (!personJob.PersonInterestGroup.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                {
                    personInterestGroupDAO.DeletePersonInterestGroup(item);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePersonJob");
            return ModelAssembler.CreatePersonJob(personJobEntity);
        }

        /// <summary>
        /// Actualizar los datos la borales de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.LabourPerson UpdateLabourPerson(Models.LabourPerson personJob)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int? def = null;

            PrimaryKey key = PersonJob.CreatePrimaryKey(personJob.IndividualId);
            PersonJob personJobEntity = null;
            personJobEntity = (PersonJob)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            PrimaryKey keyperson = Person.CreatePrimaryKey(personJob.IndividualId);
            Person personEntity = null;
            personEntity = (Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyperson);


            //Actualiza el grupo de interés
            List<Models.PersonInterestGroup> personInterestGroups = new PersonInterestGroupBusiness().GetPersonInterestGroups(personJob.IndividualId);
            foreach (Models.PersonInterestGroup item in personJob.PersonInterestGroup)
            {
                PersonInterestGroupBusiness personInterestGroupbusiness = new PersonInterestGroupBusiness();
                if (!personInterestGroups.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                {
                    personInterestGroupbusiness.CreatePersonInterestGroup(item);
                }
            }
            foreach (Models.PersonInterestGroup item in personInterestGroups)
            {
                PersonInterestGroupBusiness personInterestGroupDAO = new PersonInterestGroupBusiness();
                if (!personJob.PersonInterestGroup.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                {
                    personInterestGroupDAO.DeletePersonInterestGroup(item);
                }
            }

            //Actualiza la información de la persona
            if (personEntity != null)
            {
                personEntity.EducativeLevelCode = personJob.EducativeLevel?.Id;
                personEntity.HouseTypeCode = personJob.HouseType?.Id;
                personEntity.SocialLayerCode = personJob.SocialLayer?.Id;
                personEntity.Children = personJob.Children;
                personEntity.SpouseName = personJob.SpouseName;
                personEntity.BirthCountryCode = personJob.BirthCountryId;
                personEntity.PersonTypeCode = personJob.PersonType?.Id;


                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personEntity);

            }

            if (personJobEntity != null)
            {
                personJobEntity.OccupationCode = personJob.Occupation.Id;
                personJobEntity.IncomeLevelCode = personJob.IncomeLevel?.Id;
                personJobEntity.CompanyName = personJob.CompanyName;
                personJobEntity.JobSector = personJob.JobSector;
                personJobEntity.Position = personJob.Position;
                personJobEntity.Contact = personJob.Contact;
                personJobEntity.CompanyPhone = personJob.CompanyPhone?.Id;
                personJobEntity.SpecialityCode = personJob.Speciality?.Id;
                personJobEntity.OtherOccupationCode = personJob.OtherOccupation?.Id;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personJobEntity);

                if (personJob.PersonInterestGroup == null)
                {
                    personJob.PersonInterestGroup = new List<Models.PersonInterestGroup>();
                }
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePersonJob");
                return CreateLabourPerson(personJob, personJob.IndividualId);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePersonJob");
            return ModelAssembler.CreatePersonJob(personJobEntity);
        }
        public Models.LabourPerson UpdatePerson(Models.LabourPerson personJob)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int? def = null;

            PrimaryKey key = PersonJob.CreatePrimaryKey(personJob.IndividualId);
            PersonJob personJobEntity = null;
            personJobEntity = (PersonJob)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            PrimaryKey keyperson = Person.CreatePrimaryKey(personJob.IndividualId);
            Person personEntity = null;
            personEntity = (Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(keyperson);


            //Actualiza el grupo de interés
            List<Models.PersonInterestGroup> personInterestGroups = new PersonInterestGroupBusiness().GetPersonInterestGroups(personJob.IndividualId);
            foreach (Models.PersonInterestGroup item in personJob.PersonInterestGroup)
            {
                PersonInterestGroupBusiness personInterestGroupbusiness = new PersonInterestGroupBusiness();
                if (!personInterestGroups.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                {
                    personInterestGroupbusiness.CreatePersonInterestGroup(item);
                }
            }
            foreach (Models.PersonInterestGroup item in personInterestGroups)
            {
                PersonInterestGroupBusiness personInterestGroupDAO = new PersonInterestGroupBusiness();
                if (!personJob.PersonInterestGroup.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                {
                    personInterestGroupDAO.DeletePersonInterestGroup(item);
                }
            }

            if (personJob.PersonInterestGroup == null)
            {
                personJob.PersonInterestGroup = new List<Models.PersonInterestGroup>();
            }


            if (personJobEntity != null)
            {
                personJobEntity.OccupationCode = personJob.Occupation.Id;
                personJobEntity.IncomeLevelCode = personJob.IncomeLevel?.Id;
                personJobEntity.CompanyName = personJob.CompanyName;
                personJobEntity.JobSector = personJob.JobSector;
                personJobEntity.Position = personJob.Position;
                personJobEntity.Contact = personJob.Contact;
                personJobEntity.CompanyPhone = personJob.CompanyPhone?.Id;
                personJobEntity.SpecialityCode = personJob.Speciality?.Id;
                personJobEntity.OtherOccupationCode = personJob.OtherOccupation?.Id;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personJobEntity);
            }
            else
            {

                return CreateLabourPerson(personJob, personJob.IndividualId);
            }

            //Actualiza la información de la persona
            if (personEntity != null)
            {
                personEntity.EducativeLevelCode = personJob.EducativeLevel?.Id;
                personEntity.HouseTypeCode = personJob.HouseType?.Id;
                personEntity.SocialLayerCode = personJob.SocialLayer?.Id;
                personEntity.Children = personJob.Children;
                personEntity.SpouseName = personJob.SpouseName;
                personEntity.BirthCountryCode = personJob.BirthCountryId;
                personEntity.PersonTypeCode = personJob.PersonType?.Id;


                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personEntity);

            }


           

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePersonJob");
            return ModelAssembler.CreatePersonJob(personJobEntity);
        }

       
        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        public Models.LabourPerson GetLabourPersonByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey primaryKeyLabourPerson = PersonJob.CreatePrimaryKey(individualId);
            var labourPersonEntity = (PersonJob)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKeyLabourPerson);

            PrimaryKey primaryKeyPerson = Person.CreatePrimaryKey(individualId);
            var personEntity = (Person)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKeyPerson);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PersonInterestGroup.Properties.IndividualId, typeof(PersonInterestGroup).Name);
            filter.Equal();
            filter.Constant(individualId);

            List<Models.PersonInterestGroup> personInterestGroups = new List<Models.PersonInterestGroup>();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                BusinessCollection businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PersonInterestGroup), filter.GetPredicate()));
                personInterestGroups = ModelAssembler.CreatePersonInterestGroups(businessCollection);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonJobByIndividualId");
            return ModelAssembler.CreatePersonJob(labourPersonEntity, personEntity, personInterestGroups);
        }
    }
}
