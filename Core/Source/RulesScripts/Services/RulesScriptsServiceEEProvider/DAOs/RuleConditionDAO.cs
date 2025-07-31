using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using System.Data;
using Sistran.Core.Framework.BAF;
using System;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Application.Utilities.DataFacade;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class RuleConditionDAO
    {
        /// <summary>
        /// crea un RuleCondition
        /// </summary>
        /// <param name="RuleCondition"></param>
        /// <returns></returns>
        public static RuleCondition CreateRuleCondition(RuleCondition RuleCondition)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(RuleCondition);
                return RuleCondition;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleCondition", ex);
            }

        }

        public static void CreateRuleConditions(BusinessCollection<RuleCondition> listRuleCondition)
        {
            try
            {
                if (Context.Current == null)
                {
                    Context context = new Context();
                }
                DataFacadeManager.Instance.GetDataFacade().InsertObjects(listRuleCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleCondition", ex);
            }
        }

        /// <summary>
        /// edita un RuleCondition
        /// </summary>
        /// <param name="RuleCondition"></param>
        /// <returns></returns>
        public static RuleCondition UpdateRuleCondition(RuleCondition RuleCondition)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(RuleCondition);
                return RuleCondition;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error UpdateRuleCondition", ex);
            }
        }

        /// <summary>
        /// elimina un RuleCondition
        /// </summary>
        /// <param name="RuleCondition"></param>
        /// <returns></returns>
        public static RuleCondition GetRuleCondition(RuleCondition RuleCondition)
        {
            try
            {
                PrimaryKey key = RuleCondition.CreatePrimaryKey(RuleCondition.RuleBaseId, RuleCondition.RuleId, RuleCondition.ConditionId);
                return (RuleCondition)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error GetRuleCondition", ex);
            }

        }

        /// <summary>
        /// otiene una lsita de Models.Concept a partir del ruleBaseId
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <returns></returns>
        public static List<Models.Concept> GetConditionConcept(int ruleBaseId)
        {
            try
            {
                List<Models.Concept> concepts = new List<Models.Concept>();

                #region select
                SelectQuery select = new SelectQuery();
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptId, "concept"), "ConceptId"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.EntityId, "concept"), "EntityId"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.Description, "concept"), "Description"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptName, "concept"), "ConceptName"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptTypeCode, "concept"), "ConceptTypeCode"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.KeyOrder, "concept"), "KeyOrder"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsStatic, "concept"), "IsStatic"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.ConceptControlCode, "concept"), "ConceptControlCode"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsVisible, "concept"), "IsVisible"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsNullable, "concept"), "IsNullable"));
                select.AddSelectValue(new SelectValue(new Column(SCREN.Concept.Properties.IsPersistible, "concept"), "IsPersistible"));
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
                filter.Constant(ruleBaseId);
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
                            ConceptId = (int)reader["ConceptId"],
                            EntityId = (int)reader["EntityId"],
                            Description = (string)reader["Description"],
                            ConceptName = (string)reader["ConceptName"],
                            ConceptTypeCode = (Enums.ConceptType)(int)reader["ConceptTypeCode"],
                            KeyOrder = (int)reader["KeyOrder"],
                            IsStatic = (bool)reader["IsStatic"],
                            ConceptControlCode = (Enums.ConceptControlType)(int)reader["ConceptControlCode"],
                            IsVisible = (bool)reader["IsVisible"],
                            IsNull = (bool)reader["IsNullable"],
                            IsPersistible = (bool)reader["IsPersistible"],
                            OrderNum = (int)reader["OrderNum"]
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
        /// obtiene un BusinessCollection a partir de  ruleBaseId, ruleId
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static BusinessCollection ListRuleCondition(int ruleBaseId, int? ruleId)
        {
            try
            {
                if (Context.Current == null)
                {
                    Context context = new Context();
                }

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                if (ruleId == null)
                {
                    filter.PropertyEquals(Entities.RuleCondition.Properties.RuleBaseId, ruleBaseId);
                }
                else
                {
                    filter.PropertyEquals(Entities.RuleCondition.Properties.RuleBaseId, ruleBaseId)
                        .And()
                        .PropertyEquals(Entities.RuleCondition.Properties.RuleId, ruleId);
                }

                var list = DataFacadeManager.Instance.GetDataFacade().SelectObjects(
                    typeof(RuleCondition), filter.GetPredicate(), new string[] { Entities.RuleCondition.Properties.OrderNum });

                var collection = new BusinessCollection(list);

                return collection;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRuleCondition", ex);
            }

        }

        /// <summary>
        /// elimina un RuleCondition
        /// </summary>
        /// <param name="RuleCondition"></param>
        public static void DeleteRuleCondition(RuleCondition RuleCondition)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(RuleCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Delete RuleCondition", ex);
            }

        }

        public static BusinessCollection GetRuleCondition(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RuleCondition), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleActionConcept", ex);
            }
        }

       
    }
}
