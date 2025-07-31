using System;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using CACHEMODEL = Sistran.Core.Application.Cache.CacheBusinessService.Models;

namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {

        public static CACHEMODEL.NodeRulesetStatus CreateNodeRulesetStatus(PARAMEN.NodeRulesetStatus nodeRulesetStatus)
        {
            return new CACHEMODEL.NodeRulesetStatus
            {
                Id = nodeRulesetStatus.Id,
                Guid = nodeRulesetStatus.Guid,
                Node = nodeRulesetStatus.Node,
                CreationDate = nodeRulesetStatus.CreationDate,
				FinishDate = nodeRulesetStatus.FinishDate
			};
        }

        public static CACHEMODEL.VersionHistory CreateVersionHistory(PARAMEN.VersionHistory versionHistory)
        {
            return new CACHEMODEL.VersionHistory
            {
                Id = versionHistory.Id,
                Guid = versionHistory.Guid,
                UserId = versionHistory.UserId,
                VersionDatetime = versionHistory.VersionDatetime
            };
        }

    }
}
 