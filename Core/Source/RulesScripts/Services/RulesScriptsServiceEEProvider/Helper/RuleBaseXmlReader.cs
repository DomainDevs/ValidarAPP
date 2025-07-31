using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.CodeDom;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.Enums;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// Summary description for RuleSetFileReader.
    /// </summary>
    public class RuleBaseXmlReader
    {
        private string _namespace = "http://www.sistran.com/RuleEngine/rules.xsd";
        private string _attNamespace = string.Empty;

        public RuleBaseDef LoadFromFile(string fileName)
        {
            using (System.IO.FileStream fs = System.IO.File.OpenRead(fileName))
            {
                return LoadFromStream(fs);
            }
        }

        public RuleBaseDef LoadFromStream(System.IO.Stream s)
        {
            XPathDocument doc = new XPathDocument(s);
            XPathNavigator nav = doc.CreateNavigator();

            XPathNodeIterator ite = nav.Select("/node()");
            if (!ite.MoveNext())
            {
                throw new Exception("Formato incorrecto del archivo: falta el nodo raiz.");
            }

            return LoadRuleBaseDef(ite.Current);
        }

        public RuleBaseDef LoadRuleBaseDef(XPathNavigator nav)
        {
            XPathNodeIterator ite = nav.Select("/node()");

            if (!ite.MoveNext())
            {
                throw new Exception("Formato incorrecto del archivo: falta el nodo raiz.");
            }

            nav = ite.Current;

            RuleBaseDef rbDef = new RuleBaseDef();
            rbDef.Name = nav.GetAttribute("name", _attNamespace);

            ite = nav.SelectChildren("rule", _namespace);
            while (ite.MoveNext())
            {
                rbDef.RuleSet.Add(LoadRuleDef(ite.Current));
            }

            string s = nav.GetAttribute("type", _attNamespace);
            if (s == null || s.Equals(string.Empty))
            {
                rbDef.Type = RuleBaseType.Sequence;
            }
            else
            {
                rbDef.Type = (RuleBaseType)Enum.Parse(typeof(RuleBaseType), s, true);
            }

            return rbDef;
        }

        private RuleDef LoadRuleDef(XPathNavigator nav)
        {
            RuleDef def = new RuleDef(nav.GetAttribute("name", _attNamespace));

            XPathNodeIterator ite = nav.SelectChildren("parameter", _namespace);
            while (ite.MoveNext())
            {
                XPathNavigator par = ite.Current;

                string parName = par.GetAttribute("name", _attNamespace);
                XPathNodeIterator iteClass = par.SelectChildren("class", _namespace);
                if (!iteClass.MoveNext())
                {
                    throw new Exception("Falta el tipo del parametro.");
                }

                string className = iteClass.Current.Value;

                className = className.Split(',')[0];
                def.Parameters.Add(new CodeParameterDeclarationExpression(className, parName));
            }

            ite = nav.SelectChildren("condition", _namespace);
            while (ite.MoveNext())
            {
                XPathNodeIterator ite2 = ite.Current.SelectChildren(XPathNodeType.Element);

                if (ite2.MoveNext())
                {
                    def.Conditions.Add(LoadExpression(ite2.Current));
                }
            }

            ite = nav.SelectChildren("consequence", _namespace);
            if (ite.MoveNext())
            {
                CodeStatement[] stmts = LoadStatements(ite.Current);
                if (stmts != null)
                {
                    def.Consequence.AddRange(stmts);
                }
            }

            return def;
        }

        private Type GetPrimitiveType(string s)
        {
            if (s.Equals("String"))
            {
                return typeof(string);
            }

            if (s.Equals("Integer"))
            {
                return typeof(int);
            }

            if (s.Equals("Boolean"))
            {
                return typeof(bool);
            }

            if (s.Equals("Decimal"))
            {
                return typeof(decimal);
            }

            if (s.Equals("DateTime"))
            {
                return typeof(DateTime);
            }

            throw new Exception("No se encontró el tipo para el dato primitivo (" + s + ").");
        }

        private string GetPrimitiveTypeName(Type t)
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

        private CodeExpression LoadExpression(XPathNavigator nav)
        {
            string name = nav.Name;
            if (name.Equals("code"))
            {
                return new CodeSnippetExpression(nav.Value.Trim());
            }

            if (name.Equals("binary-op"))
            {
                string s = nav.GetAttribute("type", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("No se definió el tipo para el operador binario.");
                }

                XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                if (ite.Count != 2)
                {
                    throw new Exception("El operador binario debe contener 2 expresiones.");
                }

                ite.MoveNext();
                CodeExpression exp1 = LoadExpression(ite.Current);
                ite.MoveNext();
                CodeExpression exp2 = LoadExpression(ite.Current);
                CodeBinaryOperatorType op = (CodeBinaryOperatorType)Enum.Parse(typeof(CodeBinaryOperatorType), s, true);

                return new CodeBinaryOperatorExpression(exp1, op, exp2);
            }

            if (name.Equals("this"))
            {
                return new CodeThisReferenceExpression();
            }

            if (name.Equals("argument"))
            {
                string s = nav.GetAttribute("name", _attNamespace);

                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el nombre para la referencia a parametro.");
                }

                return new CodeArgumentReferenceExpression(s);
            }

            if (name.Equals("primitive"))
            {
                string val = nav.GetAttribute("value", _attNamespace);
                if (val == null || val.Equals(string.Empty))
                {
                    return new CodePrimitiveExpression(null);
                }

                string s = nav.GetAttribute("type", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el tipo para el dato primitivo.");
                }

                Type p = GetPrimitiveType(s);
                return new CodePrimitiveExpression(Convert.ChangeType(val, p, System.Globalization.CultureInfo.InvariantCulture));
            }

            if (name.Equals("property"))
            {
                string s = nav.GetAttribute("name", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el nombre de la propiedad.");
                }

                XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                if (!ite.MoveNext())
                {
                    throw new Exception("No se definió el objeto de la propiedad.");
                }

                CodeExpression exp = LoadExpression(ite.Current);

                return new CodePropertyReferenceExpression(exp, s);
            }

            if (name.Equals("field"))
            {
                string s = nav.GetAttribute("name", _attNamespace);

                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el nombre para el atributo.");
                }

                XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                if (!ite.MoveNext())
                {
                    return new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), s);
                }

                return new CodeFieldReferenceExpression(LoadExpression(ite.Current), s);
            }

            if (name.Equals("cast"))
            {
                string s = nav.GetAttribute("type", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el tipo para el cast.");
                }

                XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);

                if (!ite.MoveNext())
                {
                    throw new Exception("No se definió la expresión del cast.");
                }

                CodeExpression exp = LoadExpression(ite.Current);
                return new CodeCastExpression(s, exp);
            }

            if (name.Equals("indexer"))
            {
                XPathNodeIterator ite = nav.SelectChildren("target", _namespace);
                if (!ite.MoveNext())
                {
                    throw new Exception("No se definió la expresión a indexar.");
                }

                if (!ite.Current.MoveToFirstChild())
                {
                    throw new Exception("No se definió la expresión a indexar.");
                }

                CodeExpression exp = LoadExpression(ite.Current);

                ite = nav.SelectChildren("indices", _namespace);
                if (!ite.MoveNext())
                {
                    throw new Exception("No se definió ningún índice.");
                }

                if (!ite.Current.MoveToFirstChild())
                {
                    throw new Exception("No se definió ningún índice.");
                }

                List<CodeExpression> indices = new List<CodeExpression>();
                indices.Add(LoadExpression(ite.Current));

                while (ite.MoveNext())
                {
                    indices.Add(LoadExpression(ite.Current));
                }

                return new CodeIndexerExpression(exp, indices.ToArray());
            }

            if (name.Equals("invoke"))
            {
                string s = nav.GetAttribute("method", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("No se definió el nombre del método a invocar.");
                }

                XPathNodeIterator ite = nav.SelectChildren("target", _namespace);
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
                ite = nav.SelectChildren("params", _namespace);
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

                return new CodeMethodInvokeExpression(target, s, pars);
            }

            if (name.Equals("object-create"))
            {
                string s = nav.GetAttribute("type", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el tipo del objeto a crear.");
                }

                XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                CodeExpression[] pars = new CodeExpression[ite.Count];

                int i = 0;
                while (ite.MoveNext())
                {
                    pars[i] = LoadExpression(ite.Current);
                    i++;
                }

                return new CodeObjectCreateExpression(s, pars);
            }

            if (name.Equals("type-of"))
            {
                string s = nav.GetAttribute("type", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el tipo.");
                }

                return new CodeTypeOfExpression(s);
            }

            if (name.Equals("type"))
            {
                string s = nav.GetAttribute("name", _attNamespace);
                if (s == null || s.Equals(string.Empty))
                {
                    throw new Exception("Debe especificar el nombre del tipo.");
                }

                return new CodeTypeReferenceExpression(s);
            }

            throw new Exception("Expresión no reconocida (" + name + ").");
        }
       

        private CodeStatement[] LoadStatements(XPathNavigator nav)
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

        private CodeStatement LoadStatement(XPathNavigator nav)
        {
            string name = nav.Name;

            if (name.Equals("assign"))
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

            if (name.Equals("if"))
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

            if (name.Equals("invoke"))
            {
                return new CodeExpressionStatement(LoadExpression(nav));
            }

            if (name.Equals("throw"))
            {
                XPathNodeIterator ite = nav.SelectChildren(XPathNodeType.Element);
                if (!ite.MoveNext())
                {
                    throw new Exception("Falta el objeto del throw.");
                }

                return new CodeThrowExceptionStatement(LoadExpression(ite.Current));
            }

            if (name.Equals("iteration"))
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

            if (name.Equals("code"))
            {
                return new CodeSnippetStatement(nav.Value.Trim());
            }

            throw new Exception("No se reconoció el elemento de sentencia.");
        }

        public void SaveRuleBaseDef(XmlWriter writer, RuleBaseDef rb)
        {
            writer.WriteStartElement("rule-set");
            writer.WriteAttributeString("name", rb.Name);
            writer.WriteAttributeString("type", rb.Type.ToString());
            writer.WriteAttributeString("xmlns", _namespace);

            foreach (RuleDef rule in rb.RuleSet)
            {
                SaveRuleDef(writer, rule);
            }

            writer.WriteEndElement(); // rule-set
        }

        public void SaveParameterDef(XmlWriter writer, CodeParameterDeclarationExpression par)
        {
            writer.WriteStartElement("parameter");
            writer.WriteAttributeString("name", par.Name);

            writer.WriteStartElement("class");
            writer.WriteString(par.Type.BaseType);

            writer.WriteEndElement(); // class
            writer.WriteEndElement(); // parameter
        }

        public void SaveRuleDef(XmlWriter writer, RuleDef r)
        {
            writer.WriteStartElement("rule");
            writer.WriteAttributeString("name", r.Name);

            foreach (CodeParameterDeclarationExpression par in r.Parameters)
            {
                SaveParameterDef(writer, par);
            }

            foreach (CodeExpression cond in r.Conditions)
            {
                writer.WriteStartElement("condition");
                SaveExpression(writer, cond);
                writer.WriteEndElement(); // condition
            }

            writer.WriteStartElement("consequence");
            SaveStatements(writer, r.Consequence);
            writer.WriteEndElement(); // consequence

            writer.WriteEndElement(); // rule-set
        }

        public void SaveExpression(XmlWriter writer, CodeExpression exp)
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

        public void SaveStatements(XmlWriter writer, CodeStatementCollection stmts)
        {
            foreach (CodeStatement stmt in stmts)
            {
                SaveStatement(writer, stmt);
            }
        }

        public void SaveStatement(XmlWriter writer, CodeStatement stmt)
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
                    SaveStatements(writer, @if.TrueStatements);
                    writer.WriteEndElement();
                }

                if (@if.FalseStatements != null && @if.FalseStatements.Count > 0)
                {
                    writer.WriteStartElement("false");
                    SaveStatements(writer, @if.FalseStatements);
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
                SaveStatement(writer, ite.InitStatement);
                writer.WriteEndElement();

                writer.WriteStartElement("inc");
                SaveStatement(writer, ite.IncrementStatement);
                writer.WriteEndElement();

                writer.WriteStartElement("test");
                SaveExpression(writer, ite.TestExpression);
                writer.WriteEndElement();

                writer.WriteStartElement("loop");
                SaveStatements(writer, ite.Statements);
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
	}
}
