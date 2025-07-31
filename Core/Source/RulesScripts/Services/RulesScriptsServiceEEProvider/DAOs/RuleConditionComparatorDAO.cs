using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RuleConditionComparatorDAO
    {
        /// <summary>
        /// obtiene una lista RuleConditionComparator a partir del filtro 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection GetComparators(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance
                  .GetDataFacade().SelectObjects(typeof(RuleConditionComparator), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetComparators", ex);
            }
           
        }
    }
}
