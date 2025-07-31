namespace Sistran.Core.Application.AuditServices.Enums
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Operaciones
    /// </summary>
    [DataContract(Name = "AudictType")]
    public enum AudictType
    {
        [EnumMember]
        Insert = 1,
        [EnumMember]
        Update = 2,
        [EnumMember]
        Delete = 3
    }

    [DataContract(Name = "SerializeType")]
    public enum AudictSerializeType
    {
        [EnumMember]
        Json = 1,
        [EnumMember]
        Xml = 2,
        [EnumMember]
        Binary = 3
    }
}
