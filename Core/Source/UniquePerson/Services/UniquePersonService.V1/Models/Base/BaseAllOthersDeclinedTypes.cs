using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseAllOthersDeclinedTypes : Extension
    {
        [DataMember]
        public decimal Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public decimal? RoleCd { get; set; }
    }
}
