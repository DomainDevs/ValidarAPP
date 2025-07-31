
using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RangeEntityValueDAO
    {
        /// <summary>
        /// crea un RangeEntityValue
        /// </summary>
        /// <param name="rangeEntityValue"></param>
        /// <returns></returns>
        public static Models.RangeEntityValue CreateRangeEntityValue(Models.RangeEntityValue rangeEntityValue)
        {
            try
            {
                RangeEntityValue entity = Assemblers.EntityAssembler.CreateRangeEntityValue(rangeEntityValue);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(entity);
                return rangeEntityValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorCreateRangeEntityValue, ex);
            }
        }

        /// <summary>
        /// edita un RangeEntityValue
        /// </summary>
        /// <param name="rangeEntityValue"></param>
        /// <returns></returns>
        public static Models.RangeEntityValue UpdateRangeEntityValue(Models.RangeEntityValue rangeEntityValue)
        {
            try
            {
                PrimaryKey key = RangeEntityValue.CreatePrimaryKey(rangeEntityValue.RangeValueCode, rangeEntityValue.RangeEntityCode);
                RangeEntityValue entityValue = (RangeEntityValue)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                entityValue.FromValue = rangeEntityValue.FromValue;
                entityValue.ToValue = rangeEntityValue.ToValue;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityValue);
                return rangeEntityValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorUpdateRangeEntityValue, ex);
            }
        }

        /// <summary>
        /// elimina un RangeEntityValue
        /// </summary>
        /// <param name="rangeEntityValue"></param>
        /// <returns></returns>
        public static void DeleteRangeEntityValue(Models.RangeEntityValue rangeEntityValue)
        {
            try
            {
                PrimaryKey key = RangeEntityValue.CreatePrimaryKey(rangeEntityValue.RangeValueCode, rangeEntityValue.RangeEntityCode);
                RangeEntityValue entityValue = (RangeEntityValue)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityValue);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Resources.Errors.ErrorDeleteRangeEntityValue, ex);
            }
        }

        /// <summary>
        /// obtien una listya de  RangeEntityValue a partir del filtro
        /// </summary>
        /// <param name="rangeEntityValue"></param>
        /// <returns></returns>
        public static BusinessCollection ListRangeEntityValue(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
               DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(RangeEntityValue), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRangeEntityValue", ex);
            }

        }

        /// <summary>
        /// obtien un  RangeEntityValue a partir de  rangeEntityCode, rangeValueCode
        /// </summary>
        /// <param name="rangeEntityValue"></param>
        /// <returns></returns>
        public static RangeEntityValue FindRangeEntityValue(int rangeEntityCode, int rangeValueCode)
        {
            try
            {
                PrimaryKey key = RangeEntityValue.CreatePrimaryKey(rangeValueCode, rangeEntityCode);
                RangeEntityValue listEntityValue = (RangeEntityValue)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return listEntityValue;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindRangeEntityValue", ex);
            }

        }
    }
}
