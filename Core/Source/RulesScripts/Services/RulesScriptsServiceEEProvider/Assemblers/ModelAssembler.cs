using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.DAF;
using System.CodeDom;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using models = Sistran.Core.Application.RulesScriptsServices.Models;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
using APentity = Sistran.Core.Application.AuthorizationPolicies.Entities;
using TP = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers
{
    using AutoMapper;
    using ModelServices.Enums;
    using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;

    public class ModelAssembler
    {
        #region Package
        /// <summary>
        /// crea un Models.Package a apartir de Entities.Package
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static models.Package CreatePackage(Entities.Package package)
        {
            models.Package p = new models.Package
            {
                PackageId = package.PackageId,
                Description = package.Description,
                NameSpace = package.Namespace,
                Disabled = package.Disabled
            };

            return p;
        }

        /// <summary>
        /// crea un Lista de Models.Package a partir de un IList
        /// </summary>
        /// <param name="packages"></param>
        /// <returns></returns>
        public static List<models.Package> CreatePackages(IList packageList)
        {
            List<models.Package> packages = new List<models.Package>();
            foreach (Entities.Package package in packageList)
            {
                packages.Add(CreatePackage(package));
            }
            return packages;
        }

        /// <summary>
        /// crea una lista de Models.Package a partir de BusinessCollection
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<models.Package> CreatePackages2(BusinessCollection businessCollection)
        {
            List<models.Package> packages = new List<models.Package>();
            foreach (Entities.Package field in businessCollection)
            {
                packages.Add(ModelAssembler.CreatePackage(field));
            }
            return packages;
        }
        #endregion

        #region Level
        /// <summary>
        /// crea una lista de Models.Level a partir de Entities.Level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static models.Level CreateLevel(Entities.Level level)
        {
            return new models.Level()
            {
                LevelId = level.LevelId,
                Description = level.Description,
                PackageId = level.PackageId
            };
        }

        /// <summary>
        /// crea una lista de Models.Level a partir de IList
        /// </summary>
        /// <param name="levelList"></param>
        /// <returns></returns>
        public static List<models.Level> CreateLevels(IList levelList)
        {
            List<models.Level> levels = new List<models.Level>();

            foreach (Entities.Level level in levelList)
            {
                levels.Add(CreateLevel(level));
            }

            return levels;
        }
        #endregion

        #region Concept
        /// <summary>
        /// crea una lista de  Models.Concept a partir de SCREN.Concept
        /// </summary>
        /// <param name="concept"></param>
        /// <returns></returns>
        public static models.Concept CreateConcept(SCREN.Concept concept)
        {
            models.Concept c = null;

            switch (concept.ConceptTypeCode)
            {
                case 1: // BasicConcept
                    c = new models.BasicConcept
                    {
                        ConceptId = concept.ConceptId,
                        EntityId = concept.EntityId,
                        ConceptName = concept.ConceptName,
                        Description = concept.Description,
                        ConceptControlCode = (Enums.ConceptControlType)concept.ConceptControlCode,
                        IsNull = concept.IsNullable,
                        IsPersistible = concept.IsPersistible,
                        IsStatic = concept.IsStatic,
                        IsVisible = concept.IsVisible,
                        KeyOrder = concept.KeyOrder,
                        ConceptTypeCode = Enums.ConceptType.Basic,
                    };
                    break;
                case 2:
                    c = new models.RangeConcept
                    {
                        ConceptId = concept.ConceptId,
                        EntityId = concept.EntityId,
                        ConceptName = concept.ConceptName,
                        Description = concept.Description,
                        ConceptControlCode = (Enums.ConceptControlType)concept.ConceptControlCode,
                        IsNull = concept.IsNullable,
                        IsPersistible = concept.IsPersistible,
                        IsStatic = concept.IsStatic,
                        IsVisible = concept.IsVisible,
                        KeyOrder = concept.KeyOrder,
                        ConceptTypeCode = Enums.ConceptType.Range,
                    };
                    break;
                case 3:
                    c = new models.ListConcept
                    {
                        ConceptId = concept.ConceptId,
                        EntityId = concept.EntityId,
                        ConceptName = concept.ConceptName,
                        Description = concept.Description,
                        ConceptControlCode = (Enums.ConceptControlType)concept.ConceptControlCode,
                        IsNull = concept.IsNullable,
                        IsPersistible = concept.IsPersistible,
                        IsStatic = concept.IsStatic,
                        IsVisible = concept.IsVisible,
                        KeyOrder = concept.KeyOrder,
                        ConceptTypeCode = Enums.ConceptType.List
                    };
                    break;
                case 4:
                    c = new models.ReferenceConcept
                    {
                        ConceptId = concept.ConceptId,
                        EntityId = concept.EntityId,
                        ConceptName = concept.ConceptName,
                        Description = concept.Description,
                        ConceptControlCode = (Enums.ConceptControlType)concept.ConceptControlCode,
                        IsNull = concept.IsNullable,
                        IsPersistible = concept.IsPersistible,
                        IsStatic = concept.IsStatic,
                        IsVisible = concept.IsVisible,
                        KeyOrder = concept.KeyOrder,
                        ConceptTypeCode = Enums.ConceptType.Reference
                    };
                    break;
            };

            return c;
        }
        #endregion

        #region DynamicConcept
        /// <summary>
        /// crea un DynamicConcept a partir de KeyValuePair(int, ConceptValueDTO)
        /// </summary>
        /// <param name="dynamicProperty"></param>
        /// <returns></returns>
        public static DynamicConcept CreateDynamicConcept(KeyValuePair<int, ConceptValueDTO> dynamicProperty)
        {
            DynamicConcept dynamicConcept = new DynamicConcept();

            dynamicConcept.Id = dynamicProperty.Key;

            if (dynamicProperty.Value.Value != null)
            {
                dynamicConcept.Value = dynamicProperty.Value.Value;
                dynamicConcept.TypeName = dynamicProperty.Value.Value.GetType().FullName;
            }

            return dynamicConcept;
        }

        /// <summary>
        /// crea una lista de DynamicConcept a partir de IDictionary(int, ConceptValueDTO)
        /// </summary>
        /// <param name="dynamicProperties"></param>
        /// <returns></returns>
        public static List<DynamicConcept> CreateDynamicConcepts(IDictionary<int, ConceptValueDTO> dynamicProperties)
        {
            List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

            foreach (KeyValuePair<int, ConceptValueDTO> field in dynamicProperties)
            {
                dynamicConcepts.Add(ModelAssembler.CreateDynamicConcept(field));
            }

            return dynamicConcepts;
        }

        #endregion

        #region Comparator
        /// <summary>
        /// crea un Models.Comparator a partir de un DTOs.Operator
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static models.Comparator CreateComparator(DTOs.Operator op)
        {
            models.Comparator comparator = new models.Comparator
            {
                ComparatorCode = op.Code,
                Description = op.Description,
                CodeBinaryOperatorType = op.GetOperatorType
            };

            switch (op.Code)
            {
                case (int)CodeBinaryOperatorType.IdentityEquality:
                    comparator.Symbol = "=";
                    break;
                case (int)CodeBinaryOperatorType.LessThan:
                    comparator.Symbol = "<";
                    break;
                case (int)CodeBinaryOperatorType.LessThanOrEqual:
                    comparator.Symbol = "<=";
                    break;
                case (int)CodeBinaryOperatorType.GreaterThan:
                    comparator.Symbol = ">";
                    break;
                case (int)CodeBinaryOperatorType.GreaterThanOrEqual:
                    comparator.Symbol = ">=";
                    break;
                case (int)CodeBinaryOperatorType.IdentityInequality:
                    comparator.Symbol = "!=";
                    break;
            }

            return comparator;
        }

        /// <summary>
        /// crea una lista de Models.Comparator a partir de un BusinessCollection
        /// </summary>
        /// <param name="comparatorList"></param>
        /// <returns></returns>
        public static List<models.Comparator> CreateComparators(BusinessCollection comparatorList)
        {
            List<models.Comparator> Comparators = new List<models.Comparator>();
            foreach (Entities.RuleConditionComparator ComparatorE in comparatorList)
            {
                models.Comparator comparator = new models.Comparator
                {
                    ComparatorCode = ComparatorE.ComparatorCode,
                    Description = ComparatorE.Description,
                    Symbol = ComparatorE.Symbol
                };

                Comparators.Add(comparator);
            }
            return Comparators;
        }
        #endregion

        #region Rule
        /// <summary>
        /// crea un  Models.Rule a partir de Entities.Rule 
        /// </summary>
        /// <param name="rule"></param>
        /// <returns></returns>
        public static models.Rule CreateRule(Entities.Rule rule)
        {
            models.Rule r = new models.Rule
            {
                RuleBaseId = rule.RuleBaseId,
                RuleId = rule.RuleId,
                OrderNumber = rule.Order

            };
            return r;
        }

        /// <summary>
        /// crea una lista de Models.Rule a partir de IList
        /// </summary>
        /// <param name="levelList"></param>
        /// <returns></returns>
        public static List<models.Rule> CreateRules(IList RuleList)
        {
            List<models.Rule> rules = new List<models.Rule>();

            foreach (Entities.Rule rule in RuleList)
            {
                rules.Add(CreateRule(rule));
            }

            return rules;
        }
        #endregion

        #region RuleAction
        /// <summary>
        /// crea un Models.RuleAction  a partir de un Entities.RuleAction
        /// </summary>
        /// <param name="RuleAction"></param>
        /// <returns></returns>
        public static models.RuleAction CreateRuleAction(Entities.RuleAction RuleAction)
        {
            models.RuleAction ra = new models.RuleAction
            {
                RuleBaseId = RuleAction.RuleBaseId,
                RuleId = RuleAction.RuleId,
                ActionId = RuleAction.ActionId,
                EntityId = RuleAction.EntityId,
                ConceptId = RuleAction.ConceptId,
                OperationCode = RuleAction.OperatorCode,
                ValueTypeCode = RuleAction.ValueTypeCode,
                ActionVal = RuleAction.ActionValue,
                OrderNumber = RuleAction.OrderNum
            };
            return ra;
        }

        /// <summary>
        /// crea una lista de Models.RuleAction a partir de IList
        /// </summary>
        /// <param name="levelList"></param>
        /// <returns></returns>
        public static List<models.RuleAction> CreateRuleActions(IList ListRuleAction)
        {
            List<models.RuleAction> RuleRuleActions = new List<models.RuleAction>();

            foreach (Entities.RuleAction RuleAction in ListRuleAction)
            {
                RuleRuleActions.Add(CreateRuleAction(RuleAction));
            }

            return RuleRuleActions;
        }
        #endregion

        #region RuleCondition
        /// <summary>
        /// crea una lista de Models.RuleCondition a partir de una Entities.RuleCondition
        /// </summary>
        /// <param name="RuleCondition"></param>
        /// <returns></returns>
        public static models.RuleCondition CreateRuleCondition(Entities.RuleCondition RuleCondition)
        {
            models.RuleCondition rc = new models.RuleCondition
            {
                RuleBaseId = RuleCondition.RuleBaseId,
                RuleId = RuleCondition.RuleId,
                ConditionId = RuleCondition.ConditionId,
                EntityId = RuleCondition.EntityId,
                ConceptId = RuleCondition.ConceptId,
                ComparatorCode = RuleCondition.ComparatorCode,
                RuleValueTypeCode = RuleCondition.RuleValueTypeCode,
                CondValue = RuleCondition.CondValue,
                OrderNumber = RuleCondition.OrderNum

            };
            return rc;
        }

        /// <summary>
        /// crea una lista de Models.RuleCondition  a partir de IList
        /// </summary>
        /// <param name="levelList"></param>
        /// <returns></returns>
        public static List<models.RuleCondition> CreateRuleConditions(IList ListRuleCondition)
        {
            List<models.RuleCondition> RuleConditions = new List<models.RuleCondition>();

            foreach (Entities.RuleCondition RuleCondition in ListRuleCondition)
            {
                RuleConditions.Add(CreateRuleCondition(RuleCondition));
            }

            return RuleConditions;
        }
        #endregion

        #region RuleBase
        /// <summary>
        /// crea una  Models.RuleBase a partir de Entities.RuleBase
        /// </summary>
        /// <param name="ruleBase"></param>
        /// <returns></returns>
        public static models.RuleBase CreateRuleBase(Entities.RuleBase ruleBase)
        {
            models.RuleBase rb = new models.RuleBase
            {
                RuleBaseId = ruleBase.RuleBaseId,
                Description = ruleBase.Description,
                LevelId = ruleBase.LevelId,
                PackageId = ruleBase.PackageId,
                CurrentFrom = ruleBase.CurrentFrom,
                IsPublished = ruleBase.IsPublished,
                RuleBaseVersion = ruleBase.RuleBaseVersion,
                RuleEnumerator = ruleBase.RuleEnumerator

            };
            return rb;
        }
        #endregion

        #region ListEntityValue
        /// <summary>
        /// crea una Models.ListEntityValue a partir de Entities.ListEntityValue
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static models.ListEntityValue CreateListEntityValue(SCREN.ListEntityValue listEntityValue)
        {
            return new models.ListEntityValue
            {
                ListEntityCode = listEntityValue.ListEntityCode,
                ListValueCode = listEntityValue.ListValueCode,
                ListValue = listEntityValue.ListValue,
                StatusTypeService = StatusTypeService.Original
            };
        }

        /// <summary>
        /// crea una lista de Models.ListEntityValue a partir de BusinessCollection
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<models.ListEntityValue> CreateListEntityValues(BusinessCollection businessCollection)
        {
            List<models.ListEntityValue> listEntityValues = new List<models.ListEntityValue>();

            foreach (SCREN.ListEntityValue field in businessCollection)
            {
                listEntityValues.Add(ModelAssembler.CreateListEntityValue(field));
            }

            return listEntityValues;
        }

        #endregion

        #region ListEntity
        /// <summary>
        /// crea una lista de  Models.ListEntity  a partir de Entities.ListEntity
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        public static models.ListEntity CreateListEntity(Entities.ListEntity listEntity)
        {
            return new models.ListEntity
            {
                ListEntityCode = listEntity.ListEntityCode,
                Description = listEntity.Description,
                ListEntityAt = listEntity.ListValueAt,
                StatusTypeService = StatusTypeService.Original,
            };
        }

        /// <summary>
        /// crea una lsita de Models.ListEntity a partir de BusinessCollection
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<models.ListEntity> CreateListEntities(BusinessCollection businessCollection)
        {
            List<models.ListEntity> listEntity = new List<models.ListEntity>();

            foreach (Entities.ListEntity field in businessCollection)
            {
                listEntity.Add(ModelAssembler.CreateListEntity(field));
            }
            return listEntity;
        }
        #endregion

        #region RangeEntity
        /// <summary>
        /// crea un Models.RangeEntity a partir de Entities.RangeEntity
        /// </summary>
        /// <param name="rangeEntity"></param>
        /// <returns></returns>
        public static models.RangeEntity CreateRangeEntity(Entities.RangeEntity rangeEntity)
        {
            return new models.RangeEntity
            {
                RangeEntityCode = rangeEntity.RangeEntityCode,
                Description = rangeEntity.Description,
                RangeValueAt = rangeEntity.RangeValueAt,
                StatusTypeService = StatusTypeService.Original
            };
        }

        /// <summary>
        /// crea una lista de  Models.RangeEntity  a partir de BusinessCollection
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<models.RangeEntity> CreateRangeEntities(BusinessCollection businessCollection)
        {
            List<models.RangeEntity> rangeEntity = new List<models.RangeEntity>();

            foreach (Entities.RangeEntity field in businessCollection)
            {
                rangeEntity.Add(ModelAssembler.CreateRangeEntity(field));
            }
            return rangeEntity;
        }
        #endregion

        #region RangeEntityValue
        /// <summary>
        /// crea una Models.RangeEntityValue a partir de Entities.RangeEntityValue
        /// </summary>
        /// <param name="rangeEntityValues"></param>
        /// <returns></returns>
        public static models.RangeEntityValue CreateRangeEntityValue(Entities.RangeEntityValue rangeEntityValues)
        {
            return new models.RangeEntityValue
            {
                RangeEntityCode = rangeEntityValues.RangeEntityCode,
                RangeValueCode = rangeEntityValues.RangeValueCode,
                FromValue = rangeEntityValues.FromValue,
                ToValue = rangeEntityValues.ToValue,
                StatusTypeService = StatusTypeService.Original
            };
        }

        /// <summary>
        /// crea una lista de Models.RangeEntityValue a partir de BusinessCollection
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<models.RangeEntityValue> CreateRangeEntityValues(BusinessCollection businessCollection)
        {
            List<models.RangeEntityValue> rangeEntityValues = new List<models.RangeEntityValue>();

            foreach (Entities.RangeEntityValue field in businessCollection)
            {
                rangeEntityValues.Add(ModelAssembler.CreateRangeEntityValue(field));
            }

            return rangeEntityValues;
        }

        #endregion

        #region Script
        /// <summary>
        /// crea un  Models.Script  a aprtir de un Entities.Script
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        public static models.Script CreateScript(Entities.Script script)
        {
            models.Script p = new models.Script
            {
                ScriptId = script.ScriptId,
                Description = script.Description,
                LevelId = script.LevelId,
                NodeId = script.NodeId,
                PackageId = script.PackageId
            };

            return p;
        }

        /// <summary>
        /// crea una lista de  Models.Script a partir de BusinessCollection
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<models.Script> CreateScripts(BusinessCollection scriptList)
        {
            IMapper imapper = AutoMapperAssembler.CreateMapScript();
            List<Entities.Script> scripts = scriptList.Cast<Entities.Script>().ToList();
            return imapper.Map<List<Entities.Script>, List<models.Script>>(scripts);         
        }
        #endregion

        #region Question
        /// <summary>
        /// crea un Models.Question a aprtir de Entities.Question
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models.Question CreateQuestion(Entities.Question Question)
        {
            models.Question p = new models.Question
            {
                Description = Question.Description,
                ConceptId = Question.ConceptId,
                EntityId = Question.EntityId,
                QuestionId = Question.QuestionId
            };

            return p;
        }

        /// <summary>
        /// crea una lsita de Models.Question a partir de BusinessCollection
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<models.Question> CreateQuestions(BusinessCollection QuestionList)
        {
            IMapper imapper = AutoMapperAssembler.CreateMapQuestion();
            List<Entities.Question> questions = QuestionList.Cast<Entities.Question>().ToList();
            return imapper.Map<List<Entities.Question>, List<models.Question>>(questions);          
        }
        #endregion

        #region Node
        /// <summary>
        /// crea un models.Node a partir de un Entities.Node
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models.Node CreateNode(Entities.Node Node)
        {
            models.Node p = new models.Node
            {
                ScriptId = Node.ScriptId,
                NodeId = Node.NodeId,
            };

            return p;
        }

        /// <summary>
        /// Creates the nodes.
        /// </summary>
        /// <param name="NodeList">The node list.</param>
        /// <returns></returns>
        public static List<models.Node> CreateNodes(BusinessCollection NodeList)
        {

            IMapper imapper = AutoMapperAssembler.CreateMapNode();
            List<Entities.Node> nodes = NodeList.Cast<Entities.Node>().ToList();
            return imapper.Map<List<Entities.Node>, List<models.Node>>(nodes);
        }
        #endregion

        #region NodeQuestion
        /// <summary>
        /// crea un  models.NodeQuestion  a partir de Entities.NodeQuestion
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models.NodeQuestion CreateNodeQuestion(Entities.NodeQuestion NodeQuestion)
        {
            models.NodeQuestion p = new models.NodeQuestion
            {
                ScriptId = NodeQuestion.ScriptId,
                NodeId = NodeQuestion.NodeId,
                QuestionId = NodeQuestion.QuestionId,
                OrderNum = NodeQuestion.OrderNum
            };

            return p;
        }

        /// <summary>
        /// crea una lsita de models.NodeQuestion a apartir de BusinessCollection
        /// </summary>
        /// <param name="NodeQuestionList">The node question list.</param>
        /// <returns></returns>
        public static List<models.NodeQuestion> CreateNodeQuestions(BusinessCollection NodeQuestionList)
        {
            if (NodeQuestionList.Count > 0)
            {
                IMapper imapper = AutoMapperAssembler.CreateMapNodeQuestion();
                List<Entities.NodeQuestion> nodesQuestions = NodeQuestionList.Cast<Entities.NodeQuestion>().ToList();
                return imapper.Map<List<Entities.NodeQuestion>, List<models.NodeQuestion>>(nodesQuestions);                
            }
            else
            {
                return new List<models.NodeQuestion>();
            }
        }
        #endregion

        #region Edge
        /// <summary>
        /// crea un models.Edge a partir de Entities.Edge
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models.Edge CreateEdge(Entities.Edge Edge)
        {
            models.Edge p = new models.Edge
            {
                EdgeId = Edge.EdgeId,
                NodeId = Edge.NodeId,
                QuestionId = Edge.QuestionId,
                ScriptId = Edge.ScriptId,
                NextNodeId = Edge.NextNodeId
            };

            return p;
        }

        /// <summary>
        /// crea uan lista de models.Edge a partir de BusinessCollection
        /// </summary>
        /// <param name="EdgeList">The edge list.</param>
        /// <returns></returns>
        public static List<models.Edge> CreateEdges(BusinessCollection EdgeList)
        {
            if (EdgeList.Count > 0)
            {
                IMapper imapper = AutoMapperAssembler.CreateMapEdge();
                List<Entities.Edge> edges = EdgeList.Cast<Entities.Edge>().ToList();
                return imapper.Map<List<Entities.Edge>, List<models.Edge>>(edges);
            }
            else
            {
                return new List<models.Edge>();
            }          
        }
        #endregion

        #region EdgeAnswer
        /// <summary>
        /// crea un  models.EdgeAnswer a partir de Entities.EdgeAnswer
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models.EdgeAnswer CreateEdgeAnswer(Entities.EdgeAnswer EdgeAnswer)
        {
            models.EdgeAnswer p = new models.EdgeAnswer
            {
                EdgeId = EdgeAnswer.EdgeId,
                ConceptId = EdgeAnswer.ConceptId,
                EntityId = EdgeAnswer.EntityId,
                ValueCode = EdgeAnswer.ValueCode
            };

            return p;
        }

        /// <summary>
        /// crea una lista de models.EdgeAnswer a partir de un BusinessCollection
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<models.EdgeAnswer> CreateEdgeAnswers(BusinessCollection EdgeAnswerList)
        {
            List<models.EdgeAnswer> EdgeAnswers = new List<models.EdgeAnswer>();
            foreach (Entities.EdgeAnswer EdgeAnswer in EdgeAnswerList)
            {
                EdgeAnswers.Add(CreateEdgeAnswer(EdgeAnswer));
            }
            return EdgeAnswers;
        }
        #endregion

        #region RuleSet
        /// <summary>
        /// crea un models.RuleSet  a aprtir de un Entities.RuleSet 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models.RuleSet CreateRuleSet(Entities.RuleSet ruleSet)
        {

            models.RuleSet RuleSetEnt = new models.RuleSet
            {
                RuleSetId = ruleSet.RuleSetId,
                Description = ruleSet.Description,
                LevelId = ruleSet.LevelId,
                IsEvent = ruleSet.IsEvent,
            };

            return RuleSetEnt;
        }

        /// <summary>
        /// crea una lista de models.RuleSet a partir de un BusinessCollection
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static List<models.RuleSet> CreateRuleSets(BusinessCollection ruleSetList)
        {
            ConcurrentBag<models.RuleSet> RuleSets = new ConcurrentBag<models.RuleSet>();
            TP.Parallel.ForEach(ruleSetList.Cast<Entities.RuleSet>().ToList(), ruleSet =>
            {
                RuleSets.Add(CreateRuleSet(ruleSet));
            });
            return RuleSets.ToList();
        }
        /// <summary>
        /// crea una lista de models.RuleSet a partir de un BusinessCollection
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models._RuleSet CreateRuleSet(Sistran.Core.Framework.Rules.Integration.RuleSet ruleSet)
        {
            models._RuleSet _ruleSet = new models._RuleSet
            {
                RuleSetId = ruleSet.Id,
                Description = ruleSet.Name
            };
            ConcurrentBag<models._Rule> Rules = new ConcurrentBag<models._Rule>();
            TP.Parallel.ForEach(ruleSet.Rules, rule =>
            {
                Rules.Add(CreateRuleSetModel(rule));
            });
            _ruleSet.Rules = Rules.ToList();
            return _ruleSet;
        }
        /// <summary>
        /// crea un models.RuleSet  a aprtir de un Entities.RuleSet 
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public static models._Rule CreateRuleSetModel(Sistran.Core.Framework.Rules.Integration.Rule rule)
        {

            models._Rule _rule = new models._Rule
            {
                Description = rule.Name
            };

            return _rule;
        }

        #endregion

        #region ConceptType
        /// <summary>
        /// crea un  models.ConceptType  a partir de un Entities.ConceptType
        /// </summary>
        /// <param name="ConceptType"></param>
        /// <returns></returns>
        public static models.ConceptType CreateConceptType(Entities.ConceptType ConceptType)
        {
            return new models.ConceptType()
            {
                ConceptTypeId = ConceptType.ConceptTypeCode,
                Description = ConceptType.Description
            };
        }
        #endregion

        #region BasicType
        /// <summary>
        /// crea un models.BasicType  a aprtir de un Entities.BasicType
        /// </summary>
        /// <param name="BasicType"></param>
        /// <returns></returns>
        public static models.BasicType CreateBasicType(Entities.BasicType BasicType)
        {
            return new models.BasicType()
            {
                BasicTypeId = BasicType.BasicTypeCode,
                Description = BasicType.Description
            };
        }
        #endregion
        #region Entity

        public static Entity CreateEntity(PARAMEN.Entity entityEntity)
        {
            return new Entity()
            {
                EntityId = entityEntity.EntityId,
                Description = entityEntity.Description,
                EntityName = entityEntity.EntityName,
                LevelId = entityEntity.LevelId,
                PackageId = entityEntity.PackageId,
                ConfigFile = entityEntity.ConfigFile,
                PropertySearch = entityEntity.PropertySearch,
                BusinessView = entityEntity.BusinessView
            };
        }

        #endregion

        #region RuleProcessRuleSet
        public static models.RuleProcessRuleSet CreateRuleProcessRuleSet(RuleProcessRuleset ruleProcessRuleSetEntity)
        {
            return new RuleProcessRuleSet
            {
              Id = ruleProcessRuleSetEntity.RuleProcessRulesetId ,
              EntityId = ruleProcessRuleSetEntity.EntityId ,
              ConceptId = ruleProcessRuleSetEntity.ConceptId ,
              PosRuleSet = ruleProcessRuleSetEntity.PosRulesetId
            };
                
        }
        #endregion

    }
}

