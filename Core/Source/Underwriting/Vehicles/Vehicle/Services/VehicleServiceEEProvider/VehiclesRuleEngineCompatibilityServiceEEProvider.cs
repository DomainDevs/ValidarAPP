using System;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.Vehicles.VehicleServices.EEProvider
{
    public class VehiclesRuleEngineCompatibilityServiceEEProvider
    {

        public void ValidateSpecialPlate(Rules.Facade facade)
        {
            const int parameterId = 10026;

            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

            if (parameter != null && parameter.NumberParameter.HasValue)
            {
                string specialPlate = facade.GetConcept<string>(RuleConceptRisk.DynamicConcept(parameter.NumberParameter.Value));
                string licencePlate = facade.GetConcept<string>(RuleConceptRisk.LicensePlate);

                if (!string.IsNullOrEmpty(specialPlate))
                {
                    Regex rgx = new Regex(specialPlate.ToString());
                    facade.SetConcept(RuleConceptPolicies.GenerateEvent, rgx.IsMatch(licencePlate));
                }
            }

        }

    }
}
