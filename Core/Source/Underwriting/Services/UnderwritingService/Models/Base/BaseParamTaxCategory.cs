using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseParamTaxCategory 
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdTax { get; set; }

        [DataMember]
        public string Description { get; set; }
    }
}
