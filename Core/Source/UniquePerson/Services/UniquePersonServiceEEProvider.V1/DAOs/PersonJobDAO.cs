using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class PersonJobDAO
    {

        /// <summary>
        /// Guardar la informacion laboral de una persona
        /// </summary>
        /// <param name="personJob">Modelo PersonJob</param>
        /// <returns></returns>
        public Models.LabourPerson CreatePersonJob(Models.LabourPerson personJob, int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PersonJob personJobEntity = EntityAssembler.CreatePersonJob(personJob, individualId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
               daf.InsertObject(personJobEntity);
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
        public Models.LabourPerson UpdatePersonJob(Models.LabourPerson personJob)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int? def = null;
            PrimaryKey key = PersonJob.CreatePrimaryKey(personJob.IndividualId);
            PersonJob personJobEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                personJobEntity = (PersonJob)daf.GetObjectByPrimaryKey(key);
            }


            if (personJobEntity != null)
            {
                personJobEntity.OccupationCode = personJob.Occupation.Id;
                personJobEntity.IncomeLevelCode = personJob.IncomeLevel != null ? personJob.IncomeLevel.Id : def;
                personJobEntity.CompanyName = personJob.CompanyName;
                personJobEntity.JobSector = personJob.JobSector;
                personJobEntity.Position = personJob.Position;
                personJobEntity.Contact = personJob.Contact;
                personJobEntity.CompanyPhone = personJob.CompanyPhone != null ? personJob.CompanyPhone.Id : 0;
                personJobEntity.SpecialityCode = personJob.Speciality != null ? personJob.Speciality.Id : def;
                personJobEntity.OtherOccupationCode = personJob.OtherOccupation.Id != 0 ? (int?)personJob.OtherOccupation.Id : def;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.UpdateObject(personJobEntity);
                }

                if(personJob.PersonInterestGroup == null)
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
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdatePersonJob");
                return CreatePersonJob(personJob, personJob.IndividualId);
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
        public Models.LabourPerson GetPersonJobByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PersonJob.Properties.IndividualId, typeof(PersonJob).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PersonJob), filter.GetPredicate()));
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonJobByIndividualId");
            return ModelAssembler.CreatePersonJobs(businessCollection).FirstOrDefault();
        }
    }
}
