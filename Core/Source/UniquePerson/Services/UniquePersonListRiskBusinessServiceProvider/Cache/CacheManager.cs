using System.Collections.Generic;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider
{
    public class CacheManager
    {
        public static IEnumerable<UPEN.ViewListRiskPerson> entityViewListRisks = new List<UPEN.ViewListRiskPerson>();
        public static Dictionary<string, int> umbrals = new Dictionary<string, int>();
    }
}
