using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using CACHEMODEL = Sistran.Core.Application.Cache.CacheBusinessService.Models;

namespace Sistran.Core.Application.Cache.CacheBusinessService.EEProvider.Assemblers
{   
    public class EntityAssembler
    {
        public static PARAMEN.VersionHistory CreateVersionHistory(CACHEMODEL.VersionHistory versionHistory)
        {
            return new PARAMEN.VersionHistory
            {
                Id = versionHistory.Id,
                Guid = versionHistory.Guid,
                UserId = versionHistory.UserId,
                VersionDatetime = versionHistory.VersionDatetime
            };
        }

        public static PARAMEN.NodeRulesetStatus CreateNodeRulesetStatus(CACHEMODEL.NodeRulesetStatus nodeRulesetStatus)
        {
            return new PARAMEN.NodeRulesetStatus
            {
                Id = nodeRulesetStatus.Id,
                Node = nodeRulesetStatus.Node,
                Guid = nodeRulesetStatus.Guid,
                CreationDate = nodeRulesetStatus.CreationDate
            };
        }
    }
}