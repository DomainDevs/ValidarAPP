using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        /// <summary>
        /// crea un Rule a partir de un Models.Rule
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static Rule CreateRule(Models.Rule rule)
        {
            return new Entities.Rule(rule.RuleBaseId, rule.RuleId)
            {
                RuleBaseId = rule.RuleBaseId,
                RuleId = rule.RuleId,
                Order = rule.OrderNumber
            };
        }

        /// <summary>
        /// crea un RuleBase a partir de un Models.RuleBase
        /// </summary>
        /// <param name="ruleBase"></param>
        /// <returns></returns>
        public static RuleBase CreateRuleBase(Models.RuleBase ruleBase)
        {
            return new RuleBase(ruleBase.RuleBaseId)
            {
                CurrentFrom = ruleBase.CurrentFrom,
                Description = ruleBase.Description,
                LevelId = ruleBase.LevelId,
                PackageId = ruleBase.PackageId,
                RuleEnumerator = ruleBase.RuleEnumerator,
                IsPublished = ruleBase.IsPublished
            };
        }


        /// <summary>
        /// crea un RuleCondition a partir de un Models.RuleCondition
        /// </summary>
        /// <param name="RuleCondition"></param>
        /// <returns></returns>
        public static RuleCondition CreateRuleCondition(Models.RuleCondition RuleCondition)
        {
            return new RuleCondition(RuleCondition.RuleBaseId, RuleCondition.RuleId, RuleCondition.ConditionId)
            {
                RuleBaseId = RuleCondition.RuleBaseId,
                RuleId = RuleCondition.RuleId,
                ConditionId = RuleCondition.ConditionId,
                EntityId = RuleCondition.EntityId,
                ConceptId = RuleCondition.ConceptId,
                ComparatorCode = RuleCondition.ComparatorCode,
                RuleValueTypeCode = RuleCondition.RuleValueTypeCode,
                CondValue = RuleCondition.CondValue,
                OrderNum = RuleCondition.OrderNumber
            };
        }

        /// <summary>
        /// crea un RuleAction a partir de un Models.RuleAction
        /// </summary>
        /// <param name="RuleAction"></param>
        /// <returns></returns>
        public static RuleAction CreateRuleAction(Models.RuleAction RuleAction)
        {
            return new RuleAction(RuleAction.RuleBaseId, RuleAction.RuleId, RuleAction.ActionId)
            {
                RuleBaseId = RuleAction.RuleBaseId,
                RuleId = RuleAction.RuleId,
                ActionId = RuleAction.ActionId,
                EntityId = RuleAction.EntityId,
                ConceptId = RuleAction.ConceptId,
                OperatorCode = RuleAction.OperationCode,
                ValueTypeCode = RuleAction.ValueTypeCode,
                ActionValue = RuleAction.ActionVal,
                OrderNum = RuleAction.OrderNumber
            };
        }

        /// <summary>
        /// crea un Concept a partir de un Models.Concept 
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static SCREN.Concept CreateConcept(Models.Concept concept)
        {
            SCREN.Concept c = new SCREN.Concept()
            {
                EntityId = concept.EntityId,
                Description = concept.Description,
                ConceptName = concept.Description,
                ConceptTypeCode = (int)concept.ConceptTypeCode,
                ConceptControlCode = (int)concept.ConceptControlCode,
                IsNullable = concept.IsNull,
                IsPersistible = concept.IsPersistible,
                IsStatic = concept.IsStatic,
                IsReadOnly = false,
                IsVisible = concept.IsVisible
            };
            return c;
        }
        /// <summary>
        /// crea un Concept a partir de un Models.Concept 
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static SCREN.Concept CreateConceptEdit(Models.Concept concept)
        {
            SCREN.Concept c = new SCREN.Concept()
            {
                ConceptId = concept.ConceptId,
                EntityId = concept.EntityId,
                Description = concept.Description,
                ConceptName = concept.Description,
                ConceptTypeCode = (int)concept.ConceptTypeCode,
                ConceptControlCode = (int)concept.ConceptControlCode,
                IsNullable = concept.IsNull,
                IsPersistible = concept.IsPersistible,
                IsStatic = concept.IsStatic,
                IsReadOnly = false,
                IsVisible = concept.IsVisible
            };
            return c;
        }

        #region Node
        /// <summary>
        /// crea un Node a partir de un Models.Node
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static Node CreateNode(Models.Node Node)
        {
            Node p = new Node(Node.NodeId, Node.ScriptId)
            {
                ScriptId = Node.ScriptId,
                NodeId = Node.NodeId,
            };

            return p;
        }

        #endregion

        #region ListEntity
        /// <summary>
        /// crea un ListEntity a partir de un Models.ListEntity
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        public static ListEntity CreateListEntity(Models.ListEntity listEntity)
        {
            return new ListEntity
            {
                Description = listEntity.Description,
                ListValueAt = listEntity.ListEntityAt
            };
        }


        /// <summary>
        /// crea un ListEntityValue a partir de un Models.ListEntityValue 
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static SCREN.ListEntityValue CreateListEntityValue(Models.ListEntityValue listEntityValue)
        {
            return new SCREN.ListEntityValue(listEntityValue.ListValueCode, listEntityValue.ListEntityCode)
            {
                ListValueCode = listEntityValue.ListValueCode,
                ListValue = listEntityValue.ListValue,
                ListEntityCode = listEntityValue.ListEntityCode
            };
        }
        #endregion

        #region RangeEntity
        /// <summary>
        /// crea un RangeEntity a partir de un Models.RangeEntity 
        /// </summary>
        /// <param name="rangeEntity"></param>
        /// <returns></returns>
        public static RangeEntity CreateRangeEntity(Models.RangeEntity rangeEntity)
        {
            return new Entities.RangeEntity
            {
                Description = rangeEntity.Description,
                RangeValueAt = rangeEntity.RangeValueAt
            };
        }

        /// <summary>
        /// crea un RangeEntityValue a partir de un Models.RangeEntityValue
        /// </summary>
        /// <param name="rangeEntityValue"></param>
        /// <returns></returns>
        public static RangeEntityValue CreateRangeEntityValue(Models.RangeEntityValue rangeEntityValue)
        {
            return new Entities.RangeEntityValue(rangeEntityValue.RangeValueCode, rangeEntityValue.RangeEntityCode)
            {
                RangeValueCode = rangeEntityValue.RangeValueCode,
                RangeEntityCode = rangeEntityValue.RangeEntityCode,
                FromValue = rangeEntityValue.FromValue,
                ToValue = rangeEntityValue.ToValue
            };
        }
        #endregion
    }
}
