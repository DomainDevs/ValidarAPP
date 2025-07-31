using System.Collections.Concurrent;

namespace Sistran.Core.Application.AuditServices.EEProvider.Cache
{
    /// <summary>
    /// Entidades Log Auditoria
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// Entidades log Auditoria
        /// </summary>
        private static ConcurrentDictionary<string, object> auditEntities = new ConcurrentDictionary<string, object>();

        public static ConcurrentDictionary<string, object> AuditEntities { get => auditEntities; set => auditEntities = value; }
    }
}
