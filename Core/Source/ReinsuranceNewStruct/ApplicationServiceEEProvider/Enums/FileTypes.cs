using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    /// <summary>
    /// FileTypes: Tipo de Archivos
    /// </summary>
    [DataContract]
    public enum FileTypes
    {
        [EnumMember]
        Text = 1,
        [EnumMember]
        Excel = 2,
        [EnumMember]
        ExcelTemplate = 3
    }
}
