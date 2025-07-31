using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Rules;
using System;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider
{
    public class OperationQuotaRuleEngineCompatibilityServiceEEProvider
    {
        public void ValidateCauseOfDissolution(Facade facade)
        {
            decimal finalTotalHeritage = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaBusiness.FinalTotalHeritage);
            decimal finalShareCapital = facade.GetConcept<decimal>(RuleConceptAutomaticQuotaBusiness.FinalShareCapital);
            double multiplier = 0.5;
            double calculate = (Convert.ToDouble(finalShareCapital) * multiplier);

            if (Convert.ToDouble(finalTotalHeritage) < calculate)
            {
                facade.SetConcept(RuleConceptPolicies.GenerateEvent, true);
            }
        }
    } 
}
