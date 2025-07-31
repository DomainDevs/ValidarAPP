using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.TaxServices.Models.Base
{
    public class BaseTaxPeriodRate
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime CurrentFrom { get; set; }

        [DataMember]
        public decimal Rate { get; set; }

        [DataMember]
        public decimal AdditionalRate { get; set; }

        [DataMember]
        public bool BaseTaxAdditional { get; set; }

        [DataMember]
        public decimal MinBaseAMT { get; set; }

        [DataMember]
        public decimal MinAdditionalBaseAMT { get; set; }

        [DataMember]
        public decimal MinTaxAMT { get; set; }

        [DataMember]
        public decimal MinAdditionalTaxAMT { get; set; }

        [DataMember]
        public DateTime CurrentTo { get; set; }
    }
}
