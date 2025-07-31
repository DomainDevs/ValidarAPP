using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using RulesEntities = Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Framework.DAF;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class ConditionDAO
    {
        /// <summary>
        /// obtiene una lista de  Condition a partir de  ruleSetId, ruleId
        /// </summary>
        /// <param name="ruleSetId"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public static List<Condition> GetConditions(int ruleSetId, int ruleId)
        {
            try
            {

                Type resourceManager = typeof(RulesServiceEEProvider);//new ResourceManager("Sistran.Core.Application.RulesScriptsServices.EEProvider.Errors", Assembly.GetExecutingAssembly());

                List<Condition> conditions = new List<Condition>();

                if (ruleId > -1)
                {
                    string concept, comparision, expresion;

                    RulesEntities.RuleEditorData ruleEditorData = RuleSetDAO.FillRuleEditorData(ruleSetId);
                    RuleDTO ruleDTO = (RuleDTO)ruleEditorData.Rules[ruleId];

                    int i = 0;
                    foreach (ConditionDTO cdto in ruleDTO.Conditions)
                    {
                        PrimaryKey key = cdto.Concept.ConceptKey;
                        int entityId = (int)key["EntityId"];

                        SCREN.Concept conceptObj = ConceptDAO.GetConceptByConceptIdEntityId((int)key["ConceptId"], entityId);

                        concept = conceptObj.Description;
                        comparision = cdto.ComparisionOperator.Description;

                        bool isNullComparison = false;
                        if (cdto.Expression is CodePrimitiveExpression)
                        {
                            object value = (cdto.Expression as CodePrimitiveExpression).Value;
                            if (value == null)
                            {
                                isNullComparison = true;
                            }
                            else
                            {
                                expresion = value.ToString();
                            }
                        }
                        else
                        {
                            expresion = String.Empty;
                        }

                        System.Text.StringBuilder str = new System.Text.StringBuilder();
                        str.Append(concept);
                        str.Append(" ");
                        str.Append("es");
                        str.Append(" ");
                        if (isNullComparison)
                        {
                            if (cdto.ComparisionOperator.Code == ComparisonOperatorCollection.Equal.Code)
                            {
                                str.Append("NULO");
                            }
                            else
                            {
                                str.Append("NO NULO");
                            }
                        }
                        else
                        {
                            str.Append(resourceManager.GetField(comparision).GetValue(null));
                            str.Append(" ");
                            str.Append(resourceManager.GetField(cdto.ValueType.Value.ToString()).GetValue(null));
                            str.Append(" ");

                            CodeExpression expr = cdto.Expression;
                            if (expr is CodePrimitiveExpression)
                            {
                                str.Append(((CodePrimitiveExpression)expr).Value.ToString());
                            }
                            else if (expr is CodeConceptExpression)
                            {
                                CodeConceptExpression value = (CodeConceptExpression)expr;
                                PrimaryKey pk = value.ConceptKey;

                                SCREN.Concept expConcept = ConceptDAO.GetConceptByConceptIdEntityId((int)pk["ConceptId"], (int)pk["EntityId"]);

                                string expString = expConcept.Description;
                                str.Append(expString);
                            }
                            else if (expr is CodeListValueExpression)
                            {
                                str.Append(((CodeListValueExpression)expr).Value);
                            }
                            else if (expr is CodeBinaryOperatorExpression)
                            {
                                ExpressionHelper.GetAdvancedValueString(str, CultureInfo.CurrentUICulture,
                                                                        expr);
                            }
                        }
                        conditions.Add(new Condition { Id = i, Expression = str.ToString() });
                        i++;
                    }
                }

                return conditions;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConditions", ex);
            }
            
        }

        /// <summary>
        /// obtien un Condition a partir de un ConditionDTO
        /// </summary>
        /// <param name="conditionDTO"></param>
        /// <returns></returns>
        public static Condition FillCondition(ConditionDTO conditionDTO)
        {
            try
            {
                string conceptStr;
                string comparisionStr;
                string expresion;
                Condition condition = new Condition();
                Type resourceManager = typeof(RulesServiceEEProvider);//new ResourceManager("Sistran.Core.Application.RulesScriptsServices.EEProvider.Errors", Assembly.GetExecutingAssembly());

                PrimaryKey key = conditionDTO.Concept.ConceptKey;
                int entityId = (int)key["EntityId"];

                SCREN.Concept conceptObj = ConceptDAO.GetConceptByConceptIdEntityId((int)key["ConceptId"], entityId);

                condition.Concept = ModelAssembler.CreateConcept(conceptObj);
                condition.ConceptControl = RuleSetDAO.GetConceptControl((int)key["ConceptId"], entityId);

                conceptStr = conceptObj.Description;
                comparisionStr = conditionDTO.ComparisionOperator.Description;

                condition.Comparator = ModelAssembler.CreateComparator(conditionDTO.ComparisionOperator);

                bool isNullComparison = false;
                if (conditionDTO.Expression is CodePrimitiveExpression)
                {
                    object value = (conditionDTO.Expression as CodePrimitiveExpression).Value;
                    if (value == null)
                    {
                        isNullComparison = true;
                    }
                    else
                    {
                        expresion = value.ToString();
                    }
                }
                else
                {
                    expresion = String.Empty;
                }

                System.Text.StringBuilder str = new System.Text.StringBuilder();

                str.Append(conceptStr);
                str.Append(" ");
                str.Append("es");
                str.Append(" ");

                if (isNullComparison)
                {
                    if (conditionDTO.ComparisionOperator.Code == ComparisonOperatorCollection.Equal.Code)
                    {
                        str.Append("NULO");
                    }
                    else
                    {
                        str.Append("NO NULO");
                    }
                    //CodeExpression expr = conditionDTO.Expression;
                    //condition.Value = ((CodePrimitiveExpression)expr).Value.ToString();
                }
                else
                {
                    str.Append(resourceManager.GetField(comparisionStr).GetValue(null));
                    str.Append(" ");
                    str.Append(resourceManager.GetField(conditionDTO.ValueType.Value.ToString()).GetValue(null));
                    str.Append(" ");

                    CodeExpression expr = conditionDTO.Expression;
                    if (expr is CodePrimitiveExpression)
                    {
                        condition.Value = ((CodePrimitiveExpression)expr).Value.ToString();

                        str.Append(((CodePrimitiveExpression)expr).Value.ToString());
                    }
                    else if (expr is CodeConceptExpression)
                    {
                        CodeConceptExpression value = (CodeConceptExpression)expr;
                        PrimaryKey pk = value.ConceptKey;

                        SCREN.Concept expConcept = ConceptDAO.GetConceptByConceptIdEntityId((int)pk["ConceptId"], (int)pk["EntityId"]);

                        condition.ConceptValue = ModelAssembler.CreateConcept(expConcept);

                        string expString = expConcept.Description;
                        str.Append(expString);
                    }
                    else if (expr is CodeListValueExpression)
                    {
                        condition.Value = ((CodeListValueExpression)expr).Key.ToString();//CodePrimitiveExpression
                        str.Append(((CodeListValueExpression)expr).Value);
                    }
                }

                condition.Expression = str.ToString();

                return condition;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FillCondition", ex);
            }            
        }
    }
}
