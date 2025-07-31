namespace Sistran.Core.Application.AuditServices.EEProvider.Assemblers
{
    using System;
    using Newtonsoft.Json;
    using Sistran.Core.Application.AuditServices.EEProvider.Cache;
    using Sistran.Core.Framework.DAF.Audit;
    using AUDEN = Sistran.Core.Application.Audit.Entities;

    public class EntityAssembler
    {
        #region Auditoria
        public static AUDEN.LogAudit CreateAudit(AuditData auditData)
        {
            int? objectCode = null;
            string objectDescription = string.Empty;
            if (CacheManager.AuditEntities.ContainsKey(auditData.Object))
            {
                objectCode = ((AUDEN.LogEntity)CacheManager.AuditEntities[auditData.Object]).Id;
                objectDescription = ((AUDEN.LogEntity)CacheManager.AuditEntities[auditData.Object]).Description;
            }
            return new AUDEN.LogAudit(0)
            {
                PK = JsonConvert.SerializeObject(auditData.PrimaryKey),
                RegisterDate = DateTime.Now,
                Data = JsonConvert.SerializeObject(auditData.Changes),
                ObjectType = JsonConvert.SerializeObject(auditData.Object),
                ObjectDescription = objectDescription,
                TypeCode = (int?)auditData.ActionType,
                UserId = auditData.UserId,
                ObjectCode = objectCode,
                IpDescription = string.IsNullOrEmpty(auditData?.Ip) ? "" : auditData.Ip,
                IsSerialize = auditData.IsSerialize,
                ColumnName = string.IsNullOrEmpty(auditData?.ColumnName) ? "" : auditData.ColumnName,
                ColumnDescription = auditData.Description
            };
        }
        #endregion
    }
}
