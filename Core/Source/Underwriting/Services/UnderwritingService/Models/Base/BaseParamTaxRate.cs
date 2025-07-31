using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UnderwritingServices.Models.Base
{
    [DataContract]
    public class BaseParamTaxRate
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int IdTax { get; set; }

    }
}
