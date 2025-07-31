using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class ReportTypeDAO
    {
       
        public List<ReportType> GetReportTypes()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(REINSEN.ReportType.Properties.Enable, typeof(REINSEN.PriorityRetention).Name, 1);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(REINSEN.ReportType), filter.GetPredicate());
            return ModelAssembler.CreateReportTypes(businessObjects);
        }
    }
}