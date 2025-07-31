using Sistran.Core.Application.RulesScriptsServices.Models;
using System;
using System.Text;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.UnderwritingServices.Helpers
{
    public class MappingRule
    {
        public static string MappingRules(_RuleSet ruleSet)
        {

            if (ruleSet == null)
                throw new ArgumentException("Regla Vacia");
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("RuleSet: {0}\r\n", ruleSet.Description);
            sb.AppendLine("Rules:");
            sb.AppendLine("-----------");
            TP.Parallel.ForEach(ruleSet.Rules, rule =>
            {
                sb.AppendFormat("Condiciones: {0}\r\n", rule.Description);
                TP.Parallel.ForEach(rule.Conditions, condition =>
                {
                    sb.Append(GenericRule<_Condition>(condition));
                    sb.Append(";");

                });
                sb.AppendLine("Acciones:");
                TP.Parallel.ForEach(rule.Actions, action =>
                {
                    sb.Append(GenericRule<_Action>(action));
                    sb.Append(";");
                });
                sb.AppendLine("\n");
            });
            return sb.ToString();
        }

        private static string GenericRule<T>(T item)
        {
            string st = string.Empty;
            string Name = item.GetType().Name;
            switch (Name)
            {
                case "_Condition":
                    var itemNew = item as _Condition;
                    var l = itemNew.Concept as _ReferenceConcept;
                    if (l != null)
                    {
                        st += l?.Description + itemNew.Comparator.Symbol;
                        if (itemNew.Expression as _ReferenceValue != null)
                        {
                            st += (itemNew.Expression as _ReferenceValue).Description;
                        }
                    }
                    else if (itemNew.Concept as Concept != null)
                    {
                        st += (itemNew.Concept as Concept).Description + itemNew.Comparator.Symbol;
                        if (itemNew.Expression != null)
                        {
                            st += Convert.ToString(itemNew.Expression.Description);
                        }
                    }
                    break;
                case "_ActionConcept":
                    var itemAction = item as _ActionConcept;
                    var z = itemAction.Concept as _ReferenceConcept;
                    if (z != null)
                    {
                        st += z?.Description + itemAction.ArithmeticOperator.Symbol;
                        if (itemAction.Expression as _ReferenceValue != null)
                        {
                            st += (itemAction.Expression as _ReferenceValue).Description;
                        }
                    }
                    else if (itemAction.Concept as Concept != null)
                    {
                        st += (itemAction.Concept as Concept).Description + itemAction.ArithmeticOperator.Symbol;
                        if (itemAction.Expression != null)
                        {
                            st += Convert.ToString(itemAction.Expression.Description);
                        }
                    }
                    break;
            }
            return st;
        }
    }
}

