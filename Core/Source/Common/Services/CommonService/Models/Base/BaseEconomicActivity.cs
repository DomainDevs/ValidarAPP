using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.CommonService.Models.Base
{
    [DataContract]
    public class BaseEconomicActivity
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public bool IsPerson { get; set; }

        [DataMember]
        public bool IsCompany { get; set; }
    }
}
