using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Views;
using Sistran.Core.Framework.DAF.Engine;
using System;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.DataFacade;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class ListEntityValueDAO
    {
        /// <summary>
        /// crea un ListEntityValue
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static Models.ListEntityValue CreateListEntityValue(Models.ListEntityValue listEntityValue)
        {
            try
            {
                SCREN.ListEntityValue entity = EntityAssembler.CreateListEntityValue(listEntityValue);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                return listEntityValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorCreateListEntityValue, listEntityValue.ListValue), ex);
            }
        }

        /// <summary>
        /// edita un ListEntityValue
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static Models.ListEntityValue UpdateListEntityValue(Models.ListEntityValue listEntityValue)
        {
            try
            {
                PrimaryKey key = SCREN.ListEntityValue.CreatePrimaryKey(listEntityValue.ListValueCode, listEntityValue.ListEntityCode);
                SCREN.ListEntityValue entityValue = (SCREN.ListEntityValue)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                entityValue.ListValue = listEntityValue.ListValue;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityValue);
                return listEntityValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorUpdateListEntityValue, listEntityValue.ListValue), ex);
            }
        }

        /// <summary>
        /// elimina un ListEntityValue
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static void DeleteListEntityValue(Models.ListEntityValue listEntityValue)
        {
            try
            {
                PrimaryKey key = SCREN.ListEntityValue.CreatePrimaryKey(listEntityValue.ListValueCode, listEntityValue.ListEntityCode);
                SCREN.ListEntityValue entityValue = (SCREN.ListEntityValue)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorDeleteListEntityValue, listEntityValue.ListValue), ex);
            }
        }

        /// <summary>
        /// obtien una lista de ListEntityValue a partir del filtro
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static BusinessCollection ListListEntityValue(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(SCREN.ListEntityValue), filter, sort));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListListEntityValue", ex);
            }

        }

        /// <summary>
        /// obtiene un  ListEntityValue a partir de  listEntityCode, listValueCode
        /// </summary>
        /// <param name="listEntityValue"></param>
        /// <returns></returns>
        public static SCREN.ListEntityValue FindListEntityValue(int listEntityCode, int listValueCode)
        {
            try
            {
                PrimaryKey key = SCREN.ListEntityValue.CreatePrimaryKey(listValueCode, listEntityCode);
                SCREN.ListEntityValue listEntityValue = (SCREN.ListEntityValue)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return listEntityValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindListEntityValue", ex);
            }

        }

        /// <summary>
        /// Obtener lista de valores por entidad
        /// </summary>
        /// <param name="entityId">Id entidad</param>
        /// <returns>Lista de valores</returns>
        public static List<Models.ListEntityValue> GetListEntityValuesByConceptIdEntityId(int conceptId, int entityId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(ListConcept.Properties.ConceptId, "ListConcepts", conceptId);
                filter.And();
                filter.PropertyEquals(ListConcept.Properties.EntityId, "ListConcepts", entityId);

                ListValuesView view = new ListValuesView();
                ViewBuilder builder = new ViewBuilder("ListValuesView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                if (view.ListEntityValues.Count > 0)
                {
                    return ModelAssembler.CreateListEntityValues(view.ListEntityValues);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntityValuesByConceptIdEntityId", ex);
            }

        }
    }
}
