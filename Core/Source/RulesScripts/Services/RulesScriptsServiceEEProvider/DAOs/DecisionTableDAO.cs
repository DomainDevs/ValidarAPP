using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.Queries;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class DecisionTableDAO
    {

        /// <summary>
        /// obtiene una lista de Models.RuleBase
        /// </summary>
        /// <returns></returns>
        public static List<Models.RuleBase> GetDecisionTableList()
        {
            try
            {
                List<Models.RuleBase> ruleBaseList = new List<Models.RuleBase>();

                #region select
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.RuleBaseId, "ruleBase"), "RuleBaseId"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.IsPublished, "ruleBase"), "IsPublished"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.Description, "ruleBase"), "Description"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.PackageId, "ruleBase"), "PackageId"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.LevelId, "ruleBase"), "LevelId"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.CurrentFrom, "ruleBase"), "CurrentFrom"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.RuleBaseTypeCode, "ruleBase"), "RuleBaseTypeCode"));
                select.AddSelectValue(new SelectValue(new Column(RuleBase.Properties.RuleBaseVersion, "ruleBase"), "RuleBaseVersion"));

                select.AddSelectValue(new SelectValue(new Column(Entities.Level.Properties.Description, "level"), "LevelDescription"));
                select.AddSelectValue(new SelectValue(new Column(Package.Properties.Description, "package"), "PackageDescription"));

                #endregion

                #region join
                Join join = new Join(new ClassNameTable(typeof(RuleBase), "ruleBase"), new ClassNameTable(typeof(Entities.Level), "level"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(RuleBase.Properties.LevelId, "ruleBase")
                    .Equal()
                    .Property(Entities.Level.Properties.LevelId, "level")
                    .And()
                    .Property(RuleBase.Properties.PackageId, "ruleBase")
                    .Equal()
                    .Property(Entities.Level.Properties.PackageId, "level")
                    .GetPredicate());

                Join join2 = new Join(join, new ClassNameTable(typeof(Package), "package"), JoinType.Inner);
                join2.Criteria = (new ObjectCriteriaBuilder()
                    .Property(RuleBase.Properties.PackageId, "ruleBase")
                    .Equal()
                    .Property(Package.Properties.PackageId, "package")
                    .GetPredicate());
                #endregion

                #region filter
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Constant(1);
                filter.Equal();
                filter.Constant(1);
                #endregion

                select.Table = join2;
                select.Where = filter.GetPredicate();

                #region llegnado del modelo
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    Models.RuleBase ruleSetDTO = null;
                    while (reader.Read())
                    {
                        ruleSetDTO = new Models.RuleBase
                        {
                            RuleBaseId = (int)reader["RuleBaseId"],
                            Description = (string)reader["Description"],
                            Published = (bool)reader["IsPublished"] ? "Si" : "No",
                            IsPublished = (bool)reader["IsPublished"],
                            PackageId = (int)reader["PackageId"],
                            LevelId = (int)reader["LevelId"],
                            Current = ((DateTime)reader["CurrentFrom"]).ToString("dd/MM/yyyy"),
                            CurrentFrom = (DateTime)reader["CurrentFrom"],
                            RuleBaseVersion = (int)reader["RuleBaseVersion"],
                            LevelsDescription = (string)reader["LevelDescription"],
                            PackageDescription = (string)reader["PackageDescription"]
                        };

                        ruleBaseList.Add(ruleSetDTO);
                    }
                }
                #endregion

                return ruleBaseList;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDecisionTableList", ex);
            }
          
        }

        /// <summary>
        /// obtiene una lista de Models.Concept a partir del id
        /// </summary>
        /// <returns></returns>
        public static List<Models.Concept> GetConditionConcept(int id)
        {
            try
            {
                List<Models.Concept> concepts = new List<Models.Concept>();

                #region select
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptName, "concept"), "ConceptName"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.Description, "concept"), "Description"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptId, "concept"), "ConceptId"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.EntityId, "concept"), "EntityId"));
                                                                
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptControlCode, "concept"), "ConceptControlCode"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptTypeCode, "concept"), "ConceptTypeCode"));

                select.AddSelectValue(new SelectValue(new Column(RuleConditionConcept.Properties.OrderNum, "ruleConditionConcept"), "OrderNum"));
                select.AddSelectValue(new SelectValue(new Column(RuleConditionConcept.Properties.RuleBaseId, "ruleConditionConcept"), "RuleBaseId"));
                #endregion

                #region join
                Join join = new Join(new ClassNameTable(typeof(SCREN.Concept), "concept"), new ClassNameTable(typeof(RuleConditionConcept), "ruleConditionConcept"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(SCREN.Concept.Properties.EntityId, "concept")
                    .Equal()
                    .Property(RuleConditionConcept.Properties.EntityId, "ruleConditionConcept")
                    .And()
                    .Property(SCREN.Concept.Properties.ConceptId, "concept")
                    .Equal()
                    .Property(RuleConditionConcept.Properties.ConceptId, "ruleConditionConcept")
                    .GetPredicate());

                #endregion

                #region filter
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RuleConditionConcept.Properties.RuleBaseId, "ruleConditionConcept");
                filter.Equal();
                filter.Constant(id);
                #endregion

                select.Table = join;
                select.Where = filter.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    Models.Concept concept = null;
                    while (reader.Read())
                    {
                        concept = new Models.Concept
                        {
                            ConceptName = (string)reader["ConceptName"],
                            Description = (string)reader["Description"],
                            ConceptId = (int)reader["ConceptId"],
                            EntityId = (int)reader["EntityId"],
                            OrderNum = (int)reader["OrderNum"],
                            ConceptControlCode = (ConceptControlType)(int)reader["ConceptControlCode"],
                            ConceptTypeCode = (Enums.ConceptType)(int)reader["ConceptTypeCode"]
                        };

                        concepts.Add(concept);
                    }
                }

                return concepts.OrderBy(x => x.OrderNum).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConditionConcept", ex);
            }
            
        }

        /// <summary>
        /// obtiene una lista de Models.Concept a partir del id
        /// </summary>
        /// <returns></returns>
        public static List<Models.Concept> GetActionConcept(int id)
        {
            try
            {
                List<Models.Concept> concepts = new List<Models.Concept>();

                #region select
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptName, "concept"), "ConceptName"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.Description, "concept"), "Description"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptId, "concept"), "ConceptId"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.EntityId, "concept"), "EntityId"));
                                                              
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptTypeCode, "concept"), "ConceptTypeCode"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptControlCode, "concept"), "ConceptControlCode"));

                select.AddSelectValue(new SelectValue(new Column(RuleActionConcept.Properties.OrderNum, "ruleActionConcept"), "OrderNum"));
                select.AddSelectValue(new SelectValue(new Column(RuleActionConcept.Properties.RuleBaseId, "ruleActionConcept"), "RuleBaseId"));
                #endregion

                #region join
                Join join = new Join(new ClassNameTable(typeof(SCREN.Concept), "concept"), new ClassNameTable(typeof(RuleActionConcept), "ruleActionConcept"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(SCREN.Concept.Properties.EntityId, "concept")
                    .Equal()
                    .Property(RuleActionConcept.Properties.EntityId, "ruleActionConcept")
                    .And()
                    .Property(SCREN.Concept.Properties.ConceptId, "concept")
                    .Equal()
                    .Property(RuleActionConcept.Properties.ConceptId, "ruleActionConcept")
                    .GetPredicate());

                #endregion

                #region filter
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RuleActionConcept.Properties.RuleBaseId, "ruleActionConcept");
                filter.Equal();
                filter.Constant(id);
                #endregion

                select.Table = join;
                select.Where = filter.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    Models.Concept concept = null;
                    while (reader.Read())
                    {
                        concept = new Models.Concept
                        {
                            ConceptName = (string)reader["ConceptName"],
                            Description = (string)reader["Description"],
                            ConceptId = (int)reader["ConceptId"],
                            EntityId = (int)reader["EntityId"],
                            OrderNum = (int)reader["OrderNum"],
                            ConceptControlCode = (Enums.ConceptControlType)(int)reader["ConceptControlCode"],
                            ConceptTypeCode = (Enums.ConceptType)(int)(int)reader["ConceptTypeCode"]
                        };

                        concepts.Add(concept);
                    }
                }
                return concepts.OrderBy(x => x.OrderNum).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetActionConcept", ex);
            }            
        }

        /// <summary>
        /// obtiene una lista de Models.RuleCondition  a partir del ruleBaseId
        /// </summary>
        /// <returns></returns>
        public static List<Models.RuleCondition> GetDecisionTableConditionData(int ruleBaseId)
        {
            try
            {
                List<Models.RuleCondition> ruleConditions = new List<Models.RuleCondition>();

                #region select
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptId, "concept"), "ConceptId"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.EntityId, "concept"), "EntityId"));

                select.AddSelectValue(new SelectValue(new Column(RuleConditionConcept.Properties.OrderNum, "ruleConditionConcept"), "OrderNum"));
                select.AddSelectValue(new SelectValue(new Column(RuleConditionConcept.Properties.RuleBaseId, "ruleConditionConcept"), "RuleBaseId"));

                select.AddSelectValue(new SelectValue(new Column(RuleCondition.Properties.RuleId, "ruleCondition"), "RuleId"));
                select.AddSelectValue(new SelectValue(new Column(RuleCondition.Properties.CondValue, "ruleCondition"), "CondValue"));
                #endregion

                #region join
                Join join = new Join(new ClassNameTable(typeof(RuleConditionConcept), "ruleConditionConcept"),
                    new ClassNameTable(typeof(SCREN.Concept), "concept"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(RuleConditionConcept.Properties.EntityId, "ruleConditionConcept")
                    .Equal()
                    .Property(SCREN.Concept.Properties.EntityId, "concept")
                    .And()
                    .Property(RuleConditionConcept.Properties.ConceptId, "ruleConditionConcept")
                    .Equal()
                    .Property(SCREN.Concept.Properties.ConceptId, "concept")
                    .GetPredicate());

                Join join2 = new Join(join, new ClassNameTable(typeof(RuleCondition), "ruleCondition"), JoinType.Inner);
                join2.Criteria = (new ObjectCriteriaBuilder()
                    .Property(RuleConditionConcept.Properties.RuleBaseId, "ruleConditionConcept")
                    .Equal()
                    .Property(RuleCondition.Properties.RuleBaseId, "ruleCondition")
                    .And()
                    .Property(RuleConditionConcept.Properties.ConceptId, "ruleConditionConcept")
                    .Equal()
                    .Property(RuleCondition.Properties.ConceptId, "ruleCondition")
                    .GetPredicate());
                #endregion

                #region filter
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RuleConditionConcept.Properties.RuleBaseId, "ruleConditionConcept")
                    .Equal().Constant(ruleBaseId);
                #endregion

                select.Table = join2;
                select.Where = filter.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    Models.RuleCondition ruleCondition = null;
                    while (reader.Read())
                    {
                        ruleCondition = new Models.RuleCondition
                        {
                            ConceptId = (int)reader["ConceptId"],
                            EntityId = (int)reader["EntityId"],
                            OrderNumber = (int)reader["OrderNum"],
                            RuleBaseId = (int)reader["RuleBaseId"],
                            RuleId = (int)reader["RuleId"],
                            CondValue = (string)reader["CondValue"]
                        };

                        ruleConditions.Add(ruleCondition);
                    }
                }

                return ruleConditions.OrderBy(x => x.RuleId).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDecisionTableConditionData", ex);
            }
           
        }

        /// <summary>
        /// obtiene una lista de Models.RuleAction  a partir del ruleBaseId
        /// </summary>
        /// <returns></returns>
        public static List<Models.RuleAction> GetDecisionTableActionData(int ruleBaseId)
        {
            try
            {
                List<Models.RuleAction> ruleActions = new List<Models.RuleAction>();

                #region select
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptId, "concept"), "ConceptId"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.EntityId, "concept"), "EntityId"));

                select.AddSelectValue(new SelectValue(new Column(RuleActionConcept.Properties.OrderNum, "ruleActionConcept"), "OrderNum"));
                select.AddSelectValue(new SelectValue(new Column(RuleActionConcept.Properties.RuleBaseId, "ruleActionConcept"), "RuleBaseId"));

                select.AddSelectValue(new SelectValue(new Column(RuleAction.Properties.RuleId, "ruleAction"), "RuleId"));
                select.AddSelectValue(new SelectValue(new Column(RuleAction.Properties.ActionValue, "ruleAction"), "ActionValue"));
                #endregion

                #region join
                Join join = new Join(new ClassNameTable(typeof(RuleActionConcept), "ruleActionConcept"),
                    new ClassNameTable(typeof(SCREN.Concept), "concept"), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(RuleActionConcept.Properties.EntityId, "ruleActionConcept")
                    .Equal()
                    .Property(SCREN.Concept.Properties.EntityId, "concept")
                    .And()
                    .Property(RuleActionConcept.Properties.ConceptId, "ruleActionConcept")
                    .Equal()
                    .Property(SCREN.Concept.Properties.ConceptId, "concept")
                    .GetPredicate());

                Join join2 = new Join(join, new ClassNameTable(typeof(RuleCondition), "ruleCondition"), JoinType.Inner);
                join2.Criteria = (new ObjectCriteriaBuilder()
                    .Property(RuleActionConcept.Properties.RuleBaseId, "ruleActionConcept")
                    .Equal()
                    .Property(RuleCondition.Properties.RuleBaseId, "ruleCondition")
                    .And()
                    .Property(RuleActionConcept.Properties.ConceptId, "ruleActionConcept")
                    .Equal()
                    .Property(RuleCondition.Properties.ConceptId, "ruleCondition")
                    .GetPredicate());
                #endregion

                #region filter
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(RuleActionConcept.Properties.RuleBaseId, "ruleActionConcept")
                    .Equal().Constant(ruleBaseId);
                #endregion

                select.Table = join2;
                select.Where = filter.GetPredicate();

                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
                {
                    Models.RuleAction ruleAction = null;
                    while (reader.Read())
                    {
                        ruleAction = new Models.RuleAction
                        {
                            ConceptId = (int)reader["ConceptId"],
                            EntityId = (int)reader["EntityId"],
                            OrderNumber = (int)reader["OrderNum"],
                            RuleBaseId = (int)reader["RuleBaseId"],
                            RuleId = (int)reader["RuleId"],
                            ActionVal = (string)reader["ActionValue"]
                        };

                        ruleActions.Add(ruleAction);
                    }
                }

                return ruleActions.OrderBy(x => x.RuleId).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetDecisionTableActionData", ex);
            }            
        }
    }
}
