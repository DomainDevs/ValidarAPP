using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Views;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using PAEN = Sistran.Core.Application.Parameters.Entities;
using RuleSerEnum = Sistran.Core.Application.RulesScriptsServices.Enums;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// Summary description for RuleSetDefinitionReader.
    /// </summary>
    public class RuleSetDefinitionReader
    {

        RuleDefCollection _ruleDefs;
        IDictionary _params;
        IDictionary _properties;
        IList _rules;

        public RuleSetDefinitionReader(
            RuleDefCollection rules)
        {
            this._ruleDefs = rules;
            this._rules = new ArrayList(rules.Count);
            this._params = new HybridDictionary();
            this._properties = new HybridDictionary();
        }


       
        public IList GetRuleCollection()
        {
            foreach (RuleDef rdef in this._ruleDefs)
            {
                this._rules.Add(this.GetRule(rdef));
            }
            return this._rules;
        }

        public RuleDTO GetRule(RuleDef ruleDef)
        {
            RuleDTO rule = new RuleDTO();
            rule.Name = ruleDef.Name;

            foreach (CodeParameterDeclarationExpression pexp in ruleDef.Parameters)
            {
                this.AddParameter(pexp);
            }

            foreach (CodeExpression cexp in ruleDef.Conditions)
            {
                rule.Conditions.Add(this.GetCondition((CodeBinaryOperatorExpression)cexp));
            }

            foreach (CodeStatement conExp in ruleDef.Consequence)
            {
                rule.Actions.Add(this.GetAction((CodeStatement)conExp));
            }
            return rule;
        }

        private void AddParameter(CodeParameterDeclarationExpression exp)
        {
            string[] typestring = exp.Type.BaseType.Split('.');
            string entity = (string)typestring.GetValue(typestring.Length - 1);
            string package = exp.Type.BaseType.Substring(0, exp.Type.BaseType.LastIndexOf('.'));
            package = package.Substring(0, package.LastIndexOf('.'));

            int packageId = this.GetPackage(package).PackageId;
            int entityId = this.GetEntity(packageId, entity).EntityId;
            if (!this._params.Contains(exp.Name))
            {
                this._params.Add(exp.Name, entityId);
            }
        }

        private ConditionDTO GetCondition(CodeBinaryOperatorExpression expr)
        {
            ConditionDTO condition = new ConditionDTO();

            CodeExpression left = expr.Left;
            if (left is CodePropertyReferenceExpression)
            {
                condition.Concept = this.GetConceptExpression((CodePropertyReferenceExpression)left);
            }
            else if (left is CodeMethodInvokeExpression)
            {
                CodeMethodInvokeExpression mi = (CodeMethodInvokeExpression)left;
                if (mi.Method.MethodName.Equals("GetDynamicConcept"))
                {
                    condition.Concept = this.GetConceptExpression(mi);
                }
                else
                {
                    throw new ApplicationException("Condicion no reconocida.");
                }
            }
            else if ((left is CodeCastExpression && ((CodeCastExpression)left).Expression is CodeMethodInvokeExpression))
            {
                CodeCastExpression cast = (CodeCastExpression)left;

                if (cast.Expression is CodeMethodInvokeExpression)
                {
                    CodeMethodInvokeExpression mi = (CodeMethodInvokeExpression)cast.Expression;
                    if (mi.Method.MethodName.Equals("GetDynamicConcept"))
                    {
                        condition.Concept = this.GetConceptExpression(mi);
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

            condition.ComparisionOperator = new ComparisonOperatorCollection()[Convert.ToInt32(expr.Operator)];

            CodeExpression r = expr.Right;
            CodeCastExpression castExpr = (r as CodeCastExpression);
            if (castExpr != null)
            {
                r = castExpr.Expression;
            }

            if (r is CodePrimitiveExpression)
            {
                condition.ValueType = ValueTypeCollection.ConstantValue;
            }
            else if (r is CodePropertyReferenceExpression)
            {
                condition.ValueType = ValueTypeCollection.ConceptValue;
            }
            else if (r is CodeMethodInvokeExpression || (r is CodeCastExpression && ((CodeCastExpression)r).Expression is CodeMethodInvokeExpression))
            {
                condition.ValueType = ValueTypeCollection.ConceptValue;
            }
            else if (r is CodeBinaryOperatorExpression)
            {
                condition.ValueType = ValueTypeCollection.AdvancedValue;
            }
            else
            {
                throw new ApplicationException("Tipo de valor no reconocido.");
            }

            condition.Expression = this.ProcessExpression(condition.Concept.ConceptKey, r);
            return condition;
        }

        private ActionDTO GetAction(CodeExpression expr)
        {
            return new InvokeActionDTO();
        }

        private ActionDTO GetAction(CodeStatement expr)
        {
            if (expr is CodeAssignStatement)
            {
                CodeAssignStatement statement = (CodeAssignStatement)expr;

                PrimaryKey conceptKey;
                CodeExpression left = statement.Left;
                AssignActionDTOBase action;
                CodePropertyReferenceExpression propref = (left as CodePropertyReferenceExpression);
                if (propref != null)
                {
                    AssignActionDTO assAction = new AssignActionDTO();
                    assAction.Concept = this.GetConceptExpression(propref);
                    action = assAction;

                    conceptKey = assAction.Concept.ConceptKey;
                }
                else
                {
                    conceptKey = null;
                    CodeIndexerExpression idx = (left as CodeIndexerExpression);
                    if (idx != null)
                    {
                        TemporaryAssignActionDTO tmpAction = new TemporaryAssignActionDTO();
                        tmpAction.TemporaryName = (string)((CodePrimitiveExpression)idx.Indices[0]).Value;
                        action = tmpAction;
                    }
                    else
                    {
                        throw new Exception("Assign type not supported: " + left.GetType().FullName);
                    }
                }

                CodeExpression value = statement.Right;
                while (value is CodeCastExpression)
                {
                    value = ((CodeCastExpression)value).Expression;
                }

                CodeBinaryOperatorExpression binExpr = (value as CodeBinaryOperatorExpression);
                if (binExpr != null)
                {
                    left = binExpr.Left;
                    if (left is CodePropertyReferenceExpression
                        || left is CodeCastExpression)
                    {
                        action.ArithmeticOperator = new ArithmeticOperatorCollection()[(int)binExpr.Operator];
                        value = binExpr.Right;
                    }
                    else
                    {
                        action.ArithmeticOperator = ArithmeticOperatorCollection.Assign;
                    }
                }
                else
                {
                    CodeMethodInvokeExpression mi = (value as CodeMethodInvokeExpression);
                    if (mi != null && mi.Method.MethodName == "Round")
                    {
                        action.ArithmeticOperator = ArithmeticOperatorCollection.Round;
                        value = mi.Parameters[1];
                    }
                    else
                    {
                        action.ArithmeticOperator = ArithmeticOperatorCollection.Assign;
                    }
                }

                while (value is CodeCastExpression)
                {
                    value = ((CodeCastExpression)value).Expression;
                }

                if (value is CodePrimitiveExpression)
                {
                    action.ValueType = ValueTypeCollection.ConstantValue;
                }
                else if (value is CodePropertyReferenceExpression
                    || value is CodeMethodInvokeExpression
                    || (value is CodeCastExpression && ((CodeCastExpression)value).Expression is CodeMethodInvokeExpression))
                {
                    action.ValueType = ValueTypeCollection.ConceptValue;
                }
                else if (value is CodeBinaryOperatorExpression)
                {
                    action.ValueType = ValueTypeCollection.AdvancedValue;
                }
                else if (value is CodeIndexerExpression)
                {
                    action.ValueType = ValueTypeCollection.TemporaryValue;
                }
                else
                {
                    throw new ApplicationException("No se reconoce el tipo del valor.");
                }

                action.Expression = this.ProcessExpression(conceptKey, value);
                return action;
            }
            else if (expr is CodeThrowExceptionStatement)
            {
                InvokeActionDTO action = new InvokeActionDTO();
                action.Function = InvokeFuntionCollection.InvokeMessage;

                CodeThrowExceptionStatement statement = (CodeThrowExceptionStatement)expr;
                CodeExpression exp = statement.ToThrow;

                if (exp is CodeObjectCreateExpression)
                {
                    CodeObjectCreateExpression create = (CodeObjectCreateExpression)exp;
                    exp = create.Parameters[0];
                }

                action.Message = (string)((CodePrimitiveExpression)exp).Value;
                return action;
            }
            else if (expr is CodeExpressionStatement)
            {
                CodeExpression sexp = ((CodeExpressionStatement)expr).Expression;
                if (sexp is CodeMethodInvokeExpression)
                {
                    CodeMethodInvokeExpression mi = (CodeMethodInvokeExpression)sexp;
                    string methodName = mi.Method.MethodName;
                    if (methodName.Equals("SetDynamicConcept"))
                    {
                        AssignActionDTO action = new AssignActionDTO();
                        action.Concept = this.GetConceptExpression(mi);

                        CodeExpression value = mi.Parameters[1];

                        while (value is CodeCastExpression)
                        {
                            value = ((CodeCastExpression)value).Expression;
                        }

                        if (value is CodeBinaryOperatorExpression)
                        {
                            CodeBinaryOperatorExpression binExpr = (CodeBinaryOperatorExpression)value;
                            CodeExpression left = binExpr.Left;

                            if (left is CodeMethodInvokeExpression || (left is CodeCastExpression && ((CodeCastExpression)left).Expression is CodeMethodInvokeExpression))
                            {
                                action.ArithmeticOperator = new ArithmeticOperatorCollection()[(int)binExpr.Operator];
                                value = binExpr.Right;
                            }
                            else
                            {
                                action.ArithmeticOperator = ArithmeticOperatorCollection.Assign;
                            }
                        }
                        else if (value is CodeMethodInvokeExpression && ((CodeMethodInvokeExpression)value).Method.MethodName == "Round")
                        {
                            action.ArithmeticOperator = ArithmeticOperatorCollection.Round;
                            value = ((CodeMethodInvokeExpression)value).Parameters[1];
                        }
                        else
                        {
                            action.ArithmeticOperator = ArithmeticOperatorCollection.Assign;
                        }

                        while (value is CodeCastExpression)
                        {
                            value = ((CodeCastExpression)value).Expression;
                        }

                        if (value is CodePrimitiveExpression)
                        {
                            action.ValueType = ValueTypeCollection.ConstantValue;
                        }
                        else if (value is CodePropertyReferenceExpression || value is CodeMethodInvokeExpression || (value is CodeCastExpression && ((CodeCastExpression)value).Expression is CodeMethodInvokeExpression))
                        {
                            action.ValueType = ValueTypeCollection.ConceptValue;
                        }
                        else if (value is CodeBinaryOperatorExpression)
                        {
                            action.ValueType = ValueTypeCollection.AdvancedValue;
                        }
                        else if (value is CodeIndexerExpression)
                        {
                            action.ValueType = ValueTypeCollection.TemporaryValue;
                        }
                        else
                        {
                            throw new ApplicationException("No se reconoce el tipo del valor.");
                        }

                        action.Expression = this.ProcessExpression(action.Concept.ConceptKey, value);
                        return action;
                    }
                    else if (methodName.Equals("FireRules"))
                    {
                        InvokeActionDTO action = new InvokeActionDTO();
                        action.Function = InvokeFuntionCollection.InvokeRuleSet;

                        action.Message = ((CodePrimitiveExpression)mi.Parameters[0]).Value.ToString();
                        return action;
                    }
                    else if (methodName.Equals("ExecuteFunction"))
                    {
                        InvokeActionDTO action = new InvokeActionDTO();
                        action.Function = InvokeFuntionCollection.InvokeFunction;

                        action.Message = ((CodePrimitiveExpression)mi.Parameters[0]).Value.ToString();
                        return action;
                    }
                    
                }
            }

            throw new ApplicationException("Tipo de accion no reconocida.");
        }

        private CodeExpression ProcessExpression(CodeExpression exp)
        {
            if (exp is CodePropertyReferenceExpression)
            {
                return this.GetConceptExpression((CodePropertyReferenceExpression)exp);
            }
            else if (exp is CodeMethodInvokeExpression)
            {
                return this.GetConceptExpression((CodeMethodInvokeExpression)exp);
            }
            else if (exp is CodeCastExpression)
            {
                return this.ProcessExpression(((CodeCastExpression)exp).Expression);
            }
            else if (exp is CodeBinaryOperatorExpression)
            {
                CodeBinaryOperatorExpression bexp = (CodeBinaryOperatorExpression)exp;
                CodeBinaryOperatorExpression newbexp = new CodeBinaryOperatorExpression();
                newbexp.Operator = bexp.Operator;
                newbexp.Left = this.ProcessExpression(bexp.Left);
                newbexp.Right = this.ProcessExpression(bexp.Right);
                return newbexp;
            }        

            return exp;
        }

        private Package GetPackage(string nameSpace)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Package.Properties.Namespace, nameSpace);

            IList packageList = PackageDAO.ListPackage(filter.GetPredicate(), null);

            return (Package)packageList[0];
        }

        private PAEN.Entity GetEntity(int packageId, string entityName)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(PAEN.Entity.Properties.EntityName, entityName)
            .And()
            .PropertyEquals(PAEN.Entity.Properties.PackageId, packageId);

            IList entityList = EntityDAO.ListEntity(filter.GetPredicate(), null);


            return (PAEN.Entity)entityList[0];
        }

        private CodeConceptExpression GetConceptExpression(string conceptName, string objectName)
        {
            object obj = this._properties[conceptName];
            if (obj != null)
            {
                return new CodeConceptExpression(((PrimaryKey)obj));
            }

            int entityId = (int)(this._params[objectName]);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(SCREN.Concept.Properties.EntityId, entityId)
                .And()
                .PropertyEquals(SCREN.Concept.Properties.ConceptName, conceptName);

            IList conceptList = ConceptDAO.ListConcepts(filter.GetPredicate(), null);

            SCREN.Concept concept = (SCREN.Concept)conceptList[0];
            PrimaryKey pk = concept.PrimaryKey;

            this._properties.Add(conceptName, pk);

            if (concept.IsStatic)
            {
                return new CodeConceptExpression(pk);
            }

            return new CodeDynamicConceptExpression(pk);
        }

        private CodeConceptExpression GetConceptExpression(int conceptId, string objectName)
        {
            int entityId = (int)(this._params[objectName]);

            SCREN.Concept concept = ConceptDAO.GetConceptByConceptIdEntityId(conceptId, entityId);
            string conceptName = concept.ConceptName;

            PrimaryKey pk = (PrimaryKey)this._properties[conceptName];
            if (pk == null)
            {
                pk = concept.PrimaryKey;
                this._properties.Add(conceptName, pk);
            }

            if (concept.IsStatic)
            {
                return new CodeConceptExpression(pk);
            }

            return new CodeDynamicConceptExpression(pk);
        }

        private CodeConceptExpression GetConceptExpression(
            CodeCastExpression cast)
        {
            return this.GetConceptExpression((CodeMethodInvokeExpression)cast.Expression);
        }

        private CodeConceptExpression GetConceptExpression(
            CodeMethodInvokeExpression mi)
        {
            object value = ((CodePrimitiveExpression)mi.Parameters[0]).Value;
            if (value is string)
            {
                return this.GetConceptExpression((string)value, ((CodeArgumentReferenceExpression)((CodeCastExpression)mi.Method.TargetObject).Expression).ParameterName);
            }

            if (value is int)
            {
                return this.GetConceptExpression((int)value, ((CodeArgumentReferenceExpression)((CodeCastExpression)mi.Method.TargetObject).Expression).ParameterName);
            }

            throw new Exception("Definicion de concepto no reconocido: " + string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", value));
        }

        private CodeConceptExpression GetConceptExpression(
            CodePropertyReferenceExpression prop)
        {
            return this.GetConceptExpression(prop.PropertyName, ((CodeArgumentReferenceExpression)prop.TargetObject).ParameterName);
        }

        private CodeExpression ProcessExpression(PrimaryKey concept, CodeExpression exp)
        {
            ObjectCriteriaBuilder filter;
            SCREN.Concept conceptObj;
            RuleSerEnum.ConceptType conceptType;
            if (concept == null)
            {
                conceptType = RuleSerEnum.ConceptType.Basic;
                conceptObj = null;
            }
            else
            {
                //TODO GLR ver si es necesario usar un cache por ahora uso ConceptDAO
                //conceptObj = RuleEditorCache.GetConcept((int)concept[Concept.Properties.EntityId], (int)concept[Concept.Properties.ConceptId]);
                conceptObj = ConceptDAO.GetConceptByConceptIdEntityId((int)concept[SCREN.Concept.Properties.ConceptId], (int)concept[SCREN.Concept.Properties.EntityId]);
                conceptType = (RuleSerEnum.ConceptType)conceptObj.ConceptTypeCode;
            }

            switch (conceptType)
            {
                case RuleSerEnum.ConceptType.Basic:
                    return this.ProcessExpression(exp);

                case RuleSerEnum.ConceptType.List:
                    if (exp is CodePrimitiveExpression)
                    {
                        object itemCode = ((CodePrimitiveExpression)exp).Value;
                        if (itemCode == null)
                        {
                            return exp;
                        }

                        if (itemCode is bool)
                        {
                            if ((bool)itemCode)
                            {
                                itemCode = 1;
                            }
                            else
                            {
                                itemCode = 0;
                            }
                        }


                        filter = new ObjectCriteriaBuilder();
                        filter.Property(SCREN.ListEntityValue.Properties.ListValueCode, "ListEntityValues")
                              .Equal().Constant(itemCode)
                              .And()
                              .Property(ListConcept.Properties.ConceptId, "ListConcepts")
                              .Equal().Constant(conceptObj.ConceptId)
                              .And()
                              .Property(ListConcept.Properties.EntityId, "ListConcepts")
                              .Equal().Constant(conceptObj.EntityId);

                        ListValuesView listValuesView = ListEntityDAO.GetListValuesView(filter.GetPredicate());

                        SCREN.ListEntityValue value = (SCREN.ListEntityValue)listValuesView.ListEntityValues[0];
                        return new CodeListValueExpression(value.ListValueCode, value.ListValue);
                    }

                    return this.ProcessExpression(exp);

                case RuleSerEnum.ConceptType.Range:
                    if (exp is CodePrimitiveExpression)
                    {
                        object rangeCode = ((CodePrimitiveExpression)exp).Value;
                        if (rangeCode == null)
                        {
                            return exp;
                        }

                        filter = new ObjectCriteriaBuilder();
                        filter.Property(RangeEntityValue.Properties.RangeValueCode, "RangeEntityValues")
                            .Equal().Constant(rangeCode)
                            .And()
                            .Property(SCREN.RangeConcept.Properties.ConceptId, "RangeConcepts")
                            .Equal().Constant(conceptObj.ConceptId)
                            .And()
                            .Property(SCREN.RangeConcept.Properties.EntityId, "RangeConcepts")
                            .Equal().Constant(conceptObj.EntityId);

                        RangeValuesView rangeValuesView = RangeEntityDAO.GetRangeValuesView(filter.GetPredicate());

                        RangeEntityValue rangeValue = (RangeEntityValue)rangeValuesView.RangeEntityValues[0];
                        return new CodeListValueExpression(rangeValue.Description, rangeValue.Code);
                    }

                    return this.ProcessExpression(exp);

                case RuleSerEnum.ConceptType.Reference:
                    if (exp is CodePrimitiveExpression)
                    {
                        object value = ((CodePrimitiveExpression)exp).Value;
                        if (value == null)
                        {
                            return exp;
                        }

                        int key = Convert.ToInt32(value);

                        ReferenceConcept referenceConcept = ReferenceConceptDAO.FindReferenceConcept(conceptObj.ConceptId, conceptObj.EntityId);

                        return new CodeListValueExpression(key, ExpressionHelper.GetSearchComboDescription(referenceConcept.FentityId, conceptObj.ConceptId, key.ToString()));
                    }

                    return this.ProcessExpression(exp);
            }

            return null;
        }
    }
}