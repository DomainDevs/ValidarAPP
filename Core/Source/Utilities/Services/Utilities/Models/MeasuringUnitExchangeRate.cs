using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UtilitiesServices.Models
{
    [DataContract]
    public class MeasuringUnitExchangeRate
    {
        [DataMember]
        public MeasuringUnit MeasuringUnit { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public double RealValue { get; set; }
    }
}
