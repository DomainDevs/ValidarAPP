using Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs;
using Sistran.Core.Application.RulesScriptsServices.Models;
using System;
using System.CodeDom;
using System.Collections;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class ConditionDefinitionBuilder
    {
        const int BOOLEAN_LIST_CODE = 2;
        private IDictionary _paremeters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paremeters"></param>
        public ConditionDefinitionBuilder(IDictionary paremeters)
        {
            _paremeters = paremeters;
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
        /// <param name="condition"></param>
        /// <returns></returns>
        public CodeExpression GetConditionExpression(Condition condition)
        {
            CodeBinaryOperatorExpression expr = new CodeBinaryOperatorExpression();

            // en el lado izquierdo de la condición siempre es un concepto
            expr.Left = RuleHelper.GetConceptExpression(condition.Concept, _paremeters, false);

            if (condition.Comparator != null)
            {
                expr.Operator = (System.CodeDom.CodeBinaryOperatorType)condition.Comparator.ComparatorCode;    
            }            
            
            if (condition.ConceptValue != null)
            {
                expr.Right = RuleHelper.GetConceptExpression(condition.ConceptValue, _paremeters, false);
            }
            else
            {
                expr.Right = ProcessExpression(condition);
            }
            return expr;
        }

        /// <summary>
        /// Lado derecho de la condición para valores primarios
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        private CodeExpression ProcessExpression(Condition condition)
        {
            //SCREN.Concept conceptEntity = ConceptDAO.GetConceptByConceptIdEntityId(condition.Concept.ConceptId, condition.Concept.EntityId);
            Models.ConceptControl conceptControl = RuleSetDAO.GetConceptControl(condition.Concept.ConceptId, condition.Concept.EntityId);

            CodeExpression expr = new CodePrimitiveExpression(condition.Value);

            // es un tipo basico
            if (condition.ValueType == Sistran.Core.Application.RulesScriptsServices.Enums.ValueType.Value && (int)conceptControl.ConceptControlCode < 4)
            {
                //CodeExpression expr = new CodePrimitiveExpression(condition.Value);
                if ((int)conceptControl.BasicType == 1)
                {
                    return new CodePrimitiveExpression(Convert.ToInt32(condition.Value));
                }
                if ((int)conceptControl.BasicType == 3)
                {
                    return new CodePrimitiveExpression(Convert.ToDecimal(condition.Value));
                }
                if ((int)conceptControl.BasicType == 4)
                {
                    string date = (DateTime.Parse(condition.Value.ToString())).ToString("dd/MM/yyyy");
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
                SCREN.ListConcept lc = ListConceptDAO.FindListConcept(condition.Concept.ConceptId, condition.Concept.EntityId);
                if (lc == null)
                {
                    throw new Exception("No se encontro la lista relacionada al concepto.");
                }

                if (lc.ListEntityCode == BOOLEAN_LIST_CODE && condition.Value.ToString() != "")
                {
                    return new CodePrimitiveExpression(Convert.ToInt32(condition.Value) > 0);
                    //return new CodePrimitiveExpression(((int)((CodeListValueExpression)expr).Key) > 0);//mirar jonathan
                }
                
                //return new CodePrimitiveExpression(((CodeListValueExpression)expr).Key);
                return new CodePrimitiveExpression(((CodePrimitiveExpression)expr).Value);
            }

            //if (conceptEntity is ReferenceConcept || conceptEntity is RangeConcept)
            if ((int)conceptControl.ConceptTypeCode == 4 || (int)conceptControl.ConceptTypeCode == 2)
            {
                if (condition.Value != null)
                {
                    return new CodePrimitiveExpression(Convert.ToInt32(condition.Value));
                }
                else
                {

                    return new CodePrimitiveExpression(condition.Value);
                }
            }

            return null;

        }

    }
}
