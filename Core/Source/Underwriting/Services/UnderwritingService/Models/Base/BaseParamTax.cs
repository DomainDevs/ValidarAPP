using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseParamTax : Extension
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string TinyDescription { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public bool IsSurPlus { get; set; }

        [DataMember]
        public bool IsAdditionalSurPlus { get; set; }

        [DataMember]
        public bool Enabled { get; set; }

        [DataMember]
        public bool IsEarned { get; set; }

        [DataMember]
        public bool IsRetention { get; set; }
    }
}
