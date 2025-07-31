using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class EntityDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static PARAMEN.Entity CreateEntity(PARAMEN.Entity entity)
        {
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static PARAMEN.Entity UpdateEntity(PARAMEN.Entity entity)
        {
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public static void DeleteEntity(PARAMEN.Entity entity)
        {
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListEntity(Predicate filter, string[] sort)
        {
            return new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.Entity), filter, sort));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public static PARAMEN.Entity FindEntity(int entityId)
        {
            PrimaryKey key = PARAMEN.Entity.CreatePrimaryKey(entityId);
            PARAMEN.Entity entity = (PARAMEN.Entity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            return entity;
        }
        
        /// <summary>
        /// Busca el PARAMEN.Entity segun el nombre especificado
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException">
        /// PRPERR_ENTITY_NOT_EXISTS
        /// or
        /// TOO_MANY_ENTITIES_WITH_THE_SAME_NAME
        /// </exception>
        public static PARAMEN.Entity GetEntityByName(string entityName)
        {
            try
            {
                PARAMEN.Entity.EntityTypes entityType = (PARAMEN.Entity.EntityTypes)Enum.Parse(typeof(PARAMEN.Entity.EntityTypes), entityName);
                return (PARAMEN.Entity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(PARAMEN.Entity.CreatePrimaryKey((int)entityType));
            }
            catch
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(PARAMEN.Entity.Properties.EntityName);
                filter.Equal();
                filter.Constant(entityName);

                BusinessCollection entityCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(PARAMEN.Entity), filter.GetPredicate());

                if (entityCollection.Count < 1)
                {
                    throw new BusinessException("PRPERR_ENTITY_NOT_EXISTS", new object[] { entityName });
                }
                else if (entityCollection.Count > 1)
                {
                    throw new BusinessException("TOO_MANY_ENTITIES_WITH_THE_SAME_NAME", new object[] { entityName });
                }
                return (PARAMEN.Entity)entityCollection[0];
            }
        }
    }
}
