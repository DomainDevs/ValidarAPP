using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class CoHierarchyAssociationDAO
    {
        /// <summary>
        /// Get CoHierarchiesAssociation
        /// </summary>
        /// <returns>List of CoHierarchyAssociation</returns>  
        public List<CoHierarchyAssociation> GetCoHierarchiesAssociation()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.CoHierarchyAssociation)));
            List<CoHierarchyAssociation> hierarchies =Assemblers.ModelAssembler.CreateHierarchiesAssociation(businessCollection);

            return hierarchies;
        }

        /// <summary>
        /// Get CoHierarchiesAssociation
        /// </summary>
        /// <param name="moduleId">moduleId</param>
        /// <param name="subModuleId">subModuleId</param>
        /// <returns>List of CoHierarchyAssociation</returns>       
        public List<CoHierarchyAssociation> GetCoHierarchiesAssociationByModuleSubModule(int moduleId, int subModuleId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.CoHierarchyAssociation.Properties.ModuleCode, typeof(UniqueUser.Entities.CoHierarchyAssociation).Name);
            filter.Equal();
            filter.Constant(moduleId);
            filter.And();
            filter.Property(UniqueUser.Entities.CoHierarchyAssociation.Properties.SubmoduleCode, typeof(UniqueUser.Entities.CoHierarchyAssociation).Name);
            filter.Equal();
            filter.Constant(subModuleId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.CoHierarchyAssociation), filter.GetPredicate()));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetCoHierarchiesAssociationByModuleSubModule");
            return Assemblers.ModelAssembler.CreateHierarchiesAssociation(businessCollection);
        }
    }
}
