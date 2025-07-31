using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Sistran.Core.Application.AuditServices.EEProvider.Helpers
{
    using Sistran.Core.Application.AuditServices.EEProvider.Resources;
    using Sistran.Core.Application.RulesScriptsServices.Enums;
    using Sistran.Core.Application.RulesScriptsServices.Models;
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class ReadRuleHelper
    {
        public static ConcurrentBag<dynamic> LoadRules(_RuleSet ruleSet)
        {
            var rules = new ConcurrentBag<dynamic>();
            object objLock = new object();
            Parallel.ForEach(ruleSet.Rules.AsParallel().AsSequential(), rule =>
             {
                 dynamic ruleBase;
                 lock (objLock)
                 {
                     ruleBase = new { Rule = Errors.Rule + "  " + rule.Description, Conditions = new List<dynamic>(), Actions = new List<dynamic>() };
                 }
                 Parallel.ForEach(rule.Conditions.AsParallel().AsOrdered(), condition =>
                 {
                     lock (objLock)
                     {
                         ruleBase.Conditions.Add(new { condition = FillCondition(condition) });
                     }
                 });
                 Parallel.ForEach(rule.Actions.AsParallel().AsOrdered(), action =>
                 {
                     if (action as _ActionConcept != null)
                     {
                         lock (objLock)
                         {
                             ruleBase.Actions.Add(new { Action = FillAction((_ActionConcept)action) });
                         }
                     }
                     else if (action as _ActionValueTemp != null)
                     {
                         lock (objLock)
                         {
                             ruleBase.Actions.Add(new { Action = FillActionValueTemp((_ActionValueTemp)action) });
                         }

                     }
                     else if (action as _ActionInvoke != null)
                     {
                         lock (objLock)
                         {
                             ruleBase.Actions.Add(new { Action = FillActionInvoke((_ActionInvoke)action) });
                         }
                     }
                 });
                 rules.Add(ruleBase);
             });           
            return new ConcurrentBag<dynamic>(rules.OrderBy(x => x.Rule).ToList());
        }
        static string FillCondition(_Condition condition)
        {
            StringBuilder StringBuilder = new StringBuilder();
            StringBuilder.AppendFormat("{0}  {1}", Errors.If, condition.Concept.Description.ToUpper());
            StringBuilder.AppendFormat("{0}  {1} ", Errors.Is, condition.Comparator.Description.ToUpper());
            if (condition.Expression == null)
            {
                StringBuilder.AppendFormat(" {0}", Errors.Null);
            }
            else
            {
                switch (condition.ComparatorType)
                {
                    case ComparatorType.ConstantValue:
                        switch (condition.Concept.ConceptType)
                        {
                            case RulesScriptsServices.Enums.ConceptType.Range:
                                StringBuilder.AppendFormat(" {0}", Errors.ValueBetween);
                                StringBuilder.AppendFormat(" {0}  {1} {2}", condition.Expression.FromValue, Errors.And, condition.Expression.ToValue);
                                break;
                            case RulesScriptsServices.Enums.ConceptType.List:
                                StringBuilder.AppendFormat(" {0} ", Errors.TheValue);
                                StringBuilder.AppendFormat(" {0} ", condition.Expression.ListValue);
                                break;
                            case RulesScriptsServices.Enums.ConceptType.Reference:
                                StringBuilder.AppendFormat(" {0}", Errors.TheValue);
                                StringBuilder.AppendFormat(" {0}", condition.Expression.Description.ToUpper());
                                break;
                            case RulesScriptsServices.Enums.ConceptType.Basic:
                                StringBuilder.AppendFormat(" {0}", Errors.TheValue);
                                StringBuilder.AppendFormat(" {0}", condition.Expression);

                                break;
                            default:
                                StringBuilder.AppendFormat("{0}", Errors.TheValue);
                                StringBuilder.AppendFormat("{0}", condition.Expression);

                                break;
                        }
                        break;
                    case ComparatorType.ConceptValue:
                        StringBuilder.AppendFormat("{0}", Errors.ConceptValue);
                        StringBuilder.AppendFormat("{0}", condition.Expression.Description.ToUpper());
                        break;
                    case ComparatorType.ExpressionValue:
                        StringBuilder.AppendFormat("{0}", Errors.ExpressionValue);
                        StringBuilder.AppendFormat("{0}", condition.Expression);

                        break;
                    case ComparatorType.TemporalyValue:
                        StringBuilder.AppendFormat("{0}", Errors.TemporalValue);
                        StringBuilder.AppendFormat("{0}", condition.Expression);
                        break;
                }
            }
            return StringBuilder.ToString();
        }
        static string FillAction(_ActionConcept action)
        {
            StringBuilder StringBuilder = new StringBuilder();
            switch (action.AssignType)
            {
                case AssignType.ConceptAssign:
                    StringBuilder.AppendFormat("{0} ", Errors.ToConcept);
                    StringBuilder.AppendFormat("{0} ", action.Concept.Description.ToUpper());
                    StringBuilder.AppendFormat("{0}", action.ArithmeticOperator.Description.ToUpper());
                    switch (action.ComparatorType)
                    {
                        case ComparatorType.ConstantValue:
                            StringBuilder.AppendFormat("{0}", Errors.TheValue);
                            switch (action.Concept.ConceptType)
                            {
                                case RulesScriptsServices.Enums.ConceptType.Range:
                                    StringBuilder.AppendFormat("{0}", action.Expression.FromValue);
                                    StringBuilder.AppendFormat(" - {0}", action.Expression.ToValue);
                                    break;
                                case RulesScriptsServices.Enums.ConceptType.List:
                                    StringBuilder.AppendFormat("{0}", action.Expression.ListValue);

                                    break;
                                case RulesScriptsServices.Enums.ConceptType.Reference:
                                    StringBuilder.AppendFormat("{0} ({1})", action.Expression.Description.ToUpper(), action.Expression.Id);

                                    break;
                                default:
                                    StringBuilder.AppendFormat("{0} ", action.Expression);
                                    break;
                            }
                            break;

                        case ComparatorType.ConceptValue:
                            StringBuilder.AppendFormat("{0}", Errors.TheConcept);
                            StringBuilder.AppendFormat("{0}", action.Expression.Description.ToUpper());
                            break;

                        case ComparatorType.ExpressionValue:
                            StringBuilder.AppendFormat("{0}", Errors.ExpressionValue);
                            StringBuilder.AppendFormat("{0}", action.Expression);
                            break;

                        case ComparatorType.TemporalyValue:
                            StringBuilder.AppendFormat("{0}", Errors.TemporalValue);
                            StringBuilder.AppendFormat("{0}", action.Expression);

                            break;
                    }
                    break;
                case AssignType.InvokeAssign:
                    StringBuilder.AppendFormat("{0}", Errors.Invoke);
                    break;
            }


            return StringBuilder.ToString();
        }
        static string FillActionValueTemp(_ActionValueTemp action)
        {
            StringBuilder StringBuilder = new StringBuilder();
            switch (action.AssignType)
            {
                case AssignType.InvokeAssign:
                    StringBuilder.AppendFormat("{0} {1}", Errors.Invoke);
                    break;

                case AssignType.TemporalAssign:
                    StringBuilder.AppendFormat("{0} ", Errors.ToTemporalValue);
                    StringBuilder.AppendFormat("{0} ", action.ValueTemp);
                    StringBuilder.AppendFormat("{0}", action.ArithmeticOperator.Description.ToUpper());
                    switch (action.ComparatorType)
                    {
                        case ComparatorType.ConstantValue:
                            StringBuilder.AppendFormat("{0} ", Errors.TheValue);
                            StringBuilder.AppendFormat("{0}", action.Expression);

                            break;
                        case ComparatorType.ConceptValue:
                            StringBuilder.AppendFormat("{0} ", Errors.TheConcept);
                            StringBuilder.AppendFormat("{0} ", action.Expression.Description.ToUpper());
                            break;
                        case ComparatorType.ExpressionValue:
                            StringBuilder.AppendFormat("{0} ", Errors.ExpressionValue);
                            StringBuilder.AppendFormat("{0} ", action.Expression);
                            break;
                        case ComparatorType.TemporalyValue:
                            StringBuilder.AppendFormat("{0} ", Errors.TemporalValue);
                            StringBuilder.AppendFormat("{0} ", action.Expression);
                            break;
                    }
                    break;
            }


            return StringBuilder.ToString();
        }

        static string FillActionInvoke(_ActionInvoke action)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat("{0}{1}", Errors.Invoke + " ", Environment.NewLine);
            switch (action.InvokeType)
            {
                case InvokeType.MessageInvoke:
                    stringBuilder.AppendFormat("{0} ", Errors.TheMessage);
                    stringBuilder.AppendFormat("{0} ", action.Expression);
                    break;
                case InvokeType.RuleSetInvoke:
                    stringBuilder.AppendFormat("{0} ", Errors.TheRuleSet + " ");
                    stringBuilder.AppendFormat("{0} ", action.Expression.Description.ToUpper());
                    break;
                case InvokeType.FunctionInvoke:
                    stringBuilder.AppendFormat("{0} ", Errors.TheFunction);
                    stringBuilder.AppendFormat("{0} ", action.Expression.Description.ToUpper());
                    break;
                default:
                    break;
            }
            return stringBuilder.ToString();
        }

    }
}
