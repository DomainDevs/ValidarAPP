using System;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.Models;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// clase que reorganiza las filas de la trabla de desicion 
    /// </summary>
    internal class RulesCompositeComparer : IComparer<RuleComposite>
    {
        private List<Concept> _conditions;
        private List<string> Symbols = new List<string> { "=", "<", "<=", ">", ">=", "<>" };

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="_conditions"></param>
        public RulesCompositeComparer(List<Concept> _conditions)
        {
            this._conditions = _conditions;
        }

        /// <summary>
        /// compara dos valores de tipo RuleComposite
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(RuleComposite x, RuleComposite y)
        {
            System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;

            IList<Condition> conditionsx = x.Conditions;
            IList<Condition> conditionsy = y.Conditions;

            foreach (Concept conddef in _conditions)
            {
                int entityId = conddef.EntityId;
                int conceptId = conddef.ConceptId;

                Condition condx = FindCondition(entityId, conceptId, conditionsx);
                Condition condy = FindCondition(entityId, conceptId, conditionsy);

                if (condx == null || condx.Comparator == null)
                {
                    if (condy != null && condy.Comparator != null)
                    {
                        return 1;
                    }
                }
                else
                {
                    if (condy == null || condy.Comparator == null)
                    {
                        return -1;
                    }

                    int comp = Symbols.LastIndexOf(condx.Comparator.Symbol).CompareTo(Symbols.IndexOf(condy.Comparator.Symbol));
                    if (comp != 0)
                    {
                        return comp;
                    }
                    else
                    {
                        decimal output;
                        if (decimal.TryParse(condx.Value.ToString(), out output) && decimal.TryParse(condy.Value.ToString(), out output))
                        {
                            if (Convert.ToDecimal(condx.Value) < Convert.ToDecimal(condy.Value))
                            {
                                comp = -1;
                            }
                            else if (Convert.ToDecimal(condx.Value) == Convert.ToDecimal(condy.Value))
                            {
                                comp = 0;
                            }
                            else
                            {
                                comp = 1;
                            }
                        }
                        else
                        {
                            comp = string.Compare(condx.Value.ToString(), condy.Value.ToString(), false, ci);
                        }

                        if (comp != 0)
                        {
                            string ct = condx.Comparator.Symbol;
                            if (ct == ">" || ct == ">=")
                            {
                                return -comp;
                            }
                            return comp;
                        }
                    }
                }
            }

            return x.RuleId.CompareTo(y.RuleId);
        }
        
        /// <summary>
        /// busca una condicon segun el entityId y conceptId
        /// </summary>
        /// <param name="entityId">id de la entidad</param>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="conditions">lista de condiciones</param>
        /// <returns></returns>
        private Condition FindCondition(int entityId, int conceptId, IList<Condition> conditions)
        {
            foreach (Condition cond in conditions)
            {
                if (cond.Concept.EntityId == entityId && cond.Concept.ConceptId == conceptId)
                {
                    return cond;
                }
            }

            return null;
        }
    }
}