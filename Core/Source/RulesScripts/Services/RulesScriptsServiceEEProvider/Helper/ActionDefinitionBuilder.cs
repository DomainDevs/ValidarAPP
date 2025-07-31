using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.Scripts.Entities;
using Sistran.Core.Framework.BAF;
using System;
using System.CodeDom;
using System.Collections;
using PAEN = Sistran.Core.Application.Parameters.Entities;
using RuleEntities = Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    public class ActionDefinitionBuilder
    {
        private IDictionary _paremeters;
        const int BOOLEAN_LIST_CODE = 2;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public ActionDefinitionBuilder(IDictionary parameters)
        {
            _paremeters = parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary Parameters
        {
            get { return _paremeters; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assignAction"></param>
        /// <returns></returns>
        /// Modificado Jonathan Moreno Acciones
        public CodeStatement GetAssignActionStatement(Models.Action action)
        {
            CodeExpression var = null;

            #region derecha
            if (action.TemporalNameLeft != null)
            {
                var = new CodeIndexerExpression(
                new CodePropertyReferenceExpression(
                new CodeThisReferenceExpression(), "LocalValues"),
                new CodePrimitiveExpression(action.TemporalNameLeft));
            }

            else if (action.ConceptLeft != null)
            {
                var = RuleHelper.GetConceptExpression(action.ConceptLeft, _paremeters, true);
            }
            #endregion

            CodeBinaryOperatorType btype = new CodeBinaryOperatorType();
            if (action.Operator != null)
            {
                btype = (System.CodeDom.CodeBinaryOperatorType)action.Operator.OperatorCode;
            }
            
            #region izquierda            
            CodeExpression value = null;

            var conceptObj = ConceptDAO.GetConceptByConceptIdEntityId(action.ConceptLeft.ConceptId, action.ConceptLeft.EntityId);
            CodeTypeReference t = RuleHelper.GetConceptTypeReference(conceptObj);                  
            
            CodeExpression var2;
            if (action.TemporalNameRight != null)
            {
                value = new CodeIndexerExpression(
                    new CodePropertyReferenceExpression(
                    new CodeThisReferenceExpression(), "LocalValues"),
                    new CodePrimitiveExpression(action.TemporalNameRight));
                value = new CodeCastExpression(typeof(decimal), value);//lineas agregadas jonathan                
            }

            if (btype != CodeBinaryOperatorType.Assign)
            {
                var2 = var;
                if (action.TemporalNameLeft != null)
                {
                    var2 = new CodeCastExpression(typeof(decimal), var);//lineas agregadas jonathan   
                }

                if (action.Operator != null)
                {                                   
                    if (action.Operator.OperatorCode == 6 && action.Operator != null) //Redondear
                    {
                        CodeExpression val = new CodePrimitiveExpression(action.ValueRight);
                        value = new CodeMethodInvokeExpression(
                            new CodeTypeReferenceExpression(typeof(Sistran.Core.Framework.Math.BusinessMath)), "Round",
                            new CodeCastExpression(typeof(decimal), var2),
                            new CodeCastExpression(typeof(int), val));
                    }
                    else
                    {
                        value = new CodeBinaryOperatorExpression(var2, btype, value);
                    }
                }
            }
                        
            if (action.ConceptRight != null)
            {
                value = RuleHelper.GetConceptExpression(action.ConceptRight, _paremeters, false);                
            }
            else {
                value = ProcessExpression(action);
            }

            #endregion
            if (ConceptDAO.GetConceptByConceptIdEntityId(action.ConceptLeft.ConceptId, action.ConceptLeft.EntityId).IsStatic)                        
            {
                value = new CodeCastExpression(t, value);
                return new CodeAssignStatement(var, value);
            }
            else {
                var = new CodeMethodInvokeExpression(new CodeCastExpression(typeof(IDynamicConceptContainer), var), "SetDynamicConcept",
                new CodeExpression[] { new CodePrimitiveExpression(action.ConceptLeft.ConceptId), new CodeCastExpression(t, value) });
                return new CodeExpressionStatement(var);
            }            
        }

        private CodeExpression ProcessExpression(Models.Action action)
        {
            Models.ConceptControl conceptControl = RuleSetDAO.GetConceptControl(action.ConceptLeft.ConceptId, action.ConceptLeft.EntityId);
            
            CodeExpression expr = new CodePrimitiveExpression(action.ValueRight);

            // es un tipo basico
            if (action.ValueType == Sistran.Core.Application.RulesScriptsServices.Enums.ValueType.Value && (int)conceptControl.ConceptControlCode < 4)
            {                
                if ((int)conceptControl.BasicType == 1)
                {
                    return new CodePrimitiveExpression(Convert.ToInt32(action.ValueRight));
                }
                if ((int)conceptControl.BasicType == 3 && action.ValueRight != "")
                {
                    return new CodePrimitiveExpression(Convert.ToDecimal(action.ValueRight));
                }
                if ((int)conceptControl.BasicType == 4)
                {
                    string date = (DateTime.Parse(action.ValueRight.ToString())).ToString("dd/MM/yyyy");
                    DateTime dt = Convert.ToDateTime(date);
                    return new CodePrimitiveExpression(dt);
                }
                else
                {
                    return expr;
                }

            }
            
            // es un tipo lista
            if ((int)conceptControl.ConceptTypeCode == 3)//condition.Concept
            {
                SCREN.ListConcept lc = ListConceptDAO.FindListConcept(action.ConceptLeft.ConceptId, action.ConceptLeft.EntityId);
                if (lc == null)
                {
                    throw new Exception("No se encontro la lista relacionada al concepto.");
                }

                if (lc.ListEntityCode == BOOLEAN_LIST_CODE && action.ValueRight.ToString() != "")
                {
                    return new CodePrimitiveExpression(Convert.ToInt32(action.ValueRight) > 0);                    
                }         
                return new CodePrimitiveExpression(Convert.ToInt32(((CodePrimitiveExpression)expr).Value));
            }            

            if ((int)conceptControl.ConceptTypeCode == 4 || (int)conceptControl.ConceptTypeCode == 2)
            {
                if (action.ValueRight != null)
                {
                    return new CodePrimitiveExpression(Convert.ToInt32(action.ValueRight));
                }
                else
                {

                    return new CodePrimitiveExpression(action.ValueRight);
                }
            }
            
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public CodeStatement GetInvokeActionStatement(Models.Action action)
        {
            if (action.Message != null)
            {
                return new CodeThrowExceptionStatement(
                    new CodeObjectCreateExpression(typeof(BusinessException),
                    new CodePrimitiveExpression(action.Message)));
            }


            //if (key == (int)InvokeFuntionCollection.InvokeMessage.Key)
            //{
            //    return new CodeThrowExceptionStatement(
            //        new CodeObjectCreateExpression(typeof(BusinessException),
            //        new CodePrimitiveExpression(action.Message)));
            //}

            if (action.DescriptionRuleSet != null)
            {
                CodeMethodInvokeExpression mi = new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(), "FireRules");

                mi.Parameters.Add(new CodePrimitiveExpression(action.RuleSetId));
                return new CodeExpressionStatement(mi);
            }
            if (action.IdFuction != null)
            {
                CodeMethodInvokeExpression mi = new CodeMethodInvokeExpression(
                        new CodeThisReferenceExpression(), "ExecuteFunction");

                mi.Parameters.Add(new CodePrimitiveExpression((action.IdFuction)));
                return new CodeExpressionStatement(mi);
            }


            return null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="expr"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private CodeExpression GetDynamicConceptAssignExpression(Models.Action action, CodeExpression value)
        {

            Models.Concept concept = action.ConceptLeft;
            SCREN.Concept conceptEnt = ConceptDAO.GetConceptByConceptIdEntityId(concept.ConceptId, concept.EntityId);
            PAEN.Entity entity = EntityDAO.FindEntity(concept.EntityId);

            CodeParameterDeclarationExpression paramExp = RuleHelper.GetParameterExpression(entity, _paremeters);
            CodeArgumentReferenceExpression arg = new CodeArgumentReferenceExpression(paramExp.Name);

            return new CodeMethodInvokeExpression(
                new CodeCastExpression(typeof(IDynamicConceptContainer), arg),
                "SetDynamicConcept",
                new CodeExpression[] { new CodePrimitiveExpression(concept.ConceptId),
                    new CodeCastExpression(RuleHelper.GetConceptTypeReference(conceptEnt),
                    value) });
        }

    }
}
