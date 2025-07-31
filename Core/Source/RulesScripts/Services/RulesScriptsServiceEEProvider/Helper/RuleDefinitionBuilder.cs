using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using Sistran.Core.Application.RulesScriptsServices.Models;

using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.Enums;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// Summary description for RuleDefinitionBuilder.
    /// </summary>
    public class RuleDefinitionBuilder
    {
        private RuleComposite _ruleComposite;
        private IDictionary _paremeters = new HybridDictionary();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleComposite"></param>
        public RuleDefinitionBuilder(RuleComposite ruleComposite)
        {
            _ruleComposite = ruleComposite;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// modificado jonathan moreno
        public RuleDef GetRuleDefinition()
        {
            RuleDef ruleDef = new RuleDef(_ruleComposite.RuleName);
        
            // Condiciones
            ConditionDefinitionBuilder conDefBuilder = new ConditionDefinitionBuilder(_paremeters);

            if (_ruleComposite.Conditions != null)
            {
                foreach (Condition condition in _ruleComposite.Conditions)
                {
                    if (condition.Comparator != null)
                    {
                        ruleDef.Conditions.Add(conDefBuilder.GetConditionExpression(condition));
                    }                       
                }
                _paremeters = conDefBuilder.Parameters;    
            }
            
            // Acciones
            ActionDefinitionBuilder actionDefBuilder = new ActionDefinitionBuilder(_paremeters);
            
            if (_ruleComposite.Actions != null)
            {
                foreach (Models.Action action in _ruleComposite.Actions)
                {
                    if (action.AssignType == AssignType.InvokeAssign)
                    {                        
                        ruleDef.Consequence.Add(actionDefBuilder.GetInvokeActionStatement(action));
                    }
                    else if (
                        (action.AssignType == AssignType.ConceptAssign || action.AssignType == AssignType.TemporalAssign)
                        && (action.Operator != null)
                        )
                    {
                        ruleDef.Consequence.Add(actionDefBuilder.GetAssignActionStatement(action));
                    }
                    //throw new ApplicationException("Tipo de accion no definida.");
                }
                _paremeters = actionDefBuilder.Parameters;
            }
            
            // Parametros 
            foreach (DictionaryEntry de in _paremeters)
            {
                ruleDef.Parameters.Add(((CodeParameterDeclarationExpression)de.Value));
            }

            return ruleDef;
        }        
    
    }
}