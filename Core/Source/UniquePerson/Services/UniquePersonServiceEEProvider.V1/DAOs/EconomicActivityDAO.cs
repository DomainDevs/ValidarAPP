using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PERMOD = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class EconomicActivityDAO
    {
        public List<PERMOD.EconomicActivity> GetEconomicActivities()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.EconomicActivity)));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetEconomicActivities");
            return ModelAssembler.CreateEconomicActivities(businessCollection);
        }

        public PERMOD.EconomicActivity GetEconomicActivitiesByEconomicActiviti(int EconomicActiviti)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.EconomicActivity.Properties.EconomicActivityCode, typeof(COMMEN.EconomicActivity).Name);
            filter.Equal();
            filter.Constant(EconomicActiviti);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.EconomicActivity), filter.GetPredicate()));
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetEconomicActivitiesByEconomicActiviti");
            return ModelAssembler.CreateEconomicActivities(businessCollection).FirstOrDefault();
        }
    }
}
