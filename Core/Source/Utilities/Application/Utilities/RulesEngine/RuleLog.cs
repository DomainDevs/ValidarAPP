// -----------------------------------------------------------------------
// <copyright file="DebugRules.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.Utilities.RulesEngine
{
    using System;
    using System.IO;
    using System.Text;
    using Framework.Rules;
    using Framework.Rules.Integration.Expressions;
    using Rule = Framework.Rules.Integration.Rule;
    using RuleSet = Framework.Rules.Integration.RuleSet;
    using System.Linq;
    using Framework.Rules.Integration;
    using System.Threading;
    using System.Configuration;

    public class RuleLog
    {
        private RuleSet ruleSet;

        public RuleLog()
        {
        }

        public RuleLog(RuleSet ruleSet)
        {
            this.ruleSet = ruleSet;
        }

        public RuleSet SetLog()
        {
            StringBuilder str = new StringBuilder();

            ClassParameter classParameter = new ClassParameter("prmFacadeLog", "Sistran.Core.Application.Utilities.RulesEngine.FacadeLog, Sistran.Core.Application.Utilities.Entities");
            Expression ruleSetName = this.SetConceptLog(RuleConceptLog.RuleSetName.Key, this.ruleSet.Name);
            Expression ruleSetId = this.SetConceptLog(RuleConceptLog.RuleSetId.Key, this.ruleSet.Id.ToString());
            Expression facade = this.SetConceptLog(RuleConceptLog.Facade.Key, $"Package=\"{this.ruleSet.PackageId}\" Level=\"{this.ruleSet.LevelId}\"");
            InvokeMethodExpression function = this.SetFunctionLog("LogRuleSet");

            /*REGLAS*/
            Rule ruleIn = new Rule();
            ruleIn.Parameters.Add(classParameter);
            ruleIn.Consequences.Add(ruleSetId);
            ruleIn.Consequences.Add(ruleSetName);
            ruleIn.Consequences.Add(facade);
            ruleIn.Consequences.Add(function);

            /*REGLAS*/
            Rule ruleOut = new Rule();
            ruleOut.Parameters.Add(classParameter);
            ruleOut.Consequences.Add(this.SetConceptLog(RuleConceptLog.RuleSetId.Key, "CloseRuleSet"));
            ruleOut.Consequences.Add(function);

            foreach (Rule t in this.ruleSet.Rules)
            {
                str.Clear();

                t.Parameters.Add(classParameter);

                /*Nombre de la regla*/
                Expression ruleName = this.SetConceptLog(RuleConceptLog.RuleName.Key, t.Name);

                /*Condiciones*/
                for (int index = 0; index < t.Conditions.Count; index++)
                {
                    Expression condition = t.Conditions[index];
                    str.AppendLine($"<Condicion Valor=\"{condition}\"/>");
                }
                Expression ruleCondition = this.SetConceptLog(RuleConceptLog.Condition.Key, str.ToString());

                /*Acciones*/
                str.Clear();
                for (int index = 0; index < t.Consequences.Count; index++)
                {
                    Expression action = t.Consequences[index];
                    str.AppendLine($"<Accion Valor=\"{action}\"/>");
                }
                Expression ruleAction = this.SetConceptLog(RuleConceptLog.Action.Key, str.ToString());

                t.Consequences.Insert(0, ruleName);
                t.Consequences.Insert(1, ruleCondition);
                t.Consequences.Insert(2, ruleAction);
                t.Consequences.Insert(3, this.SetConceptLog(RuleConceptLog.RuleSetId.Key, "OpenRule"));
                t.Consequences.Insert(4, function);
                if (this.ruleSet.RuleSetType == RuleSetType.Option)
                {
                    t.Consequences.Insert(0, ruleSetId);
                    t.Consequences.Insert(1, ruleSetName);
                    t.Consequences.Insert(2, facade);
                    t.Consequences.Insert(3, function);
                }
                //Ejecucion normal de la regla
                t.Consequences.Add(this.SetConceptLog(RuleConceptLog.RuleSetId.Key, "CloseRule"));
                t.Consequences.Add(function);
                if (this.ruleSet.RuleSetType == RuleSetType.Option)
                {
                    t.Consequences.Add(this.SetConceptLog(RuleConceptLog.RuleSetId.Key, "CloseRuleSet"));
                    t.Consequences.Add(function);
                }
            }

            if (this.ruleSet.RuleSetType == RuleSetType.Sequence)
            {
                this.ruleSet.Rules.Insert(0, ruleIn);
                this.ruleSet.Rules.Add(ruleOut);
            }

            return this.ruleSet;
        }

        private InvokeMethodExpression SetFunctionLog(string value)
        {
            ParametersExpression parametersExpression = new ParametersExpression();
            PrimitiveExpression primitiveExpression = new PrimitiveExpression("String", value);
            parametersExpression.InnerExpressions.Add(primitiveExpression);

            return new InvokeMethodExpression("ExecuteFunction", parametersExpression, new TargetExpression(new ThisExpression()));
        }

        private Expression SetConceptLog(string conceptName, string value)
        {
            ArgumentExpression argumentException = new ArgumentExpression("prmFacadeLog");
            PropertyExpression propertyExpression = new PropertyExpression(conceptName, argumentException);

            PrimitiveExpression primitiveExpression = new PrimitiveExpression("String", value);
            CastExpression castExpression = new CastExpression("System.String", primitiveExpression);

            AssignExpression assignExpression = new AssignExpression(propertyExpression, castExpression);

            return assignExpression;
        }

        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        public void LogRuleSet(Facade facade)
        {
            string ruleSetId = facade.GetConcept<string>(RuleConceptLog.RuleSetId);
            string ruleSetName = facade.GetConcept<string>(RuleConceptLog.RuleSetName);
            string ruleName = facade.GetConcept<string>(RuleConceptLog.RuleName);
            string condition = facade.GetConcept<string>(RuleConceptLog.Condition);
            string action = facade.GetConcept<string>(RuleConceptLog.Action);
            string _facade = facade.GetConcept<string>(RuleConceptLog.Facade);

            string path = ConfigurationManager.AppSettings["FolderLog"];
            if (string.IsNullOrEmpty(path))
            {
                path = $"C:\\";
            }
            path += $"LogRuleSet_{DateTime.Now:yy-MM-dd}.xml";

            _readWriteLock.EnterWriteLock();
            try
            {
                using (FileStream fs = File.Open(path, FileMode.Append))
                {
                    StringBuilder sb = new StringBuilder();

                    switch (ruleSetId)
                    {
                        case "CloseRuleSet":
                            sb.AppendLine($"</Ejecutando_Paquete_Reglas>");
                            break;
                        case "OpenRule":
                            sb.AppendLine($"<Ejecutando_Regla Nombre=\"{ruleName}\">");
                            sb.AppendLine("<Conceptos_Entrada>");
                            facade.Concepts.AsParallel().Where(x => x.EntityId != RuleConceptLog.Id)
                                .OrderBy(x => x.Name).ThenBy(x => x.Id)
                                .Select(x => $"<Concepto Nombre=\"{x.Name}\" Id=\"{x.Id}\" Facade=\"{x.EntityId}\" Valor=\"{x.Value}\" />")
                                .ToList().ForEach(x => sb.AppendLine(x));
                            sb.AppendLine("</Conceptos_Entrada>");
                            sb.AppendLine("<Condiciones>");
                            sb.AppendLine(condition);
                            sb.AppendLine("</Condiciones>");
                            sb.AppendLine("<Acciones>");
                            sb.AppendLine(action);
                            sb.AppendLine("</Acciones>");
                            break;
                        case "CloseRule":
                            sb.AppendLine("<Conceptos_Salida>");
                            facade.Concepts.AsParallel().Where(x => x.EntityId != RuleConceptLog.Id)
                                .OrderBy(x => x.Name).ThenBy(x => x.Id)
                                .Select(x => $"<Concepto Nombre=\"{x.Name}\" Id=\"{x.Id}\" Facade=\"{x.EntityId}\" Valor=\"{x.Value}\" />")
                                .ToList().ForEach(x => sb.AppendLine(x));
                            sb.AppendLine("</Conceptos_Salida>");
                            sb.AppendLine("</Ejecutando_Regla>");
                            break;
                        default:
                            sb.AppendLine($"<Ejecutando_Paquete_Reglas Id=\"{ruleSetId}\" Nombre=\"{ruleSetName}\" {_facade}>");
                            break;
                    }

                    string dataasstring = sb.ToString();
                    byte[] info = new UTF8Encoding(true).GetBytes(dataasstring);
                    fs.Write(info, 0, info.Length);
                }
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}