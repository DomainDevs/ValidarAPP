using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
//using Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Framework.Math;
using Sistran.Core.Framework.Math.Intervals;
using Sistran.Core.Framework.Queries;
using EmRules = Sistran.Core.Application.RulesScriptsServices.Enums;
using EnParam = Sistran.Core.Application.Parameters.Entities;
using EnRules = Sistran.Core.Application.Script.Entities;
using MRules = Sistran.Core.Application.RulesScriptsServices.Models;
using Type = System.Type;
using USHELPER = Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.Enums;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.Helper
{
    public static class ConvertHelper
    {
        public static decimal ConvertToDecimal(dynamic value)
        {
            try
            {
                CultureInfo culture = CultureInfo.InvariantCulture;
                dynamic valueTmp = value.ToString().Replace(culture.NumberFormat.CurrencyGroupSeparator, culture.NumberFormat.CurrencyDecimalSeparator);
                return decimal.Parse(valueTmp, culture);
            }
            catch (Exception ex)
            {
                throw new BusinessException($"Formato invalido: {value}", ex);
            }
        }
    }

    public static class XmlHelperReader
    {
        private static ResourceManager _resx = Resources.Errors.ResourceManager;
        private static readonly string _namespace = "http://www.sistran.com/RuleEngine/rules.xsd";
        private static readonly string _attNamespace = string.Empty;

        internal static readonly _ConceptDao _conceptsDao = new _ConceptDao();
        internal static readonly _RuleSetDao _ruleSetDao = new _RuleSetDao();
        internal static readonly _RuleFunctionDao _ruleFunctionDao = new _RuleFunctionDao();

        /// <summary>
        /// obtiene una lista del contenido del xml
        /// </summary>
        /// <param name="xmlBytes">bytes de la regla</param>
        /// <returns></returns>
        public static MRules._RuleSet GetRuleSetByXml(byte[] xmlBytes)
        {
            XPathNavigator xPathNavigator = GetXPathNavigator(xmlBytes);
            return FillRuleSet(xPathNavigator);
        }

        private static XPathNavigator GetXPathNavigator(byte[] xmlBytes)
        {
            using (MemoryStream memoryStream = new MemoryStream(xmlBytes))
            {
                XPathDocument xpdoc = new XPathDocument(memoryStream);
                XPathNavigator xPathNavigator = xpdoc.CreateNavigator();
                return xPathNavigator;
            }
        }

        public static MRules._RuleSet FillRuleSet(XPathNavigator nav)
        {
            int ruleId = 0;
            List<Exception> exceptions = new List<Exception>();

            XPathNodeIterator ite = nav.Select("/node()");

            if (!ite.MoveNext())
            {
                throw new Exception("Formato incorrecto del archivo: falta el nodo raiz.");
            }

            nav = ite.Current;

            string nameAttribute = nav.GetAttribute("name", _attNamespace);
            string typeAttribute = nav.GetAttribute("type", _attNamespace);

            MRules._RuleSet ruleSet = new MRules._RuleSet
            {
                Description = nameAttribute,
                Type = string.IsNullOrEmpty(typeAttribute) ? EmRules.RuleBaseType.Sequence : (EmRules.RuleBaseType)Enum.Parse(typeof(EmRules.RuleBaseType), typeAttribute, true),
                Rules = new List<MRules._Rule>()
            };

            ite = nav.SelectChildren("rule", _namespace);

            while (ite.MoveNext())
            {
                try
                {
                    MRules._Rule rule = FillRule(ite.Current, ++ruleId);
                    rule.HasError = rule.Actions.Any(x => x.HasError) || rule.Conditions.Any(x => x.HasError);
                    ruleSet.Rules.Add(rule);
                }
                catch (AggregateException e)
                {
                    string error = string.Empty;
                    if (ruleId != 0)
                    {
                        //error = ", Error en la regla: " + e.Message + "(" + ruleId + ") "; Se comenta para evitar mensaje en ingles en ambiente de cliente.
                        error = ", Error en la regla " + "(" + ruleId + ") ";
                    }
                    e.InnerExceptions.ToList().ForEach(item => error = error + item.Message);
                    exceptions.Add(new Exception(error));
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException("Error deserealizando XML " + ruleSet.Description, exceptions);
            }

            return ruleSet;
        }

        private static MRules._Rule FillRule(XPathNavigator nav, int ruleId)
        {

            List<Exception> exceptions = new List<Exception>();
            CodeExpressionCollection conditions = new CodeExpressionCollection();
            CodeStatementCollection actions = new CodeStatementCollection();

            MRules._Rule rule = new MRules._Rule
            {
                Description = nav.GetAttribute("name", _attNamespace),
                Parameters = FillParameters(nav.SelectChildren("parameter", _namespace)),
                Conditions = new List<MRules._Condition>(),
                Actions = new List<MRules._Action>(),
                RuleId = ruleId
            };

            XPathNodeIterator parameterChildren = nav.SelectChildren("condition", _namespace);
            while (parameterChildren.MoveNext())
            {
                XPathNodeIterator xPathNodeIterator = parameterChildren.Current.SelectChildren(XPathNodeType.Element);

                if (xPathNodeIterator.MoveNext())
                {
                    conditions.Add(LoadExpression(xPathNodeIterator.Current));
                }
            }

            parameterChildren = nav.SelectChildren("consequence", _namespace);
            if (parameterChildren.MoveNext())
            {
                CodeStatement[] stmts = LoadStatements(parameterChildren.Current);
                if (stmts != null)
                {
                    actions.AddRange(stmts);
                }
            }

            int contador = 0;
            foreach (CodeBinaryOperatorExpression condition in conditions)
            {
                try
                {
                    contador++;
                    rule.Conditions.Add(FillCondition(condition, rule.Conditions));
                }
                catch (Exception)
                {
                    exceptions.Add(new Exception($"Error en la Condicion ({contador}) "));
                }
            }

            contador = 0;
            foreach (CodeStatement action in actions)
            {
                try
                {
                    contador++;
                    rule.Actions.Add(FillAction(action, rule.Actions.Where(x => x.AssignType == EmRules.AssignType.ConceptAssign).Cast<MRules._ActionConcept>().ToList()));
                }
                catch (Exception)
                {
                    exceptions.Add(new Exception($"Error en la Accion ({contador}) "));
                }
            }

            if (exceptions.Count > 0)
            {
                throw new AggregateException(rule.Description, exceptions);
            }

            return rule;
        }


        private static void FillExpressionAction(CodeExpression expresion, ref MRules._ActionConcept actionConcept)
        {
            CodeExpression right = expresion;


            if (right is CodeCastExpression)
            {
                actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                {
                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign,
                    Description = _resx.GetString(EmRules.ArithmeticOperatorType.Assign.ToString()),
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(EmRules.ArithmeticOperatorType.Assign).Value
                };

                right = (right as CodeCastExpression).Expression;


                if (right is CodeMethodInvokeExpression)
                {
                    string method = (right as CodeMethodInvokeExpression).Method.MethodName;
                    if (method == "Round")
                    {
                        right = (right as CodeMethodInvokeExpression).Parameters[1];
                        if ((right as CodeCastExpression)?.Expression is CodeCastExpression)
                        {
                            right = (right as CodeCastExpression).Expression;
                            if (right is CodeMethodInvokeExpression)
                            {
                                actionConcept.Expression = FillExpression(right);
                                actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                            }
                        }
                    }
                    else
                    {
                        actionConcept.Expression = FillExpression(right);
                        actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                    }
                }
                else if (right is CodeIndexerExpression || (right as CodeCastExpression)?.Expression is CodeIndexerExpression)
                {
                    actionConcept.Expression = FillExpression(right);
                    actionConcept.ComparatorType = EmRules.ComparatorType.TemporalyValue;
                }
            }
            else if (right is CodePrimitiveExpression)
            {
                actionConcept.ComparatorType = EmRules.ComparatorType.ConstantValue;
                actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                {
                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign,
                    Description = _resx.GetString(EmRules.ArithmeticOperatorType.Assign.ToString()),
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(EmRules.ArithmeticOperatorType.Assign).Value
                };
                actionConcept.Expression = FillExpression(right);
            }
            else if (right is CodeBinaryOperatorExpression)
            {
                CodeExpression left = (right as CodeBinaryOperatorExpression).Left;
                if (left is CodePrimitiveExpression || left is CodeBinaryOperatorExpression)
                {
                    actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                    {
                        ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign,
                        Description = _resx.GetString(EmRules.ArithmeticOperatorType.Assign.ToString()),
                        Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(EmRules.ArithmeticOperatorType.Assign).Value
                    };
                }
                else
                {
                    right = (right as CodeBinaryOperatorExpression).Right;
                    actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                    {
                        ArithmeticOperatorType = (EmRules.ArithmeticOperatorType)Enum.Parse(typeof(EmRules.ArithmeticOperatorType), ((CodeBinaryOperatorExpression)expresion).Operator.ToString())
                    };
                    actionConcept.ArithmeticOperator.Description = _resx.GetString(actionConcept.ArithmeticOperator.ArithmeticOperatorType.ToString());
                    actionConcept.ArithmeticOperator.Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(actionConcept.ArithmeticOperator.ArithmeticOperatorType).Value;
                }

                actionConcept.Expression = FillExpression(right);

                if (right is CodePrimitiveExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ConstantValue;
                }
                else if ((right as CodeCastExpression)?.Expression is CodeMethodInvokeExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                }
                else if (right is CodeBinaryOperatorExpression || (right as CodeCastExpression)?.Expression is CodeBinaryOperatorExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ExpressionValue;
                }
                else if ((right as CodeCastExpression)?.Expression is CodeIndexerExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.TemporalyValue;
                }
                else if (right is CodePropertyReferenceExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                }
            }
            else if (right is CodeMethodInvokeExpression)
            {
                string method = (right as CodeMethodInvokeExpression).Method.MethodName;
                actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                {
                    ArithmeticOperatorType = (EmRules.ArithmeticOperatorType)Enum.Parse(typeof(EmRules.ArithmeticOperatorType), method)
                };
                actionConcept.ArithmeticOperator.Description = _resx.GetString(actionConcept.ArithmeticOperator.ArithmeticOperatorType.ToString());
                actionConcept.ArithmeticOperator.Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(actionConcept.ArithmeticOperator.ArithmeticOperatorType).Value;

                right = (right as CodeMethodInvokeExpression).Parameters[1];
                actionConcept.Expression = FillExpression(right);

                if ((right as CodeCastExpression)?.Expression is CodePrimitiveExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ConstantValue;
                }
                else if ((right as CodeCastExpression)?.Expression is CodeCastExpression)
                {
                    right = (right as CodeCastExpression).Expression;
                    if (right is CodeMethodInvokeExpression || (right as CodeCastExpression)?.Expression is CodeMethodInvokeExpression)
                    {
                        actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                    }
                    else if ((right as CodeCastExpression)?.Expression is CodeIndexerExpression)
                    {
                        actionConcept.ComparatorType = EmRules.ComparatorType.TemporalyValue;
                    }
                }
                else if ((right as CodeCastExpression)?.Expression is CodeBinaryOperatorExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ExpressionValue;
                }
                else if ((right as CodeCastExpression)?.Expression is CodePropertyReferenceExpression)
                {
                    actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                }
            }
            else if (right is CodePropertyReferenceExpression)
            {
                actionConcept.Expression = FillExpression(right);
                actionConcept.ComparatorType = EmRules.ComparatorType.ConceptValue;
                actionConcept.ArithmeticOperator = new MRules._ArithmeticOperator
                {
                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign,
                    Description = _resx.GetString(EmRules.ArithmeticOperatorType.Assign.ToString()),
                    Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(EmRules.ArithmeticOperatorType.Assign).Value
                };
            }
        }

        private static MRules._Action FillAction(CodeStatement statement, List<MRules._ActionConcept> lastActions)
        {
            if (statement is CodeAssignStatement)
            {
                object action;

                CodeAssignStatement codeAssignStatement = (CodeAssignStatement)statement;

                CodeExpression left = codeAssignStatement.Left;
                CodePropertyReferenceExpression propref = left as CodePropertyReferenceExpression;
                if (propref != null)
                {
                    action = new MRules._ActionConcept();
                    ((MRules._ActionConcept)action).AssignType = EmRules.AssignType.ConceptAssign;
                    ((MRules._ActionConcept)action).Concept = FillConcept(propref);
                }
                else
                {
                    CodeIndexerExpression idx = left as CodeIndexerExpression;
                    if (idx != null)
                    {
                        action = new MRules._ActionValueTemp();
                        ((MRules._ActionValueTemp)action).AssignType = EmRules.AssignType.TemporalAssign;
                        ((MRules._ActionValueTemp)action).ValueTemp = (string)((CodePrimitiveExpression)idx.Indices[0]).Value;
                    }
                    else
                    {
                        throw new Exception("Assign type not supported: " + left.GetType().FullName);
                    }
                }

                CodeExpression right = codeAssignStatement.Right;
                while (right is CodeCastExpression)
                {
                    right = ((CodeCastExpression)right).Expression;
                }

                CodeBinaryOperatorExpression binExpr = right as CodeBinaryOperatorExpression;
                if (binExpr != null)
                {
                    left = binExpr.Left;
                    if (left is CodePropertyReferenceExpression || left is CodeCastExpression)
                    {
                        if (action is MRules._ActionValueTemp)
                        {
                            ((MRules._ActionValueTemp)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType =
                                        (EmRules.ArithmeticOperatorType)Enum.Parse(typeof(EmRules.ArithmeticOperatorType),
                                            binExpr.Operator.ToString()),
                                };

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Description =
                                    _resx.GetString(((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType.ToString());

                        }
                        else
                        {
                            ((MRules._ActionConcept)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType =
                                        (EmRules.ArithmeticOperatorType)Enum.Parse(typeof(EmRules.ArithmeticOperatorType),
                                            binExpr.Operator.ToString()),
                                };

                            ((MRules._ActionConcept)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionConcept)action).ArithmeticOperator.Description =
                                _resx.GetString(((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }
                        right = binExpr.Right;
                    }
                    else
                    {
                        if (action is MRules._ActionValueTemp)
                        {
                            ((MRules._ActionValueTemp)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign
                                };

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Description =
                                _resx.GetString(((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }
                        else
                        {
                            ((MRules._ActionConcept)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign
                                };

                            ((MRules._ActionConcept)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionConcept)action).ArithmeticOperator.Description =
                                _resx.GetString(((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }
                    }
                }
                else
                {
                    CodeMethodInvokeExpression mi = right as CodeMethodInvokeExpression;
                    if (mi != null && mi.Method.MethodName == "Round")
                    {
                        if (action is MRules._ActionValueTemp)
                        {
                            ((MRules._ActionValueTemp)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Round
                                };

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Description =
                                _resx.GetString(((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }
                        else
                        {
                            ((MRules._ActionConcept)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Round
                                };

                            ((MRules._ActionConcept)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionConcept)action).ArithmeticOperator.Description =
                                  _resx.GetString(((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }


                        right = mi.Parameters[1];
                    }
                    else
                    {
                        if (action is MRules._ActionValueTemp)
                        {
                            ((MRules._ActionValueTemp)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign
                                };

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionValueTemp)action).ArithmeticOperator.Description =
                                _resx.GetString(((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }
                        else
                        {
                            ((MRules._ActionConcept)action).ArithmeticOperator =
                                new MRules._ArithmeticOperator
                                {
                                    ArithmeticOperatorType = EmRules.ArithmeticOperatorType.Assign,
                                };

                            ((MRules._ActionConcept)action).ArithmeticOperator.Symbol =
                                EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.ArithmeticOperatorType>(
                                    ((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType).Value;

                            ((MRules._ActionConcept)action).ArithmeticOperator.Description =
                                _resx.GetString(((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType.ToString());
                        }
                    }
                }


                while (right is CodeCastExpression)
                {
                    right = ((CodeCastExpression)right).Expression;
                }

                EmRules.ComparatorType valueType;
                if (right is CodePrimitiveExpression)
                {
                    valueType = EmRules.ComparatorType.ConstantValue;
                }
                else if (right is CodePropertyReferenceExpression || right is CodeMethodInvokeExpression || right is CodeCastExpression && ((CodeCastExpression)right).Expression is CodeMethodInvokeExpression)
                {
                    valueType = EmRules.ComparatorType.ConceptValue;
                }
                else if (right is CodeBinaryOperatorExpression)
                {
                    valueType = EmRules.ComparatorType.ExpressionValue;
                }
                else if (right is CodeIndexerExpression)
                {
                    valueType = EmRules.ComparatorType.TemporalyValue;
                }
                else
                {
                    throw new ApplicationException("No se reconoce el tipo del valor.");
                }

                if (action is MRules._ActionValueTemp)
                {
                    ((MRules._ActionValueTemp)action).ComparatorType = valueType;
                    ((MRules._ActionValueTemp)action).Expression = FillExpression(right);
                    return (MRules._ActionValueTemp)action;
                }
                else
                {
                    ((MRules._ActionConcept)action).ComparatorType = valueType;
                    ((MRules._ActionConcept)action).Expression = FillExpression(right);

                    MRules._Concept conceptTmp = (MRules._Concept)((MRules._ActionConcept)action).Concept;
                    dynamic valueExpression = ((MRules._ActionConcept)action).Expression;

                    if (((MRules._ActionConcept)action).ComparatorType == EmRules.ComparatorType.ConstantValue)
                    {
                        if (valueExpression.GetType().Name == "DateTime")
                        {
                            ((MRules._ActionConcept)action).Expression = Convert.ToDateTime(valueExpression).ToString("dd/MM/yyyy");
                        }
                        try
                        {
                            switch (conceptTmp.ConceptType)
                            {

                                case EmRules.ConceptType.Range:

                                    MRules._RangeConcept rangeConcept = _conceptsDao.GetRangeConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);
                                    rangeConcept.ConceptDependences = ((MRules._Concept)((MRules._ActionConcept)action).Concept).ConceptDependences;
                                    ((MRules._ActionConcept)action).Concept = rangeConcept;
                                    ((MRules._ActionConcept)action).Expression = rangeConcept.RangeEntity.RangeEntityValues.First(x => x.RangeValueCode == (int)valueExpression);
                                    break;

                                case EmRules.ConceptType.List:
                                    if (valueExpression.GetType().Name == "Boolean")
                                    {
                                        MRules._ListConcept listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);
                                        listConcept.ConceptDependences = ((MRules._Concept)((MRules._ActionConcept)action).Concept).ConceptDependences;
                                        ((MRules._ActionConcept)action).Concept = listConcept;
                                        ((MRules._ActionConcept)action).Expression = listConcept.ListEntity.ListEntityValues.First(x => Convert.ToBoolean(x.ListValueCode) == Convert.ToBoolean(valueExpression));
                                    }
                                    else
                                    {
                                        MRules._ListConcept listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);
                                        listConcept.ConceptDependences = ((MRules._Concept)((MRules._ActionConcept)action).Concept).ConceptDependences;
                                        ((MRules._ActionConcept)action).Concept = listConcept;
                                        ((MRules._ActionConcept)action).Expression = listConcept.ListEntity.ListEntityValues.First(x => x.ListValueCode == (int)valueExpression);
                                    }
                                    break;

                                case EmRules.ConceptType.Reference:
                                    List<string> dependency = new List<string>();

                                    foreach (MRules._ConceptDependence dependence in conceptTmp.ConceptDependences)
                                    {
                                        MRules._ActionConcept conceptTmp2 = lastActions.FirstOrDefault(x =>
                                            ((MRules._Concept)x.Concept).ConceptId ==
                                            dependence.DependsConcept.ConceptId &&
                                            ((MRules._Concept)x.Concept).Entity.EntityId ==
                                            dependence.DependsConcept.Entity.EntityId);

                                        if (conceptTmp2 != null)
                                        {
                                            dependency.Add(conceptTmp2.Expression.Id);
                                        }
                                    }

                                    MRules._ReferenceConcept referenceConcept = _conceptsDao.GetReferenceConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId, dependency.ToArray());
                                    referenceConcept.ConceptDependences = ((MRules._Concept)((MRules._ActionConcept)action).Concept).ConceptDependences;
                                    ((MRules._ActionConcept)action).Concept = referenceConcept;
                                    ((MRules._ActionConcept)action).Expression = referenceConcept.ReferenceValues.First(x => x.Id == valueExpression.ToString());

                                    break;
                            }
                        }
                        catch (Exception)
                        {
                            ((MRules._ActionConcept)action).HasError = true;
                            ((MRules._ActionConcept)action).ErrorDescription = "Error al evaluar la expresión";
                        }
                    }

                    return (MRules._ActionConcept)action;
                }
            }
            if (statement is CodeThrowExceptionStatement)
            {
                MRules._ActionInvoke action = new MRules._ActionInvoke
                {
                    InvokeType = EmRules.InvokeType.MessageInvoke,
                    AssignType = EmRules.AssignType.InvokeAssign
                };

                CodeExpression exp = ((CodeThrowExceptionStatement)statement).ToThrow;
                if (exp is CodeObjectCreateExpression)
                {
                    CodeObjectCreateExpression create = (CodeObjectCreateExpression)exp;
                    exp = create.Parameters[0];
                }

                action.Expression = FillExpression(exp);
                return action;
            }
            if (statement is CodeExpressionStatement)
            {
                CodeExpression codeExpression = ((CodeExpressionStatement)statement).Expression;
                if (codeExpression is CodeMethodInvokeExpression)
                {
                    CodeMethodInvokeExpression invokeExpression = (CodeMethodInvokeExpression)codeExpression;
                    object paramExpression = FillExpression(invokeExpression.Parameters[0]);

                    MRules._ActionInvoke actionInvoke = new MRules._ActionInvoke();
                    switch (invokeExpression.Method.MethodName)
                    {
                        case "FireRules":
                            try
                            {
                                actionInvoke.AssignType = EmRules.AssignType.InvokeAssign;
                                actionInvoke.InvokeType = EmRules.InvokeType.RuleSetInvoke;
                                actionInvoke.Expression = _ruleSetDao.GetRuleSetByIdRuleSet((int)paramExpression, false);
                            }
                            catch (Exception)
                            {
                                actionInvoke.HasError = true;
                                actionInvoke.ErrorDescription = "Error al invocar accion";
                            }
                            return actionInvoke;

                        case "ExecuteFunction":
                            try
                            {
                                actionInvoke.AssignType = EmRules.AssignType.InvokeAssign;
                                actionInvoke.InvokeType = EmRules.InvokeType.FunctionInvoke;
                                actionInvoke.Expression = _ruleFunctionDao.GetRuleFunctionByNameRuleFunction((string)paramExpression);
                                return actionInvoke;
                            }
                            catch (Exception)
                            {
                                actionInvoke.HasError = true;
                                actionInvoke.ErrorDescription = "Error al invocar accion";
                            }
                            return actionInvoke;

                        case "SetDynamicConcept":
                            CodeExpression castExpression = ((CodeCastExpression)invokeExpression.Parameters[1]).Expression;
                            MRules._ActionConcept actionConcept = new MRules._ActionConcept
                            {
                                AssignType = EmRules.AssignType.ConceptAssign,
                                Concept = FillConcept(invokeExpression)
                            };

                            FillExpressionAction(castExpression, ref actionConcept);

                            if (actionConcept.Expression != null && actionConcept.ComparatorType == EmRules.ComparatorType.ConstantValue)
                            {
                                dynamic valueExpression = actionConcept.Expression;

                                if (actionConcept.Expression.GetType().Name == "DateTime")
                                {
                                    actionConcept.Expression = Convert.ToDateTime(valueExpression).ToString("dd/MM/yyyy");
                                }

                                try
                                {
                                    switch (((MRules._Concept)actionConcept.Concept).ConceptType)
                                    {
                                        case EmRules.ConceptType.Range:

                                            MRules._RangeConcept rangeConcept = _conceptsDao.GetRangeConceptByIdConceptIdEntity(((MRules._Concept)actionConcept.Concept).ConceptId, ((MRules._Concept)actionConcept.Concept).Entity.EntityId);
                                            actionConcept.Concept = rangeConcept;
                                            actionConcept.Expression = rangeConcept.RangeEntity.RangeEntityValues.First(x => x.RangeValueCode == (int)valueExpression);
                                            break;

                                        case EmRules.ConceptType.List:

                                            if (valueExpression.GetType().Name == "Boolean")
                                            {
                                                MRules._ListConcept listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(((MRules._Concept)actionConcept.Concept).ConceptId, ((MRules._Concept)actionConcept.Concept).Entity.EntityId);
                                                actionConcept.Concept = listConcept;
                                                actionConcept.Expression = listConcept.ListEntity.ListEntityValues
                                                    .First(x => Convert.ToBoolean(x.ListValueCode) == Convert.ToBoolean(valueExpression));
                                            }
                                            else
                                            {
                                                MRules._ListConcept listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(((MRules._Concept)actionConcept.Concept).ConceptId, ((MRules._Concept)actionConcept.Concept).Entity.EntityId);
                                                actionConcept.Concept = listConcept;
                                                actionConcept.Expression = listConcept.ListEntity.ListEntityValues
                                                    .First(x => x.ListValueCode == (int)valueExpression);
                                            }
                                            break;

                                        case EmRules.ConceptType.Reference:

                                            MRules._ReferenceConcept referenceConcept = _conceptsDao.GetReferenceConceptByIdConceptIdEntity(((MRules._Concept)actionConcept.Concept).ConceptId, ((MRules._Concept)actionConcept.Concept).Entity.EntityId, new string[0]);
                                            actionConcept.Concept = referenceConcept;
                                            actionConcept.Expression = referenceConcept.ReferenceValues.First(x => x.Id == valueExpression.ToString());
                                            break;
                                    }
                                }
                                catch (Exception)
                                {
                                    actionConcept.HasError = true;
                                    actionConcept.ErrorDescription = "Error al evaluar la accion de la regla";
                                }
                            }

                            return actionConcept;

                        default:
                            throw new Exception("Definicion de la Accion no es reconocida: " + invokeExpression.Method.MethodName);
                    }
                }
                throw new Exception("Definicion de la Accion no es reconocida");
            }
            throw new Exception("Definicion de la Accion no es reconocida");
        }

        #region FillCondition

        public static MRules._Condition FillCondition(CodeBinaryOperatorExpression expression, List<MRules._Condition> lastConditions)
        {
            List<string> dependency = new List<string>();
            MRules._Condition condition = new MRules._Condition();

            CodeExpression left = expression.Left;
            if (left is CodePropertyReferenceExpression)
            {
                condition.Concept = FillConcept((CodePropertyReferenceExpression)left);
                foreach (MRules._ConceptDependence dependence in ((MRules._Concept)condition.Concept).ConceptDependences)
                {
                    MRules._Condition conditionParent = lastConditions.FirstOrDefault(x =>
                        ((MRules._Concept)x.Concept).ConceptId == dependence.DependsConcept.ConceptId &&
                        ((MRules._Concept)x.Concept).Entity.EntityId == dependence.DependsConcept.Entity.EntityId);
                    if (conditionParent != null)
                    {
                        dependency.Add(((MRules._ReferenceValue)conditionParent.Expression).Id);
                    }
                }
            }
            else if (left is CodeMethodInvokeExpression)
            {
                CodeMethodInvokeExpression methodInvokeExpression = (CodeMethodInvokeExpression)left;
                if (methodInvokeExpression.Method.MethodName.Equals("GetDynamicConcept"))
                {
                    condition.Concept = FillConcept(methodInvokeExpression);
                }
                else
                {
                    throw new ApplicationException("Condicion no reconocida.");
                }
            }
            else if ((left as CodeCastExpression)?.Expression is CodeMethodInvokeExpression)
            {
                CodeCastExpression cast = (CodeCastExpression)left;

                if (cast.Expression is CodeMethodInvokeExpression)
                {
                    CodeMethodInvokeExpression mi = (CodeMethodInvokeExpression)cast.Expression;
                    if (mi.Method.MethodName.Equals("GetDynamicConcept"))
                    {
                        condition.Concept = FillConcept(mi);
                    }
                    else
                    {
                        throw new ApplicationException("Condicion no reconocida.");
                    }
                }
                else
                {
                    throw new ApplicationException("Condicion no reconocida.");
                }
            }

            condition.Comparator = FillComparator(expression.Operator);

            CodeExpression right = expression.Right;
            CodeCastExpression castExpr = right as CodeCastExpression;
            if (castExpr != null)
            {
                right = castExpr.Expression;
            }

            if (right is CodePrimitiveExpression)
            {
                condition.ComparatorType = EmRules.ComparatorType.ConstantValue;
            }
            else if (right is CodePropertyReferenceExpression)
            {
                condition.ComparatorType = EmRules.ComparatorType.ConceptValue;
            }
            else if (right is CodeMethodInvokeExpression || (right as CodeCastExpression)?.Expression is CodeMethodInvokeExpression)
            {
                condition.ComparatorType = EmRules.ComparatorType.ConceptValue;
            }
            else if (right is CodeBinaryOperatorExpression)
            {
                condition.ComparatorType = EmRules.ComparatorType.ExpressionValue;
            }
            else
            {
                throw new ApplicationException("Tipo de valor no reconocido.");
            }

            condition.Expression = FillExpression(right);

            if (condition.Expression != null && condition.ComparatorType == EmRules.ComparatorType.ConstantValue)
            {
                dynamic valueExpression = condition.Expression;

                if (condition.Expression.GetType().Name == "DateTime")
                {
                    condition.Expression = Convert.ToDateTime(valueExpression).ToString("dd/MM/yyyy");
                }

                try
                {
                    switch (((MRules._Concept)condition.Concept).ConceptType)
                    {
                        case EmRules.ConceptType.Range:

                            MRules._RangeConcept rangeConcept = _conceptsDao.GetRangeConceptByIdConceptIdEntity(((MRules._Concept)condition.Concept).ConceptId, ((MRules._Concept)condition.Concept).Entity.EntityId);
                            rangeConcept.ConceptDependences = ((MRules._Concept)condition.Concept).ConceptDependences;
                            condition.Concept = rangeConcept;
                            condition.Expression = rangeConcept.RangeEntity.RangeEntityValues.First(x => x.RangeValueCode == Convert.ToInt32(valueExpression));
                            break;

                        case EmRules.ConceptType.List:
                            if (valueExpression.GetType().Name == "Boolean")
                            {
                                MRules._ListConcept listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(((MRules._Concept)condition.Concept).ConceptId, ((MRules._Concept)condition.Concept).Entity.EntityId);
                                listConcept.ConceptDependences = ((MRules._Concept)condition.Concept).ConceptDependences;
                                condition.Concept = listConcept;
                                condition.Expression = listConcept.ListEntity.ListEntityValues.First(x => Convert.ToBoolean(x.ListValueCode) == Convert.ToBoolean(valueExpression));
                            }
                            else
                            {

                                MRules._ListConcept listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(((MRules._Concept)condition.Concept).ConceptId, ((MRules._Concept)condition.Concept).Entity.EntityId);
                                listConcept.ConceptDependences = ((MRules._Concept)condition.Concept).ConceptDependences;
                                condition.Concept = listConcept;
                                condition.Expression = listConcept.ListEntity.ListEntityValues.First(x => x.ListValueCode == Convert.ToInt32(valueExpression));
                            }
                            break;

                        case EmRules.ConceptType.Reference:
                            MRules._ReferenceConcept referenceConcept = _conceptsDao.GetReferenceConceptByIdConceptIdEntity(((MRules._Concept)condition.Concept).ConceptId, ((MRules._Concept)condition.Concept).Entity.EntityId, dependency.ToArray());
                            referenceConcept.ConceptDependences = ((MRules._Concept)condition.Concept).ConceptDependences;
                            condition.Concept = referenceConcept;
                            condition.Expression = referenceConcept.ReferenceValues.First(x => x.Id == valueExpression.ToString());

                            break;
                    }
                }
                catch (Exception)
                {
                    condition.ErrorDescription = "Error al evaluar la condición de la regla";
                    condition.HasError = true;
                }
            }

            return condition;
        }

        #endregion

        #region FillExpression

        private static object FillExpression(CodeExpression expression)
        {
            if (expression is CodePrimitiveExpression)
            {
                return ((CodePrimitiveExpression)expression).Value;
            }
            if (expression is CodePropertyReferenceExpression)
            {
                return FillConcept((CodePropertyReferenceExpression)expression);
            }
            if (expression is CodeMethodInvokeExpression)
            {
                return FillConcept((CodeMethodInvokeExpression)expression);
            }
            if (expression is CodeCastExpression)
            {
                return FillExpression(((CodeCastExpression)expression).Expression);
            }
            if (expression is CodeBinaryOperatorExpression)
            {
                CodeBinaryOperatorExpression bexp = (CodeBinaryOperatorExpression)expression;

                string newExpression = string.Empty;

                object left = FillExpression(bexp.Left);
                object right = FillExpression(bexp.Right);

                string leftValue = left is MRules._Concept ? "[" + ((MRules._Concept)left).Description + "]" : left.ToString();
                string rightValue = right is MRules._Concept ? "[" + ((MRules._Concept)right).Description + "]" : right.ToString();

                switch (bexp.Operator)
                {
                    case CodeBinaryOperatorType.Add:
                        newExpression = "(" + leftValue + " + " + rightValue + ")";
                        break;

                    case CodeBinaryOperatorType.Subtract:
                        newExpression = "(" + leftValue + " - " + rightValue + ")";
                        break;

                    case CodeBinaryOperatorType.Multiply:
                        newExpression = "(" + leftValue + " * " + rightValue + ")";
                        break;

                    case CodeBinaryOperatorType.Divide:
                        newExpression = "(" + leftValue + " / " + rightValue + ")";
                        break;
                }
                return newExpression;
            }
            if (expression is CodeIndexerExpression)
            {
                return FillExpression(((CodeIndexerExpression)expression).Indices[0]);
            }
            throw new Exception("Definicion de la expresion no es reconocida");
        }

        #endregion

        #region FillComparator

        private static MRules._Comparator FillComparator(CodeBinaryOperatorType @operator)
        {
            MRules._Comparator comparator = new MRules._Comparator();

            switch (@operator)
            {
                case CodeBinaryOperatorType.IdentityInequality:
                    comparator.Operator = EmRules.OperatorConditionType.Distinct;
                    break;
                case CodeBinaryOperatorType.IdentityEquality:
                    comparator.Operator = EmRules.OperatorConditionType.Equals;
                    break;
                case CodeBinaryOperatorType.LessThan:
                    comparator.Operator = EmRules.OperatorConditionType.LessThan;
                    break;
                case CodeBinaryOperatorType.LessThanOrEqual:
                    comparator.Operator = EmRules.OperatorConditionType.LessThanOrEquals;
                    break;
                case CodeBinaryOperatorType.GreaterThan:
                    comparator.Operator = EmRules.OperatorConditionType.GreaterThan;
                    break;
                case CodeBinaryOperatorType.GreaterThanOrEqual:
                    comparator.Operator = EmRules.OperatorConditionType.GreaterThanOrEquals;
                    break;
                default:
                    throw new Exception("Comparador de la condicion no es valido");
            }

            comparator.Description = _resx.GetString(Enum.GetName(typeof(EmRules.OperatorConditionType), comparator.Operator));
            comparator.Symbol = EnumHelper.GetValueMember<EnumMemberAttribute, EmRules.OperatorConditionType>(comparator.Operator).Value;
            return comparator;
        }

        #endregion

        #region Fill Concepts

        private static MRules._Concept FillConcept(CodeMethodInvokeExpression left)
        {
            object value = ((CodePrimitiveExpression)left.Parameters[0]).Value;
            if (value is string)
            {
                return FillConcept((string)value, ((CodeArgumentReferenceExpression)((CodeCastExpression)left.Method.TargetObject).Expression).ParameterName);
            }

            if (value is int)
            {
                return FillConcept((int)value, ((CodeArgumentReferenceExpression)((CodeCastExpression)left.Method.TargetObject).Expression).ParameterName);
            }

            throw new Exception("Definicion de concepto no reconocido: " + string.Format(CultureInfo.InvariantCulture, "{0}", value));
        }

        private static MRules._Concept FillConcept(CodePropertyReferenceExpression left)
        {
            return FillConcept(left.PropertyName, ((CodeArgumentReferenceExpression)left.TargetObject).ParameterName);
        }

        private static MRules._Concept FillConcept(string conceptName, string objectName)
        {
            if (objectName.Contains("prm"))
            {
                objectName = string.Concat("RULE_", string.Concat(objectName.Substring(3).Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper());
            }

            int idEntity = int.Parse(Utilities.Helper.EnumHelper.GetEnumParameterValue((FacadeType)Enum.Parse(typeof(FacadeType), objectName)).ToString());


            MRules._Concept conceptTmp = _conceptsDao.GetConceptByFilter(new List<int> { idEntity }, conceptName).First();
            switch (conceptTmp.ConceptType)
            {
                case EmRules.ConceptType.Basic:
                    return _conceptsDao.GetBasicConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);

                case EmRules.ConceptType.Range:
                    return _conceptsDao.GetRangeConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);

                case EmRules.ConceptType.List:
                    return _conceptsDao.GetListConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);

                case EmRules.ConceptType.Reference:
                    MRules._Concept conceptReference = _conceptsDao.GetReferenceConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId, new string[] { });
                    conceptReference.ConceptDependences = conceptTmp.ConceptDependences;
                    return conceptReference;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static MRules._Concept FillConcept(int conceptId, string objectName)
        {
            if (objectName.Contains("prm"))
            {
                objectName = string.Concat("RULE_", string.Concat(objectName.Substring(3).Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper());
            }

            int idEntity = int.Parse(Utilities.Helper.EnumHelper.GetEnumParameterValue((FacadeType)Enum.Parse(typeof(FacadeType), objectName)).ToString());
            MRules._Concept conceptTmp = _conceptsDao.GetConceptByIdConceptIdEntity(conceptId, idEntity);
            switch (conceptTmp.ConceptType)
            {
                case EmRules.ConceptType.Basic:
                    return _conceptsDao.GetBasicConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);

                case EmRules.ConceptType.Range:
                    return _conceptsDao.GetRangeConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);

                case EmRules.ConceptType.List:
                    return _conceptsDao.GetListConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId);

                case EmRules.ConceptType.Reference:
                    return _conceptsDao.GetReferenceConceptByIdConceptIdEntity(conceptTmp.ConceptId, conceptTmp.Entity.EntityId, new string[] { });

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region Fills Parameters

        private static List<MRules._Parameter> FillParameters(XPathNodeIterator xPathNodeIterator)
        {
            List<MRules._Parameter> ruleParameters = new List<MRules._Parameter>();

            while (xPathNodeIterator.MoveNext())
            {
                XPathNavigator xPathNavigator = xPathNodeIterator.Current;

                XPathNodeIterator iteClass = xPathNavigator.SelectChildren("class", _namespace);
                if (!iteClass.MoveNext())
                {
                    throw new Exception("Falta el tipo del parametro.");
                }

                string typeAttribute = iteClass.Current.Value.Split(',')[0];
                string nameAttribute = xPathNavigator.GetAttribute("name", _attNamespace);

                ruleParameters.Add(new MRules._Parameter { Type = typeAttribute, Name = nameAttribute });
            }

            return ruleParameters;
        }

        #endregion

        private static CodeStatement[] LoadStatements(XPathNavigator nav)
        {
            ArrayList stmts = new ArrayList();
            XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
            while (ite.MoveNext())
            {
                stmts.Add(LoadStatement(ite.Current));
            }

            if (stmts.Count == 0)
            {
                return null;
            }

            return (CodeStatement[])stmts.ToArray(typeof(CodeStatement));
        }

        private static CodeStatement LoadStatement(XPathNavigator nav)
        {
            string name = nav.Name;
            switch (name)
            {
                case "assign":
                    {
                        XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                        if (ite.Count != 2)
                        {
                            throw new Exception("Una asignación debe contener 2 expresiones.");
                        }

                        ite.MoveNext();
                        CodeExpression exp1 = LoadExpression(ite.Current);
                        ite.MoveNext();
                        CodeExpression exp2 = LoadExpression(ite.Current);

                        return new CodeAssignStatement(exp1, exp2);
                    }

                case "if":
                    {
                        XPathNodeIterator ite = nav.SelectChildren("condition", _namespace);
                        if (!ite.MoveNext())
                        {
                            throw new Exception("Falta la condición para el condicional.");
                        }

                        ite = ite.Current.SelectChildren(XPathNodeType.Element);
                        if (!ite.MoveNext())
                        {
                            throw new Exception("Falta la condición para el condicional.");
                        }

                        CodeExpression cond = LoadExpression(ite.Current);

                        ite = nav.SelectChildren("true", _namespace);
                        CodeStatement[] trues = null;
                        if (ite.MoveNext())
                        {
                            trues = LoadStatements(ite.Current);
                        }

                        ite = nav.SelectChildren("false", _namespace);
                        CodeStatement[] falses = null;
                        if (ite.MoveNext())
                        {
                            falses = LoadStatements(ite.Current);
                        }

                        if ((trues == null || trues.Length == 0) && (falses == null || falses.Length == 0))
                        {
                            throw new Exception("El condicional debe tener al menos una parte verdadera o una falsa.");
                        }

                        return new CodeConditionStatement(cond, trues, falses);
                    }

                case "invoke":
                    {
                        return new CodeExpressionStatement(LoadExpression(nav));
                    }

                case "throw":
                    {
                        XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                        if (!ite.MoveNext())
                        {
                            throw new Exception("Falta el objeto del throw.");
                        }

                        return new CodeThrowExceptionStatement(LoadExpression(ite.Current));
                    }

                case "iteration":
                    {
                        CodeStatement init = null, inc = null;
                        CodeExpression test = null;

                        XPathNodeIterator ite = nav.SelectChildren("init", _namespace);
                        if (ite.MoveNext())
                        {
                            ite = ite.Current.SelectChildren(XPathNodeType.Element);
                            if (ite.MoveNext())
                            {
                                init = LoadStatement(ite.Current);
                            }
                        }

                        ite = nav.SelectChildren("inc", _namespace);
                        if (ite.MoveNext())
                        {
                            ite = ite.Current.SelectChildren(XPathNodeType.Element);
                            if (ite.MoveNext())
                            {
                                inc = LoadStatement(ite.Current);
                            }
                        }

                        ite = nav.SelectChildren("test", _namespace);
                        if (ite.MoveNext())
                        {
                            ite = ite.Current.SelectChildren(XPathNodeType.Element);
                            if (ite.MoveNext())
                            {
                                test = LoadExpression(ite.Current);
                            }
                        }

                        CodeStatement[] loop = null;
                        ite = nav.SelectChildren("loop", _namespace);
                        if (ite.MoveNext())
                        {
                            ite = ite.Current.SelectChildren(XPathNodeType.Element);
                            loop = new CodeStatement[ite.Count];
                            int i = 0;
                            while (ite.MoveNext())
                            {
                                loop[i] = LoadStatement(ite.Current);
                                i++;
                            }
                        }

                        return new CodeIterationStatement(init, test, inc, loop);
                    }

                case "code":
                    {
                        return new CodeSnippetStatement(nav.Value.Trim());
                    }
                default:
                    {
                        throw new Exception("No se reconoció el elemento de sentencia.");
                    }
            }
        }

        private static CodeExpression LoadExpression(XPathNavigator xPathNavigator)
        {
            string typeAttribute = xPathNavigator.GetAttribute("type", _attNamespace);
            string valueAttribute = xPathNavigator.GetAttribute("value", _attNamespace);
            string nameAttribute = xPathNavigator.GetAttribute("name", _attNamespace);
            string methodAttribute = xPathNavigator.GetAttribute("method", _attNamespace);

            switch (xPathNavigator.Name)
            {
                case "code":
                    {
                        return new CodeSnippetExpression { Value = xPathNavigator.Value.Trim() };
                    }

                case "binary-op":
                    {

                        if (string.IsNullOrEmpty(typeAttribute))
                        {
                            throw new Exception("No se definió el tipo para el operador binario.");
                        }

                        XPathNodeIterator ite = xPathNavigator.SelectChildren(XPathNodeType.Element);
                        if (ite.Count != 2)
                        {
                            throw new Exception("El operador binario debe contener 2 expresiones.");
                        }

                        ite.MoveNext();
                        CodeExpression exp1 = LoadExpression(ite.Current);
                        ite.MoveNext();
                        CodeExpression exp2 = LoadExpression(ite.Current);
                        CodeBinaryOperatorType op =
                            (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), typeAttribute, true);

                        return new CodeBinaryOperatorExpression(exp1, op, exp2);
                    }

                case "this":
                    {
                        return new CodeThisReferenceExpression();
                    }

                case "argument":
                    {
                        if (string.IsNullOrEmpty(nameAttribute))
                        {
                            throw new Exception("Debe especificar el nombre para la referencia a parametro.");
                        }

                        return new CodeArgumentReferenceExpression(nameAttribute);
                    }

                case "primitive":
                    {

                        if (string.IsNullOrEmpty(valueAttribute))
                        {
                            return new CodePrimitiveExpression(null);
                        }
                        if (string.IsNullOrEmpty(typeAttribute))
                        {
                            throw new Exception("Debe especificar el tipo para el dato primitivo.");
                        }

                        System.Type type = GetPrimitiveType(typeAttribute);
                        return new CodePrimitiveExpression(Convert.ChangeType(valueAttribute, type,
                            System.Globalization.CultureInfo.InvariantCulture));
                    }

                case "property":
                    {
                        if (string.IsNullOrEmpty(nameAttribute))
                        {
                            throw new Exception("Debe especificar el nombre de la propiedad.");
                        }

                        XPathNodeIterator ite = xPathNavigator.SelectChildren(XPathNodeType.Element);
                        if (!ite.MoveNext())
                        {
                            throw new Exception("No se definió el objeto de la propiedad.");
                        }

                        CodeExpression exp = LoadExpression(ite.Current);
                        return new CodePropertyReferenceExpression(exp, nameAttribute);
                    }

                case "field":
                    {
                        if (string.IsNullOrEmpty(nameAttribute))
                        {
                            throw new Exception("Debe especificar el nombre para el atributo.");
                        }

                        XPathNodeIterator ite = xPathNavigator.SelectChildren(XPathNodeType.Element);
                        if (!ite.MoveNext())
                        {
                            return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), nameAttribute);
                        }

                        return new CodeFieldReferenceExpression(LoadExpression(ite.Current), nameAttribute);
                    }

                case "cast":
                    {
                        if (string.IsNullOrEmpty(typeAttribute))
                        {
                            throw new Exception("Debe especificar el tipo para el cast.");
                        }

                        XPathNodeIterator ite = xPathNavigator.SelectChildren(XPathNodeType.Element);

                        if (!ite.MoveNext())
                        {
                            throw new Exception("No se definió la expresión del cast.");
                        }

                        CodeExpression exp = LoadExpression(ite.Current);
                        return new CodeCastExpression(typeAttribute, exp);
                    }

                case "indexer":
                    {
                        XPathNodeIterator ite = xPathNavigator.SelectChildren("target", _namespace);
                        if (!ite.MoveNext())
                        {
                            throw new Exception("No se definió la expresión a indexar.");
                        }

                        if (!ite.Current.MoveToFirstChild())
                        {
                            throw new Exception("No se definió la expresión a indexar.");
                        }

                        CodeExpression exp = LoadExpression(ite.Current);

                        ite = xPathNavigator.SelectChildren("indices", _namespace);
                        if (!ite.MoveNext())
                        {
                            throw new Exception("No se definió ningún índice.");
                        }

                        if (!ite.Current.MoveToFirstChild())
                        {
                            throw new Exception("No se definió ningún índice.");
                        }

                        List<CodeExpression> indices = new List<CodeExpression>
                        {
                            LoadExpression(ite.Current)
                        };

                        while (ite.MoveNext())
                        {
                            indices.Add(LoadExpression(ite.Current));
                        }

                        return new CodeIndexerExpression(exp, indices.ToArray());
                    }

                case "invoke":
                    {
                        if (string.IsNullOrEmpty(methodAttribute))
                        {
                            throw new Exception("No se definió el nombre del método a invocar.");
                        }

                        XPathNodeIterator ite = xPathNavigator.SelectChildren("target", _namespace);
                        CodeExpression target = null;
                        if (ite.MoveNext())
                        {
                            ite = ite.Current.SelectChildren(XPathNodeType.Element);
                            if (ite.MoveNext())
                            {
                                target = LoadExpression(ite.Current);
                            }
                        }

                        if (target == null)
                        {
                            target = new CodeThisReferenceExpression();
                        }

                        CodeExpression[] pars = null;
                        ite = xPathNavigator.SelectChildren("params", _namespace);
                        if (ite.MoveNext())
                        {
                            ite = ite.Current.SelectChildren(XPathNodeType.Element);
                            pars = new CodeExpression[ite.Count];

                            int i = 0;
                            while (ite.MoveNext())
                            {
                                pars[i] = LoadExpression(ite.Current);
                                i++;
                            }
                        }

                        return new CodeMethodInvokeExpression(target, methodAttribute, pars);
                    }

                case "object-create":
                    {
                        if (string.IsNullOrEmpty(typeAttribute))
                        {
                            throw new Exception("Debe especificar el tipo del objeto a crear.");
                        }

                        XPathNodeIterator ite = xPathNavigator.SelectChildren(XPathNodeType.Element);
                        CodeExpression[] pars = new CodeExpression[ite.Count];

                        int i = 0;
                        while (ite.MoveNext())
                        {
                            pars[i] = LoadExpression(ite.Current);
                            i++;
                        }

                        return new CodeObjectCreateExpression(typeAttribute, pars);
                    }

                case "type-of":
                    {
                        if (string.IsNullOrEmpty(typeAttribute))
                        {
                            throw new Exception("Debe especificar el tipo.");
                        }

                        return new CodeTypeOfExpression(typeAttribute);
                    }


                case "type":
                    {
                        if (string.IsNullOrEmpty(nameAttribute))
                        {
                            throw new Exception("Debe especificar el nombre del tipo.");
                        }

                        return new CodeTypeReferenceExpression(nameAttribute);
                    }

                default:
                    {
                        throw new Exception("Expresión no reconocida (" + xPathNavigator.Name + ").");
                    }
            }
        }

        private static System.Type GetPrimitiveType(string typeAttribute)
        {
            switch (typeAttribute)
            {
                case "String":
                    {
                        return typeof(string);
                    }
                case "Integer":
                    {
                        return typeof(int);
                    }
                case "Boolean":
                    {
                        return typeof(bool);
                    }
                case "Decimal":
                    {
                        return typeof(decimal);
                    }
                case "DateTime":
                    {
                        return typeof(DateTime);
                    }
                default:
                    {
                        throw new Exception("No se encontró el tipo para el dato primitivo (" + typeAttribute + ").");
                    }
            }
        }
    }

    public class XmlHelperWriter
    {
        private readonly string _namespace = "http://www.sistran.com/RuleEngine/rules.xsd";
        private readonly string _attNamespace = string.Empty;

        private readonly _ConceptDao _conceptsDao = new _ConceptDao();
        internal readonly _RuleSetDao _ruleSetDao = new _RuleSetDao();
        internal readonly _RuleFunctionDao _ruleFunctionDao = new _RuleFunctionDao();
        private readonly _PositionEntityDao _positionEntityDao = new _PositionEntityDao();
        private readonly ExpressionParserHelper _expressionParserHelper = new ExpressionParserHelper();
        private readonly ConcurrentBag<MRules._Concept> concepts = new ConcurrentBag<MRules._Concept>();

        public async Task<byte[]> GetXmlByRuleSet(MRules._RuleSet ruleSet)
        {
            try
            {
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Async = true,
                    Encoding = Encoding.UTF8,
                    OmitXmlDeclaration = true,
                    Indent = true
                };

                using (MemoryStream ms = new MemoryStream())
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {
                    this.SaveRuleSet(writer, ruleSet);
                    await writer.FlushAsync();

                    int len = Convert.ToInt32(ms.Length);
                    byte[] bs = new byte[len];
                    ms.Position = 0;
                    ms.Read(bs, 0, len);

                    return bs;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error al obtener el Xml de la regla", ex);
            }
        }

        private void SaveRuleSet(XmlWriter writer, MRules._RuleSet ruleSet)
        {
            writer.WriteStartElement("rule-set", _namespace);
            writer.WriteAttributeString("name", ruleSet.Description);
            writer.WriteAttributeString("type", ruleSet.Type.ToString());

            List<Models.Entity> entities = _positionEntityDao.GetEntitiesByPackageIdLevelId(ruleSet.Package.PackageId, ruleSet.Level.LevelId);

            List<MRules._Parameter> parameters = new List<MRules._Parameter>();
            foreach (Models.Entity entity in entities)
            {
                FacadeType facadeType = USHELPER.EnumHelper.GetItemNameParameterValue<FacadeType>(entity.EntityId.ToString());
                string name = USHELPER.EnumHelper.GetDescription(facadeType);

                parameters.Add(new MRules._Parameter
                {
                    Type = "Sistran.Core.Application.Quotations.Entities." + name + ", Sistran.Core.Application.Quotations.Entities",
                    Name = "prm" + name
                });
            }

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Async = true,
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true,
                Indent = true
            };
            ConcurrentDictionary<int, string> concurrent = new ConcurrentDictionary<int, string>();

            int numProcess = Convert.ToInt32(ConfigurationManager.AppSettings["MaxProcessThreadRuleSet"]);
            int numThread = Math.Max(ruleSet.Rules.Count, numProcess) / numProcess;
            List<Task> threads = new List<Task>();

            for (int t = 0; t < numThread; t++)
            {
                int length = numProcess;
                int ii = t;

                Task thread = new Task(async () =>
                {
                    if (ii == numThread - 1)
                    {
                        length = ruleSet.Rules.Count - (numProcess * ii);
                    }


                    for (int index = (numProcess * ii); index < (numProcess * ii) + length; index++)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        using (XmlWriter writerLocal = XmlWriter.Create(ms, settings))
                        {
                            MRules._Rule rule = ruleSet.Rules[index];
                            rule.Parameters = parameters;
                            SaveRule(writerLocal, rule, index);
                            await writerLocal.FlushAsync();

                            int len = Convert.ToInt32(ms.Length);
                            byte[] buffer = new byte[len];

                            ms.Position = 0;
                            await ms.ReadAsync(buffer, 0, len);

                            string st = new UTF8Encoding().GetString(buffer, 3, len - 3);
                            concurrent.TryAdd(index, st);
                        }
                    }

                    DataFacadeManager.Dispose();
                });
                threads.Add(thread);
            }

            //Se procesan los hilos
            int threadRuleSet = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadRuleSet"]);
            while (threads.Any(x => x.Status == TaskStatus.Created))
            {
                List<Task> threadsRun = threads.Where(x => x.Status == TaskStatus.Created).Skip(0).Take(threadRuleSet).ToList();
                threadsRun.ForEach(x => x.Start());
                Task.WaitAll(threadsRun.ToArray());
            }

            concurrent.OrderBy(x => x.Key).ToList().ForEach(async x => { await writer.WriteRawAsync(x.Value); });
            writer.WriteEndElement(); // rule-set
        }

        private void SaveRule(XmlWriter writer, MRules._Rule rule, int index)
        {
            writer.WriteStartElement("rule");

            if (rule.Description == "r_" + rule.RuleId)
            {
                writer.WriteAttributeString("name", "r_" + index);
            }
            else
            {
                writer.WriteAttributeString("name", rule.Description);
            }

            foreach (MRules._Parameter parameter in rule.Parameters)
            {
                SaveParameter(writer, parameter);
            }

            foreach (MRules._Condition cond in rule.Conditions)
            {
                if (cond.Comparator != null)
                {
                    writer.WriteStartElement("condition");
                    SaveCondition(writer, FillCondition(cond));
                    writer.WriteEndElement(); // condition
                }
            }

            writer.WriteStartElement("consequence");

            SaveAction(writer, FillActions(rule.Actions));

            writer.WriteEndElement(); // consequence
            writer.WriteEndElement(); // rule
        }

        private CodeStatementCollection FillActions(List<MRules._Action> actions)
        {
            CodeStatementCollection collection = new CodeStatementCollection();
            foreach (MRules._Action action in actions)
            {

                if (action is MRules._ActionConcept)
                {
                    if ((action as MRules._ActionConcept).ArithmeticOperator == null)
                    {
                        continue;
                    }
                }

                CodeObject actionFill = FillAction(action);
                if (actionFill is CodeStatement)
                {
                    collection.Add(actionFill as CodeStatement);
                }
                else if (actionFill is CodeExpression)
                {
                    collection.Add(actionFill as CodeExpression);
                }
            }
            return collection;
        }

        private CodeObject FillAction(dynamic action)
        {
            switch (action.AssignType)
            {
                case EmRules.AssignType.ConceptAssign:
                    dynamic leftAssign = XmlConcept(action.Concept, null, true);
                    CodeExpression rightAssign;

                    dynamic conceptTmp;
                    if (action.Concept.ConceptType == EmRules.ConceptType.Basic)
                    {

                        if (this.concepts.Any(x => x.ConceptId == action.Concept.ConceptId && x.Entity.EntityId == action.Concept.Entity.EntityId))
                        {
                            conceptTmp = this.concepts.First(x => x.ConceptId == action.Concept.ConceptId && x.Entity.EntityId == action.Concept.Entity.EntityId) as MRules._BasicConcept;
                        }
                        else
                        {
                            conceptTmp = _conceptsDao.GetBasicConceptByIdConceptIdEntity(action.Concept.ConceptId, action.Concept.Entity.EntityId);
                            this.concepts.Add(conceptTmp);
                        }
                    }
                    else
                    {
                        conceptTmp = (MRules._Concept)action.Concept;
                    }

                    string typeDataConcept = GetTypeDataConcept(conceptTmp);

                    if (conceptTmp.IsStatic)
                    {
                        switch (((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType)
                        {
                            case EmRules.ArithmeticOperatorType.Assign:
                                switch ((EmRules.ComparatorType)action.ComparatorType)
                                {
                                    case EmRules.ComparatorType.ConstantValue:
                                        rightAssign = new CodePrimitiveExpression();
                                        if (typeDataConcept.Contains("int"))
                                        {
                                            if (action.Expression is MRules._ReferenceValue)
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression.Id);
                                            }
                                            else if (action.Expression is MRules._ListEntityValue)
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression.ListValueCode);
                                            }
                                            else if (action.Expression is MRules._RangeEntityValue)
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression.RangeValueCode);
                                            }
                                            else
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression);
                                            }
                                        }
                                        else if (typeDataConcept.Contains("decimal"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = ConvertHelper.ConvertToDecimal(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("DateTime"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = DateTime.ParseExact(action.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else if (typeDataConcept.Contains("String"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = Convert.ToString(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("bool"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = Convert.ToBoolean(action.Expression.ListValueCode);
                                        }
                                        break;

                                    case EmRules.ComparatorType.ConceptValue:
                                        rightAssign = XmlConcept(action.Expression, null, true);
                                        break;

                                    case EmRules.ComparatorType.ExpressionValue:
                                        rightAssign = XmlExpression(_expressionParserHelper.GetExpression(action.Expression));
                                        break;
                                    case EmRules.ComparatorType.TemporalyValue:
                                        rightAssign = new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression));
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;

                            case EmRules.ArithmeticOperatorType.Add:
                            case EmRules.ArithmeticOperatorType.Subtract:
                            case EmRules.ArithmeticOperatorType.Multiply:
                            case EmRules.ArithmeticOperatorType.Divide:

                                CodeExpression binaryright;
                                CodeBinaryOperatorType @operator = (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), action.ArithmeticOperator.ArithmeticOperatorType.ToString());

                                switch ((EmRules.ComparatorType)action.ComparatorType)
                                {
                                    case EmRules.ComparatorType.ConstantValue:
                                        binaryright = new CodePrimitiveExpression();
                                        if (typeDataConcept.Contains("int"))
                                        {
                                            if (action.Expression is MRules._ReferenceValue)
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression.Id);
                                            }
                                            else if (action.Expression is MRules._ListEntityValue)
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression.ListValueCode);
                                            }
                                            else if (action.Expression is MRules._RangeEntityValue)
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression.RangeValueCode);
                                            }
                                            else
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression);
                                            }
                                        }
                                        else if (typeDataConcept.Contains("decimal"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = ConvertHelper.ConvertToDecimal(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("DateTime"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = DateTime.ParseExact(action.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else if (typeDataConcept.Contains("String"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = Convert.ToString(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("bool"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = Convert.ToBoolean(action.Expression);
                                        }
                                        break;
                                    case EmRules.ComparatorType.ConceptValue:
                                        binaryright = XmlConcept(action.Expression, null, true);
                                        break;
                                    case EmRules.ComparatorType.ExpressionValue:
                                        binaryright = XmlExpression(_expressionParserHelper.GetExpression(action.Expression));
                                        break;
                                    case EmRules.ComparatorType.TemporalyValue:
                                        binaryright = new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression));
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                rightAssign = new CodeBinaryOperatorExpression(leftAssign, @operator, binaryright);
                                break;

                            case EmRules.ArithmeticOperatorType.Round:
                                CodeTypeReferenceExpression targetConstant = new CodeTypeReferenceExpression(typeof(BusinessMath).FullName);
                                CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(targetConstant, "Round");
                                CodeCastExpression @params1 = new CodeCastExpression("System.Decimal", leftAssign);
                                CodeExpression @params2;

                                switch ((EmRules.ComparatorType)action.ComparatorType)
                                {
                                    case EmRules.ComparatorType.ConstantValue:
                                        if (typeDataConcept.Contains("int"))
                                        {
                                            if (action.Expression is MRules._ReferenceValue)
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression.Id)));
                                            }
                                            else if (action.Expression is MRules._ListEntityValue)
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression.ListValueCode)));
                                            }
                                            else if (action.Expression is MRules._RangeEntityValue)
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression.RangeValueCode)));
                                            }
                                            else
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression)));
                                            }
                                        }
                                        else if (typeDataConcept.Contains("decimal"))
                                        {
                                            @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(action.Expression)));
                                        }
                                        else if (typeDataConcept.Contains("DateTime"))
                                        {
                                            @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(DateTime.ParseExact(action.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
                                        }
                                        else if (typeDataConcept.Contains("String"))
                                        {
                                            @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToString(action.Expression)));
                                        }
                                        else if (typeDataConcept.Contains("bool"))
                                        {
                                            @params2 = new CodeCastExpression("System.Boolean", new CodePrimitiveExpression(Convert.ToBoolean(action.Expression)));
                                        }
                                        else
                                        {
                                            throw new ArgumentOutOfRangeException();
                                        }
                                        break;

                                    case EmRules.ComparatorType.ConceptValue:
                                        @params2 = new CodeCastExpression("System.Int32", XmlConcept(action.Expression, null, true));
                                        break;

                                    case EmRules.ComparatorType.ExpressionValue:
                                        @params2 = new CodeCastExpression("System.Int32", XmlExpression(_expressionParserHelper.GetExpression(action.Expression)));
                                        break;

                                    case EmRules.ComparatorType.TemporalyValue:
                                        @params2 = new CodeCastExpression("System.Int32", new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression)));
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                rightAssign = new CodeMethodInvokeExpression(method, @params1, @params2);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        if (typeDataConcept.Contains("int"))
                        {
                            if (((MRules._Concept)conceptTmp).ConceptType == EmRules.ConceptType.Range)
                            {
                                typeDataConcept = typeof(int).FullName;
                            }
                            if (((MRules._Concept)conceptTmp).ConceptType == EmRules.ConceptType.List)
                            {
                                typeDataConcept = typeof(int).FullName;
                            }
                            if (((MRules._Concept)conceptTmp).ConceptType == EmRules.ConceptType.Reference)
                            {
                                typeDataConcept = typeof(int).FullName;
                            }
                            else if (((MRules._Concept)conceptTmp).ConceptType == EmRules.ConceptType.Basic)
                            {
                                typeDataConcept = typeof(int).FullName;
                            }
                        }
                        else if (typeDataConcept.Contains("decimal"))
                        {
                        }
                        else if (typeDataConcept.Contains("DateTime"))
                        {
                            typeDataConcept = typeof(DateTime).FullName;
                        }
                        else if (typeDataConcept.Contains("String"))
                        {
                        }
                        else if (typeDataConcept == "bool")
                        {
                            typeDataConcept = typeof(bool).FullName;
                        }

                        rightAssign = new CodeCastExpression(typeDataConcept, rightAssign);
                        return new CodeAssignStatement(leftAssign, rightAssign);
                    }
                    else
                    {
                        CodeExpression invokeSet = ((CodeCastExpression)XmlConcept(conceptTmp, null, false)).Expression;
                        dynamic invokeGet = XmlConcept(conceptTmp, null, true);

                        switch (((MRules._ActionConcept)action).ArithmeticOperator.ArithmeticOperatorType)
                        {
                            case EmRules.ArithmeticOperatorType.Assign:
                                switch ((EmRules.ComparatorType)action.ComparatorType)
                                {
                                    case EmRules.ComparatorType.ConstantValue:
                                        rightAssign = new CodePrimitiveExpression();
                                        if (typeDataConcept.Contains("int"))
                                        {
                                            if (action.Expression is MRules._ReferenceValue)
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression.Id);
                                            }
                                            else if (action.Expression is MRules._ListEntityValue)
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression.ListValueCode);
                                            }
                                            else if (action.Expression is MRules._RangeEntityValue)
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression.RangeValueCode);
                                            }
                                            else
                                            {
                                                ((CodePrimitiveExpression)rightAssign).Value = Convert.ToInt32(action.Expression);
                                            }
                                        }
                                        else if (typeDataConcept.Contains("decimal"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = ConvertHelper.ConvertToDecimal(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("DateTime"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = DateTime.ParseExact(action.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else if (typeDataConcept.Contains("String"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = Convert.ToString(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("bool"))
                                        {
                                            ((CodePrimitiveExpression)rightAssign).Value = Convert.ToBoolean(action.Expression.ListValueCode);
                                        }
                                        break;

                                    case EmRules.ComparatorType.ConceptValue:
                                        rightAssign = XmlConcept(action.Expression, null, true);
                                        break;

                                    case EmRules.ComparatorType.ExpressionValue:
                                        rightAssign = XmlExpression(_expressionParserHelper.GetExpression(action.Expression));
                                        break;

                                    case EmRules.ComparatorType.TemporalyValue:
                                        rightAssign = new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression));
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                rightAssign = new CodeCastExpression(typeDataConcept, rightAssign);
                                break;

                            case EmRules.ArithmeticOperatorType.Add:
                            case EmRules.ArithmeticOperatorType.Subtract:
                            case EmRules.ArithmeticOperatorType.Multiply:
                            case EmRules.ArithmeticOperatorType.Divide:

                                CodeExpression binaryright;
                                CodeBinaryOperatorType @operator = (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), action.ArithmeticOperator.ArithmeticOperatorType.ToString());

                                switch ((EmRules.ComparatorType)action.ComparatorType)
                                {
                                    case EmRules.ComparatorType.ConstantValue:
                                        binaryright = new CodePrimitiveExpression();
                                        if (typeDataConcept.Contains("int"))
                                        {
                                            if (action.Expression is MRules._ReferenceValue)
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression.Id);
                                            }
                                            else if (action.Expression is MRules._ListEntityValue)
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression.ListValueCode);
                                            }
                                            else if (action.Expression is MRules._RangeEntityValue)
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression.RangeValueCode);
                                            }
                                            else
                                            {
                                                ((CodePrimitiveExpression)binaryright).Value = Convert.ToInt32(action.Expression);
                                            }
                                        }
                                        else if (typeDataConcept.Contains("decimal"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = ConvertHelper.ConvertToDecimal(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("DateTime"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = DateTime.ParseExact(action.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                        }
                                        else if (typeDataConcept.Contains("String"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = Convert.ToString(action.Expression);
                                        }
                                        else if (typeDataConcept.Contains("bool"))
                                        {
                                            ((CodePrimitiveExpression)binaryright).Value = Convert.ToBoolean(action.Expression.ListValueCode);
                                        }

                                        break;
                                    case EmRules.ComparatorType.ConceptValue:
                                        binaryright = XmlConcept(action.Expression, null, true);
                                        break;
                                    case EmRules.ComparatorType.ExpressionValue:
                                        binaryright = XmlExpression(_expressionParserHelper.GetExpression(action.Expression));
                                        break;
                                    case EmRules.ComparatorType.TemporalyValue:
                                        binaryright = new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression));
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                rightAssign = new CodeCastExpression(typeDataConcept, new CodeBinaryOperatorExpression(invokeGet, @operator, binaryright));
                                break;

                            case EmRules.ArithmeticOperatorType.Round:
                                CodeTypeReferenceExpression targetConstant = new CodeTypeReferenceExpression(typeof(BusinessMath).FullName);
                                CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(targetConstant, "Round");
                                CodeCastExpression @params1 = new CodeCastExpression("System.Decimal", invokeGet);
                                CodeExpression @params2;

                                switch ((EmRules.ComparatorType)action.ComparatorType)
                                {
                                    case EmRules.ComparatorType.ConstantValue:
                                        if (typeDataConcept.Contains("int"))
                                        {
                                            if (action.Expression is MRules._ReferenceValue)
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression.Id)));
                                            }
                                            else if (action.Expression is MRules._ListEntityValue)
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression.ListValueCode)));
                                            }
                                            else if (action.Expression is MRules._RangeEntityValue)
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToInt32(action.Expression.RangeValueCode)));
                                            }
                                            else
                                            {
                                                @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(action.Expression)));
                                            }
                                        }
                                        else if (typeDataConcept.Contains("decimal"))
                                        {
                                            @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(action.Expression)));
                                        }
                                        else if (typeDataConcept.Contains("DateTime"))
                                        {
                                            @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(DateTime.ParseExact(action.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
                                        }
                                        else if (typeDataConcept.Contains("String"))
                                        {
                                            @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(Convert.ToString(action.Expression)));
                                        }
                                        else if (typeDataConcept.Contains("bool"))
                                        {
                                            @params2 = new CodeCastExpression("System.Boolean", new CodePrimitiveExpression(Convert.ToBoolean(action.Expression)));
                                        }
                                        else
                                        {
                                            throw new Exception("No se pudo validat el tipo de dato " + typeDataConcept);
                                        }

                                        break;

                                    case EmRules.ComparatorType.ConceptValue:
                                        @params2 = new CodeCastExpression("System.Int32", XmlConcept(action.Expression, null, true));
                                        break;

                                    case EmRules.ComparatorType.ExpressionValue:
                                        @params2 = new CodeCastExpression("System.Int32", XmlExpression(_expressionParserHelper.GetExpression(action.Expression)));
                                        break;

                                    case EmRules.ComparatorType.TemporalyValue:
                                        @params2 = new CodeCastExpression("System.Int32", new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression)));
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }

                                rightAssign = new CodeMethodInvokeExpression(method, @params1, @params2);
                                rightAssign = new CodeCastExpression(typeDataConcept, rightAssign);
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        ((CodeMethodInvokeExpression)invokeSet).Parameters.Add(rightAssign);
                        return invokeSet;
                    }


                case EmRules.AssignType.InvokeAssign:
                    switch (((MRules._ActionInvoke)action).InvokeType)
                    {
                        case EmRules.InvokeType.MessageInvoke:
                            CodePrimitiveExpression message = new CodePrimitiveExpression(action.Expression);
                            CodeObjectCreateExpression objCreate = new CodeObjectCreateExpression(typeof(BusinessException).FullName, message);
                            return new CodeThrowExceptionStatement(objCreate);

                        case EmRules.InvokeType.RuleSetInvoke:
                            CodePrimitiveExpression paramRuleset = new CodePrimitiveExpression(Convert.ToInt32(action.Expression.RuleSetId));
                            CodeThisReferenceExpression targetRuleSet = new CodeThisReferenceExpression();
                            return new CodeMethodInvokeExpression(targetRuleSet, "FireRules", paramRuleset);

                        case EmRules.InvokeType.FunctionInvoke:
                            CodePrimitiveExpression paramFunction = new CodePrimitiveExpression(Convert.ToString(action.Expression.FunctionName));
                            CodeThisReferenceExpression targetFunction = new CodeThisReferenceExpression();
                            return new CodeMethodInvokeExpression(targetFunction, "ExecuteFunction", paramFunction);

                        default:
                            throw new ArgumentOutOfRangeException(((MRules._ActionInvoke)action).InvokeType.ToString(), "Tipo de accion invoke no valida");
                    }

                case EmRules.AssignType.TemporalAssign:
                    dynamic left = XmlTempValue(action.ValueTemp);

                    CodeExpression right = null;

                    switch (((MRules._ActionValueTemp)action).ArithmeticOperator.ArithmeticOperatorType)
                    {

                        case EmRules.ArithmeticOperatorType.Assign:
                            switch (((MRules._ActionValueTemp)action).ComparatorType)
                            {
                                case EmRules.ComparatorType.ConstantValue:
                                    right = new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(action.Expression));
                                    break;

                                case EmRules.ComparatorType.ConceptValue:
                                    right = XmlConcept(action.Expression, null, true);
                                    break;

                                case EmRules.ComparatorType.ExpressionValue:
                                    right = XmlExpression(_expressionParserHelper.GetExpression(action.Expression));
                                    break;

                                case EmRules.ComparatorType.TemporalyValue:
                                    right = new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression));
                                    break;
                            }
                            break;

                        case EmRules.ArithmeticOperatorType.Add:
                        case EmRules.ArithmeticOperatorType.Subtract:
                        case EmRules.ArithmeticOperatorType.Multiply:
                        case EmRules.ArithmeticOperatorType.Divide:

                            CodeExpression binaryright;
                            CodeCastExpression binaryLeft = new CodeCastExpression("System.Decimal", left);
                            CodeBinaryOperatorType @operator = (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), action.ArithmeticOperator.ArithmeticOperatorType.ToString());

                            switch (((MRules._ActionValueTemp)action).ComparatorType)
                            {
                                case EmRules.ComparatorType.ConstantValue:
                                    binaryright = new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(action.Expression));
                                    break;

                                case EmRules.ComparatorType.ConceptValue:
                                    binaryright = XmlConcept(action.Expression, null, true);
                                    break;

                                case EmRules.ComparatorType.ExpressionValue:
                                    binaryright = XmlExpression(_expressionParserHelper.GetExpression(action.Expression));
                                    break;

                                case EmRules.ComparatorType.TemporalyValue:
                                    binaryright = new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression));
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            right = new CodeBinaryOperatorExpression(binaryLeft, @operator, binaryright);
                            break;



                        case EmRules.ArithmeticOperatorType.Round:
                            CodeTypeReferenceExpression targetConstant = new CodeTypeReferenceExpression(typeof(BusinessMath).FullName);
                            CodeMethodReferenceExpression method = new CodeMethodReferenceExpression(targetConstant, "Round");
                            CodeCastExpression @params1 = new CodeCastExpression("System.Decimal", new CodeCastExpression("System.Decimal", left));
                            CodeExpression @params2;

                            switch (((MRules._ActionValueTemp)action).ComparatorType)
                            {
                                case EmRules.ComparatorType.ConstantValue:
                                    @params2 = new CodeCastExpression("System.Int32", new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(action.Expression)));
                                    break;

                                case EmRules.ComparatorType.ConceptValue:
                                    @params2 = new CodeCastExpression("System.Int32", XmlConcept(action.Expression, null, true));
                                    break;

                                case EmRules.ComparatorType.ExpressionValue:
                                    @params2 = new CodeCastExpression("System.Int32", XmlExpression(_expressionParserHelper.GetExpression(action.Expression)));
                                    break;

                                case EmRules.ComparatorType.TemporalyValue:
                                    @params2 = new CodeCastExpression("System.Int32", new CodeCastExpression("System.Decimal", XmlTempValue(action.Expression)));
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }

                            right = new CodeMethodInvokeExpression(method, @params1, @params2);
                            break;
                    }
                    return new CodeAssignStatement(left, right);

                default:
                    throw new ArgumentOutOfRangeException("No se puede validar el valor " + action.AssignType);
            }
        }

        private void SaveAction(XmlWriter writer, CodeStatementCollection stmts)
        {
            foreach (CodeObject stmt in stmts)
            {
                SaveAction(writer, stmt as CodeStatement);
            }
        }

        private void SaveAction(XmlWriter writer, CodeStatement stmt)
        {
            if (stmt is CodeAssignStatement)
            {
                CodeAssignStatement assign = (CodeAssignStatement)stmt;

                writer.WriteStartElement("assign");
                SaveExpression(writer, assign.Left);
                SaveExpression(writer, assign.Right);
                writer.WriteEndElement();

                return;
            }

            if (stmt is CodeConditionStatement)
            {
                CodeConditionStatement @if = (CodeConditionStatement)stmt;

                writer.WriteStartElement("if");

                writer.WriteStartElement("condition");
                SaveExpression(writer, @if.Condition);
                writer.WriteEndElement();

                if (@if.TrueStatements != null && @if.TrueStatements.Count > 0)
                {
                    writer.WriteStartElement("true");
                    SaveAction(writer, @if.TrueStatements);
                    writer.WriteEndElement();
                }

                if (@if.FalseStatements != null && @if.FalseStatements.Count > 0)
                {
                    writer.WriteStartElement("false");
                    SaveAction(writer, @if.FalseStatements);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                return;
            }

            if (stmt is CodeExpressionStatement)
            {
                CodeExpressionStatement exp = (CodeExpressionStatement)stmt;
                SaveExpression(writer, exp.Expression);
                return;
            }

            if (stmt is CodeIterationStatement)
            {
                CodeIterationStatement ite = (CodeIterationStatement)stmt;

                writer.WriteStartElement("iteration");

                writer.WriteStartElement("init");
                SaveAction(writer, ite.InitStatement);
                writer.WriteEndElement();

                writer.WriteStartElement("inc");
                SaveAction(writer, ite.IncrementStatement);
                writer.WriteEndElement();

                writer.WriteStartElement("test");
                SaveExpression(writer, ite.TestExpression);
                writer.WriteEndElement();

                writer.WriteStartElement("loop");
                SaveAction(writer, ite.Statements);
                writer.WriteEndElement();

                writer.WriteEndElement();
                return;
            }

            if (stmt is CodeSnippetStatement)
            {
                writer.WriteStartElement("code");
                writer.WriteString((stmt as CodeSnippetStatement).Value);
                writer.WriteEndElement();
                return;
            }

            if (stmt is CodeThrowExceptionStatement)
            {
                writer.WriteStartElement("throw");
                SaveExpression(writer, ((CodeThrowExceptionStatement)stmt).ToThrow);
                writer.WriteEndElement();
                return;
            }

            throw new Exception("No se reconoció el elemento de sentencia: " + stmt.GetType().FullName);
        }

        private void SaveExpression(XmlWriter writer, CodeExpression exp)
        {
            if (exp is CodeBinaryOperatorExpression)
            {
                CodeBinaryOperatorExpression bin = (CodeBinaryOperatorExpression)exp;

                writer.WriteStartElement("binary-op");
                writer.WriteAttributeString("type", bin.Operator.ToString());
                SaveExpression(writer, bin.Left);
                SaveExpression(writer, bin.Right);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeThisReferenceExpression)
            {
                writer.WriteElementString("this", null);
                return;
            }

            if (exp is CodeArgumentReferenceExpression)
            {
                CodeArgumentReferenceExpression arg = (CodeArgumentReferenceExpression)exp;

                writer.WriteStartElement("argument");
                writer.WriteAttributeString("name", arg.ParameterName);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodePrimitiveExpression)
            {
                CodePrimitiveExpression prim = (CodePrimitiveExpression)exp;

                writer.WriteStartElement("primitive");
                if (prim.Value != null)
                {
                    writer.WriteAttributeString("type", GetPrimitiveTypeName(prim.Value.GetType()));
                    writer.WriteAttributeString("value", string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", prim.Value));
                }
                writer.WriteEndElement();

                return;
            }

            if (exp is CodePropertyReferenceExpression)
            {
                CodePropertyReferenceExpression prop = (CodePropertyReferenceExpression)exp;

                writer.WriteStartElement("property");
                writer.WriteAttributeString("name", prop.PropertyName);

                SaveExpression(writer, prop.TargetObject);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeFieldReferenceExpression)
            {
                CodeFieldReferenceExpression fld = (CodeFieldReferenceExpression)exp;

                writer.WriteStartElement("field");
                writer.WriteAttributeString("name", fld.FieldName);

                SaveExpression(writer, fld.TargetObject);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeCastExpression)
            {
                CodeCastExpression cast = (CodeCastExpression)exp;

                writer.WriteStartElement("cast");
                writer.WriteAttributeString("type", cast.TargetType.BaseType);

                SaveExpression(writer, cast.Expression);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeIndexerExpression)
            {
                CodeIndexerExpression ind = (CodeIndexerExpression)exp;

                writer.WriteStartElement("indexer");

                writer.WriteStartElement("indices");
                foreach (CodeExpression index in ind.Indices)
                {
                    SaveExpression(writer, index);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("target");
                SaveExpression(writer, ind.TargetObject);
                writer.WriteEndElement();

                writer.WriteEndElement();

                return;
            }

            if (exp is CodeMethodInvokeExpression)
            {
                CodeMethodInvokeExpression inv = (CodeMethodInvokeExpression)exp;

                writer.WriteStartElement("invoke");
                writer.WriteAttributeString("method", inv.Method.MethodName);

                writer.WriteStartElement("params");
                foreach (CodeExpression par in inv.Parameters)
                {
                    SaveExpression(writer, par);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("target");
                SaveExpression(writer, inv.Method.TargetObject);
                writer.WriteEndElement();

                writer.WriteEndElement();

                return;
            }

            if (exp is CodeObjectCreateExpression)
            {
                CodeObjectCreateExpression create = (CodeObjectCreateExpression)exp;

                writer.WriteStartElement("object-create");
                writer.WriteAttributeString("type", create.CreateType.BaseType);

                foreach (CodeExpression par in create.Parameters)
                {
                    SaveExpression(writer, par);
                }

                writer.WriteEndElement();

                return;
            }

            if (exp is CodeTypeOfExpression)
            {
                CodeTypeOfExpression t = (CodeTypeOfExpression)exp;

                writer.WriteStartElement("type-of");
                writer.WriteAttributeString("type", t.Type.BaseType);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeTypeReferenceExpression)
            {
                CodeTypeReferenceExpression t = (CodeTypeReferenceExpression)exp;

                writer.WriteStartElement("type");
                writer.WriteAttributeString("name", t.Type.BaseType);
                writer.WriteEndElement();

                return;
            }

            throw new Exception("Expresión no reconocida (" + exp.GetType().FullName + ").");
        }

        private void SaveParameter(XmlWriter writer, MRules._Parameter parameter)
        {
            writer.WriteStartElement("parameter");
            writer.WriteAttributeString("name", parameter.Name);

            writer.WriteStartElement("class");
            writer.WriteString(parameter.Type);

            writer.WriteEndElement(); // class
            writer.WriteEndElement(); // parameter
        }

        private CodeExpression FillCondition(MRules._Condition condition)
        {
            CodeBinaryOperatorExpression exp = new CodeBinaryOperatorExpression();
            MRules._BasicConcept basicConcept = new MRules._BasicConcept();

            switch (condition.Comparator.Operator)
            {
                case EmRules.OperatorConditionType.Equals:
                    exp.Operator = CodeBinaryOperatorType.IdentityEquality;
                    break;
                case EmRules.OperatorConditionType.LessThan:
                    exp.Operator = CodeBinaryOperatorType.LessThan;
                    break;
                case EmRules.OperatorConditionType.LessThanOrEquals:
                    exp.Operator = CodeBinaryOperatorType.LessThanOrEqual;
                    break;
                case EmRules.OperatorConditionType.GreaterThan:
                    exp.Operator = CodeBinaryOperatorType.GreaterThan;
                    break;
                case EmRules.OperatorConditionType.GreaterThanOrEquals:
                    exp.Operator = CodeBinaryOperatorType.GreaterThanOrEqual;
                    break;
                case EmRules.OperatorConditionType.Distinct:
                    exp.Operator = CodeBinaryOperatorType.IdentityInequality;
                    break;
            }

            if (condition.Concept.ConceptType == EmRules.ConceptType.Basic)
            {
                if (this.concepts.Any(x => x.ConceptId == condition.Concept.ConceptId && x.Entity.EntityId == condition.Concept.Entity.EntityId))
                {
                    basicConcept = this.concepts.First(x => x.ConceptId == condition.Concept.ConceptId && x.Entity.EntityId == condition.Concept.Entity.EntityId) as MRules._BasicConcept;
                }
                else
                {
                    basicConcept = _conceptsDao.GetBasicConceptByIdConceptIdEntity(condition.Concept.ConceptId, condition.Concept.Entity.EntityId);
                    this.concepts.Add(basicConcept);
                }

                exp.Left = XmlConcept(condition.Concept, basicConcept, true);
            }
            else
            {
                exp.Left = XmlConcept(condition.Concept, null, true);
            }


            switch (condition.ComparatorType)
            {
                case EmRules.ComparatorType.ConstantValue:
                    if (condition.Expression != null)
                    {
                        switch (((MRules._Concept)condition.Concept).ConceptType)
                        {
                            case EmRules.ConceptType.Basic:
                                switch (basicConcept.BasicType)
                                {
                                    case EmRules.BasicType.Decimal:
                                        exp.Right = new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(condition.Expression));
                                        break;

                                    case EmRules.BasicType.Numeric:
                                        exp.Right = new CodePrimitiveExpression((int)ConvertHelper.ConvertToDecimal(condition.Expression));
                                        break;

                                    case EmRules.BasicType.Text:
                                        exp.Right = new CodePrimitiveExpression(Convert.ToString(condition.Expression));
                                        break;
                                    case EmRules.BasicType.Date:
                                        exp.Right = new CodePrimitiveExpression(DateTime.ParseExact(condition.Expression, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                        break;
                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                                break;

                            case EmRules.ConceptType.Range:
                                exp.Right = new CodePrimitiveExpression(Convert.ToInt32(condition.Expression.RangeValueCode));
                                break;

                            case EmRules.ConceptType.List:

                                MRules._ListConcept listConcept;
                                if (this.concepts.Any(x => x.ConceptId == condition.Concept.ConceptId && x.Entity.EntityId == condition.Concept.Entity.EntityId))
                                {
                                    listConcept = this.concepts.First(x => x.ConceptId == condition.Concept.ConceptId && x.Entity.EntityId == condition.Concept.Entity.EntityId) as MRules._ListConcept;
                                }
                                else
                                {
                                    listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(condition.Concept.ConceptId, condition.Concept.Entity.EntityId);
                                    this.concepts.Add(listConcept);
                                }

                                if (listConcept.ListEntity.DescriptionList == "Booleano")
                                {
                                    exp.Right = new CodePrimitiveExpression(Convert.ToInt32(condition.Expression.ListValueCode) == listConcept.ListEntity.ListEntityValues.Max(x => x.ListValueCode));
                                }
                                else
                                {
                                    exp.Right = new CodePrimitiveExpression(Convert.ToInt32(condition.Expression.ListValueCode));
                                }
                                break;

                            case EmRules.ConceptType.Reference:
                                exp.Right = new CodePrimitiveExpression(Convert.ToInt32(condition.Expression.Id));
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    else
                    {
                        exp.Right = new CodePrimitiveExpression();
                    }
                    break;

                case EmRules.ComparatorType.ConceptValue:
                    if (condition.Expression.ConceptType == EmRules.ConceptType.Basic)
                    {
                        dynamic tmpBasicConcept;

                        if (this.concepts.Any(x => x.ConceptId == condition.Expression.ConceptId && x.Entity.EntityId == condition.Expression.Entity.EntityId))
                        {
                            tmpBasicConcept = this.concepts.First(x => x.ConceptId == condition.Expression.ConceptId && x.Entity.EntityId == condition.Expression.Entity.EntityId);
                        }
                        else
                        {
                            tmpBasicConcept = _conceptsDao.GetBasicConceptByIdConceptIdEntity(condition.Expression.ConceptId, condition.Expression.Entity.EntityId);
                            this.concepts.Add(tmpBasicConcept);
                        }

                        exp.Right = XmlConcept(condition.Expression, tmpBasicConcept, true);
                    }
                    else
                    {
                        exp.Right = XmlConcept(condition.Expression, null, true);
                    }

                    break;

                case EmRules.ComparatorType.ExpressionValue:

                    exp.Right = XmlExpression(_expressionParserHelper.GetExpression(condition.Expression));
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return exp;
        }

        private CodeExpression XmlExpression(ExpressionParserHelper.Expression expression)
        {
            if (expression is ExpressionParserHelper.Operator)
            {
                CodeExpression left = XmlExpression(((ExpressionParserHelper.Operator)expression).LeftExpression);
                CodeExpression right = XmlExpression(((ExpressionParserHelper.Operator)expression).RightExpression);

                switch (((ExpressionParserHelper.Operator)expression).Op)
                {
                    case "+":
                        return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.Add, right);
                    case "-":
                        return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.Subtract, right);
                    case "*":
                        return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.Multiply, right);
                    case "/":
                        return new CodeBinaryOperatorExpression(left, CodeBinaryOperatorType.Divide, right);

                    default:
                        throw new ArgumentOutOfRangeException(((ExpressionParserHelper.Operator)expression).Op, "Operador no reconocido");
                }
            }
            if (expression is ExpressionParserHelper.Number)
            {
                return new CodePrimitiveExpression(ConvertHelper.ConvertToDecimal(((ExpressionParserHelper.Number)expression).Value));
            }
            throw new ArgumentOutOfRangeException(expression.GetType().Name, "Expresion no reconocida");
        }

        private CodeExpression XmlConcept(MRules._Concept concept, MRules._BasicConcept basicConcept, bool isGet)
        {
            FacadeType facadeType = USHELPER.EnumHelper.GetItemNameParameterValue<FacadeType>(concept.Entity.EntityId);
            string name = USHELPER.EnumHelper.GetDescription(facadeType);
            string prmEntity = "prm" + name;


            CodeArgumentReferenceExpression argument = new CodeArgumentReferenceExpression(prmEntity);

            if (concept.IsStatic)
            {
                return new CodePropertyReferenceExpression(argument, concept.ConceptName);
            }
            else
            {
                CodePrimitiveExpression @params = new CodePrimitiveExpression(concept.ConceptId);
                CodeCastExpression cast = new CodeCastExpression("Sistran.Core.Application.Scripts.Entities.IDynamicConceptContainer",
                    argument);

                CodeMethodReferenceExpression target = isGet
                    ? new CodeMethodReferenceExpression(cast, "GetDynamicConcept")
                    : new CodeMethodReferenceExpression(cast, "SetDynamicConcept");

                CodeMethodInvokeExpression invoke = new CodeMethodInvokeExpression(target, @params);

                switch (concept.ConceptType)
                {
                    case EmRules.ConceptType.Basic:
                        if (basicConcept == null)
                        {
                            if (this.concepts.Any(x => x.ConceptId == concept.ConceptId && x.Entity.EntityId == concept.Entity.EntityId))
                            {
                                basicConcept = this.concepts.First(x => x.ConceptId == concept.ConceptId && x.Entity.EntityId == concept.Entity.EntityId) as MRules._BasicConcept;
                            }
                            else
                            {
                                basicConcept = _conceptsDao.GetBasicConceptByIdConceptIdEntity(concept.ConceptId, concept.Entity.EntityId);
                                this.concepts.Add(basicConcept);
                            }
                        }

                        return new CodeCastExpression(GetTypeDataConcept(basicConcept), invoke);

                    case EmRules.ConceptType.List:
                    case EmRules.ConceptType.Range:
                    case EmRules.ConceptType.Reference:
                        return new CodeCastExpression(GetTypeDataConcept(concept), invoke);

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private CodeExpression XmlTempValue(string tempValue)
        {
            CodePropertyReferenceExpression target = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "LocalValues");
            CodePrimitiveExpression param = new CodePrimitiveExpression(tempValue);
            return new CodeIndexerExpression(target, param);
        }

        private string GetTypeDataConcept(MRules._Concept concept)
        {
            if (concept is MRules._BasicConcept)
            {
                switch (((MRules._BasicConcept)concept).BasicType)
                {
                    case EmRules.BasicType.Numeric:
                        return concept.IsNulleable || !concept.IsStatic ? "int?" : "int";

                    case EmRules.BasicType.Text:
                        return "System.String";

                    case EmRules.BasicType.Decimal:
                        return concept.IsNulleable || !concept.IsStatic ? "decimal?" : "decimal";

                    case EmRules.BasicType.Date:
                        return concept.IsNulleable || !concept.IsStatic ? "DateTime?" : "DateTime";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                if (concept.ConceptType == EmRules.ConceptType.List)
                {
                    MRules._ListConcept listConcept;

                    if (this.concepts.Any(x => x.ConceptId == concept.ConceptId && x.Entity.EntityId == concept.Entity.EntityId))
                    {
                        listConcept = this.concepts.First(x => x.ConceptId == concept.ConceptId && x.Entity.EntityId == concept.Entity.EntityId) as MRules._ListConcept;
                    }
                    else
                    {
                        listConcept = _conceptsDao.GetListConceptByIdConceptIdEntity(concept.ConceptId, concept.Entity.EntityId);
                        this.concepts.Add(listConcept);
                    }


                    if (listConcept.ListEntity.DescriptionList == "Booleano")
                    {
                        return concept.IsNulleable || !concept.IsStatic ? "bool?" : "bool";
                    }
                }

                return concept.IsNulleable || !concept.IsStatic ? "int?" : "int";
            }
        }

        private void SaveCondition(XmlWriter writer, CodeExpression exp)
        {
            if (exp is CodeBinaryOperatorExpression)
            {
                CodeBinaryOperatorExpression bin = (CodeBinaryOperatorExpression)exp;

                writer.WriteStartElement("binary-op");
                writer.WriteAttributeString("type", bin.Operator.ToString());
                SaveCondition(writer, bin.Left);
                SaveCondition(writer, bin.Right);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeThisReferenceExpression)
            {
                writer.WriteElementString("this", null);
                return;
            }

            if (exp is CodeArgumentReferenceExpression)
            {
                CodeArgumentReferenceExpression arg = (CodeArgumentReferenceExpression)exp;

                writer.WriteStartElement("argument");
                writer.WriteAttributeString("name", arg.ParameterName);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodePrimitiveExpression)
            {
                CodePrimitiveExpression prim = (CodePrimitiveExpression)exp;

                writer.WriteStartElement("primitive");
                if (prim.Value != null)
                {
                    writer.WriteAttributeString("type", GetPrimitiveTypeName(prim.Value.GetType()));
                    writer.WriteAttributeString("value", string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", prim.Value));
                }
                writer.WriteEndElement();

                return;
            }

            if (exp is CodePropertyReferenceExpression)
            {
                CodePropertyReferenceExpression prop = (CodePropertyReferenceExpression)exp;

                writer.WriteStartElement("property");
                writer.WriteAttributeString("name", prop.PropertyName);

                SaveCondition(writer, prop.TargetObject);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeFieldReferenceExpression)
            {
                CodeFieldReferenceExpression fld = (CodeFieldReferenceExpression)exp;

                writer.WriteStartElement("field");
                writer.WriteAttributeString("name", fld.FieldName);

                SaveCondition(writer, fld.TargetObject);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeCastExpression)
            {
                CodeCastExpression cast = (CodeCastExpression)exp;

                writer.WriteStartElement("cast");
                writer.WriteAttributeString("type", cast.TargetType.BaseType);

                SaveCondition(writer, cast.Expression);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeIndexerExpression)
            {
                CodeIndexerExpression ind = (CodeIndexerExpression)exp;

                writer.WriteStartElement("indexer");

                writer.WriteStartElement("indices");
                foreach (CodeExpression index in ind.Indices)
                {
                    SaveCondition(writer, index);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("target");
                SaveCondition(writer, ind.TargetObject);
                writer.WriteEndElement();

                writer.WriteEndElement();

                return;
            }

            if (exp is CodeMethodInvokeExpression)
            {
                CodeMethodInvokeExpression inv = (CodeMethodInvokeExpression)exp;

                writer.WriteStartElement("invoke");
                writer.WriteAttributeString("method", inv.Method.MethodName);

                writer.WriteStartElement("params");
                foreach (CodeExpression par in inv.Parameters)
                {
                    SaveCondition(writer, par);
                }
                writer.WriteEndElement();

                writer.WriteStartElement("target");
                SaveCondition(writer, inv.Method.TargetObject);
                writer.WriteEndElement();

                writer.WriteEndElement();

                return;
            }

            if (exp is CodeObjectCreateExpression)
            {
                CodeObjectCreateExpression create = (CodeObjectCreateExpression)exp;

                writer.WriteStartElement("object-create");
                writer.WriteAttributeString("type", create.CreateType.BaseType);

                foreach (CodeExpression par in create.Parameters)
                {
                    SaveCondition(writer, par);
                }

                writer.WriteEndElement();

                return;
            }

            if (exp is CodeTypeOfExpression)
            {
                CodeTypeOfExpression t = (CodeTypeOfExpression)exp;

                writer.WriteStartElement("type-of");
                writer.WriteAttributeString("type", t.Type.BaseType);
                writer.WriteEndElement();

                return;
            }

            if (exp is CodeTypeReferenceExpression)
            {
                CodeTypeReferenceExpression t = (CodeTypeReferenceExpression)exp;

                writer.WriteStartElement("type");
                writer.WriteAttributeString("name", t.Type.BaseType);
                writer.WriteEndElement();

                return;
            }

            throw new Exception("Expresión no reconocida (" + exp.GetType().FullName + ").");
        }

        private string GetPrimitiveTypeName(System.Type t)
        {
            if (t == typeof(string))
            {
                return "String";
            }

            if (t == typeof(int))
            {
                return "Integer";
            }

            if (t == typeof(bool))
            {
                return "Boolean";
            }

            if (t == typeof(decimal))
            {
                return "Decimal";
            }

            if (t == typeof(DateTime))
            {
                return "DateTime";
            }

            throw new Exception("No se encontró el tipo para el dato primitivo (" + t.FullName + ").");
        }
    }

    public static class EnumHelper
    {
        public static T GetValueMember<T, E>(Enum @enum)
        {
            Type type = typeof(E);
            MemberInfo[] memInfo = type.GetMember(@enum.ToString());
            object[] attributes = memInfo[memInfo.Length - 1].GetCustomAttributes(typeof(T), false);
            return (T)attributes[attributes.Length - 1];
        }
    }

    public class ExpressionParserHelper
    {
        private int _parenthesisCounter = 0;
        private readonly ListDictionary _parenthesis = new ListDictionary();
        private readonly Regex _re;


        public ExpressionParserHelper()
        {
            string number = @"(\d+)((?<!\d{4,})(,\d{3}))*(\.\d+)?";
            string concept = @"[A-Za-z_](\w|\d)+";
            string concept2 = @"\[(?>.+?((?<!\\)\]))";
            string @operator = @"[-\+\*\/]";
            string operand = number + "|" + concept + "|" + concept2;

            string rexp = @"^(\s*(?<op1>" + operand + @"))(\s*(?<op>" + @operator + @")\s*(?<op2>" + operand + @"))*\s*$";
            _re = new Regex(rexp, RegexOptions.ExplicitCapture);
        }

        public string ValidateExpression(string expression)
        {
            Expression exp = GetExpression(expression);
            exp = ReplaceParenthesis(exp);

            StringBuilder sb = new StringBuilder();
            exp.GetExpressionString(sb);
            string str = sb.ToString();

            if (str.Contains('[') || str.Contains(']'))
            {
                throw new Exception("Invalid expression.");
            }

            return str;
        }

        public Expression GetExpression(string expression)
        {
            string expstring = expression.Trim().Replace("\n", " ").ToString().Replace(",", ".");
            int to;
            string s = ReplaceParenthesis(" " + expstring, 0, out to);
            Expression exp = ParseExpression(s);
            return ReplaceParenthesis(exp);
        }

        private string GetNewParenthesisName()
        {
            this._parenthesisCounter++;
            return "__par_" + this._parenthesisCounter.ToString();
        }



        private Expression ReplaceParenthesis(Expression exp)
        {
            if (exp is Variable)
            {
                string name = ((Variable)exp).Name;
                if (name.StartsWith("__par_"))
                {
                    return this.ReplaceParenthesis((Expression)this._parenthesis[name]);
                }

                return exp;
            }

            if (exp is Operator)
            {
                Operator op = (Operator)exp;
                op.LeftExpression = this.ReplaceParenthesis(op.LeftExpression);
                op.RightExpression = this.ReplaceParenthesis(op.RightExpression);

                return op;
            }

            return exp;
        }

        private string ReplaceParenthesis(string s, int from, out int to)
        {
            char c;
            StringBuilder sb = new StringBuilder();
            int last;
            if (s[from] == '(')
            {
                last = from + 1;
            }
            else
            {
                last = from;
            }

            int p = last;
            bool inCor = false;
            while (p < s.Length && ((c = s[p]) != ')' || inCor))
            {
                if (c == '(')
                {
                    if (!inCor)
                    {
                        sb.Append(s.Substring(last, p - last));
                        sb.Append(this.ReplaceParenthesis(s, p, out p));

                        last = p + 1;
                    }
                }
                else if (c == '[')
                {
                    inCor = true;
                }
                else if (c == ']')
                {
                    if (p > 0 && s[p - 1] != '\\')
                    {
                        inCor = false;
                    }
                }

                p++;
            }

            if (p >= s.Length && s[from] == '(')
            {
                throw new Exception("Se esperaba ')'.");
            }

            sb.Append(s.Substring(last, p - last));

            string res = sb.ToString().Trim();
            if (res.Equals(string.Empty))
            {
                to = p;
                return string.Empty;
            }

            Expression exp = this.ParseExpression(res);
            string name = this.GetNewParenthesisName();
            this._parenthesis.Add(name, exp);
            to = p;

            return name;
        }

        private Expression ParseExpression(string s)
        {
            Match m = this._re.Match(s);
            if (!m.Success)
            {
                throw new Exception("Invalid expression.");
            }

            Group g = m.Groups["op1"];
            string val = g.Value;

            Expression exp;
            if (val.StartsWith("["))
            {
                exp = new Variable(val.Substring(1, val.Length - 2).Replace("\\]", "]"));
            }
            else if (!char.IsDigit(val[0]))
            {
                exp = new Variable(val);
            }
            else
            {
                exp = new Number(val);
            }

            g = m.Groups["op2"];
            if (g.Success)
            {
                Group opg = m.Groups["op"];
                int i = 0;
                foreach (Capture c in g.Captures)
                {
                    Expression right;
                    val = c.Value;
                    if (val.StartsWith("["))
                    {
                        right = new Variable(val.Substring(1, val.Length - 2).Replace("\\]", "]"));
                    }
                    else if (!char.IsDigit(val[0]))
                    {
                        right = new Variable(val);
                    }
                    else
                    {
                        right = new Number(val);
                    }

                    exp = new Operator(opg.Captures[i].Value, exp, right);
                    i++;
                }
            }

            return exp;
        }

        public class Expression
        {
            public virtual void GetExpressionString(StringBuilder sb)
            {
            }

        }

        public class Number : Expression
        {
            public string Value;

            public Number(string value)
            {
                this.Value = value;
            }

            public override void GetExpressionString(StringBuilder sb)
            {
                sb.Append(this.Value);
            }

        }

        public class Operator : Expression
        {
            public string Op;
            public Expression LeftExpression;
            public Expression RightExpression;

            public Operator(string op, Expression left, Expression right)
            {
                this.Op = op;
                this.LeftExpression = left;
                this.RightExpression = right;
            }

            public override void GetExpressionString(StringBuilder sb)
            {
                sb.Append("( ");
                this.LeftExpression.GetExpressionString(sb);
                sb.Append(" ");
                sb.Append(this.Op);
                sb.Append(" ");
                this.RightExpression.GetExpressionString(sb);
                sb.Append(" )");
            }

        }

        public class Variable : Expression
        {
            public string Name;

            public Variable(string name)
            {
                this.Name = name;
            }

            public override void GetExpressionString(StringBuilder sb)
            {
                sb.Append("[");
                sb.Append(this.Name);
                sb.Append("]");
            }

        }
    }

    public class DecisionTableOrder : IComparer<MRules._Rule>
    {
        private readonly List<MRules._Concept> _concepts;
        private List<string> Symbols = new List<string> { "=", "<", "<=", ">", ">=", "<>" };

        /// <summary>
        /// constructor de la clase
        /// </summary>
        /// <param name="concepts"></param>
        public DecisionTableOrder(List<MRules._Concept> concepts)
        {
            _concepts = concepts;
        }

        /// <summary>
        /// compara dos valores de tipo RuleComposite
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(MRules._Rule x, MRules._Rule y)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;

            IList<MRules._Condition> conditionsx = x.Conditions;
            IList<MRules._Condition> conditionsy = y.Conditions;

            foreach (MRules._Concept conddef in _concepts)
            {
                int entityId = conddef.Entity.EntityId;
                int conceptId = conddef.ConceptId;

                MRules._Condition condx = FindCondition(entityId, conceptId, conditionsx);
                MRules._Condition condy = FindCondition(entityId, conceptId, conditionsy);

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

                    int comp = Symbols.LastIndexOf(condx.Comparator.Symbol).CompareTo(Symbols.IndexOf(condy.Comparator.Symbol));
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

            return x.RuleId.CompareTo(y.RuleId);
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

    public class DecisionTableValidator
    {
        private class IntervalContainer
        {
            public Interval Interval = InfiniteInterval.GetSingleton();
            public ListDictionary Children;

            public IntervalContainer GetChildren(Interval i)
            {
                if (Children == null)
                {
                    return null;
                }

                return (IntervalContainer)Children[i];
            }

            public IntervalContainer GetChildrenOrCreate(Interval i)
            {
                IntervalContainer cont;
                if (Children == null)
                {
                    Children = new ListDictionary();
                    cont = null;
                }
                else
                {
                    cont = (IntervalContainer)Children[i];
                }

                if (cont == null)
                {
                    cont = new IntervalContainer();
                    Children.Add(i, cont);
                }

                return cont;
            }
        }

        public List<int> ValidatePublish(List<MRules._Rule> rules, out bool isValid)
        {
            List<int> unreachable = new List<int>();
            int conditionCount = rules.Last().Conditions.Count;

            IntervalContainer rootContainer = new IntervalContainer();

            foreach (MRules._Rule rule in rules)
            {
                bool unreachableRule = true;
                IntervalContainer levelContainer = rootContainer;

                int i = 0;
                foreach (MRules._Condition conddef in rule.Conditions)
                {
                    //RuleCondition cond = RuleHelper.FindCondition(conddef.EntityId, conddef.ConceptId, conds);

                    Interval inter = CreateInterval(conddef);

                    Interval remInterval = levelContainer.Interval;
                    if (!remInterval.IsEmpty)
                    {
                        Interval intersection = remInterval.Intersect(inter);

                        if (!intersection.IsEmpty)
                        {
                            levelContainer.Interval = remInterval.Substract(intersection);

                            unreachableRule = false;
                        }
                    }

                    i++;

                    if (i < conditionCount)
                    {
                        levelContainer = levelContainer.GetChildrenOrCreate(inter);
                    }
                }

                if (unreachableRule)
                {
                    unreachable.Add(rule.RuleId);
                }
            }

            isValid = CheckCompleteness(rootContainer);

            return unreachable;
        }

        private bool CheckCompleteness(IntervalContainer rootContainer)
        {
            if (!rootContainer.Interval.IsEmpty)
            {
                return false;
            }

            if (rootContainer.Children == null)
            {
                return true;
            }

            IntervalContainer generic = rootContainer.GetChildren(InfiniteInterval.GetSingleton());
            if (generic != null)
            {
                return CheckCompleteness(generic);
            }

            foreach (IntervalContainer con in rootContainer.Children.Values)
            {
                bool completeness = CheckCompleteness(con);
                if (!completeness)
                {
                    return false;
                }
            }

            return true;
        }

        private Interval CreateInterval(MRules._Condition cond)
        {
            if (cond == null || cond.Comparator == null)
            {
                return InfiniteInterval.GetSingleton();
            }

            EmRules.OperatorConditionType comp = cond.Comparator.Operator;

            //object condValue = cond.Value;
            if (cond.Expression == null)
            {
                return EmptyInterval.GetSingleton();
            }
            // Verificar si es string y convertirlo a float
            double value;
            string valueStr = "";
            if (cond.Expression != null)
            {
                if (cond.Expression is MRules._ListEntityValue)
                {
                    valueStr = cond.Expression.ListValueCode.ToString();
                }
                else if (cond.Expression is MRules._RangeEntityValue)
                {
                    valueStr = cond.Expression.RangeValueCode.ToString();
                }
                else if (cond.Expression is MRules._ReferenceValue)
                {
                    valueStr = cond.Expression.Id.ToString();
                }
                else
                {
                    valueStr = cond.Expression.ToString();
                }
            }

            if (double.TryParse(valueStr, out value))
            {
                //Acá es string ver que hace
                value = double.Parse(valueStr, CultureInfo.InvariantCulture);
            }
            else
            {
                DateTime date;
                if (DateTime.TryParseExact(valueStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    value = date.Ticks;
                }
                else
                {
                    string ascii = string.Empty;
                    foreach (char c in valueStr)
                    {
                        ascii += ((int)c).ToString();
                    }

                    value = Convert.ToDouble(ascii);
                }
            }

            switch (comp)
            {
                case EmRules.OperatorConditionType.Equals:
                    return new SimpleInterval(value);

                case EmRules.OperatorConditionType.LessThan:
                    return new SimpleInterval(Edge.NegativeInfinite, new Edge(value, false));

                case EmRules.OperatorConditionType.LessThanOrEquals:
                    return new SimpleInterval(Edge.NegativeInfinite, new Edge(value, true));

                case EmRules.OperatorConditionType.GreaterThan:
                    return new SimpleInterval(new Edge(value, false), Edge.Infinite);

                case EmRules.OperatorConditionType.GreaterThanOrEquals:
                    return new SimpleInterval(new Edge(value, true), Edge.Infinite);

                case EmRules.OperatorConditionType.Distinct:
                    return new SimpleIntervalList(
                        new SimpleInterval(Edge.NegativeInfinite, new Edge(value, false)),
                        new SimpleInterval(new Edge(value, false), Edge.Infinite));

                default:
                    throw new Exception("Rule condition comparator type not supported: " + comp);
            }
        }
    }

    public class _DecisionTableLoader
    {
        private DataSet _dataSet;
        private MRules.DecisionTableMapping _decisionTableMapping;
        private Dictionary<string, MRules.DecisionTableMappingExcelPageColumn> condicionConcepts = new Dictionary<string, MRules.DecisionTableMappingExcelPageColumn>();
        private Dictionary<string, MRules.DecisionTableMappingExcelPageColumn> actionConcepts = new Dictionary<string, MRules.DecisionTableMappingExcelPageColumn>();
        private readonly string _pathXml;
        private readonly string _pathXls;

        public _DecisionTableLoader(string pathXml, string pathXls)
        {
            _pathXml = pathXml;
            _pathXls = pathXls;
        }

        public bool ReadMappingFile()
        {
            try
            {
                _ConceptDao conceptDao = new _ConceptDao();

                using (StreamReader reader = new StreamReader(_pathXml))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(MRules.DecisionTableMapping));
                    _decisionTableMapping = (MRules.DecisionTableMapping)ser.Deserialize(reader);

                    //Obtener los IDs de entidad y concepto a partir del nombre
                    condicionConcepts.Clear();
                    foreach (MRules.DecisionTableMappingExcelPage excelPage in _decisionTableMapping.ExcelPage)
                    {
                        foreach (MRules.DecisionTableMappingExcelPageColumn column in excelPage.Column)
                        {
                            if (column.entity != "Operator")
                            {
                                var objectName = string.Concat("RULE_", string.Concat(column.entity.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToUpper());

                                int idEntity = int.Parse(USHELPER.EnumHelper.GetEnumParameterValue((FacadeType)Enum.Parse(typeof(FacadeType), objectName)).ToString());

                                Models.Entity entity = new Models.Entity { EntityName = column.entity, EntityId = idEntity };
                                List<MRules._Concept> concepts = conceptDao.GetConceptByFilter(new List<int> { entity.EntityId }, column.concept);

                                if (concepts.Count == 0)
                                {
                                    throw new Exception($"Error leyendo el XML: El concepto ({column.concept}) de la entidad ({column.entity}), no existe");
                                }


                                column.EntityId = entity.EntityId;
                                column.ConceptId = concepts[0].ConceptId;
                                column.IsDecimal = (byte)concepts[0].ConceptControlType == 2;

                                bool hasDependence = true;
                                List<string> dependency = new List<string>();

                                if (column.type == MRules.DecisionTableMappingExcelPageColumnType.condition && !condicionConcepts.ContainsKey(column.entity))
                                {
                                    foreach (MRules._ConceptDependence dependence in concepts[0].ConceptDependences)
                                    {
                                        KeyValuePair<string, MRules.DecisionTableMappingExcelPageColumn> conceptTmp = condicionConcepts.FirstOrDefault(x =>
                                            x.Value.EntityId == dependence.DependsConcept.Entity.EntityId &&
                                            x.Value.ConceptId == dependence.DependsConcept.ConceptId);

                                        if (conceptTmp.Key == null)
                                        {
                                            dependency.Add(dependence.DependsConcept.Description);
                                            hasDependence = false;
                                        }
                                    }

                                    if (hasDependence)
                                    {
                                        condicionConcepts.Add(column.entity_concept, column);
                                    }
                                    else
                                    {
                                        throw new Exception($"El concepto ({concepts[0].Description}) depende de los conceptos: </br>*{string.Join("</br>*", dependency)}");
                                    }
                                }
                                else if (column.type == MRules.DecisionTableMappingExcelPageColumnType.action && !actionConcepts.ContainsKey(column.entity_concept))
                                {
                                    foreach (MRules._ConceptDependence dependence in concepts[0].ConceptDependences)
                                    {
                                        KeyValuePair<string, MRules.DecisionTableMappingExcelPageColumn> conceptTmp = actionConcepts.FirstOrDefault(x =>
                                            x.Value.EntityId == dependence.DependsConcept.Entity.EntityId &&
                                            x.Value.ConceptId == dependence.DependsConcept.ConceptId);

                                        if (conceptTmp.Key == null)
                                        {
                                            dependency.Add(dependence.DependsConcept.Description);
                                            hasDependence = false;
                                        }
                                    }

                                    if (hasDependence)
                                    {
                                        actionConcepts.Add(column.entity_concept, column);
                                    }
                                    else
                                    {
                                        throw new Exception($"El concepto ({concepts[0].Description}) depende de los conceptos: </br>*{string.Join("</br>*", dependency)}");
                                    }
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MRules._DecisionTableMappingResult ReadExcelFile()
        {
            try
            {
                List<string[]> exceptions = new List<string[]>();
                string errorMessage = string.Empty;
                foreach (MRules.DecisionTableMappingExcelPage excelPage in _decisionTableMapping.ExcelPage)
                {
                    DataTable dt = null;
                    try
                    {
                        dt = Utilities.Helper.ExcelFilesLoadHelper.GetExcelFileWorkSheetInfo(_pathXls, ref errorMessage, excelPage.name, excelPage.range);
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.Message;
                    }
                    if (errorMessage != "")
                    {
                        throw new BusinessException("No se encuentra la hoja\"" + excelPage.name + "\"\nVerifique que la hoja exista en el archivo Excel e intente de nuevo");
                    }
                    else
                    {
                        if (excelPage.Column.Length != dt.Columns.Count)
                        {
                            errorMessage = "El rango no coincide con la definicion en el archivo de mapeo:\nNombre hoja: " + excelPage.name + "\nColumnas definidas: " + excelPage.Column.Length + "\nRango definido: " + excelPage.range;
                            exceptions.Add(new string[] { "General", errorMessage });
                        }
                        bool firtLine = true;
                        int positionRow = 0;
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (firtLine)
                            {
                                firtLine = false;
                            }
                            else
                            {
                                DataRow row = _dataSet.Tables[excelPage.name].NewRow();
                                foreach (MRules.DecisionTableMappingExcelPageColumn column in excelPage.Column)
                                {
                                    try
                                    {
                                        row[column.entity_concept] = dr[column.order - 1];
                                    }
                                    catch (Exception)
                                    {
                                        exceptions.Add(new string[] { $"{positionRow}", $"Regla No. {positionRow} columna {column.order} con error" });
                                    }
                                }
                                _dataSet.Tables[excelPage.name].Rows.Add(row);
                            }
                            positionRow++;
                        }
                    }
                }
                if (exceptions.Count == 1)
                {
                    return new MRules._DecisionTableMappingResult() { ErrorMessage = exceptions[0][1] };
                }
                if (exceptions.Count > 1)
                {
                    FileDAO fileDao = new FileDAO();
                    string fileWithExcepctions = fileDao.GenerateFileToDecisionTableByExceptions(exceptions);
                    return new MRules._DecisionTableMappingResult() { FileExceptions = fileWithExcepctions };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// Reemplaza para BD SP ASE el metodo CreateRuleBase, Inserta los valores de los archivos 
        /// para la funcionalidad de carga de tablas de decision
        /// </summary>
        /// <param name="isEvent"></param>
        /// <returns></returns>
        public MRules._DecisionTableMappingResult CreateRuleBase(bool isEvent)
        {
            List<string[]> exceptions = new List<string[]>();
            _ConceptDao conceptDao = new _ConceptDao();
            EnRules.RuleBase ruleBase = new EnRules.RuleBase();
            ConcurrentBag<MRules._Concept> concepts = new ConcurrentBag<MRules._Concept>();
            ConcurrentBag<EnRules.Rule> listRules = new ConcurrentBag<EnRules.Rule>();
            ConcurrentBag<EnRules.RuleCondition> listRuleCondition = new ConcurrentBag<EnRules.RuleCondition>();
            ConcurrentBag<EnRules.RuleAction> listRuleAction = new ConcurrentBag<EnRules.RuleAction>();

            int ruleConditionCount = 0;
            int ruleActionCount = 0;
            int ruleCount = 0;

            IDataFacade _df = DataFacadeManager.Instance.GetDataFacade(); //Pido un DataFacade al Manager de DataFacades
            IDbConnection connection = _df.GetCurrentConnection(); //Le pido la conexión a mi DataFacade

            EnParam.Package package;
            EnParam.Levels level;
            List<EnRules.PositionEntity> positionEntities;

            using (Context.Current)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Clear();
                filter.Property(EnParam.Package.Properties.Description).Equal().Constant(_decisionTableMapping.ExcelPage[0].package);
                package = new BusinessCollection(_df.SelectObjects(typeof(EnParam.Package), filter.GetPredicate())).Cast<EnParam.Package>().FirstOrDefault();
                if (package == null)
                {
                    throw new BusinessException("El paquete " + _decisionTableMapping.ExcelPage[0].package + " es invalido");
                }

                filter.Clear();
                filter.Property(EnParam.Levels.Properties.Description).Equal().Constant(_decisionTableMapping.ExcelPage[0].level);
                filter.And().Property(EnParam.Levels.Properties.PackageId).Equal().Constant(package.PackageId);
                level = new BusinessCollection(_df.SelectObjects(typeof(EnParam.Levels), filter.GetPredicate())).Cast<EnParam.Levels>().FirstOrDefault();
                if (level == null)
                {
                    throw new BusinessException("El nivel " + _decisionTableMapping.ExcelPage[0].level + " es invalido");
                }

                if (isEvent == true)
                {
                    PackageDAO packageDao = new PackageDAO();
                    List<MRules.Package> parameters = packageDao.GetPackagesPolicies();

                    if (!parameters.Exists(x => x.PackageId == package.PackageId))
                    {
                        throw new BusinessException("La tabla de decisión cargada no es de tipo política");
                    }
                }
                else if (isEvent == false && package.PackageId != 1)
                {
                    throw new BusinessException("La tabla de decisión cargada no es de tipo suscripción");
                }
                #region RuleBase

                ruleBase.RuleBaseId = 0;
                ruleBase.Description = _decisionTableMapping.ExcelPage[0].rulebase;
                ruleBase.PackageId = package.PackageId;
                ruleBase.LevelId = level.LevelId;
                ruleBase.CurrentFrom = DateTime.Now;
                ruleBase.IsPublished = false;
                ruleBase.IsEvent = isEvent;
                ruleBase.RuleBaseTypeCode = 2;
                ruleBase.RuleBaseVersion++;


                #endregion

                filter.Clear();
                filter.Property(EnRules.PositionEntity.Properties.PackageId).Equal().Constant(package.PackageId);
                filter.And().Property(EnRules.PositionEntity.Properties.LevelId).Equal().Constant(level.LevelId);
                positionEntities = new BusinessCollection(_df.SelectObjects(typeof(EnRules.PositionEntity), filter.GetPredicate())).Cast<EnRules.PositionEntity>().ToList();

                List<Task> threads = new List<Task>();
                foreach (MRules.DecisionTableMappingExcelPage page in _decisionTableMapping.ExcelPage)
                {
                    int rows = _dataSet.Tables[page.name].Rows.Count;
                    int numProcess = Convert.ToInt32(ConfigurationManager.AppSettings["MaxProcessThreadRuleSet"]);
                    int numThread = Math.Max(rows, numProcess) / numProcess;

                    /*Consulta los conceptos*/
                    for (int y = 0; y < page.Column.Length; y++)
                    {
                        try
                        {
                            MRules.DecisionTableMappingExcelPageColumn column = page.Column[y];

                            if (column.entity != "Operator")
                            {
                                object specificConcept;
                                if (concepts.Count(x => x.ConceptId == column.ConceptId && x.Entity.EntityId == column.EntityId) == 0)
                                {
                                    MRules._Concept concept = conceptDao.GetConceptByIdConceptIdEntity(column.ConceptId, column.EntityId);
                                    specificConcept = conceptDao.GetSpecificConceptWithVales(column.ConceptId, column.EntityId, new string[0], concept.ConceptType);

                                    if (!positionEntities.Any(b => b.EntityId == column.EntityId))
                                    {
                                        exceptions.Add(new string[] { ($"Concepto_{ y + 1}"), ("El concepto " + concept.Description + " no es valido a nivel de " + level.Description) });
                                    }

                                    ((MRules._Concept)specificConcept).ConceptDependences = concept.ConceptDependences;
                                    concepts.Add(specificConcept as MRules._Concept);
                                }
                                else
                                {
                                    specificConcept = concepts.First(x => x.ConceptId == column.ConceptId && x.Entity.EntityId == column.EntityId);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            exceptions.Add(new string[] { ($"Concepto_{ y + 1}"), ("Error en concepto") });
                        }
                    }

                    if (!exceptions.Any())
                    {
                        for (int t = 0; t < numThread; t++)
                        {
                            int length = numProcess * (t + 1);
                            int ii = t;

                            Task thread = new Task(() =>
                            {

                                if (ii == numThread - 1)
                                {
                                    length += (rows - length);
                                }

                                for (int rIndex = numProcess * ii; rIndex < length; rIndex++)
                                {
                                    DataRow row = _dataSet.Tables[page.name].Rows[rIndex];
                                    listRules.Add(new EnRules.Rule(ruleBase.RuleBaseId, rIndex + 1) { OrderNum = 0 });

                                    int conditionId = 1;
                                    int actionId = 1;
                                    int iComparatorCd = 1;

                                    for (int y = 0; y < page.Column.Length; y++)
                                    {

                                        try
                                        {
                                            MRules.DecisionTableMappingExcelPageColumn column = page.Column[y];

                                            if (column.entity != "Operator")
                                            {
                                                object value = row[column.entity_concept];
                                                object specificConcept = concepts.First(x => x.ConceptId == column.ConceptId && x.Entity.EntityId == column.EntityId);

                                                if (column.type == MRules.DecisionTableMappingExcelPageColumnType.condition)
                                                {
                                                    EnRules.RuleCondition ruleCondition =
                                                        new EnRules.RuleCondition(ruleBase.RuleBaseId, rIndex + 1, conditionId)
                                                        {
                                                            EntityId = column.EntityId,
                                                            ConceptId = column.ConceptId,
                                                            RuleValueTypeCode = 1,
                                                            OrderNum = 0,
                                                            ComparatorCode = !string.IsNullOrEmpty(value.ToString())
                                                                ? iComparatorCd
                                                                : (int?)null,
                                                            CondValue = null
                                                        };
                                                    if (ruleCondition.ComparatorCode != null)
                                                    {
                                                        if (specificConcept is MRules._BasicConcept)
                                                        {
                                                            value = value.ToString().Trim();
                                                            string[] formats = new[]
                                                            {
                                                                    "dd/MM/yyyy HH:mm:ss", "dd/MM/yyyy",
                                                                    "dd-MM-yyyy HH:mm:ss", "dd-MM-yyyy",
                                                                    "MM/dd/yyyy HH:mm:ss", "MM/dd/yyyy",
                                                                    "MM-dd-yyyy HH:mm:ss", "MM-dd-yyyy",
                                                                    "yyyy-MM-dd HH:mm:ss", "yyyy-MM-dd",
                                                                    "yyyy/MM/dd HH:mm:ss", "yyyy/MM/dd"
                                                                };
                                                            switch ((specificConcept as MRules._BasicConcept).BasicType)
                                                            {
                                                                case EmRules.BasicType.Numeric:
                                                                    ruleCondition.CondValue = Convert.ToInt32(value).ToString();
                                                                    break;
                                                                case EmRules.BasicType.Text:
                                                                    ruleCondition.CondValue = value.ToString();
                                                                    break;
                                                                case EmRules.BasicType.Decimal:
                                                                    ruleCondition.CondValue = ConvertHelper.ConvertToDecimal(value.ToString()).ToString();
                                                                    break;
                                                                case EmRules.BasicType.Date:
                                                                    ruleCondition.CondValue = DateTime.ParseExact(value.ToString(), formats, CultureInfo.InvariantCulture, DateTimeStyles.None).ToString("dd/MM/yyyy");
                                                                    break;
                                                                default:
                                                                    exceptions.Add(new string[] { ($"{ruleCondition.RuleId}"), ("El tipo de concepto basico " + (specificConcept as MRules._BasicConcept).BasicType + " no se pudo validar") });
                                                                    break;
                                                            }
                                                        }
                                                        else if (specificConcept is MRules._ListConcept)
                                                        {
                                                            if ((specificConcept as MRules._ListConcept).ListEntity.ListEntityValues.FirstOrDefault(x => x.ListValueCode == Convert.ToInt32(value)) == null)
                                                            {
                                                                exceptions.Add(new string[] { ($"{ruleCondition.RuleId}"), ("El concepto " + (specificConcept as MRules._ListConcept).Description + " no posee el valor " + value) });
                                                            }

                                                            ruleCondition.CondValue = value.ToString();
                                                        }
                                                        else if (specificConcept is MRules._RangeConcept)
                                                        {
                                                            if ((specificConcept as MRules._RangeConcept).RangeEntity.RangeEntityValues.FirstOrDefault(x => x.RangeValueCode == Convert.ToInt32(value)) == null)
                                                            {
                                                                exceptions.Add(new string[] { ($"{ruleCondition.RuleId}"), ("El concepto " + (specificConcept as MRules._RangeConcept).Description + " no posee el valor " + value) });
                                                            }

                                                            ruleCondition.CondValue = value.ToString();
                                                        }
                                                        else if (specificConcept is MRules._ReferenceConcept)
                                                        {
                                                            if (((MRules._ReferenceConcept)specificConcept).ConceptDependences.Count != 0)
                                                            {
                                                                List<string> dependency = new List<string>();

                                                                foreach (MRules._ConceptDependence dependence in ((MRules._ReferenceConcept)specificConcept).ConceptDependences)
                                                                {
                                                                    bool validateDependence = listRuleCondition.Cast<EnRules.RuleCondition>()
                                                                        .Any(x => x.EntityId == dependence.DependsConcept.Entity.EntityId &&
                                                                                  x.ConceptId == dependence.DependsConcept.ConceptId &&
                                                                                  x.RuleId == ruleCondition.RuleId);
                                                                    if (!validateDependence)
                                                                    {
                                                                        exceptions.Add(new string[] { ($"{ruleCondition.RuleId}"), ($"Se debe incluir el concepto  '{dependence.DependsConcept.Description}', por dependencia con el concepto '{((MRules._ReferenceConcept)specificConcept).Description}'") });
                                                                    }

                                                                    EnRules.RuleCondition conceptTmp = listRuleCondition.Cast<EnRules.RuleCondition>()
                                                                        .First(x => x.EntityId == dependence.DependsConcept.Entity.EntityId &&
                                                                                    x.ConceptId == dependence.DependsConcept.ConceptId &&
                                                                                    x.RuleId == ruleCondition.RuleId);

                                                                    if (conceptTmp.ComparatorCode != 1 || conceptTmp.CondValue == null)
                                                                    {
                                                                        exceptions.Add(new string[] { ($"{ruleCondition.RuleId}"), ($"El concepto {dependence.DependsConcept.Description} debe ser igual a un valor constante") });
                                                                    }

                                                                    dependency.Add(conceptTmp.CondValue);
                                                                }

                                                                specificConcept = conceptDao.GetSpecificConceptWithVales(column.ConceptId, column.EntityId, dependency.ToArray(), EmRules.ConceptType.Reference);
                                                            }

                                                            if ((specificConcept as MRules._ReferenceConcept).ReferenceValues.FirstOrDefault(x => x.Id == value.ToString()) == null)
                                                            {
                                                                exceptions.Add(new string[] { ($"{ruleCondition.RuleId}"), ($"El concepto {(specificConcept as MRules._ReferenceConcept).Description} no posee el valor {value}") });
                                                            }

                                                            ruleCondition.CondValue = string.IsNullOrEmpty(value.ToString()) ? string.Empty : decimal.Parse(value.ToString()).ToString();
                                                        }
                                                    }

                                                    conditionId++;
                                                    ruleConditionCount++;
                                                    listRuleCondition.Add(ruleCondition);
                                                }
                                                else
                                                {
                                                    EnRules.RuleAction ruleAction =
                                                        new EnRules.RuleAction(ruleBase.RuleBaseId, rIndex + 1, actionId)
                                                        {
                                                            EntityId = column.EntityId,
                                                            ConceptId = column.ConceptId,
                                                            ActionTypeCode = 1,
                                                            ValueTypeCode = 1,
                                                            OrderNum = 0,
                                                            OperatorCode = !string.IsNullOrEmpty(value.ToString())
                                                                ? iComparatorCd
                                                                : (int?)null,
                                                            ActionValue = null
                                                        };

                                                    if (ruleAction.OperatorCode != null)
                                                    {
                                                        if (specificConcept is MRules._BasicConcept)
                                                        {
                                                            switch ((specificConcept as MRules._BasicConcept).BasicType)
                                                            {
                                                                case EmRules.BasicType.Numeric:
                                                                    ruleAction.ActionValue = Convert.ToInt32(value).ToString();
                                                                    break;
                                                                case EmRules.BasicType.Text:
                                                                    ruleAction.ActionValue = value.ToString();
                                                                    break;
                                                                case EmRules.BasicType.Decimal:
                                                                    ruleAction.ActionValue = ConvertHelper.ConvertToDecimal(value.ToString()).ToString();
                                                                    break;
                                                                case EmRules.BasicType.Date:
                                                                    ruleAction.ActionValue =
                                                                        DateTime.ParseExact(value.ToString(), "MM/dd/yyyy", CultureInfo.InvariantCulture).ToString("MM/dd/yyyy");
                                                                    break;
                                                                default:
                                                                    exceptions.Add(new string[] { ($"{ruleAction.RuleId}"), ("El tipo de concepto basico " + (specificConcept as MRules._BasicConcept).BasicType + " no se pudo validar") });
                                                                    break;
                                                            }
                                                        }
                                                        else if (specificConcept is MRules._ListConcept)
                                                        {
                                                            if ((specificConcept as MRules._ListConcept).ListEntity.ListEntityValues.FirstOrDefault(x => x.ListValueCode == Convert.ToInt32(value)) == null)
                                                            {
                                                                exceptions.Add(new string[] { ($"{ruleAction.RuleId}"), ("El concepto " + (specificConcept as MRules._ListConcept).Description + " no posee el valor " + value) });
                                                            }

                                                            ruleAction.ActionValue = value.ToString();
                                                        }
                                                        else if (specificConcept is MRules._RangeConcept)
                                                        {
                                                            if ((specificConcept as MRules._RangeConcept).RangeEntity.RangeEntityValues.FirstOrDefault(x => x.RangeValueCode == Convert.ToInt32(value)) == null)
                                                            {
                                                                exceptions.Add(new string[] { ($"{ruleAction.RuleId}"), ("El concepto " + (specificConcept as MRules._RangeConcept).Description + " no posee el valor " + value) });
                                                            }

                                                            ruleAction.ActionValue = value.ToString();
                                                        }
                                                        else if (specificConcept is MRules._ReferenceConcept)
                                                        {
                                                            if (((MRules._ReferenceConcept)specificConcept).ConceptDependences.Count != 0)
                                                            {
                                                                List<string> dependency = new List<string>();

                                                                foreach (MRules._ConceptDependence dependence in ((MRules._ReferenceConcept)specificConcept).ConceptDependences)
                                                                {
                                                                    EnRules.RuleAction conceptTmp = listRuleAction.Cast<EnRules.RuleAction>()
                                                                        .First(x => x.EntityId == dependence.DependsConcept.Entity.EntityId &&
                                                                                    x.ConceptId == dependence.DependsConcept.ConceptId &&
                                                                                    x.RuleId == ruleAction.RuleId);

                                                                    if (conceptTmp.OperatorCode != 1 || conceptTmp.ActionValue == null)
                                                                    {
                                                                        exceptions.Add(new string[] { ($"{ruleAction.RuleId}"), ($"El concepto {dependence.DependsConcept.Description} debe ser igual a un valor constante") });
                                                                    }

                                                                    dependency.Add(conceptTmp.ActionValue);
                                                                }

                                                                specificConcept = conceptDao.GetSpecificConceptWithVales(column.ConceptId, column.EntityId, dependency.ToArray(), EmRules.ConceptType.Reference);
                                                            }


                                                            if ((specificConcept as MRules._ReferenceConcept).ReferenceValues.FirstOrDefault(x => x.Id == value.ToString()) == null)
                                                            {
                                                                exceptions.Add(new string[] { ($"{ruleAction.RuleId}"), ($"El concepto {(specificConcept as MRules._ReferenceConcept).Description} no posee el valor {value}") });
                                                            }

                                                            ruleAction.ActionValue = value.ToString();
                                                        }
                                                    }

                                                    actionId++;
                                                    ruleActionCount++;
                                                    listRuleAction.Add(ruleAction);

                                                }

                                                iComparatorCd = 1;
                                            }
                                            else
                                            {
                                                string symbol = row[column.entity_concept].ToString().Trim();
                                                if (!string.IsNullOrEmpty(symbol))
                                                {
                                                    switch (symbol)
                                                    {
                                                        case "=":
                                                            iComparatorCd = 1;
                                                            break;

                                                        case "<":
                                                        case "+":
                                                            iComparatorCd = 2;
                                                            break;

                                                        case "<=":
                                                        case "-":
                                                            iComparatorCd = 3;
                                                            break;

                                                        case ">":
                                                        case "*":
                                                            iComparatorCd = 4;
                                                            break;

                                                        case ">=":
                                                        case "/":
                                                            iComparatorCd = 5;
                                                            break;

                                                        case "!=":
                                                        case "<>":
                                                            iComparatorCd = 6;
                                                            break;
                                                        default:
                                                            exceptions.Add(new string[] { ($"{rIndex + 1}"), ("El operador " + symbol + " no es valido") });
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            exceptions.Add(new string[] { ($"{rIndex + 1}"), ($"Columna {y + 1} - La cadena de entrada no tiene el formato correcto.") });
                                        }
                                    }

                                    ruleCount++;
                                }
                                DataFacadeManager.Dispose();
                            });

                            threads.Add(thread);
                        }

                        //Se procesan los hilos
                        int threadRuleSet = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadRuleSet"]);

                        while (threads.Any(x => x.Status == TaskStatus.Created))
                        {
                            List<Task> threadsRun = threads.Where(x => x.Status == TaskStatus.Created).Skip(0).Take(threadRuleSet).ToList();
                            threadsRun.ForEach(x => x.Start());
                            Task.WaitAll(threadsRun.ToArray());
                        }
                    }
                }

                if (exceptions.Count == 1)
                {
                    return new MRules._DecisionTableMappingResult()
                    {
                        ErrorMessage = "Regla:" + exceptions[0][0].ToString() + "," + exceptions[0][1].ToString()
                    };
                }
                else if (exceptions.Count > 1)
                {
                    FileDAO fileDao = new FileDAO();
                    string file = fileDao.GenerateFileToDecisionTableByExceptions(exceptions);

                    return new MRules._DecisionTableMappingResult()
                    {
                        FileExceptions = file
                    };
                }

                try
                {
                    #region Guardado Por Store Procedure
                    DataTable createRules = new DataTable("SCR_PARAM_RULES_RULE");
                    createRules.Columns.Add("RULE_BASE_ID", typeof(int));
                    createRules.Columns.Add("RULE_ID", typeof(int));
                    createRules.Columns.Add("ORDER_NUM", typeof(int));

                    DataTable createRuleConditions = new DataTable("SCR_PARAM_RULES_CONDITION");
                    createRuleConditions.Columns.Add("RULE_BASE_ID", typeof(int));
                    createRuleConditions.Columns.Add("RULE_ID", typeof(int));
                    createRuleConditions.Columns.Add("CONDITION_ID", typeof(int));
                    createRuleConditions.Columns.Add("ENTITY_ID", typeof(int));
                    createRuleConditions.Columns.Add("CONCEPT_ID", typeof(int));
                    createRuleConditions.Columns.Add("COMPARATOR_CD", typeof(int));
                    createRuleConditions.Columns.Add("RULE_VALUE_TYPE_CD", typeof(int));
                    createRuleConditions.Columns.Add("COND_VALUE", typeof(string));
                    createRuleConditions.Columns.Add("ORDER_NUM", typeof(int));

                    DataTable createRuleActions = new DataTable("SCR_PARAM_RULES_ACTION");
                    createRuleActions.Columns.Add("RULE_BASE_ID", typeof(int));
                    createRuleActions.Columns.Add("RULE_ID", typeof(int));
                    createRuleActions.Columns.Add("ACTION_ID", typeof(int));
                    createRuleActions.Columns.Add("ACTION_TYPE_CD", typeof(int));
                    createRuleActions.Columns.Add("ENTITY_ID", typeof(int));
                    createRuleActions.Columns.Add("CONCEPT_ID", typeof(int));
                    createRuleActions.Columns.Add("OPERATOR_CD", typeof(int));
                    createRuleActions.Columns.Add("RULE_VALUE_TYPE_CD", typeof(int));
                    createRuleActions.Columns.Add("ACTION_VALUE", typeof(string));
                    createRuleActions.Columns.Add("ORDER_NUM", typeof(int));

                    Task taskRules = TP.Task.Run(() =>
                    {
                        foreach (EnRules.Rule item in listRules)
                        {
                            DataRow row = createRules.NewRow();
                            row["RULE_BASE_ID"] = item.RuleBaseId;
                            row["RULE_ID"] = item.RuleId;
                            row["ORDER_NUM"] = item.OrderNum;

                            createRules.Rows.Add(row);
                        }
                    });
                    Task taskConditions = TP.Task.Run(() =>
                    {
                        foreach (EnRules.RuleCondition item in listRuleCondition)
                        {
                            DataRow row = createRuleConditions.NewRow();
                            row["RULE_BASE_ID"] = item.RuleBaseId;
                            row["RULE_ID"] = item.RuleId;
                            row["CONDITION_ID"] = item.ConditionId;
                            row["ENTITY_ID"] = item.EntityId;
                            row["CONCEPT_ID"] = item.ConceptId;
                            row["COMPARATOR_CD"] = item.ComparatorCode ?? (object)DBNull.Value;
                            row["RULE_VALUE_TYPE_CD"] = item.RuleValueTypeCode;
                            row["COND_VALUE"] = item.CondValue ?? (object)DBNull.Value;
                            row["ORDER_NUM"] = item.OrderNum;

                            createRuleConditions.Rows.Add(row);
                        }
                    });
                    Task taskActions = TP.Task.Run(() =>
                    {
                        foreach (EnRules.RuleAction item in listRuleAction)
                        {
                            DataRow row = createRuleActions.NewRow();
                            row["RULE_BASE_ID"] = item.RuleBaseId;
                            row["RULE_ID"] = item.RuleId;
                            row["ACTION_ID"] = item.ActionId;
                            row["ACTION_TYPE_CD"] = item.ActionTypeCode;
                            row["ENTITY_ID"] = item.EntityId ?? (object)DBNull.Value;
                            row["CONCEPT_ID"] = item.ConceptId ?? (object)DBNull.Value;
                            row["OPERATOR_CD"] = item.OperatorCode ?? (object)DBNull.Value;
                            row["RULE_VALUE_TYPE_CD"] = item.ValueTypeCode;
                            row["ACTION_VALUE"] = item.ActionValue ?? (object)DBNull.Value;
                            row["ORDER_NUM"] = item.OrderNum;

                            createRuleActions.Rows.Add(row);
                        }
                    });

                    Task.WaitAll(taskRules, taskConditions, taskActions);
                    NameValue[] parameters = new NameValue[7];
                    parameters[0] = new NameValue("@Description", ruleBase.Description);
                    parameters[1] = new NameValue("@Package", ruleBase.PackageId);
                    parameters[2] = new NameValue("@Level", ruleBase.LevelId);
                    parameters[3] = new NameValue("@IsEvent", ruleBase.IsEvent);
                    parameters[4] = new NameValue("@Rules", createRules);
                    parameters[5] = new NameValue("@Conditions", createRuleConditions);
                    parameters[6] = new NameValue("@Actions", createRuleActions);

                    DataTable dataTable;

                    using (DynamicDataAccess pdb = new DynamicDataAccess())
                    {
                        dataTable = pdb.ExecuteSPDataTable("SCR.INSERT_DESICION_TABLE", parameters);
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        ruleBase.RuleBaseId = int.Parse(row[0].ToString());
                        ruleBase.RuleBaseVersion = int.Parse(row[1].ToString());
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Error al importar la tabla de decisión", e);
                }
                #endregion
            }
            MRules._RuleBase ruleBaseResult = _ModelAssembler.CreateRuleBase(ruleBase);
            ruleBaseResult.Package = new MRules._Package { PackageId = package.PackageId, Description = package.Description };
            ruleBaseResult.Level = new MRules._Level { LevelId = level.LevelId, Description = level.Description };

            return new MRules._DecisionTableMappingResult
            {
                RuleBase = ruleBaseResult,
                CountCondition = ruleConditionCount,
                CountActions = ruleActionCount,
                CountRules = ruleCount
            };
        }
        //fin de cambio para SAP ASE

        public void DeleteFiles()
        {
            if (File.Exists(_pathXml))
            {
                File.Delete(_pathXml);
            }
            if (File.Exists(_pathXls))
            {
                File.Delete(_pathXls);
            }
        }

        public void CreateDataSet()
        {
            try
            {
                _dataSet = new DataSet();
                foreach (MRules.DecisionTableMappingExcelPage excelPage in _decisionTableMapping.ExcelPage)
                {
                    _dataSet.Tables.Add(excelPage.name);
                    foreach (MRules.DecisionTableMappingExcelPageColumn column in excelPage.Column)
                    {
                        _dataSet.Tables[excelPage.name].Columns.Add(column.entity_concept, typeof(string));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetDataSet()
        {
            return _dataSet.Tables[0];
        }
    }

    public class ImportHelper
    {
        public List<string[]> ErrorMessage { get; }

        public ImportHelper()
        {
            ErrorMessage = new List<string[]>();
        }

        public async Task<MRules._RuleSet> ImportRuleSet(MRules._RuleSet ruleSet)
        {
            _RuleSetDao ruleSetDao = new _RuleSetDao();
            XmlHelperWriter xmlHelperWriter = new XmlHelperWriter();

            for (int r = 0; r < ruleSet.Rules.Count; r++)
            {
                MRules._Rule rule = ruleSet.Rules[r];
                for (int c = 0; c < rule.Conditions.Count; c++)
                {
                    MRules._Condition condition = rule.Conditions[c];
                    ruleSet.Rules[r].Conditions[c].Concept = ImportConcept(condition.Concept, rule.Description);
                    ruleSet.Rules[r].Conditions[c].Expression = ImportConditionValue(condition, rule.Description);
                }
                for (int a = 0; a < rule.Actions.Count; a++)
                {
                    MRules._Action action = rule.Actions[a];
                    switch (action.AssignType)
                    {
                        case EmRules.AssignType.ConceptAssign:
                            ((MRules._ActionConcept)rule.Actions[a]).Concept = ImportConcept(((MRules._ActionConcept)action).Concept, rule.Description);
                            break;

                        case EmRules.AssignType.InvokeAssign:
                        case EmRules.AssignType.TemporalAssign:
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("Tipo de accion no valida " + action.AssignType);
                    }

                    action.Expression = ImportActionValue(action, rule.Description);
                }
            }

            if (ErrorMessage.Count == 1)
            {
                throw new BusinessException($"Error en la regla: {ErrorMessage[0][0]}, {ErrorMessage[0][1]}");
            }
            else if (ErrorMessage.Count > 1)
            {
                FileDAO fileDao = new FileDAO();
                string fileWithExcepctions = fileDao.GenerateFileToDecisionTableByExceptions(ErrorMessage, "Error al importar reglas");
                return new MRules._RuleSet() { FileExceptions = fileWithExcepctions, HasError = true, Type = EmRules.RuleBaseType.Sequence };
            }

            MRules._RuleSet ruleTmp = ruleSetDao.GetRulesByFilter(ruleSet.Package.PackageId, new List<int> { ruleSet.Level.LevelId }, false, (bool)ruleSet.IsEvent, ruleSet.Description, true)
                .FirstOrDefault(x => x.Description == ruleSet.Description);
            try
            {
                if (ruleTmp == null)
                {
                    ruleSet.RuleSetId = 0;
                    ruleSet = ruleSetDao.CreateRuleSet(ruleSet, await xmlHelperWriter.GetXmlByRuleSet(ruleSet));
                }
                else
                {
                    ruleSet.RuleSetId = ruleTmp.RuleSetId;
                    ruleSet.RuleSetVer = ruleTmp.RuleSetVer;
                    ruleSet = ruleSetDao.UpdateRuleSet(ruleSet, await xmlHelperWriter.GetXmlByRuleSet(ruleSet));
                }
            }
            catch (Exception e)
            {
                throw new ValidationException(e.Message, e);
            }

            return ruleSet;
        }

        private dynamic ImportActionValue(MRules._Action action, string nameRule)
        {
            _RuleSetDao ruleSetDao = new _RuleSetDao();
            _RuleFunctionDao functionDao = new _RuleFunctionDao();

            switch (action.AssignType)
            {
                case EmRules.AssignType.ConceptAssign:
                    {
                        switch (((MRules._ActionConcept)action).ComparatorType)
                        {
                            case EmRules.ComparatorType.ConstantValue:
                                return ImportValueConcept(((MRules._ActionConcept)action).Concept, ((MRules._ActionConcept)action).Expression, nameRule);

                            case EmRules.ComparatorType.ExpressionValue:
                            case EmRules.ComparatorType.TemporalyValue:
                                return action.Expression;

                            case EmRules.ComparatorType.ConceptValue:
                                return ImportConcept(action.Expression, nameRule);

                            default:
                                throw new ArgumentOutOfRangeException("Tipo de comparador no valido " + ((MRules._ActionConcept)action).ComparatorType);
                        }
                    }
                case EmRules.AssignType.InvokeAssign:
                    {
                        switch (((MRules._ActionInvoke)action).InvokeType)
                        {
                            case EmRules.InvokeType.MessageInvoke:
                                return action.Expression;

                            case EmRules.InvokeType.RuleSetInvoke://valida si existe la regla
                                MRules._RuleSet invokeRule = (MRules._RuleSet)action.Expression;

                                invokeRule = ruleSetDao.GetRulesByFilter(invokeRule.Package.PackageId, new List<int> { invokeRule.Level.LevelId }, true, (bool)invokeRule.IsEvent, invokeRule.Description, true)
                                    .FirstOrDefault(x => x.Description == invokeRule.Description);

                                if (invokeRule == null)
                                {
                                    ErrorMessage.Add(new string[] { nameRule, string.Format(Resources.Errors.RuleNotExist, action.Expression.Description) });
                                }
                                return invokeRule;

                            case EmRules.InvokeType.FunctionInvoke://valida si existe la funcion
                                MRules._RuleFunction ruleFunction = (MRules._RuleFunction)action.Expression;

                                ruleFunction = functionDao.GetRuleFunctionByNameRuleFunction(ruleFunction.FunctionName);
                                if (ruleFunction == null)
                                {
                                    ErrorMessage.Add(new string[] { nameRule, string.Format(Resources.Errors.FunctionNotExist, action.Expression.FunctionName) });
                                }
                                return ruleFunction;

                            default:
                                throw new ArgumentOutOfRangeException("Tipo de invoke no es valido " + ((MRules._ActionInvoke)action).InvokeType);
                        }
                    }
                case EmRules.AssignType.TemporalAssign:
                    switch (((MRules._ActionValueTemp)action).ComparatorType)
                    {
                        case EmRules.ComparatorType.ConstantValue:
                        case EmRules.ComparatorType.ExpressionValue:
                        case EmRules.ComparatorType.TemporalyValue:
                            return action.Expression;

                        case EmRules.ComparatorType.ConceptValue:
                            return ImportConcept(action.Expression, nameRule);

                        default:
                            throw new ArgumentOutOfRangeException("Tipo de comparador no valido " + ((MRules._ActionValueTemp)action).ComparatorType);
                    }

                default:
                    throw new ArgumentOutOfRangeException("Tipo de accion no es valido " + action.AssignType);
            }
        }

        private dynamic ImportConditionValue(MRules._Condition condition, string nameRule)
        {
            switch (condition.ComparatorType)
            {
                case EmRules.ComparatorType.ConstantValue:
                    return ImportValueConcept(condition.Concept, condition.Expression, nameRule);

                case EmRules.ComparatorType.ConceptValue:
                    return ImportConcept(condition.Expression, nameRule);

                case EmRules.ComparatorType.ExpressionValue:
                case EmRules.ComparatorType.TemporalyValue:
                    return condition.Expression;

                default:
                    throw new ArgumentOutOfRangeException("Tipo de comparador no valido " + condition.ComparatorType);
            }
        }

        private MRules._Concept ImportConcept(MRules._Concept concept, string nameRule)
        {
            _ConceptDao conceptDao = new _ConceptDao();
            _RangeEntityDao rangeEntityDao = new _RangeEntityDao();
            _ListEntityDao listEntityDao = new _ListEntityDao();
            _PositionEntityDao positionEntityDao = new _PositionEntityDao();

            List<MRules._Concept> concepts = conceptDao.GetConceptByFilter(new List<int> { concept.Entity.EntityId }, concept.ConceptName);
            if (concepts.Count == 0)//no existe el concepto
            {
                switch (concept.ConceptType)
                {
                    case EmRules.ConceptType.Basic:
                        break;

                    case EmRules.ConceptType.Range:
                        MRules._RangeEntity rangeEntity = rangeEntityDao.GetRangeEntityByFilter(((MRules._RangeConcept)concept).RangeEntity.DescriptionRange).FirstOrDefault(x => x.DescriptionRange == ((MRules._RangeConcept)concept).RangeEntity.DescriptionRange);
                        if (rangeEntity == null) //se crea la lista de rangos con sus valores
                        {
                            rangeEntity = rangeEntityDao.CreateRangeEntity(((MRules._RangeConcept)concept).RangeEntity);
                        }
                        else
                        {
                            rangeEntity = rangeEntityDao.GetRangeEntityById(rangeEntity.RangeEntityCode);
                        }
                        ((MRules._RangeConcept)concept).RangeEntity = rangeEntity;

                        break;

                    case EmRules.ConceptType.List:
                        MRules._ListEntity listEntity = listEntityDao.GetListEntityByFilter(((MRules._ListConcept)concept).ListEntity.DescriptionList).FirstOrDefault(x => x.DescriptionList == ((MRules._ListConcept)concept).ListEntity.DescriptionList);
                        if (listEntity == null) //se crea la lista de rangos con sus valores
                        {
                            listEntity = listEntityDao.CreateListEntity(((MRules._ListConcept)concept).ListEntity);
                        }
                        else
                        {
                            listEntity = listEntityDao.GetListEntityById(listEntity.ListEntityCode);
                        }
                        ((MRules._ListConcept)concept).ListEntity = listEntity;
                        break;

                    case EmRules.ConceptType.Reference:
                        Models.Entity entity = positionEntityDao.GetEntitiesByPackageIdLevelId(((MRules._ReferenceConcept)concept).FEntity.PackageId, ((MRules._ReferenceConcept)concept).FEntity.LevelId).FirstOrDefault(x => x.EntityName == ((MRules._ReferenceConcept)concept).FEntity.EntityName);
                        if (entity == null) //se crea la lista de rangos con sus valores
                        {
                            ErrorMessage.Add(new string[] { nameRule, string.Format(Resources.Errors.EntityNotExist, ((MRules._ReferenceConcept)concept).FEntity.EntityName, ((MRules._ReferenceConcept)concept).Description) });
                            return null;
                            //throw new ValidationException(string.Format(Resources.Errors.EntityNotExist, ((MRules._ReferenceConcept)concept).FEntity.EntityName, ((MRules._ReferenceConcept)concept).Description));
                        }
                        ((MRules._ReferenceConcept)concept).FEntity = entity;
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("Tipo de concepto no valido " + concept.ConceptType);
                }

                MRules._Concept conceptTmp = conceptDao.InsertConcept(concept);
                concept.ConceptId = conceptTmp.ConceptId;
                return concept;
            }
            else //existe el concepto
            {
                if (concepts.First().ConceptType != concept.ConceptType)
                {
                    ErrorMessage.Add(new string[] { nameRule, string.Format(Resources.Errors.ConceptNotType, concept.Description, concept.ConceptType) });
                    return null;
                    //throw new ValidationException(string.Format(Resources.Errors.ConceptNotType, concept.Description, concept.ConceptType));
                }

                MRules._Concept conceptTmp = (MRules._Concept)conceptDao.GetSpecificConceptWithVales(concepts.First().ConceptId, concepts.First().Entity.EntityId, null, concepts.First().ConceptType);
                switch (conceptTmp.ConceptType)
                {
                    case EmRules.ConceptType.Basic:
                        {
                            if (((MRules._BasicConcept)conceptTmp).BasicType != ((MRules._BasicConcept)concept).BasicType)
                            {
                                ErrorMessage.Add(new string[] { nameRule, string.Format(Resources.Errors.ConceptNotType, concept.Description, ((MRules._BasicConcept)concept).BasicType) });
                                return null;
                                //throw new ValidationException(string.Format(Resources.Errors.ConceptNotType, concept.Description, ((MRules._BasicConcept)concept).BasicType));
                            }
                        }
                        return (MRules._BasicConcept)conceptTmp;

                    case EmRules.ConceptType.Range:
                        return (MRules._RangeConcept)conceptTmp;

                    case EmRules.ConceptType.List:
                        return (MRules._ListConcept)conceptTmp;

                    case EmRules.ConceptType.Reference:
                        return (MRules._ReferenceConcept)conceptTmp;

                    default:
                        throw new ArgumentOutOfRangeException("Tipo de concepto no valido" + conceptTmp.ConceptType);
                }
            }
        }

        private dynamic ImportValueConcept(MRules._Concept concept, dynamic expression, string nameRule)
        {
            if (expression == null || concept == null)
            {
                return null;
            }

            _RangeEntityDao rangeEntityDao = new _RangeEntityDao();
            _ListEntityDao listEntityDao = new _ListEntityDao();

            switch (concept.ConceptType)
            {
                case EmRules.ConceptType.Basic:
                    return expression;

                case EmRules.ConceptType.Range:
                    MRules._RangeEntityValue rangeEntityValue = ((MRules._RangeConcept)concept).RangeEntity.RangeEntityValues.FirstOrDefault(x => x.ToValue == expression.ToValue && x.FromValue == expression.FromValue);
                    if (rangeEntityValue == null)//inserta el valor del rango
                    {
                        expression.RangeValueCode = ((MRules._RangeConcept)concept).RangeEntity.RangeEntityValues.Max(x => x.RangeValueCode) + 1;
                        rangeEntityValue = rangeEntityDao.CreateRangeEntityValue(((MRules._RangeConcept)concept).RangeEntity.RangeEntityCode, expression);
                    }
                    return rangeEntityValue;

                case EmRules.ConceptType.List:
                    MRules._ListEntityValue listEntityValue = ((MRules._ListConcept)concept).ListEntity.ListEntityValues.FirstOrDefault(x => x.ListValue == expression.ListValue);
                    if (listEntityValue == null)//inserta el valor de la lista
                    {
                        expression.ListValueCode = ((MRules._ListConcept)concept).ListEntity.ListEntityValues.Max(x => x.ListValueCode) + 1;
                        listEntityValue = listEntityDao.CreateListEntityValue(((MRules._ListConcept)concept).ListEntity.ListEntityCode, expression);
                    }
                    return listEntityValue;

                case EmRules.ConceptType.Reference:
                    MRules._ReferenceValue referenceValue = ((MRules._ReferenceConcept)concept).ReferenceValues.FirstOrDefault(x => x.Description == expression.Description);
                    if (referenceValue == null)
                    {
                        ErrorMessage.Add(new string[] { nameRule, string.Format(Resources.Errors.ConceptNotReference, concept.Description, expression.Description) });
                        //throw new ValidationException(string.Format(Resources.Errors.ConceptNotReference, concept.Description, expression.Description));
                    }
                    return referenceValue;

                default:
                    throw new ArgumentOutOfRangeException("Tipo de concepto no valido " + concept.ConceptType);
            }
        }
    }
}
