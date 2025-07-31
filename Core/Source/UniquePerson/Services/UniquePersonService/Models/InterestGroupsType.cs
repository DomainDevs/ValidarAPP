using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    [DataContract]
    public class InterestGroupsType : Extension
    {
        [DataMember]
        public int InterestGroupTypeId { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
