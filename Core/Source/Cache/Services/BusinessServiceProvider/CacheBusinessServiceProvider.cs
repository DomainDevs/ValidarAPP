using Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Business;
using Sistran.Core.Application.Cache.CacheBusinessService.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.Utilities.DAOs;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider
{
	[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
	public class CacheBusinessServiceProvider : ICacheBusinessService
	{
		public void LoadCache(VersionHistory versionHistory)
		{
            try
            {
                new CacheBusiness().LoadCache(versionHistory);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Libera cache
        /// </summary>
        /// <returns></returns>
        public void RemoveRuleSets()
        {
            RulesEngineDelegate.RemoveRuleSets();
            InProcCache.Remove(RulesConstant.ruleSet);
        }

		/// <summary>
		/// Guardar y notificar un cambio 
		/// </summary>
		/// <returns></returns>
		public Models.VersionHistory CreateVersionHistory(int userId)
		{
			try
			{
				return new CacheBusiness().CreateVersionHistory(userId);
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Ejecutar Recargar caché por Json
		/// </summary>
		/// <returns></returns>
		public void LoadCacheByJson(string historyVersionJson)
		{
			try
			{
				new CacheBusiness().RunReloadCacheByJson(historyVersionJson);
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Guardar registro de que el nodo actualizo la RuleSet
		/// </summary>
		/// <returns></returns>
		/// 
		public NodeRulesetStatus CreateNodeRulesetStatusByHistoryVersion(VersionHistory versionHistory)
		{
			try
			{
				return new CacheBusiness().CreateNodeRulesetStatusByHistoryVersion(versionHistory);
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Guardar registro de que el nodo actualizo la RuleSet
		/// </summary>
		/// <returns></returns>
		/// 
		public NodeRulesetStatus UpdateNodeRulesetStatusByFinish(NodeRulesetStatus nodeRulesetStatus)
		{
			try
			{
				return new CacheBusiness().UpdateNodeRulesetStatusByFinish(nodeRulesetStatus);
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Listado de maquinas con la lista de reglas actuales
		/// </summary>
		/// <returns>NodeRulessetEsatus</returns>
		public List<Models.NodeRulesetStatus> GetNodeRulSet()
		{
			try
			{
				return new CacheBusiness().GetNodeRulSet();
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Obtiene la lista las ultimas versiones Publicadas
		/// </summary>
		/// <returns>NodeRulessetEsatus</returns>
		public List<Models.VersionHistory> GetVersionHistory(int count)
		{
			try
			{
				return new CacheBusiness().GetVersionHistory(count);
			}
			catch (Exception ex)
			{
				throw new BusinessException(ex.Message, ex);
			}
		}

        public void LoadEnumParameterValuesFromDB()
        {
            try
            { 
                LoadParametrizationSystemDAO loadParametrizationSystemDAO = new LoadParametrizationSystemDAO();
                loadParametrizationSystemDAO.LoadEnumParameterValuesFromDB();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}