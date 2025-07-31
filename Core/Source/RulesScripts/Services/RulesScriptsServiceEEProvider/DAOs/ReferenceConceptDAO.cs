using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class ReferenceConceptDAO
    {
        /// <summary>
        /// crea un ReferenceConcept
        /// </summary>
        /// <param name="referenceConcept"></param>
        /// <returns></returns>
        public static ReferenceConcept CreateReferenceConcept(ReferenceConcept referenceConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(referenceConcept);
                return referenceConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateReferenceConcept", ex);
            }

        }

        /// <summary>
        /// actualiza un ReferenceConcept
        /// </summary>
        /// <param name="referenceConcept"></param>
        /// <returns></returns>
        public static ReferenceConcept UpdateReferenceConcept(ReferenceConcept referenceConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(referenceConcept);
                return referenceConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateReferenceConcept", ex);
            }

        }

        /// <summary>
        /// elimina un ReferenceConcept
        /// </summary>
        /// <param name="referenceConcept"></param>
        /// <returns></returns>
        public static void DeleteReferenceConcept(ReferenceConcept referenceConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(referenceConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteReferenceConcept", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de ReferenceConcept a partir del filtro
        /// </summary>
        /// <param name="referenceConcept"></param>
        /// <returns></returns>
        public static BusinessCollection ListReferenceConcept(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ReferenceConcept), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListReferenceConcept", ex);
            }

        }

        /// <summary>
        /// obtiene un ReferenceConcept a partir de  conceptId,  entityId
        /// </summary>
        /// <param name="referenceConcept"></param>
        /// <returns></returns>
        public static ReferenceConcept FindReferenceConcept(int conceptId, int entityId)
        {
            try
            {
                ReferenceConcept referenceConcept = null;
                PrimaryKey key = ReferenceConcept.CreatePrimaryKey(conceptId, entityId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    referenceConcept = (ReferenceConcept)daf.GetObjectByPrimaryKey(key);

                }

                return referenceConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindReferenceConcept", ex);
            }
        }
    }
}
