using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using SCREN = Sistran.Core.Application.Script.Entities;
namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public static class RangeConceptDAO
    {
        /// <summary>
        /// crea un RangeConcept
        /// </summary>
        /// <param name="rangeConcept"></param>
        /// <returns></returns>
        public static SCREN.RangeConcept CreateRangeConcept(SCREN.RangeConcept rangeConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(rangeConcept);
                return rangeConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateRangeConcept", ex);
            }

        }

        /// <summary>
        /// edita un RangeConcept
        /// </summary>
        /// <param name="rangeConcept"></param>
        /// <returns></returns>
        public static SCREN.RangeConcept UpdateRangeConcept(SCREN.RangeConcept rangeConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(rangeConcept);
                return rangeConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateRangeConcept", ex);
            }

        }

        /// <summary>
        /// elimina un RangeConcept
        /// </summary>
        /// <param name="rangeConcept"></param>
        /// <returns></returns>
        public static void DeleteRangeConcept(SCREN.RangeConcept rangeConcept)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(rangeConcept);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteRangeConcept", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de RangeConcept a partir del filtro
        /// </summary>
        /// <param name="rangeConcept"></param>
        /// <returns></returns>
        public static BusinessCollection ListRangeConcept(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(
                DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(SCREN.RangeConcept), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListRangeConcept", ex);
            }

        }

        /// <summary>
        /// obtiene un RangeConcept a aprtir de conceptId, entityId
        /// </summary>
        /// <param name="rangeConcept"></param>
        /// <returns></returns>
        public static SCREN.RangeConcept FindRangeConcept(int conceptId, int entityId)
        {
            try
            {
                SCREN.RangeConcept rangeConcept = null;
                PrimaryKey key = SCREN.RangeConcept.CreatePrimaryKey(conceptId, entityId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    rangeConcept = (SCREN.RangeConcept)daf.GetObjectByPrimaryKey(key);
                }

                return rangeConcept;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindRangeConcept", ex);
            }

        }
    }
}
