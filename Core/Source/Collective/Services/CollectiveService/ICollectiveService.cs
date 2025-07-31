using System;
using System.ServiceModel;
using System.Collections.Generic;
using Sistran.Core.Application.CollectiveServices.Models;

namespace Sistran.Core.Application.CollectiveServices
{
    [ServiceContract]
    public interface ICollectiveServiceCore
    {
        /// <summary>
        /// Crear Cargue Colectivo
        /// </summary>
        /// <param name="collectiveLoad">Cargue Colectivo</param>
        /// <returns>Cargue Colectivo</returns>
        [OperationContract]
        CollectiveLoad CreateCollectiveLoad(CollectiveLoad collectiveLoad);

        /// <summary>
        /// Obtener Cargues Colectivos Por Temporal
        /// </summary>
        /// <param name="temporalId">Id Temporal</param>
        /// <returns>Cargues Colectivos</returns>
        [OperationContract]
        List<CollectiveLoad> GetCollectiveLoadsByTemporalId(int temporalId);
        
        /// <summary>
        /// Obtener Tipos De Cargue Por Tipo De Endoso
        /// </summary>
        /// <param name="endosermentTypeId">Id Tipo De Endoso</param>
        /// <returns>Tipos De Cargue</returns>
        [OperationContract]
        List<LoadType> GetLoadTypesByEndosermentTypeId(int endosermentTypeId);        
    }
}