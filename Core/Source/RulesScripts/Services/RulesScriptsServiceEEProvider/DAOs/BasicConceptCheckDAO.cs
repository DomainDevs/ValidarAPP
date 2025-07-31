using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class BasicConceptCheckDAO
    {
        /// <summary>
        /// crea un BasicConceptCheck
        /// </summary>
        /// <param name="basicConceptCheck"></param>
        /// <returns></returns>
        public static BasicConceptCheck Create(BasicConceptCheck basicConceptCheck)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(basicConceptCheck);
                return basicConceptCheck;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Create", ex);
            }
            
        }

        /// <summary>
        /// actualiza un BasicConceptCheck
        /// </summary>
        /// <param name="basicConceptCheck"></param>
        /// <returns></returns>
        public static BasicConceptCheck Update(BasicConceptCheck basicConceptCheck)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(basicConceptCheck);
                return basicConceptCheck;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Update", ex);
            }
            
        }

        /// <summary>
        /// elimina un BasicConceptCheck
        /// </summary>
        /// <param name="basicConceptCheck"></param>
        /// <returns></returns>
        public static void Delete(BasicConceptCheck basicConceptCheck)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(basicConceptCheck);
                DataFacadeManager.Dispose();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Delete", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de BasicConceptCheck a partir de un filtro
        /// </summary>
        /// <param name="basicConceptCheck"></param>
        /// <returns></returns>
        public static BusinessCollection List(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(BasicConceptCheck), filter, sort));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener List", ex);
            }

        }

        /// <summary>
        /// obtiene un  BasicConceptCheck a partir de conceptId, entityId, basicCheckCode
        /// </summary>
        /// <param name="basicConceptCheck"></param>
        /// <returns></returns>
        public static BasicConceptCheck Find(int conceptId, int entityId, int basicCheckCode)
        {
            try
            {
                BasicConceptCheck basicConceptCheck = null;
                PrimaryKey key = BasicConceptCheck.CreatePrimaryKey(entityId, basicCheckCode, conceptId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    basicConceptCheck = (BasicConceptCheck)daf.GetObjectByPrimaryKey(key);
                }
                
                return basicConceptCheck;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Find", ex);
            }

        }
    }
}
