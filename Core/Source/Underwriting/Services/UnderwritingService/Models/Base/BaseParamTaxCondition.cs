using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseParamTaxCondition
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdTax { get; set; }

        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public bool HasNationalRate { get; set; }
        [DataMember]
        public bool IsIndependent { get; set; }
        [DataMember]
        public bool IsDefault { get; set; }
    }
}
