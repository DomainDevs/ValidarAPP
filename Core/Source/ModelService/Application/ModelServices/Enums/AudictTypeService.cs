using System.Runtime.Serialization;

namespace Sistran.Core.Application.AuditServices.Enums
{
    /// <summary>
    /// Operaciones
    /// </summary>
    [DataContract]
    public enum AudictTypeService
    {
        [EnumMember]
        Insert = 1,
        [EnumMember]
        Update = 2,
        [EnumMember]
        Delete = 3
    }
}
