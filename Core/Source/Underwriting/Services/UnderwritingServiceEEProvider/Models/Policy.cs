using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels
{
    public class Policy
    {
        /// <summary>
        /// Modelo Póliza
        /// </summary>
        Models.Policy PolicyModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="policy">Póliza</param>
        public Policy(Models.Policy policy)
        {
            PolicyModel = policy;
        }

        /// <summary>
        /// Ejecutar Reglas Póliza
        /// </summary>
        /// <returns>Póliza</returns>
        public Models.Policy RunRulesPolicy(int ruleSetId)
        {
            RunRules(ruleSetId);
            return PolicyModel;
        }

        /// <summary>
        /// Ejecutar Reglas
        /// </summary>
        /// <param name="ruleSetId">Id Regla</param>
        private void RunRules(int ruleSetId)
        {
            Rules.Facade facade = new Rules.Facade();

            EntityAssembler.CreateFacadeGeneral(facade, PolicyModel);

            facade = RulesEngineDelegate.ExecuteRules(ruleSetId, facade);

            ModelAssembler.CreatePolicy(PolicyModel, facade);
        }
    }
}
