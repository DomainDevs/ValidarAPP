using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using MCommon = Sistran.Core.Application.CommonService.Models;
using EmRules = Sistran.Core.Application.RulesScriptsServices.Enums;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public static class RuleHelper
    {
        public static MRules._RuleSet GetRuleSet(string ruleSetStr)
        {
            var rulesetDynamic = JsonConvert.DeserializeObject<dynamic>(ruleSetStr);

            var rulesetObj = new MRules._RuleSet
            {
                Type = (EmRules.RuleBaseType)Enum.Parse(typeof(EmRules.RuleBaseType), rulesetDynamic.Type.ToString()),
                Rules = FillRules(rulesetDynamic.Rules)
            };

            if (rulesetDynamic.RuleSetId != null)
            {
                rulesetObj.RuleSetId = Convert.ToInt32(rulesetDynamic.RuleSetId.Value);
            }

            if (rulesetDynamic.Description != null)
            {
                rulesetObj.Description = rulesetDynamic.Description.Value;
            }
            if (rulesetDynamic.Level != null)
            {
                rulesetObj.Level = new MRules._Level
                {
                    LevelId = Convert.ToInt32(rulesetDynamic.Level.LevelId.Value)
                };
            }
            if (rulesetDynamic.Package != null)
            {
                rulesetObj.Package = new MRules._Package
                {
                    PackageId = Convert.ToInt32(rulesetDynamic.Package.PackageId.Value)
                };
            }
            if (rulesetDynamic.IsEvent != null)
            {
                rulesetObj.IsEvent = rulesetDynamic.IsEvent.Value;
            }
            if (rulesetDynamic.Active != null)
            {
                rulesetObj.Active = rulesetDynamic.Active;
            }
            if (rulesetDynamic.ActiveType != null || rulesetDynamic.ActiveType > 0)
            {
                rulesetObj.ActiveType = rulesetDynamic.ActiveType > 0 ? (Application.Utilities.Enums.ActiveRuleSetType)(rulesetDynamic.ActiveType) : (Application.Utilities.Enums.ActiveRuleSetType?)(null);
            }
            return rulesetObj;
        }

        public static List<MRules._Rule> FillRules(dynamic rules)
        {
            List<MRules._Rule> listRules = new List<MRules._Rule>();

            foreach (var ruleDynamic in rules)
            {
                var rule = new MRules._Rule
                {
                    RuleId = ruleDynamic.RuleId,
                    Description = ruleDynamic.Description,
                    Parameters = new List<MRules._Parameter>(),
                    Conditions = FillConditions(ruleDynamic.Conditions),
                    Actions = FillActions(ruleDynamic.Actions)
                };

                listRules.Add(rule);
            }

            return listRules;
        }

        private static List<MRules._Condition> FillConditions(dynamic conditions)
        {
            List<MRules._Condition> listConditions = new List<MRules._Condition>();

            foreach (var conditionDynamic in conditions)
            {
                var condition = new MRules._Condition
                {

                    ComparatorType = Enum.Parse(typeof(EmRules.ComparatorType), conditionDynamic.ComparatorType.Value.ToString()),
                    Concept = FillConcept(conditionDynamic.Concept)
                };

                if (conditionDynamic.Comparator != null)
                {
                    condition.Comparator = new MRules._Comparator
                    {
                        Description = conditionDynamic.Comparator.Description,
                        Operator = conditionDynamic.Comparator.Operator,
                        Symbol = conditionDynamic.Comparator.Symbol
                    };
                }

                if (conditionDynamic.Expression == null)
                {
                    condition.Expression = null;
                }
                else
                {
                    switch (condition.ComparatorType)
                    {
                        case EmRules.ComparatorType.ConstantValue:
                            {
                                switch ((EmRules.ConceptType)condition.Concept.ConceptType)
                                {
                                    case EmRules.ConceptType.Basic:
                                        condition.Expression = conditionDynamic.Expression.Value;
                                        break;
                                    case EmRules.ConceptType.Range:
                                        condition.Expression = new MRules._RangeEntityValue
                                        {
                                            FromValue = conditionDynamic.Expression.FromValue,
                                            RangeValueCode = conditionDynamic.Expression.RangeValueCode,
                                            ToValue = conditionDynamic.Expression.ToValue
                                        };
                                        break;
                                    case EmRules.ConceptType.List:
                                        condition.Expression = new MRules._ListEntityValue
                                        {
                                            ListValue = conditionDynamic.Expression.ListValue,
                                            ListValueCode = conditionDynamic.Expression.ListValueCode
                                        };
                                        break;
                                    case EmRules.ConceptType.Reference:
                                        condition.Expression = new MRules._ReferenceValue
                                        {
                                            Id = conditionDynamic.Expression.Id,
                                            Description = conditionDynamic.Expression.Description
                                        };
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;
                            }
                        case EmRules.ComparatorType.ConceptValue:
                            condition.Expression = FillConcept(conditionDynamic.Expression);
                            break;
                        case EmRules.ComparatorType.ExpressionValue:
                        case EmRules.ComparatorType.TemporalyValue:
                            condition.Expression = conditionDynamic.Expression.Value;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                listConditions.Add(condition);
            }

            return listConditions;
        }

        private static List<MRules._Action> FillActions(dynamic action)
        {
            List<MRules._Action> listActions = new List<MRules._Action>();


            foreach (var actionDynamic in action)
            {
                switch ((EmRules.AssignType)actionDynamic.AssignType)
                {
                    case EmRules.AssignType.ConceptAssign:
                        var actionConcept = new MRules._ActionConcept
                        {
                            Concept = FillConcept(actionDynamic.Concept),
                            ComparatorType = Enum.Parse(typeof(EmRules.ComparatorType), actionDynamic.ComparatorType.Value.ToString()),

                            AssignType = Enum.Parse(typeof(EmRules.AssignType), actionDynamic.AssignType.Value.ToString()),
                        };

                        if (actionDynamic.ArithmeticOperator != null)
                        {
                            actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                            {
                                ArithmeticOperatorType = Enum.Parse(typeof(EmRules.ArithmeticOperatorType),
                                    actionDynamic.ArithmeticOperator.ArithmeticOperatorType.Value.ToString()),
                                Symbol = actionDynamic.ArithmeticOperator.Symbol
                            };
                        }


                        switch (actionConcept.ComparatorType)
                        {
                            case EmRules.ComparatorType.ConstantValue:
                                {
                                    switch ((EmRules.ConceptType)actionConcept.Concept.ConceptType)
                                    {
                                        case EmRules.ConceptType.Basic:
                                            actionConcept.Expression = actionDynamic.Expression.Value;
                                            break;
                                        case EmRules.ConceptType.Range:
                                            actionConcept.Expression = null;
                                            if (actionDynamic.Expression != null)
                                            {
                                                actionConcept.Expression = new MRules._RangeEntityValue
                                                {
                                                    FromValue = actionDynamic.Expression.FromValue,
                                                    RangeValueCode = actionDynamic.Expression.RangeValueCode,
                                                    ToValue = actionDynamic.Expression.ToValue
                                                };
                                            }

                                            break;
                                        case EmRules.ConceptType.List:
                                            actionConcept.Expression = null;
                                            if (actionDynamic.Expression != null)
                                            {
                                                actionConcept.Expression = new MRules._ListEntityValue
                                                {
                                                    ListValue = actionDynamic.Expression.ListValue,
                                                    ListValueCode = actionDynamic.Expression.ListValueCode
                                                };
                                            }
                                            break;
                                        case EmRules.ConceptType.Reference:
                                            actionConcept.Expression = null;
                                            if (actionDynamic.Expression != null)
                                            {
                                                actionConcept.Expression = new MRules._ReferenceValue
                                                {
                                                    Id = actionDynamic.Expression.Id,
                                                    Description = actionDynamic.Expression.Description
                                                };
                                            }
                                            break;
                                        default:
                                            throw new ArgumentOutOfRangeException();
                                    }
                                    break;
                                }
                            case EmRules.ComparatorType.ConceptValue:
                                actionConcept.Expression = FillConcept(actionDynamic.Expression);
                                break;
                            case EmRules.ComparatorType.ExpressionValue:
                            case EmRules.ComparatorType.TemporalyValue:
                                actionConcept.Expression = actionDynamic.Expression.Value;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                        listActions.Add(actionConcept);
                        break;

                    case EmRules.AssignType.InvokeAssign:
                        MRules._ActionInvoke actionInvoke = new MRules._ActionInvoke
                        {
                            AssignType = Enum.Parse(typeof(EmRules.AssignType), actionDynamic.AssignType.Value.ToString()),
                            InvokeType = Enum.Parse(typeof(EmRules.InvokeType), actionDynamic.InvokeType.Value.ToString())
                        };

                        switch (actionInvoke.InvokeType)
                        {
                            case EmRules.InvokeType.MessageInvoke:
                                actionInvoke.Expression = actionDynamic.Expression.Value;
                                break;

                            case EmRules.InvokeType.RuleSetInvoke:
                                actionInvoke.Expression = new MRules._RuleSet
                                {
                                    RuleSetId = actionDynamic.Expression.RuleSetId,
                                    Type = actionDynamic.Expression.Type,
                                    Description = actionDynamic.Expression.Description,
                                    Package = new MRules._Package { PackageId = actionDynamic.Expression.Package.PackageId },
                                    Level = new MRules._Level { LevelId = actionDynamic.Expression.Level.LevelId },
                                    IsEvent = actionDynamic.Expression.IsEvent
                                };
                                break;

                            case EmRules.InvokeType.FunctionInvoke:
                                actionInvoke.Expression = new MRules._RuleFunction
                                {
                                    RuleFunctionId = actionDynamic.Expression.RuleFunctionId,
                                    FunctionName = actionDynamic.Expression.FunctionName,
                                    Description = actionDynamic.Expression.Description
                                };
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }




                        listActions.Add(actionInvoke);
                        break;

                    case EmRules.AssignType.TemporalAssign:
                        var actiontemp = new MRules._ActionValueTemp
                        {
                            AssignType = Enum.Parse(typeof(EmRules.AssignType), actionDynamic.AssignType.Value.ToString()),
                            ComparatorType = Enum.Parse(typeof(EmRules.ComparatorType), actionDynamic.ComparatorType.Value.ToString()),
                            ArithmeticOperator = new MRules._ArithmeticOperator
                            {
                                ArithmeticOperatorType = Enum.Parse(typeof(EmRules.ArithmeticOperatorType), actionDynamic.ArithmeticOperator.ArithmeticOperatorType.Value.ToString())
                            },
                            ValueTemp = actionDynamic.ValueTemp
                        };

                        switch (actiontemp.ComparatorType)
                        {
                            case EmRules.ComparatorType.ConceptValue:
                                actiontemp.Expression = FillConcept(actionDynamic.Expression);
                                break;
                            case EmRules.ComparatorType.ExpressionValue:
                            case EmRules.ComparatorType.TemporalyValue:
                            case EmRules.ComparatorType.ConstantValue:
                                actiontemp.Expression = actionDynamic.Expression.Value;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        listActions.Add(actiontemp);
                        break;
                }
            }

            return listActions;
        }
        private static MRules._Concept FillConcept(dynamic concept)
        {
            MRules._Concept conceptTmp = new MRules._Concept();

            EmRules.ConceptType conceptType = (EmRules.ConceptType)Enum.Parse(typeof(EmRules.ConceptType), concept.ConceptType.Value.ToString());

            switch (conceptType)
            {             
                case EmRules.ConceptType.Basic:
                    if (concept.BasicType != null)
                    {
                        conceptTmp = new MRules._BasicConcept
                        {
                            BasicType = Enum.Parse(typeof(EmRules.BasicType), concept.BasicType.Value.ToString())
                        };
                    }
                    else
                    {
                        conceptTmp = new MRules._BasicConcept();
                    }
                    break;
                case EmRules.ConceptType.Range:
                    conceptTmp = new MRules._RangeConcept();
                    break;
                case EmRules.ConceptType.List:
                    conceptTmp = new MRules._ListConcept();
                    break;
                case EmRules.ConceptType.Reference:
                    conceptTmp = new MRules._ReferenceConcept();
                    break;
            }


            conceptTmp.ConceptId = concept.ConceptId;
            conceptTmp.Entity = new MRules.Entity
            {
                EntityId = concept.Entity.EntityId
            };
            conceptTmp.Description = concept.Description;
            conceptTmp.ConceptName = concept.ConceptName;
            conceptTmp.ConceptType = Enum.Parse(typeof(EmRules.ConceptType), concept.ConceptType.Value.ToString());
            conceptTmp.KeyOrder = concept.KeyOrder;
            conceptTmp.IsStatic = concept.IsStatic;
            conceptTmp.ConceptControlType = Enum.Parse(typeof(EmRules.ConceptControlType), concept.ConceptControlType.Value.ToString());
            conceptTmp.IsReadOnly = concept.IsReadOnly;
            conceptTmp.IsVisible = concept.IsVisible;
            conceptTmp.IsNulleable = concept.IsNulleable;
            conceptTmp.IsPersistible = concept.IsPersistible;

            return conceptTmp;
        }
    }
}