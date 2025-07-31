using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Models
{
    [DataContract]
    public class GoodExperienceYear
    {
        [DataMember]
        public int GoodExperienceNum { get; set; }
        [DataMember]
        public string GoodExpNumRate { get; set; }
        [DataMember]
        public int GoodExpNumPrinter { get; set; }
        [DataMember]
        public DateTime? MaximumVigency { get; set; }
        [DataMember]
        public string IsEffectiveDate { get; set; }
    }
}