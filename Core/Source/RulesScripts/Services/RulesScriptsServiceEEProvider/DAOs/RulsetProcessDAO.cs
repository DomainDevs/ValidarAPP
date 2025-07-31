using System.Linq;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class RulsetProcessDAO
    {

        public Models.RuleProcessRuleSet GetRulestByRulsetProcessType(int processType)
        {
           Models.RuleProcessRuleSet ruleProcess = new Models.RuleProcessRuleSet();
            

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(RuleProcessRuleset.Properties.RuleProcessId, processType);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RuleProcessRuleset), filter.GetPredicate()));
            if (businessCollection != null && businessCollection.Any())
            {
                ruleProcess = ModelAssembler.CreateRuleProcessRuleSet(businessCollection.Cast<RuleProcessRuleset>().First());
            }
           
            return ruleProcess;
        }
    }
}
