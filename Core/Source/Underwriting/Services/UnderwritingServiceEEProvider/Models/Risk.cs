using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using System.Collections;
using System.Collections.Generic;
using Rules = Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels
{
    public class Risk
    {
        /// <summary>
        /// Modelo Riesgo
        /// </summary>
        Models.Risk RiskModel;

        /// <summary>
        /// Modelo Póliza
        /// </summary>
        Models.Policy PolicyModel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="risk">Riesgo</param>
        public Risk(Models.Risk risk)
        {
            RiskModel = risk;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="risk">Riesgo</param>
        public Risk(Models.Policy policy, Models.Risk risk)
        {
            RiskModel = risk;
            PolicyModel = policy;
        }
        
        /// <summary>
        /// Tarifar
        /// </summary>
        public void Quotate()
        {
            RiskModel.Premium = 0;
            RiskModel.AmountInsured = 0;

            foreach (Models.Coverage coverage in RiskModel.Coverages)
            {
                Coverage businessCoverage = new Coverage(coverage,0,0);

                businessCoverage.Quotate(2);
                RiskModel.Premium += coverage.PremiumAmount;
                RiskModel.AmountInsured += coverage.LimitAmount;
            }
        }

        /// <summary>
        /// Ejecutar Reglas Póliza
        /// </summary>
        /// <returns>Póliza</returns>
        public Models.Risk RunRulesRisk(int ruleSetId)
        {
            Rules.Facade facade = new Rules.Facade();

            if (facade == null)
            {
                Assemblers.EntityAssembler.CreateFacadeGeneral(facade, PolicyModel);
                Assemblers.EntityAssembler.CreateFacadeRisk(facade, RiskModel);
            }
            else
            {
                throw new BusinessException("Error Load Facade");
            }

            facade = RulesEngineDelegate.ExecuteRules(ruleSetId, facade);
            Assemblers.ModelAssembler.CreateRisk(RiskModel, facade);
            return RiskModel;
        }

        
    }
}
