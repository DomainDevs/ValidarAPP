using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RuleConditionConceptDAO
    {
        /// <summary>
        /// crea un RuleConditionConcept
        /// </summary>
        /// <param name="RuleConditionConcept"></param>
        /// <returns></returns>
        public static RuleConditionConcept CreateRuleConditionConcept(RuleConditionConcept RuleConditionConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(RuleConditionConcept);
                return RuleConditionConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleConditionConcept", ex);
            }
           
        }

        /// <summary>
        /// edita un RuleConditionConcept
        /// </summary>
        /// <param name="RuleConditionConcept"></param>
        /// <returns></returns>
        public static RuleConditionConcept UpdateRuleConditionConcept(RuleConditionConcept RuleConditionConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(RuleConditionConcept);
                return RuleConditionConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRuleConditionConcept", ex);
            }
            
        }

        /// <summary>
        /// elimina un RuleConditionConcept
        /// </summary>
        /// <param name="RuleConditionConcept"></param>
        public static void DeleteRuleConditionConcept(RuleConditionConcept RuleConditionConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(RuleConditionConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteRuleConditionConcept", ex);
            }
            
        }

        /// <summary>
        /// obtiene un BusinessCollection a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection GetRuleConditionConcept(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RuleConditionConcept), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleActionConcept", ex);
            }
        }

    }
}
