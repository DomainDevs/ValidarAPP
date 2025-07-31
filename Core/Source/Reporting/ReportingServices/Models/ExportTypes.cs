using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReportingServices.Models
{
    [DataContract]
    public enum ExportTypes
    {
        [EnumMember]
        Excel = 1,
        [EnumMember]
        PDF=2,
        [EnumMember]
        ExcelTemplate=3
    }
}
