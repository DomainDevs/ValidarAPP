using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.DTOs
{
    /// <summary>
    /// LineCumulusTypeDTO
    /// </summary>
    [DataContract]
    public class LineCumulusTypeDTO
    {
        [DataMember]
        public int LineId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CumulusTypeId { get; set; }
        [DataMember]
        public string CumulusTypeDescription { get; set; }
    }
}