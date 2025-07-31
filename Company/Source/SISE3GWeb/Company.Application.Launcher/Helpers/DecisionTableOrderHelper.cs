// -----------------------------------------------------------------------
// <copyright file="DecisionTableOrderHelper.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Areas.RulesAndScripts.Models;
    using MRules = Application.RulesScriptsServices.Models;

    public class DecisionTableOrderHelper : IComparer<RuleModelView>
    {
        private readonly List<MRules._Concept> _concepts;
        private readonly List<string> _symbols = new List<string> { "=", "<", "<=", ">", ">=", "<>" };

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="concepts"></param>
        public DecisionTableOrderHelper(List<MRules._Concept> concepts)
        {
            this._concepts = concepts;
        }

        /// <summary>
        /// compara dos valores de tipo RuleComposite
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(RuleModelView x, RuleModelView y)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            IList<MRules._Condition> conditionsx = x.Rule.Conditions;
            IList<MRules._Condition> conditionsy = y.Rule.Conditions;

            foreach (MRules._Concept conddef in this._concepts)
            {
                int entityId = conddef.Entity.EntityId;
                int conceptId = conddef.ConceptId;

                MRules._Condition condx = this.FindCondition(entityId, conceptId, conditionsx);
                MRules._Condition condy = this.FindCondition(entityId, conceptId, conditionsy);

                if (condx?.Comparator == null)
                {
                    if (condy?.Comparator != null)
                    {
                        return 1;
                    }
                }
                else
                {
                    if (condy?.Comparator == null)
                    {
                        return -1;
                    }

                    int comp = this._symbols.LastIndexOf(condx.Comparator.Symbol).CompareTo(this._symbols.IndexOf(condy.Comparator.Symbol));
                    if (comp != 0)
                    {
                        return comp;
                    }
                    else
                    {
                        decimal output;

                        string valueX = "";
                        string valueY = "";
                        if (condx.Expression != null)
                        {
                            if (condx.Expression is MRules._ListEntityValue)
                            {
                                valueX = condx.Expression.ListValueCode.ToString();
                            }
                            else if (condx.Expression is MRules._RangeEntityValue)
                            {
                                valueX = condx.Expression.RangeValueCode.ToString();
                            }
                            else if (condx.Expression is MRules._ReferenceValue)
                            {
                                valueX = condx.Expression.Id.ToString();
                            }
                            else
                            {
                                valueX = condx.Expression.ToString();
                            }
                        }

                        if (condy.Expression != null)
                        {
                            if (condy.Expression is MRules._ListEntityValue)
                            {
                                valueY = condy.Expression.ListValueCode.ToString();
                            }
                            else if (condy.Expression is MRules._RangeEntityValue)
                            {
                                valueY = condy.Expression.RangeValueCode.ToString();
                            }
                            else if (condy.Expression is MRules._ReferenceValue)
                            {
                                valueY = condy.Expression.Id.ToString();
                            }
                            else
                            {
                                valueY = condy.Expression.ToString();
                            }
                        }



                        if (decimal.TryParse(valueX, out output) && decimal.TryParse(valueY, out output))
                        {
                            if (Convert.ToDecimal(valueX) < Convert.ToDecimal(valueY))
                            {
                                comp = -1;
                            }
                            else if (Convert.ToDecimal(valueX) == Convert.ToDecimal(valueY))
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
                            comp = string.Compare(valueX, valueY, false, ci);
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

            return x.Rule.RuleId.CompareTo(y.Rule.RuleId);
        }

        /// <summary>
        /// busca una condicon segun el entityId y conceptId
        /// </summary>
        /// <param name="entityId">id de la entidad</param>
        /// <param name="conceptId">id del concepto</param>
        /// <param name="conditions">lista de condiciones</param>
        /// <returns></returns>
        private MRules._Condition FindCondition(int entityId, int conceptId, IList<MRules._Condition> conditions)
        {
            foreach (MRules._Condition cond in conditions)
            {
                if (cond.Concept.Entity.EntityId == entityId && cond.Concept.ConceptId == conceptId)
                {
                    return cond;
                }
            }

            return null;
        }
    }
}