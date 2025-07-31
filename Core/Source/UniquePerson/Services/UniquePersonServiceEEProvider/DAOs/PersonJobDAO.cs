using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class PersonJobDAO
    {

        /// <summary>
        /// Guardar la informacion laboral de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.LaborPerson CreatePersonJob(Models.LaborPerson personJob, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PersonJob personJobEntity = EntityAssembler.CreatePersonJob(personJob.LaborPerson, individualId);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(personJobEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreatePersonJob");
            return ModelAssembler.CreatePersonJob(personJobEntity);
        }
        /// <summary>
        /// Actualizar los datos la borales de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.LaborPerson UpdatePersonJob(Models.LaborPerson personJob)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey key = PersonJob.CreatePrimaryKey(personJob.IndividualId);
            PersonJob personJobEntity = null;
            personJobEntity = (PersonJob)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);


            if (personJobEntity != null)
            {
                personJobEntity.OccupationCode = personJob.LaborPerson.Occupation.Id;
                personJobEntity.IncomeLevelCode = personJob.LaborPerson.IncomeLevel != null ? personJob.LaborPerson.IncomeLevel.Id : null;
                personJobEntity.CompanyName = personJob.LaborPerson.CompanyName;
                personJobEntity.JobSector = personJob.LaborPerson.JobSector;
                personJobEntity.Position = personJob.LaborPerson.Position;
                personJobEntity.Contact = personJob.LaborPerson.Contact;
                personJobEntity.CompanyPhone = personJob.LaborPerson.CompanyPhone != null ? personJob.LaborPerson.CompanyPhone.Id : 0;
                personJobEntity.SpecialityCode = personJob.LaborPerson.Speciality != null ? personJob.LaborPerson.Speciality.Id : null;
                personJobEntity.OtherOccupationCode = personJob.LaborPerson.OtherOccupation.Id != 0 ? (int?)personJob.LaborPerson.OtherOccupation.Id : null;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personJobEntity);

                if (personJob.PersonInterestGroup == null)
                {
                    personJob.PersonInterestGroup = new List<Models.PersonInterestGroup>();
                }

                List<Models.PersonInterestGroup> personInterestGroups = new PersonInterestGroupDAO().GetPersonInterestGroups(personJob.IndividualId);
                foreach (Models.PersonInterestGroup item in personJob.PersonInterestGroup)
                {
                    PersonInterestGroupDAO personInterestGroupDAO = new PersonInterestGroupDAO();
                    if (!personInterestGroups.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                    {
                        personInterestGroupDAO.CreatePersonInterestGroup(item);
                    }
                }
                foreach (Models.PersonInterestGroup item in personInterestGroups)
                {
                    PersonInterestGroupDAO personInterestGroupDAO = new PersonInterestGroupDAO();
                    if (!personJob.PersonInterestGroup.Exists(x => x.InterestGroupTypeId == item.InterestGroupTypeId))
                    {
                        personInterestGroupDAO.DeletePersonInterestGroup(item);
                    }
                }

            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.UpdatePersonJob");
                return CreatePersonJob(personJob, personJob.IndividualId);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.UpdatePersonJob");
            return ModelAssembler.CreatePersonJob(personJobEntity);
        }
        /// <summary>
        /// Buscar la informacion laboral de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        public Models.LaborPerson GetPersonJobByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PersonJob.Properties.IndividualId, typeof(PersonJob).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PersonJob), filter.GetPredicate()));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetPersonJobByIndividualId");
            return ModelAssembler.CreatePersonJobs(businessCollection).FirstOrDefault();
        }
    }
}
