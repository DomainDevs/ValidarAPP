// -----------------------------------------------------------------------
// <copyright file="RuleModelView.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.RulesScriptsServices.Enums;
    using MRules = Application.RulesScriptsServices.Models;

    /// <summary>
    /// ModelVIew de la regla (para DT)
    /// </summary>
    public class RuleModelView
    {
        /// <summary>
        /// Obtiene o establece la regla original
        /// </summary>
        public MRules._Rule Rule { get; set; }

        /// <summary>
        /// Obtiene o establece es estado
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Obtiene o establece las condiciones
        /// </summary>
        public List<string> Conditions { get; set; }

        /// <summary>
        /// Obtiene o establece las acciones 
        /// </summary>
        public List<string> Actions { get; set; }

        /// <summary>
        /// Obtiene o establece el separador
        /// </summary>
        public string Separator { get; } = ">>>>";


        /// <summary>
        /// Realiza el mapeo de un modelo a un modelView
        /// </summary>
        /// <param name="rules">lista de modelos</param>
        /// <returns>lista de modelsView</returns>
        internal static List<RuleModelView> RulesModelView(List<MRules._Rule> rules)
        {
            List<RuleModelView> modelViews = new List<RuleModelView>();
            foreach (MRules._Rule rule in rules.AsParallel().AsOrdered())
            {
                RuleModelView ruleModelView = new RuleModelView
                {
                    Rule = rule,
                    Actions = rule.Actions.Select(FillExpression).ToList(),
                    Conditions = rule.Conditions.Select(FillExpression).ToList()
                };
                modelViews.Add(ruleModelView);
            }
            return modelViews;
        }

        /// <summary>
        /// Realiza la visualizacion de una accion/condicion a un string
        /// </summary>
        /// <param name="data">accion/  condicion</param>
        /// <returns>expresion de la accion/condicion</returns>
        private static string FillExpression(dynamic data)
        {
            string expression = string.Empty;
            if (data is MRules._Condition)
            {
                if (data.Expression == null && data.Comparator == null)
                {
                    return "[INDISTINTO]";
                }

                if (data.Expression == null && data.Comparator != null)
                {
                    return data.Comparator.Description;
                }

                expression = data.Comparator.Symbol;
            }
            if (!(data is MRules._Condition))
            {
                if (data.Expression == null)
                {
                    return "[NADA]";
                }

                expression = data.ArithmeticOperator.Symbol;
            }

            ConceptType conceptType = data.Concept.ConceptType;
            if (data.HasError)
            {
                return data.Expression;
            }
            switch (conceptType)
            {
                case ConceptType.Basic:
                    return expression + data.Expression.ToString();

                case ConceptType.Range:
                    return expression + data.Expression.FromValue + "-" + data.Expression.ToValue;

                case ConceptType.List:
                    return expression + data.Expression.ListValue.ToString();

                case ConceptType.Reference:
                    return expression + data.Expression.Description + "(" + data.Expression.Id + ")";
                default:
                    return "ERROR DATA";
            }
        }
    }
}