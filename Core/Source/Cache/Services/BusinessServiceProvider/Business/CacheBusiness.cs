using Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Assemblers;
using Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Enums;
using Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Resources;
using System;
using System.Linq;
using Sistran.Core.Application.Cache.CacheBusinessService.Models;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.Utilities.RulesEngine;
using System.Diagnostics;
using System.Data;
using Sistran.Co.Application.Data;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using SCREN = Sistran.Core.Application.Script.Entities;
using System.Configuration;
using Sistran.Core.Framework.Queues;
using Newtonsoft.Json;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Business
{
	public class CacheBusiness
    {
        public void LoadCache(VersionHistory versionHistory)
        {
            bool removeCache = false;
            int version = 0;

            if (versionHistory == null)
            {
                versionHistory = new Models.VersionHistory();
                List<VersionHistory> versionHistories = GetVersionHistory(1);

                if (versionHistories != null && versionHistories.Count > 0)
                {
                    versionHistory = versionHistories.FirstOrDefault();
                    version = versionHistory.Id;
                }
                else
                {
                    versionHistory.Guid = default(Guid).ToString();
                }
            }
            else
            {
                version = versionHistory.Id;
                removeCache = true;
            }

            NodeRulesetStatus nodeRulesetStatus = CreateNodeRulesetStatusByHistoryVersion(versionHistory);

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(SCREN.RuleSet.Properties.Active, typeof(SCREN.RuleSet).Name, true);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(SCREN.RuleSet), filter.GetPredicate()));

            foreach (SCREN.RuleSet entityRuleSet in businessCollection)
            {
                try
                {
                    InProcCache.Add(RulesConstant.ruleSet, entityRuleSet.RuleSetId.ToString(), entityRuleSet);

                    RulesEngineDelegate.CompileRuleSet(entityRuleSet.RuleSetId, version);
                }
                catch (Exception ex)
                {
                    EventLog.WriteEntry("Application", "Error Load Rule: " + entityRuleSet.RuleSetId + " " + ex.Message, EventLogEntryType.Error);
                }
            }

            int versionOld = InProcCache.GetCurrentVersion();
            InProcCache.SetCurrentVersion(version);

            UpdateNodeRulesetStatusByFinish(nodeRulesetStatus);

            if (removeCache)
            {
                RulesEngineDelegate.RemoveRuleSets();
                InProcCache.Remove(RulesConstant.ruleSet);
            }
        }

        public void RunReloadCacheByJson(string historyVersionJson)
        {
            VersionHistory versionHistory = HelperAssembler.DeserializeJson<VersionHistory>(historyVersionJson);
            LoadCache(versionHistory);
        }

        public void TimetoPublish()
        {
            VersionHistory versionHistories = GetVersionHistory(1).FirstOrDefault();

            if (versionHistories!= null)
            {
                DateTime lastPublish = versionHistories.VersionDatetime;
                DateTime expectative = lastPublish.AddHours((int)TimeToPublish.Hours).AddMinutes((int)TimeToPublish.Minutes).AddSeconds((int)TimeToPublish.Seconds);
                TimeSpan timeSpan = expectative - DateTime.Now;

                if (timeSpan.Hours > 0 || timeSpan.Minutes > 0 || timeSpan.Seconds > 0)
                {
                    throw new Exception(string.Format(Errors.ErrorTimeCache,timeSpan.Hours,timeSpan.Minutes, timeSpan.Seconds));
                }
            }
        }

        /// <summary>
		/// Obtiene la lista de nodos  con actual version de cache
		/// </summary>
		/// <returns></returns>
		public List<NodeRulesetStatus> GetNodeRulSet()
        {
            DataTable dataTable;
            NameValue[] parameters = new NameValue[0];

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                dataTable = pdb.ExecuteSPDataTable("PARAM.GET_NODE_RULE_SET_STATUS", parameters);
            }

            List<NodeRulesetStatus> listNodeRulesetStatus = new List<NodeRulesetStatus>();

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow row = dataTable.Rows[i];
                    NodeRulesetStatus nodeRulesetStatus = new NodeRulesetStatus();
                    nodeRulesetStatus.Id = (int)row[0];
                    nodeRulesetStatus.Node = (string)row[1];
                    nodeRulesetStatus.Guid = (string)row[2];
                    nodeRulesetStatus.CreationDate = (DateTime)row[3];

                    if (!string.IsNullOrEmpty(row[4].ToString()))
                    {
                        nodeRulesetStatus.FinishDate = (DateTime)row[4];
                    }

                    listNodeRulesetStatus.Add(nodeRulesetStatus);
                }
            }

            return listNodeRulesetStatus;
        }

        /// <summary>
        /// Obtiene la lista las ultimas versiones Publicadas
        /// </summary>
        /// <returns></returns>
        public List<VersionHistory> GetVersionHistory(int count)
        {
            string[] sort = new string[1];
            sort[0] = "-" + PARAMEN.VersionHistory.Properties.Id;

            IDataReader response = DataFacadeManager.Instance.GetDataFacade().SelectObjectsPartial(typeof(PARAMEN.VersionHistory), null, sort, 0, count, false);
            BusinessCollection businessCollection = new BusinessCollection(response);

            return businessCollection.Select(x => ModelAssembler.CreateVersionHistory((PARAMEN.VersionHistory)x)).OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// Guardar y notificar un cambio 
        /// </summary>
        /// <returns></returns>
        ///
        public VersionHistory CreateVersionHistory(int userId)
        {
            TimetoPublish();

            VersionHistory versionHistory = new VersionHistory()
            {
                UserId = userId,
                VersionDatetime = DateTime.Now,
                Guid = Guid.NewGuid().ToString()
            };

            PARAMEN.VersionHistory versionHistoryEntity = EntityAssembler.CreateVersionHistory(versionHistory);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(versionHistoryEntity);

            versionHistory = ModelAssembler.CreateVersionHistory(versionHistoryEntity);

            string HistoryVersionJson = JsonConvert.SerializeObject(versionHistory);
            PutOnQueueJsonByExchange(HistoryVersionJson, "RuleSetCache.fanout");
            
            return versionHistory;
        }

        public static void PutOnQueueJsonByExchange(object item, string keyExchangeName)
        {
            try
            {
                //string exchangeName = ConfigurationManager.AppSettings[keyExchangeName];
                //if (string.IsNullOrEmpty(exchangeName))
                //{
                //    exchangeName = keyExchangeName;
                //}
                //CreateQueueParameters createQueueParameters = new CreateQueueParameters
                //{
                //    QueueName = string.Empty,
                //    ExchangeName = exchangeName,
                //    ExchangeType = ExchangeType.Fanout,
                //    RoutingKey = string.Empty
                //}; 
            string queueName = Environment.MachineName.ToString();
                queueName += " - " + "INTERACTIVO";
        
            string exchangeName = ConfigurationManager.AppSettings[keyExchangeName];
            Dictionary<string, object> args = new Dictionary<string, object>
                {
                    { ArgumentsNames.DeadLetterExchange, exchangeName },
                    { ArgumentsNames.MaxLength, 500 }
                };
                CreateQueueParameters createQueueParameter = new CreateQueueParameters()
                {
                    QueueName = queueName,
                    ExchangeName = exchangeName,
                    ExchangeType = "fanout",
                    RoutingKey = string.Empty,
                    PersistentMessages = true,
                    Arguments = args
                };
                PutOnQueue(createQueueParameter, item);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                throw ex;
            }
        }

        private static void PutOnQueue(CreateQueueParameters createQueueParameters, object item)
        {
            try
            {
                createQueueParameters.Serialization = ConfigurationManager.AppSettings["Serialization"];
                IQueue queue = new BaseQueueFactory().CreateQueue(createQueueParameters);
                queue.PutOnQueue(item);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("Application", ex.Message, EventLogEntryType.Error);
                throw ex;
            }
        }

        /// <summary>
        /// Guardar registro de que el nodo actualizo la RuleSet
        /// </summary>
        /// <returns></returns>
        public NodeRulesetStatus CreateNodeRulesetStatusByHistoryVersion(VersionHistory versionHistory)
        {
            string node = Environment.MachineName;
            node += " - " + ConfigurationManager.AppSettings["ServiceNode"];
            NodeRulesetStatus nodeRulesetStatus = new NodeRulesetStatus()
            {
                Id = 0,
                Node = node,
                Guid = versionHistory.Guid,
                CreationDate = DateTime.Now
            };

            PARAMEN.NodeRulesetStatus nodeRulesetStatusEntity = EntityAssembler.CreateNodeRulesetStatus(nodeRulesetStatus);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(nodeRulesetStatusEntity);

            nodeRulesetStatus = ModelAssembler.CreateNodeRulesetStatus(nodeRulesetStatusEntity);
            return nodeRulesetStatus;
        }

        /// <summary>
        /// Guardar registro de que el nodo actualizo la RuleSet
        /// </summary>
        /// <returns></returns>
        public NodeRulesetStatus UpdateNodeRulesetStatusByFinish(NodeRulesetStatus nodeRulesetStatus)
        {
            PrimaryKey key = PARAMEN.NodeRulesetStatus.CreatePrimaryKey(nodeRulesetStatus.Id);

            PARAMEN.NodeRulesetStatus nodeRulesetStatusEntity = (PARAMEN.NodeRulesetStatus)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            nodeRulesetStatusEntity.FinishDate = DateTime.Now;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(nodeRulesetStatusEntity);

            return ModelAssembler.CreateNodeRulesetStatus(nodeRulesetStatusEntity);
        }
    }
}