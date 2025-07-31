using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BasePersonInterestGroup : Extension
    {
        [DataMember]
        public int IndividualId { get; set; }

        [DataMember]
        public int InterestGroupTypeId { get; set; }
    }
}
