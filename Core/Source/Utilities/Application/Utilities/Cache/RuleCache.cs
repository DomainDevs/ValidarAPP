using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TM = Sistran.Core.Application.Utilities.Utility;
namespace Sistran.Core.Application.Utilities.Cache
{
    public class RuleCache
    {
        public static void LoadRuleCache()
        {

            //Cache Reglas
            List<Task> tasks = new List<Task>();
            tasks.Add(TM.Task.Run(() => RuleEngineDAO.GetConcepts()));
            tasks.Add(TM.Task.Run(() => RuleEngineDAO.GetEntities()));
            tasks.Add(TM.Task.Run(() => RuleEngineDAO.GetPositionEntitiesInitial()));
            tasks.Add(TM.Task.Run(() => RuleEngineDAO.GetBasicConcep()));
            tasks.Add(TM.Task.Run(() => RuleEngineDAO.GetListConcept()));
            //tasks.Add(Task.Run(() => RuleEngineDAO.GetRulseSets()));
            tasks.Add(TM.Task.Run(() => RuleEngineDAO.GetConceptDependencies()));
            try
            {
            Task.WaitAll(tasks.ToArray());
            }
            catch (System.AggregateException ae)
            {

                EventLog.WriteEntry("Application", ae.Message);
            }
            
        }
    }
}
