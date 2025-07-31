using Sistran.Core.Application.Script.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.DataFacade;
//using Sistran.Core.Application.Utilities.Entities;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public static class RuleEngineDAO
    {
        public static BasicConcept GetBasicConceptByConceptIdEntityId(int conceptId, int entityId)
        {
            PrimaryKey primaryKey = BasicConcept.CreatePrimaryKey(conceptId, entityId);
            return (BasicConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
        }

        public static ListConcept GetListConceptByConceptIdEntityId(int conceptId, int entityId)
        {
            PrimaryKey primaryKey = ListConcept.CreatePrimaryKey(conceptId, entityId);
            return (ListConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
        }

        public static Concept GetConceptByConceptIdEntityId(int conceptId, int entityId)
        {
            PrimaryKey primaryKey = Concept.CreatePrimaryKey(conceptId, entityId);
            return (Concept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
        }

        public static Concept GetConceptByConceptNameEntityId(string conceptName, int entityId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Concept.Properties.ConceptName, typeof(Concept).Name).Equal().Constant(conceptName);
            filter.And();
            filter.Property(Concept.Properties.EntityId, typeof(Concept).Name).Equal().Constant(entityId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(Concept), filter.GetPredicate());
            return (Concept)businessCollection[0];
        }

        public static List<Concept> GetConceptsByEntityId(int entityId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Concept.Properties.EntityId, typeof(Concept).Name).Equal().Constant(entityId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(Concept), filter.GetPredicate());
            return businessCollection.Cast<Concept>().ToList();
        }

        public static int GetEntityIdByEntityName(string entityName)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Entity.Properties.EntityName, typeof(Entity).Name).Equal().Constant(entityName);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(Entity), filter.GetPredicate());
            return ((Entity)businessCollection[0]).EntityId;
        }
        public static Entity GetEntityIdByEntityNameInitial(string entityName)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Entity.Properties.EntityName, typeof(Entity).Name).Equal().Constant(entityName);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(Entity), filter.GetPredicate());
            return (Entity)businessCollection[0];
        }

        public static List<PositionEntity> GetPositionEntities()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PositionEntity)));
            return businessCollection.Cast<PositionEntity>().ToList();
        }

        public static List<PositionEntity> GetPositionEntitiesByLevelIdPackageId(int levelId, int packageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PositionEntity.Properties.LevelId, typeof(PositionEntity).Name).Equal().Constant(levelId);
            filter.And();
            filter.Property(PositionEntity.Properties.PackageId, typeof(PositionEntity).Name).Equal().Constant(packageId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(PositionEntity), filter.GetPredicate());
            return businessCollection.Cast<PositionEntity>().ToList();
        }

        public static RuleSet GetRuleSetByRuleSetId(int ruleSetId)
        {
            PrimaryKey primaryKey = RuleSet.CreatePrimaryKey(ruleSetId);
            return (RuleSet)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
        }

        public static RuleSet UpdateRuleSetByRuleSet(RuleSet ruleSet)
        {
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(ruleSet);
            if (ruleSet != null)
            {
                return ruleSet;
            }
            else
            {
                return null;
            }
        }

        #region Cargar Objetos de Reglas en Cache
        /// <summary>
        /// Obtener todos Los conceptos
        /// </summary>
        public static void GetConcepts()
        {
            try
            {
                Stopwatch stw = new Stopwatch();
                stw.Start();
                if (!InProcCache.StateCache.ContainsKey(RulesConstant.Entityconcept))
                {
                    List<Concept> concepts = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                        filter.Property(Concept.Properties.EntityId).In().ListValue();
                        KeySettings.RulesValues.ForEach(x => filter.Constant(x));
                        filter.EndList();
                        concepts = daf.List(typeof(Concept), filter.GetPredicate()).Cast<Concept>().ToList();
                    }
                    if (concepts != null)
                    {
                        InProcCache.StateCache.TryAdd(RulesConstant.Entityconcept, concepts);
                    }
                }
                stw.Stop();
                Debug.WriteLine(stw.Elapsed);
            }
            finally { }
        }

        public static void GetEntities()
        {
            try
            {
                Stopwatch stw = new Stopwatch();
                stw.Start();
                if (!InProcCache.StateCache.ContainsKey(RulesConstant.entityId))
                {
                    List<Entity> entities = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        entities = daf.List(typeof(Entity), null).Cast<Entity>().ToList();
                    }
                    if (entities != null)
                    {
                        if (!InProcCache.StateCacheCurrent.ContainsKey(RulesConstant.entityId))
                        {
                            ConcurrentDictionary<string, object> concurrentDictionary = new ConcurrentDictionary<string, object>();
                            InProcCache.StateCacheCurrent[RulesConstant.entityId] = concurrentDictionary;
                        }
                        IEnumerable<KeyValuePair<string, object>> entiesBase = entities.Select((pair) => new KeyValuePair<string, object>(pair.EntityId.ToString(), pair.EntityName));
                        InProcCache.StateCacheCurrent[RulesConstant.entityId] = new ConcurrentDictionary<string, object>(entiesBase);
                    }
                }
                stw.Stop();
                Debug.WriteLine(stw.Elapsed);
            }
            finally { }
        }

        public static void GetPositionEntitiesInitial()
        {
            try
            {
                Stopwatch stw = new Stopwatch();
                stw.Start();
                if (!InProcCache.StateCache.ContainsKey(RulesConstant.positionEntities))
                {
                    List<PositionEntity> positionEntities = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        positionEntities = daf.List(typeof(PositionEntity), null).Cast<PositionEntity>().ToList();
                    }
                    if (positionEntities != null)
                    {
                        InProcCache.StateCache.TryAdd(RulesConstant.positionEntities, positionEntities);
                    }
                }
                stw.Stop();
                Debug.WriteLine(stw.Elapsed);

            }
            finally { }
        }
        public static void GetRulseSets()
        {
            try
            {
                Stopwatch stw = new Stopwatch();
                stw.Start();
                if (!InProcCache.StateCache.ContainsKey(RulesConstant.ruleSet))
                {
                    List<RuleSet> ruleSets = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        ruleSets = daf.List(typeof(RuleSet), null).Cast<RuleSet>().ToList();
                    }
                    if (ruleSets != null)
                    {
                        if (!InProcCache.StateCacheCurrent.ContainsKey(RulesConstant.ruleSet))
                        {
                            ConcurrentDictionary<string, object> concurrentDictionary = new ConcurrentDictionary<string, object>();
                            InProcCache.StateCacheCurrent[RulesConstant.ruleSet] = concurrentDictionary;
                        }
                        IEnumerable<KeyValuePair<string, object>> entiesBase = ruleSets.Select((pair) => new KeyValuePair<string, object>(pair.RuleSetId.ToString(), pair));
                        InProcCache.StateCacheCurrent[RulesConstant.ruleSet] = new ConcurrentDictionary<string, object>(entiesBase);
                    }
                }
            }
            finally { }
        }
        public static void GetListConcept()
        {
            try
            {
                if (!InProcCache.StateCache.ContainsKey(RulesConstant.entityListConcept))
                {
                    List<ListConcept> listConcept = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        listConcept = daf.List(typeof(ListConcept), null).Cast<ListConcept>().ToList();
                    }
                    if (listConcept != null)
                    {
                        InProcCache.StateCache.TryAdd(RulesConstant.entityListConcept, listConcept);
                    }
                }
            }
            finally { }
        }
        /// <summary>
        /// Obtenr CocneptosBasicos
        /// </summary>
        public static void GetBasicConcep()
        {
            try
            {
                if (!InProcCache.StateCache.ContainsKey(RulesConstant.entityBasicConcept))
                {
                    List<BasicConcept> basicConcept = null;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        basicConcept = daf.List(typeof(BasicConcept), null).Cast<BasicConcept>().ToList();
                    }
                    if (basicConcept != null)
                    {
                        InProcCache.StateCache.TryAdd(RulesConstant.entityBasicConcept, basicConcept);
                    }
                }
            }
            finally { }
        }
        public static void GetConceptDependencies()
        {
            try
            {
                List<ConceptDependencies> conceptDependencies = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    conceptDependencies = daf.List(typeof(ConceptDependencies), null).Cast<ConceptDependencies>().ToList();
                }
                if (conceptDependencies != null)
                {
                    InProcCache.StateCache.TryAdd(RulesConstant.ConceptDependencies, conceptDependencies);
                }
            }
            finally { }
        }

        #endregion reglas
    }
}