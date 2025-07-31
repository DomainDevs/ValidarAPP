using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class BasicConceptDAO
    {
        /// <summary>
        /// Obtiene el BasicConcept segun el EntotyId y ConceptId
        /// </summary>
        /// <param name="entityId">Identificador de Entity</param>
        /// <param name="conceptId">Identificador de Concept</param>
        /// <returns></returns>
        public static SCREN.BasicConcept GetBasicConceptByEntityIdConceptId(int conceptId, int entityId)
        {
            try
            {
                SCREN.BasicConcept basicConcept = null;
                PrimaryKey key = SCREN.BasicConcept.CreatePrimaryKey(conceptId, entityId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    basicConcept = (SCREN.BasicConcept)daf.GetObjectByPrimaryKey(key);
                }
                return basicConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetBasicConceptByEntityIdConceptId", ex);
            }

        }

        /// <summary>
        /// crea un BasicConcept
        /// </summary>
        /// <param name="basicConcept"></param>
        /// <returns></returns>
        public static BasicConcept Create(BasicConcept basicConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(basicConcept);
                return basicConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Create", ex);
            }
            
        }

        /// <summary>
        /// actualiza un BasicConcept
        /// </summary>
        /// <param name="basicConcept"></param>
        /// <returns></returns>
        public static BasicConcept Update(BasicConcept basicConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(basicConcept);
                return basicConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Update", ex);
            }
            
        }

        /// <summary>
        /// elimina un BasicConcept
        /// </summary>
        /// <param name="basicConcept"></param>
        /// <returns></returns>
        public static void Delete(BasicConcept basicConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(basicConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener Delete", ex);
            }
            
        }

        /// <summary>
        /// obtiene una lista de BasicConcept a partir del filtro
        /// </summary>
        /// <param name="basicConcept"></param>
        /// <returns></returns>
        public static BusinessCollection List(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(BasicConcept),
                filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener List", ex);
            }
            
        }
    }
}
