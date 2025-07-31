using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class EconomyActivityBusiness
    {
        public EconomicActivity GetEconomicActivitiesById(int id)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(EconomicActivity).Name);
            filter.Equal();
            filter.Constant(id);
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity), filter.GetPredicate());
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetEconomicActivitiesById");
            return ModelAssembler.CreateEconomicActivities(businessCollection).FirstOrDefault();
        }

        public List<Models.EconomicActivity> GetEconomicActivities()
        {
            return ModelAssembler.CreateEconomicActivities(DataFacadeManager.GetObjects(typeof(COMMEN.EconomicActivity)));
        }
    }
}