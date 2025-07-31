using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Application.Utilities.DataFacade;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class RuleActionDAO
    {
        /// <summary>
        /// crea un RuleAction
        /// </summary>
        /// <param name="RuleAction"></param>
        /// <returns></returns>
        public static RuleAction CreateRuleAction(RuleAction RuleAction)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(RuleAction);
                return RuleAction;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleAction", ex);
            }

        }

        public static void CreateRuleActions(BusinessCollection<RuleAction> listRuleAction)
        {
            try
            {
                if (Context.Current == null)
                {
                    Context context = new Context();
                }
                DataFacadeManager.Instance.GetDataFacade().InsertObjects(listRuleAction);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleAction", ex);
            }

        }

        /// <summary>
        /// edita un RuleAction
        /// </summary>
        /// <param name="RuleAction"></param>
        /// <returns></returns>
        public static RuleAction UpdateRuleAction(RuleAction RuleAction)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(RuleAction);
                return RuleAction;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRuleAction", ex);
            }

        }

        /// <summary>
        /// obtiene un RuleAction
        /// </summary>
        /// <param name="RuleAction"></param>
        /// <returns></returns>
        public static RuleAction GetRuleAction(RuleAction RuleAction)
        {
            try
            {
                PrimaryKey key = RuleAction.CreatePrimaryKey(RuleAction.RuleBaseId, RuleAction.RuleId, RuleAction.ActionId);
                return (RuleAction)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteLevel", ex);
            }
        }

        /// <summary>
        /// crea un RuleAction
        /// </summary>
        /// <param name="RuleAction"></param>
        /// <returns></returns>
        public static void DeleteRuleAction(RuleAction RuleAction)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(RuleAction);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Delete RuleAction", ex);
            }

        }

        /// <summary>
        /// obtiene un alista de Models.Concept a partir de ruleBaseId
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <returns></returns>
        public static List<Models.Concept> GetActionConcept(int ruleBaseId)
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
                throw new BusinessException("Error Obtener GetActionConcept", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de RuleAction a partir de ruleBaseId, ruleId
        /// </summary>
        /// <param name="ruleBaseId"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static BusinessCollection ListRuleAction(int ruleBaseId, int? ruleId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                if (ruleId == null)
                {
                    filter.PropertyEquals(Entities.RuleAction.Properties.RuleBaseId, ruleBaseId);
                }
                else
                {
                    filter.PropertyEquals(Entities.RuleAction.Properties.RuleBaseId, ruleBaseId)
                        .And()
                        .PropertyEquals(Entities.RuleAction.Properties.RuleId, ruleId);
                }


                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(
                    typeof(RuleAction), filter.GetPredicate(), new string[] { Entities.RuleAction.Properties.OrderNum }));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRuleAction", ex);
            }

        }

       
    }
}
