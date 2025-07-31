using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities;
using UnderModels = Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using Sistran.Core.Framework.BAF;
using System.Collections;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.BusinessModels
{
    public class Coverage : Sistran.Core.Application.UnderwritingServices.EEProvider.BusinessModels.Coverage
    {
        public Coverage(UnderModels.Coverage coverage, int policyId, int riskId)
            : base(coverage, policyId, riskId)
        {

        }

        public void RunRules(int ruleSetId)
        {
            //if (FacadeGeneral == null)
            //{
            //    throw new BusinessException("FacadeGeneral no esta definido");
            //}
            //if (FacadeVehicle == null)
            //{
            //    throw new BusinessException("FacadeVehicle no esta definido");
            //}

            //if (FacadeCoverage == null)
            //{
            //    FacadeCoverage = new FacadeCoverage();
            //    FacadeCoverage.LimitClaimantAmount = 0;
            //}

            //List<FacadeBasic> facts = new List<FacadeBasic>();
            //facts.Add(FacadeCoverage);
            //facts.Add(FacadeVehicle);
            //facts.Add(FacadeGeneral);
            //RulesEngineDelegate rulesEngineDelegate = new RulesEngineDelegate((IList)facts, ruleSetId);
            //FacadeCoverage = (FacadeCoverage)rulesEngineDelegate.OutFacade;
            ////base.CoverageModel = FacadeCoverage.GetCoverage();
        }
    }
}
