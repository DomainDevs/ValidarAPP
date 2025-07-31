using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.Rules;
using Sistran.Core.Framework.Rules.Engine;
using Sistran.Core.Framework.Rules.Integration;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RulesEngineDelegate
    {
        private static object asynkLock = new object();
        private static IRuleEngine instance;
        private static volatile RulesBridge rulesBridge;

        public static IRuleEngine Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (asynkLock)
                    {
                        if (instance == null)
                        {
                            rulesBridge = new RuleBridgeDelegate();
                            instance = new RuleEngine(rulesBridge, rulesBridge, new RuleFunctionDelegate(), new RuleMessageDelegate());
                        }
                    }
                }

                return instance;
            }
        }

        private RulesEngineDelegate()
        {

        }

        public static Facade ExecuteRules(int ruleId, Facade facade)
        {
            return Instance.ExecuteRules(ruleId, InProcCache.GetCurrentVersion(), facade);
        }

        public static void CompileRuleSet(int ruleId, int versionCache)
        {
            Instance.CompileRuleSet(ruleId, versionCache);
        }

        public static void RemoveRuleSets()
        {
            //Instance.RemoveRuleSets();
        }

        public static void CreateInstance()
        {
            lock (asynkLock)
            {
                rulesBridge = new RuleBridgeDelegate();
                instance = new RuleEngine(rulesBridge, rulesBridge, new RuleFunctionDelegate(), new RuleMessageDelegate());
            }
        }
    }
}