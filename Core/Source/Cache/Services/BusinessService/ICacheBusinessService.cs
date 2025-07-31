using Sistran.Core.Application.Cache.CacheBusinessService;
using Sistran.Core.Application.Cache.CacheBusinessService.Models;
using System.Collections.Generic;
using System.ServiceModel;
using CACHEMODEL = Sistran.Core.Application.Cache.CacheBusinessService.Models;

namespace Sistran.Core.Application.Cache.CacheBusinessService
{
    [ServiceContract]
    public interface ICacheBusinessService
    {
        /// <summary>
        /// Crear version de cache
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        VersionHistory CreateVersionHistory(int userId);

        /// <summary>
        /// Compilar y agregar reglas a cache
        /// </summary>
        [OperationContract]
        void LoadCache(VersionHistory versionHistory);
        
        /// <summary>
        /// Listado de maquinas con la lista de reglas actuales
        /// </summary>
        /// <returns>NodeRulessetEsatus</returns>
        [OperationContract]
        List<CACHEMODEL.NodeRulesetStatus> GetNodeRulSet();

        /// <summary>
        /// Obtiene la lista las ultimas versiones Publicadas
        /// </summary>
        /// <returns>NodeRulessetEsatus</returns>
        [OperationContract]
        List<CACHEMODEL.VersionHistory> GetVersionHistory(int count);

        

        /// <summary>
        /// Guardar registro de que el nodo actualizo la RuleSet
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        CACHEMODEL.NodeRulesetStatus CreateNodeRulesetStatusByHistoryVersion(CACHEMODEL.VersionHistory versionHistory);

        /// <summary>
        /// Ejecutar Recargar caché por Json
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void LoadCacheByJson(string historyVersionJson);

        /// <summary>
        /// Libera cache
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        void RemoveRuleSets();

        /// <summary>
        /// Metodo para cargar la parametrización del sistema.
        /// </summary>
        [OperationContract]
        void LoadEnumParameterValuesFromDB();
    }
}