using Sistran.Core.Framework.BAF;
using System;
using System.Collections;
using Sistran.Core.Application.CommonService.Models;
using Rules = Sistran.Core.Framework.Rules;
using Sistran.Core.Application.Utilities.RulesEngine;

namespace Sistran.Core.Application.Locations.EEProvider
{
    public class LocationsRuleEngineCompatibilityServiceEEProvider
    {
        /// <summary>
        /// Asignar valor del objeto del seguro
        /// </summary>
        /// <param name="parameters"> lista de Facade</param>
        /// <returns>Lista de facade</returns>
        public void SetInsuredObjectAmountToCoverage(Rules.Facade facade)
        {
            Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(6001);
            
            if (parameter.NumberParameter.GetValueOrDefault() > 0)
            {
                facade.SetConcept(RuleConceptCoverage.DynamicConcept(parameter.NumberParameter.Value), facade.GetConcept<decimal>(RuleConceptCoverage.InsuredObjectAmount));
            }
        }

        /// <summary>
        /// Obtener el valor variable del seguro.
        /// </summary>
        /// <param name="parameters">Lista de facade</param>
        /// <returns>La lista de los parámetros con los conceptos dinámicos asignados</returns>
        //public IList GetValuePercentageVariableInsured(IList parameters)
        //{
        //    int parameterId = 6032;
        //    int indexFacadeCoverage = 0;
        //    FacadeCoverage facadeCoverage = null;
        //    for (int i = 0; i < parameters.Count; i++)
        //    {
        //        if (parameters[i] is FacadeCoverage)
        //        {
        //            facadeCoverage = (FacadeCoverage)parameters[i];
        //            indexFacadeCoverage = i;
        //            break;
        //        }
        //    }
        //    Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

        //    if (facadeCoverage != null && parameter != null && parameter.NumberParameter.GetValueOrDefault() > 0)
        //    {
        //        //  (decimal)tempRiskInsuredObject.PercentageVariableIndex;
        //        facadeCoverage.SetDynamicConcept(parameter.NumberParameter.Value, null);
        //        parameters[0] = facadeCoverage;
        //    }
        //    return parameters;
        //}

        /// <summary>
        /// Obtener el valor acumulado del objeto del seguro.
        /// </summary>
        /// <param name="parameters">Lista de facade</param>
        /// <returns>La lista de los parámetros con los conceptos dinámicos asignados</returns>
        //public IList GetValueAcumInsuredObjectConcept(IList parameters)
        //{
        //    int parameterId = 10019;
        //    int indexFacadeRisk = 0;
        //    FacadeRisk facadeRisk = null;
        //    for (int i = 0; i < parameters.Count; i++)
        //    {
        //        if (parameters[i] is FacadeRisk)
        //        {
        //            facadeRisk = (FacadeRisk)parameters[i];
        //            indexFacadeRisk = i;
        //            break;
        //        }
        //    }
        //    Parameter parameter = DelegateService.commonServiceCore.GetParameterByParameterId(parameterId);

        //    if (facadeRisk != null && parameter != null && parameter.NumberParameter.GetValueOrDefault() > 0)
        //    {
        //        //  facadeRisk.SetDynamicConcept(concept.ConceptId, null)
        //        facadeRisk.SetDynamicConcept(parameter.NumberParameter.Value, facadeRisk.InsuredId);
        //        parameters[indexFacadeRisk] = facadeRisk;
        //    }
        //    return parameters;
        //}
    }
}