using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models
{
    [DataContract]
    public class PersonInterestGroup : Extension
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int InterestGroupTypeId { get; set; }
    }
}
