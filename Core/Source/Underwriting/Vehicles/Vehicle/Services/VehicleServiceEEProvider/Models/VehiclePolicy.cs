using System.Collections.Generic;
using UnderModels = Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities;
using System.Collections;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.Rules;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider.BusinessModels
{

    public class VehiclePolicy
    {
        Models.VehiclePolicy VehiclePolicyModel;
        public VehiclePolicy(Models.VehiclePolicy vehiclePolicy)
        {
            this.VehiclePolicyModel = vehiclePolicy;
        }

        private void RunRules(int ruleSetId)
        {
            //FacadeGeneral = new FacadeGeneral();

            //List<FacadeBasic> facts = new List<FacadeBasic>();
            //facts.Add(FacadeGeneral);
            //RulesEngineDelegate rulesEngineDelegate = new RulesEngineDelegate((IList)facts, ruleSetId);
            //FacadeGeneral = (FacadeGeneral)rulesEngineDelegate.OutFacade;
        }

        public Models.VehiclePolicy Quotate()
        {
            if (VehiclePolicyModel.Product.RuleSetId.GetValueOrDefault(0) > 0)
            {
                RunRules(VehiclePolicyModel.Product.RuleSetId.Value);
            }

            if (VehiclePolicyModel.Vehicles != null && VehiclePolicyModel.Vehicles.Count > 0)
            {
                BusinessModels.Vehicle vehicle;

                foreach (Models.Vehicle risk in VehiclePolicyModel.Vehicles)
                {
                    vehicle = new Vehicle(risk);
                    vehicle.Quotate();
                }
            }

            return VehiclePolicyModel;
        }

    }
}
