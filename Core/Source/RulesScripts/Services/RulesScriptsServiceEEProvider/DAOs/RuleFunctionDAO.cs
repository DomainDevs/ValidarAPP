using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using RuleModel = Sistran.Core.Application.RulesScriptsServices.Models;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RuleFunctionDAO
    {
        /// <summary>
        /// obtiene BusinessCollection a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListRuleFunction (Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RuleFunction),
                filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRuleFunction", ex);
            }
            
        }

        /// <summary>
        /// obtiene IDictionary a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IDictionary DictionaryRuleFunction(Predicate filter, string[] sort)
        {
            try
            {
                IList ruleFunctionList = ListRuleFunction(filter, sort);

                SortedList _ruleFunctionList = new SortedList();

                foreach (RuleFunction ruleFunction in ruleFunctionList)
                {
                    _ruleFunctionList.Add(ruleFunction.FunctionName, ruleFunction.Description);
                }

                return _ruleFunctionList;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DictionaryRuleFunction", ex);
            }
           
        }

        /// <summary>
        /// obtien RuleFunction a partir de ruleFunctionId
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>
        public static RuleFunction FindRuleFunction(int ruleFunctionId)
        {
            try
            {
                PrimaryKey key = RuleFunction.CreatePrimaryKey(ruleFunctionId);
                RuleFunction ruleFunction = (RuleFunction)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return ruleFunction;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindRuleFunction", ex);
            }
           
        }

        /// <summary>
        /// obtiene RuleModel.RuleFunction a aprtir de RuleFunction
        /// </summary>
        /// <param name="ruleFunction"></param>
        /// <returns></returns>
        public static RuleModel.RuleFunction ConvertToRuleFunctionModel(RuleFunction ruleFunction)
        {
            try
            {
                RuleModel.RuleFunction rf = new RuleModel.RuleFunction
                {
                    Id = ruleFunction.RuleFunctionId,
                    Description = ruleFunction.Description,
                    FunctionName = ruleFunction.FunctionName
                };

                return rf;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ConvertToRuleFunctionModel", ex);
            }
    
        }

        /// <summary>
        /// obtiene una lista de RuleModel.RuleFunction a partir de IList
        /// </summary>
        /// <param name="ruleFunctionList"></param>
        /// <returns></returns>
        public static List<RuleModel.RuleFunction> ConvertToRuleFunctionsModel(IList ruleFunctionList)
        {
            try
            {
                List<RuleModel.RuleFunction> ruleFunctions = new List<RuleModel.RuleFunction>();

                foreach (RuleFunction ruleFunction in ruleFunctionList)
                {
                    ruleFunctions.Add(ConvertToRuleFunctionModel(ruleFunction));
                }

                return ruleFunctions;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ConvertToRuleFunctionsModel", ex);
            }
            
        }
    }
}
