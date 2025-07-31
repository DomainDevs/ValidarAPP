using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RuleActionConceptDAO
    {
        /// <summary>
        /// crea un RuleActionConcept
        /// </summary>
        /// <param name="RuleActionConcept"></param>
        /// <returns></returns>
        public static RuleActionConcept CreateRuleActionConcept(RuleActionConcept RuleActionConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(RuleActionConcept);
                return null;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRuleActionConcept", ex);
            }
            
        }

        /// <summary>
        /// edita un RuleActionConcept
        /// </summary>
        /// <param name="RuleActionConcept"></param>
        /// <returns></returns>
        public static RuleActionConcept UpdateRuleActionConcept(RuleActionConcept RuleActionConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(RuleActionConcept);
                return RuleActionConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRuleActionConcept", ex);
            }
            
        }

        /// <summary>
        /// elimina un RuleActionConcept
        /// </summary>
        /// <param name="RuleActionConcept"></param>
        /// <returns></returns>
        public static void DeleteRuleActionConcept(RuleActionConcept RuleActionConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(RuleActionConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteRuleActionConcept", ex);
            }
           
        }

        /// <summary>
        /// obtiene una lista de  RuleActionConcept
        /// </summary>
        /// <param name="RuleActionConcept"></param>
        /// <returns></returns>
        public static BusinessCollection GetRuleActionConcept(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RuleActionConcept), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRuleActionConcept", ex);
            }
        }
    }
}
