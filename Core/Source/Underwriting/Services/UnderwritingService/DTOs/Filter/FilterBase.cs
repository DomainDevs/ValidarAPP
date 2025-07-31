using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.DTOs.Filter
{
    [DataContract]
    public class FilterBase
    {
        [DataMember]
        public int Id { get; set; }
    }
}
