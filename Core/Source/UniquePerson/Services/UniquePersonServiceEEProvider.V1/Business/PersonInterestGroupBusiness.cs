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
    class PersonInterestGroupBusiness
    {
        public List<Models.InterestGroupsType> GetInterestGroupTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(InterestGroupType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetInterestGroupTypes");
            return ModelAssembler.CreatePersonInterests(businessCollection);
        }

        public List<Models.PersonInterestGroup> GetPersonInterestGroups(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PersonInterestGroup.Properties.IndividualId, typeof(PersonInterestGroup).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PersonInterestGroup), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPersonInterestGroups");

            return ModelAssembler.CreatePersonInterestGroups(businessCollection); ;
        }

        public Models.PersonInterestGroup CreatePersonInterestGroup(Models.PersonInterestGroup personInterestGroup)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            DataFacadeManager.Insert(EntityAssembler.CreatePersonInterestGroup(personInterestGroup));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreatePersonInterestGroups");

            return personInterestGroup;
        }

        public Models.PersonInterestGroup DeletePersonInterestGroup(Models.PersonInterestGroup personInterestGroup)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = PersonInterestGroup.CreatePrimaryKey(personInterestGroup.IndividualId, personInterestGroup.InterestGroupTypeId);
            DataFacadeManager.Delete(key);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.DeletePersonInterestGroup");

            return personInterestGroup;
        }
    }
}
