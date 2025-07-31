using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.DAF;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using System.Collections;
using System.Linq;
using RuleSerEnum = Sistran.Core.Application.RulesScriptsServices.Enums;
using SCREN = Sistran.Core.Application.Script.Entities;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class ActionDAO 
    {
        /// <summary>
        /// Arma la descripcion de las acciones
        /// </summary>
        /// 
        /// <param name="ruleSetId">Id Paquete de Reglas</param>
        /// <param name="ruleId">Id de la Regla</param>
        /// <returns>El modelo de la accion con la descripcion y el id</returns>        
        public static List<Models.Action> GetActions(int ruleSetId, int ruleId)
        {
            try
            {
                Type resourceManager = typeof(RulesServiceEEProvider);//new ResourceManager("Sistran.Core.Application.RulesScriptsServices.EEProvider.Errors", Assembly.GetExecutingAssembly());

                List<Models.Action> actions = new List<Models.Action>();
                if (ruleId > -1)
                {
                    RuleEditorData ruleEditorData = RuleSetDAO.FillRuleEditorData(ruleSetId);
                    RuleDTO ruleDTO = (RuleDTO)ruleEditorData.Rules[ruleId];

                    IDictionary functions = null;
                    int i = 0;
                    foreach (ActionDTO item in ruleDTO.Actions)
                    {
                        if (item is AssignActionDTOBase)
                        {
                            string operation;
                            SCREN.Concept conceptObj;

                            System.Text.StringBuilder str = new System.Text.StringBuilder();

                            AssignActionDTOBase action = (AssignActionDTOBase)item;

                            if (item is AssignActionDTO)
                            {
                                str.Append(resourceManager.GetField("LBL_ASSIGNACTION").GetValue(null).ToString().ToLower());

                                AssignActionDTO assAction = (AssignActionDTO)action;

                                PrimaryKey key = assAction.Concept.ConceptKey;
                                conceptObj = ConceptDAO.GetConceptByConceptIdEntityId((int)key["ConceptId"], (int)key["EntityId"]);
                                string concept = conceptObj.Description;

                                str.Append(" ");
                                str.Append(concept);
                                str.Append(" ");
                            }
                            else if (item is TemporaryAssignActionDTO)
                            {
                                str.Append(resourceManager.GetField("LBL_TEMPORARYASSIGNACTION").GetValue(null).ToString().ToLower());
                                str.Append(" \"");
                                str.Append(((TemporaryAssignActionDTO)item).TemporaryName);
                                str.Append("\" ");
                            }
                            else
                            {
                                throw new Exception("Assign action not supported: " + item.GetType().FullName);
                            }

                            operation = action.ArithmeticOperator.Description;

                            str.Append(resourceManager.GetField(operation).GetValue(null));
                            str.Append(" ");
                            str.Append(resourceManager.GetField(action.ValueType.Value.ToString()).GetValue(null));
                            str.Append(" ");

                            CodeExpression expr = action.Expression;

                            if (expr is CodeCastExpression)
                            {
                                expr = ((CodeCastExpression)expr).Expression;
                            }

                            if (expr is CodePrimitiveExpression)
                            {
                                str.Append((expr as CodePrimitiveExpression).Value.ToString());
                            }
                            else if (expr is CodeConceptExpression)
                            {
                                CodeConceptExpression value = (CodeConceptExpression)expr;

                                PrimaryKey key = value.ConceptKey;
                                SCREN.Concept expConcept = ConceptDAO.GetConceptByConceptIdEntityId((int)key["ConceptId"], (int)key["EntityId"]);
                                string expString = expConcept.Description;
                                str.Append(expString);
                            }
                            else if (expr is CodeListValueExpression)
                            {
                                str.Append(((CodeListValueExpression)expr).Value);
                            }
                            else if (expr is CodeBinaryOperatorExpression)
                            {
                                ExpressionHelper.GetAdvancedValueString(str, CultureInfo.CurrentUICulture, expr);
                            }
                            else if (expr is CodeIndexerExpression)
                            {
                                str.Append("\"");
                                str.Append((string)((CodePrimitiveExpression)((CodeIndexerExpression)expr).Indices[0]).Value);
                                str.Append("\"");
                            }
                            else
                            {
                                throw new ApplicationException("Tipo de expressión no reconocido: " + expr.GetType().FullName);
                            }

                            //line--;
                            actions.Add(new Models.Action { Id = i, Expression = str.ToString() });
                            i++;
                        }
                        else
                        {
                            string function;
                            string message;

                            InvokeActionDTO action = (InvokeActionDTO)item;

                            function = resourceManager.GetField(action.Function.Value.ToString()).GetValue(null).ToString();
                            message = action.Message;

                            System.Text.StringBuilder str = new System.Text.StringBuilder();
                            str.Append(resourceManager.GetField(ActionTypeCollection.InvokeAction.Description).GetValue(null));
                            str.Append(" ");
                            str.Append(function);
                            str.Append(" \"");

                            if ((int)action.Function.Key == (int)InvokeFuntionCollection.InvokeRuleSet.Key)
                            {
                                str.Append(ExpressionHelper.GetRuleSetDescriptionById(Convert.ToInt32(message)));
                            }
                            else if ((int)action.Function.Key == (int)InvokeFuntionCollection.InvokeFunction.Key)
                            {
                                if (functions == null)
                                {
                                    functions = RuleFunctionDAO.DictionaryRuleFunction(null, null);
                                }

                                str.Append(functions[message]);
                            }
                            else
                            {
                                str.Append(message);
                            }

                            str.Append("\"");

                            int value;
                            if (Int32.TryParse(message, out value))
                            {
                                actions.Add(new Models.Action { Id = i, Expression = str.ToString(), RuleSetId = Convert.ToInt32(message) });
                                i++;
                            }
                            else
                            {
                                actions.Add(new Models.Action { Id = i, Expression = str.ToString() });
                                i++;
                            }
                        }
                    }
                }

                return actions;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetActions", ex);
            }

            
        }

        /// <summary>
        /// Llena el objeto accion con la accion de la regla
        /// </summary>
        /// <param name="actionDTO">Modelo accion del xml de la regla</param>
        /// <returns> Modelo de la accion contodos los datos de la accion  </returns>
        public static Models.Action FillAction(ActionDTO actionDTO)
        {
            try
            {
                Type resourceManager = typeof (RulesServiceEEProvider);//new ResourceManager("Sistran.Core.Application.RulesScriptsServices.EEProvider.Errors", Assembly.GetExecutingAssembly());
                List<Models.Operator> operators = new List<Models.Operator>();
                List<Models.RuleFunction> ruleFunctions = new List<Models.RuleFunction>();

                Models.Action accion = new Models.Action();

                if (actionDTO is AssignActionDTOBase)
                {
                    string operation;

                    if (operators == null)
                    {
                        operators = RuleSetDAO.GetOperationTypes(14, 83); //traigo todos los operadores
                    }

                    SCREN.Concept conceptObj;

                    System.Text.StringBuilder str = new System.Text.StringBuilder();

                    AssignActionDTOBase assActionBase = (AssignActionDTOBase)actionDTO;

                    if (actionDTO is AssignActionDTO)
                    {
                        //accion = new AssignConceptAction();
                        accion.AssignType = RuleSerEnum.AssignType.ConceptAssign;//jonathan
                        str.Append(resourceManager.GetField("LBL_ASSIGNACTION").GetValue(null).ToString().ToLower());

                        AssignActionDTO assAction = (AssignActionDTO)assActionBase;

                        PrimaryKey key = assAction.Concept.ConceptKey;
                        conceptObj = ConceptDAO.GetConceptByConceptIdEntityId((int)key["ConceptId"], (int)key["EntityId"]);
                        accion.ConceptLeft = ModelAssembler.CreateConcept(conceptObj);

                        string concept = conceptObj.Description;


                        str.Append(" ");
                        str.Append(concept);
                        str.Append(" ");
                    }
                    else if (actionDTO is TemporaryAssignActionDTO)
                    {
                        //accion = new AssignTemporalAction();
                        accion.AssignType = RuleSerEnum.AssignType.TemporalAssign;//jonathan
                        accion.TemporalNameLeft = ((TemporaryAssignActionDTO)actionDTO).TemporaryName;

                        str.Append(resourceManager.GetField("LBL_TEMPORARYASSIGNACTION").GetValue(null).ToString().ToLower());
                        str.Append(" \"");
                        str.Append(((TemporaryAssignActionDTO)actionDTO).TemporaryName);
                        str.Append("\" ");
                    }
                    else
                    {
                        throw new Exception("Assign action not supported: " + actionDTO.GetType().FullName);
                    }

                    Models.Operator operatorM = new Models.Operator();

                    operatorM.Description = assActionBase.ArithmeticOperator.Description;
                    operatorM.OperatorCode = assActionBase.ArithmeticOperator.Code;
                    accion.Operator = operatorM;

                    operation = assActionBase.ArithmeticOperator.Description;

                    str.Append(resourceManager.GetField(operation).GetValue(null));
                    str.Append(" ");
                    str.Append(resourceManager.GetField(assActionBase.ValueType.Value.ToString()).GetValue(null));
                    str.Append(" ");

                    CodeExpression expr = assActionBase.Expression;

                    if (expr is CodeCastExpression)
                    {
                        expr = ((CodeCastExpression)expr).Expression;
                    }

                    if (expr is CodePrimitiveExpression)
                    {
                        accion.ValueRight = (expr as CodePrimitiveExpression).Value.ToString();
                        str.Append((expr as CodePrimitiveExpression).Value.ToString());
                    }
                    else if (expr is CodeConceptExpression)
                    {
                        CodeConceptExpression value = (CodeConceptExpression)expr;

                        PrimaryKey pk = value.ConceptKey;

                        SCREN.Concept expConcept = ConceptDAO.GetConceptByConceptIdEntityId((int)pk["ConceptId"], (int)pk["EntityId"]);

                        accion.ConceptRight = ModelAssembler.CreateConcept(expConcept);

                        string expString = expConcept.Description;
                        str.Append(expString);

                        //pregutnarle a gaston
                        //((AssignAction)accion).ConceptRight = ModelAssembler.CreateConcept(expConcept);
                        //string expString = expConcept.Description;
                        //str.Append(expString);
                    }
                    else if (expr is CodeListValueExpression)
                    {
                        accion.ValueRight = ((CodeListValueExpression)expr).Key.ToString();
                        str.Append(((CodeListValueExpression)expr).Value);
                    }
                    else if (expr is CodeBinaryOperatorExpression)
                    {
                        ExpressionHelper.GetAdvancedValueString(str, CultureInfo.CurrentUICulture, expr);
                    }
                    else if (expr is CodeIndexerExpression)
                    {
                        accion.TemporalNameRight = ((string)((CodePrimitiveExpression)((CodeIndexerExpression)expr).Indices[0]).Value);

                        str.Append("\"");
                        str.Append((string)((CodePrimitiveExpression)((CodeIndexerExpression)expr).Indices[0]).Value);
                        str.Append("\"");
                    }
                    else
                    {
                        throw new BusinessException("Tipo de expresión no reconocido: " + expr.GetType().FullName);
                    }

                    accion.Expression = str.ToString();
                }
                else
                {
                    string function;
                    string message;
                    accion.AssignType = RuleSerEnum.AssignType.InvokeAssign;//jonathan
                    InvokeActionDTO action = (InvokeActionDTO)actionDTO;

                    function = resourceManager.GetField(action.Function.Value.ToString()).GetValue(null).ToString();
                    message = action.Message;

                    System.Text.StringBuilder str = new System.Text.StringBuilder();
                    str.Append(resourceManager.GetField(ActionTypeCollection.InvokeAction.Description).GetValue(null));
                    str.Append(" ");
                    str.Append(function);
                    str.Append(" \"");

                    if ((int)action.Function.Key == (int)InvokeFuntionCollection.InvokeRuleSet.Key)
                    {
                        Entities.RuleSet ruleSet = RuleSetDAO.FindRuleSet(int.Parse(action.Message));
                        accion.RuleSetId = int.Parse(action.Message);
                        accion.DescriptionRuleSet = ruleSet.Description;

                        str.Append(ExpressionHelper.GetRuleSetDescriptionById(Convert.ToInt32(message)));
                    }
                    else if ((int)action.Function.Key == (int)InvokeFuntionCollection.InvokeFunction.Key)
                    {
                        IList ruleFunctionList = RuleFunctionDAO.ListRuleFunction(null, null);
                        ruleFunctions = RuleFunctionDAO.ConvertToRuleFunctionsModel(ruleFunctionList);

                        accion.IdFuction = action.Message;
                        str.Append(ruleFunctions.FirstOrDefault(i => i.FunctionName == action.Message).Description);
                    }
                    else
                    {

                        accion.Message = message;
                        str.Append(message);
                    }

                    str.Append("\"");

                    accion.Expression = str.ToString();

                }

                return accion;

            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FillAction", ex);
            }
        }
    }
}


