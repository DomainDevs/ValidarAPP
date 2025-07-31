using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.UniqueUserServices.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    /// <summary>
    /// Dao CoHierarchyAccess
    /// </summary>
    public class CoHierarchyAccessDAO
    {

        /// <summary>
        /// Delete UserSalesPoint by userId
        /// </summary>
        /// <param name="userId">userId</param>
        public void DeleteCoHierarchyAccessByUserId(int userId)
        {
            DeleteQuery delete = new DeleteQuery();
            delete.Table = new ClassNameTable(typeof(UniqueUser.Entities.CoHierarchyAccesses));
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(UniqueUser.Entities.CoHierarchyAccesses.Properties.UserId).Equal().Constant(userId);
            delete.Where = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(delete);
        }
        /// <summary>
        /// Save CoHierarchyAccess ByUserId
        /// </summary>
        /// <param name="coHierarchiesAssociation">List of coHierarchiesAssociation</param>
        /// <param name="userId">userId</param>
        public void CreateCoHierarchyAccessByUserId(List<CoHierarchyAssociation> coHierarchiesAssociation, int userId)
        {
            List<UniqueUser.Entities.CoHierarchyAccesses> entities = GetCoHierarchiesAccessByUserId(userId);

            foreach (UniqueUser.Entities.CoHierarchyAccesses item in entities)
            {
                CoHierarchyAssociation coHierarchyAssociation = coHierarchiesAssociation.Where(x => x.Module.Id == item.ModuleCode
                    && x.SubModule.Id == item.SubmoduleCode && x.Id == item.HierarchyCode).FirstOrDefault();
                if (coHierarchyAssociation == null)
                {
                    DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
                }
            }

            foreach (CoHierarchyAssociation coHierarchyAssociation in coHierarchiesAssociation)
            {
                PrimaryKey key = UniqueUser.Entities.CoHierarchyAccesses.CreatePrimaryKey(userId, coHierarchyAssociation.Module.Id, coHierarchyAssociation.SubModule.Id, coHierarchyAssociation.Id);
                UniqueUser.Entities.CoHierarchyAccesses entityHierarchy = (UniqueUser.Entities.CoHierarchyAccesses)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (entityHierarchy == null)
                {
                    entityHierarchy = Assemblers.EntityAssembler.CreateCoHierarchyAccess(coHierarchyAssociation, userId);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entityHierarchy);
                }
            }
        }

        /// <summary>
        /// Get CoHierarchyAccess ByUserId
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns> List of CoHierarchyAccess</returns>
        public List<UniqueUser.Entities.CoHierarchyAccesses> GetCoHierarchiesAccessByUserId(int userId)
        {
            List<UniqueUser.Entities.CoHierarchyAccesses> entities = new List<UniqueUser.Entities.CoHierarchyAccesses>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniqueUser.Entities.CoHierarchyAccesses.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniqueUser.Entities.CoHierarchyAccesses), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                entities = businessCollection.Cast<UniqueUser.Entities.CoHierarchyAccesses>().ToList();
            }
            return entities;
        }


        public List<CoHierarchyAssociation> GetHierarchiesAccessByUserId(int userId)
        {
            List<CoHierarchyAssociation> hierarchies = new List<CoHierarchyAssociation>();

            HierarchyAccessView hierarchiesView = new HierarchyAccessView();
            ViewBuilder builder = new ViewBuilder("HierarchyAccessView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(UniqueUser.Entities.CoHierarchyAccesses.Properties.UserId);
            filter.Equal();
            filter.Constant(userId);

            builder.Filter = filter.GetPredicate();
         
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, hierarchiesView);
            }

            if (hierarchiesView.CoHierarchyAssociations.Count > 0)
            {
                hierarchies = Assemblers.ModelAssembler.CreateModelHierarchiesAssociation(hierarchiesView.CoHierarchyAssociations, hierarchiesView.Modules, hierarchiesView.SubModules);
            }
            
            return hierarchies;
        }
    }
}
