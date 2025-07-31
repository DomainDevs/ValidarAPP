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

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RangeEntityDAO
    {
        /// <summary>
        /// obtiene una lista de RangeEntity a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static BusinessCollection ListRangeEntity(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
                    DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RangeEntity), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRangeEntity", ex);
            }
        }

        /// <summary>
        /// obtiene un RangeEntity a partir de rangeEntityCode
        /// </summary>
        /// <param name="rangeEntityCode"></param>
        /// <returns></returns>
        public static RangeEntity FindRangeEntity(int rangeEntityCode)
        {
            try
            {
                PrimaryKey key = RangeEntity.CreatePrimaryKey(rangeEntityCode);
                RangeEntity rangeEntity = (RangeEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return rangeEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindRangeEntity", ex);
            }

        }

        /// <summary>
        /// obtiene un RangeValuesView a partir del filtro
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static RangeValuesView GetRangeValuesView(Predicate filter)
        {
            try
            {
                RangeValuesView view = new RangeValuesView();
                ViewBuilder builder = new ViewBuilder("RangeValuesView");

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
                throw new BusinessException("Error Obtener GetRangeValuesView", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de Models.RangeEntity
        /// </summary>
        /// <returns></returns>
        public static List<Models.RangeEntity> GetRangeEntity()
        {
            try
            {
                BusinessCollection bcRangeEntity = new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RangeEntity)));

                List<Models.RangeEntity> rangeEntity = ModelAssembler.CreateRangeEntities(bcRangeEntity);

                if (rangeEntity.Count != 0)
                {
                    int counter = 0;
                    foreach (Models.RangeEntity item in rangeEntity)
                    {
                        ObjectCriteriaBuilder bcFilter = new ObjectCriteriaBuilder();
                        bcFilter.Property(Entities.RangeEntityValue.Properties.RangeEntityCode, typeof(Entities.RangeEntityValue).Name);
                        bcFilter.Equal();
                        bcFilter.Constant(item.RangeEntityCode);

                        var bcrangeEntityValue = RangeEntityValueDAO.ListRangeEntityValue(bcFilter.GetPredicate(), null);

                        rangeEntity[counter].RangeEntityValue = ModelAssembler.CreateRangeEntityValues(bcrangeEntityValue);
                        rangeEntity[counter].RangeEntityValue.ForEach(x => x.RangeEntityCode = item.RangeEntityCode);
                        counter++;
                    }
                }


                return rangeEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRangeEntity", ex);
            }
        }

        /// <summary>
        /// obtiene una lista de Models.RangeEntity a partir de Description
        /// </summary>
        /// <param name="Description"></param>
        /// <returns></returns>
        public static List<Models.RangeEntity> GetRangeEntityByDescription(string Description)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Entities.RangeEntity.Properties.Description, typeof(Entities.RangeEntity).Name);
                filter.Like();
                filter.Constant("%" + Description + "%");

                var rangeEntity = DataFacadeManager.Instance.GetDataFacade().List(typeof(RangeEntity),
                    filter.GetPredicate(), new[] { RangeEntity.Properties.Description });

                return ModelAssembler.CreateRangeEntities(rangeEntity);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetRangeEntityByDescription", ex);
            }
        }

        /// <summary>
        /// obtiene una lista de Models.RangeEntity a partir de rangeEntityCode
        /// </summary>
        /// <param name="rangeEntityCode"></param>
        /// <returns></returns>
        public static List<Models.RangeEntity> GetRangeEntityValueByRangeEntityCode(int rangeEntityCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(RangeEntity.Properties.RangeEntityCode, typeof(RangeEntity).Name);
            filter.Equal();
            filter.Constant(rangeEntityCode);

            var bcRangeEntity = new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RangeEntity), filter.GetPredicate(), null));

            List<Models.RangeEntity> rangeEntity = ModelAssembler.CreateRangeEntities(bcRangeEntity);

            if (rangeEntity.Count != 0)
            {
                ObjectCriteriaBuilder bcFilter = new ObjectCriteriaBuilder();
                bcFilter.Property(RangeEntityValue.Properties.RangeEntityCode, typeof(Entities.RangeEntityValue).Name);
                bcFilter.Equal();
                bcFilter.Constant(rangeEntityCode);

                var bcRangeEntityValue = RangeEntityValueDAO.ListRangeEntityValue(bcFilter.GetPredicate(), null);

                rangeEntity[0].RangeEntityValue = new List<Models.RangeEntityValue>();
                rangeEntity[0].RangeEntityValue = ModelAssembler.CreateRangeEntityValues(bcRangeEntityValue);
                rangeEntity[0].RangeEntityValue.ForEach(x => x.RangeEntityCode = rangeEntityCode);
            }

            return rangeEntity;
        }

        /// <summary>
        /// crea un   Models.RangeEntity 
        /// </summary>
        /// <param name="rangeEntity"></param>
        /// <returns></returns>
        public static Models.RangeEntity CreateRangeEntity(Models.RangeEntity rangeEntity)
        {
            try
            {
                RangeEntity entity = EntityAssembler.CreateRangeEntity(rangeEntity);

                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                rangeEntity.RangeEntityCode = entity.RangeEntityCode;

                return rangeEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorCreateRangeEntity, ex);
            }
        }

        /// <summary>
        /// edita un   Models.RangeEntity 
        /// </summary>
        /// <param name="rangeEntity"></param>
        /// <returns></returns>
        public static Models.RangeEntity UpdateRangeEntity(Models.RangeEntity rangeEntity)
        {
            try
            {
                PrimaryKey key = RangeEntity.CreatePrimaryKey(rangeEntity.RangeEntityCode);
                RangeEntity entity = (RangeEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                entity.Description = rangeEntity.Description;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entity);
                return rangeEntity;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorUpdateRangeEntity, ex);
            }
        }

        /// <summary>
        /// elimina  un   Models.RangeEntity 
        /// </summary>
        /// <param name="rangeEntity"></param>
        /// <returns></returns>
        public static bool DeleteRangeEntity(Models.RangeEntity rangeEntity)
        {
            try
            {
                PrimaryKey key = RangeEntity.CreatePrimaryKey(rangeEntity.RangeEntityCode);
                RangeEntity entity = (RangeEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entity);
                return true;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorDeleteRangeEntity, ex);
            }
        }
    }
}
