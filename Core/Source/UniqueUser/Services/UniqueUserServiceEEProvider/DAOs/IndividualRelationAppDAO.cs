using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniqueUserServices.EEProvider.DAOs
{
    public class IndividualRelationAppDAO
    {
        /// <summary>
        /// elimina relación de individuos por userId
        /// </summary>
        /// <param name="parentIndividualId">parentIndividualId</param>
        /// <returns></returns>
        public void DeleteIndividualRelationAppByUserId(int parentIndividualId)
        {
            DeleteQuery delete = new DeleteQuery();
            delete.Table = new ClassNameTable(typeof(UniquePerson.Entities.IndividualRelationApp));
            ObjectCriteriaBuilder where = new ObjectCriteriaBuilder();
            where.Property(UniquePerson.Entities.IndividualRelationApp.Properties.ParentIndividualId).Equal().Constant(parentIndividualId);
            delete.Where = where.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().Execute(delete);
        }

        /// <summary>
        /// crea relación de individuos
        /// </summary>
        /// <param name="individualRelationApp">IndividualRelation a crear</param>
        /// <returns></returns>
        public UniquePerson.Entities.IndividualRelationApp CreateIndividualRelationApp(UniquePerson.Entities.IndividualRelationApp individualRelationApp)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(individualRelationApp);
            return individualRelationApp;
        }

        /// <summary>
        /// get relación de individuos
        /// </summary>
        /// <param name="individualRelationApp">IndividualRelation a crear</param>
        /// <returns></returns>
        public UniquePerson.Entities.IndividualRelationApp GetIndividualRelationApp(Models.IndividualRelationApp individualRelationApp)
        {
            PrimaryKey key = UniquePerson.Entities.IndividualRelationApp.CreatePrimaryKey(individualRelationApp.Id);
            UniquePerson.Entities.IndividualRelationApp entity = (UniquePerson.Entities.IndividualRelationApp)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return entity;
        }

        /// <summary>
        /// guarda relación de individuos
        /// </summary>
        /// <param name="List<Models.IndividualRelationApp>">list of IndividualRelationApp</param>
        /// <returns></returns>
        public void SaveIndividualRelationApp(List<Models.IndividualRelationApp> individualsRelationApp)
        {
            List<UniquePerson.Entities.IndividualRelationApp> entities = GetIndividualsRelationAppByIndividualId(individualsRelationApp.FirstOrDefault().Id);

            foreach (UniquePerson.Entities.IndividualRelationApp item in entities)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(item);
            }

            foreach (Models.IndividualRelationApp item in individualsRelationApp)
            {
                PrimaryKey key = UniquePerson.Entities.IndividualRelationApp.CreatePrimaryKey(item.Id);
                UniquePerson.Entities.IndividualRelationApp entity = (UniquePerson.Entities.IndividualRelationApp)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                if (entity == null)
                {
                    entity = Assemblers.EntityAssembler.CreateIndividualRelationApp(item);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                }
                else
                {
                    entity.AgentAgencyId = item.Agency.Id;
                    entity.ChildIndividualId = item.Agency.Agent.IndividualId;
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
                }
            }
        }

        public List<UniquePerson.Entities.IndividualRelationApp> GetIndividualsRelationAppByIndividualId(int individualId)
        {
            List<UniquePerson.Entities.IndividualRelationApp> entities = new List<UniquePerson.Entities.IndividualRelationApp>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property("ParentIndividualId");
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.IndividualRelationApp), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                entities = businessCollection.Cast<UniquePerson.Entities.IndividualRelationApp>().ToList();
            }
            return entities;
        }
    }
}