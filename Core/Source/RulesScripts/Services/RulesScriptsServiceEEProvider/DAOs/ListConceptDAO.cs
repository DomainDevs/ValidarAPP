using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class ListConceptDAO
    {
        /// <summary>
        /// crea un ListConcept
        /// </summary>
        /// <param name="listConcept"></param>
        /// <returns></returns>
        public static ListConcept CreateListConcept(ListConcept listConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(listConcept);
                return listConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateListConcept", ex);
            }
            
        }

        /// <summary>
        /// edita un ListConcept
        /// </summary>
        /// <param name="listConcept"></param>
        /// <returns></returns>
        public static ListConcept UpdateListConcept(ListConcept listConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(listConcept);
                return listConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateListConcept", ex);
            }
            
        }

        /// <summary>
        /// elimina un ListConcept
        /// </summary>
        /// <param name="listConcept"></param>
        /// <returns></returns>
        public static void DeleteListConcept(ListConcept listConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(listConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteListConcept", ex);
            }
            
        }

        /// <summary>
        /// obtiene una lista de ListConcept a partir del filtro
        /// </summary>
        /// <param name="listConcept"></param>
        /// <returns></returns>
        public static BusinessCollection ListListConcept(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
              DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ListConcept), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListListConcept", ex);
            }
          
        }

        /// <summary>
        /// obtiene un ListConcept a partir del  conceptId,  entityId
        /// </summary>
        /// <param name="listConcept"></param>
        /// <returns></returns>
        public static SCREN.ListConcept FindListConcept(int conceptId, int entityId)
        {
            try
            {
                SCREN.ListConcept listConcept = null;
                PrimaryKey key = SCREN.ListConcept.CreatePrimaryKey(conceptId, entityId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    listConcept = (SCREN.ListConcept)daf.GetObjectByPrimaryKey(key);
                }

                return listConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindListConcept", ex);
            }

        }

        /// <summary>
        /// Obtiene el ListConcept segun ConceptId y EntotyId especificados
        /// </summary>
        /// <param name="conceptId">The concept identifier.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns></returns>
        public static ListConcept GetListConceptByConceptIdEntityId(int conceptId, int entityId)
        {
            try
            {
                PrimaryKey key = ListConcept.CreatePrimaryKey(conceptId, entityId);

                return (ListConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListConceptByConceptIdEntityId", ex);
            }

        }
    }
}
