using System;
using System.Collections.Generic;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Views;
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Utilities.DataFacade;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    using CommonService.Enums;

    public static class ListEntityDAO
    {
        /// <summary>
        /// obtiene una lista de ListEntity a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListListEntity(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ListEntity), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListListEntity", ex);
            }

        }

        /// <summary>
        /// obtiene un ListEntity a partir de listEntityCode
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static ListEntity FindListEntity(int listEntityCode)
        {
            try
            {
                PrimaryKey key = ListEntity.CreatePrimaryKey(listEntityCode);
                ListEntity listEntity = (ListEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return listEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindListEntity", ex);
            }

        }

        /// <summary>
        /// obtiene un ListValuesView a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static ListValuesView GetListValuesView(Predicate filter)
        {
            try
            {
                ListValuesView view = new ListValuesView();
                ViewBuilder builder = new ViewBuilder("ListValuesView");

                if (filter != null)
                {
                    builder.Filter = filter;
                }

                //Activar y Llenar la vista ...
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                return view;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListValuesView", ex);
            }

        }

        /// <summary>
        /// Obtener todas las listas de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public static List<Models.ListEntity> GetListEntity()
        {
            try
            {
                BusinessCollection bcListEntity = new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ListEntity)));

                List<Models.ListEntity> listEntity = ModelAssembler.CreateListEntities(bcListEntity);

                if (listEntity.Count != 0)
                {
                    int counter = 0;
                    foreach (Models.ListEntity item in listEntity)
                    {
                        ObjectCriteriaBuilder bcFilter = new ObjectCriteriaBuilder();
                        bcFilter.Property(SCREN.ListEntityValue.Properties.ListEntityCode, typeof(SCREN.ListEntityValue).Name);
                        bcFilter.Equal();
                        bcFilter.Constant(item.ListEntityCode);

                        var bclistEntityValue = ListEntityValueDAO.ListListEntityValue(bcFilter.GetPredicate(), null);

                        listEntity[counter].ListEntityValue = ModelAssembler.CreateListEntityValues(bclistEntityValue);
                        listEntity[counter].ListEntityValue.ForEach(x => x.ListEntityCode = item.ListEntityCode);
                        counter++;
                    }
                }

                return listEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntity", ex);
            }
        }

        /// <summary>
        /// Obtener las listas de valores que coinciden con la descripción.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public static List<Models.ListEntity> GetListEntityByDescription(string Description)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.ListEntity.Properties.Description, typeof(Entities.ListEntity).Name);
                filter.Like();
                filter.Constant('%' + Description + '%');

                var bcListEntity = new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ListEntity), filter.GetPredicate(), new[] { ListEntity.Properties.Description }));

                return ModelAssembler.CreateListEntities(bcListEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntityByDescription", ex);
            }
        }

        /// <summary>
        /// Obtener los valores de lista por código de lista de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de listas de valores</returns>
        public static List<Models.ListEntity> GetListEntityValueByListEntityCode(int listEntityCode)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.ListEntity.Properties.ListEntityCode, typeof(Entities.ListEntity).Name);
                filter.Equal();
                filter.Constant(listEntityCode);

                var bcListEntity = new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ListEntity), filter.GetPredicate(), null));

                List<Models.ListEntity> listEntity = ModelAssembler.CreateListEntities(bcListEntity);

                if (listEntity.Count != 0)
                {
                    ObjectCriteriaBuilder bcFilter = new ObjectCriteriaBuilder();
                    bcFilter.Property(SCREN.ListEntityValue.Properties.ListEntityCode, typeof(SCREN.ListEntityValue).Name);
                    bcFilter.Equal();
                    bcFilter.Constant(listEntityCode);

                    var bclistEntityValue = ListEntityValueDAO.ListListEntityValue(bcFilter.GetPredicate(), null);

                    listEntity[0].ListEntityValue = new List<Models.ListEntityValue>();
                    listEntity[0].ListEntityValue = ModelAssembler.CreateListEntityValues(bclistEntityValue);
                    listEntity[0].ListEntityValue.ForEach(x => x.ListEntityCode = listEntityCode);
                }

                return listEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetListEntity", ex);
            }
        }

        /// <summary>
        /// Crear una lista de valores.
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns>lista de valores</returns>
        public static Models.ListEntity CreateListEntity(Models.ListEntity listEntity)
        {
            try
            {
                ListEntity entity = EntityAssembler.CreateListEntity(listEntity);

                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                listEntity.ListEntityCode = entity.ListEntityCode;

                return listEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorCreateListEntity, listEntity.Description), ex);
            }
        }

        /// <summary>
        /// Actualizar una lista de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>lista de valores</returns>
        public static Models.ListEntity UpdateListEntity(Models.ListEntity listEntity)
        {
            try
            {
                PrimaryKey key = ListEntity.CreatePrimaryKey(listEntity.ListEntityCode);
                ListEntity entity = (ListEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                entity.Description = listEntity.Description;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
                return listEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorUpdateListEntity, listEntity.Description), ex);
            }
        }

        /// <summary>
        /// Eliminar una lista de valores.
        /// </summary>
        /// <param name=""></param>
        /// <returns>confirmacion booleana true/false</returns>
        public static bool DeleteListEntity(Models.ListEntity listEntity)
        {
            try
            {
                PrimaryKey key = ListEntity.CreatePrimaryKey(listEntity.ListEntityCode);
                ListEntity entity = (ListEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(string.Format(Resources.Errors.ErrorDeleteListEntity, listEntity.Description), ex);
            }
        }
    }
}
