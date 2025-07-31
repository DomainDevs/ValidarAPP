using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    public class PositionEntityDAO
    {
        /// <summary>
        /// Obtiene el PositionEntity segun los parametros de busqueda
        /// </summary>
        /// <param name="entityId">Identificador de Entity</param>
        /// <param name="levelId">Identificador de Level</param>
        /// <param name="packageId">Identificador de Package</param>
        /// <returns></returns>
        public static PositionEntity GetPositionEntityByEntityIdLevelIdPackageId(int entityId, int levelId, int packageId)
        {
            try
            {
                PrimaryKey key = PositionEntity.CreatePrimaryKey(entityId, levelId, packageId);

                return (PositionEntity)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetPositionEntityByEntityIdLevelIdPackageId", ex);
            }
            
        }

        /// <summary>
        /// Obtiene una lista de PositionEntity segun el fitro suministrado y el orden espesificado
        /// </summary>
        /// <param name="filter">Filtro a aplicar en la busqueda</param>
        /// <param name="sort">Orden en que se devuelven los resultados</param>
        /// <returns></returns>
        public static IList ListPositionEntityByFilterSort(Predicate filter, string[] sort)
        {
            try
            {
                return (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(PositionEntity), filter, sort);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListPositionEntityByFilterSort", ex);
            }
            
        }
    }
}
