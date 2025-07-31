using System.Collections.Generic;

using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs;
using Sistran.Core.Application.RulesScriptsServices.Enums;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider
{
    /// <summary>
    /// 
    /// </summary>
    public class RuleSetDefinitionBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="rules"></param>
        /// <returns></returns>
        public RuleBaseDef GetRuleSetBaseDefinition(string name, List<RuleComposite> rules, bool IsTable)
        {            
            RuleBaseDef ruleBase = new RuleBaseDef(name, IsTable? RuleBaseType.Option : RuleBaseType.Sequence);
            ruleBase.RuleSet = GetRuleCollection(rules);
            return ruleBase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleComposites"></param>
        /// <returns></returns>
        public RuleDefCollection GetRuleCollection(List<RuleComposite> ruleComposites)
        {
            RuleDefinitionBuilder ruleDefBuilder;
            RuleDefCollection ruleDefList = new RuleDefCollection();

            foreach (RuleComposite ruleComposite in ruleComposites)
            {
                ruleDefBuilder = new RuleDefinitionBuilder(ruleComposite);
                ruleDefList.Add(ruleDefBuilder.GetRuleDefinition());
            }

            return ruleDefList;
        }
    }
}

