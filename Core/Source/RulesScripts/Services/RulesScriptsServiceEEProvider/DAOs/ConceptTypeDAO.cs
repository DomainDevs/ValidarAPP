using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class ConceptTypeDAO
    {
        /// <summary>
        /// obtiene una lista de ConceptType a partiel del filtro 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static List<ConceptType> GetConceptType(Predicate filter, string[] sort)
        {
            try
            {
                return DataFacadeManager.Instance.GetDataFacade().List(typeof(ConceptType), filter, sort).Cast<ConceptType>().ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptType", ex);
            }
        }

        /// <summary>
        /// obtiene una lista de ConceptType a partiel del filtro 
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static List<BasicType> GetBasicType(Predicate filter, string[] sort)
        {
            try
            {
                return DataFacadeManager.Instance.GetDataFacade().List(typeof(BasicType), filter, sort).Cast<BasicType>().ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("error en GetConceptType", ex);
            }
        }
    }
}
