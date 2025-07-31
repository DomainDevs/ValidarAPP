using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class RuleDAO
    {
        /// <summary>
        /// crea una Entities.Rule
        /// </summary>
        /// <param name="Rule"></param>
        /// <returns></returns>
        public static Entities.Rule CreateRule(Entities.Rule Rule)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(Rule);
                return Rule;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRule", ex);
            }

        }

        /// <summary>
        /// crea una Entities.Rule
        /// </summary>
        /// <param name="Rule"></param>
        /// <returns></returns>
        public static void CreateRules(BusinessCollection<Entities.Rule> Rules)
        {
            try
            {
                if (Context.Current == null)
                {
                    Context context = new Context();
                }
                DataFacadeManager.Instance.GetDataFacade().InsertObjects(Rules);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRule", ex);
            }

        }

        /// <summary>
        /// actualiza una Entities.Rule
        /// </summary>
        /// <param name="Rule"></param>
        /// <returns></returns>
        public static Entities.Rule UpdateRule(Entities.Rule Rule)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(Rule);
                return Rule;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRule", ex);
            }

        }

        /// <summary>
        /// elimina una Entities.Rule
        /// </summary>
        /// <param name="Rule"></param>
        public static void DeleteRule(Entities.Rule Rule)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(Rule);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteLevel", ex);
            }

        }

        /// <summary>
        /// obtiene una Entities.Rule a partir de Entities.Rule
        /// </summary>
        /// <param name="Rule"></param>
        /// <returns></returns>
        public static Entities.Rule GetRule(Entities.Rule Rule)
        {
            try
            {
                PrimaryKey key = Entities.Rule.CreatePrimaryKey(Rule.RuleBaseId, Rule.RuleId);
                return (Entities.Rule)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteLevel", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de Entities.Rule a parrtir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListRule(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Entities.Rule), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListPackage", ex);
            }

        }
    }
}
